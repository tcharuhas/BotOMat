import Vue from 'vue'
import Vuex from 'vuex'

Vue.use(Vuex)

// STATE
// basically just the data in the state
const state = {
  robotList: []
}

// GETTERS
// called to get state data - can shorten and clean up code
const getters = {
  get_robotList: state => {
    return state.robotList;
  }
}

// MUTATIONS
// called so state data isn't directly modified - allows better tracking
const mutations = {
  set_robotList: (state, payload) => {
    state.robotList = payload;
  },

  add_robot: (state, payload) => {
    state.robotList.add(payload);
  },

  remove_robot: (state, robot) => {
    for (var i = 0; i < state.robotList.length; i++) {
      if (state.robotList[i].name == robot) {
        state.robotList.Remove(state.robotList[i]);
      }
    }
  }

}

// ACTIONS
// called to perform mutations - allows async processes (mutations are only sync)
const actions = {

  do_set_robots: ({
    commit
  }, payload) => {
    commit('set_robots', payload)
  }

}

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters,
})