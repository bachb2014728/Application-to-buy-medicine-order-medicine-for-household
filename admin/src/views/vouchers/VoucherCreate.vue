<template>
  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Nhập thông tin mã khuyến mãi</h5>
      <form class="g-2" @submit.prevent="createItem">
        <div class="mb-3">
          <label class="col-form-label" for="title">Nhập tiêu đề mã khuyến mãi</label>
          <input type="text" class="form-control" id="title" name="title" v-model="data.title">
        </div>
        <div class="row mb-3">
          <div class="col-sm">
            <label class="col-form-label" for="discountAmount">Số tiền được giảm (nếu có)</label>
            <input type="number" class="form-control" id="discountAmount" name="discountAmount" v-model="data.discountAmount">
          </div>
          <div class="col-6">
            <label class="col-form-label" for="discountPercentage">Phần trăm được giảm (nếu có): {{ data.discountPercentage }}%</label>
            <input type="range" min="0" max="100" class="form-range" id="discountPercentage" v-model="data.discountPercentage">
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm-6">
            <label class="col-form-label" for="startTime">Thời gian bắt đầu</label>
            <input class="form-control" id="startTime" type="datetime-local" v-model="data.startTime">
          </div>
          <div class="col-sm-6">
            <label class="col-form-label" for="endTime">Thời gian kết thúc</label>
            <input class="form-control" id="endTime" type="datetime-local" v-model="data.endTime">
          </div>
        </div>
        <div class="text-center">
          <button type="submit" class="btn btn-sm btn-primary me-2">Thêm mới</button>
          <button type="reset" class="btn btn-sm btn-secondary" @click="resetForm">Reset</button>
        </div>
      </form>
    </div>
  </div>
</template>
<script>
import VoucherService from "@/services/voucher.service.js";
import {useToast} from "vue-toastification";
import {onMounted, reactive} from "vue";

export default {
  name: "VoucherCreate",
  mounted() {
    document.title='Thêm khuyến mãi';
    if (this.$route.query.value !== undefined){
      this.data.storeId = parseInt(this.$route.query.value)
    }
  },
  setup(){
    const toast = useToast();
    const data = reactive({
      title: '',  discountAmount: 0, discountPercentage:0, startTime:'',endTime:'',storeId:0
    })
    onMounted(async () =>{});
    return{data, toast}
  },
  methods:{
    resetForm() {
      this.data.title = '';
      this.data.discountAmount = 0;
      this.data.discountPercentage = 0;
      this.data.startTime = '';
      this.data.endTime = '';
    },
    async createItem(){
      console.log(this.data)
      VoucherService.create(this.data).then(async response => {
        this.toast.success("Thêm mới danh mục thành công.")
      }).catch(error =>{
        this.toast.error(error.response.data.detail)
      })
    }
  }
}
</script>

<style scoped>

</style>