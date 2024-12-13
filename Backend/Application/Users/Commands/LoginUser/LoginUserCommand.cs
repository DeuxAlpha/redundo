using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands.LoginUser
{
	public class LoginUserCommand : IRequest<UserTokenModel>
	{
		public string Username { get; set; }
		public string Password { get; set; }
	}
}