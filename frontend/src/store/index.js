import { createStore } from 'vuex'

export default createStore({
    state: {
        auth: false,
        role:'',
        message: ''
    },
    mutations: {
        setAuth(state, auth){
            state.auth = auth;
        },
        setRole(state,value){
            state.role = value;
        },
        setMessage(state, value) {
            state.message = value;
        },
    },
    actions: {
        setAuth({ commit }, auth){
            commit('setAuth',auth);
        },
        setRole({ commit }, value) {
            commit('setRole', value);
        },
        setMessage({ commit }, value) {
            commit('setMessage', value);
        },
    },
    modules: {
    }
})