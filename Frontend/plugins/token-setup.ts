import TokenModel from '../models/getter/user/TokenModel';
import CookieService from '../services/CookieService';

export default({store, $axios}) => {
  const token = store.getters['UserStore/tokenModel'] as TokenModel;
  if (token.JwtToken && token.RefreshToken) {
    CookieService.SetPersistentCookie('jwt', token.JwtToken, token.RefreshTokenExpirationDate);
    CookieService.SetPersistentCookie('refresh-token', token.RefreshToken, token.RefreshTokenExpirationDate);
    $axios.setToken(token.JwtToken, 'Bearer');
  }
}
