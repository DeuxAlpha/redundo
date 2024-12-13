using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Database.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.UserGroups.Queries.UserGroupList
{
    public class UserGroupListQueryHandler : IRequestHandler<UserGroupListQuery, UserGroupListViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public UserGroupListQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public Task<UserGroupListViewModel> Handle(UserGroupListQuery request, CancellationToken cancellationToken)
        {
            var userGroups = _context.UserGroups
                .Include(ug => ug.Group)
                .Include(ug => ug.User)
                .Where(ug => ug.GroupId == request.GroupId)
                .PaginateItems(request.Page, request.Items);

            var userGroupListViewModel = new UserGroupListViewModel(userGroups);

            return Task.FromResult(userGroupListViewModel);
        }
    }
}