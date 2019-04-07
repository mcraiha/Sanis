<template>
  <div id="app">
    <DevLog v-bind:devLogs="devLog" />
    <TextInput v-bind:searchTerm.sync="searchTerm"/>
    <p>{{ searchTerm }}</p>
    <LanguagePairSelect />
    <ShowResults v-bind:exactSearchTerm="searchTerm"  v-bind:exactMatch="getExactMatch(searchTerm)" v-bind:closestMatches="closestMatches" />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
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
      closestMatches: ['ab', 'bb', 'bc'] as string[], 
      dataLoaded: false as boolean, 
      dictionary: null as any,

      // Development log
      devLogEnabled: false as boolean,
      devLog: ['Dev log start'] as string[], 
      }
  },
  methods: {
    getExactMatch(searchKeyword: string): string {
      if (this.$data.dictionary.hasOwnProperty(searchKeyword))
      {
        return this.$data.dictionary[searchKeyword];
      }

      return '';
    }
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
