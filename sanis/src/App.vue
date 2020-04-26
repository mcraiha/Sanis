<template v-if="dataLoaded === false">
    <p>Ladataan...</p>
</template>
<template v-else>
  <div id="app">
    <DevLog v-bind:devLogs="devLog" />
    <TextInput v-bind:searchTerm.sync="searchTerm"/>
    <p>{{ searchTerm }}</p>
    <LanguagePairSelect v-bind:pairs="getAllLanguagePairEntries()" @selected="onSelected" />
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
import { LanguageEntries } from './definitions/LanguageEntries';
import { Languages } from './definitions/LanguageEnums';

@Component({
  components: {
    TextInput,
    LanguagePairSelect,
    ShowResults,
    DevLog,
    CustomFooter,
  },
  data: function() {
    return {
      searchTerm: '' as string,
      searchLanguage: '' as string,
      dataLoaded: false as boolean,
      dictionary: null as any,
      currentTrie: null as any,
      currentDictionaryIndex: 0 as number,
      currentDictionaryDefinition: null as unknown,

      // Development log
      devLogEnabled: false as boolean,
      devLog: ['Dev log start'] as string[],
      }
  },
  watch: {
    searchTerm: function(newSearchTerm, oldSearchTerm) {
      // Update URL here, TODO: add language
      if (newSearchTerm && newSearchTerm.length > 0) {
        history.replaceState({}, `Hakusana: ${newSearchTerm}`, `index.html?lang=${this.$data.currentDictionaryDefinition.from}-${this.$data.currentDictionaryDefinition.to}&search=${newSearchTerm}`);
      }
    },
    searchLanguage: function(newSearchLanguage, oldSearchLanguage) {

    },
  },
  methods: {
    getExactMatch(searchKeyword: string): IDictionaryEntry {
      const lowerCaseToSeek : string = searchKeyword.toLowerCase();
      if (this.$data.dictionary && this.$data.dictionary.hasOwnProperty(lowerCaseToSeek)) {
        return { word: this.$data.dictionary[lowerCaseToSeek].word, translations: this.$data.dictionary[lowerCaseToSeek].translations, links: this.$data.dictionary[lowerCaseToSeek].links };
      }

      return { word: '', translations: [], links: [] };
    },

    onSelected(value) {
      console.log(value);
    },

    getAllLanguagePairEntries(): IDictionaryDefinition[] {
      return LanguageEntries.entries;
    },

    getPartialMatches(searchKeyword: string, maxAmount: number): IDictionaryEntry[] {

      const returnArray: IDictionaryEntry[] = [];

      // Do not check anything if search keyword is less than 2 chars
      if (searchKeyword.length < 2) {
        return returnArray;
      }

      // Do not check anything if trie isn't inited
      if (!this.$data.currentTrie) {
        return returnArray;
      }

      // Take one more in case the results also contain exact match
      const seekAmount: number = maxAmount + 1;
      const sliced = this.$data.currentTrie.getPrefix(searchKeyword).slice(0, seekAmount);
      const gotEnough: boolean = (seekAmount === sliced.length);

      const possibleIndexOfMatch: number = sliced.indexOf(searchKeyword);
      if (possibleIndexOfMatch > -1) {
        // Remove exact match
        sliced.splice(possibleIndexOfMatch, 1);
      }
      else if (gotEnough) {
        // No exact match, and there is one match too many, so remove last array element
        sliced.pop();
      }

      for (let i = 0; i < sliced.length; i++) {
        // Take lowercase because all of our keys are lowercase
        const partialMatchWord: string = sliced[i].toLowerCase();

        returnArray.push({ word: this.$data.dictionary[partialMatchWord].word, translations: this.$data.dictionary[partialMatchWord].translations, links: this.$data.dictionary[partialMatchWord].links});
      }

      return returnArray;
    },
  }
})

export default class App extends Vue {


  // Lifecycle hook
  public async mounted() {
    

    this.ParseSearchParams();

    await this.LoadChosenDictionary();
  }

  private async LoadChosenDictionary() {
    this.$data.currentDictionaryDefinition = LanguageEntries.entries[this.$data.currentDictionaryIndex];

    const filename: string = `${this.$data.currentDictionaryDefinition.from}-${this.$data.currentDictionaryDefinition.to}.zst`;

    let asUint8Array: Uint8Array = new Uint8Array(); // Default value is not used

    if (this.CheckIfWeHaveToDownload(this.$data.currentDictionaryDefinition)) {
      this.$data.devLog.push(`Downloading from server`);
      asUint8Array = await this.DownloadAndStoreToLocalStorage(filename);
    } else {
      this.$data.devLog.push(`Downloading from local storage`);
      asUint8Array = this.LoadFileFromLocalStorage(filename);
      this.DoUpdateIfNeeded(filename);
    }

    ZstdCodec.run((zstd: any) => {
      const zstdDecodeStartTime = performance.now();

      const simple = new zstd.Simple();
      const jsonBytes = simple.decompress(asUint8Array);

      const zstdDecodeEndTime = performance.now();

      this.$data.devLog.push(`ZSTD decode took: ${zstdDecodeEndTime - zstdDecodeStartTime} milliseconds`);

      const jsonText = new TextDecoder('utf-8').decode(jsonBytes);

      const wireData = JSON.parse(jsonText);

      const jsonHandleStartTime = performance.now();
      const data: {[k: string]: any} = {};

      for (const key in wireData) {
        // We need more info so we have to "juice up" the wireData format before we use it 
        data[key.toLowerCase()] = { word: key, translations: wireData[key].t, links: wireData[key].l };
      }

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

    });
  }

