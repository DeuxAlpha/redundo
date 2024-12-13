<template>
  <v-layout row>
    <v-dialog v-model="showDeleteUserForm" max-width="600px">
      <delete-user @close="showDeleteUserForm = false" @submit="onDeleteSubmitted" :owned-groups="OwnedGroups"></delete-user>
    </v-dialog>
    <v-dialog v-model="showUpdatePasswordForm" max-width="600px">
      <user-password @close="showUpdatePasswordForm = false" @submit="showUpdatePasswordForm = false"></user-password>
    </v-dialog>
    <v-flex xs12 md6 offset-md3>
      <v-list>
        <v-list-tile @click="showUpdatePasswordForm = true">
          <v-list-tile-content>
            <v-list-tile-title>Update password</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider></v-divider>
        <v-list-tile @click="showDeleteUserForm = true">
          <v-list-tile-content>
            <v-list-tile-title>Delete Account</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
        <v-divider></v-divider>
        <v-list-tile @click="onLogoutClicked">
          <v-list-tile-content>
            <v-list-tile-title>Logout</v-list-tile-title>
          </v-list-tile-content>
        </v-list-tile>
      </v-list>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import {Component, Vue} from 'vue-property-decorator';
  import { Action } from 'vuex-class';
  import Router from 'vue-router';
  import DeleteUser from '../../components/user/delete-user.vue';
  import UserPassword from '../../components/user/user-password.vue';
  import GroupListItemModel from '../../models/getter/group/GroupListItemModel';

  @Component({
    components: {DeleteUser, UserPassword},
    layout: 'backend',
    middleware: 'auth',
    async asyncData(context) {
      const userGroups: Array<GroupListItemModel> = [];
      await context.$axios.get(`groups?userId=${context.store.getters['UserStore/user'].Id}`)
        .then(response => response.data.groups.map(g => userGroups.push(new GroupListItemModel(g))))
        .catch(error => console.dir(error));
      return {
        userGroups
      }
    }
  })

  export default class UserPage extends Vue {
    @Action('UserStore/logout') Logout: Function;
    $router: Router;
    showDeleteUserForm: boolean = false;
    showUpdatePasswordForm: boolean = false;
    userGroups: Array<GroupListItemModel>;

    get OwnedGroups(): Array<GroupListItemModel> {
      return this.userGroups.filter(ug => ug.IsOwner);
    }

    onLogoutClicked() {
      this.Logout();
      this.$router.push('/login?exit=1')
    }

    onDeleteSubmitted() {
      this.Logout();
      this.$router.push('/login?delete=1');
    }
  }
</script>

<style scoped lang="scss">

</style>
