<template>
  <v-app>
    <v-content>
      <nuxt/>
    </v-content>
  </v-app>
</template>

<script lang="ts">
  import { Vue, Component } from 'vue-property-decorator';

  @Component({})

  export default class DefaultLayout extends Vue {

  }
</script>

<style scoped lang="scss"></style>
