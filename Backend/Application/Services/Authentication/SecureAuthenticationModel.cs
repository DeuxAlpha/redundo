using System;
using Domain.Models;

namespace Application.Services.Authentication
{
    public class SecureAuthenticationModel
    {
        public User User { get; set; }
        public DateTime ExpirationDate { get; set; }
        public string SecurityKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}