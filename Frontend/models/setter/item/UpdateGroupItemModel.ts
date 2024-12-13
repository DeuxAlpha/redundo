import { ItemStatus } from '../../enums/ItemStatus';
import DashboardItemModel from '../../getter/dashboard/DashboardItemModel';

export default class UpdateGroupItemModel {
  public GroupId: number;
  public ItemId: number;
  public ItemStatusId: ItemStatus;
  public Notes: string;
  public OneTimePurchase: boolean;
  public DoNotBuy: boolean;

  public static Create(item: DashboardItemModel, newItemStatus: ItemStatus): UpdateGroupItemModel {
    const updateItem = new UpdateGroupItemModel();
    updateItem.GroupId = item.Group.Id;
    updateItem.ItemId = item.Id;
    updateItem.ItemStatusId = newItemStatus;
    updateItem.Notes = item.Notes;
    updateItem.OneTimePurchase = item.OneTimePurchase;
    updateItem.DoNotBuy = newItemStatus === ItemStatus.Stocked && item.OneTimePurchase;
    return updateItem;
  }

  public static Generate(item: DashboardItemModel): UpdateGroupItemModel {
    const updateItem = new UpdateGroupItemModel();
    updateItem.GroupId = item.Group.Id;
    updateItem.ItemId = item.Id;
    if (item.PendingOkayStatus) {
      updateItem.ItemStatusId = ItemStatus.Stocked;
    } else if (item.PendingAlmostOutStatus) {
      updateItem.ItemStatusId = ItemStatus.AlmostOut;
    } else if (item.PendingOutStatus) {
      updateItem.ItemStatusId = ItemStatus.Out;
    }
    updateItem.Notes = item.Notes;
    updateItem.OneTimePurchase = item.OneTimePurchase;
    updateItem.DoNotBuy = updateItem.ItemStatusId === ItemStatus.Stocked && item.OneTimePurchase;
    return updateItem;
  }
}
