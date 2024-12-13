using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Commands.DeleteUser
{
	public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand>
	{
		private readonly PurchaseManagerContext _context;
		private readonly IAuthService _authService;
		private readonly IPasswordService _passwordService;

		public DeleteUserCommandHandler(PurchaseManagerContext context, IAuthService authService, IPasswordService passwordService)
		{
			_context = context;
			_authService = authService;
			_passwordService = passwordService;
		}

		public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

			if (user == null)
				throw new NotFoundException(nameof(User), request.Id);

			if (!_authService.IsUserIdSelf(user.Id))
				throw new AuthorizationException("Only user may delete own account.");

			if (!_passwordService.PasswordsMatch(user, user.Password, request.Password))
				throw new AuthorizationException("Password was invalid.");

			_context.Users.Remove(user);

			await _context.SaveChangesAsync(cancellationToken);

			return Unit.Value;
		}
	}
}