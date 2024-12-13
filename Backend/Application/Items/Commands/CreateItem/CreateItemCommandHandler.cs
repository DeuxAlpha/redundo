using System.Threading;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.Items.Models;
using Database.Context;
using Domain.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Items.Commands.CreateItem
{
    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, ItemDto>
    {
        private readonly PurchaseManagerContext _context;

        public CreateItemCommandHandler(PurchaseManagerContext context)
        {
            _context = context;
        }

        public async Task<ItemDto> Handle(CreateItemCommand request, CancellationToken cancellationToken)
        {
            var existingItem =
                await _context.Items.SingleOrDefaultAsync(i => i.Name == request.Name, cancellationToken);
            if (existingItem != null)
                throw new UniqueConstraintException(nameof(Item), existingItem.Id);
            var item = new Item
            {
                Name = request.Name
            };

            await _context.Items.AddAsync(item, cancellationToken);

            await _context.SaveChangesAsync(cancellationToken);

            return new ItemDto(item);
        }
    }
}