<template>
  <div class="row" v-if="products">
    <div class="col-sm-6 order-0">
      <div class="card">
        <div class="card-header d-flex align-items-center justify-content-between pb-0">
          <div class="card-title mb-0">
            <h5 class="m-0 me-2">Doanh thu bán hàng</h5>
            <small class="text-muted">
              Số lượng sản phẩm bán ra : {{products.reduce((accumulator, product) => accumulator + product.totalQuantity, 0)}}
            </small>
          </div>
          <div class="dropdown">
            <button class="btn p-0" type="button" id="orederStatistics" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <i class="bx bx-dots-vertical-rounded"></i>
            </button>
            <div class="dropdown-menu dropdown-menu-end" aria-labelledby="orederStatistics">
              <a class="dropdown-item" href="javascript:void(0);">Refresh</a>
            </div>
          </div>
        </div>
        <div class="card-body">
          <div class="d-flex justify-content-between align-items-center mb-3" style="position: relative;">
            <h5 class="m-3">
              Tổng tiền : {{orders.reduce((accumulator, product) => accumulator + product.totalPrice, 0)}}
            </h5>
          </div>
          <ul class="p-0 m-0">
            <li class="d-flex mb-4 pb-1">
              <div class="avatar flex-shrink-0 me-3">
                <span class="avatar-initial rounded bg-label-primary"><i class="bx bx-mobile-alt"></i></span>
              </div>
              <div class="d-flex w-100 flex-wrap align-items-center justify-content-between gap-2">
                <div class="me-2">
                  <h6 class="mb-0">Sản phẩm</h6>
                  <small class="text-muted">
                    Có {{products.length}} loại sản phẩm
                  </small>
                </div>
                <div class="user-progress">
                  <small class="fw-medium">{{products.reduce((accumulator, product) => accumulator + product.product.quantity, 0)}}</small>
                </div>
              </div>
            </li>
            <li class="d-flex mb-4 pb-1">
              <div class="avatar flex-shrink-0 me-3">
                <span class="avatar-initial rounded bg-label-success"><i class="bx bx-closet"></i></span>
              </div>
              <div class="d-flex w-100 flex-wrap align-items-center justify-content-between gap-2" v-if="customerIsBest">
                <div class="me-2">
                  <h6 class="mb-0">Khách hàng mua nhiều nhất</h6>
                  <small class="text-muted">Số lần mua (kể cả đợi đang vận chuyển) : {{customerIsBest.count}}</small>
                </div>
                <div class="user-progress">
                  <small class="fw-medium">{{customerIsBest.customer.firstName}} {{customerIsBest.customer.lastName}}</small>
                </div>
              </div>
            </li>
            <li class="d-flex pb-1">
              <div class="avatar flex-shrink-0 me-3">
                <span class="avatar-initial rounded bg-label-info"><i class="bx bx-home-alt"></i></span>
              </div>
              <div class="d-flex w-100 flex-wrap align-items-center justify-content-between gap-2">
                <div class="me-2">
                  <h6 class="mb-0">Khuyến mãi</h6>
                  <small class="text-muted">Có 2 loại khuyến mãi </small>
                </div>
                <div class="user-progress">
                  <small class="fw-medium">{{vouchers.length}}</small>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
    <div class="col-sm-6 order-2 mb-4">
      <div class="card">
        <div class="card-header d-flex align-items-center justify-content-between">
          <h5 class="card-title m-0 me-2">Doanh thu sản phẩm</h5>
          <div class="dropdown">
            <button class="btn p-0" type="button" id="transactionID" data-bs-toggle="dropdown" aria-haspopup="true" aria-expanded="false">
              <i class="bx bx-dots-vertical-rounded"></i>
            </button>
            <div class="dropdown-menu dropdown-menu-end" aria-labelledby="transactionID">
              <a class="dropdown-item" href="javascript:void(0);">Trong 1 tháng</a>
              <a class="dropdown-item" href="javascript:void(0);">Trong 1 tuần</a>
              <a class="dropdown-item" href="javascript:void(0);">Trong ngày</a>
            </div>
          </div>
        </div>
        <div class="card-body">
          <ul class="p-0 m-0">
            <li class="d-flex mb-4 pb-1" v-for="(item,index) in products" :key="index">
              <div class="avatar flex-shrink-0 me-3" v-if="images.some(image=>image.productId === item.product.id)">
                <img :src="`data:image/jpeg;base64,`+images.find(image=>image.productId === item.product.id)['image'].file" alt="User" class="rounded">
              </div>
              <div class="d-flex w-100 flex-wrap align-items-center justify-content-between gap-2">
                <div class="me-2">
                  <small class="text-muted d-block mb-1">{{item.product.name}}</small>
                  <span class="mb-0">Số lượng còn lại : {{item.product.quantity}}</span>
                </div>
                <div class="user-progress d-flex align-items-center gap-1">
                  <h6 class="mb-0">{{item.totalPrice}}</h6>
                  <span class="text-muted">VND</span>
                </div>
              </div>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import ProductService from "@/services/product.service.js";
import ImageService from "@/services/image.service.js";
import OrderService from "@/services/order.service.js";
import StoreService from "@/services/store.service.js";
import CustomerService from "@/services/customer.service.js";
import VoucherService from "@/services/voucher.service.js";

export default {
  name: "StoreStatisticalManager",
  mounted() {
    document.title='Thống kê cửa hàng'
  },
  setup(){
    const products = ref([])
    const url = ref()
    const images = ref([])
    const orders = ref([])
    const store = ref()
    const vouchers = ref([])
    const customerIsBest = ref()
    const customers = new Set();
    const route = useRoute()
    onMounted(async () =>{
      url.value = route.params.url;
      StoreService.getOneByURL(url.value).then(res=>{
        store.value = res.data
        OrderService.getAllOrderOfStore(res.data.id).then(response=>{
          orders.value = response.data.filter(item=>item.orderStatus === 2)
          response.data.forEach(obj => {
            customers.add(obj.customerId);
          });
          const customerIdCounts = {};

          response.data.forEach(obj => {
            const { customerId } = obj;
            if (customerIdCounts[customerId]) {
              customerIdCounts[customerId]++;
            } else {
              customerIdCounts[customerId] = 1;
            }
          });

          let maxCount = 0;
          let mostFrequentCustomerId = null;

          for (const customerId in customerIdCounts) {
            if (customerIdCounts[customerId] > maxCount) {
              maxCount = customerIdCounts[customerId];
              mostFrequentCustomerId = customerId;
            }
          }
          CustomerService.getOne(mostFrequentCustomerId).then(resCus=>{
            customerIsBest.value = {
              customer : resCus.data,
              count: maxCount
            }
          })

          ProductService.getAllByStore(url.value).then(resProduct =>{
            for (let item of resProduct.data){
              ImageService.getOne(item.imageId).then(res=>{
                images.value.push({image:res.data,productId : item.id})
              })
            }
            for (let item of resProduct.data){
              let obj = {product: item, totalPrice: 0, totalQuantity:0 }
              for (let order of response.data.filter(item=>item.orderStatus === 2)){
                for (let orderItem of order.products){
                  if (item.id === orderItem.productId){
                    obj.totalPrice += orderItem.totalPrice;
                    obj.totalQuantity += orderItem.totalQuantity;
                  }
                }
              }
              products.value.push(obj)
            }
          })
          VoucherService.getAllVoucherOfStore(res.data.id).then(resVoucher=>{
            vouchers.value = resVoucher.data
          })
        })
      })
    })
    return{products,images,orders,customers, customerIsBest,vouchers}
  }

}
</script>
<style scoped>

</style>