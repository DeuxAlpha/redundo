<template>
  <v-app>
    <v-toolbar app>
      <v-toolbar-items class="full-flex-row-around">
        <v-btn block color="primary" class="link" flat active-class="active-link" nuxt to="/groups">Groups</v-btn>
        <v-btn block color="primary" class="link" flat active-class="active-link" nuxt to="/dashboard">Dashboard
        </v-btn>
        <v-btn block color="primary" class="link" flat active-class="active-link" nuxt to="/menu">Menu</v-btn>
      </v-toolbar-items>
    </v-toolbar>
    <v-snackbar :color="Snack.color" bottom v-model="Snack.show" :timeout="0">{{ Snack.message }}</v-snackbar>
    <v-content>
      <nuxt/>
    </v-content>
  </v-app>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import { Getter } from 'vuex-class';
  import { ISnackState } from '../store/SnackStore';

  @Component({})

  export default class Backend extends Vue {
    @Getter('SnackStore/snack') Snack: ISnackState;
  }
</script>

<style scoped lang="scss">
  .full-flex-row-around {
    width: 100%;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
  }

  .link {
    border-bottom: 2px solid #133133;
  }

  .active-link {
    border-bottom: 4px solid #133133;
  }

  @media only screen and (min-width: 960px) {
    .full-flex-row-around {
      justify-content: flex-end;
    }
  }
</style>
