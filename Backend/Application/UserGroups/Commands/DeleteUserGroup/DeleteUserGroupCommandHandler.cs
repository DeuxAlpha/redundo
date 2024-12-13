using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserGroups.Commands.DeleteUserGroup
{
    public class DeleteUserGroupCommandHandler : IRequestHandler<DeleteUserGroupCommand>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public DeleteUserGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<Unit> Handle(DeleteUserGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroups = _context.UserGroups
                .Include(ug => ug.Group)
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == request.GroupId);
            var userGroupToDelete = await 
                userGroups.SingleOrDefaultAsync(ug => ug.UserId == request.UserId, cancellationToken);
            if (userGroupToDelete == null)
                throw new NotFoundException(nameof(UserGroup), request.UserId);
            var requesterGroup = await
                userGroups.SingleOrDefaultAsync(ug => ug.UserId == _authService.GetUserId(), cancellationToken);
            if (userGroupToDelete == requesterGroup)
                return await RemoveUserAndReturn(userGroupToDelete, cancellationToken);
            if (!_authService.UserIsOwnerOfGroup(requesterGroup.Group) &&
                !_authService.UserIsManagerOfGroup(requesterGroup.Group))
                throw new AuthorizationException("Only managers and owner can delete users from group.");
            if (_authService.UserIsOwnerOfGroup(userGroupToDelete.User, userGroupToDelete.Group))
                throw new InvalidOperationException("Owner can not be deleted from group.");
            if (!_authService.UserIsOwnerOfGroup(requesterGroup.Group) &&
                _authService.UserIsManagerOfGroup(userGroupToDelete.User, userGroupToDelete.Group))
                throw new AuthorizationException("Only owner can delete managers from group.");

            return await RemoveUserAndReturn(userGroupToDelete, cancellationToken);
        }

        private async Task<Unit> RemoveUserAndReturn(UserGroup userGroupToDelete, CancellationToken cancellationToken)
        {
            _context.UserGroups.Remove(userGroupToDelete);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}