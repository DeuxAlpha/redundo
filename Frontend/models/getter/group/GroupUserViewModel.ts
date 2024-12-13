export default class GroupUserViewModel {
  public Id: number;
  public Name: string;
  public IsManager: boolean;
  public IsOwner: boolean;
  public IsAcceptedByUser: boolean;
  public IsAcceptedByManager: boolean;
  public RequestMessage: string;

  constructor(data?) {
    if (!data) return;
    this.Id = data.id;
    this.Name = data.name;
    this.IsManager = data.isManager;
    this.IsOwner = data.isOwner;
    this.IsAcceptedByUser = data.isAcceptedByUser;
    this.IsAcceptedByManager = data.isAcceptedByManager;
    this.RequestMessage = data.requestMessage;
  }
}
