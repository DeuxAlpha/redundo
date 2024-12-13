<template>
  <v-container d-flex justify-center>
    <v-flex xs12 md4>
      <v-alert v-for="(error, index) of errors" :key="index" v-model="error.Show" :type="error.AlertType"
               transition="scale-transition">{{ error.Message }}
      </v-alert>
      <v-card>
        <v-form v-model="valid" @submit.prevent="onFormSubmit">
          <v-layout row wrap>
            <v-flex px-2 xs12>
              <v-text-field hint="Min. 6 chars" counter :rules="NameRules" v-model="user.Username"
                            label="Username"></v-text-field>
            </v-flex>
            <v-flex px-2 xs12>
              <v-text-field hint="Min. 7 chars" counter :rules="PasswordRules" v-model="user.Password" label="Password"
                            type="password"></v-text-field>
            </v-flex>
            <v-flex px-2 xs12>
              <v-text-field counter :rules="RepeatPasswordRules" v-model="user.RepeatPassword" label="Repeat Password"
                            type="password"></v-text-field>
            </v-flex>
            <v-flex px-2 xs12>
              <v-btn block :disabled="!valid" :loading="loading" color="success" type="submit">Register</v-btn>
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
  import UserRegisterModel from "../../models/setter/user/UserRegisterModel";
  import AlertDisplay, { AlertType } from "../../models/app/AlertDisplay";

  @Component({})

  export default class RegisterPage extends Vue {
    valid: boolean = false;
    user: UserRegisterModel = new UserRegisterModel();
    loading: boolean = false;
    errors: Array<AlertDisplay> = [];

    async onFormSubmit() {
      this.loading = true;
      await this.$axios.post('users', this.user)
        .then(() => {
          this.$router.push(`/login?username=${ this.user.Username }`);
        })
        .catch(error => {
          console.dir(error);
          if (!error.response)
            this.errors.push(new AlertDisplay(AlertType.Error, error.message, 2500));
          else if (error.response.data.includes('already exists'))
            this.errors.push(new AlertDisplay(AlertType.Error, 'Username is already taken.', 2500));
        })
        .finally(() => this.loading = false);
    }

    get NameRules(): Array<any> {
      return [
        v => !!v || 'Name is required.',
        v => v.length >= 6 || 'Username needs at least 6 characters.'
      ]
    }

    get PasswordRules(): Array<any> {
      return [
        v => !!v || 'Password is required.',
        v => v.length >= 7 || 'Password needs at least 7 characters.'
      ]
    }

    get RepeatPasswordRules(): Array<any> {
      return [
        v => !!v || 'Repeat password is required.',
        v => v === this.user.Password || 'Passwords must match.'
      ]
    }
  }
</script>

<style scoped lang="scss">

</style>
