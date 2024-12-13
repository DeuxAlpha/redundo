using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.Users.Models;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.UpdateUserPassword
{
	public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, UserDto>
	{
		private readonly PurchaseManagerContext _context;
		private readonly IAuthService _authService;
		private readonly IPasswordService _passwordService;

		public UpdateUserPasswordCommandHandler(PurchaseManagerContext context, IAuthService authService, IPasswordService passwordService)
		{
			_context = context;
			_authService = authService;
			_passwordService = passwordService;
		}

		public async Task<UserDto> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

			if (user == null)
				throw new NotFoundException(nameof(User), request.Id);

			if (!_authService.IsUserIdSelf(user.Id))
				throw new AuthenticationException("May not change password of other users.");

			if (!_passwordService.PasswordsMatch(user, user.Password, request.OldPassword))
				throw new AuthorizationException("Password was not valid.");

			user.Password = _passwordService.SecurePassword(user, request.NewPassword);

			await _context.SaveChangesAsync(cancellationToken);

			return new UserDto(user);
		}
	}
}