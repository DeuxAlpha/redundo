using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Groups.Commands.DeleteGroup
{
    public class DeleteGroupCommandHandler : IRequestHandler<DeleteGroupCommand>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public DeleteGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Unit> Handle(DeleteGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(g => g.GroupUsers)
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (group == null)
                throw new NotFoundException(nameof(Group), request.Id);
            if (!_authService.UserIsOwnerOfGroup(group))
                throw new AuthorizationException("User is not owner.");

            _context.Groups.Remove(group);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}