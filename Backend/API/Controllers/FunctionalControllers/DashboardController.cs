using System.Threading.Tasks;
using Application.Dashboard.Queries;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FunctionalControllers
{
    public class DashboardController : BaseController
    {
        public DashboardController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetDashboard()
        {
            try
            {
                return Ok(await Mediator.Send(new DashboardQuery()));
            }
            catch (AuthenticationException)
            {
                return Forbid();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
        }
    }
}