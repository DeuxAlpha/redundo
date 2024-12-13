using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands.RefreshUser
{
    public class RefreshUserCommand : IRequest<UserTokenModel>
    {
        public string JwtToken { get; set; }
        public string RefreshToken { get; set; }
    }
}