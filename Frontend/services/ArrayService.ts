export default class ArrayService {
  public static RemoveFromArray<T>(array: Array<T>, item: T): Array<T> {
    return array.splice(array.indexOf(item), 1);
  }
}
