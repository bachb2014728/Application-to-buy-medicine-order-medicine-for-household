import './assets/main.css'
import router from './router'
import store from "@/store/index.js";
import 'bootstrap/dist/css/bootstrap.min.css'
import 'bootstrap/dist/js/bootstrap.bundle.min.js'
import { createApp } from 'vue'
import App from './App.vue'
import AuthService from "@/services/auth.service.js";
import Toast, { POSITION } from 'vue-toastification'
import  'vue-toastification/dist/index.css'
import axios from "axios";
import CKEditor from '@ckeditor/ckeditor5-vue';

async function checkAuthentication() {
    const token = localStorage.getItem('token');
    if (token != null) {
        await AuthService.profile().then(async response => {
            if(response.data.roleName === 'CUSTOMER'){
                await store.dispatch('setMessage', response.data.firstName + " "+ response.data.lastName);
            }else{
                await store.dispatch('setMessage', response.data.name);
            }
            await store.dispatch('setAuth', true);
            await store.dispatch('setRole',response.data.roleName);
        }).catch(async error => {
            localStorage.removeItem('token');
            axios.defaults.headers.common['Authorization'] = '';
            await store.dispatch('setMessage', '')
            await store.dispatch('setRole', '')
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
