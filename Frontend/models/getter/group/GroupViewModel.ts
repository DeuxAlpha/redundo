import GroupUserViewModel from './GroupUserViewModel';
import GroupItemViewModel from './GroupItemViewModel';

export default class GroupViewModel {
  public Id: number;
  public Name: string;
  public Users: Array<GroupUserViewModel> = [];
  public Items: Array<GroupItemViewModel> = [];

  constructor(data?) {
    if (!data) return;
    this.Id = data.id;
    this.Name = data.name;
    this.Users = data.users.map(u => new GroupUserViewModel(u));
    this.Items = data.items.map(i => new GroupItemViewModel(i));
  }
}
