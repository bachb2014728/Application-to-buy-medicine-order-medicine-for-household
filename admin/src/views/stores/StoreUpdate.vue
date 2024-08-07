<template>
  <div v-if="store" class="card">
    <div class="card-body">
      <h5 class="card-title">Thông tin nhà thuốc</h5>
      <form class="row g-3" @submit.prevent="updateItem">
        <div class="row mb-3">
          <div class="col-sm">
            <label for="name" class="form-label">Tên nhà thuốc</label>
            <input type="text" class="form-control" @input="updateSlug" id="name" v-model="store.name" :class="{ 'is-invalid': errors.name }">
            <p v-if="errors.name" class="text-danger label">{{ errors.name }}</p>
          </div>
          <div class="col-sm">
            <label for="URL" class="form-label">Đường dẫn</label>
            <input type="text" class="form-control" id="url" v-model="store.url" :class="{ 'is-invalid': errors.url }">
            <p v-if="errors.url" class="text-danger label">{{ errors.url }}</p>
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm">
            <label for="province" class="form-label">Tỉnh thành phố</label>
            <select id="province" name="province" class="form-select mb-3" v-model="selectedCity" @change="updateCityId" >
              <option value="" disabled selected>Chọn tỉnh thành</option>
              <option v-for="(city, index) in cities" :value="city" :key="index">{{ city.name }}</option>
            </select>
          </div>
          <div class="col-sm">
            <label for="district" class="form-label">Quận huyện</label>
            <select id="district" name="district" class="form-select mb-3" v-model="selectedDistrict" @change="updateDistrictId" >
              <option value="" disabled selected>Chọn quận huyện</option>
              <option v-for="(district, index) in districts" :value="district" :key="index">{{ district.name }}</option>
            </select>
          </div>
          <div class="col-sm">
            <label for="ward" class="form-label">Phường xã</label>
            <select id="ward" name="ward" class="form-select mb-3" v-model="selectedWard" @change="updateWardId" >
              <option value="" disabled selected>Chọn phường xã</option>
              <option v-for="(ward, index) in wards" :value="ward" :key="index">{{ ward.name }}</option>
            </select>
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm">
            <label for="detail" class="form-label">Số nhà đường phố</label>
            <input v-model="detail" type="text" class="form-control" :class="{ 'is-invalid': errors.address }" placeholder="Số nhà" >
            <p v-if="errors.address" class="text-danger label">{{ errors.address }}</p>
          </div>
          <div class="col-sm">
            <label for="address" class="form-label">Địa chỉ nhà thuốc</label>
            <input v-model="store.address" type="text" class="form-control" :class="{ 'is-invalid': errors.address}" disabled>
          </div>
        </div>
        <div class="row mb-3">
          <label for="info" class="form-label">Thông tin nhà thuốc</label>
          <CKEDITOR @updateData="handleUpdateData" :data="store.info" />
        </div>
        <div class="text-center">
          <button type="submit" class="btn btn-sm btn-primary" style="margin-right: 5px">Cập nhật nhà thuốc</button>
        </div>
      </form>

    </div>
  </div>
</template>
<script>
import CKEDITOR from "@/components/CKEDITOR.vue";
import {useToast} from "vue-toastification";
import {onMounted, reactive, ref} from "vue";
import {useRoute} from "vue-router";
import StoreService from "@/services/store.service.js";
import axios from "axios";
const apiClient = axios.create({
  withCredentials: false,
  headers: {
    Accept: 'application/json',
    'Content-Type': 'application/json'
  }
});
export default {
  name: "StoreUpdate",
  components: {CKEDITOR},
  mounted() {
    document.title = 'Cập nhật nhà thuốc'
  },
  setup(){
    const toast = useToast();
    const store = ref();
    const data = reactive({
      address:'',name:'',url:'',info:'', followers:[]
    });
    const route = useRoute();
    onMounted(async () => {
      const url = route.params.url;
      await StoreService.getOneByURL(url).then(response => {
        store.value = response.data
        console.log(store)
      }).catch(error =>{
        toast.error("Lỗi hệ thống.")
      })
    })
    return{
      store,
      data,
      toast
    }
  },
  data(){
    return{
      cities: [],
      districts: [],
      wards:[],
      selectedCity: '', selectedDistrict: '', selectedWard: '', detail:'',
      errors:{},
    }
  },
  methods:{
    handleUpdateData(editorData) {
      this.store.info = editorData
    },
    updateCityId(event) {
      const selectedOption = event.target.options[event.target.selectedIndex];
      this.getDistricts();
    },
    updateDistrictId(event) {
      const selectedOption = event.target.options[event.target.selectedIndex];
      this.getWards();
    },
    updateWardId(event){
      const selectedOption = event.target.options[event.target.selectedIndex];
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
      this.store.address = this.detail + ', '+this.selectedWard.name+', '+this.selectedDistrict.name+', '+this.selectedCity.name
    },
    updateSlug() {
      this.store.url = this.slugify(this.store.name);
    },
    slugify(title) {
      var slug;
      slug = title.toLowerCase();
      slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
      slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
      slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
      slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
      slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
      slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
      slug = slug.replace(/đ/gi, 'd');
      slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
      slug = slug.replace(/ /gi, "-");
      slug = slug.replace(/\-\-\-\-\-/gi, '-');
      slug = slug.replace(/\-\-\-\-/gi, '-');
      slug = slug.replace(/\-\-\-/gi, '-');
      slug = slug.replace(/\-\-/gi, '-');
      slug = '@' + slug + '@';
      slug = slug.replace(/\@\-|\-\@|\@/gi, '');
      return slug;
    },
    async updateItem() {
      this.errors={}
      if(!this.store.name) this.errors.name = 'Vui lòng nhập tên nhà thuốc.'
      if(!this.store.url) this.errors.url ='Vui lòng nhập đường dẫn.'
      if (!this.store.info){
        this.errors.info = 'Vui lòng nhập thông tin nhà thuốc.'
        this.toast.error("Vui lòng nhập thông tin nhà thuốc.")
      }
      if(!this.store.address) this.errors.address='Vui lòng nhập đầy đủ thông tin địa chỉ nhà thuốc.'
      if (Object.keys(this.errors).length > 0) {
        return;
      }else{
        this.data.name = this.store.name
        this.data.url = this.store.url
        this.data.info = this.store.info
        this.data.address = this.store.address
        await StoreService.update(this.store.id,this.data).then(response =>{
          this.toast.success("Thêm nhà thuốc mới thành công.");
          this.store = response.data
        }).catch(error=>{
          this.toast.error("Cập nhật nhà thuốc thất bại.");
        })
      }
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

</style>