import DayJs from 'dayjs';

export default class DayJsService {
  public static ConvertToLocalTime(dateTime: string): DayJs.Dayjs {
    return DayJs(dateTime).add(DayJs().utcOffset(), 'minute');
  }
}
