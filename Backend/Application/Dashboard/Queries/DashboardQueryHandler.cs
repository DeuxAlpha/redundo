using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Interfaces;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;
using AuthenticationException = System.Security.Authentication.AuthenticationException;

namespace Application.Dashboard.Queries
{
    public class DashboardQueryHandler : IRequestHandler<DashboardQuery, DashboardViewModel>
    {
        private readonly PurchaseManagerContext _context;
        private readonly IAuthService _authService;

        public DashboardQueryHandler(PurchaseManagerContext context, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<DashboardViewModel> Handle(DashboardQuery request, CancellationToken cancellationToken)
        {
            var userId = _authService.GetUserId();
            if (userId == null)
                throw new AuthenticationException("Can not identify user.");
            var user = await _context.Users
                .Include(u => u.UserGroups)
                .ThenInclude(ug => ug.Group)
                .ThenInclude(g => g.GroupItems)
                .ThenInclude(gi => gi.Item)
                .SingleOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
                throw new NotFoundException(nameof(User), userId);
            return new DashboardViewModel(user);
        }
    }
}