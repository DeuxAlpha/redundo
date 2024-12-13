using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.GroupItems.Models;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.GroupItems.Commands.UpdateGroupItem
{
    public class UpdateGroupItemCommandHandler : IRequestHandler<UpdateGroupItemCommand, GroupItemDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public UpdateGroupItemCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GroupItemDto> Handle(UpdateGroupItemCommand request, CancellationToken cancellationToken)
        {
            var groupItem = await _context.GroupItems
                .Include(gi => gi.Group)
                .ThenInclude(g => g.GroupUsers)
                .SingleOrDefaultAsync(
                gi => gi.GroupId == request.GroupId && gi.ItemId == request.ItemId, cancellationToken);
            if (groupItem == null)
                throw new NotFoundException(nameof(GroupItem),
                    $"GroupID: ${request.GroupId}, ItemID: {request.ItemId}");
            if (!_authService.UserIsPartOfGroup(groupItem.Group))
                throw new AuthorizationException("User is not part of group.");

            groupItem.ItemStatusId = request.ItemStatusId;
            groupItem.Notes = request.Notes;
            groupItem.OneTimePurchase = request.OneTimePurchase;
            groupItem.DoNotBuy = request.DoNotBuy;
            groupItem.LastUpdate = DateTime.UtcNow;

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupItemDto(groupItem);
        }
    }
}