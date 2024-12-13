<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Request to {{ group.Name }}</v-card-title>
    <v-card-text class="maintain-white-space">{{ group.RequestMessage }}</v-card-text>
    <v-card-actions>
      <v-btn color="error" :loading="deletionLoading" @click="onDeleteRequestClicked">Delete request</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="primary" @click="onCloseClicked">Close</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupListItemModel from "../../models/getter/group/GroupListItemModel";
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import { Action, Getter } from 'vuex-class';

	@Component({})

	export default class RequestView extends Vue {
	  deletionLoading: boolean = false;
	  @Prop(Object) readonly group: GroupListItemModel;
	  @Getter('UserStore/user') User: UserAccountModel;
	  @Action('SnackStore/addSnack') AddSnack: Function;

    @Emit('close')
    onCloseClicked() {}

    @Emit('delete')
    onRequestDeleted(): number {
      return this.group.Id;
    }

    async onDeleteRequestClicked() {
      this.deletionLoading = true;
      await this.$axios.delete(`groups/${this.group.Id}/users/${this.User.Id}`)
        .then(() => this.onRequestDeleted())
        .catch(error => console.dir(error))
        .finally(() => this.deletionLoading = false);
    }
	}
</script>

<style scoped lang="scss">

</style>
