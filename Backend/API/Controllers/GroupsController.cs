using System;
using System.Threading.Tasks;
using Application.Exceptions;
using Application.GroupItems.Commands.CreateGroupItem;
using Application.GroupItems.Commands.DeleteGroupItem;
using Application.GroupItems.Commands.UpdateGroupItem;
using Application.GroupItems.Queries.GroupItemList;
using Application.Groups.Commands.CreateGroup;
using Application.Groups.Commands.DeleteGroup;
using Application.Groups.Commands.UpdateGroup;
using Application.Groups.Queries.GroupList;
using Application.UserGroups.Commands.CreateUserGroup;
using Application.UserGroups.Commands.DeleteUserGroup;
using Application.UserGroups.Commands.UpdateUserGroup;
using Application.UserGroups.Queries.UserGroupList;
using API.Controllers.FunctionalControllers;
using API.Models;
using Application.Groups.Queries.GroupDetail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class GroupsController : BaseController
    {
        public GroupsController(IMediator mediator) : base(mediator)
        {
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] PageQuery pageQuery, [FromQuery] string name, [FromQuery] int? userId)
        {
            return Ok(await Mediator.Send(new GroupListQuery
            {
                Name = name,
                UserId = userId,
                Page = pageQuery.Page,
                Items = pageQuery.Items
            }));
        }

        [HttpGet("{id}", Name = "GetGroup")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                return Ok(await Mediator.Send(new GroupDetailQuery
                {
                    Id = id
                }));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
        }

        [HttpGet("{id}/items")]
        public async Task<IActionResult> GetGroupItems(int id, [FromQuery] PageQuery pageQuery, [FromQuery] string name)
        {
            return Ok(await Mediator.Send(new GroupItemListQuery
            {
                GroupId = id,
                Name = name,
                Page = pageQuery.Page,
                Items = pageQuery.Items
            }));
        }

        [HttpGet("{id}/users")]
        public async Task<IActionResult> GetGroupUsers(int id, [FromQuery] PageQuery pageQuery)
        {
            return Ok(await Mediator.Send(new UserGroupListQuery
            {
                GroupId = id,
                Page = pageQuery.Page,
                Items = pageQuery.Items
            }));
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGroupCommand command)
        {
            try
            {
                var group = await Mediator.Send(command);

                return Created(Url.Link("GetGroup", new {id = group.Id}), group);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
            catch (UniqueConstraintException uniqueConstraintException)
            {
                return BadRequest(uniqueConstraintException.Message);
            }
        }

        [HttpPost("{id}/items")]
        public async Task<IActionResult> CreateGroupItem(int id, [FromBody] CreateGroupItemCommand command)
        {
            if (id != command.GroupId)
                return BadRequest();

            try
            {
                var groupItem = await Mediator.Send(command);

                return Ok(groupItem);
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
            catch (UniqueConstraintException uniqueConstraintException)
            {
                return Conflict(uniqueConstraintException.Message);
            }
        }

        [HttpPost("{id}/users")]
        public async Task<IActionResult> CreateGroupUser(int id, [FromBody] CreateUserGroupCommand command)
        {
            if (id != command.GroupId)
                return BadRequest();

            try
            {
                var groupUser = await Mediator.Send(command);

                return Ok(groupUser);
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (UniqueConstraintException uniqueConstraintException)
            {
                return BadRequest(uniqueConstraintException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGroupCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
            catch (InvalidOperationException invalidOperationException)
            {
                return BadRequest(invalidOperationException.Message);
            }
        }

        [HttpPut("{id}/items/{itemId}")]
        public async Task<IActionResult> UpdateGroupItem(int id, int itemId, [FromBody] UpdateGroupItemCommand command)
        {
            if (id != command.GroupId || itemId != command.ItemId)
                return BadRequest();

            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }

        [HttpPut("{id}/users/{userId}")]
        public async Task<IActionResult> UpdateGroupUser(int id, int userId, [FromBody] UpdateUserGroupCommand command)
        {
            if (id != command.GroupId || userId != command.UserId)
                return BadRequest();
            
            try
            {
                return Ok(await Mediator.Send(command));
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await Mediator.Send(new DeleteGroupCommand
                {
                    Id = id
                });

                return NoContent();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}/items/{itemId}")]
        public async Task<IActionResult> DeleteGroupItem(int id, int itemId)
        {
            try
            {
                await Mediator.Send(new DeleteGroupItemCommand
                {
                    GroupId = id,
                    ItemId = itemId
                });

                return NoContent();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }

        [HttpDelete("{id}/users/{userId}")]
        public async Task<IActionResult> DeleteUserGroup(int id, int userId)
        {
            try
            {
                await Mediator.Send(new DeleteUserGroupCommand
                {
                    GroupId = id,
                    UserId = userId
                });

                return NoContent();
            }
            catch (NotFoundException notFoundException)
            {
                return NotFound(notFoundException.Message);
            }
            catch (AuthorizationException)
            {
                return Forbid();
            }
        }
    }
}