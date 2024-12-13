export enum ItemStatus {
  Stocked = 1,
  AlmostOut = 2,
  Out = 3,
}

export default class ItemStatusModel {
  public Id: number;
  public Status: string;

  constructor(id: ItemStatus, status: string) {
    this.Id = id;
    this.Status = status;
  }

  public static GetItemStatuses(): Array<ItemStatusModel> {
    return [
      new ItemStatusModel(ItemStatus.Stocked, 'Stocked'),
      new ItemStatusModel(ItemStatus.AlmostOut, 'Almost out'),
      new ItemStatusModel(ItemStatus.Out, 'Out'),
    ]
  }
}
