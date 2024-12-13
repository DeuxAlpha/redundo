using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Application.UserGroups.Models;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserGroups.Commands.CreateUserGroup
{
    public class CreateUserGroupCommandHandler : IRequestHandler<CreateUserGroupCommand, UserGroupDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public CreateUserGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<UserGroupDto> Handle(CreateUserGroupCommand request, CancellationToken cancellationToken)
        {
            var userGroups = _context.UserGroups.Include(ug => ug.Group)
                .Where(ug => ug.GroupId == request.GroupId)
                .AsNoTracking();
            if (!await userGroups.AnyAsync(cancellationToken))
                throw new NotFoundException(nameof(UserGroup), request.GroupId);
            var userIsManager =
                _authService.UserIsManagerOfGroup(
                    (await userGroups.SingleOrDefaultAsync(ug => ug.UserId == _authService.GetUserId(), cancellationToken))?
                    .Group);
            var userGroup = new UserGroup
            {
                UserId = request.UserId,
                GroupId = request.GroupId,
                IsAcceptedByUser = _authService.IsUserIdSelf(request.UserId),
                IsAcceptedByManager = userIsManager,
                RequestMessage = request.RequestMessage
            };
            if (userGroup.IsAcceptedByManager != true && userGroup.IsAcceptedByUser != true) // Someone unauthorized tried to add user to group
                throw new AuthorizationException("You are not allowed to add users to this group.");
            await _context.UserGroups.AddAsync(userGroup, cancellationToken);

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("unique constraint"))
                    throw new UniqueConstraintException(nameof(User), dbUpdateException);
                throw;
            }

            return new UserGroupDto(userGroup);
        }
    }
}