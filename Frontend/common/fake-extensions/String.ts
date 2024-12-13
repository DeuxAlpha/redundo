export default class String {
  public static SplitByFirst(value: string, separator: string, returnFirst: boolean = true) : string {
    return returnFirst
      ? value.split(separator)[0]
      : value.split(separator).slice(1).join(separator);
  }
}
