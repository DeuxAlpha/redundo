<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onFormSubmitted">
      <v-card-title class="headline grey lighten-2">Add new user</v-card-title>
      <v-card-text>
        <v-autocomplete v-model="newUser.UserId"
                        :loading="newUserAutocomplete.Loading"
                        :search-input.sync="search"
                        clearable
                        :rules="NewUserRules"
                        :items="newUserAutocomplete.Items"
                        :item-value="newUserAutocomplete.ValueProperty"
                        :item-text="newUserAutocomplete.TextProperty"
                        hide-no-data
                        label="User"></v-autocomplete>
        <v-textarea v-model="newUser.RequestMessage"
                    label="Invitation Message"
                    clearable
                    no-resize
                    rows="4"></v-textarea>
      </v-card-text>
      <v-card-actions>
        <v-btn color="error" @click="onCloseClicked">Cancel</v-btn>
        <v-spacer></v-spacer>
        <v-btn color="success" type="submit" :disabled="!valid" :loading="loading">Submit</v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue, Watch } from 'vue-property-decorator';
  import AutocompleteModel from "../../models/app/AutocompleteModel";
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import CreateGroupUserModel from "../../models/setter/group/CreateGroupUserModel";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import { Action, Getter } from 'vuex-class';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class NewGroupUser extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    valid: boolean = false;
    newUserAutocomplete: AutocompleteModel = new AutocompleteModel('username', 'id');
    newUser: CreateGroupUserModel = new CreateGroupUserModel();
    search: string = '';
    writingTimeout;
    loading: boolean = false;
    @Getter('UserStore/user') User: UserAccountModel;
    @Action('SnackStore/addSnack') AddSnack: Function;

    @Emit('close')
    onCloseClicked() {}

    @Emit('submit')
    onUserSubmitted(user: GroupUserViewModel): GroupUserViewModel {
      return user;
    }

    mounted() {
      this.setRequestMessage();
    }

    setRequestMessage() {
      this.newUser.RequestMessage = `Hello! Please join our group '${this.group.Name}'.\r\n\r\nRegards, ${this.User.Username}`;
    }

    get NewUserRules(): Array<any>{
      return [
        v => !!v || 'User is required.'
      ]
    }

    async onFormSubmitted() {
      this.newUser.GroupId = this.group.Id;
      this.loading = true;
      await this.$axios.post(`groups/${this.newUser.GroupId}/users`, this.newUser)
        .then(response => {
          const user = new GroupUserViewModel();
          user.Id  = response.data.userId;
          user.Name = this.search;
          user.IsManager = response.data.isManager;
          user.IsOwner = response.data.isOwner;
          user.IsAcceptedByManager = response.data.isAcceptedByManager;
          user.IsAcceptedByUser = response.data.isAcceptedByUser;
          this.search = '';
          this.newUser = new CreateGroupUserModel();
          this.setRequestMessage();
          this.onUserSubmitted(user);
        })
        .catch(error => {
          if (error.response.status === 400)
            this.AddSnack(new SnackMessage(error.response.data, 'error'));
          else
            console.dir(error);
        })
        .finally(() => this.loading = false);
    }

    @Watch('search')
    async onNewUserChanged() {
      if (!this.search || this.search.length <= 0) return;
      if (this.writingTimeout)
        clearTimeout(this.writingTimeout);
      if (this.newUserAutocomplete.Loading) return;
      this.writingTimeout = setTimeout(async () => {
        this.newUserAutocomplete.Loading = true;
        await this.$axios.get(`users?username=${this.search}`)
          .then(response => {
            this.newUserAutocomplete.SetItems(response.data.users);
          })
          .catch(error => console.dir(error))
          .finally(() => this.newUserAutocomplete.Loading = false);
      }, 750);
    }
  }
</script>

<style scoped lang="scss">

</style>
