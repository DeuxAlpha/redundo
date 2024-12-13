using System.Collections.Generic;

namespace Domain.Models
{
    public class Group
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
        public User Owner { get; set; }
        public string Name { get; set; }
        
        public ICollection<UserGroup> GroupUsers { get; }
        public ICollection<GroupItem> GroupItems { get; }

        public Group()
        {
            GroupUsers = new HashSet<UserGroup>();
            GroupItems = new HashSet<GroupItem>();
        }
    }
}