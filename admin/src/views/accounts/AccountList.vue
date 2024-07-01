<template>
  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Danh sách</h5>
      <table class="table table-sm">
        <thead>
        <tr>
          <th scope="col">#</th>
          <th scope="col">Tên</th>
          <th scope="col">Số điện thoại</th>
          <th scope="col">Địa chỉ</th>
          <th scope="col">Email</th>
          <th scope="col">Username</th>
          <th scope="col">Action</th>
        </tr>
        </thead>
        <tbody>
        <tr v-if="customers && customers.length" v-for="(customer,index) in customers" :key="index">
          <th scope="row">{{index+1}}</th>
          <td>{{customer.firstName}} {{customer.lastName}}</td>
          <td>{{customer.phoneNumber}}</td>
          <td>{{customer.address}}</td>
          <td>{{customer.email}}</td>
          <td>{{customer.username}}</td>
          <td></td>
        </tr>
        <tr v-else class="text-center">
          <td colspan="7">{{message}}</td>
        </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>
<script>
import CustomerService from "@/services/customer.service.js";
import {onMounted, ref} from "vue";

export default {
  name: "AccountList",
  mounted() {
    document.title='Quản lý tài khoản'
  },
  setup(){
    const customers = ref([])
    const message = ref()
    const orders = ref([])
    onMounted(async ()=>{
      CustomerService.getAll().then(response=>{
        for (let item of response.data){
          CustomerService.getOne(item.id).then(resCus=>{
            customers.value.push(resCus.data)
          })
        }
      }).catch(err=>{
        message.value = err.response.data.detail;
      })
    })
    return{customers,message}
  }
}
</script>
<style scoped>

</style>