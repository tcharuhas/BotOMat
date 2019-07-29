import Vue from 'vue'
import axios from 'axios'
import router from './router/index'
import store from './store'
import {
  sync
} from 'vuex-router-sync'
import App from 'components/app-root'
import {
  FontAwesomeIcon
} from './icons'
import Vuetify from 'vuetify'
import theme from './theme'
import Uploader from 'vue-simple-uploader'
import VueCookies from 'vue-cookies'

// Use Vuetify
Vue.use(Vuetify, {
  theme
})

// Use vue-simple-uploader
Vue.use(Uploader)

Vue.use(VueCookies)

// Add font-awesome icons
// USE: <fa-icon :icon="{icon name}"></fa-icon>
Vue.component('fa-icon', FontAwesomeIcon)

// Event Bus
export const EventBus = new Vue()

// make Axios default $http
Vue.prototype.$http = axios

sync(store, router)

const app = new Vue({
  store,
  router,
  ...App
})

export {
  app,
  router,
  store
}

