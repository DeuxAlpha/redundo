using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.UserGroups.Models;
using Database.Context;
using MediatR;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.EntityFrameworkCore;

namespace Application.UserGroups.Commands.UpdateUserGroup
{
    public class UpdateUserGroupCommandHandler : IRequestHandler<UpdateUserGroupCommand, UserGroupDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public UpdateUserGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<UserGroupDto> Handle(UpdateUserGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroups = _context.UserGroups
                .Include(ug => ug.Group)
                .Where(ug => ug.GroupId == request.GroupId);
            var userGroupToChange = 
                await userGroups.SingleOrDefaultAsync(ug => ug.UserId == request.UserId, cancellationToken);
            var requesterGroup =
                await userGroups.SingleOrDefaultAsync(ug => ug.UserId == _authService.GetUserId(), cancellationToken);
            if (!userGroups.Any() || userGroupToChange == null)
                throw new NotFoundException(nameof(userGroups),
                    $"UserID: ${request.UserId}, GroupID: ${request.GroupId}");
            if (requesterGroup == null)
                throw new AuthorizationException("User is not part of group.");
            if (request.UserAccepted != null &&
                userGroupToChange.IsAcceptedByUser != request.UserAccepted &&
                _authService.IsUserIdSelf(request.UserId))
                userGroupToChange.IsAcceptedByUser = request.UserAccepted == true;
            if (request.ManagerAccepted != null &&
                userGroupToChange.IsAcceptedByManager != request.ManagerAccepted &&
                _authService.UserIsManagerOfGroup(requesterGroup.Group))
                userGroupToChange.IsAcceptedByManager = request.ManagerAccepted == true;
            if (request.IsManager != null &&
                userGroupToChange.IsManager != request.IsManager &&
                !_authService.UserIsOwnerOfGroup(requesterGroup.Group))
                userGroupToChange.IsManager = request.IsManager == true;

            await _context.SaveChangesAsync(cancellationToken);

            return new UserGroupDto(userGroupToChange);
        }
    }
}