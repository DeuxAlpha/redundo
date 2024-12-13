export enum AlertType {
  Info,
  Warning,
  Error,
  Success
}

export default class AlertDisplay {
  public Message: string;
  public Show: boolean = false;
  private readonly alertType: AlertType;
  private timeout;

  constructor(alertType: AlertType, message: string = '', timeout?: number, animateIn: boolean = true) {
    this.Message = message;
    this.alertType = alertType;
    if (animateIn) setTimeout(() => {
      this.Show = true;
    }, 50);
    else
      this.Show = true;
    if (!timeout) return;
    this.timeout = setTimeout(() => {
      this.Show = false;
    }, timeout);
  }

  get AlertType(): string {
    switch (this.alertType) {
      case AlertType.Info:
        return 'info';
      case AlertType.Warning:
        return 'warning';
      case AlertType.Error:
        return 'error';
      case AlertType.Success:
        return 'success';
      default:
        return 'info';
    }
  }
}
