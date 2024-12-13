<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Delete {{ user.Name }} from {{ group.Name }}</v-card-title>
    <v-card-text class="body-2">
      Are you sure you want to delete {{ user.Name }} from the group?<br/>
    </v-card-text>
    <v-card-actions>
      <v-btn color="primary" @click="onCloseClicked">Cancel</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="error" :loading="loading" @click="onDeleteUserClicked">Delete user</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import { Action } from 'vuex-class';
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class DeleteGroupUser extends Vue {
    @Prop(Object) readonly user: GroupUserViewModel;
    @Prop(Object) readonly group: GroupViewModel;
    loading: boolean = false;
    @Action('SnackStore/addSnack') AddSnack: Function;

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('delete')
    onUserDeleted() {}

    async onDeleteUserClicked() {
      this.loading = true;
      await this.$axios.delete(`groups/${this.group.Id}/users/${this.user.Id}`)
        .then(() => {
          this.AddSnack(new SnackMessage(`${this.user.Name} has been removed from ${this.group.Name}.`, 'info'));
          this.onUserDeleted();
        })
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }
  }
</script>

<style scoped lang="scss">

</style>
