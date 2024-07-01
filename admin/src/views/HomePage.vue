<template>
  <main id="main" class="main">
    <div class="pagetitle">
      <h1>Thống kê</h1>
      <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/" style="text-decoration: none">Trang chủ</router-link></li>
        </ol>
      </nav>
    </div>
    <section class="section">
      <div class="row">
        <div class="col">
          <div class="card"  v-if="stores && stores.length">
            <div class="card-body">
              <h5 class="card-title">Ứng dụng</h5>
              <div class="d-flex align-items-center">
                <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                  <i class="bi bi-cart"></i>
                </div>
                <div class="ps-3">
                  <h6>Số nhà thuốc : {{stores.length}}</h6>
                  <span class="text-muted small pt-2 ps-1">Số khách hàng : </span>
                  <span class="text-success small pt-1 fw-bold">{{customers.length}}</span>
                </div>
              </div>
            </div>

          </div>
        </div>
        <div class="col">
          <div class="card ">
            <div class="card-body">
              <h5 class="card-title">Doanh thu</h5>
              <div class="d-flex align-items-center">
                <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                  <i class="bi bi-currency-dollar"></i>
                </div>
                <div class="ps-3">
                  <h6>Tổng tiền : {{orders.reduce((acc1,order)=> acc1 + order.reduce((acc2,item)=> acc2 + item.totalPrice,0) , 0)}}</h6>
                  <span class="text-muted small pt-2 ps-1">Số hóa đơn : </span>
                  <span class="text-success small pt-1 fw-bold">{{orders.reduce((acc,item) => acc + item.length,0)}}</span>
                </div>
              </div>
            </div>

          </div>
        </div>
        <div class="col">
          <div class="card">
            <div class="card-body">
              <h5 class="card-title">Sản phẩm</h5>
              <div class="d-flex align-items-center">
                <div class="card-icon rounded-circle d-flex align-items-center justify-content-center">
                  <i class="bi bi-people"></i>
                </div>
                <div class="ps-3">
                  <h6>Số sản phẩm : {{products.length}}</h6>
                  <span class="text-muted small pt-2 ps-1">Số lượng còn lại : </span>
                  <span class="text-danger small pt-1 fw-bold">{{products.reduce((acc,product) => acc+product.quantity,0)}}</span>
                </div>
              </div>

            </div>
          </div>

        </div>
      </div>
    </section>
  </main>

</template>
<script>
import {onMounted, ref} from "vue";
import StoreService from "@/services/store.service.js";
import OrderService from "@/services/order.service.js";
import ProductService from "@/services/product.service.js";
import CustomerService from "@/services/customer.service.js";

export default {
  name: "HomePage",
  mounted() {
    document.title='Thống kê'
  },
  setup(){
    const products = ref([])
    const images = ref([])
    const orders = ref([])
    const stores = ref()
    const customers =ref([]);
    onMounted(async () =>{
      StoreService.getAll().then(res=>{
        stores.value =res.data
        for(let store of res.data){
          OrderService.getAllOrderOfStore(store.id).then(response=>{
            orders.value.push(response.data.filter(item=>item.orderStatus === 2))
          })
        }
      })
      ProductService.getAll().then(res=>{
        products.value = res.data
      })
      CustomerService.getAll().then(response=>{
        customers.value=response.data
      })
    })
    return{products,images,orders,stores,customers}
  }
}
</script>

<style scoped>

</style>