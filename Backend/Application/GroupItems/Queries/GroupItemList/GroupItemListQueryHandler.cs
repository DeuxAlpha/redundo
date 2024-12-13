using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Common.Extensions;
using Database.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.GroupItems.Queries.GroupItemList
{
    public class GroupItemListQueryHandler : IRequestHandler<GroupItemListQuery, GroupItemListViewModel>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public GroupItemListQueryHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public Task<GroupItemListViewModel> Handle(GroupItemListQuery request, CancellationToken cancellationToken)
        {
            var groupItems = _context.GroupItems
                .Include(gi => gi.Group)
                .Include(gi => gi.Item)
                .Where(gi => gi.Item.Name.HasValue(request.Name));

            if (groupItems.Any(gi => !_authService.UserIsPartOfGroup(gi.Group)))
                throw new AuthorizationException("User is not part of group.");

            var groupItemListViewModel = new GroupItemListViewModel(groupItems);

            return Task.FromResult(groupItemListViewModel);
        }
    }
}