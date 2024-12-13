using MediatR;

namespace Application.UserGroups.Commands.DeleteUserGroup
{
    public class DeleteUserGroupCommand : IRequest
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
    }
}