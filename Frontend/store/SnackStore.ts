// Store to hold snackbars, which will be displayed in the Backend layout.

import SnackMessage from '../models/app/SnackMessage';

export interface ISnackState {
  message: string;
  color: string;
  show: boolean;
}

export const state = (): ISnackState => {
  let message = '';
  let color = '';
  let show = false;
  return {
    message,
    color,
    show
  }
};

export const mutations = {
  setMessage(state: ISnackState, message: string) {
    state.message = message;
  },
  setColor(state: ISnackState, color: string) {
    state.color = color;
  },
  setActive(state: ISnackState) {
    state.show = true;
  },
  setInactive(state: ISnackState) {
    state.show = false;
  }
};

export const actions = {
  addSnack(context, snack: SnackMessage) {
    context.commit('setMessage', snack.Message);
    context.commit('setColor', snack.Color);
    context.commit('setActive');
    setTimeout(() => {
      context.commit('setInactive');
    }, snack.Timeout);
  }
};

export const getters = {
  snack(state: ISnackState): ISnackState {
    return state
  }
};
