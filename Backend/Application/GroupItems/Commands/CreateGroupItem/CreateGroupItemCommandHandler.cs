using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.GroupItems.Models;
using Application.Interfaces;
using Database.Context;
using Domain.Enums;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.GroupItems.Commands.CreateGroupItem
{
    public class CreateGroupItemCommandHandler : IRequestHandler<CreateGroupItemCommand, GroupItemDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public CreateGroupItemCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GroupItemDto> Handle(CreateGroupItemCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups.Include(g => g.GroupUsers)
                .SingleOrDefaultAsync(g => g.Id == request.GroupId, cancellationToken);
            if (group == null)
                throw new NotFoundException(nameof(Group), request.GroupId);
            var item = await _context.Items.SingleOrDefaultAsync(i => i.Id == request.ItemId, cancellationToken);
            if (item == null)
                throw new NotFoundException(nameof(Item), request.ItemId);
            var existingGroupItem =
                await _context.GroupItems.SingleOrDefaultAsync(
                    gi => gi.GroupId == request.GroupId && gi.ItemId == request.ItemId, cancellationToken);
            if (existingGroupItem != null)
                throw new UniqueConstraintException(nameof(GroupItem), $"GroupID: {request.GroupId}, ItemID: {request.ItemId}");
            if (!_authService.UserIsPartOfGroup(group))
                throw new AuthorizationException("User is not part of group.");
            
            var groupItem = new GroupItem
            {
                GroupId = request.GroupId,
                ItemId = request.ItemId,
                LastUpdate = DateTime.UtcNow,
                ItemStatusId = request.ItemStatusId,
                Notes = request.Notes,
                OneTimePurchase = request.OneTimePurchase
            };

            await _context.GroupItems.AddAsync(groupItem, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupItemDto(groupItem);
        }
    }
}