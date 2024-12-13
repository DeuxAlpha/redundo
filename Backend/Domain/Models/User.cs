using System.Collections.Generic;

namespace Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        
        public ICollection<UserGroup> UserGroups { get; }
        public ICollection<RefreshToken> RefreshTokens { get; }
        public ICollection<Group> OwnedGroups { get; }

        public User()
        {
            UserGroups = new HashSet<UserGroup>();
            RefreshTokens = new HashSet<RefreshToken>();
            OwnedGroups = new HashSet<Group>();
        }
    }
}