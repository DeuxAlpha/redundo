using Application.Users.Models;
using MediatR;

namespace Application.Users.Commands.UpdateUserPassword
{
	public class UpdateUserPasswordCommand : IRequest<UserDto>
	{
		public int Id { get; set; }
		public string OldPassword { get; set; }
		public string NewPassword { get; set; }
	}
}