<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onAddItemClicked">
      <v-card-title class="headline grey lighten-2">
        <template v-if="groups">
          Add items
        </template>
        <template v-else>
          Add items for {{ group.Name }}
        </template>
      </v-card-title>
      <v-card-text>
        <v-alert v-for="(error, index) of errors" :key="index" :type="error.AlertType" v-model="error.Show"
                 transition="scale-transition">
          {{ error.Message }}
        </v-alert>
        <v-select :rules="GroupsRules" v-if="groups" label="Group" :items="groups" v-model="group.Id" item-value="Id"
                  item-text="Name"></v-select>
        <v-combobox :loading="itemLoading"
                    :search-input.sync="itemSearch"
                    :items="findItemAutocomplete.Items"
                    :item-value="findItemAutocomplete.ValueProperty"
                    :item-text="findItemAutocomplete.TextProperty"
                    label="Item"></v-combobox>
        <v-select v-model="itemStatusId" :items="itemStatuses" item-text="Status" item-value="Id"
                  label="Item status"></v-select>
        <v-textarea v-model="notes" counter="255" :rules="NotesRules" label="Notes" rows="3"></v-textarea>
        <v-checkbox v-model="oneTimePurchase" label="One time purchase only" color="primary"></v-checkbox>
        <v-btn color="success" type="submit" block :disabled="!this.itemSearch || this.itemSearch.length < 2 || !valid"
               :loading="loading">
          Add Item
        </v-btn>
      </v-card-text>
      <v-card-actions>
        <v-btn color="error" @click="onCloseClicked">Close</v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue, Watch } from 'vue-property-decorator';
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import AutocompleteModel from "../../models/app/AutocompleteModel";
  import CreateGroupItemModel from "../../models/setter/item/CreateGroupItemModel";
  import ItemStatusModel, { ItemStatus } from "../../models/enums/ItemStatus";
  import CreateItemModel from "../../models/setter/item/CreateItemModel";
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";
  import GroupItemViewModel from "../../models/getter/group/GroupItemViewModel";
  import { Action } from 'vuex-class';
  import SnackMessage from '../../models/app/SnackMessage';
  import DashboardGroupModel from "../../models/getter/dashboard/DashboardGroupModel";

  @Component({})

  export default class AddItemsView extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    @Prop(Array) readonly groups: Array<DashboardGroupModel>;
    @Prop(Boolean) readonly provideGroup: boolean;
    @Action('SnackStore/addSnack') AddSnack: Function;
    valid: boolean = false;
    notes: string = '';
    oneTimePurchase: boolean = false;
    findItemAutocomplete: AutocompleteModel = new AutocompleteModel('name', 'id');
    itemStatusId: ItemStatus = ItemStatus.Stocked;
    itemLoading: boolean = false;
    itemSearch: string = '';
    writingTimeout;
    itemStatuses: Array<ItemStatusModel> = ItemStatusModel.GetItemStatuses();
    loading: boolean = false;
    errors: Array<AlertDisplay> = [];

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('add')
    onItemAdded(item: GroupItemViewModel): GroupItemViewModel {
      this.AddSnack(new SnackMessage(`${ item.Name } has been added`, 'success'));
      return item;
    }

    resetState() {
      this.itemSearch = '';
      this.itemStatusId = ItemStatus.Stocked;
      this.notes = '';
      this.oneTimePurchase = false;
    }

    @Watch('itemSearch')
    async onItemSearchChanged() {
      if (!this.itemSearch || this.itemSearch.length <= 0) return;
      if (this.writingTimeout)
        clearTimeout(this.writingTimeout);
      if (this.findItemAutocomplete.Loading) return;
      this.writingTimeout = setTimeout(async () => {
        this.findItemAutocomplete.Loading = true;
        await this.$axios.get(`items?name=${ this.itemSearch }`)
          .then(response => {
            this.findItemAutocomplete.SetItems(response.data.items);
          })
          .catch(error => console.dir(error))
          .finally(() => this.findItemAutocomplete.Loading = false);
      }, 750)
    }

    async onAddItemClicked() {
      this.loading = true;
      const newGroupItem: CreateGroupItemModel = new CreateGroupItemModel();
      newGroupItem.GroupId = this.group.Id;
      newGroupItem.ItemStatusId = this.itemStatusId;
      newGroupItem.Notes = this.notes;
      newGroupItem.OneTimePurchase = this.oneTimePurchase;
      if (!this.SelectedItem) {
        const newItem: CreateItemModel = new CreateItemModel();
        newItem.Name = this.itemSearch;
        await this.$axios.post('items', newItem)
          .then(response => {
            newGroupItem.ItemId = response.data.id;
          })
          .catch(error => console.dir(error));
      } else {
        newGroupItem.ItemId = this.SelectedItem.id;
      }
      await this.$axios.post(`groups/${ this.group.Id }/items`, newGroupItem)
        .then(response => {
          const newItem = new GroupItemViewModel(response.data);
          newItem.Name = this.itemSearch;
          if (!this.group.Name)
            this.group.Name = this.groups.find(g => g.Id === this.group.Id).Name;
          newItem.Group = this.group;
          this.onItemAdded(newItem);
          this.resetState();
        })
        .catch(error => {
          if (error.response.status === 409) {
            this.errors.push(new AlertDisplay(AlertType.Error, `${ this.itemSearch } is already being tracked.`, 2500));
          }
          console.dir(error)
        })
        .finally(() => this.loading = false);
    }

    get SelectedItem(): any {
      return this.findItemAutocomplete.Items.find(i => (i as any).name === this.itemSearch) as CreateItemModel;
    }

    get NotesRules(): Array<any> {
      return [
        v => v.length <= 255 || 'Maximum 255 characters.'
      ]
    }

    get GroupsRules(): Array<any> {
      return [
        v => (this.groups && !!v) || 'Group is required.'
      ]
    }
  }
</script>

<style scoped lang="scss">

</style>
