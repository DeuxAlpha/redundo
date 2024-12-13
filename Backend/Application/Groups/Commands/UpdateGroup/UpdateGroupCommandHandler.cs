using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Groups.Models;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommandHandler : IRequestHandler<UpdateGroupCommand, GroupDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public UpdateGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GroupDto> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
        {
            var group = await _context.Groups
                .Include(g => g.GroupUsers)
                .SingleOrDefaultAsync(g => g.Id == request.Id, cancellationToken);
            if (group == null)
                throw new NotFoundException(nameof(Group), request.Id);
            if (!_authService.UserIsOwnerOfGroup(group))
                throw new AuthorizationException("User is not owner.");

            var newOwnerGroupUser = group.GroupUsers.SingleOrDefault(gu => gu.UserId == request.OwnerId);
            if (newOwnerGroupUser == null)
                throw new NotFoundException(nameof(UserGroup), request.OwnerId);
            if (newOwnerGroupUser.IsAcceptedByManager == false || newOwnerGroupUser.IsAcceptedByUser == false)
                throw new InvalidOperationException("User is not part of group yet.");
            
            newOwnerGroupUser.IsManager = true;
            group.OwnerId = request.OwnerId;

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupDto(group);
        }
    }
}