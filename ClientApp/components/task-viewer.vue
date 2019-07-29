
<template>
    <ejs-grid :dataSource="taskData" :detailTemplate="detailTemplate">
        <e-columns>
            <e-column field="name" headerText="Task Name"></e-column>
            <e-column field="time" headerText="Time (milliseconds)"></e-column>
            <e-column field="state" headerText="Current State"></e-column>
        </e-columns>
    </ejs-grid>
</template>
<script>
import axios from 'axios'
import Vue from 'vue'
import { GridPlugin, DetailRow} from '@syncfusion/ej2-vue-grids'

Vue.use(GridPlugin);

export default {
    name: 'app',
    props: ['taskData'],
    data: function () {
        return {
            detailTemplate: () => ({
                template: ''
            })
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