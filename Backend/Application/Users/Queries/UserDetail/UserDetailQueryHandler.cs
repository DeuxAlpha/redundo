using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Users.Queries.UserDetail
{
    public class UserDetailQueryHandler : IRequestHandler<UserDetailQuery, UserViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public UserDetailQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public async Task<UserViewModel> Handle(UserDetailQuery request, CancellationToken cancellationToken)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

            if (user == null)
                throw new NotFoundException(nameof(User), request.Id);

            return new UserViewModel(user);
        }
    }
}