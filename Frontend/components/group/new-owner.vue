<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Make {{ newOwner.Name }} owner of {{ group.Name }}</v-card-title>
    <v-card-text class="body-2">
      Are you sure you want to complete this action?<br/>
      Doing so will replace you as owner of this group!
    </v-card-text>
    <v-card-actions>
      <v-btn color="primary" @click="onCloseClicked">Cancel</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="error" :loading="loading" outline @click="onUpdateOwnerClicked">Update owner</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupUserViewModel from "../../models/getter/group/GroupUserViewModel";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import UpdateGroupModel from "../../models/setter/group/UpdateGroupModel";

	@Component({})

	export default class NewOwner extends Vue {
	  @Prop(Object) readonly newOwner: GroupUserViewModel;
	  @Prop(Object) readonly group: GroupViewModel;
	  loading: boolean = false;

	  @Emit('close')
    onCloseClicked() {}

    @Emit('update')
    onGroupUpdated() {}

    async onUpdateOwnerClicked() {
	    this.loading = true;
      const updateGroup: UpdateGroupModel = new UpdateGroupModel();
      updateGroup.Id = this.group.Id;
      updateGroup.OwnerId = this.newOwner.Id;
      await this.$axios.put(`groups/${updateGroup.Id}`, updateGroup)
        .then(() => this.onGroupUpdated())
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }
	}
</script>

<style scoped lang="scss">

</style>
