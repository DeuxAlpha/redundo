export default class UserGroupRoleModel {
  public Id: number;
  public Value: string;

  constructor(id: number, value: string) {
    this.Id = id;
    this.Value = value;
  }
}
