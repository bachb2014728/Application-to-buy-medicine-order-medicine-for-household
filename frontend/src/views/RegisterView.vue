<template>
  <div class="card">
    <div class="card-body">
      <h4 class="mb-2 text-center">Cuộc chơi giờ mới bắt đầu 🚀</h4>
      <p class="mb-4 text-center"><span class="text-danger fw-medium">Tạo tài khoản</span> để quản lý ứng dụng trở nên thú vị hơn!</p>

      <form id="formAuthentication" class="mb-3" @submit.prevent="submit">
<!--        email,username-->
        <div class="row mb-3">
          <div class="col-sm">
            <label for="username" class="form-label">Username</label>
            <div class="input-group">
              <span id="username" class="input-group-text"><i class="bi bi-person-vcard"></i></span>
              <input type="text" class="form-control" id="username" name="username" placeholder="Nhập username" autofocus=""
                     v-model="data.username" :class="{ 'is-invalid': errors.username }"  @input="onChange('username')">
            </div>
            <p v-if="errors.username" class="text-danger fw-medium">{{ errors.username }}</p>
          </div>
          <div class="col-sm">
            <label for="email" class="form-label">Địa chỉ e-mail</label>
            <div class="input-group">
              <span id="email" class="input-group-text"><i class="bi bi-envelope"></i></span>
              <input type="email" class="form-control" id="email" name="email" placeholder="Nhập địa chỉ e-mail "
                     v-model="data.email" :class="{ 'is-invalid': errors.email }" @input="onChange('email')">
            </div>
            <p v-if="errors.email" class="text-danger fw-medium">{{ errors.email }}</p>
          </div>
        </div>
<!--        họ,tên,số điện thoại-->
        <div class="row mb-3">
          <div class="col-sm">
            <label class="form-label" for="firstname">Họ người dùng</label>
            <div class="input-group">
              <span id="firstname" class="input-group-text"><i class="bi bi-person"></i></span>
              <input type="text" class="form-control" id="firstname" placeholder="Nhập họ" aria-describedby="firstname"
                     v-model="data.firstName" :class="{ 'is-invalid': errors.firstName }" @input="checkFirstName">
            </div>
            <p v-if="errors.firstName" class="text-danger fw-medium">{{ errors.firstName }}</p>
          </div>
          <div class="col-sm">
            <label class="form-label" for="lastname">Tên người dùng</label>
            <div class="input-group">
              <span id="lastname" class="input-group-text"><i class="bi bi-person"></i></span>
              <input type="text" class="form-control" id="lastname" placeholder="Nhập tên" aria-describedby="lastname"
                     v-model="data.lastName" :class="{ 'is-invalid': errors.lastName }" @input="checkLastName">
            </div>
            <p v-if="errors.lastName" class="text-danger fw-medium">{{ errors.lastName }}</p>
          </div>
          <div class="col-sm">
            <label class="form-label" for="phone">Số điện thoại</label>
            <div class="input-group">
              <span id="phone" class="input-group-text"><i class="bi bi-telephone"></i></span>
              <input type="text" class="form-control" id="phone" placeholder="Nhập số điện thoại" aria-describedby="phone"
                     v-model="data.phone" :class="{ 'is-invalid': errors.phone }"  @input="checkPhoneNumber"
                     minlength="10" maxlength="11">
            </div>
            <p v-if="errors.phone" class="text-danger fw-medium">{{ errors.phone }}</p>
          </div>
        </div>
<!--        tỉnh,huyện,xã-->
        <div class="row mb-3">
          <div class="col-sm">
            <label for="province" class="form-label">Tỉnh, Thành phố</label>
            <select class="form-select" id="province" aria-label="Default select example" v-model="selectedCity" @change="updateCityId">
              <option value="" disabled selected>Chọn tỉnh thành</option>
              <option v-for="(city, index) in cities" :value="city" :key="index">{{ city.name }}</option>
            </select>
          </div>
          <div class="col-sm">
            <label for="district" class="form-label">Quận, huyện</label>
            <select class="form-select" id="district" aria-label="Default select example" v-model="selectedDistrict" @change="updateDistrictId">
              <option value="" disabled selected>Chọn quận huyện</option>
              <option v-for="(district, index) in districts" :value="district" :key="index">{{ district.name }}</option>
            </select>
          </div>
          <div class="col-sm">
            <label for="ward" class="form-label">Xã, phường</label>
            <select class="form-select" id="ward" aria-label="Default select example" v-model="selectedWard" @change="updateWardId">
              <option value="" disabled selected>Chọn phường xã</option>
              <option v-for="(ward, index) in wards" :value="ward" :key="index">{{ ward.name }}</option>
            </select>
          </div>
        </div>
