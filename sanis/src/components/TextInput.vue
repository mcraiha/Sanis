<template>
  <div class="searchfield">
    <input v-on:input="searchUpdated" v-model="searchTerm" id="word-to-search" type="text">
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class TextInput extends Vue {
  @Prop() private searchTerm!: string;

  private searchUpdated() {
      this.$emit('update:searchTerm', this.searchTerm);
  }
}
</script>
