<template>
  <div class="searchfield">
    <input v-on:input="$emit('update:searchTerm', $event.target.value)" v-bind:value="searchTerm" id="word-to-search" type="text">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class TextInput extends Vue {
  @Prop() private searchTerm!: string;
}
</script>
