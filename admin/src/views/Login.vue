<template>
  <div class="container">
    <section class="section register min-vh-100 d-flex flex-column align-items-center justify-content-center py-4">
      <div class="container">
        <div class="row justify-content-center">
          <div class="col-lg-4 col-md-6 d-flex flex-column align-items-center justify-content-center">
            <div class="card mb-3">
              <div class="card-body">
                <div class="pt-4 pb-2">
                  <h5 class="card-title text-center pb-0 fs-4">Đăng nhập quản trị viên</h5>
                  <p class="text-center small">Nhập username và mật khẩu để đăng nhập</p>
                </div>
                <form class="row g-3 needs-validation" @submit.prevent="submit">
                  <div class="col-12">
                    <label for="yourUsername" class="form-label">Username</label>
                    <input v-model="data.username" type="text" class="form-control" :class="{ 'is-invalid': errors.username }"  @input="onChange('username')">
                    <p v-if="errors.username" class="text-danger label"> {{ errors.username }}</p>
                  </div>

                  <div class="col-12">
                    <label for="yourPassword" class="form-label">Password</label>
                    <input v-model="data.password" type="password" class="form-control " :class="{ 'is-invalid': errors.password }"  @input="onChange('password')">
                    <p v-if="errors.password" class="text-danger label"> {{ errors.password }}</p>
                  </div>
                  <div class="col-12">
                    <button class="btn btn-primary w-100" type="submit">Đăng nhập</button>
                  </div>
                </form>
              </div>
            </div>
          </div>
        </div>
      </div>

    </section>

  </div>
</template>
<script>
import {useToast} from "vue-toastification";
import {reactive} from "vue";
import {useStore} from "vuex";
import {useRouter} from "vue-router";
import AuthService from "@/services/auth.service.js";
import axios from "axios";

export default {
  name: "LoginView",
  mounted() {
    document.title = 'Đăng nhập - Quản trị viên'
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
      if (!data.username) errors.username = 'Vui lòng nhập username.';
      if (!data.password) errors.password = 'Vui lòng nhập mật khẩu.';
      if (Object.values(errors).every(error => !error)) {
        AuthService.login(data)
            .then(async response => {
              if (response && response.status === 200) {
                sessionStorage.setItem('token', response.data.token);
                axios.defaults.headers.common['Authorization'] = `Bearer ${response.data.token}`;
                await store.dispatch('setAuth', true);
                toast.success("Đăng nhập thành công.")
                console.log(sessionStorage.getItem('token'))
                await router.push('/');
              }
            })
            .catch(error => {
              console.log(error)
              if (error.response) {
                toast.error(error.response.data.detail);
              }
              else {
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

</style>