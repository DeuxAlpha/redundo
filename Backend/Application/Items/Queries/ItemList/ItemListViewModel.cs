using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.Items.Queries.ItemList
{
    public class ItemListViewModel
    {
        public IEnumerable<ItemListItemModel> Items { get; set; }

        public ItemListViewModel(IEnumerable<Item> items)
        {
            Items = items.Select(i => new ItemListItemModel(i));
        }
    }
}