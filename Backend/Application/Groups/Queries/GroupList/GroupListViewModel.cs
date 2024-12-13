using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Domain.Models;

namespace Application.Groups.Queries.GroupList
{
    public class GroupListViewModel
    {
        public IEnumerable<GroupListItemViewModel> Groups { get; set; }

        public GroupListViewModel(IEnumerable<UserGroup> userGroups)
        {
            Groups = userGroups.Select(g => new GroupListItemViewModel(g)).OrderBy(g => g.Name);
        }
    }
}