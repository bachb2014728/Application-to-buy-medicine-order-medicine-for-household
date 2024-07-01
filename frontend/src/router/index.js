import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {path:"/",component: () => import('@/views/Home.vue')},
    {path: "/register", component: () => import('@/views/RegisterView.vue')},
    {path: "/login",component: () => import('@/views/LoginView.vue')},
    {
        path: "/products",
        component:() => import('@/views/products/ProductList.vue')
    },
    {
        path: "/products/:productURL",
        component:() => import('@/views/products/ProductDetail.vue')
    },
    {
        path: "/stores",
        component:()=>import('@/components/StoreComponent.vue'),
        meta: {requiresAuth: true},
        children:[
            {
                path:"",
                component:()=> import('@/views/stores/StoreOverview.vue'),
                meta:{
                    breadcrumb: [{name: 'Cửa hàng'},]
                }
            },
            {
                path: "create",
                component:() => import('@/views/stores/NewStore.vue'),
                meta:{
                    breadcrumb: [
                        {name: 'Cửa hàng', href:'/stores'},
                        {name: 'Thêm cửa hàng'}
                    ]
                }
            },
            {
                path: ":url",
                component:()=>import('@/components/StoreManagerComponent.vue'),
                meta:{
                    breadcrumb: [
                        {name: 'Cửa hàng', href:'/stores'},
                        {name: 'Quản lý cửa hàng'}
                    ]
                },
                children:[
                    {
                        path:"overview",
                        component:()=>import('@/views/stores/StoreOverviewManager.vue')
                    },
                    {
                        path: "members",
                        component:()=>import('@/views/stores/StoreCustomerManager.vue')
                    },
                    {
                        path: "members/:customerId",
                        component:()=>import('@/views/stores/CustomerDetail.vue')
                    },
                    {
                        path:"orders",
                        component:()=>import('@/views/stores/StoreOrderManager.vue')
                    },
                    {
                        path: "statistical",
                        component:()=>import('@/views/stores/StoreStatisticalManager.vue')
                    },
                    {
                        path:"order-confirm",
                        component:()=>import('@/views/stores/StoreOrderConfirmManager.vue')
                    },
                    {
                        path: "notification",
                        component:()=>import('@/views/stores/StoreNotificationManager.vue')
                    },
                    {
                        path: "vouchers",
                        component:()=>import('@/views/stores/StoreVoucherManager.vue')
                    },
                    {
                        path: "vouchers/create",
                        component:()=>import('@/views/vouchers/VoucherCreate.vue'),
                    },
                    {
                        path: "products",
                        component:()=>import('@/views/stores/StoreProductManager.vue')
                    },
                    {
                        path: "products/create",
                        component: () => import('@/views/products/ProductCreate.vue')
                    },
                    {
                        path: "products/:product/update",
                        component: () => import('@/views/products/ProductUpdate.vue')
                    },
                ]
            },
        ]
    },
    {
        path: "/vouchers",
        component:()=>import('@/views/vouchers/VoucherList.vue')
    },
    {
        path: "/carts",
        name: "Cart",
        component:()=>import('@/views/Cart.vue'),
        meta: {requiresAuth: true}
    },
    {
        path: "/orders",
        name: 'OrderPage',
        component:()=> import('@/views/OrderPage.vue'),
        props: true
    },
    {
        path: "/notification-after-order",
        component:()=>import('@/views/NotificationAfterOrder.vue'),
        meta: {requiresAuth: true}
    },
    {
        path: "/account",
        component:() => import('@/components/AccountManagerComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path: "profile",
                component:()=>import('@/views/accounts/AccountManage.vue'),
            },
            {
                path: "notification",
                component:()=>import('@/views/accounts/Notification.vue'),
            },
            {
                path: "order-manage",
                component:()=>import('@/views/accounts/OrderManage.vue'),
            },
            {
                path: "receivers",
                component:()=>import('@/views/accounts/ReceiverManage.vue'),
            },
            {
                path: "comments",
                component:()=>import('@/views/accounts/CommentManager.vue')
            }
        ]
    },
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