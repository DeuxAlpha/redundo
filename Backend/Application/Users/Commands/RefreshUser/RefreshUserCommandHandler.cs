using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Services.Authentication;
using Application.Users.Models;
using Common;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Application.Users.Commands.RefreshUser
{
    public class RefreshUserCommandHandler : IRequestHandler<RefreshUserCommand, UserTokenModel>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IConfiguration _configuration;
        private readonly IAuthService _authService;

        public RefreshUserCommandHandler(PurchaseManagerContext context, IConfiguration configuration, IAuthService authService)
        {
            _context = context;
            _configuration = configuration;
            _authService = authService;
        }

        public async Task<UserTokenModel> Handle(RefreshUserCommand request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                throw new AuthenticationException("No User ID found.");
            var _ = AuthenticationService.GetPrincipalFromTokenString(new StandardAuthenticationModel
            {
                JwtToken = request.JwtToken,
                SecurityKey = _configuration["SecurityKey"],
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"]
            });
            var storedRefreshToken =
                await _context.RefreshTokens.SingleOrDefaultAsync(
                    t => t.Token == request.RefreshToken,
                    cancellationToken);
            if (storedRefreshToken == null)
                throw new AuthenticationException("Refresh token not found.");
            if (storedRefreshToken.IsInvalidated == true || storedRefreshToken.ExpirationDate < DateTime.UtcNow)
            {
                storedRefreshToken.IsInvalidated = true;
                await _context.SaveChangesAsync(cancellationToken);
                throw new AuthenticationException("Refresh token is invalid.");
            }

            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new AuthenticationException("User not found.");

            var newTokenString = AuthenticationService.CreateTokenString(new SecureAuthenticationModel
            {
                User = user,
                ExpirationDate = ExpirationDateService.JwtExpirationDate,
                SecurityKey = _configuration["SecurityKey"],
                Issuer = _configuration["Issuer"],
                Audience = _configuration["Audience"]
            });
            var refreshToken = await _context.RefreshTokens.AddAsync(new RefreshToken
            {
                UserId = (int)userId,
                ExpirationDate = ExpirationDateService.RefreshTokenExpirationDate,
                CreationDate = DateTime.Now,
                Token = Randomizer.GetRandomBase64String()
            }, cancellationToken);
            storedRefreshToken.IsInvalidated = true;
            await _context.SaveChangesAsync(cancellationToken);
            
            return new UserTokenModel(user)
            {
                JwtToken = newTokenString,
                RefreshToken = refreshToken.Entity.Token,
                JwtExpirationDate = ExpirationDateService.JwtExpirationDate,
                RefreshTokenExpirationDate = ExpirationDateService.RefreshTokenExpirationDate
            };
        }
    }
}