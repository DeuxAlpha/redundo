<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onUpdateItemClicked">
      <v-card-title class="headline grey lighten-2">Modify {{ item.Name }}</v-card-title>
      <v-card-text>
        <v-select :items="itemStatuses" item-text="Status" item-value="Id" label="Item Status" v-model="item.ItemStatusId"></v-select>
        <v-textarea v-model="item.Notes" counter="255" :rules="NotesRules" label="Notes" rows="3"></v-textarea>
        <v-checkbox v-model="item.OneTimePurchase" label="One time purchase only" color="primary" hint="Sets do not buy flag on purchasing item" persistent-hint></v-checkbox>
        <v-checkbox v-model="item.DoNotBuy" label="Do not buy" color="primary" hint="Hides items from dashboard" persistent-hint></v-checkbox>
      </v-card-text>
      <v-card-actions>
        <v-btn color="error" @click="onCloseClicked">Close</v-btn>
        <v-spacer></v-spacer>
        <v-btn color="success" :disabled="!valid" type="submit" :loading="loading">Update</v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupItemViewModel from "../../models/getter/group/GroupItemViewModel";
  import ItemStatusModel from "../../models/enums/ItemStatus";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import CreateGroupItemModel from "../../models/setter/item/CreateGroupItemModel";
  import { Action } from 'vuex-class';
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class ManageItemView extends Vue {
    @Prop(Object) readonly item: GroupItemViewModel;
    @Prop(Object) readonly group: GroupViewModel;
    valid: boolean = false;
    itemStatuses: Array<ItemStatusModel> = ItemStatusModel.GetItemStatuses();
    loading: boolean = false;
    @Action('SnackStore/addSnack') AddSnack: Function;

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('update')
    onItemUpdated() {}

    async onUpdateItemClicked() {
      this.loading = true;
      await this.$axios.put(`groups/${this.group.Id}/items/${this.item.Id}`, CreateGroupItemModel.Create(this.item, this.group.Id))
        .then(() => {
          this.AddSnack(new SnackMessage(`Updated ${this.item.Name}`, 'success'));
          this.onItemUpdated()
        })
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }

    get NotesRules(): Array<any> {
      return [
        v => v.length <= 255 || 'Maximum 255 characters.'
      ]
    }
  }
</script>

<style scoped lang="scss">

</style>
