import { createRouter, createWebHistory } from 'vue-router'

const routes = [
    {
        path:"/",
        component: () => import('@/views/HomePage.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/categories",
        component:() => import('@/views/categories/CategoryList.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/categories/create",
        component:() => import('@/views/categories/CategoryCreate.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/categories/:url",
        component:() => import('@/views/categories/CategoryUpdate.vue'),
        meta: {
            requiresAuth: true
        }
    },
    {
        path: "/manufacturers",
        component: () => import('@/components/ManufacturerComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path:'',
                component: () => import('@/views/categories/manufacturers/ManufacturerList.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà sản xuất' }
                    ]
                }
            },
            {
                path: ':id',
                component: () => import('@/views/categories/manufacturers/ManufacturerOverview.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà sản xuất ' , href: '/manufacturers' },
                        { name: 'Xem chi tiết nhà sản xuất'}
                    ]
                }
            }
        ]
    },
    {
        path: "/uses",
        component: () => import('@/components/UseComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path:'',
                component: () => import('@/views/categories/uses/UseList.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Công dụng' }
                    ]
                }
            },
            {
                path: ':id',
                component: () => import('@/views/categories/uses/UseOverview.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Công dụng ' , href: '/uses' },
                        { name: 'Xem chi tiết công dụng'}
                    ]
                }
            }
        ]
    },
    {
        path: "/dosage-forms",
        component: () => import('@/components/DosageFormComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path:'',
                component: () => import('@/views/categories/dosage-forms/DosageFormList.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Dạng bào chế' }
                    ]
                }
            },
            {
                path: ':id',
                component: () => import('@/views/categories/manufacturers/ManufacturerOverview.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà sản xuất ' , href: '/manufacturers' },
                        { name: 'Xem chi tiết nhà sản xuất'}
                    ]
                }
            }
        ]
    },
    {
        path: "/contraindications",
        component: () => import('@/components/ContraindicationComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path:'',
                component: () => import('@/views/categories/contraindications/ContraindicationList.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Chống chỉ định' }
                    ]
                }
            },
            {
                path: ':id',
                component: () => import('@/views/categories/contraindications/ContraindicationOverview.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Chống chỉ định ' , href: '/contraindications' },
                        { name: 'Xem chi tiết chống chỉ định'}
                    ]
                }
            }
        ]
    },
    {
        path: "/products",
        component: () => import('@/components/ProductComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path: '',
                component:()=>import('@/views/products/ProductList.vue'),
                meta: {
                    breadcrumb: [{name:'Sản phẩm'}]
                }
            },
            {
                path: 'create',
                component:()=> import('@/views/products/ProductCreate.vue'),
                meta: {
                    breadcrumb: [
                        {name: 'Sản phẩm',href: '/products'},
                        {name:'Thêm mới sản phẩm'}
                    ]
                }
            },
            {
                path: ':url',
                component:()=>import('@/views/products/ProductOverview.vue'),
                meta: {
                    breadcrumb: [
                        {name: 'Sản phẩm',href: '/products'},
                        {name:'Xem chi tiết sản phẩm'}
                    ]
                }

            },
            {
                path: ':id/update',
                component:()=>import('@/views/products/ProductUpdate.vue'),
                meta: {
                    breadcrumb: [
                        {name: 'Sản phẩm',href: '/products'},
                        {name:'Cập nhật sản phẩm'}
                    ]
                }

            }
        ]
    },
    {
        path:"/vouchers",
        component:()=>import('@/components/VoucherComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path: '',
                component:()=>import('@/views/vouchers/VoucherList.vue'),
                meta: {
                    breadcrumb: [{name:'Khuyến mãi'}]
                }
            },
            {
                path: 'create',
                component:()=>import('@/views/vouchers/VoucherCreate.vue'),
                meta: {
                    breadcrumb: [
                        {name:'Khuyến mãi',href: '/vouchers'},
                        {name:'Thêm khuyến mãi'}

                    ]
                }
            },
            {
                path: ':id/update',
                component:()=> import('@/views/vouchers/VoucherUpdate.vue'),
                meta: {
                    breadcrumb: [
                        {name: 'Khuyến mãi', href:'/vouchers'},
                        {name: 'Cập nhật mã khuyến mãi'}
                    ]
                }
            },
            {
                path: ':id',
                component:()=> import('@/views/vouchers/VoucherOverview.vue'),
                meta: {
                    breadcrumb:[
                        {name: 'Khuyến mãi',href:'/vouchers'},
                        {name: 'Xem chi tiết mã khuyến mãi'}
                    ]
                }
            }
        ]
    },
    {
        path: "/accounts",
        component:()=>import('@/components/AccountComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path: '',
                component:()=>import('@/views/accounts/AccountList.vue'),
                meta: {
                    breadcrumb: [{name : 'Tài khoản người dung'}]
                }
            }
        ]
    },
    {
        path:"/stores",
        component: () => import('@/components/StoreComponent.vue'),
        meta: {requiresAuth: true},
        children: [
            {
                path:'',
                component: () => import('@/views/stores/StorePage.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' }
                    ]
                }
            },
            {
                path:'my-store',
                component: () => import('@/views/stores/MyStore.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' , href: '/stores' },
                        { name: 'Nhà thuốc của tôi'}
                    ]
                }
            },
            {
                path:'list-store',
                component: () => import('@/views/stores/StoreList.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' , href: '/stores' },
                        { name: 'Nhà thuốc cá nhân'}
                    ]
                }
            },
            {
                path: 'create',
                component: () => import('@/views/stores/StoreCreate.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' , href: '/stores' },
                        { name: 'Nhà thuốc của tôi', href: '/stores/my-store'},
                        { name: 'Thêm nhà thuốc'}
                    ]
                }

            },
            {
                path: 'my-store/:url/update',
                component: () => import('@/views/stores/StoreUpdate.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' , href: '/stores' },
                        { name: 'Nhà thuốc của tôi', href: '/stores/my-store'},
                        { name: 'Cập nhật nhà thuốc'}
                    ]
                }
            },
            {
                path: ':url',
                component: () => import('@/views/stores/StoreOverview.vue'),
                meta: {
                    breadcrumb: [
                        { name: 'Nhà thuốc' , href: '/stores' },
                        { name: 'Nhà thuốc cá nhân', href: '/stores/list-store'},
                        { name: 'Xem chi tiết nhà thuốc'}
                    ]
                }
            }
        ]

    },

    {path: "/login",component: () => import('@/views/Login.vue')},
]

let router = createRouter({
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
    return !!sessionStorage.getItem('token');
}