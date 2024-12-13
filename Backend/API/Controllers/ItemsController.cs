using System.Threading.Tasks;
using Application.GroupItems.Commands.UpdateGroupItem;
using Application.Items.Commands.CreateItem;
using Application.Items.Queries.ItemList;
using API.Controllers.FunctionalControllers;
using API.Models;
using Application.Exceptions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace API.Controllers
{
    public class ItemsController : BaseController
    {
        public ItemsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageQuery pageQuery, [FromQuery] string name)
        {
            return Ok(await Mediator.Send(new ItemListQuery
            {
                Name = name,
                Page = pageQuery.Page,
                Items = pageQuery.Items
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateItemCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (UniqueConstraintException uniqueConstraintException)
            {
                return Conflict(new
                {
                    uniqueConstraintException.Message, uniqueConstraintException.UniqueValue
                });
            }
        }
    }
}