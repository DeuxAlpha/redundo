import { ItemStatus } from '../../enums/ItemStatus';
import GroupViewModel from './GroupViewModel';

export default class GroupItemViewModel {
  public Id: number;
  public Name: string;
  public ItemStatusId: ItemStatus;
  public LastUpdate: string;
  public Notes: string = '';
  public DoNotBuy: boolean;
  public OneTimePurchase: boolean;
  public DeletionLoading: boolean = false;
  public Group: GroupViewModel;

  constructor(data?: any, group?: GroupViewModel) {
    if (!data) return;
    this.Id = data.id || data.itemId;
    this.Name = data.name;
    this.ItemStatusId = data.itemStatusId;
    this.LastUpdate = data.lastUpdate;
    this.Notes = data.notes;
    this.DoNotBuy = data.doNotBuy;
    this.OneTimePurchase = data.oneTimePurchase;
    if (group)
      this.Group = group;
  }
}
