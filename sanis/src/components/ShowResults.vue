<template>
  <div class="showResults">
    <p>Hakusana: {{ exactSearchTerm }}</p>
    <p>Osuma: 
      <details v-if="doesExactMatchContainSomething()" open>
        <summary>{{ exactMatch.word }}</summary>
        <p v-for="exactTranslation in this.exactMatch.translations">
          {{ exactTranslation }}
        </p>
      </details>
    </p>
    <p>Sivuosumat:
      <details v-for="item in this.closestMatches" open>
        <summary>{{ item.word }}</summary>
        <p v-for="translation in item.translations">
          {{ translation }}
        </p>
      </details>
    </p>
  </div>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';

import { IDictionaryEntry } from '../interfaces/IDictionaryEntry';

@Component
export default class ShowResults extends Vue {
  @Prop() private exactSearchTerm!: string;
  @Prop() private exactMatch!: IDictionaryEntry;
  @Prop() private closestMatches!: IDictionaryEntry[];

  private doesExactMatchContainSomething() : boolean
  {
    return !(this.exactMatch.word == null || this.exactMatch.word.length < 1);
  }
}
</script>