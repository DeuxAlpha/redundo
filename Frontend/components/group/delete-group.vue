<template>
  <v-card>
    <v-card-title class="headline grey lighten-2">Delete {{ group.Name }}</v-card-title>
    <v-card-text>
      <p class="body-2 ma-0">
        Are you absolutely sure that you wish to delete your group?<br/>
        This act is irreversible!
      </p>
    </v-card-text>
    <v-card-actions>
      <v-btn color="primary" @click="onCloseClicked">Cancel</v-btn>
      <v-spacer></v-spacer>
      <v-btn color="error" outline type="submit" @click="onDeleteGroupClicked">Delete</v-btn>
    </v-card-actions>
    <v-slide-y-transition>
      <v-card-actions v-show="loading">
        <v-flex>
          <p class="subheading text-xs-center">Deleting group.</p>
          <v-progress-linear v-model="LoadingStatus" :active="loading"></v-progress-linear>
        </v-flex>
      </v-card-actions>
    </v-slide-y-transition>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Prop, Vue } from 'vue-property-decorator';
  import GroupViewModel from "../../models/getter/group/GroupViewModel";
  import { Action } from 'vuex-class';
  import SnackMessage from "../../models/app/SnackMessage";

  @Component({})

  export default class DeleteGroup extends Vue {
    @Prop(Object) readonly group: GroupViewModel;
    @Action('SnackStore/addSnack') AddSnack: Function;
    loading: boolean = false;
    loadingProgress: number = 0;

    @Emit('close')
    onCloseClicked() {
    }

    @Emit('delete')
    onGroupDeleted() {
      this.AddSnack(new SnackMessage(`Deleted ${this.group.Name}`, 'info'));
    }

    async onDeleteGroupClicked() {
      this.loading = true;
      for (let user of this.group.Users) {
        await this.$axios.delete(`groups/${ this.group.Id }/users/${ user.Id }`)
          .then(() => this.loadingProgress++)
          .catch(error => console.dir(error));
      }
      await this.$axios.delete(`groups/${ this.group.Id }`)
        .then(() => {
          this.loadingProgress++;
          this.onGroupDeleted();
        })
        .catch(error => console.dir(error))
        .finally(() => this.loading = false);
    }

    get LoadingStatus(): number {
      return this.loadingProgress / (this.group.Users.length + 1) * 100;
    }
  }
</script>

<style scoped lang="scss">

</style>
