<template>
  <nav>
    <ol class="breadcrumb">
      <li class="breadcrumb-item">
        <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
      </li>
      <li class="breadcrumb-item">Giỏ hàng</li>
    </ol>
  </nav>
  <div class="card">
    <div class="table-responsive text-nowrap" style="max-height: 20rem">
      <table class="table table-bordered">
        <thead>
        <tr>
          <th>tên sản phẩm</th>
          <th>Giá tiền</th>
          <th>Số lượng</th>
          <th>Cửa hàng</th>
          <th>Chọn</th>
          <th></th>
        </tr>
        </thead>
        <tbody v-if="carts">
        <tr v-for="(item,index) in carts" :key="index" v-if="stores">
          <td><span class="fw-medium">{{item.name}}</span></td>
          <td>{{item.price}}</td>
          <td>{{item.quantity}}</td>
          <td v-if="(stores.find(store=>store.cart.id === item.id))">
            <ul class="list-unstyled users-list m-0 avatar-group d-flex align-items-center">
              <li data-bs-toggle="tooltip" data-popup="tooltip-custom" data-bs-placement="top" class="avatar avatar-xs pull-up"
                  :aria-label="stores" :data-bs-original-title="stores">
                <img src="@/assets/images/multiple-user.png" alt="Avatar" class="rounded-circle">
              </li>
              {{(stores.find(store=>store.cart.id === item.id)).store.name}}
            </ul>
          </td>
          <td>
            <input class="form-check-input" type="checkbox" :value="item.productId" v-model="item.isChecked" @click="AddToOrder(item)">
          </td>
          <td>
            <div class="btn-group" role="group" aria-label="Basic example">
              <button @click.prevent="deleteItem(item)" class="btn btn-outline-danger btn-sm me-1">Xóa</button>
              <button @click.prevent="decreaseItem(item)" class="btn btn-outline-warning btn-sm me-1">Giảm</button>
              <button @click.prevent="increaseItem(item)" class="btn btn-sm btn-outline-success">Tăng</button>
            </div>
          </td>
        </tr>
        </tbody>
      </table>
    </div>
  </div>
  <div class="text-center m-3">
    <router-link to="/orders" class="btn btn-sm btn-success">Tạo hóa đơn</router-link>
  </div>
</template>

<script>
import { onMounted, ref } from 'vue';
import CartService from '@/services/cart.service.js';
import { useRouter } from 'vue-router';
import { useToast } from 'vue-toastification';
import StoreService from "@/services/store.service.js";

export default {
  name: 'Cart',
  mounted() {
    document.title='Giỏ hàng'
  },
  setup() {
    const carts = ref([]);
    const toast = useToast();
    const stores = ref([])
    const router = useRouter();

    onMounted(async () => {
      CartService.getAll().then(async response => {
        carts.value = response.data
        for (let item of response.data){
          StoreService.getOne(item.storeId).then(res=>{
            const obj = {store: res.data , cart : item}
            stores.value.push(obj)
          })
        }

      })
    });
    async function increaseItem(item) {
      const data = { cartId: item.id, quantity: 1 };
      CartService.update(item.id,data).then(response=>{
        item.quantity += 1;
        // toast.success('Thêm thành công.');
      }).catch(err=>{
        toast.error('Lỗi thêm sản phẩm.');
      })
    }
    async function deleteItem(item){
      let index = carts.value.indexOf(item);
      CartService.deleteOne(item.id).then(response=>{
        carts.value.splice(index,1)
      }).catch(err=>{
        toast.error("Xóa sản phẩm khỏi giỏ hàng thất bại.")
      })
    }
    async function decreaseItem(item) {
      const data = { cartId: item.id, quantity: -1 };
      let index = carts.value.indexOf(item);
      CartService.update(item.id,data).then(response =>{
        item.quantity -= 1;
        if (item.quantity === 0){
          carts.value.splice(index,1);
        }
      }).catch(err=>{
        toast.error('Lỗi xóa sản phẩm.');
      })
    }

    return { carts,toast ,increaseItem, decreaseItem,router,deleteItem,stores};
  },
  methods:{
    AddToOrder(item){
      const data = {idCart : item.id}
      CartService.checked(data).then(response =>{
        console.log(response.data)
      })
    }
  }
};
</script>

<style scoped>
</style>
