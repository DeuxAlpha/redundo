using System.Data;
using Domain.Models;

namespace Application.UserGroups.Queries.UserGroupList
{
    public class UserGroupListItemModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public int UserId { get; set; }
        public string Username { get; set; }
        public bool IsOwner { get; set; }
        public bool IsManager { get; set; }
        public bool IsAccepted { get; set; }

        public UserGroupListItemModel(UserGroup userGroup)
        {
            GroupId = userGroup.GroupId;
            GroupName = userGroup.Group.Name;
            UserId = userGroup.UserId;
            Username = userGroup.User.Username;
            IsOwner = userGroup.Group.OwnerId == userGroup.UserId;
            IsManager = userGroup.IsManager == true;
            IsAccepted = userGroup.IsAcceptedByUser == true && userGroup.IsAcceptedByManager == true;
        }
    }
}