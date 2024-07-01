<template>
  <div class="row">
    <div class="card col-sm-8">
      <h5 class="card-header">Danh sách đơn hàng</h5>
      <div class="card-body">
        <div class="table-responsive text-nowrap">
          <table class="table table-bordered table-sm">
            <thead>
            <tr>
              <th>stt</th>
              <th>tổng phụ</th>
              <th>khuyến mãi</th>
              <th>tổng tiền</th>
              <th>trạng thái</th>
              <th>action</th>
            </tr>
            </thead>
            <tbody>
            <tr v-for="(order,index) in orders" :key="index" @click="selectItem(order)" v-if="orders && orders.length">
              <td>{{index+1}}</td>
              <td>
                {{formatPrice(order.products.reduce((accumulator, product) => accumulator + product.totalPrice, 0))}}
              </td>
              <td>
                <li class="nav-link" v-for="(voucher,vou) in order.vouchers" :key="vou">
                  <span class="badge bg-label-secondary">{{voucher.title}}</span>
                </li>
              </td>
              <td>{{formatPrice(order.totalPrice)}}</td>
              <td>
                <span v-if="order.orderStatus === 0" class="badge bg-label-info">Chờ xác nhận</span>
                <span v-if="order.orderStatus === 1" class="badge bg-label-warning">Đang vận chuyển</span>
                <span v-if="order.orderStatus === 2" class="badge bg-label-success">Đã giao hàng</span>
                <span v-if="order.orderStatus === 3" class="badge bg-label-danger">Đã hủy</span>
              </td>
              <td>
                <div class="dropdown" v-if="order.orderStatus === 0">
                  <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                    <i class="bx bx-dots-vertical-rounded"></i>
                  </button>
                  <div class="dropdown-menu">
                    <a class="dropdown-item" @click="cancelOrder(order)"><i class="bx bx-trash me-1"></i>Hủy đơn hàng</a>
                  </div>
                </div>
                <div class="dropdown" v-if="order.orderStatus === 3">
                  <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                    <i class="bx bx-dots-vertical-rounded"></i>
                  </button>
                  <div class="dropdown-menu">
                    <a class="dropdown-item" href="javascript:void(0);"><i class="bx bx-edit-alt me-1"></i>Đặt lại đơn này</a>
                    <a class="dropdown-item" href="javascript:void(0);"><i class="bx bx-trash me-1"></i> Xóa đơn hàng</a>
                  </div>
                </div>
              </td>
            </tr>
            <tr v-else class="text-center">
              <td colspan="6">Không có hóa đơn nào.</td>
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
            <span v-if="selectedItem.orderStatus === 3" class="badge bg-label-danger">Đã hủy</span>
          </div>
          <div class="text-center" >
            <button class="btn btn-sm btn-success" type="button" data-bs-toggle="offcanvas" v-if="selectedItem.orderStatus === 0"
                    data-bs-target="#offcanvasBoth" aria-controls="offcanvasBoth" @click="itemUpdate = selectedItem">
              <i class="bx bx-edit-alt me-1"></i>Chỉnh sửa đơn hàng
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
  <div class="offcanvas offcanvas-end" data-bs-scroll="true" tabindex="-1" id="offcanvasBoth"
       aria-labelledby="offcanvasBothLabel" v-if="itemUpdate">
    <div class="offcanvas-header">
      <h5 id="offcanvasBothLabel" class="offcanvas-title">Cập nhật đơn hàng</h5>
      <button type="button" class="btn-close text-reset" data-bs-dismiss="offcanvas" aria-label="Close"></button>
    </div>
    <form class="offcanvas-body my-auto mx-0 flex-grow-0">
      <div class="mb-3" v-if="receivers">
        <span class="fw-medium">Thông tin người nhận</span>
        <div v-for="(receiver, num) in receivers" :key="num" class="form-check mt-3">
          <input name="receiver" class="form-check-input" type="radio" @click = "checkedReceiver(receiver,itemUpdate)"
                 :checked="(receiver.name === itemUpdate.receiverName && receiver.phone===itemUpdate.receiverPhone && receiver.address === itemUpdate.receiverAddress)">

          <label class="form-check-label">
            {{ receiver.name }} - {{ receiver.phone }} - {{ receiver.address }}
          </label>
        </div>
      </div>
      <div class="">
        <button type="button" class="btn btn-sm btn-secondary d-grid w-100" data-bs-dismiss="offcanvas">Cancel</button>
      </div>
    </form>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import OrderService from "@/services/order.service.js";
import {formatPrice} from "@/utils/convert.utils.js";
import {formatDate} from "@/utils/time.utils.js";
import ReceiverService from "@/services/receiver.service.js";
import {useToast} from "vue-toastification";

export default {
  name: "OrderManage",
  mounted() {
    document.title='Quản lý đơn hàng'
  },
  setup(){
    const orders = ref([])
    const toast = useToast()
    const selectedItem = ref()
    const itemUpdate = ref()
    const receivers = ref()
    onMounted(async ()=>{
      OrderService.getAllOfCustomer().then(response=>{
        orders.value = response.data
        selectedItem.value = orders.value[0]
      })
      ReceiverService.getAll().then(response=>{
        receivers.value = response.data
      })
    })
    return{orders,selectedItem,itemUpdate,receivers,toast}
  },
  methods:{
    selectItem(order){
      this.selectedItem = order;
    },
    formatDate(date){return formatDate(date)},
    formatPrice(number){return formatPrice(number)},
    checkedReceiver(receiver,order){
      let index = this.orders.findIndex(item=>item.id === order.id)
      if (index > -1 ){
        const formData = {receiverName:receiver.name,receiverPhone:receiver.phone,receiverAddress: receiver.address}
        OrderService.update(order.id,formData).then(response=>{
          this.orders[index] = ({
            ...order,
            receiverName: receiver.name,receiverPhone:receiver.phone,receiverAddress: receiver.address
          })
          this.selectedItem = this.orders[index]
          this.toast.success("Cập nhật hóa đơn thành công.")
        }).catch(err=>{
          this.toast.error("Cập nhật hóa đơn thất bại.")
        })
      }
    },
    cancelOrder(item) {
      if (window.confirm('Bạn có chắc chắn hủy đơn này này không ?')) {
        this.cancelItem(item);
      }
    },
    async cancelItem(item) {
      let index = this.orders.indexOf(item);
      if (index > -1) {
        OrderService.cancelOrder(item.id).then(response=>{
          this.orders[index] = ({
            ...this.orders[index],
            orderStatus: 0
          })
          this.selectedItem = this.orders[index]
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