<template>
  <v-card>
    <v-form v-model="valid" @submit.prevent="onFormSubmitted">
      <v-card-title class="headline grey lighten-2">Update password</v-card-title>
      <v-card-text>
        <v-alert v-for="(error, index) of errors" :key="index" :type="error.AlertType" v-model="error.Show" transition="scale-transition">
          {{ error.Message }}
        </v-alert>
        <v-text-field :rules="OldPasswordRules" v-model="updatePassword.OldPassword" label="Old password" type="password"></v-text-field>
        <v-text-field hint="Min. 7 chars" counter :rules="NewPasswordRules" v-model="updatePassword.NewPassword" label="New password" type="password"></v-text-field>
        <v-text-field counter :rules="RepeatPasswordRules" v-model="updatePassword.RepeatPassword" label="Repeat new password" type="password"></v-text-field>
      </v-card-text>
      <v-card-actions>
        <v-btn color="error" @click="onCloseClicked">Cancel</v-btn>
        <v-spacer></v-spacer>
        <v-btn color="success" type="submit" :disabled="!valid" :loading="loading">Submit</v-btn>
      </v-card-actions>
    </v-form>
  </v-card>
</template>

<script lang="ts">
  import { Component, Emit, Vue } from 'vue-property-decorator';
  import UserPasswordModel from "../../models/setter/user/UserPasswordModel";
  import UserAccountModel from "../../models/setter/user/UserAccountModel";
  import { Getter } from 'vuex-class';
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";

  @Component({
    mounted() {
      this.updatePassword.Id = this.User.Id;
    }
  })

	export default class UserPassword extends Vue {
	  valid: boolean = false;
	  loading: boolean = false;
	  updatePassword: UserPasswordModel = new UserPasswordModel();
	  errors: Array<AlertDisplay> = [];

	  @Getter('UserStore/user') User: UserAccountModel;

	  @Emit('close')
    onCloseClicked() {
	    this.updatePassword.Reset();
    }

    @Emit('submit')
    onPasswordSubmitted() {}

	  get OldPasswordRules(): Array<any> {
	    return [
	      v => !!v || 'Old password is required.'
      ]
    }

    get NewPasswordRules(): Array<any> {
	    return [
        v => !!v || 'New password is required.',
        v => v.length >= 7 || 'Password needs at least 7 characters.'
      ]
    }

    get RepeatPasswordRules(): Array<any> {
	    return [
	      v => !!v || 'Repeat password is required.',
	      v => v === this.updatePassword.NewPassword || 'Passwords do not match.'
      ]
    }

    async onFormSubmitted() {
	    this.loading = true;
	    await this.$axios.put(`users/${this.updatePassword.Id}/password`, this.updatePassword)
        .then(() => {
          this.updatePassword.Reset();
          this.onPasswordSubmitted()
        })
        .catch(error => {
          console.dir(error);
          if (error.response.status === 403)
            this.errors.push(new AlertDisplay(AlertType.Error, 'Invalid old password.', 2500));
        })
        .finally(() => this.loading = false);
    }
	}
</script>

<style scoped lang="scss">

</style>
