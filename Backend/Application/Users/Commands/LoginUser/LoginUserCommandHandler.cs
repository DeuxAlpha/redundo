using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.Authentication;
using Application.Users.Models;
using Common;
using Common.Extensions;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Commands.LoginUser
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, UserTokenModel>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;

        public LoginUserCommandHandler(PurchaseManagerContext context, IPasswordService passwordService, IConfiguration configuration)
        {
            _context = context;
            _passwordService = passwordService;
            _configuration = configuration;
        }

        public async Task<UserTokenModel> Handle(LoginUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(
                u => u.Username.IsValue(request.Username),
                cancellationToken);

            if (user == null || !_passwordService.PasswordsMatch(user, user.Password, request.Password))
                throw new AuthenticationException();

            var tokenString = AuthenticationService.CreateTokenString(new SecureAuthenticationModel
            {
                User = user,
                ExpirationDate = ExpirationDateService.JwtExpirationDate,
                SecurityKey = _configuration["SecurityKey"],
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"]
            });

            var refreshToken = await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = user.Id,
                ExpirationDate = ExpirationDateService.RefreshTokenExpirationDate,
                CreationDate = DateTime.Now,
                Token = Randomizer.GetRandomBase64String()
            }, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new UserTokenModel(user)
            {
                JwtToken = tokenString,
                RefreshToken = refreshToken.Entity.Token,
                JwtExpirationDate = ExpirationDateService.JwtExpirationDate,
                RefreshTokenExpirationDate = ExpirationDateService.RefreshTokenExpirationDate
            };
        }
    }
}