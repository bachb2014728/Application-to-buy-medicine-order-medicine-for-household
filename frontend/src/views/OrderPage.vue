<template>
  <div>
    <nav>
      <ol class="breadcrumb">
        <li class="breadcrumb-item">
          <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
        </li>
        <li class="breadcrumb-item">Hóa đơn</li>
      </ol>
    </nav>

    <div class="row mb-3" v-for="(cart, indexCart) in data" :key="indexCart">
      <div class="col-sm-8">
        <span class="fw-medium">Danh sách sản phẩm của hóa đơn {{indexCart+1}}</span>
        <div class="card mt-3 mb-3">
          <div class="table-responsive text-nowrap">
            <table class="table table-sm table-bordered">
              <thead>
              <tr>
                <th>STT</th>
                <th>Tên</th>
                <th>Giá tiền</th>
                <th>số lượng</th>
                <th>Tổng</th>
              </tr>
              </thead>
              <tbody>
              <tr v-for="(item, idx) in cart['cartItems']" :key="item.id">
                <td>{{ idx + 1 }}</td>
                <td>{{ item.name }}</td>
                <td>{{ item.price }}</td>
                <td>{{ item.quantity }}</td>
                <td>{{ item.quantity * item.price }}</td>
              </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div class="card mb-3">
          <div class="table-responsive text-nowrap">
            <table class="table table-bordered">
              <thead>
              <tr>
                <th>Tổng phụ</th>
                <th>{{ cart.subTotal }}</th>
              </tr>
              </thead>
              <tbody>
              <tr>
                <th>Khuyến mãi (nhà thuốc)</th>
                <th class="text-danger" v-if="GetVoucherOfStore(cart) > 0">
                  -{{ GetVoucherOfStore(cart) }}
                </th>
                <th v-else>0</th>
              </tr>
              <tr>
                <th>Khuyến mãi (toàn bộ)</th>
                <th class="text-danger" v-if="GetVoucherOfGlobal(cart) > 0">
                  -{{ GetVoucherOfGlobal(cart) }}
                </th>
                <th v-else>0</th>
              </tr>
              <tr>
                <th>Tổng tiền</th>
                <th>{{ GetTotal(cart) }}</th>
              </tr>
              </tbody>
            </table>
          </div>
        </div>

        <div class="text-center">
          <button class="btn btn-success btn-sm" @click="submitOrder(cart)">Gửi đơn hàng</button>
        </div>
      </div>
      <div class="col-sm-4">
        <div class="mb-3" style="max-height: 10rem" v-if="receivers">
          <span class="fw-medium">Thông tin người nhận</span>
          <div v-for="(receiver, num) in receivers" :key="num" class="form-check mt-3">
            <input name="receiver" class="form-check-input" type="radio" @click = "checkedReceiver(receiver)"
                   :checked="receiver.isChecked">
            <label class="form-check-label">
              {{ receiver.name }} - {{ receiver.phone }} - {{ receiver.address }}
            </label>
          </div>
        </div>
        <div class="mb-3">
          <span class="fw-medium">Cửa hàng: <span class="text-info">{{ cart.store.name }}</span></span>
        </div>

        <span class="fw-medium">Áp mã khuyến mãi</span>
        <div class="alert alert-warning alert-dismissible" role="alert"
             v-if="voucherOfStore.some(item=>item.cart === cart)">
          {{ voucherOfStore.find(item=>item.cart).item.title}}
          <button type="button" class="btn-close" @click="RemoveVoucherStore(cart)"></button>
        </div>
        <div class="alert alert-info alert-dismissible" role="alert" v-if="voucherOfGlobal && voucherOfGlobal.cart === cart">
          {{ voucherOfGlobal.item.title }}
          <button type="button" class="btn-close" @click="RemoveVoucherGlobal(cart)"></button>
        </div>

        <div class="mt-3">
          <button type="button" class="btn btn-success btn-sm" data-bs-toggle="modal"
                  :data-bs-target="'#basicModal' + indexCart">
            Thêm khuyến mãi
          </button>
          <div class="modal fade" :id="'basicModal' + indexCart" tabindex="-1" aria-hidden="true">
            <div class="modal-dialog" role="document">
              <div class="modal-content">
                <div class="modal-header">
                  <h5 class="modal-title">Chọn khuyến mãi</h5>
                  <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                  <div class="demo-inline-spacing mt-3">
                    <div class="list-group">
                      <a v-for="(item, index) in vouchers" :key="index" data-bs-dismiss="modal" @click="AddToVoucher(item, cart)"
                         class="list-group-item list-group-item-action flex-column align-items-start">
                        <div class="d-flex justify-content-between w-100">
                          <h6 class="text-primary">{{ item.title }}</h6>
                          <small>{{ item.endTime }}</small>
                        </div>
                        <small v-if="!item.isGlobal">Chỉ được sử dụng cho 1 số sản phẩm nhất định.</small>
                        <small v-if="item.isGlobal">Sử dụng cho mọi sản phẩm.</small>
                      </a>
                    </div>
                  </div>
                </div>
                <div class="modal-footer">
                  <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Close</button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <br>
        <small class="fw-medium">Tổng tiền: {{ getTotalAmount() }}</small>
      </div>
    </div>
  </div>
</template>

<script>
import {onMounted, reactive, ref} from "vue";
import { useToast } from "vue-toastification";
import OrderService from "@/services/order.service.js";
import { useRouter } from "vue-router";
import CartService from "@/services/cart.service.js";
import ReceiverService from "@/services/receiver.service.js";
import VoucherService from "@/services/voucher.service.js";

