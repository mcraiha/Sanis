<template>
  <div class="showResults">
    <p>Hakusana: {{ exactSearchTerm }}</p>
    <p>Osuma: {{ exactMatch }}</p>
    <ul id="example-1">
        <li v-for="item in this.closestMatches">
            {{ item }}
        </li>
    </ul>
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
}
</script>