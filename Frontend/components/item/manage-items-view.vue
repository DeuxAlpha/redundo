<template>
  <v-card>
    <v-dialog v-model="showManageItemDialog" max-width="600px">
      <ManageItemView :item="selectedItem" :group="group" @close="showManageItemDialog = false" @update="showManageItemDialog = false"></ManageItemView>
    </v-dialog>
    <v-card-title class="headline grey lighten-2">Manage items for {{ group.Name }}</v-card-title>
    <v-card-text>
      <v-list subheader v-if="group.Items.length > 0">
        <template v-if="ItemsThatAreOut.length > 0">
          <v-subheader inset>Out</v-subheader>
          <v-list-tile v-for="item of ItemsThatAreOut" :key="item.Id">
            <v-list-tile-avatar color="primary" @click="onModifyItemClicked(item)">
              <v-btn icon>
                <v-icon color="white">edit</v-icon>
              </v-btn>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
              <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
            </v-list-tile-content>
            <v-list-tile-action v-if="item.Notes || item.DoNotBuy || item.OneTimePurchase">
              <v-layout row justify-end align-center>
                <v-tooltip bottom v-if="item.DoNotBuy">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="error" v-on="on">pause</v-icon>
                  </template>
                  <span>Do not buy</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.OneTimePurchase">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="warning" v-on="on">repeat_one</v-icon>
                  </template>
                  <span>One time purchase</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.Notes">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="primary" v-on="on">info</v-icon>
                  </template>
                  <span>{{ item.Notes }}</span>
                </v-tooltip>
              </v-layout>
            </v-list-tile-action>
          </v-list-tile>
          <v-divider></v-divider>
        </template>
        <template v-if="ItemsThatAreAlmostOut.length > 0">
          <v-subheader inset>Almost out</v-subheader>
          <v-list-tile v-for="item of ItemsThatAreAlmostOut" :key="item.Id">
            <v-list-tile-avatar color="primary" @click="onModifyItemClicked(item)">
              <v-btn icon>
                <v-icon color="white">edit</v-icon>
              </v-btn>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
              <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
            </v-list-tile-content>
            <v-list-tile-action v-if="item.Notes || item.DoNotBuy || item.OneTimePurchase">
              <v-layout row justify-end align-center>
                <v-tooltip bottom v-if="item.DoNotBuy">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="error" v-on="on">pause</v-icon>
                  </template>
                  <span>Do not buy</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.OneTimePurchase">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="warning" v-on="on">repeat_one</v-icon>
                  </template>
                  <span>One time purchase</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.Notes">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="primary" v-on="on">info</v-icon>
                  </template>
                  <span>{{ item.Notes }}</span>
                </v-tooltip>
              </v-layout>
            </v-list-tile-action>
          </v-list-tile>
          <v-divider></v-divider>
        </template>
        <template v-if="ItemsThatAreOkay.length > 0">
          <v-subheader inset>Stocked</v-subheader>
          <v-list-tile v-for="item of ItemsThatAreOkay" :key="item.Id">
            <v-list-tile-avatar color="primary" @click="onModifyItemClicked(item)">
              <v-btn icon>
                <v-icon color="white">edit</v-icon>
              </v-btn>
            </v-list-tile-avatar>
            <v-list-tile-content>
              <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
              <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
            </v-list-tile-content>
            <v-list-tile-action v-if="item.Notes || item.DoNotBuy || item.OneTimePurchase">
              <v-layout row justify-end align-center>
                <v-tooltip bottom v-if="item.DoNotBuy">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="error" v-on="on">pause</v-icon>
                  </template>
                  <span>Do not buy</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.OneTimePurchase">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="warning" v-on="on">repeat_one</v-icon>
                  </template>
                  <span>One time purchase</span>
                </v-tooltip>
                <v-tooltip bottom v-if="item.Notes">
                  <template v-slot:activator="{on}">
                    <v-icon class="default-cursor" color="primary" v-on="on">info</v-icon>
                  </template>
                  <span>{{ item.Notes }}</span>
                </v-tooltip>
              </v-layout>
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
  import GroupItemViewModel from "../../models/getter/group/GroupItemViewModel";
  import { ItemStatus } from '../../models/enums/ItemStatus';
  import DayJs from 'dayjs';
  import ManageItemView from './manage-item-view.vue';
  import DayJsService from "../../services/DayJsService";

  @Component({
    components: {ManageItemView}
  })

  export default class ManageItemsView extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    selectedItem: GroupItemViewModel = new GroupItemViewModel();
    showManageItemDialog: boolean = false;

    @Emit('close')
    onCloseClicked() {
    }

    get ItemsThatAreOut(): Array<GroupItemViewModel> {
      return this.group.Items.filter(i => i.ItemStatusId === ItemStatus.Out);
    }

    get ItemsThatAreAlmostOut(): Array<GroupItemViewModel> {
      return this.group.Items.filter(i => i.ItemStatusId === ItemStatus.AlmostOut);
    }

    get ItemsThatAreOkay(): Array<GroupItemViewModel> {
      return this.group.Items.filter(i => i.ItemStatusId === ItemStatus.Stocked);
    }

    getLastUpdate(item: GroupItemViewModel) {
      return DayJsService.ConvertToLocalTime(item.LastUpdate).format('MMM D, YYYY');
    }

    onModifyItemClicked(item: GroupItemViewModel) {
      this.selectedItem = item;
      this.showManageItemDialog = true;
    }
  }
</script>

<style scoped lang="scss">

</style>
