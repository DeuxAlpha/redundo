export default class AutocompleteModel {
  public ValueProperty: string;
  public TextProperty: string;
  public Loading: boolean = false;
  public Items: Array<object> = [];

  constructor(textProperty: string, valueProperty: string) {
    this.ValueProperty = valueProperty;
    this.TextProperty = textProperty;
  }

  public SetItems(items: Array<object>) {
    this.Items = [];
    for (let item of items) {
      this.Items.push(item);
    }
  }
}
