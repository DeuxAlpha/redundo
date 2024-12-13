using System;
using Domain.Enums;
using Domain.Models;
using Microsoft.AspNetCore.Razor.Language.Extensions;

namespace Application.GroupItems.Models
{
    public class GroupItemDto
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public DateTime LastUpdate { get; set; }
        public string Notes { get; set; }
        public bool DoNotBuy { get; set; }
        public bool OneTimePurchase { get; set; }
        
        public GroupItemDto(GroupItem groupItem)
        {
            GroupId = groupItem.GroupId;
            ItemId = groupItem.ItemId;
            ItemStatusId = groupItem.ItemStatusId;
            LastUpdate = groupItem.LastUpdate;
            Notes = groupItem.Notes;
            DoNotBuy = groupItem.DoNotBuy == true;
            OneTimePurchase = groupItem.OneTimePurchase == true;
        }
    }
}