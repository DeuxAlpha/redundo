using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Extensions;
using Database.Context;
using MediatR;

namespace Application.Items.Queries.ItemList
{
    public class ItemListQueryHandler : IRequestHandler<ItemListQuery, ItemListViewModel>
    {
        private readonly PurchaseManagerContext _context;

        public ItemListQueryHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public Task<ItemListViewModel> Handle(ItemListQuery request, CancellationToken cancellationToken)
        {
            var items = _context.Items
                .Where(i => i.Name.HasValue(request.Name))
                .PaginateItems(request.Page, request.Items);
            
            var itemListViewModel = new ItemListViewModel(items);

            return Task.FromResult(itemListViewModel);
        }
    }
}