using System.Collections.Generic;

namespace Domain.Models
{
    public class Item
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public ICollection<GroupItem> GroupItems { get; }

        public Item()
        {
            GroupItems = new HashSet<GroupItem>();
        }
    }
}