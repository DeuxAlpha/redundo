<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Request from {{ user.Name }}</v-card-title>
    <v-card-text class="maintain-white-space">{{ user.RequestMessage }}</v-card-text>
    <v-card-actions>
      <v-btn color="error" :loading="denyLoading" @click="onDenyClicked">Deny</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="secondary" @click="onCloseClicked">Ignore</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="success" :loading="acceptLoading" @click="onAcceptClicked">Accept</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import UpdateGroupUserModel from "../../models/setter/group/UpdateGroupUserModel";

  @Component({})

  export default class UserConfirmation extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    @Prop(Object) readonly user: GroupUserViewModel;
    denyLoading: boolean = false;
    acceptLoading: boolean = false;

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('deny')
    onInvitationDenied() {
    }

    @Emit('accept')
    onInvitationAccepted() {
    }

    async onDenyClicked() {
      this.denyLoading = true;
      await this.$axios.delete(`groups/${ this.group.Id }/users/${ this.user.Id }`)
        .then(() => this.onInvitationDenied())
        .catch(error => console.dir(error))
        .finally(() => this.denyLoading = false);
    }

    async onAcceptClicked() {
      this.acceptLoading = true;
      const updateGroupUser = new UpdateGroupUserModel();
      updateGroupUser.UserId = this.user.Id;
      updateGroupUser.GroupId = this.group.Id;
      updateGroupUser.ManagerAccepted = true;
      await this.$axios.put(`groups/${ updateGroupUser.GroupId }/users/${ updateGroupUser.UserId }`, updateGroupUser)
        .then(() => this.onInvitationAccepted())
        .catch(error => console.dir(error))
        .finally(() => this.acceptLoading = false);
    }
  }
</script>

<style scoped lang="scss">

</style>
