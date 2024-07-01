<template>
  <div class="card">
    <h5 class="card-header">Danh sách khách hàng</h5>
    <div class="card-body">
      <div class="table-responsive text-nowrap">
        <table class="table table-bordered table-sm">
          <thead>
          <tr>
            <th>stt</th>
            <th>Tên khách hàng</th>
            <th>phone</th>
            <th>địa chỉ</th>
            <th>orders</th>
            <th>products</th>
            <th>tổng tiền</th>
            <th>Actions</th>
          </tr>
          </thead >
          <tbody>
          <tr v-if="customers && customers.length" v-for="(customer,index) in customers" :key="index">
            <td>{{index+1}}</td>
            <td>
              <span class="fw-medium">{{customer.customer.firstName}} {{customer.customer.lastName}}</span>
            </td>
            <td>{{customer.customer.phoneNumber}}</td>
            <td>{{customer.customer.address}}</td>
            <td>{{customer.orders.length}}</td>
            <td>{{customer.orders.reduce((accumulator, order) => accumulator + order.products.reduce((acc, product) => acc + product.totalQuantity, 0), 0)}}</td>
            <td>{{customer.orders.reduce((accumulator, order) => accumulator + order.totalPrice, 0)}}</td>
            <td>
              <div class="dropdown">
                <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                  <i class="bx bx-dots-vertical-rounded"></i>
                </button>
                <div class="dropdown-menu">
                  <router-link :to="`/stores/${url}/members/${customer.customer.id}`" class="dropdown-item" ><i class="bx bx-show me-1"></i> Xem chi tiết</router-link>
                  <a class="dropdown-item"><i class="bx bx-trash me-1"></i>Tạm khóa</a>
                </div>
              </div>
            </td>
          </tr>
          <tr v-else class="text-center">
            <td>Không có khách hàng nào cả.</td>
          </tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import OrderService from "@/services/order.service.js";
import StoreService from "@/services/store.service.js";
import CustomerService from "@/services/customer.service.js";

export default {
  name: "StoreCustomerManager",
  mounted() {
    document.title='Quản lý khách hàng'
  },
  setup(){
    const url = ref()
    const route = useRoute()
    const orders = ref([])
    const customers = ref([])
    onMounted(async =>{
      url.value = route.params.url
      StoreService.getOneByURL(url.value).then(resStore =>{
        OrderService.getAllOrderOfStore(resStore.data.id).then(resOrder=>{
          const uniqueIds = new Set();
          resOrder.data.forEach(obj => {
            uniqueIds.add(obj.customerId);
          });
          const customerId = Array.from(uniqueIds);
          for (let id of customerId){
            CustomerService.getOne(id).then(resCus=>{
              const item = {
                customer : resCus.data,
                orders : resOrder.data.filter(ord=>ord.customerId === id)
              }
              customers.value.push(item)
            })
          }
        })
      })

    })
    return{customers,url}
  }
}
</script>
<style scoped>

</style>