using System.Threading.Tasks;
using Application.Exceptions;
using Application.Users.Commands.CreateUser;
using Application.Users.Commands.DeleteUser;
using Application.Users.Commands.LoginUser;
using Application.Users.Commands.RefreshUser;
using Application.Users.Commands.UpdateUserPassword;
using Application.Users.Queries.UserDetail;
using Application.Users.Queries.UserList;
using API.Controllers.FunctionalControllers;
using API.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageQuery pageQuery, [FromQuery] string username)
        {
            return Ok(await Mediator.Send(new UserListQuery
            {
                Page = pageQuery.Page,
                Items = pageQuery.Items,
                Username = username
            }));
        }

        [HttpGet("{id}", Name = "GetUser")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new UserDetailQuery
                {
                    Id = id
                }));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateUserCommand command)
        {
            try
            {
                var user = await Mediator.Send(command);

                return Created(Url.Link("GetUser", new {id = user.Id}), user);
            }
            catch (UniqueConstraintException uniqueConstraintException)
            {
                return BadRequest(uniqueConstraintException.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginUserCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
        }

        [HttpPost("refresh")]
        [AllowAnonymous]
        public async Task<IActionResult> Refresh([FromBody] RefreshUserCommand command)
        {
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
        }

        [HttpPut("{id}/password")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUserPasswordCommand passwordCommand)
        {
            if (id != passwordCommand.Id)
                return BadRequest();
            try
            {
                return Ok(await Mediator.Send(passwordCommand));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
            catch (AuthenticationException)
            {
                return Unauthorized();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, [FromBody] DeleteUserCommand command)
        {
            if (id != command.Id)
                return BadRequest();
            try
            {
                await Mediator.Send(command);

                return NoContent();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                // TODO: Require update of groups and before deleting user.
                return Forbid();
            }
        }
    }
}