using System.Collections.Generic;
using System.Linq;
using Domain.Models;

namespace Application.Dashboard.Queries
{
    public class DashboardViewModel
    {
        public IEnumerable<DashboardItemViewModel> Items { get; set; }
        public bool AnyGroups { get; set; }

        public DashboardViewModel(User user)
        {
            AnyGroups = user.UserGroups.Any();
            Items = user.UserGroups
                .Where(ug => ug.IsAcceptedByManager == true && ug.IsAcceptedByUser == true)
                .Select(u => u.Group)
                .SelectMany(g => g.GroupItems)
                .Where(gi => gi.DoNotBuy != true)
                .Select(gi => new DashboardItemViewModel(gi))
                .OrderBy(i => i.Name);
        }
    }
}