export default {
  name: "Order",
  mounted() {
    document.title = 'Hóa đơn';
  },
  setup() {
    const toast = useToast();
    const orders = ref([])
    const order = reactive({
      totalPrice:'',receiverName:'',receiverPhone:'',receiverAddress:'',carts:[],vouchers:[]
    })
    const vouchers = ref([]);
    const data = ref([]);
    let voucherOfStore = ref([])
    let voucherOfGlobal = ref()
    const router = useRouter();
    const receivers = ref([]);

    onMounted(() => {
      let count = 1;
      CartService.cartByStore().then(response => {
        data.value = response.data.map(item => ({
          ...item,
          id:count++,
          subTotal: item.cartItems.reduce((accumulator, product) => accumulator + product.price * product.quantity, 0),
          discountStore: 0,
          discountGlobal: 0
        }));
      });

      ReceiverService.getAll().then(response => {
        receivers.value = response.data;
      }).catch(err=>{console.log(err)});

      VoucherService.myVoucher().then(response => {
        vouchers.value = response.data.filter(item=>item.isUsed === false).map(item => ({
          ...item,
          isChecked: false
        }));
      }).catch(err=>{console.log(err)})
    });
    const checkedReceiver = async (item) =>{
      let index = receivers.value.findIndex(x=>x.id === item.id)
      if (index !== -1){
        const data = {receiverId : item.id}
        ReceiverService.checked(data).then(response=>{
          receivers.value.map(receiver=> ({
            ...receiver,
            isChecked: false
          }))
          receivers.value[index].isChecked = true
        })
      }else {
        toast.warning("Chọn người nhận thất bại.")
      }

    }

    const getTotalAmount = () => {
      let total = 0;
      data.value.forEach(item => {
        total += GetTotal(item);
      });
      return total;
    };

    const submitOrder = async (cart) => {
      let vouchers = [...voucherOfStore.value];
      if (voucherOfGlobal.value && voucherOfGlobal.value.cart.id === cart.id){
         vouchers.push(voucherOfGlobal.value)
      }
      let receiver = receivers.value.find(item=>item.isChecked)
      const orderData = {
        carts: cart.cartItems.map(item => item.id),
        totalPrice: GetTotal(cart),
        receiverName: receiver.name,
        payment:"Thanh toán khi nhận hàng",
        receiverPhone: receiver.phone,
        storeId: cart.store.id,
        receiverAddress: receiver.address,
        vouchers: vouchers.filter(item=>item.cart.id === cart.id).map(x=>x.item.id)
      };
      OrderService.create(orderData).then(response=>{
        toast.success("Tạo đơn hàng thành công")
        router.push("/notification-after-order")
      }).catch(err=>{
        console.log(err)
      })
    };

    const AddToVoucher = (item,cart) => {
      let index = vouchers.value.findIndex(a=>a.id === item.id);
      if (item.isGlobal) {
        vouchers.value[index].isChecked = true;
        voucherOfGlobal.value = {item:item, cart:cart}
      } else {
        if (item.storeId !== cart.store.id){
          toast.warning("Khuyến mãi này không áp dụng cho hóa đơn này.")
        }else{
          let indexOfStore = voucherOfStore.value.findIndex(voucher=>voucher.cart.id === cart.id)
          if (indexOfStore !== -1){
            voucherOfStore.value.splice(indexOfStore,1);
          }
          const obj = {item:item , cart:cart}
          voucherOfStore.value.push(obj)
        }
      }
      updateTotalAmount();
    };

    const RemoveVoucherStore = (cart) => {
      let index = voucherOfStore.value.findIndex(item=>item.cart.id === cart.id);
      if (index !== -1) {
        voucherOfStore.value.splice(index, 1);
        updateTotalAmount();
      } else {
        console.log('Không tìm thấy đối tượng có cart = true.');
      }

    };

    const RemoveVoucherGlobal = () => {
      voucherOfGlobal.value = null;
      updateTotalAmount();
    };

    const GetVoucherOfStore = (cart) => {
      let index = voucherOfStore.value.findIndex(item=>item.cart === cart);
      if (index !== -1){
        let voucher = voucherOfStore.value[index].item;
        if (voucher.discountPercentage === 0) {
          return voucher.discountAmount;
        }
        if (voucher.discountAmount === 0) {
          return cart.subTotal * (voucher.discountPercentage / 100);
        }
      }
      return 0;
    };

    const GetVoucherOfGlobal = (cart) => {
      let voucher = voucherOfGlobal.value;
      if (voucher && voucher.cart === cart){
        if (voucher.item.discountPercentage === 0) {
          return voucher.item.discountAmount;
        }
        if (voucher.item.discountAmount === 0) {
          return cart.subTotal * (voucher.item.discountPercentage / 100);
        }
      }
      return 0;
    };

    const GetTotal = (item) => {
      let total = item.subTotal - GetVoucherOfStore(item) - GetVoucherOfGlobal(item);
      if (total > 0) {
        return total;
      }
      return 0;
    };

    const updateTotalAmount = () => {
      data.value.forEach(item => {
        item.totalAmount = GetTotal(item);
      });
    };

    return { toast, data, getTotalAmount, submitOrder, receivers,GetVoucherOfGlobal,GetVoucherOfStore,GetTotal,
      voucherOfGlobal,voucherOfStore,checkedReceiver,
      vouchers, AddToVoucher, RemoveVoucherStore, RemoveVoucherGlobal };
  }
};
</script>

<style>
.modal-body {
  max-height: 25rem;
  min-height: 20rem;
  overflow-y: auto;
}
</style>
