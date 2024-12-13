<template>
  <v-container>
    <v-card>
      <v-layout row wrap>
        <template v-if="Token">
          <v-flex xs12>
            <v-btn block to="/dashboard" color="success">Dashboard</v-btn>
          </v-flex>
        </template>
        <template v-else>
          <v-flex px-2 xs12 md6>
            <v-btn block to="/login" color="primary">Login</v-btn>
          </v-flex>
          <v-flex px-2 xs12 md6>
            <v-btn block to="/register" color="primary">Register</v-btn>
          </v-flex>
        </template>
      </v-layout>
    </v-card>
  </v-container>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import UserAccountModel from "../models/setter/user/UserAccountModel";
  import { Getter } from 'vuex-class';

  @Component({})

  export default class Index extends Vue {
    @Getter('UserStore/jwtToken') Token: string;
  }
</script>

<style scoped lang="scss">
</style>
