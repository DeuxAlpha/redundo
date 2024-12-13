using System;

namespace Domain.Models
{
    public class RefreshToken
    {
        public int UserId { get; set; }
        public User User { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public bool? IsInvalidated { get; set; }
    }
}