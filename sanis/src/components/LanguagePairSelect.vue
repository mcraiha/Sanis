<template>
  <div class="selectLanguagePair">
    <select v-on:change="languagePairUpdated($event)">
      <option v-for="(pair, index) in pairs" v-bind:value="index">
        {{ pair.entryName }}
      </option>
    </select>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import { IDictionaryDefinition } from '../interfaces/IDictionaryDefinition';

@Component
export default class LanguagePairSelect extends Vue {
  @Prop() private pairs!: IDictionaryDefinition[];

  private languagePairUpdated(event: any) {
      this.$emit('selected', event.target.value);
  }
}
</script>
