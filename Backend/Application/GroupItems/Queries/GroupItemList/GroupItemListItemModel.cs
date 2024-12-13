using System;
using Application.GroupItems.Models;
using Domain.Enums;
using Domain.Models;

namespace Application.GroupItems.Queries.GroupItemList
{
    public class GroupItemListItemModel
    {
        public int GroupId { get; set; }
        public int ItemId { get; set; }
        public ItemStatusEnum ItemStatusId { get; set; }
        public string ItemStatus { get; set; }
        public string Notes { get; set; }
        public DateTime LastUpdate { get; set; }

        public GroupItemListItemModel(GroupItem groupItem)
        {
            GroupId = groupItem.GroupId;
            ItemId = groupItem.ItemId;
            ItemStatusId = groupItem.ItemStatusId;
            ItemStatus = groupItem.ItemStatus.Name;
            Notes = groupItem.Notes;
            LastUpdate = groupItem.LastUpdate;
        }
    }
}