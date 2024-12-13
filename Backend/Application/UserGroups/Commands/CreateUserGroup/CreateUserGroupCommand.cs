using Application.UserGroups.Models;
using MediatR;

namespace Application.UserGroups.Commands.CreateUserGroup
{
    public class CreateUserGroupCommand : IRequest<UserGroupDto>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public string RequestMessage { get; set; }
    }
}