<template>
  <v-container d-flex justify-center>
    <v-flex xs12 md4>
      <v-alert v-if="registeredAlert" :type="registeredAlert.AlertType" v-model="registeredAlert.Show">
        Thank you for registering!<br/>Please login to get started!
      </v-alert>
      <v-alert v-if="loggedOutAlert" :type="loggedOutAlert.AlertType" v-model="loggedOutAlert.Show">
        You were successfully logged out.
      </v-alert>
      <v-alert v-if="deletedAlert" :type="deletedAlert.AlertType" v-model="deletedAlert.Show">
        You account has been deleted.
      </v-alert>
      <v-alert v-for="(error, index) of errors" :key="index" :type="error.AlertType" v-model="error.Show" transition="scale-transition">
        {{ error.Message }}
      </v-alert>
      <v-card>
        <v-form v-model="valid" @submit.prevent="onFormSubmit">
          <v-layout row wrap>
            <v-flex px-2 xs12>
              <v-text-field :rules="NameRules" v-model="user.Username" label="Username"></v-text-field>
            </v-flex>
            <v-flex px-2 xs12>
              <v-text-field :rules="PasswordRules" v-model="user.Password" label="Password"
                            type="password"></v-text-field>
            </v-flex>
            <v-flex px-2 xs12>
              <v-btn block :disabled="!valid" :loading="loading" color="success" type="submit">Login</v-btn>
            </v-flex>
            <v-flex px-2 xs12>
              <v-btn block color="error" to="/">Cancel</v-btn>
            </v-flex>
          </v-layout>
        </v-form>
      </v-card>
    </v-flex>
  </v-container>
</template>

<script lang="ts">
  import { Component, Vue } from 'vue-property-decorator';
  import UserLoginModel from "../../models/setter/user/UserLoginModel";
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";
  import { Action, Getter } from 'vuex-class';
  import UserAccountModel from "../../models/setter/user/UserAccountModel";

  @Component({})

  export default class LoginPage extends Vue {
    valid: boolean = false;
    loading: boolean = false;
    user: UserLoginModel = new UserLoginModel();
    registeredAlert: AlertDisplay = null;
    loggedOutAlert: AlertDisplay = null;
    deletedAlert: AlertDisplay = null;
    errors: Array<AlertDisplay> = [];
    @Action('UserStore/login') Login;
    @Getter('UserStore/user') User: UserAccountModel;

    mounted() {
      if (this.User)
        this.$router.push('/dashboard');
      this.setupLoginForm();
    }

    setupLoginForm() {
      if (this.$route.query.username) {
        this.registeredAlert = new AlertDisplay(AlertType.Success);
        this.user.Username = this.$route.query.username as string;
      }
      if (this.$route.query.exit) {
        this.loggedOutAlert = new AlertDisplay(AlertType.Success);
      }
      if (this.$route.query.delete) {
        this.deletedAlert = new AlertDisplay(AlertType.Info);
      }
    }

    async onFormSubmit() {
      this.loading = true;
      const result = await this.Login(this.user);
      if (result.status === 200)
        this.$router.push('/dashboard');
      else if (result.response.status === 401)
        this.errors.push(new AlertDisplay(AlertType.Error, 'Invalid username or password.', 2500));
      else
        this.errors.push(new AlertDisplay(AlertType.Error, result.message, 2500));
      this.loading = false;
    }

    get NameRules(): Array<any> {
      return [
        v => !!v || 'Username is required.'
      ];
    }

    get PasswordRules(): Array<any> {
      return [
        v => !!v || 'Password is required.'
      ];
    }
  }
</script>

<style scoped lang="scss">

</style>
