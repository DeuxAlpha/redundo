using Application.UserGroups.Models;
using MediatR;

namespace Application.UserGroups.Commands.UpdateUserGroup
{
    public class UpdateUserGroupCommand : IRequest<UserGroupDto>
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool? IsManager { get; set; }
        public bool? UserAccepted { get; set; }       
        public bool? ManagerAccepted { get; set; }
    }
}