using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Database.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Groups.Queries.GroupList
{
    public class GroupListQueryHandler : IRequestHandler<GroupListQuery, GroupListViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public GroupListQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public Task<GroupListViewModel> Handle(GroupListQuery request, CancellationToken cancellationToken)
        {
            var groups = request.UserId == null
                ? _context.UserGroups.Include(ug => ug.Group)
                    .Where(ug => ug.Group.Name.HasValue(request.Name))
                    .PaginateItems(request.Page, request.Items)
                : _context.UserGroups
                    .Include(ug => ug.Group)
                    .Where(ug => ug.Group.Name.HasValue(request.Name) && ug.UserId == request.UserId)
                    .PaginateItems(request.Page, request.Items);

            var groupListViewModel = new GroupListViewModel(groups);

            return Task.FromResult(groupListViewModel);
        }
    }
}