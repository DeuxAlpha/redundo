using System;
using Domain.Enums;
using Domain.Models;

namespace Application.Groups.Queries.GroupDetail
{
	public class GroupItemViewModel
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public ItemStatusEnum ItemStatusId { get; set; }
		public DateTime LastUpdate { get; set; }
		public string Notes { get; set; }
		public bool DoNotBuy { get; set; }
		public bool OneTimePurchase { get; set; }

		public GroupItemViewModel(GroupItem groupItem)
		{
			Id = groupItem.ItemId;
			Name = groupItem.Item.Name;
			ItemStatusId = groupItem.ItemStatusId;
			LastUpdate = groupItem.LastUpdate;
			Notes = groupItem.Notes;
			DoNotBuy = groupItem.DoNotBuy == true;
			OneTimePurchase = groupItem.OneTimePurchase == true;
		}
	}
}