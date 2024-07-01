<template>
  <div class="row">
    <div class="col-sm-8">
      <div class="card mb-4" >
        <div class="table-responsive text-nowrap">
          <table class="table table-sm">
            <thead>
            <tr>
              <th>stt</th>
              <th>tên</th>
              <th>số điện thoại</th>
              <th>địa chỉ</th>
              <th>Actions</th>
            </tr>
            </thead>
            <tbody class="table-border-bottom-0">
            <tr v-for="(item,index) in data" :key="index">
              <td>{{index +1}}</td>
              <td>{{item.name}}</td>
              <td>{{item.phone}}</td>
              <td>{{item.address}}</td>
              <td>
                <button class="btn btn-sm btn-danger" @click="confirmDelete(item)">Xóa</button>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </div>
    <div class="col-sm-4">
      <div class="card ">
        <div class="card-body">
          <form @submit.prevent="createItem">
            <div class="mb-3">
              <label class="form-label" for="name">Tên người nhận</label>
              <input type="text" class="form-control" id="name"  placeholder="Vui lòng nhập tên người nhận"
                     v-model="receiver.name"
              >
            </div>
            <div class="mb-3">
              <label class="form-label" for="phone">Số điện thoại</label>
              <input type="text" id="phone" class="form-control phone-mask" placeholder="Vui lòng nhập số điện thoại"
                     v-model="receiver.phone"
              >
            </div>
            <div class="mb-3">
              <label class="form-label" for="address">Địa chỉ nhận</label>
              <input type="text" id="address" class="form-control" placeholder="Vui lòng nhập địa chỉ nhận"
                     v-model="receiver.address"
              >
            </div>
            <div class="text-center">
              <button type="submit" class="btn btn-success btn-sm">Thêm thông tin nhận</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, reactive, ref} from "vue";
import ReceiverService from "@/services/receiver.service.js";
import {useToast} from "vue-toastification";
import VoucherService from "@/services/voucher.service.js";

export default {
  name: "ReceiverManage",
  mounted() {
    document.title='Quản lý địa chỉ nhận'
  },
  setup(){
    const data = ref([])
    const toast = useToast()
    let receiver = reactive({name:'',phone:'',address:''})
    onMounted(async ()=>[
        ReceiverService.getAll().then(response=>{
          data.value = response.data
        })
    ])
    const createItem = () => {
      ReceiverService.create(receiver).then(response=>{
        data.value.push(receiver)
        toast.success("Thêm địa chỉ nhận thành công.")
      }).catch(err=>{
        toast.error("Thêm địa chỉ nhận thất bại.")
      })
    }
    return{data,receiver,createItem,toast}
  },
  methods:{
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn xóa dữ liệu này không ?')) {
        this.deleteItem(item);
      }
    },
    async deleteItem(item) {
      let index = this.data.findIndex(receiver => receiver.id === item.id);
      if (index > -1) {
        ReceiverService.deleteOne(item.id).then(response => {
          this.data.splice(index, 1);
          this.toast.success("Xóa địa chỉ nhận thành công.")
        }).catch(err => {
          this.toast.error("Xảy ra lỗi khi xóa.")
        })
      }
    },
  }
}
</script>
<style scoped>

</style>