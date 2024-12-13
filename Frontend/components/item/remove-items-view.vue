<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Remove items from {{ group.Name }}</v-card-title>
    <v-card-text>
      <v-list v-if="group.Items.length > 0">
        <template v-for="(item, index) of group.Items">
          <v-divider v-if="index === 0"></v-divider>
          <v-list-tile :key="index">
            <v-list-tile-content>
              <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
            </v-list-tile-content>
            <v-list-tile-action>
              <v-btn color="error" flat :loading="item.DeletionLoading" @click="onDeleteItemClicked(item.Id)">Delete</v-btn>
            </v-list-tile-action>
          </v-list-tile>
          <v-divider></v-divider>
        </template>
      </v-list>
      <h6 v-else class="title">No items exist in this group.</h6>
    </v-card-text>
    <v-card-actions>
      <v-btn color="error" @click="onCloseClicked">Close</v-btn>
    </v-card-actions>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import { Action } from 'vuex-class';
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class RemoveItemsView extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    @Action('SnackStore/addSnack') AddSnack: Function;

    @Emit('close')
    onCloseClicked() {
    }

    async onDeleteItemClicked(id: number) {
      const item = this.group.Items.find(i => i.Id === id);
      item.DeletionLoading = true;
      await this.$axios.delete(`groups/${this.group.Id}/items/${item.Id}`)
        .then(() => {
          this.AddSnack(new SnackMessage(`Removed ${item.Name} from group`, 'info'));
          this.group.Items.splice(this.group.Items.indexOf(item), 1);
        })
        .catch(error => console.dir(error));
    }
  }
</script>

<style scoped lang="scss">

</style>