<!--        địa chỉ-->
        <div class="row mb-3">
          <div class="col-sm">
            <label for="detail" class="form-label">Số nhà, đường phố</label>
            <div class="input-group">
              <span id="detail" class="input-group-text"><i class="bi bi-house"></i></span>
              <input type="text" class="form-control" id="detail" name="detail" placeholder="Nhập số nhà, đường phố"
                     v-model="detail" :class="{ 'is-invalid': errors.address }" @input="checkDetail">
            </div>
            <p v-if="errors.address" class="text-danger fw-medium">{{ errors.address }}</p>
          </div>
          <div class="col-sm">
            <label for="address" class="form-label">Địa chỉ người dùng</label>
            <div class="input-group">
              <span id="detail" class="input-group-text"><i class="bi bi-info-circle"></i></span>
              <input v-model="data.address" type="text" class="form-control" id="address" name="address"
                     placeholder="Nhập số nhà, đường phố" :class="{ 'is-invalid': errors.address}" @input="checkDetail">
            </div>
            <p v-if="errors.address" class="text-danger fw-medium">{{ errors.address }}</p>
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm">
            <label class="form-label" for="password">Mật khẩu</label>
            <div class="input-group">
              <span id="password" class="input-group-text"><i class="bi bi-lock"></i></span>
              <input type="password" id="password" class="form-control" name="password" placeholder="Nhập mật khẩu" aria-describedby="password"
                     v-model="data.password" :class="{ 'is-invalid': errors.password }" @input="onChange('password')">
            </div>
            <p v-if="errors.password" class="text-danger fw-medium">{{ errors.password }}</p>
          </div>
          <div class="col-sm">
            <label class="form-label" for="password-confirm">Nhập lại mật khẩu</label>
            <div class="input-group">
              <span id="password-confirm" class="input-group-text"><i class="bi bi-lock"></i></span>
              <input type="password" id="password-confirm" class="form-control" name="password" placeholder="Nhập lại mật khẩu" aria-describedby="password"
                     v-model="data.passwordConfirm" :class="{ 'is-invalid': errors.passwordConfirm }" @input="onChange('passwordConfirm')">
            </div>
            <p v-if="errors.passwordConfirm" class="text-danger fw-medium">{{ errors.passwordConfirm }}</p>
          </div>
        </div>
        <div class="mb-3">
          <div class="form-check">
            <input class="form-check-input" type="checkbox" id="terms-conditions" name="terms" v-model="data.isChecked">
            <label class="form-check-label" for="terms-conditions">
              Bạn có đồng ý <span class="text-warning fw-medium">những yêu cầu và chính sách</span> của <router-link to="/" style="text-decoration: none" class="text-success fw-medium">thegioithuoc.com</router-link>
            </label>
          </div>
        </div>
        <button class="btn btn-primary d-grid w-100">Tạo tài khoản</button>
      </form>

      <p class="text-center">
        <span>Bạn đã có tài khoản? </span><router-link to="/login" class="fw-medium text-primary" style="text-decoration: none"><span>Đăng nhập</span></router-link>
      </p>
    </div>
  </div>
</template>
<script>
import {reactive, ref} from "vue";
import { useRouter } from "vue-router";
import AuthService from "@/services/auth.service.js";
import axios from "axios";

