using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.GroupItems.Queries.GroupItemList
{
    public class GroupItemListViewModel
    {
        public IEnumerable<GroupItemListItemModel> Items { get; set; }

        public GroupItemListViewModel(IEnumerable<GroupItem> groupItems)
        {
            Items = groupItems.Select(gi => new GroupItemListItemModel(gi));
        }
    }
}