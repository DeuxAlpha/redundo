import DayJs from 'dayjs';
import Cookies from 'js-cookie';
import String from '../common/fake-extensions/String';

export default class CookieService {
  public static SetPersistentCookie(name: string, value: string, expirationDate: string) {
    const expirationDays = Math.abs(Math.round(DayJs().diff(DayJs(expirationDate), 'hour') / 24));
    Cookies.set(name, value, {expires: expirationDays});
  }

  public static SetSessionCookie(name: string, value: string) {
    Cookies.set(name, value);
  }

  public static RemoveCookie(name: string) {
    Cookies.remove(name);
  }

  public static GetCookie(name: string): string {
    const cookie = Cookies.get(name);
    if (cookie) return cookie;
    throw new Error('Cookie is null');
  }

  public static GetServerCookie(name: string, serverContext: any): string | undefined {
    if (!serverContext.req.headers.cookie)
      return '';
    const cookie = serverContext.req.headers.cookie
      .split('; ')
      .filter(ca => {
        return String.SplitByFirst(ca, '=') === name;
      })[0] as string;
    if (!cookie) return undefined;
    return String.SplitByFirst(cookie, '=', false);
  }
}
