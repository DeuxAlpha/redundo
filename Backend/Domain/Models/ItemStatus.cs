using System.Collections.Generic;
using Domain.Enums;

namespace Domain.Models
{
    public class ItemStatus
    {
        public ItemStatusEnum Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<GroupItem> GroupItems { get; }

        public ItemStatus()
        {
            GroupItems = new HashSet<GroupItem>();
        }
    }
}