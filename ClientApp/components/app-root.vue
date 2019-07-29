<template>
  <v-app>
    <div>
      <v-toolbar flat color="white">
        <v-toolbar-title>
          Robot Summary
        </v-toolbar-title>
      </v-toolbar>
    </div>
    <v-data-table :headers="headers" :items="robotLists" item-key="name">
      <template v-slot:items="props">
        <tr @click="props.expanded = !props.expanded">
          <td>{{ props.item.name }}</td>
          <td>{{ props.item.type }}</td>
          <td>{{ props.item.completedTasks}}</td>
        </tr>
      </template>
      <template v-slot:expand="props">
        <task-viewer :taskData="props.item.tasks"></task-viewer>
      </template>
    </v-data-table>

    <!-- Insert Robot component -->
    <div id="insertToggleMe" v-if="dialog == 'Insert'" style="z-index:999px; width:300px;margin:0 auto;">
      <v-layout justify-center>
        <v-dialog v-model="dialog" persistent max-width="600px">
          <template v-slot:activator="{ on }">
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">Robot Creation</span>
            </v-card-title>
            <v-card-text>
              <v-container grid-list-md>
                <v-layout wrap>
                  <v-flex xs12>
                    <v-text-field id="robotName" v-model="name" label="Robot Name*"></v-text-field>
                  </v-flex>
                  <v-flex xs12>
                    <v-select field="robotType" v-model="type" :items="typeOptions" label="Type*"></v-select>
                  </v-flex>
                </v-layout>
              </v-container>
              <small>*indicates required field</small>
            </v-card-text>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="dialog = ''">Close</v-btn>
              <v-btn :disabled="!readyToSubmit" color="blue darken-1" text @click="insertList(),dialog = ''">Save
              </v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-layout>
    </div>

    <!-- Complete task component -->
    <div id="completeTask" v-if="dialog == 'completeTask'" style="z-index:999px; width:300px;margin:0 auto;">
      <v-layout justify-center>
        <v-dialog v-model="dialog" persistent max-width="600px">
          <template v-slot:activator="{ on }">
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">Task Manager</span>
            </v-card-title>
            <complete-task :robotDatabase="robotDatabase" :robotNames="extractRobotNames"></complete-task>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="dialog = ''">Close</v-btn>
              <v-btn color="blue darken-1" text @click="dialog = ''">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-layout>
    </div>

    <!-- Complete Robot component -->
    <div id="completeRobot" v-if="dialog == 'retireRobot'" style="z-index:999px; width:300px;margin:0 auto;">
      <v-layout justify-center>
        <v-dialog v-model="dialog" persistent max-width="600px">
          <template v-slot:activator="{ on }">
          </template>
          <v-card>
            <v-card-title>
              <span class="headline">Control Panel</span>
            </v-card-title>
            <delete-robot :robotNames="extractRobotNames"></delete-robot>
            <v-card-actions>
              <v-spacer></v-spacer>
              <v-btn color="blue darken-1" text @click="dialog = ''">Close</v-btn>
              <v-btn :disabled="!readyToSubmit" color="blue darken-1" text @click="dialog = ''">Save</v-btn>
            </v-card-actions>
          </v-card>
        </v-dialog>
      </v-layout>
    </div>

    <!-- Bottom nav component -->
    <div id="bottomNav">
      <v-bottom-nav absolute :value="true" color="transparent">
        <v-btn flat color="teal" value="insert" @click="dialog = 'Insert'">
          <span>Create Robot</span>
          <v-icon>folder_open</v-icon>
        </v-btn>
        <v-btn flat color="teal" value="delete" @click="dialog = 'completeTask'">
          <span>Start Task</span>
          <v-icon>gavel</v-icon>
        </v-btn>
        <v-btn flat color="teal" value="search" @click="dialog = 'retireRobot'">
          <span>Complete</span>
          <v-icon>info</v-icon>
        </v-btn>
      </v-bottom-nav>
    </div>
  </v-app>
</template>

<script>
import axios from 'axios'
import Vue from 'vue'
import TaskViewer from "./task-viewer.vue";
import completeTask from "./complete-task-component.vue";
import deleteRobot from "./delete-robot.vue";
import store from "../store/index.js";
import { mapGetters, mapMutations} from 'vuex';

//  Declaration of outside components
Vue.component('complete-task', completeTask);
Vue.component('task-viewer', TaskViewer);
Vue.component('delete-robot', deleteRobot);

