<template>
  <div class="devLogs">
    <ul id="example-1">
        <li v-for="item in this.devLogs">
            {{ item }}
        </li>
    </ul>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class DevLog extends Vue {
  @Prop() private devLogs!: string[];
}
</script>