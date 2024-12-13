<template>
  <v-container>
    <v-dialog v-model="showAddItemsDialog" max-width="600px">
      <AddItemsView :group="addItemGroup" :groups="dashboard.Groups" @close="showAddItemsDialog = false"
                    @add="onItemAdded"></AddItemsView>
    </v-dialog>
    <template v-if="!dashboard || !dashboard.AnyGroups">
      <v-flex xs12 md8 offset-md2>
        <v-card>
          <v-card-text>
            <v-flex>
              <h4 class="headline">Get started</h4>
              <h6 class="subheading">Create a group or join an existing one.</h6>
              <v-layout justify-space-between>
                <v-btn nuxt to="/groups?create=1" class="mx-0" color="primary">Create Group</v-btn>
                <v-btn nuxt to="/groups?join=1" class="mx-0" color="primary">Join Group</v-btn>
              </v-layout>
            </v-flex>
          </v-card-text>
        </v-card>
      </v-flex>
    </template>
    <template v-else-if="dashboard.Items.length <= 0">
      <v-flex xs12 md6 offset-md3>
        <v-card>
          <v-card-text class="body-2">
            No items found.
          </v-card-text>
        </v-card>
      </v-flex>
    </template>
    <template v-else>
      <v-flex xs12 md6 offset-md3>
        <v-card>
          <v-fab-transition>
            <v-btn fixed fab top right color="success" v-show="ItemsToUpdate.length > 0"
                   @click="onItemsSubmitted" :loading="loading" style="top: 75px">
              <v-icon>done_outline</v-icon>
            </v-btn>
          </v-fab-transition>
          <template v-if="dashboard.Groups.length > 1">
            <v-select class="px-2 pt-3"
                      chips
                      small-chips
                      multiple
                      label="Filter by group"
                      clearable
                      :items="dashboard.Groups"
                      item-text="Name"
                      item-value="Id"
                      v-model="filterGroupIds"></v-select>
          </template>
          <template v-if="ItemsThatAreOut.length > 0">
            <v-list two-line subheader>
              <v-subheader inset class="error--text">Out</v-subheader>
              <v-list-tile avatar v-for="(item, index) of ItemsThatAreOut" :key="index" class="mb-2"
                           v-if="!item.DoNotBuy">
                <v-list-tile-action>
                  <v-layout justify-center align-center>
                    <v-btn icon v-if="item.PendingOkayStatus" class="mr-2" @click="resetItemStatus(item)"
                           color="success">
                      <v-icon size="12">brightness_1</v-icon>
                    </v-btn>
                    <v-btn v-else icon color="success" outline class="mr-2" @click="item.PendingOkayStatus = true">
                      <v-icon size="12" color="success">brightness_1</v-icon>
                    </v-btn>
                  </v-layout>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
                  <v-list-tile-sub-title class="text--primary">{{ item.Group.Name }}</v-list-tile-sub-title>
                  <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
                </v-list-tile-content>
                <v-list-tile-action v-if="item.Notes || item.OneTimePurchase">
                  <v-layout row justify-end align-center>
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
            </v-list>
            <v-divider></v-divider>
          </template>
          <template v-if="ItemsThatAreAlmostOut.length > 0">
            <v-list two-line subheader>
              <v-subheader inset class="warning--text">Almost out</v-subheader>
              <v-list-tile avatar v-for="(item, index) of ItemsThatAreAlmostOut" :key="index" class="mb-2"
                           v-if="!item.DoNotBuy">
                <v-list-tile-action>
                  <v-layout column justify-space-around align-center fill-height>
                    <v-btn v-if="item.PendingOkayStatus" class="mr-2" icon color="success"
                           @click="resetItemStatus(item)">
                      <v-icon size="12">brightness_1</v-icon>
                    </v-btn>
                    <v-btn v-else-if="item.PendingOutStatus" class="mr-2" icon color="error"
                           @click="resetItemStatus(item)">
                      <v-icon size="12">brightness_1</v-icon>
                    </v-btn>
                    <template v-else>
                      <v-btn icon small color="error" outline class="mr-2" @click="item.PendingOutStatus = true">
                        <v-icon size="12" color="error">brightness_1</v-icon>
                      </v-btn>
                      <v-btn icon small color="success" outline class="mr-2" @click="item.PendingOkayStatus = true">
                        <v-icon size="12" color="success">brightness_1</v-icon>
                      </v-btn>
                    </template>
                  </v-layout>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
                  <v-list-tile-sub-title class="text--primary">{{ item.Group.Name }}</v-list-tile-sub-title>
                  <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
                </v-list-tile-content>
                <v-list-tile-action v-if="item.Notes || item.OneTimePurchase">
                  <v-layout row justify-end align-center>
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
            </v-list>
            <v-divider></v-divider>
          </template>
          <template v-if="ItemsThatAreOkay.length > 0">
            <v-list two-line subheader>
              <v-subheader inset class="success--text text--darken-1">Stocked</v-subheader>
              <v-list-tile v-for="(item, index) of ItemsThatAreOkay" :key="index" class="mb-2" v-if="!item.DoNotBuy">
                <v-list-tile-action>
                  <v-layout column justify-space-around align-center fill-height>
                    <v-btn v-if="item.PendingOutStatus" icon class="mr-2" color="error" @click="resetItemStatus(item)">
                      <v-icon size="12">brightness_1</v-icon>
                    </v-btn>
                    <v-btn v-else-if="item.PendingAlmostOutStatus" icon class="mr-2" color="warning"
                           @click="resetItemStatus(item)">
                      <v-icon size="12">brightness_1</v-icon>
                    </v-btn>
                    <template v-else>
                      <v-btn icon small color="error" outline class="mr-2" @click="item.PendingOutStatus = true">
                        <v-icon size="12" color="error">brightness_1</v-icon>
                      </v-btn>
                      <v-btn icon small color="warning" outline class="mr-2"
                             @click="item.PendingAlmostOutStatus = true">
                        <v-icon size="12" color="warning">brightness_1</v-icon>
                      </v-btn>
                    </template>
                  </v-layout>
                </v-list-tile-action>
                <v-list-tile-content>
                  <v-list-tile-title>{{ item.Name }}</v-list-tile-title>
                  <v-list-tile-sub-title class="text--primary">{{ item.Group.Name }}</v-list-tile-sub-title>
                  <v-list-tile-sub-title>Updated {{ getLastUpdate(item) }}</v-list-tile-sub-title>
                </v-list-tile-content>
                <v-list-tile-action v-if="item.Notes || item.OneTimePurchase">
                  <v-layout row justify-end align-center>
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
            </v-list>
          </template>
          <v-btn fixed fab bottom right color="success" @click="showAddItemsDialog = true">
            <v-icon>add</v-icon>
          </v-btn>
        </v-card>
      </v-flex>
    </template>
  </v-container>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import DashboardModel from "../../models/getter/dashboard/DashboardModel";
  import DashboardItemModel from "../../models/getter/dashboard/DashboardItemModel";
  import { ItemStatus } from '../../models/enums/ItemStatus';
  import DayJsService from "../../services/DayJsService";
  import UpdateGroupItemModel from "../../models/setter/item/UpdateGroupItemModel";
  import ArrayService from "../../services/ArrayService";
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import AddItemsView from '../../components/item/add-items-view.vue';
  import GroupItemViewModel from "../../models/getter/group/GroupItemViewModel";

  @Component({
    components: {AddItemsView},
    layout: 'backend',
    middleware: 'auth',
    mounted() {
      if ('serviceWorker' in navigator) {
        navigator.serviceWorker.register('/sw.js');
      }
    },
    async asyncData(context) {
      let dashboard: DashboardModel;
      await context.$axios.get('dashboard')
        .then(response => {
          dashboard = new DashboardModel(response.data);
        })
        .catch(error => console.dir(error));
      return {
        dashboard
      }
    }
  })

  export default class DashboardPage extends Vue {
    dashboard: DashboardModel;
    filterGroupIds: Array<number> = [];
    addItemGroup: GroupViewModel = new GroupViewModel();
    showAddItemsDialog: boolean = false;
    loading: boolean = false;

    get ItemsThatAreOut(): Array<DashboardItemModel> {
      return this.filterGroupIds.length > 0
        ? this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.Out && this.filterGroupIds.find(g => g === i.Group.Id))
        : this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.Out);
    }

    get ItemsThatAreAlmostOut(): Array<DashboardItemModel> {
      return this.filterGroupIds.length > 0
        ? this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.AlmostOut && this.filterGroupIds.find(g => g === i.Group.Id))
        : this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.AlmostOut);
    }

    get ItemsThatAreOkay(): Array<DashboardItemModel> {
      return this.filterGroupIds.length > 0
        ? this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.Stocked && this.filterGroupIds.find(g => g === i.Group.Id))
        : this.dashboard.Items.filter(i => i.ItemStatusId === ItemStatus.Stocked);
    }

    get ItemsToUpdate(): Array<DashboardItemModel> {
      return this.dashboard.Items.filter(i => i.PendingOkayStatus || i.PendingAlmostOutStatus || i.PendingOutStatus);
    }

    getLastUpdate(item: DashboardItemModel): string {
      return DayJsService.ConvertToLocalTime(item.LastUpdate).format('MMM D, YYYY');
    }

    onItemAdded(item: GroupItemViewModel) {
      if (!item.DoNotBuy)
        this.dashboard.Items.push(DashboardItemModel.Create(item));
    }

    resetItemStatus(item: DashboardItemModel) {
      item.PendingOutStatus = false;
      item.PendingAlmostOutStatus = false;
      item.PendingOkayStatus = false;
    }

    async onItemsSubmitted() {
      this.loading = true;
      for (let item of this.ItemsToUpdate) {
        const updateItem = UpdateGroupItemModel.Generate(item);
        await this.$axios.put(`groups/${ updateItem.GroupId }/items/${ updateItem.ItemId }`, updateItem)
          .then(response => {
            this.resetItemStatus(item);
            item.ItemStatusId = response.data.itemStatusId;
            item.LastUpdate = response.data.lastUpdate;
          })
          .catch(error => console.dir(error));
      }
      this.loading = false;
    }
  }
</script>

<style scoped lang="scss">
</style>
