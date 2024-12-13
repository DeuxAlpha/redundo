<template>
  <v-card>
    <v-card-text class="headline grey lighten-2">Leave {{ group.Name }}</v-card-text>
    <v-card-text>
      <p class="body-2 ma-0">Are you sure you that you wish to leave the group?</p>
    </v-card-text>
    <v-card-actions>
      <v-btn color="primary" @click="onCloseClicked">Cancel</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="error" outline @click="onSubmitClicked" :loading="loading">Leave</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import { Action, Getter } from 'vuex-class';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import SnackMessage from "../../models/app/SnackMessage";

	@Component({})

	export default class LeaveGroupView extends Vue {
	  @Prop(Object) readonly group: GroupViewModel;
	  @Getter('UserStore/user') readonly User: UserAccountModel;
	  @Action('SnackStore/addSnack') AddSnack: Function;
	  loading: boolean = false;

    @Emit('close')
    onCloseClicked(){}

    async onSubmitClicked() {
      this.loading = true;
      await this.$axios.delete(`groups/${this.group.Id}/users/${this.User.Id}`)
        .then(() => {
          this.AddSnack(new SnackMessage(`Successfully left ${this.group.Name}`, 'info'));
          this.$router.push('/groups');
        })
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }
	}
</script>

<style scoped lang="scss">

</style>
