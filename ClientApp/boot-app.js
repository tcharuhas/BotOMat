import './css/site.css'
import './css/color-palette.scss' // custom color palette
import 'core-js/es6/promise'
import 'core-js/es6/array'

import 'vuetify/dist/vuetify.min.css' // Vuetify
import 'vuetify/es5/util/colors' // Vuetify Colors

import 'ag-grid-community/dist/styles/ag-grid.css' // ag Grid css
import 'ag-grid-community/dist/styles/ag-theme-balham.css' // ag Grid theme
import 'ag-grid-community/dist/styles/ag-theme-balham-dark.css' // ag Grid dark theme

import {
    app
} from './app'

app.$mount('#app')