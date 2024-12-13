using Domain.Models;

namespace Application.Groups.Queries.GroupDetail
{
    public class GroupUserViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsManager { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAcceptedByManager { get; set; }
        public bool IsAcceptedByUser { get; set; }
        public string RequestMessage { get; set; }

        public GroupUserViewModel(UserGroup userGroup)
        {
            Id = userGroup.UserId;
            Name = userGroup.User.Username;
            IsManager = userGroup.IsManager == true;
            IsOwner = userGroup.Group.OwnerId == Id;
            IsAcceptedByManager = userGroup.IsAcceptedByManager == true;
            IsAcceptedByUser = userGroup.IsAcceptedByUser == true;
            RequestMessage = userGroup.RequestMessage;
        }
    }
}