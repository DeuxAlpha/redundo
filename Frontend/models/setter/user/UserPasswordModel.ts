export default class UserPasswordModel {
  public Id: number;
  public OldPassword: string = '';
  public NewPassword: string = '';
  public RepeatPassword: string = '';

  public Reset() {
    this.OldPassword = '';
    this.NewPassword = '';
    this.RepeatPassword = '';
  }
}
