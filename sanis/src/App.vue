<template>
  <div id="app">
    <DevLog v-bind:devLogs="devLog" />
    <TextInput v-bind:searchTerm.sync="searchTerm"/>
    <p>{{ searchTerm }}</p>
    <LanguagePairSelect />
    <ShowResults v-bind:exactSearchTerm="searchTerm" v-bind:exactMatch="getExactMatch(searchTerm)" v-bind:closestMatches="getPartialMactches(searchTerm, 5)" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import { Trie } from "prefix-trie-ts";

import { IDictionaryEntry } from './interfaces/IDictionaryEntry';

import TextInput from './components/TextInput.vue';
import LanguagePairSelect from './components/LanguagePairSelect.vue';
import ShowResults from './components/ShowResults.vue';
import DevLog from './components/DevLog.vue';

@Component({
  components: {
    TextInput,
    LanguagePairSelect,
    ShowResults,
    DevLog
  },
  data: function () {
    return { 
      searchTerm: 'abs' as string, 
      dataLoaded: false as boolean, 
      dictionary: null as any,
      currentTrie: null as any,

      // Development log
      devLogEnabled: false as boolean,
      devLog: ['Dev log start'] as string[], 
      }
  },
  methods: {
    getExactMatch(searchKeyword: string): IDictionaryEntry {
      if (this.$data.dictionary.hasOwnProperty(searchKeyword))
      {
        return { word: searchKeyword, translations: this.$data.dictionary[searchKeyword].translations };
      }

      return { word: '', translations: [] };
    },

    getPartialMactches(searchKeyword: string, maxAmount: number): IDictionaryEntry[] {

      // Do not check anything if search keyword is less than 2 chars
      if (searchKeyword.length < 2)
      {
        return [];
      }

      // Take one more in case the results also contain exact match
      const seekAmount: number = maxAmount + 1;
      const sliced = this.$data.currentTrie.getPrefix(searchKeyword).slice(0, seekAmount);
      const gotEnough: boolean = (seekAmount === sliced.length);

      const possibleIndexOfMatch : number = sliced.indexOf(searchKeyword);
      if (possibleIndexOfMatch > -1)
      {
        // Remove exact match
        sliced.splice(possibleIndexOfMatch, 1);
      }
      else if (gotEnough)
      {
        // No exact match, and there is one match too many, so remove last array element
        sliced.pop();
      }

      const returnArray: IDictionaryEntry[] = [];

      for (let i = 0; i < sliced.length; i++)
      {
        const partialMatchWord: string = sliced[i];
        returnArray.push({ word: partialMatchWord, translations: this.$data.dictionary[partialMatchWord].translations});
      }

      return returnArray;
    },
  }
})

export default class App extends Vue {

  // Lifecycle hook
  async mounted ()
  {
    const jsonHandleStartTime = performance.now();

    const response = await fetch('dictionaries/1-2.json');
    const data = await response.json();
    this.$data.dictionary = data;

    const jsonHandleEndTime = performance.now();

    this.$data.devLog.push(`json handling took: ${jsonHandleEndTime - jsonHandleStartTime} milliseconds`);


    this.$data.devLog.push(`dictionary has ${Object.keys(data).length} entries`);


    const trieCreateStartTime = performance.now();

    const trie = new Trie(Object.keys(data));

    const trieCreateEndTime = performance.now();

    this.$data.currentTrie = trie;

    this.$data.devLog.push(`Trie construction took: ${trieCreateEndTime - trieCreateStartTime} milliseconds`);

    // console.log(data);
    this.$data.dataLoaded = true;
    //this.$data.searchTerm = 'arebawebawbbaw';
  }
}
</script>

<style>
#app {
  font-family: 'Avenir', Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