/*
    Points of interest in the frontend:

    Use of 'async'
    ------------------------------
    -  All of the async functions contain an await expression, and what this
       does is waits for the passed Promise's resolution, once it recieves the 
       resolution, it resumes the async function's execution.  This is useful
       for our use of querying the database and grabbing info from the store -
       it ensures the data will be entered/modified already so we don't run into
       any exceptions
    
    Use of 'Vuex'
    -------------------------------
    -  Vuex is just a central state management system - it allows us to access 
       data across components easier and in realtime

*/

export default {
  store,
  name: 'app',
  data: function () {
    return {
      expanded: {},
      dialog: '',
      headers: [{
          text: 'Name',
          value: 'name'
        },
        {
          text: 'Type',
          value: 'type'
        },
        {
          text: 'Tasks Completed',
          value: 'tasksCompleted'
        }
      ],

      robotDatabase: [],
      name: '',
      type: '',
    }
  },
  methods: {
    ...mapMutations({
      setRobotList: 'set_robotList',
      addRobot: 'add_robot'
    }),
    //  Here I loaded up a Vuex store - this is a useful library to keep
    //  'state' without having to constantly reload the page
    async loadStore() {
      this.robotDatabase = (await axios.get("/api/robots")).data;
      this.setRobotList(this.robotDatabase);
    },
    async insertList() {
      var payload = {
        name: this.name,
        type: this.type,
        tasks: this.taskDictionary[this.type]
      };
      await axios.post("api/robots/create", payload),
        await this.loadStore();
    }

  },
  computed: {
    //  Here we actually go grab the data from our Vuex store
    robotLists() {
      return this.$store.getters.get_robotList.robots;
    },
    //  Basic bounds checking - a regex could also be used for thoroughness
    readyToSubmit() {
      return this.name.length > 0 && this.type != '';
    },
    extractRobotNames() {
      return this.robotLists.map(robot => robot.name);
    },
    //  Possibly could rethink this data structure
    //  Pro : O(1) lookup & addition time!
    //  Con : Memory usage  (Array & Dictionary)
    taskDictionary() {
      var tasks = [{
        name: 'do the dishes',
        time: 1000,
        State: 'Unstarted'
      }, {
        name: 'sweep the house',
        time: 3000,
        State: 'Unstarted'
      }, {
        name: 'do the laundry',
        time: 10000,
        State: 'Unstarted'
      }, {
        name: 'take out the recycling',
        time: 4000,
        State: 'Unstarted'
      }, {
        name: 'make a sammich',
        time: 7000,
        State: 'Unstarted'
      }, {
        name: 'mow the lawn',
        time: 20000,
        State: 'Unstarted'
      }, {
        name: 'rake the leaves',
        time: 18000,
        State: 'Unstarted'
      }, {
        name: 'give the dog a bath',
        time: 14500,
        State: 'Unstarted'
      }, {
        name: 'bake some cookies',
        time: 8000,
        State: 'Unstarted'
      }, {
        name: 'wash the car',
        time: 20000,
        State: 'Unstarted'
      }];

      var dictionary = {
        'Unipedal': [tasks[0], tasks[6], tasks[2], tasks[8]],
        'Bipedal': [tasks[1], tasks[7], tasks[3], tasks[9]],
        'Quadrupedal': [tasks[2], tasks[8], tasks[4], tasks[9]],
        'Arachnid': [tasks[3], tasks[9], tasks[5], tasks[0]],
        'Radial': [tasks[4], tasks[9], tasks[6], tasks[1]],
        'Aeronautical': [tasks[5], tasks[1], tasks[7], tasks[2]]
      };
      return dictionary;
    },
    //  The dictionary is versatile - allows us to access
    //  both the 'keys' and 'values'
    typeOptions() {
      return Object.keys(this.taskDictionary);
    },
  },
  async beforeMount() {
    await this.loadStore()
  }
}

</script>
<style scoped>
@import "../../node_modules/@syncfusion/ej2-vue-grids/styles/material.css";
@import "../../node_modules/@syncfusion/ej2-base/styles/material.css";
@import "../../node_modules/@syncfusion/ej2-inputs/styles/material.css";
@import "../../node_modules/@syncfusion/ej2-dropdowns/styles/material.css";
@import "../../node_modules/@syncfusion/ej2-navigations/styles/material.css";
</style>