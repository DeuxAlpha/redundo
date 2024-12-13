using Application.Items.Models;
using MediatR;

namespace Application.Items.Commands.CreateItem
{
    public class CreateItemCommand : IRequest<ItemDto>
    {
        public string Name { get; set; }
    }
}