using Application.Interfaces;
using MediatR;

namespace Application.Users.Queries.UserList
{
    public class UserListQuery : IPageRequest, IRequest<UserListViewModel>
    {
        public int Items { get; set; }
        public int Page { get; set; }
        public string Username { get; set; }
    }
}