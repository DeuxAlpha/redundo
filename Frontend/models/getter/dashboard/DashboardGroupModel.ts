import GroupViewModel from '../group/GroupViewModel';

export default class DashboardGroupModel {
  public Id: number;
  public Name: string;

  constructor(data?) {
    if (!data) return;
    this.Id = data.id;
    this.Name = data.name;
  }

  public static Create(group: GroupViewModel): DashboardGroupModel {
    const dashboardGroup: DashboardGroupModel = new DashboardGroupModel();
    dashboardGroup.Id = group.Id;
    dashboardGroup.Name = group.Name;
    return dashboardGroup;
  }
}
