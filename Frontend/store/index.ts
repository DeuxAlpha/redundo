import CookieService from '../services/CookieService';

export const actions = {
  async nuxtServerInit(store, context) {
    await refreshToken(store, context);
  }
};

async function refreshToken(store, context) {
  const jwt = CookieService.GetServerCookie('jwt', context);
  const refreshToken = CookieService.GetServerCookie('refresh-token', context);
  if (!jwt || !refreshToken) return;
  context.$axios.setToken(jwt, 'Bearer');
  await store.dispatch('UserStore/refresh', {
    jwtToken: jwt,
    refreshToken: refreshToken
  });
}
