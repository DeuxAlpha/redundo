using Domain.Models;

namespace Application.Groups.Queries.GroupList
{
    public class GroupListItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsAcceptedByManager { get; set; }
        public bool IsAcceptedByUser { get; set; }
        public bool IsOwner { get; set; }
        public bool IsManager { get; set; }
        public string RequestMessage { get; set; }

        public GroupListItemViewModel(UserGroup userGroup)
        {
            Id = userGroup.GroupId;
            Name = userGroup.Group.Name;
            IsOwner = userGroup.UserId == userGroup.Group.OwnerId;
            IsManager = userGroup.IsManager == true;
            IsAcceptedByManager = userGroup.IsAcceptedByManager == true;
            IsAcceptedByUser = userGroup.IsAcceptedByUser == true;
            RequestMessage = userGroup.RequestMessage;
        }
    }
}