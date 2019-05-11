<template>
  <div class="showResults">
    <p>Hakusana: {{ exactSearchTerm }}</p>
    <p>Osuma: 
      <details v-if="doesExactMatchContainSomething()" open>
        <summary><a :href="getFromUrl(exactMatch.word)">{{ exactMatch.word }}</a></summary>
        <p v-for="exactTranslation in this.exactMatch.translations">
          <a :href="getToUrl(exactTranslation)">{{ exactTranslation }}</a>
        </p>
      </details>
    </p>
    <p>Sivuosumat:
      <details v-for="item in this.closestMatches" open>
        <summary><a :href="getFromUrl(item.word)">{{ item.word }}</a></summary>
        <p v-for="translation in item.translations">
          <a :href="getToUrl(translation)">{{ translation }}</a>
        </p>
      </details>
    </p>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

import { IDictionaryDefinition } from '../interfaces/IDictionaryDefinition';

import { IDictionaryEntry } from '../interfaces/IDictionaryEntry';

@Component
export default class ShowResults extends Vue {
  @Prop() private exactSearchTerm!: string;
  @Prop() private exactMatch!: IDictionaryEntry;
  @Prop() private closestMatches!: IDictionaryEntry[];
  @Prop() private dictionaryDefinition!: IDictionaryDefinition;

  private doesExactMatchContainSomething(): boolean
  {
    return !(this.exactMatch.word == null || this.exactMatch.word.length < 1);
  }

  private getFromUrl(term: string): string {
    return `${this.dictionaryDefinition.fromUrl}${term}`;
  }

  private getToUrl(term: string): string {
    return `${this.dictionaryDefinition.toUrl}${term}`;
  }
}
</script>