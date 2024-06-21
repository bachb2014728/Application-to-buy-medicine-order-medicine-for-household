<template>
  <form @submit.prevent="submit">
    <h1 class="h3 mb-3 card-title text-center">Đăng nhập</h1>
    <div class="mb-3">
      <div class="form-floating form-group">
        <input v-model="data.username" type="text" class="form-control form-control-lg" :class="{ 'is-invalid': errors.username }"  placeholder="name@example.com" @input="onChange('username')">
        <label>Username</label>
        <p v-if="errors.username" class="text-danger label"> {{ errors.username }}</p>
      </div>
    </div>
    <div class="mb-3">
      <div class="form-floating form-group">
        <input v-model="data.password" type="password" class="form-control form-control-lg" :class="{ 'is-invalid': errors.password }"  placeholder="password" @input="onChange('password')">
        <label>Mật khẩu</label>
        <p v-if="errors.password" class="text-danger label"> {{ errors.password }}</p>
      </div>
    </div>
    <button class="w-100 btn btn-lg btn-success" type="submit">Đăng nhập</button>
  </form>
</template>
<script>
import {reactive, ref} from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { useStore } from 'vuex';
import AuthService from "@/services/auth.service.js";
import { useToast } from 'vue-toastification'
import store from "@/store/index.js";

export default {
  name: "LoginView",
  mounted() {
    document.title = 'Đăng nhập'
  },
  setup() {
    const toast = useToast()
    const data = reactive({
      username: '',
      password: ''
    });
    const errors = reactive({
      username: '',
      password: ''
    });
    const store = useStore();
    const router = useRouter();
    const onChange = (field) => {
      errors[field] = '';
    }
    const submit = () => {
      if (!data.username) errors.username = 'Username là rỗng.';
      if (!data.password) errors.password = 'Password là rỗng.';
      if (Object.values(errors).every(error => !error)) {
        AuthService.login(data)
            .then( async response => {
              if (response && response.status === 200) {
                await AuthService.profile().then(res =>{
                  store.dispatch('setAuth', true);
                  store.dispatch('setRole',res.data.roleName)
                  if(response.data.roleName === 'CUSTOMER'){
                    store.dispatch('setMessage', res.data.firstName + " "+ res.data.lastName);
                  }else{
                    store.dispatch('setMessage', res.data.name);
                  }
                  localStorage.setItem('token', response.data.token);
                  axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                  toast.success("Đăng nhập thành công.")
                  router.push('/');
                })
              }
            })
            .catch(error => {
              if (error.response && error.response.status === 404) {
                toast.error(error.response.data.detail);
              } else {
                console.error(error);
                toast.error('Có lỗi xảy ra khi đăng nhập.');
              }
            });
      }
    };
    return {
      data,
      submit,
      errors,
      onChange
    }
  }
}
</script>

<style scoped>
.is-invalid {
  border-color: red;
}
</style>