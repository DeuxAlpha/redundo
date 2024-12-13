<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onFormSubmitted">
      <v-card-title class="headline grey lighten-2">Delete your account</v-card-title>
      <v-card-text>
        <v-alert v-for="(error, index) of errors" :key="index" :type="error.AlertType" v-model="error.Show" transition="scale-transition">
          {{ error.Message }}
        </v-alert>
        <p class="body-2 mb-2">
          Are you absolutely sure that you wish to delete your account?<br/>
          This act is irreversible!
        </p>
        <p class="mb-2" v-if="ownedGroups.length > 0">
          Please decide what to do with the groups you own.
        </p>
        <v-list two-line dense>
          <v-list-tile class="owned-group" v-for="(group, index) of groups" :key="group.Id" v-if="group">
            <v-list-tile-content>
              <v-list-tile-title>{{ group.Name }}</v-list-tile-title>
            </v-list-tile-content>
            <v-list-tile-action class="owned-group-action">
              <v-list-tile-action-text>
                <v-select v-if="getOtherUsers(group).length > 0" dense :items="getOtherUsers(group)" item-value="Id"
                          item-text="Name" label="New owner" :rules="OwnerRules" v-model="groupActions[index]"></v-select>
                <template v-else>
                  Group will be deleted
                </template>
              </v-list-tile-action-text>
            </v-list-tile-action>
          </v-list-tile>
        </v-list>
        <v-text-field :rules="PasswordRules" v-model="userDelete.Password" label="Password"
                      type="password"></v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-btn color="primary" @click="onCloseClicked">Cancel</v-btn>
        <v-spacer></v-spacer>
        <v-btn color="error" outline type="submit" :disabled="!valid">Submit</v-btn>
      </v-card-actions>
      <v-slide-y-transition>
        <v-card-actions v-show="loading">
          <v-flex>
            <p class="subheading text-xs-center">{{ loadingAction }}</p>
            <v-progress-linear v-model="LoadingStatus"
                               :active="loading"></v-progress-linear>
          </v-flex>
        </v-card-actions>
      </v-slide-y-transition>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import UserDeleteModel from "../../models/setter/user/UserDeleteModel";
  import { Getter } from 'vuex-class';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";
  import GroupViewModel from '../../models/getter/group/GroupViewModel';
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import UpdateGroupModel from "../../models/setter/group/UpdateGroupModel";
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";

  @Component({
    async mounted() {
      this.userDelete.Id = this.User.Id;
      await this.getGroupDetails();
    }
  })

  export default class DeleteUser extends Vue {
    @Prop(Array) readonly ownedGroups: Array<GroupListItemModel>;
    groups: Array<GroupViewModel> = [];
    groupActions: Array<number> = [];
    valid: boolean = false;
    userDelete: UserDeleteModel = new UserDeleteModel();
    loading: boolean = false;
    loadingProgress: number = 0;
    loadingAction: string = '';
    @Getter('UserStore/user') User: UserAccountModel;
    errors: Array<AlertDisplay> = [];

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('submit')
    onDeleteSubmitted() {
    }

    async getGroupDetails() {
      for (let group of this.ownedGroups)
        await this.$axios.get(`groups/${ group.Id }`)
          .then(response => {
            this.groups.push(new GroupViewModel(response.data));
            this.groupActions.push(null);
          })
          .catch(error => console.dir(error));
    }

    async onFormSubmitted() {
      this.loading = true;
      for (let i = 0; i < this.groupActions.length; i++) {
        const groupAction = this.groupActions[i];
        const group = this.groups[i];
        if (groupAction) {
          const groupUpdate = new UpdateGroupModel();
          groupUpdate.Id = group.Id;
          groupUpdate.OwnerId = groupAction;
          this.loadingAction = `Updating owner for ${group.Name}.`;
          await this.$axios.put(`groups/${group.Id}`, groupUpdate)
            .then(() => {
              this.loadingProgress++;
              this.groups.splice(this.groups.indexOf(group), 1);
            })
            .catch(error => console.dir(error));
        } else {
          this.loadingAction = `Deleting group ${group.Name}.`;
          await this.$axios.delete(`groups/${group.Id}`)
            .then(() => {
              this.loadingProgress++;
              this.groups.splice(this.groups.indexOf(group), 1);
            })
            .catch(error => console.dir(error));
        }
      }
      this.loadingAction = 'Deleting your account.';
      await this.$axios.delete(`users/${ this.User.Id }`, {data: this.userDelete})
        .then(() => {
          this.loadingProgress++;
          this.onDeleteSubmitted()
        })
        .catch(error => {
          if (error.response.status === 403)
            this.errors.push(new AlertDisplay(AlertType.Error, 'You password was invalid.'));
          else
            console.dir(error)
        })
        .finally(() => this.loading = false);
    }

    get PasswordRules(): Array<any> {
      return [
        v => !!v || 'Password is required.'
      ]
    }

    get OwnerRules(): Array<any> {
      return [
        v => !!v || 'New owner is required.',
        v => v >= 0 || 'Invalid owner selected'
      ]
    }

    get LoadingStatus(): number {
      return this.loadingProgress / (this.groups.length + 1) * 100;
    }

    getOtherUsers(group: GroupViewModel): Array<GroupUserViewModel> {
      return group.Users.filter(u => u.Id !== this.User.Id && u.IsAcceptedByManager && u.IsAcceptedByUser);
    }
  }
</script>

<style scoped lang="scss">
  .owned-group {
    background: #f1f1f1;
  }

  .owned-group-action {
    max-width: 25vw;
  }
</style>
