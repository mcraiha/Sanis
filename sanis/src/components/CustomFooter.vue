<template>
    <footer>
        <div>
            <small><strong>Sanis</strong> - Ilmainen, avoin, mainosvapaa ja yksityisyyttä kunnioittava sanakirja</small>
        </div>
        <div>
            <a href="tietoja.html">Tietoja</a>
        </div>
        <div>
            <small>{{ getBuildTime() }} {{ getGitCommit() }}</small>
        </div>
    </footer>
</template>

<script lang="ts">
import { Component, Prop, Vue } from 'vue-property-decorator';
import buildinfo from '../buildinfo.json';

@Component
export default class CustomFooter extends Vue {
    private getBuildTime(): string {
        return buildinfo.buildTime;
    }

    private getGitCommit(): string {
        return buildinfo.gitCommit;
    }
}
</script>