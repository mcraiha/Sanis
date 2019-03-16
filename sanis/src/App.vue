<template>
  <div id="app">
    <TextInput v-bind:searchTerm.sync="searchTerm"/>
    <p>{{ searchTerm }}</p>
    <LanguagePairSelect />
  </div>
</template>

<script lang="ts">
import { Component, Vue } from 'vue-property-decorator';
import TextInput from './components/TextInput.vue';
import LanguagePairSelect from './components/LanguagePairSelect.vue';

@Component({
  components: {
    TextInput,
    LanguagePairSelect,
  },
  data: function () {
    return { searchTerm: 'abs' }
  },
})
export default class App extends Vue {}
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
