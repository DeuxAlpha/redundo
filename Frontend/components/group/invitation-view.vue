<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Invitation from {{ group.Name }}</v-card-title>
    <v-card-text class="maintain-white-space">{{ group.RequestMessage }}</v-card-text>
    <v-card-actions>
      <v-btn color="error" :loading="deletionLoading" @click="onDeleteInvitationClicked">Delete</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="secondary" @click="onCloseClicked">Ignore</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="success" :loading="acceptanceLoading" @click="onAcceptInvitationClicked">Accept</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue, Watch } from 'vue-property-decorator';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import { Action, Getter } from 'vuex-class';
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";
  import UpdateGroupUserModel from '../../models/setter/group/UpdateGroupUserModel';

  @Component({})

  export default class InvitationView extends Vue {
    deletionLoading: boolean = false;
    acceptanceLoading: boolean = false;
    @Prop(Object) readonly group: GroupListItemModel;
    @Getter('UserStore/user') User: UserAccountModel;

    @Watch('group')
    onGroupChanged() {
    }

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('delete')
    onInvitationDeleted() {
      return this.group.Id;
    }

    @Emit('accept')
    onInvitationAccepted() {
      return this.group.Id;
    }

    async onDeleteInvitationClicked() {
      this.deletionLoading = true;
      await this.$axios.delete(`groups/${ this.group.Id }/users/${ this.User.Id }`)
        .then(() => this.onInvitationDeleted())
        .catch(error => console.dir(error))
        .finally(() => this.deletionLoading = false)
    }

    async onAcceptInvitationClicked() {
      this.acceptanceLoading = true;
      const updateGroupUser = new UpdateGroupUserModel();
      updateGroupUser.UserId = this.User.Id;
      updateGroupUser.GroupId = this.group.Id;
      updateGroupUser.UserAccepted = true;
      await this.$axios.put(`groups/${ updateGroupUser.GroupId }/users/${ updateGroupUser.UserId }`, updateGroupUser)
        .then(() => this.onInvitationAccepted())
        .catch(error => console.dir(error))
        .finally(() => this.acceptanceLoading = false);
    }
  }
</script>

<style scoped lang="scss">

</style>
