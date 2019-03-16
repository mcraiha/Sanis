<template>
  <div class="selectLanguagePair">
    <select v-on:change="languagePairUpdated($event)">
    <option value="1-1">🇫🇮 -> 🇫🇮</option>
    <option value="1-2">🇫🇮 -> 🇺🇸</option>
    <option value="2-1">🇺🇸 -> 🇫🇮</option>
  </select>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

@Component
export default class LanguagePairSelect extends Vue {
  private languagePairUpdated(event: any) {
      console.log(event.target.value);
  }
}
</script>
