using System.Collections.Generic;
using System.Linq;
using Application.GroupItems.Models;
using Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Application.Dashboard.Queries
{
    public class DashboardGroupViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public DashboardGroupViewModel(Group group)
        {
            Id = group.Id;
            Name = group.Name;
        }
    }
}