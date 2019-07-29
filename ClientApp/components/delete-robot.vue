
<template>
    <div>
        <v-card-text>
            <v-container grid-list-md>
                <v-layout wrap>
                    <v-flex xs12>
                        <v-select field="robotType" v-model="selected" :items="robotNames" label="Select Robot">
                        </v-select>
                    </v-flex>
                    <v-flex v-if="selected != ''" xs12>
                        <p> {{ selected }} </p>
                        <v-btn @click="retireRobot">Retire</v-btn>
                    </v-flex>
                </v-layout>
            </v-container>
        </v-card-text>
    </div>
</template>
<script>
import axios from 'axios'
import Vue from 'vue'
import store from "../store/index.js";
import { mapMutations } from 'vuex';

export default {
    store,
    props: ['robotNames'],
    data: function () {
        return {
            selected: '',
        }
    },
    methods: {
        ...mapMutations({
            deleteRobot: 'remove_robot',
            setRobotList: 'set_robotList',
        }),
        async loadDatabase() {
            this.robotDatabase = (await axios.get("/api/robots")).data;
            this.setRobotList(this.robotDatabase);
        },
        async retireRobot() {
            await axios.delete("api/robots/delete/" + this.selected);
            this.deleteRobot(this.selected);
            await this.loadDatabase()

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