using Domain.Models;

namespace Application.Items.Models
{
    public class ItemDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ItemDto(Item item)
        {
            Id = item.Id;
            Name = item.Name;
        }
    }
}