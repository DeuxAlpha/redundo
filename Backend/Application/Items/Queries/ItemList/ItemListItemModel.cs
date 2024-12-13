using Domain.Models;

namespace Application.Items.Queries.ItemList
{
    public class ItemListItemModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ItemListItemModel(Item item)
        {
            Id = item.Id;
            Name = item.Name;
        }
    }
}