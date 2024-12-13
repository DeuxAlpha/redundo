using Application.Groups.Models;
using MediatR;

namespace Application.Groups.Commands.CreateGroup
{
    public class CreateGroupCommand : IRequest<GroupDto>
    {
        public string Name { get; set; }
        /// <summary>
        /// The user creating the group to immediately create a UserGroup for it.
        /// </summary>
        public int GroupCreatorId { get; set; }
    }
}