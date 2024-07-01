<template>
  <div class="card" v-if="data">
    <h5 class="card-header">Thông tin cá nhân</h5>
    <div class="card-body">
      <form onsubmit="return false" @submit.prevent="updateItem">
        <div class="mb-3">
          <div class="row mb-3">
            <div class="col-sm">
              <label for="firstName" class="form-label">Họ người dùng</label>
              <input class="form-control" type="text" id="firstName" name="firstName"
                     v-model="data.firstName" autofocus="" placeholder="Vui lòng nhập họ người dùng">
            </div>
            <div class="col-sm">
              <label for="lastName" class="form-label">Tên người dùng</label>
              <input class="form-control" type="text" name="lastName" id="lastName"
                     v-model="data.lastName" placeholder="Vui lòng nhập tên người dùng">
            </div>
            <div class="col-sm">
              <label class="form-label" for="phoneNumber">Số điện thoại</label>
              <div class="input-group">
                <span class="input-group-text">VN (+84)</span>
                <input type="text" id="phoneNumber" name="phoneNumber" class="form-control"
                       v-model="data.phoneNumber" placeholder="Vui lòng nhập số điện thoại.">
              </div>
            </div>
          </div>
          <div class="row mb-3">
            <div class="col-sm">
              <label for="email" class="form-label">E-mail</label>
              <input class="form-control" type="text" id="email" name="email"
                     v-model="data.email" placeholder="Vui lòng nhập địa chỉ e-mail">
            </div>
            <div class="col-sm">
              <label for="username" class="form-label">Username</label>
              <input type="text" class="form-control" id="username" name="username" v-model="data.username" readonly>
            </div>
            <div class="col-sm">
              <label for="exampleFormControlSelect1" class="form-label">Giới tính</label>
              <select class="form-select" id="exampleFormControlSelect1" v-model="data['gender']">
                <option selected="">Chọn giới tính</option>
                <option value="-1">Nữ</option>
                <option value="1">Nam</option>
                <option value="0">Khác</option>
              </select>
            </div>
          </div>
          <div class="row mb-3">
            <div class="col-sm-4">
              <label for="zipCode" class="form-label">Căn cước công dân</label>
              <input type="text" class="form-control" id="zipCode" name="zipCode"
                     placeholder="Nhập số căn cước công dân" maxlength="12" v-model="data.zipCode">
            </div>
            <div class="col-sm-8">
              <label for="address" class="form-label">Địa chỉ</label>
              <input type="text" class="form-control" id="address" v-model="data.address"
                     name="address" placeholder="Nhập địa chỉ người dùng">
            </div>
          </div>
        </div>
        <div class="mt-2">
          <button type="submit" class="btn btn-primary me-2">Lưu thay đổi</button>
          <button type="reset" class="btn btn-outline-secondary">Hủy</button>
        </div>
      </form>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import CustomerService from "@/services/customer.service.js";
import {useToast} from "vue-toastification";
import AuthService from "@/services/auth.service.js";

export default {
  name: "AccountManage",
  mounted() {
    document.title="Thông tin cá nhân"
  },
  setup(){
    const data = ref()
    const toast =useToast()
    onMounted(async ()=>{
      AuthService.profile().then(response=>{
        data.value = response.data;
      })
    })
    return{data,toast}
  },
  methods:{
    updateItem(){
      CustomerService.update(this.data.id,this.data).then(response=>{
        this.toast.success("Cập nhật dữ liệu thành công.")
      }).catch(err=>{
        console.log(err.response)
        this.toast.error(err.response.data.detail)
      })
    }
  }
}
</script>
<style scoped>

</style>