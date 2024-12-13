<template>
  <v-card>
    <v-dialog v-model="showNewOwnerDialog" max-width="600px">
      <NewOwner :new-owner="user" :group="group" @close="showNewOwnerDialog = false" @update="onOwnerUpdated"></NewOwner>
    </v-dialog>
    <v-dialog v-model="showDeleteUserDialog" max-width="600px">
      <DeleteGroupUser :user="user" :group="group" @close="showDeleteUserDialog = false" @delete="onUserDeleted"></DeleteGroupUser>
    </v-dialog>
    <v-card-title class="headline grey lighten-2">{{ user.Name }}</v-card-title>
    <v-card-text>
      <v-select :items="roles" item-value="Id" item-text="Value" label="Role" v-model="userRole" v-if="groupUser.IsOwner"></v-select>
      <v-btn color="primary" @click="showNewOwnerDialog = true" block v-if="groupUser.IsOwner">Promote to owner</v-btn>
      <v-btn color="error" @click="showDeleteUserDialog = true" block v-if="groupUser.IsOwner || groupUser.IsManager && !user.IsManager">Delete user</v-btn>
    </v-card-text>
    <v-card-actions>
      <v-btn color="error" @click="onCloseClicked">Close</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="success" v-if="groupUser.IsOwner" :loading="loading" @click="onUpdateClicked" :disabled="!ValuesChanged">Update</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue, Watch } from 'vue-property-decorator';
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import UserGroupRoleModel from "../../models/app/UserGroupRoleModel";
  import NewOwner from './new-owner.vue';
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import UpdateGroupUserModel from "../../models/setter/group/UpdateGroupUserModel";
  import DeleteGroupUser from './delete-group-user.vue';

	@Component({
    components: {DeleteGroupUser, NewOwner},
    mounted(): void {
      this.roles.push(new UserGroupRoleModel(1, 'User'));
      this.roles.push(new UserGroupRoleModel(2, 'Manager'));
    }
  })

	export default class GroupUserView extends Vue {
	  @Prop(Object) readonly user: GroupUserViewModel;
	  @Prop(Object) readonly groupUser: GroupUserViewModel;
	  @Prop(Object) readonly group: GroupViewModel;
	  loading: boolean = false;
	  roles: Array<UserGroupRoleModel> = [];
	  userRole: number = -1;
	  showNewOwnerDialog: boolean = false;
	  showDeleteUserDialog: boolean = false;

	  @Watch('user', {deep: true})
    onUserChanged() {
	    this.userRole = this.user.IsManager ? 2 : 1;
    }

	  @Emit('close')
    onCloseClicked() {}

    @Emit('update')
    onUserUpdated(): GroupUserViewModel {
	    return this.user;
    }

    @Emit('delete')
    onUserDeleted(): GroupUserViewModel {
	    return this.user;
    }

    @Emit('owner-update')
    onOwnerUpdated(): GroupUserViewModel {
	    this.showNewOwnerDialog = false;
	    return this.user;
    }

    async onUpdateClicked() {
	    this.loading = true;
      const updateUser: UpdateGroupUserModel = new UpdateGroupUserModel();
      updateUser.GroupId = this.group.Id;
      updateUser.IsManager = this.userRole === 2;
      updateUser.UserId = this.user.Id;
      await this.$axios.put(`groups/${this.group.Id}/users/${this.user.Id}`, updateUser)
        .then(() => {
          this.user.IsManager = updateUser.IsManager;
          this.onUserUpdated();
        })
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }

    get ValuesChanged(): boolean {
	    if (this.user.IsManager) {
	      return this.userRole !== 2;
      } else {
	      return this.userRole !== 1;
      }
    }
	}
</script>

<style scoped lang="scss">

</style>
