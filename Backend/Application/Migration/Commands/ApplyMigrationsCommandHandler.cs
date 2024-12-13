using System.Threading;
using System.Threading.Tasks;
using Database.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Migration.Commands
{
	public class ApplyMigrationsCommandHandler : IRequestHandler<ApplyMigrationsCommand>
	{
		private readonly PurchaseManagerContext _context;

		public ApplyMigrationsCommandHandler(PurchaseManagerContext context)
		{
			_context = context;
		}

		public async Task<Unit> Handle(ApplyMigrationsCommand request, CancellationToken cancellationToken)
		{
			await _context.Database.MigrateAsync(cancellationToken);

			return Unit.Value;
		}
	}
}