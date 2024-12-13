using System.Threading.Tasks;
using Application.Migration.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FunctionalControllers
{
	public class MigrationController : BaseController
	{
		public MigrationController(IMediator mediator) : base(mediator)
		{
		}

		[AllowAnonymous]
		[HttpPost]
		public async Task<IActionResult> Migrate()
		{
			await Mediator.Send(new ApplyMigrationsCommand());

			return Ok();
		}
	}
}