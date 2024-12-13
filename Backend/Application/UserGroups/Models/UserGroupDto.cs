using Application.Interfaces;
using Domain.Models;

namespace Application.UserGroups.Models
{
    public class UserGroupDto
    {
        public int UserId { get; set; }
        public int GroupId { get; set; }
        public bool IsManager { get; set; }
        public bool IsAcceptedByUser { get; set; }
        public bool IsAcceptedByManager { get; set; }
        public string RequestMessage { get; set; }

        public UserGroupDto(UserGroup userGroup)
        {
            UserId = userGroup.UserId;
            GroupId = userGroup.GroupId;
            IsManager = userGroup.IsManager == true;
            IsAcceptedByUser = userGroup.IsAcceptedByUser == true;
            IsAcceptedByManager = userGroup.IsAcceptedByManager == true;
            RequestMessage = userGroup.RequestMessage;
        }
    }
}