using Domain.Models;

namespace Application.Interfaces
{
	public interface IPasswordService
	{
		string SecurePassword(User user, string plainTextPassword);
		bool PasswordsMatch(User user, string hashedPassword, string plainTextPassword);
	}
}