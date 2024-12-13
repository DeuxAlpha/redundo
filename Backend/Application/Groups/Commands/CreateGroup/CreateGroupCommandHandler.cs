using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Groups.Models;
using Application.Interfaces;
using Common.Extensions;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommandHandler : IRequestHandler<CreateGroupCommand, GroupDto>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public CreateGroupCommandHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<GroupDto> Handle(CreateGroupCommand request, CancellationToken cancellationToken)
        {
            var existingGroup =
                await _context.Groups.SingleOrDefaultAsync(g => g.Name.IsValue(request.Name), cancellationToken);
            if (existingGroup != null)
                throw new UniqueConstraintException(nameof(Group), request.Name);
            var userId = _authService.GetUserId();
            if (userId == null)
                throw new AuthorizationException("Not a valid user.");
            if (request.GroupCreatorId != userId)
                throw new AuthorizationException("User must be creator.");
            
            var group = new Group
            {
                Name = request.Name,
                OwnerId = (int)userId
            };

            await _context.Groups.AddAsync(group, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);
            
            var userGroup = new UserGroup
            {
                UserId = (int) userId,
                GroupId = group.Id,
                IsManager = true,
                IsAcceptedByManager = true,
                IsAcceptedByUser = true
            };

            await _context.UserGroups.AddAsync(userGroup, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new GroupDto(group);
        }
    }
}