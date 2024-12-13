import { ItemStatus } from '../../enums/ItemStatus';
import GroupItemViewModel from '../../getter/group/GroupItemViewModel';

export default class CreateGroupItemModel {
  public GroupId: number;
  public ItemId: number;
  public ItemStatusId: ItemStatus;
  public Notes: string;
  public OneTimePurchase: boolean;
  public DoNotBuy: boolean;

  public static Create(groupItem: GroupItemViewModel, groupId: number): CreateGroupItemModel {
    const item = new CreateGroupItemModel();
    item.GroupId = groupId;
    item.ItemId = groupItem.Id;
    item.ItemStatusId = groupItem.ItemStatusId;
    item.Notes = groupItem.Notes;
    item.OneTimePurchase = groupItem.OneTimePurchase;
    item.DoNotBuy = groupItem.DoNotBuy;
    return item;
  }
}
