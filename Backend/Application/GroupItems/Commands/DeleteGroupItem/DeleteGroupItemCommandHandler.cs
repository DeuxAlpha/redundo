using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.GroupItems.Commands.DeleteGroupItem
{
    public class DeleteGroupItemCommandHandler : IRequestHandler<DeleteGroupItemCommand>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public DeleteGroupItemCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Unit> Handle(DeleteGroupItemCommand request, CancellationToken cancellationToken)
        {
            var groupItem = await _context.GroupItems
                .Include(gi => gi.Group)
                .ThenInclude(g => g.GroupUsers)
                .SingleOrDefaultAsync(gi => gi.GroupId == request.GroupId && gi.ItemId == request.ItemId,
                    cancellationToken);
            if (groupItem == null)
                throw new NotFoundException(nameof(GroupItem), $"GroupID: {request.GroupId}, ItemID: {request.ItemId}");
            if (!_authService.UserIsManagerOfGroup(groupItem.Group))
                throw new AuthorizationException("User is not manager of group.");

            _context.GroupItems.Remove(groupItem);

            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}