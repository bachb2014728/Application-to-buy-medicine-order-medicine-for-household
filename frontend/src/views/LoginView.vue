<template>
  <div class="row d-flex flex-row  align-items-center">
    <div class="col-8">
      <div class="card">
        <div class="card-body">
          <h4 class="mb-2 text-center">Chào mừng đến với <router-link to="/" style="text-decoration: none;" class="text-success">thegioithuoc.com</router-link> 👋</h4>
          <p class="mb-4 text-center"><span class="text-danger fw-medium">Đăng nhập</span> vào trang web để có trải nghiệm tốt nhất.</p>
          <form id="formAuthentication" class="mb-3" @submit.prevent="submit">
            <div class="mb-3">
              <label for="email" class="form-label">Email or Username</label>
              <input type="text" class="form-control" id="email" name="username" v-model="data.username"
                     :class="{ 'is-invalid': errors.username }" @input="onChange('username')"
                     placeholder="Vui lòng nhập username " autofocus="">
              <p v-if="errors.username" class="text-danger fw-medium"> {{ errors.username }}</p>
            </div>
            <div class="mb-3">
              <label class="form-label" for="password">Mật khẩu</label>
              <input type="password" id="password" class="form-control" name="password"
                     v-model="data.password" :class="{ 'is-invalid': errors.password }" @input="onChange('password')"
                     placeholder="Vui lòng nhập mật khẩu" aria-describedby="password">
              <p v-if="errors.password" class="text-danger fw-medium"> {{ errors.password }}</p>
            </div>
            <div class="mb-3">
              <button class="btn btn-primary d-grid w-100" type="submit">Đăng nhập</button>
            </div>
          </form>
          <p class="text-center">
            <span>Nếu bạn chưa có tài khoản ?</span>
            <router-link to="/register" style="text-decoration: none"><span class="text-primary fw-medium"> Tạo tài khoản</span></router-link>
          </p>
        </div>
      </div>
    </div>
    <div class="col-4">
      <img src="@/assets/images/log-in.png" class="img-fluid" alt="" >
    </div>
  </div>
</template>
<script>
import {reactive, ref} from "vue";
import { useRouter } from "vue-router";
import axios from "axios";
import { useStore } from 'vuex';
import AuthService from "@/services/auth.service.js";
import { useToast } from 'vue-toastification'
import CartService from "@/services/cart.service.js";

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
    const carts = ref([])
    const onChange = (field) => {
      errors[field] = '';
    }
    const submit = () => {
      if (!data.username) errors.username = 'Username là rỗng.';
      if (!data.password) errors.password = 'Password là rỗng.';
      if (Object.values(errors).every(error => !error)) {
        AuthService.login(data)
            .then( async response => {
              localStorage.setItem('token', response.data.token);
              axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
              if (response && response.status === 200) {
                CartService.getAll().then(resCart => {
                  carts.value = resCart.data
                })
                await AuthService.profile().then(res =>{
                  store.dispatch('setAuth', true);
                  store.dispatch('setRole','Customer')
                  store.dispatch('setCart',resCart.data.length)
                  store.dispatch('setMessage', res.data.firstName + " "+ res.data.lastName);
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