import UserAccountModel from '../models/setter/user/UserAccountModel';
import TokenModel from '../models/getter/user/TokenModel';
import UserLoginModel from '../models/setter/user/UserLoginModel';
import CookieService from '../services/CookieService';
import RefreshModel from '../models/setter/user/RefreshModel';
import DayJs from 'dayjs';

interface IRefreshIntervalModel {
  context;
}

interface IUserState {
  user: UserAccountModel;
  tokenModel: TokenModel;
  refreshInterval;
}

export const state = (): IUserState => {
  let user = new UserAccountModel();
  let tokenModel = new TokenModel();
  let refreshInterval = null;
  return {
    user,
    tokenModel,
    refreshInterval
  }
};

export const mutations = {
  storeUser(state: IUserState, user) {
    state.user.Id = user.id;
    state.user.Username = user.username;
  },
  storeToken(state: IUserState, tokenModel: TokenModel) {
    state.tokenModel = tokenModel;
  },
  setTimeout(state: IUserState, payload: IRefreshIntervalModel) {
    const timeoutInMs = Math.abs(DayJs().diff(DayJs(state.tokenModel.JwtExpirationDate), 'millisecond'));
    state.refreshInterval = setTimeout(() => {
      payload.context.dispatch('refresh', {
        jwtToken: state.tokenModel.JwtToken,
        refreshToken: state.tokenModel.RefreshToken
      }).then(() => {
        payload.context.commit('setTimeout', {
          context: payload.context
        });
      });
    }, timeoutInMs);
  },
  clearUser(state: IUserState) {
    state.user = new UserAccountModel();
  },
  clearToken(state: IUserState) {
    state.tokenModel = new TokenModel();
  },
  clearRefreshInterval(state: IUserState) {
    clearTimeout(state.refreshInterval);
  }
};

export const actions = {
  async login(context, loginModel: UserLoginModel) {
    return await this.$axios.post('users/login', loginModel)
      .then(response => {
        context.commit('storeUser', response.data.user);
        context.commit('storeToken', {
          JwtToken: response.data.jwtToken,
          RefreshToken: response.data.refreshToken,
          JwtExpirationDate: response.data.jwtExpirationDate,
          RefreshTokenExpirationDate: response.data.refreshTokenExpirationDate
        });
        context.commit('setTimeout', {
          context: context
        });
        CookieService.SetPersistentCookie('jwt', response.data.jwtToken, response.data.refreshTokenExpirationDate);
        CookieService.SetPersistentCookie('refresh-token', response.data.refreshToken, response.data.refreshTokenExpirationDate);
        this.$axios.setToken(response.data.jwtToken, 'Bearer');
        return response;
      })
      .catch(error => {
        return error;
      })
  },
  async refresh(context, refreshModel: RefreshModel) {
    await this.$axios.post('users/refresh', refreshModel)
      .then(response => {
        context.commit('storeUser', response.data.user);
        context.commit('storeToken', {
          JwtToken: response.data.jwtToken,
          RefreshToken: response.data.refreshToken,
          JwtExpirationDate: response.data.jwtExpirationDate,
          RefreshTokenExpirationDate: response.data.refreshTokenExpirationDate
        });
        CookieService.SetPersistentCookie('jwt', response.data.jwtToken, response.refreshTokenExpirationDate);
        CookieService.SetPersistentCookie('refresh-token', response.data.refreshToken, response.refreshTokenExpirationDate);
        this.$axios.setToken(response.data.jwtToken, 'Bearer');
        return response;
      })
      .catch(error => {
        return error;
      })
  },
  setupTimeout(context) {
    context.commit('setTimeout', {
      context: context,
      tokenModel: context.getters['tokenModel']
    });
  },
  logout(context) {
    context.commit('clearUser');
    context.commit('clearToken');
    context.commit('clearRefreshInterval');
    CookieService.RemoveCookie('jwt');
    CookieService.RemoveCookie('refresh-token');
    this.$axios.setToken(false, 'Bearer');
  }
};

export const getters = {
  user(state: IUserState): UserAccountModel {
    return state.user;
  },
  tokenModel(state: IUserState): TokenModel {
    return state.tokenModel;
  },
  jwtToken(state: IUserState): string {
    return state.tokenModel.JwtToken;
  }
};
