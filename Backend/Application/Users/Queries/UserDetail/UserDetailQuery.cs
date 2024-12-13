using MediatR;

namespace Application.Users.Queries.UserDetail
{
    public class UserDetailQuery : IRequest<UserViewModel>
    {
        public int Id { get; set; }
    }
}