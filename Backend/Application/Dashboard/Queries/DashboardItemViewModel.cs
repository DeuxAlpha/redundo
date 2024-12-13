using System;
using Domain.Enums;
using Domain.Models;

namespace Application.Dashboard.Queries
{
    public class DashboardItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Notes { get; set; }
        public bool OneTimePurchase { get; set; }
        public DashboardGroupViewModel Group { get; set; }

        public DashboardItemViewModel(GroupItem groupItem)
        {
            Id = groupItem.ItemId;
            Name = groupItem.Item.Name;
            ItemStatusId = groupItem.ItemStatusId;
            LastUpdate = groupItem.LastUpdate;
            Notes = groupItem.Notes;
            OneTimePurchase = groupItem.OneTimePurchase == true;
            Group = new DashboardGroupViewModel(groupItem.Group);
        }
    }
}