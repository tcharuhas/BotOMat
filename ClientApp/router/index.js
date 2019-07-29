import Vue from 'vue'
import VueRouter from 'vue-router'
import { routes } from './routes'
import Vuetify from 'vuetify'

Vue.use(VueRouter)
Vue.use(Vuetify)

let router = new VueRouter({
  mode: 'history',
  routes
})

export default router
