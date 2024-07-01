<template>
  <div class="row">
    <div class="card col-sm-8">
      <h5 class="card-header">Danh sách yêu cầu đơn hàng</h5>
      <div class="card-body">
        <div class="table-responsive text-nowrap">
          <table class="table table-bordered table-sm">
            <thead>
            <tr>
              <th>stt</th>
              <th>Tổng phụ</th>
              <th>mã khuyến mãi</th>
              <th>tổng tiền</th>
              <th>Thông báo</th>
              <th>action</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(order,index) in orders" :key="index" v-if="orders && orders.length" @click="selectItem(order)">
              <td>{{index+1}}</td>
              <td>{{formatPrice(order.products.reduce((accumulator, product) => accumulator + product.totalPrice, 0))}}</td>

              <td>
                <li class="nav-link" v-for="(voucher,vou) in order.vouchers" :key="vou">
                  <span class="badge bg-label-secondary">{{voucher.title}}</span>
                </li>
              </td>
              <td>{{formatPrice(order.totalPrice)}}</td>
              <td>

              </td>
              <td>
                <div class="dropdown">
                  <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown" aria-expanded="false">
                    <i class="bx bx-dots-vertical-rounded"></i>
                  </button>
                  <div class="dropdown-menu">
                    <a class="dropdown-item" @click="acceptOrder(order)">
                      <i class='bx bxs-quote-single-left'></i>Duyệt đơn hàng
                    </a>
                    <a class="dropdown-item" @click="cancelOrder(order)">
                      <i class="bx bx-trash me-1"></i>Từ chối đơn hàng
                    </a>
                  </div>
                </div>
              </td>
            </tr>
            <tr v-else>
              <td colspan="6" class="text-center">Không có đơn hàng nào cả.</td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="col-sm-4" v-if="selectedItem">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Thông tin đơn hàng
            <span class="badge bg-label-success">{{formatDate(selectedItem.createdOn)}}</span>
          </h5>
          <div class="card-subtitle text-muted mb-3" v-if="customers && customers.length">
            Đơn hàng tạo bởi {{customers.find(item=>item.orderId === selectedItem.id)['customer'].firstName}}
             {{customers.find(item=>item.orderId === selectedItem.id)['customer'].lastName}}
          </div>
          <div class="card-text">
            <span>Thông tin nhận : </span>
            <ul>
              <li>Tên người nhận : {{selectedItem.receiverName}}</li>
              <li>Số điện thoại nhận : {{selectedItem.receiverPhone}}</li>
              <li>Địa chỉ nhận : {{selectedItem.receiverAddress}}</li>
            </ul>
          </div>
          <div class="card-text">
            <span>Sản phẩm : </span>
            <ul style="max-height: 6rem; overflow: auto;">
              <li v-for="(product,index) in selectedItem.products">
                <span>{{product.productName}}</span><br>
                <span>Giá tiền : {{formatPrice(product.price)}}</span><br>
                <span>Số lượng : {{product.totalQuantity}}</span>
              </li>
            </ul>
          </div>
          <div class="card-text">Thanh toán : {{selectedItem.payment}}</div>
          <div class="card-text mb-2">Trạng thái đơn hàng :
            <span v-if="selectedItem.orderStatus === 0" class="badge bg-label-primary">Chờ xác nhận</span>
            <span v-if="selectedItem.orderStatus === 1" class="badge bg-label-warning">Đang vận chuyển</span>
            <span v-if="selectedItem.orderStatus === 2" class="badge bg-label-success">Đã giao hàng</span>
            <span v-if="selectedItem.orderStatus === 3" class="badge bg-label-danger">Chờ xác nhận</span>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import {formatDate} from "@/utils/time.utils.js";
import {formatPrice} from "@/utils/convert.utils.js";
import StoreService from "@/services/store.service.js";
import OrderService from "@/services/order.service.js";
import CustomerService from "@/services/customer.service.js";
import {useToast} from "vue-toastification";
import VoucherService from "@/services/voucher.service.js";

export default {
  name: "StoreOrderConfirmManager",
  mounted() {
    document.title='Quản lý đơn hàng'
  },
  setup(){
    const url = ref()
    const toast =useToast()
    const route =useRoute()
    const customers = ref([])
    const selectedItem = ref()
    const orders =ref([])
    onMounted(async ()=>{
      url.value = route.params.url;
      StoreService.getOneByURL(url.value).then(response=>{
        OrderService.getAllOrderOfStore(response.data.id).then(res=>{
          orders.value = res.data.filter(item=>item.orderStatus === 0)
          for (let item of res.data){
            CustomerService.getOne(item.customerId).then(cus=>{
              customers.value.push({customer:cus.data,orderId:item.id})
            })
          }
          selectedItem.value = orders.value[0]
        })

      })
    })

    return{orders,selectedItem,customers,toast}
  },
  methods:{
    selectItem(order){
      this.selectedItem = order;
    },
    formatDate(date){return formatDate(date)},
    formatPrice(number){return formatPrice(number)},
    acceptOrder(order){
      let index = this.orders.findIndex(item=>item.id === order.id);
      if (index !== -1){
        const formData ={orderId: order.id , orderStatus: 1}
        OrderService.changeStatusOrder(order.id,formData).then(response=>{
          this.orders.splice(index,1);
          if (this.orders.length === 0){
            this.selectedItem = null;
          }else{
            this.selectedItem = this.orders[0]
          }
          this.toast.success("Xét duyệt thành công");
        })
      }else{
        this.toast.warning("Không thể xét duyệt đơn hàng.")
      }
    },
    cancelOrder(item) {
      const userInput = window.prompt('Nhập lý do hủy đơn hàng :');
      if (userInput !== null) {
        if (window.confirm('Bạn có chắc chắn hủy đơn này này không ?')) {
          this.deleteItem(item, userInput);
        }
      } else {
        console.log('Người dùng đã hủy xóa.');
      }
    },
    async deleteItem(item,userInput) {
      let index = this.orders.indexOf(item);
      if (index > -1) {
        OrderService.cancelOrder(item.id).then(response=>{
          this.orders.splice(index,1);
          if (this.orders.length === 0){
            this.selectedItem = null;
          }else{
            this.selectedItem = this.orders[0]
          }
          this.toast.success("Hủy đơn hàng thành công");
        })
      }else{
        this.toast.error("Xảy ra lỗi khi xóa.")
      }
    },
  }
}
</script>
<style scoped>

</style>