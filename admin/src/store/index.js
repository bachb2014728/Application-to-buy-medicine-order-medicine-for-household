import { createStore } from 'vuex'

export default createStore({
    state: {
        // auth: JSON.parse(sessionStorage.getItem('auth')) || false,
        // message: sessionStorage.getItem('message') || ''
        auth: false,
        message: ''
    },
    mutations: {
        setAuth(state, auth){
            state.auth = auth;
            sessionStorage.setItem('auth', JSON.stringify(auth));
        },
        setMessage(state, value) {
            state.message = value;
            sessionStorage.setItem('message', value);
        },
    },
    actions: {
        setAuth({ commit }, auth){
            commit('setAuth',auth);
        },
        setMessage({ commit }, value) {
            commit('setMessage', value);
        },
    },
    modules: {
    }
})