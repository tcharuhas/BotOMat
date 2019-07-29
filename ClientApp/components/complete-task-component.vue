<template>
<v-card-text>
    <v-container grid-list-md>
        <v-layout wrap>
            <v-flex xs12>
                <v-select field="robotType" v-model="selected" :items="robotNames" label="Type*"></v-select>
            </v-flex>
            <v-flex xs12>
                <div v-if="loadTasksList && loadTasksList.length">
                    <div v-for="task in loadTasksList" :key="task.name">
                        <div v-if="task.state == 'Unstarted'">
                            {{task.name}} in {{task.time / 1000}} seconds

                            <v-btn v-if="currentTask != task.name" @click="timer(task.name,task.time)">start</v-btn>
                            <p v-if="loading && currentTask == task.name">completing ...</p>
                        </div>
                        <div v-else>
                            <p>{{task.name}} - Your robot is finished with this task!</p>
                        </div>
                    </div>
                </div>
            </v-flex>
        </v-layout>
    </v-container>
    <small>*indicates required field</small>
</v-card-text>
</template>
<script>
import axios from 'axios'
import Vue from 'vue'
import store from "../store/index.js";
import { mapMutations } from 'vuex';

export default {
    store,
    name: 'app',
    props: ['robotDatabase', 'robotNames'],
    data: function () {
        return {
            loading: false,
            currentTask: '',
            names: [],
            renderedDatabase: [],
            selected: ''
        }
    },
    methods: {
        ...mapMutations({
            deleteRobot: 'remove_robot',
            setRobotList: 'set_robotList',
            addRobot: 'add_robot'

        }),
        //  Use of the Vuex store again ...
        async loadDatabase() {
            this.renderedDatabase = (await axios.get("/api/robots")).data;
            this.setRobotList(this.renderedDatabase);
        },
        async startTask(name) {
            this.loading = false;
            await axios.post("api/robots/" + this.selected + "/" + name);
            this.loadDatabase();
        },
        //  Arrow function utilized to implement Javascript's timeout function
        timer(name, time) {
            this.loading = true;
            this.currentTask = name;
            setTimeout(() => {
                this.startTask(name)
            }, time);
        }
    },
    computed: {
        robotLists() {
            return this.$store.getters.get_robotList.robots;
        },
        loadTasksList() {
            var obj = null;
            for (var i = 0; i < this.robotLists.length; i++) {
                if (this.robotLists[i].name == this.selected) {
                    obj = this.robotLists[i];
                }
            }
            return obj ? obj.tasks : null;
        }
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