import { ItemStatus } from '../../enums/ItemStatus';
import DashboardGroupModel from './DashboardGroupModel';
import GroupItemViewModel from '../group/GroupItemViewModel';

export default class DashboardItemModel {
  public Id: number;
  public Name: string;
  public ItemStatusId: ItemStatus;
  public LastUpdate: string;
  public Notes: string;
  public OneTimePurchase: boolean;
  public PendingOutStatus: boolean = false;
  public PendingAlmostOutStatus: boolean = false;
  public PendingOkayStatus: boolean = false;
  public Group: DashboardGroupModel;

  constructor(data?) {
    if (!data) return;
    this.Id = data.id;
    this.Name = data.name;
    this.ItemStatusId = data.itemStatusId;
    this.LastUpdate = data.lastUpdate;
    this.Notes = data.notes;
    this.OneTimePurchase = data.oneTimePurchase;
    this.Group = new DashboardGroupModel(data.group);
  }

  public static Create(item: GroupItemViewModel): DashboardItemModel {
    const dashboardItem = new DashboardItemModel();
    dashboardItem.Id = item.Id;
    dashboardItem.Group = DashboardGroupModel.Create(item.Group);
    dashboardItem.Name = item.Name;
    dashboardItem.ItemStatusId = item.ItemStatusId;
    dashboardItem.LastUpdate = item.LastUpdate;
    dashboardItem.Notes = item.Notes;
    dashboardItem.OneTimePurchase = item.OneTimePurchase;
    return dashboardItem;
  }
}
