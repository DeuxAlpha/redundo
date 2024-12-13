using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.Groups.Queries.GroupDetail
{
    public class GroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<GroupUserViewModel> Users { get; set; }
        public IEnumerable<GroupItemViewModel> Items { get; set; }

        public GroupViewModel(IReadOnlyCollection<UserGroup> userGroups)
        {
            var group = userGroups.First().Group;
            Id = group.Id;
            Name = group.Name;
            Users = userGroups
                .Select(ug => new GroupUserViewModel(ug))
                .OrderByDescending(u => u.IsOwner)
                .ThenByDescending(u => u.IsManager)
                .ThenByDescending(u => u.IsAcceptedByManager)
                .ThenByDescending(u => u.IsAcceptedByUser);
            Items = group.GroupItems.Select(gi => new GroupItemViewModel(gi))
                .OrderBy(gi => gi.Name);
        }
    }
}