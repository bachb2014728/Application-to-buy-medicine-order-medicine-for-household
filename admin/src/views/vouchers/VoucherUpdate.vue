<template>
  <div class="card">
    <div class="card-body">
      <h5 class="card-title">Nhập thông tin mã khuyến mãi</h5>
      <form class="g-2" @submit.prevent="updateItem" v-if="voucher">
        <div class="mb-3">
          <label class="col-form-label" for="title">Nhập tiêu đề mã khuyến mãi</label>
          <input type="text" class="form-control" id="title" name="title" v-model="voucher.title">
        </div>
        <div class="row mb-3">
          <div class="col-sm">
            <label class="col-form-label" for="discountAmount">Số tiền được giảm (nếu có)</label>
            <input type="number" class="form-control" id="discountAmount" name="discountAmount" v-model="voucher.discountAmount">
          </div>
          <div class="col-6">
            <label class="col-form-label" for="discountPercentage">Phần trăm được giảm (nếu có): {{ voucher.discountPercentage }}%</label>
            <input type="range" min="0" max="100" class="form-range" id="discountPercentage" v-model="voucher.discountPercentage">
          </div>
        </div>
        <div class="row mb-3">
          <div class="col-sm-6">
            <label class="col-form-label" for="startTime">Thời gian bắt đầu : <span class="badge bg-info">{{formatDate(voucher.startTime)}}</span></label>
            <input class="form-control" id="startTime" type="datetime-local" v-model="voucher.startTime">
          </div>
          <div class="col-sm-6">
            <label class="col-form-label" for="endTime">Thời gian kết thúc : <span class="badge bg-success">{{formatDate(voucher.endTime)}}</span></label>
            <input class="form-control" id="endTime" type="datetime-local" v-model="voucher.endTime">
          </div>
        </div>
        <div class="text-center">
          <button type="submit" class="btn btn-sm btn-primary me-2">Cập nhật khuyến mãi</button>
        </div>
      </form>
    </div>
  </div>
</template>
<script>
import VoucherService from "@/services/voucher.service.js";
import {useToast} from "vue-toastification";
import {onMounted, reactive, ref} from "vue";
import {useRoute} from "vue-router";
import {formatDate} from "@/helpers/time.utils.js";

export default {
  name: "VoucherUpdate",
  mounted() {
    document.title='Cập nhật khuyến mãi'
  },
  setup(){
    const toast = useToast();
    const route = useRoute();
    let id = ref();
    const voucher = ref();
    onMounted(async () =>{
      id = route.params.id;
      VoucherService.getOne(id).then(resposne =>{
        voucher.value = resposne.data
      })
    });
    return{voucher, toast}
  },
  methods:{
    formatDate(datetime) {
      return formatDate(datetime);
    },
    async updateItem(){
      VoucherService.update(this.voucher.id,this.voucher).then(response=>{
        this.toast.success("Cập nhật mã khuyến mãi thành công.")
      }).catch(err=>{
        this.toast.error("Lỗi khi cập nhật mã khuyến mãi.")
      })
    }
  }
}
</script>

<style scoped>

</style>