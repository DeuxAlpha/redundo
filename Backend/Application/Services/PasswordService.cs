using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Services
{
	public class PasswordService : IPasswordService
	{
		private readonly IPasswordHasher<User> _passwordHasher;

		public PasswordService(IPasswordHasher<User> passwordHasher)
		{
			_passwordHasher = passwordHasher;
		}
		
		public string SecurePassword(User user, string plainTextPassword)
		{
			return _passwordHasher.HashPassword(user, plainTextPassword);
		}

		public bool PasswordsMatch(User user, string hashedPassword, string plainTextPassword)
		{
			var passwordResult = _passwordHasher.VerifyHashedPassword(user, hashedPassword, plainTextPassword);

			return passwordResult == PasswordVerificationResult.Success ||
			       passwordResult == PasswordVerificationResult.SuccessRehashNeeded;
		}
	}
}