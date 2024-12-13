using System;
using Domain.Models;

namespace Application.Users.Models
{
	public class UserTokenModel
	{
		public UserDto User { get; set; }
		public string JwtToken { get; set; }
		public string RefreshToken { get; set; }
		public DateTime JwtExpirationDate { get; set; }
		public DateTime RefreshTokenExpirationDate { get; set; }

		public UserTokenModel(User user)
		{
			User = new UserDto(user);
		}
	}
}