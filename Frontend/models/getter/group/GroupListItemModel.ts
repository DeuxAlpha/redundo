export default class GroupListItemModel {
  public Id: number;
  public Name: string;
  public IsAcceptedByManager: boolean;
  public IsAcceptedByUser: boolean;
  public IsOwner: boolean;
  public IsManager: boolean;
  public RequestMessage: string;

  constructor(data?) {
    if (!data) return;
    this.Id = data.id;
    this.Name = data.name;
    this.IsAcceptedByManager = data.isAcceptedByManager;
    this.IsAcceptedByUser = data.isAcceptedByUser;
    this.IsOwner = data.isOwner;
    this.IsManager = data.isManager;
    this.RequestMessage = data.requestMessage;
  }
}
