using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace Application.Services.Authentication
{
    public static class AuthenticationService
	{
		public static string CreateTokenString(SecureAuthenticationModel secureAuthenticationModel)
		{
			var tokenHandler = new JwtSecurityTokenHandler();
			var key = Encoding.ASCII.GetBytes(secureAuthenticationModel.SecurityKey);
			var tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]
				{
					new Claim(ClaimTypes.PrimarySid, secureAuthenticationModel.User.Id.ToString()),
					new Claim(ClaimTypes.NameIdentifier, secureAuthenticationModel.User.Username)
				}),
				Issuer = secureAuthenticationModel.Issuer,
				Audience = secureAuthenticationModel.Audience,
				Expires = secureAuthenticationModel.ExpirationDate,
				NotBefore = DateTime.UtcNow,
				SigningCredentials =
					new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};
			var jwtToken = tokenHandler.CreateToken(tokenDescriptor);
			var tokenString = tokenHandler.WriteToken(jwtToken);
			return tokenString;
		}

		public static ClaimsPrincipal GetPrincipalFromTokenString(StandardAuthenticationModel standardAuthenticationModel)
		{
			var tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidAudience = standardAuthenticationModel.Audience,
				ValidateIssuer = true,
				ValidIssuer = standardAuthenticationModel.Issuer,
				ValidateIssuerSigningKey = true,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(standardAuthenticationModel.SecurityKey)),
				ValidateLifetime = false
			};
			
			var tokenHandler = new JwtSecurityTokenHandler();
			var principal = tokenHandler.ValidateToken(
				standardAuthenticationModel.JwtToken,
				tokenValidationParameters,
				out var securityToken);
			if (!(securityToken is JwtSecurityToken jwtSecurityToken) ||
			    !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
				    StringComparison.InvariantCultureIgnoreCase))
				throw new SecurityTokenException("Invalid Token");
			return principal;
		}
	}
}