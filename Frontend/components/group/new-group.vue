<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onFormSubmitted">
      <v-card-title class="headline grey lighten-2">New group</v-card-title>
      <v-card-text>
        <v-alert v-for="(error, index) of errors" :key="index" :type="error.AlertType" v-model="error.Show" transition="scale-transition">
          {{ error.Message }}
        </v-alert>
        <v-text-field hint="Min. 3 chars" counter :rules="NameRules" v-model="newGroup.Name"
                      label="Name"></v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-btn color="error" @click="onCloseClicked">Cancel</v-btn>
        <v-spacer></v-spacer>
        <v-btn :disabled="!valid" color="success" type="submit">Submit</v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Vue } from 'vue-property-decorator';
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import { Action, Getter } from 'vuex-class';
  import CreateGroupModel from '../../models/setter/group/CreateGroupModel';
  import SnackMessage from "../../models/app/SnackMessage";
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";

  @Component({})

  export default class NewGroup extends Vue {
    valid: boolean = false;
    newGroup: CreateGroupModel = new CreateGroupModel();
    loading: boolean = false;
    errors: Array<AlertDisplay> = [];
    @Getter('UserStore/user') User: UserAccountModel;
    @Action('SnackStore/addSnack') AddSnack: Function;

    mounted() {
      this.newGroup.GroupCreatorId = this.User.Id;
    }

    @Emit('close')
    onCloseClicked() {
      this.newGroup.Name = '';
    }

    @Emit('create')
    onGroupCreated(id: number, name: string): GroupListItemModel {
      const group = new GroupListItemModel();
      group.IsAcceptedByManager = true;
      group.IsAcceptedByUser = true;
      group.Name = name;
      group.Id = id;
      group.IsManager = true;
      group.IsOwner = true;
      this.newGroup.Name = '';
      return group;
    }

    async onFormSubmitted() {
      this.loading = true;
      await this.$axios.post('groups', this.newGroup)
        .then(response => {
          this.AddSnack(new SnackMessage(`Added group '${ response.data.name }'`, 'success'));
          this.onGroupCreated(response.data.id, response.data.name);
        })
        .catch(error => {
          console.dir(error);
          if (error.response.status === 400)
            this.errors.push(new AlertDisplay(AlertType.Error, error.response.data, 2500));
        })
        .finally(() => this.loading = false);
    }

    get NameRules(): Array<any> {
      return [
        v => !!v || 'Name is required.',
        v => v.length >= 3 || 'Name needs at least 3 characters.'
      ]
    }
  }
</script>

<style scoped lang="scss">

</style>
