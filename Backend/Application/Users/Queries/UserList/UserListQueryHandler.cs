using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Database.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.UserList
{
    public class UserListQueryHandler : IRequestHandler<UserListQuery, UserListViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public UserListQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public Task<UserListViewModel> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var users = _context.Users
                .Where(u => u.Username.HasValue(request.Username))
                .Include(u => u.UserGroups)
                .PaginateItems(request.Page, request.Items);

            var userListViewModel = new UserListViewModel(users);

            return Task.FromResult(userListViewModel);
        }
    }
}