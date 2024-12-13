<template>
  <v-layout row>
    <v-dialog v-model="showNewUserForm" persistent max-width="600px">
      <new-group-user :group="group" @close="showNewUserForm = false" @submit="onUserSubmitted"></new-group-user>
    </v-dialog>
    <v-dialog v-model="showUserInvitation" max-width="600px">
      <user-confirmation :user="selectedUser" :group="group" @accept="onInvitationAccepted" @deny="onInvitationDenied"
                         @close="showUserInvitation = false"></user-confirmation>
    </v-dialog>
    <v-dialog v-model="showDeleteGroupForm" max-width="600px">
      <delete-group :group="group" @close="showDeleteGroupForm = false" @delete="onGroupDeleted"></delete-group>
    </v-dialog>
    <v-dialog v-model="showGroupUserDialog" max-width="600px">
      <GroupUserView :user="selectedUser"
                     :group-user="groupUser"
                     :group="group"
                     @close="showGroupUserDialog = false"
                     @update="onUserUpdated"
                     @delete="onUserDeleted"
                     @owner-update="onOwnerUpdated"></GroupUserView>
    </v-dialog>
    <v-dialog v-model="showAddItemsDialog" max-width="600px">
      <AddItemsView :group="group" @close="showAddItemsDialog = false"
                    @add="onItemAdded"></AddItemsView>
    </v-dialog>
    <v-dialog v-model="showRemoveItemsDialog" scrollable max-width="600px">
      <RemoveItemsView :group="group" @close="showRemoveItemsDialog = false"></RemoveItemsView>
    </v-dialog>
    <v-dialog v-model="showManageItemsDialog" scrollable max-width="600px">
      <ManageItemsView :group="group" @close="showManageItemsDialog = false"></ManageItemsView>
    </v-dialog>
    <v-dialog v-model="showLeaveGroupDialog" max-width="600px">
      <LeaveGroupView :group="group" @close="showLeaveGroupDialog = false"></LeaveGroupView>
    </v-dialog>
    <v-flex xs12 md6 offset-md3>
      <v-card>
        <v-layout class="pa-3">
          <template v-if="group">
            <v-layout row justify-space-between align-center>
              <h6 class="title">
                {{ group.Name }}
              </h6>
              <v-btn v-if="IsGroupManager" color="success" @click="showNewUserForm = true">Add user</v-btn>
            </v-layout>
          </template>
          <template v-else>
            <h6 class="title">
              No group found
            </h6>
          </template>
        </v-layout>
        <template v-if="group">
          <v-list>
            <v-list-tile @click="showManageItemsDialog = true">
              <v-list-tile-action>
                <v-icon>create</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Manage items</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <v-divider></v-divider>
            <v-list-tile @click="showAddItemsDialog = true">
              <v-list-tile-action>
                <v-icon>add</v-icon>
              </v-list-tile-action>
              <v-list-tile-content>
                <v-list-tile-title>Add items</v-list-tile-title>
              </v-list-tile-content>
            </v-list-tile>
            <template v-if="IsGroupManager">
              <v-divider></v-divider>
              <v-list-tile @click="showRemoveItemsDialog = true">
                <v-list-tile-action>
                  <v-icon>remove</v-icon>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>Remove items</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <v-divider></v-divider>
            <v-list-group prepend-icon="group">
              <template v-slot:activator>
                <v-list-tile>
                  <v-list-tile-content>
                    <v-list-tile-title>View users</v-list-tile-title>
                  </v-list-tile-content>
                </v-list-tile>
              </template>
              <v-list-tile v-on=" isClickable(user) ? { click: () => onUserClicked(user) } : {}"
                           v-for="user of group.Users" :key="user.Id">
                <v-list-tile-action>
                  <v-icon>person</v-icon>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>{{ user.Name }}</v-list-tile-title>
                  <v-list-tile-sub-title v-if="user.IsOwner">Owner</v-list-tile-sub-title>
                  <v-list-tile-sub-title v-else-if="user.IsManager">Manager</v-list-tile-sub-title>
                </v-list-tile-content>
                <v-list-tile-action v-if="!user.IsAcceptedByManager || !user.IsAcceptedByUser">
                  <v-tooltip bottom>
                    <template v-slot:activator="{ on }">
                      <v-icon :color="!user.IsAcceptedByManager ? 'error' : 'warning'" v-on="on">warning</v-icon>
                    </template>
                    <span>{{ getAcceptanceWarnings(user) }}</span>
                  </v-tooltip>
                </v-list-tile-action>
              </v-list-tile>
            </v-list-group>
            <v-divider></v-divider>
            <template v-if="!IsGroupOwner">
              <v-list-tile @click="showLeaveGroupDialog = true">
                <v-list-tile-action>
                  <v-icon>block</v-icon>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>Leave group</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
            <template v-if="IsGroupOwner">
              <v-divider></v-divider>
              <v-list-tile @click="showDeleteGroupForm = true">
                <v-list-tile-action>
                  <v-icon>delete_forever</v-icon>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>Delete group</v-list-tile-title>
                </v-list-tile-content>
              </v-list-tile>
            </template>
          </v-list>
        </template>
      </v-card>
    </v-flex>
  </v-layout>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import GroupViewModel from "../../../models/getter/group/GroupViewModel";
  import GroupUserViewModel from "../../../models/getter/group/GroupUserViewModel";
  import { Action, Getter } from 'vuex-class';
  import UserAccountModel from "../../../models/setter/user/UserAccountModel";
  import NewGroupUser from '../../../components/group/new-group-user.vue';
  import UserConfirmation from '../../../components/group/user-confirmation.vue';
  import DeleteGroup from '../../../components/group/delete-group.vue'
  import GroupUserView from '../../../components/group/group-user-view.vue';
  import AddItemsView from '../../../components/item/add-items-view.vue';
  import GroupItemViewModel from "../../../models/getter/group/GroupItemViewModel";
  import RemoveItemsView from '../../../components/item/remove-items-view.vue';
  import ManageItemsView from '../../../components/item/manage-items-view.vue';
  import LeaveGroupView from '../../../components/group/leave-group-view.vue';

  @Component({
    components: {
      LeaveGroupView,
      ManageItemsView,
      RemoveItemsView, AddItemsView, GroupUserView, UserConfirmation, NewGroupUser, DeleteGroup
    },
    layout: 'backend',
    middleware: 'auth',
    async asyncData(context) {
      let group: GroupViewModel;
      await context.$axios.get(`groups/${ context.params.id }`)
        .then(response => {
          group = new GroupViewModel(response.data);
        })
        .catch(error => console.dir(error));
      return {
        group
      }
    }
  })

  export default class GroupPage extends Vue {
    group: GroupViewModel;
    groupUser: GroupUserViewModel = new GroupUserViewModel();
    selectedUser: GroupUserViewModel = new GroupUserViewModel();
    @Getter('UserStore/user') User: UserAccountModel;
    @Action('SnackStore/addSnack') AddSnack;
    showNewUserForm: boolean = false;
    showUserInvitation: boolean = false;
    showDeleteGroupForm: boolean = false;
    showGroupUserDialog: boolean = false;
    showAddItemsDialog: boolean = false;
    showRemoveItemsDialog: boolean = false;
    showManageItemsDialog: boolean = false;
    showLeaveGroupDialog: boolean = false;

    mounted() {
      this.groupUser = this.group.Users.find(u => u.Id === this.User.Id);
    }

    get IsGroupManager(): boolean {
      return this.groupUser.IsManager;
    }

    get IsGroupOwner(): boolean {
      return this.group.Users.find(u => u.Id === this.User.Id).IsOwner;
    }

    isClickable(user: GroupUserViewModel): boolean {
      if (!this.IsGroupManager) return false; // Only managers can click on user
      const groupUser = this.group.Users.find(u => u.Id === user.Id);
      if (groupUser.Id === this.User.Id) return false; // Manager should not need/be allowed to modify self
      if (groupUser.IsOwner) return false; // Nobody should be allowed to modify owner
      if (groupUser.IsManager && !this.IsGroupOwner) return false; // Only owner can modify managers
      if (!groupUser.IsAcceptedByUser) return false; // Should not be able to modify user who has not accepted invitation
      return true;
    }

    getAcceptanceWarnings(user: GroupUserViewModel) {
      if (!user.IsAcceptedByUser && !user.IsAcceptedByManager) return 'This user should not be here';
      if (!user.IsAcceptedByUser) return 'User confirmation is pending';
      if (!user.IsAcceptedByManager) return 'Manager confirmation is pending'
    }

    get NewUserRules(): Array<any> {
      return [
        v => !!v || 'User is required.'
      ]
    }

    onUserSubmitted(user: GroupUserViewModel) {
      this.group.Users.push(user);
      this.showNewUserForm = false;
    }

    onUserClicked(user: GroupUserViewModel) {
      this.selectedUser = this.group.Users.find(u => u.Id === user.Id);
      if (!this.selectedUser.IsAcceptedByManager)
        this.showUserInvitation = true;
      else
        this.showGroupUserDialog = true;
    }

    onInvitationDenied() {
      this.group.Users.splice(this.group.Users.indexOf(this.selectedUser), 1);
      this.showUserInvitation = false;
    }

    onInvitationAccepted() {
      this.selectedUser.IsAcceptedByManager = true;
      this.showUserInvitation = false;
    }

    onGroupDeleted() {
      this.$router.push('/groups');
    }

    onUserUpdated(user: GroupUserViewModel) {
      this.selectedUser = user;
      this.group.Users[this.group.Users.indexOf(this.selectedUser)] = this.selectedUser;
      this.showGroupUserDialog = false;
    }

    onUserDeleted(user: GroupUserViewModel) {
      this.selectedUser = user;
      this.group.Users.splice(this.group.Users.indexOf(this.selectedUser), 1);
      this.showGroupUserDialog = false;
    }

    onOwnerUpdated(newOwner: GroupUserViewModel) {
      this.showGroupUserDialog = false;
      this.selectedUser = newOwner;
      this.group.Users.find(u => u.IsOwner).IsOwner = false;
      this.group.Users[this.group.Users.indexOf(this.selectedUser)].IsOwner = true;
      this.group.Users[this.group.Users.indexOf(this.selectedUser)].IsManager = true;
    }

    onItemAdded(item: GroupItemViewModel) {
      this.group.Items.push(item);
    }
  }
</script>

<style scoped lang="scss">

</style>
