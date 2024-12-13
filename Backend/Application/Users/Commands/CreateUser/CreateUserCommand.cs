using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands.CreateUser
{
    public class CreateUserCommand : IRequest<UserDto>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}