const apiClient = axios.create({
  baseURL: 'https://vnprovinces.pythonanywhere.com/api/',
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  }
});
import { useToast } from 'vue-toastification'
export default {
  name: 'RegisterView',
  mounted() {
    document.title = 'Đăng ký';
  },
  data(){return{cities: [], districts: [], wards:[], selectedCity: '', selectedDistrict: '', selectedWard: '', detail:''}
  },
  setup() {
    const toast = useToast();
    const isChecked = ref(true);
    const data = reactive({
      firstName:'', lastName:'',username:'', email:'', password:'', passwordConfirm: '', phone:'',address:'',isChecked:false
    });
    const errors = reactive({
      email: '', username:'', phone:'', password:'', passwordConfirm:'', firstName:'',lastName:'',address:''
    });
    const router = useRouter();
    const onChange = (field) => {
      errors[field] = '';
    }
    const correctLength = /^[0-9]{10,11}$/;
    const submit = async () => {
      if(!data.email) errors.email = 'Vui lòng nhập email.';
      if(!data.username) errors.username = 'Vui lòng nhập username.';
      if(!data.password) errors.password = 'Vui lòng nhập mật khẩu.';
      if(!data.firstName) errors.firstName ='Vui lòng nhập họ.';
      if(!data.phone) errors.phone = 'Vui lòng nhập số điện thoại.'
      if (data.phone.length === 10) {data.phone = '84' + data.phone.substring(1);}
      // if (!correctLength.test(data.phone)) errors.phone = 'Vui lòng nhập số điện thoại có độ dài từ 10 đến 11 chữ số.';
      // if (data.phone[0] !== '0' || data.phone.startsWith('84')) errors.phone = 'Số điện thoại không đúng định dạng.'
      if(!data.lastName) errors.lastName = 'Vui lòng nhập tên.'
      if(!data.passwordConfirm) errors.passwordConfirm = 'Vui lòng nhập lại mật khẩu.';
      if(!data.address) errors.address = 'Địa chỉ không để rỗng'
      if(data.password !== data.passwordConfirm) {
        errors.passwordConfirm = 'Mật khẩu và mật khẩu xác nhận không khớp. Vui lòng nhập lại.';
      }
      if(Object.values(errors).every(error => !error)) {
        if (data.isChecked){
          AuthService.signup(data).then(response => {
            toast.success("Đăng ký tài khoản mới thành công.")
            router.push('/login');
          }).catch(error =>{
            if(error.response.status === 409){
              toast.error(error.response.data.detail)
            }
            else{
              console.error(error);
              toast.error("Có lỗi khi đăng ký.");
            }
          });
        }else{
          toast.warning("Bạn chưa chấp nhập các điều khoản.")
        }
      }
    }
    return{data, submit, errors, onChange,isChecked}
  },
  methods:{
    checkFirstName(){
      if (this.data.firstName == null) {this.errors.firstName = 'Vui lòng nhập họ.';}
      else {this.errors.firstName = '';}
    },
    checkDetail(){
      if (this.data.address == null) {this.errors.address = 'Vui lòng nhập địa chỉ.';}
      else {this.errors.address = '';}
    },
    checkLastName(){
      if (this.data.lastName == null) {this.errors.lastName = 'Vui lòng nhập tên.';}
      else {this.errors.lastName = '';}
    },
    checkPhoneNumber() {
      const isNumber = /^[0-9]*$/;
      if (!isNumber.test(this.data.phone)) {
        this.errors.phone = 'Vui lòng chỉ nhập số.';
      }
      else {
        this.errors.phone = '';
      }
    },
    updateCityId(event) {
      const selectedOption = event.target.options[event.target.selectedIndex];
      // this.selectedCity.idProvince = selectedOption.getAttribute('data-id');
      this.getDistricts();
    },
    updateDistrictId(event) {
      const selectedOption = event.target.options[event.target.selectedIndex];
      // this.selectedDistrict.code = selectedOption.getAttribute('data-id');
      this.getWards();
    },
    updateWardId(event){
      const selectedOption = event.target.options[event.target.selectedIndex];
      // this.selectedWard.code = selectedOption.getAttribute('data-id');
    },
    getProvinces() {
      apiClient.get('https://toinh-api-tinh-thanh.onrender.com/province').then(response => {
        this.cities = response.data;
      });
    },
    getDistricts() {
      if (this.selectedCity) {
        apiClient.get(`https://toinh-api-tinh-thanh.onrender.com/district?idProvince=${this.selectedCity.idProvince}`).then(response => {
          this.districts = response.data;
        });
      }
    },
    getWards() {
      if (this.selectedDistrict) {
        apiClient.get(`https://toinh-api-tinh-thanh.onrender.com/commune?idDistrict=${this.selectedDistrict.idDistrict}`).then(response => {
          this.wards = response.data;
        });
      }
    },
    emitLocation() {
      this.data.address = this.detail + ', '+this.selectedWard.name+', '+this.selectedDistrict.name+', '+this.selectedCity.name
    },
  },
  watch: {
    detail() {
      this.emitLocation();
    }
  },
  created() {
    this.getProvinces();
  }
}
</script>

<style scoped>
.is-invalid {
  border-color: red;
}
</style>