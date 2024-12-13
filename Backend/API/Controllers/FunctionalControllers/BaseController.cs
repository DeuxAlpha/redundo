using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers.FunctionalControllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BaseController : Controller
    {
        protected readonly IMediator Mediator;

        protected BaseController(IMediator mediator)
        {
            Mediator = mediator;
        }
    }
}