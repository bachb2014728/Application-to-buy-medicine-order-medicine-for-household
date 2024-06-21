import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {path:"/",component: () => import('@/views/Home.vue')},
    {path: "/register", component: () => import('@/views/RegisterView.vue')},
    {path: "/login",component: () => import('@/views/LoginView.vue')},
    {
        path: "/store/create",
        component:() => import('@/views/stores/NewStore.vue'),
        meta: {
            requiresAuth: true
        }
    }
]

const router = createRouter({
    history: createWebHistory(import.meta.env.BASE_URL),
    routes
})
router.beforeEach((to, from, next) => {
    if (to.matched.some(record => record.meta.requiresAuth)) {
        if (!isLoggedIn()) {
            next({ path: '/login' });
        } else {
            next();
        }
    } else {
        next();
    }
});

export default router;

function isLoggedIn() {
    return !!localStorage.getItem('token');
}