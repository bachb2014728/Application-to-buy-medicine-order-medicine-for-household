import './assets/main.css'
import './assets/custom.css'
import './assets/css/upload-file.css'
import './assets/css/theme-default.css'
import router from './router'
import store from "@/store/index.js";
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import { createApp } from 'vue'
import App from './App.vue'
import AuthService from "@/services/auth.service.js";
import Toast, { POSITION } from 'vue-toastification';
import  'vue-toastification/dist/index.css'
import axios from "axios";
import CKEditor from '@ckeditor/ckeditor5-vue';

async function checkAuthentication() {
    const token = sessionStorage.getItem('token')
    if (token != null ) {
        await AuthService.profileAdmin().then(async response => {
            await store.dispatch('setMessage', response.data.username);
            await store.dispatch('setAuth', true);
        }).catch(async error => {
            sessionStorage.removeItem('token');
            axios.defaults.headers.common['Authorization'] = '';
            await store.dispatch('setMessage', '')
            await store.dispatch('setAuth', false);
            await router.push('/login');
        })
    } else {
        await store.dispatch('setAuth', false);
    }
}
checkAuthentication().then(() => {
    createApp(App).use(store).use(CKEditor).use(Toast, {
        position: POSITION.TOP_RIGHT
    }).use(router).mount('#app');
});
