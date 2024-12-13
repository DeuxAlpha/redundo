using Application.Groups.Models;
using MediatR;

namespace Application.Groups.Commands.UpdateGroup
{
    public class UpdateGroupCommand : IRequest<GroupDto>
    {
        public int Id { get; set; }
        public int OwnerId { get; set; }
    }
}