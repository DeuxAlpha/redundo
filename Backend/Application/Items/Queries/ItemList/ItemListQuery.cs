using Application.Interfaces;
using MediatR;

namespace Application.Items.Queries.ItemList
{
    public class ItemListQuery : IPageRequest, IRequest<ItemListViewModel>
    {
        public string Name { get; set; }
        public int Items { get; set; } = 20;
        public int Page { get; set; } = 1;
    }
}