<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onFormSubmitted">
      <v-card-title class="headline grey lighten-2">Find group</v-card-title>
      <v-card-text>
        <v-autocomplete v-model="newGroup.GroupId"
                        :loading="findGroupAutocomplete.Loading"
                        :search-input.sync="search"
                        clearable
                        :rules="GroupRules"
                        :items="findGroupAutocomplete.Items"
                        :item-value="findGroupAutocomplete.ValueProperty"
                        :item-text="findGroupAutocomplete.TextProperty"
                        hide-no-data
                        label="Group"></v-autocomplete>
        <v-textarea v-model="newGroup.RequestMessage"
                    label="Application Message"
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
  import { Component, Emit, Vue, Watch } from 'vue-property-decorator';
  import CreateGroupUserModel from "../../models/setter/group/CreateGroupUserModel";
  import AutocompleteModel from "../../models/app/AutocompleteModel";
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";
  import { Action, Getter } from 'vuex-class';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class FindGroup extends Vue {
    valid: boolean = false;
    newGroup: CreateGroupUserModel = new CreateGroupUserModel();
    findGroupAutocomplete: AutocompleteModel = new AutocompleteModel('name', 'id');
    loading: boolean = false;
    search: string = '';
    writingTimeout;
    @Getter('UserStore/user') User: UserAccountModel;
    @Action('SnackStore/addSnack') AddSnack: Function;

    mounted() {
      this.setRequestMessage();
    }

    @Emit('close')
    onCloseClicked() {
      this.search = '';
      this.newGroup.GroupId = undefined;
      this.setRequestMessage();
    }

    @Emit('join')
    onGroupJoined(group: GroupListItemModel): GroupListItemModel {
      this.search = '';
      this.newGroup.GroupId = undefined;
      return group;
    }

    setRequestMessage() {
      this.newGroup.RequestMessage = `Hello! I'd like to join your group.\r\n\r\nRegards, ${this.User.Username}`;
    }

    @Watch('search')
    async onGroupChanged() {
      if (!this.search || this.search.length <= 0) return;
      if (this.writingTimeout)
        clearTimeout(this.writingTimeout);
      if (this.findGroupAutocomplete.Loading) return;
      this.writingTimeout = setTimeout(async () => {
        this.findGroupAutocomplete.Loading = true;
        await this.$axios.get(`groups?name=${ this.search }`)
          .then(response => {
            this.findGroupAutocomplete.SetItems(response.data.groups);
          })
          .catch(error => console.dir(error))
          .finally(() => this.findGroupAutocomplete.Loading = false);
      }, 750);
    }

    async onFormSubmitted() {
      this.loading = true;
      this.newGroup.UserId = this.User.Id;
      await this.$axios.post(`groups/${this.newGroup.GroupId}/users`, this.newGroup)
        .then(response => {
          const group = new GroupListItemModel();
          group.Id = response.data.groupId;
          group.Name = this.search;
          group.IsAcceptedByUser = response.data.isAcceptedByUser;
          group.IsAcceptedByManager = response.data.isAcceptedByManager;
          group.IsManager = response.data.isManager;
          group.IsOwner = false;
          group.RequestMessage = response.data.requestMessage;
          this.AddSnack(new SnackMessage(`Sent application to group '${this.search}'`, 'success'));
          this.onGroupJoined(group);
        })
        .catch(error => {
          if (error.response.status === 400) {
            this.AddSnack(new SnackMessage(`You are already part of group '${this.search}'`, 'error'));
          }
          console.dir(error)
        })
        .finally(() => this.loading = false);
    }

    get GroupRules(): Array<any> {
      return [
        v => !!v || 'Group is required.'
      ]
    }
  }
</script>

<style scoped lang="scss">

</style>
