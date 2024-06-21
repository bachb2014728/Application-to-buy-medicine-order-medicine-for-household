
<template>
  <form @submit.prevent="submit">
    <h1 class="h3 mb-3 card-title text-center">Đăng ký</h1>
    <div class="row">
      <div class="col-sm">
        <div class="form-floating form-group mb-3">
          <input v-model="data.email" type="email" class="form-control form-control-lg" :class="{ 'is-invalid': errors.email }"  placeholder="name@example.com" @input="onChange('email')">
          <label >Địa chỉ email</label>
          <p v-if="errors.email" class="text-danger label">{{ errors.email }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.username" type="text" class="form-control form-control-lg" :class="{ 'is-invalid': errors.username }"  placeholder="name@example.com" @input="onChange('username')">
          <label >Username</label>
          <p v-if="errors.username" class="text-danger label">{{ errors.username }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.phone" type="text" class="form-control form-control-lg" :class="{ 'is-invalid': errors.phone }"  placeholder="" @input="checkPhoneNumber">
          <label >Số điện thoại</label>
          <p v-if="errors.phone" class="text-danger label">{{ errors.phone }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.firstName" type="text" class="form-control form-control-lg" placeholder="firstname" :class="{ 'is-invalid': errors.firstName }">
          <label >Họ</label>
          <p v-if="errors.firstName" class="text-danger label">{{ errors.firstName }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.lastName" type="text" class="form-control form-control-lg" placeholder="lastname" :class="{ 'is-invalid': errors.lastName }">
          <label >Tên</label>
          <p v-if="errors.lastName" class="text-danger label">{{ errors.lastName }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.password" type="password" class="form-control form-control-lg" :class="{ 'is-invalid': errors.password }"  placeholder="password" @input="onChange('password')">
          <label >Mật khẩu</label>
          <p v-if="errors.password" class="text-danger label">{{ errors.password }}</p>
        </div>
      </div>
      <div class="col-sm">
        <div class="form-floating form-group mb-3" >
          <select id="province" name="province" class="form-select mb-3" v-model="selectedCity" @change="updateCityId" required aria-label="select example">
            <option value="" disabled selected>Chọn tỉnh thành</option>
            <option v-for="(city, index) in cities" :value="city" :key="index">{{ city.name }}</option>
          </select>

          <select id="district" name="district" class="form-select mb-3" v-model="selectedDistrict" @change="updateDistrictId" required aria-label="select example">
            <option value="" disabled selected>Chọn quận huyện</option>
            <option v-for="(district, index) in districts" :value="district" :key="index">{{ district.name }}</option>
          </select>

          <select id="ward" name="ward" class="form-select mb-3" v-model="selectedWard" @change="updateWardId" required aria-label="select example">
            <option value="" disabled selected>Chọn phường xã</option>
            <option v-for="(ward, index) in wards" :value="ward" :key="index">{{ ward.name }}</option>
          </select>

        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="detail" type="text" class="form-control form-control-lg" :class="{ 'is-invalid': errors.address }" placeholder="" >
          <label>Số nhà</label>
          <p v-if="errors.address" class="text-danger label">{{ errors.address }}</p>
        </div>
        <div class="form-floating form-group mb-3">
          <input v-model="data.address" type="text" class="form-control form-control-lg" :class="{ 'is-invalid': errors.address}" disabled>
          <label>Địa chỉ</label>
          <p v-if="errors.address" class="text-danger label">{{ errors.address }}</p>
        </div>

        <div class="form-floating form-group mb-3">
          <input v-model="data.passwordConfirm" type="password" class="form-control form-control-lg" :class="{ 'is-invalid': errors.passwordConfirm }" placeholder="password confirm" @input="onChange('passwordConfirm')">
          <label >Nhập lại mật khẩu</label>
          <p v-if="errors.passwordConfirm" class="text-danger label">{{ errors.passwordConfirm }}</p>
        </div>
      </div>
    </div>
    <button class="w-100 btn btn-lg btn-success" type="submit">Đăng ký</button>
  </form>
</template>
<script>
import { reactive } from "vue";
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
  data(){
    return{
      cities: [],
      districts: [],
      wards:[],
      selectedCity: '', selectedDistrict: '', selectedWard: '', detail:''
    }
  },
  setup() {
    const toast = useToast();
    const data = reactive({
      firstName:'', lastName:'',username:'',
      email:'', password:'', passwordConfirm: '',
      phone:'',address:''
    });
    const errors = reactive({
      email: '', username:'', phone:'', password: '', passwordConfirm: '', firstName:'',lastName:'',address:''
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
      if (!correctLength.test(data.phone)) errors.phone = 'Vui lòng nhập số điện thoại có độ dài từ 10 đến 11 chữ số.';
      if (data.phone[0] !== '0' || data.phone.startsWith('84')) errors.phone = 'Số điện thoại không đúng định dạng.'
      if (data.phone.length === 10) {data.phone = '84' + data.phone.substring(1);}
      if(!data.lastName) errors.lastName = 'Vui lòng nhập tên.'
      if(!data.passwordConfirm) errors.passwordConfirm = 'Vui lòng nhập lại mật khẩu.';
      if(!data.address) errors.address = 'Địa chỉ không để rỗng'
      if(data.password !== data.passwordConfirm) {
        errors.passwordConfirm = 'Mật khẩu và mật khẩu xác nhận không khớp. Vui lòng nhập lại.';
      }

      if(Object.values(errors).every(error => !error)) {

        AuthService.signup(data)
            .then(response => {
              if(response.status === 200){
                toast.success("Đăng ký tài khoản mới thành công.")
                router.push('/login');
              }
            })
            .catch(error =>{
              if(error.response.status === 409){
                toast.error(error.response.data.detail)
              }
              else{
                console.error(error);
                toast.error("Có lỗi khi đăng ký.");
              }
            });

      }
    }
    return{
      data,
      submit,
      errors,
      onChange
    }
  },
  methods:{
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