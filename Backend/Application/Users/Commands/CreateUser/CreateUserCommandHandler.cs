using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.Models;
using Common.Extensions;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IPasswordService _passwordService;

        public CreateUserCommandHandler(PurchaseManagerContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            var existingUser = await _context.Users.SingleOrDefaultAsync(
                u => u.Username.IsValue(request.Username), cancellationToken);

            if (existingUser != null)
                throw new UniqueConstraintException(nameof(User), request.Username);

            var user = new User()
            {
                Username = request.Username
            };

            user.Password = _passwordService.SecurePassword(user, request.Password);

            await _context.Users.AddAsync(user, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new UserDto(user);
        }
    }
}