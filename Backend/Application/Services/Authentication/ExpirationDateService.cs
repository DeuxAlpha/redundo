using System;

namespace Application.Services.Authentication
{
    public static class ExpirationDateService
    {
        public static DateTime JwtExpirationDate => DateTime.UtcNow.AddHours(3);

        public static DateTime RefreshTokenExpirationDate => DateTime.UtcNow.AddDays(7);
    }
}