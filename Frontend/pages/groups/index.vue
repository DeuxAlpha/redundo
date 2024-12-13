<template>
  <v-layout row>
    <v-dialog v-model="showNewGroupDialog" persistent max-width="600px">
      <NewGroup @close="showNewGroupDialog = false" @create="onNewGroupCreated"></NewGroup>
    </v-dialog>
    <v-dialog v-model="showFindGroupDialog" persistent max-width="600px">
      <FindGroup @close="showFindGroupDialog = false" @join="onNewGroupJoined"></FindGroup>
    </v-dialog>
    <v-dialog v-model="showInvitationDialog" max-width="600px">
      <InvitationView @close="showInvitationDialog = false" @delete="onInvitationDeleted" @accept="onInvitationAccepted"
                      :group="invitationGroup"></InvitationView>
    </v-dialog>
    <v-dialog v-model="showRequestDialog" max-width="600px">
      <RequestView @close="showRequestDialog = false" @delete="onRequestDeleted" :group="requestGroup"></RequestView>
    </v-dialog>
    <v-flex xs12 md6 offset-md3>
      <v-card>
        <v-layout row justify-space-between class="pa-2">
          <v-btn color="success" @click="showNewGroupDialog = true">New group</v-btn>
          <v-btn color="primary" @click="showFindGroupDialog = true">Find group</v-btn>
        </v-layout>
        <v-divider v-if="groups.length > 0"></v-divider>
        <v-list>
          <template v-for="(group, index) of groups">
            <v-list-tile nuxt :to="getGroupLink(group)" :key="index">
              <v-list-tile-content>
                <v-list-tile-title>{{ group.Name }}</v-list-tile-title>
                <v-list-tile-sub-title v-if="!group.IsAcceptedByManager">Activation pending</v-list-tile-sub-title>
                <v-list-tile-sub-title v-else-if="!group.IsAcceptedByUser">Invitation received</v-list-tile-sub-title>
                <v-list-tile-sub-title v-else-if="group.IsOwner">You own this group</v-list-tile-sub-title>
                <v-list-tile-sub-title v-else-if="group.IsManager">You manage this group</v-list-tile-sub-title>
              </v-list-tile-content>
              <v-list-tile-action v-if="!group.IsAcceptedByUser">
                <v-btn icon @click="onInvitationInfoClicked(group)">
                  <v-icon color="primary">info</v-icon>
                </v-btn>
              </v-list-tile-action>
              <v-list-tile-action v-else-if="!group.IsAcceptedByManager">
                <v-btn small flat color="primary" @click="onRequestInfoClicked(group)">
                  View request
                </v-btn>
              </v-list-tile-action>
            </v-list-tile>
            <v-divider></v-divider>
          </template>
        </v-list>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import { Action, Getter } from 'vuex-class';
  import SnackMessage from "../../models/app/SnackMessage";
  import InvitationView from '../../components/group/invitation-view.vue';
  import NewGroup from '../../components/group/new-group.vue';
  import FindGroup from '../../components/group/find-group.vue';
  import RequestView from '../../components/group/request-view.vue';

  @Component({
    components: {RequestView, FindGroup, InvitationView, NewGroup},
    layout: 'backend',
    middleware: 'auth',
    async asyncData(context) {
      let groups: Array<GroupListItemModel> = [];
      let showNewGroupDialog: boolean = context.query.create;
      let showFindGroupDialog: boolean = context.query.join;
      await context.$axios.get(`groups?userId=${ context.store.getters['UserStore/user'].Id }`)
        .then(response => {
          for (let group of response.data.groups) {
            groups.push(new GroupListItemModel(group));
          }
        })
        .catch(error => console.dir(error));
      return {
        groups,
        showNewGroupDialog,
        showFindGroupDialog
      }
    }
  })

  export default class GroupsPage extends Vue {
    groups: Array<GroupListItemModel>;
    invitationGroup: GroupListItemModel = new GroupListItemModel();
    requestGroup: GroupListItemModel = new GroupListItemModel();
    @Getter('UserStore/user') User: UserAccountModel;
    @Action('SnackStore/addSnack') AddSnack;
    showInvitationDialog: boolean = false;
    showNewGroupDialog: boolean = false;
    showFindGroupDialog: boolean = false;
    showRequestDialog: boolean = false;

    getGroupLink(group: GroupListItemModel) {
      if (!group.IsAcceptedByUser || !group.IsAcceptedByManager) return;
      return `/groups/${ group.Id }`;
    }

    onInvitationInfoClicked(group: GroupListItemModel) {
      this.invitationGroup = group;
      this.showInvitationDialog = true;
    }

    onRequestInfoClicked(group: GroupListItemModel) {
      this.requestGroup = group;
      this.showRequestDialog = true;
    }

    onInvitationDeleted(groupId: number) {
      this.AddSnack(new SnackMessage(`Deleted invitation from '${ this.invitationGroup.Name }'`, 'info', 6000));
      this.closeInvitationDialog();
      this.groups.splice(this.groups.indexOf(this.getGroup(groupId)), 1);
    }

    onInvitationAccepted(groupId: number) {
      this.AddSnack(new SnackMessage(`Accepted invitation from ${ this.invitationGroup.Name }`, 'success', 6000));
      this.closeInvitationDialog();
      this.getGroup(groupId).IsAcceptedByUser = true;
    }

    onRequestDeleted(groupId: number) {
      this.AddSnack(new SnackMessage(`Deleted request to '${ this.requestGroup.Name }'`, 'info', 6000));
      this.closeRequestDialog();
      this.groups.splice(this.groups.indexOf(this.getGroup(groupId)), 1);
    }

    getGroup(id: number): GroupListItemModel {
      return this.groups.find(g => g.Id === id);
    }

    onNewGroupCreated(group: GroupListItemModel) {
      this.groups.push(group);
      this.showNewGroupDialog = false;
    }

    onNewGroupJoined(group: GroupListItemModel) {
      this.groups.push(group);
      this.showFindGroupDialog = false;
    }

    closeInvitationDialog() {
      this.invitationGroup = new GroupListItemModel();
      this.showInvitationDialog = false;
    }

    closeRequestDialog() {
      this.requestGroup = new GroupListItemModel();
      this.showRequestDialog = false;
    }
  }
</script>

<style scoped lang="scss">
</style>
