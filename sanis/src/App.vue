<template>
  <div id="app">
    <DevLog v-bind:devLogs="devLog" />
    <TextInput v-bind:searchTerm.sync="searchTerm"/>
    <p>{{ searchTerm }}</p>
    <LanguagePairSelect />
    <ShowResults v-bind:exactSearchTerm="searchTerm" v-bind:exactMatch="getExactMatch(searchTerm)" v-bind:closestMatches="getPartialMatches(searchTerm, 5)" v-bind:dictionaryDefinition="currentDictionaryDefinition" />
    <CustomFooter />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';

import { Trie } from 'prefix-trie-ts';

const ZstdCodec = require('zstd-codec').ZstdCodec;

import { IDictionaryEntry } from './interfaces/IDictionaryEntry';

import TextInput from './components/TextInput.vue';
import LanguagePairSelect from './components/LanguagePairSelect.vue';
import ShowResults from './components/ShowResults.vue';
import DevLog from './components/DevLog.vue';
import CustomFooter from './components/CustomFooter.vue';
import { IDictionaryDefinition } from './interfaces/IDictionaryDefinition';
import { LanguageEntries } from './definitions/LanguageEntries'

@Component({
  components: {
    TextInput,
    LanguagePairSelect,
    ShowResults,
    DevLog,
    CustomFooter
  },
  data: function() {
    return {
      searchTerm: '' as string,
      dataLoaded: false as boolean,
      dictionary: null as any,
      currentTrie: null as any,
      currentDictionaryDefinition: LanguageEntries.entries[0] as IDictionaryDefinition,

      // Development log
      devLogEnabled: false as boolean,
      devLog: ['Dev log start'] as string[],
      }
  },
  watch: {
    searchTerm: function (newSearchTerm, oldSearchTerm) {
      // Update URL here, TODO: add language
      if (newSearchTerm && newSearchTerm.length > 0)
      {
        history.replaceState({}, `Hakusana: ${newSearchTerm}`, `index.html?search=${newSearchTerm}`);
      }
    },
  },
  methods: {
    getExactMatch(searchKeyword: string): IDictionaryEntry {
      if (this.$data.dictionary && this.$data.dictionary.hasOwnProperty(searchKeyword))
      {
        return { word: searchKeyword, translations: this.$data.dictionary[searchKeyword].translations };
      }

      return { word: '', translations: [] };
    },

    getPartialMatches(searchKeyword: string, maxAmount: number): IDictionaryEntry[] {

      const returnArray: IDictionaryEntry[] = [];

      // Do not check anything if search keyword is less than 2 chars
      if (searchKeyword.length < 2)
      {
        return returnArray;
      }

      // Do not check anything if trie isn't inited
      if (!this.$data.currentTrie)
      {
        return returnArray;
      }

      // Take one more in case the results also contain exact match
      const seekAmount: number = maxAmount + 1;
      const sliced = this.$data.currentTrie.getPrefix(searchKeyword).slice(0, seekAmount);
      const gotEnough: boolean = (seekAmount === sliced.length);

      const possibleIndexOfMatch: number = sliced.indexOf(searchKeyword);
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
  public async mounted()
  {
    const jsonHandleStartTime = performance.now();

    const response = await fetch('dictionaries/1-2.zst');
    const asArrayBuffer = await response.arrayBuffer();
    const asUint8Array = new Uint8Array(asArrayBuffer);

    ZstdCodec.run((zstd: any) => {
      const simple = new zstd.Simple();
      const jsonBytes = simple.decompress(asUint8Array);
      const jsonText = new TextDecoder('utf-8').decode(jsonBytes);
      
      const data = JSON.parse(jsonText);

      this.$data.dictionary = data;

      const jsonHandleEndTime = performance.now();

      this.$data.devLog.push(`json handling took: ${jsonHandleEndTime - jsonHandleStartTime} milliseconds`);


      this.$data.devLog.push(`dictionary has ${Object.keys(data).length} entries`);


      const trieCreateStartTime = performance.now();

      const trie = new Trie(Object.keys(data));

      const trieCreateEndTime = performance.now();

      this.$data.currentTrie = trie;

      this.$data.devLog.push(`Trie construction took: ${trieCreateEndTime - trieCreateStartTime} milliseconds`);

      this.ParseSearchParams();

      // console.log(data);
      this.$data.dataLoaded = true;
     
    });
    
  }

  private ParseSearchParams(): void
  {
    const params = new URLSearchParams(document.location.search.substring(1));
    
    const lang = params.get("lang");
    if (lang)
    {
      // TODO: Handle language here
    }

    const search = params.get("search");
    if (search)
    {
      this.$data.searchTerm = search;
    }
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
