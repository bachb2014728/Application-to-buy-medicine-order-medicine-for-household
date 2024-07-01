import { createStore } from 'vuex';

export default createStore({
    state: {
        auth: false,
        role: '',
        message: '',
        cart: ''
    },
    mutations: {
        setAuth(state, auth) {
            state.auth = auth;
        },
        setRole(state, value) {
            state.role = value;
        },
        setMessage(state, value) {
            state.message = value;
        },
        setCart(state, value) {
            state.cart = value;
        }
    },
    actions: {
        setAuth({ commit }, auth) {
            commit('setAuth', auth);
        },
        setRole({ commit }, value) {
            commit('setRole', value);
        },
        setMessage({ commit }, value) {
            commit('setMessage', value);
        },
        setCart({ commit }, product) {
            commit('setCart', product);
        }
    },
    modules: {}
});
