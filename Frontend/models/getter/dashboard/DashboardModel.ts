import DashboardItemModel from './DashboardItemModel';
import DashboardGroupModel from './DashboardGroupModel';

export default class DashboardModel {
  public Items: Array<DashboardItemModel>;
  public Groups: Array<DashboardGroupModel>;
  public AnyGroups: boolean;

  private setupGroups() {
    this.Groups = [];
    for (let item of this.Items) {
      if (this.Groups.find(g => g.Id === item.Group.Id))
        continue;
      this.Groups.push(item.Group);
    }
  }

  constructor(data?) {
    if (!data) return;
    this.AnyGroups = data.anyGroups;
    this.Items = data.items.map(i => new DashboardItemModel(i));
    this.setupGroups();
  }
}
