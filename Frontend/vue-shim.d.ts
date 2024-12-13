import {AxiosInstance} from 'axios';
import Router from 'vue-router';
import Vue, {ComponentOptions} from 'vue';

declare module 'vue/types/vue' {
  interface Vue {
    $axios: AxiosInstance;
    $router: Router;
  }
}

declare module 'vue/types/options' {
  interface ComponentOptions<V extends Vue> {
    middleware?: string | string[];
    layout?: string;
    asyncData?: Function;
    fetch?: Function;
  }
}
