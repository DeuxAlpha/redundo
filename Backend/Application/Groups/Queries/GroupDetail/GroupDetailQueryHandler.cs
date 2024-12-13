using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Groups.Queries.GroupDetail
{
    public class GroupDetailQueryHandler : IRequestHandler<GroupDetailQuery, GroupViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public GroupDetailQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public async Task<GroupViewModel> Handle(GroupDetailQuery request, CancellationToken cancellationToken)
        {
            var userGroups = await _context.UserGroups
                .Include(ug => ug.Group)
                .ThenInclude(g => g.GroupItems)
                .ThenInclude(gi => gi.Item)
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == request.Id)
                .ToListAsync(cancellationToken);
            if (userGroups == null)
                throw new NotFoundException(nameof(Group), request.Id);

            return new GroupViewModel(userGroups);
        }
    }
}