  private ParseSearchParams(): void {
    const params = new URLSearchParams(document.location.search.substring(1));

    const lang = params.get('lang');
    if (lang) {
      const splitted: string[] = lang.split('-');
      if (splitted.length === 2) {
        const firstNumber: number = parseInt(splitted[0], 10);
        const secondNumber: number = parseInt(splitted[1], 10);
        if (firstNumber in Languages && secondNumber in Languages) {
          const languageIndex = LanguageEntries.entries.findIndex((entry) => entry.from === firstNumber && entry.to === secondNumber);
          if (languageIndex > -1) {
            this.$data.currentDictionaryIndex = languageIndex;
          }
        }
      }
      
    }

    const search = params.get('search');
    if (search) {
      this.$data.searchTerm = search;
    }
  }

  private CheckIfWeHaveToDownload(dictionaryDefinition: IDictionaryDefinition): boolean {
    const finalFilename = this.GetFilenameFromDefinition(dictionaryDefinition);

    if (localStorage.getItem(finalFilename) === null) {
      return true;
    }

    return false;
  }

  private async DownloadAndStoreToLocalStorage(filename: string): Promise<Uint8Array> {
  
    // Download wanted dictionary
    const response = await fetch(`dictionaries/${filename}`);
    const asArrayBuffer = await response.arrayBuffer();
    const asUint8Array = new Uint8Array(asArrayBuffer);

    // Store downloaded dictionary, file size and timestamp to localStorage
    const base64String = btoa(asUint8Array.reduce((data, byte) => data + String.fromCharCode(byte), ''));
    localStorage.setItem(filename, base64String);

    const currentTimeStampAsString: string = Date.now().toString();
    localStorage.setItem(this.GetUpdateTimeKeyName(filename), currentTimeStampAsString);

    const lengthAsString = this.GetLengthFromResponse(response);
    localStorage.setItem(this.GetSizeKeyName(filename), lengthAsString);

    return asUint8Array;
  }

  private async CheckLengthMatchFromHeadersWithoutFullDownload(filename: string): Promise<boolean> {
    const response = await fetch(`dictionaries/${filename}`, {method: 'HEAD'});
    if (this.GetLengthFromResponse(response) === localStorage.getItem(this.GetSizeKeyName(filename))) {
      return true;
    }
    return false;
  }

  private GetLengthFromResponse(response: Response): string {
    return response.headers.get('content-length')!;
  }

  private LoadFileFromLocalStorage(filename: string): Uint8Array {
    const base64String: string = localStorage.getItem(filename)!; // We are sure that we have something in localStorage
    return Uint8Array.from(atob(base64String), c => c.charCodeAt(0));
  }

  private async DoUpdateIfNeeded(filename: string): Promise<void> {
    const finalKeyName = this.GetUpdateTimeKeyName(filename);

    const lastUpdateTimeAsUnixMilliseconds: string = localStorage.getItem(finalKeyName)!;
    const numberUnixMilliseconds: number = +lastUpdateTimeAsUnixMilliseconds;

    const compareDate: number = new Date(Date.now() - 12096e5).getTime(); // 12096e5 is 14 days in milliseconds

    if (compareDate > numberUnixMilliseconds) {
      const areSizesIdentical: boolean = await this.CheckLengthMatchFromHeadersWithoutFullDownload(filename);
      if (!areSizesIdentical) {
        // Do update since data has been updated
        this.DownloadAndStoreToLocalStorage(filename);
      } else {
        // Update timestamp since file has not changed
        const currentTimeStampAsString: string = Date.now().toString();
        localStorage.setItem(finalKeyName, currentTimeStampAsString);
      }
    }
  }


  private GetFilenameFromDefinition(dictionaryDefinition: IDictionaryDefinition): string {
    if (dictionaryDefinition === null) {
      return 'ERROR!';
    }

    return `${dictionaryDefinition.from}-${dictionaryDefinition.to}.zst`;
  }

  private GetUpdateTimeKeyName(filename: string) {
    return `${filename}_timestamp`;
  }

  private GetSizeKeyName(filename: string) {
    return `${filename}_size`;
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
