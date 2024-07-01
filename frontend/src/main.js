import 'bootstrap/dist/css/bootstrap.min.css'
import './assets/css/main.css'
import './assets/css/theme.css'

import './assets/css/base.css'

import './assets/js/toast.js'
import router from './router'
import store from "@/store/index.js";

import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import { createApp } from 'vue'
import App from './App.vue'
import Toast, { POSITION } from 'vue-toastification'
import  'vue-toastification/dist/index.css'
import axios from "axios";
import CKEditor from '@ckeditor/ckeditor5-vue';
import AuthService from "@/services/auth.service.js";
import CartService from "@/services/cart.service.js";

async function checkAuthentication() {
    const token = localStorage.getItem('token');
    if (token != null) {
        await AuthService.profile().then(async response => {
            CartService.getAll().then( async resCart => {
                await store.dispatch('setCart', resCart.data.length);
            })
            await store.dispatch('setMessage', response.data.firstName + " "+ response.data.lastName);
            await store.dispatch('setAuth', true);
        }).catch(async error => {
            localStorage.removeItem('token');
            axios.defaults.headers.common['Authorization'] = '';
            await store.dispatch('setMessage', '')
            await store.dispatch('setRole', '')
            await store.dispatch('setCart','')
            await store.dispatch('setAuth', false);
            await router.push('/');
        })
    } else {
        await store.dispatch('setAuth', false);
    }
}
checkAuthentication().then(() => {
    createApp(App).use(CKEditor).use(store).use(Toast, {
        position: POSITION.TOP_RIGHT
    }).use(router).mount('#app');
});
