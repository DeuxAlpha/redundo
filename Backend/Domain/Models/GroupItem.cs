using System;
using Domain.Enums;

namespace Domain.Models
{
    public class GroupItem
    {
        public int GroupId { get; set; }
        public Group Group { get; set; }
        public int ItemId { get; set; }
        public Item Item { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public ItemStatus ItemStatus { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Notes { get; set; }
        public bool? OneTimePurchase { get; set; }
        public bool? DoNotBuy { get; set; }
    }
}