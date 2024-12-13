using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.UserGroups.Queries.UserGroupList
{
    public class UserGroupListViewModel
    {
        public IEnumerable<UserGroupListItemModel> Users { get; set; }

        public UserGroupListViewModel(IEnumerable<UserGroup> userGroups)
        {
            Users = userGroups
                .Select(ug => new UserGroupListItemModel(ug))
                .OrderBy(u => u.GroupName)
                .ThenBy(u => u.Username);
        }
    }
}