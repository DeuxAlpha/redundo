export default class SnackMessage {
  public Message: string;
  public Color: string;
  public Timeout: number;

  constructor(message: string, color: string, timeout: number = 6000) {
    this.Message = message;
    this.Color = color;
    this.Timeout = timeout;
  }
}
