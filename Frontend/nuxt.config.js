import VuetifyLoaderPlugin from 'vuetify-loader/lib/plugin'
import pkg from './package'

const host = process.env.HOST || 'localhost';
const port = process.env.PORT || '3000';

export default {
  mode: 'universal',

  env: {
    baseUrl: process.env.BASE_URL || `http://${ host }:${ port }`
  },

  /*
  ** Headers of the page
  */
  head: {
    title: 'Redundo',
    meta: [
      {charset: 'utf-8'},
      {name: 'viewport', content: 'width=device-width, initial-scale=1'},
      {hid: 'description', name: 'description', content: pkg.description}
    ],
    link: [
      {rel: 'icon', type: 'image/x-icon', href: '/favicon.ico'},
      {
        rel: 'stylesheet',
        href:
          'https://fonts.googleapis.com/css?family=Roboto:300,400,500,700|Material+Icons'
      }
    ]
  },

  /*
  ** Customize the progress-bar color
  */
  loading: {color: '#fff'},

  /*
  ** Global CSS
  */
  css: [
    '~/assets/style/app.styl',
    '~/assets/style/app.scss'
  ],

  /*
  ** Plugins to load before mounting the App
  */
  plugins: [
    '@/plugins/vuetify',
    {src: '@/plugins/token-setup', ssr: false},
    '@/plugins/dayjs-plugin'
  ],

  /*
  ** Nuxt.js modules
  */
  modules: [
    // Doc: https://axios.nuxtjs.org/usage
    '@nuxtjs/axios',
    '@nuxtjs/pwa',
  ],
  /*
  ** Axios module configuration
  */
  axios: {
    // See https://github.com/nuxt-community/axios-module#options
    withCredentials: true,

    baseURL: process.env.API_URL || 'http://localhost:5000/api/',
    browserBaseURL: process.env.API_BROWSER_URL || 'http://localhost:5000/api/',

    debug: false
  },

  workbox: {
    autoRegister: false
  },

  meta: {
    mobileAppIOS: true,
    name: 'Redundo',
    theme_color: '#abf2db'
  },

  manifest: {
    name: 'Redundo',
    background_color: '#abf2db',
    description: pkg.description,
    display: 'standalone',
    lang: 'en-US',
    theme_color: '#abf2db',
    start_url: '/dashboard',
  },

  /*
  ** Build configuration
  */
  build: {
    transpile: ['vuetify/lib'],
    plugins: [new VuetifyLoaderPlugin()],
    loaders: {
      stylus: {
        import: ['~assets/style/variables.styl']
      }
    },
    /*
    ** You can extend webpack config here
    */
    extend(config, ctx) {
      // Check for development environment
      if (host === 'localhost') {
        config.devtool = ctx.isClient ? 'eval-source-map' : 'inline-source-map'
      }
    }
  }
}