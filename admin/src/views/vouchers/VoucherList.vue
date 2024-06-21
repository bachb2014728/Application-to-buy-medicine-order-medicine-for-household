<template>
  <div class="card">
    <h5 class="card-header">Danh sách mã khuyến mãi</h5>
    <div class="table-responsive" v-if="vouchers && vouchers.length">
      <table class="table">
        <thead class="table-light">
        <tr>
          <th>Mã khuyến mãi</th>
          <th>Tên khuyến mãi</th>
          <th>Phúc lợi</th>
          <th>Ngày bắt đầu</th>
          <th>Ngày kết thúc</th>
          <th>Actions</th>
        </tr>
        </thead>
        <tbody class="table-border-bottom-0">
        <tr  v-for="(item, index) in vouchers" :key="index">
          <td>{{item.code}}</td>
          <td><span class="fw-medium">{{item.title}}</span></td>
          <td>
            <span v-if="item.discountAmount" class="badge bg-secondary">Khuyến mãi {{item.discountAmount}}K</span>
            <span v-if="item.discountPercentage" class="badge bg-primary">Khuyến mãi {{item.discountPercentage}}%</span>
          </td>
          <td>{{formatDate(item.startTime)}}</td>
          <td>{{formatDate(item.endTime)}}</td>
          <td>
            <router-link :to="`vouchers/${item.id}`" class="btn btn-secondary btn-sm me-3">
              <i class="bi bi-eye"></i>
            </router-link>
            <router-link :to="`vouchers/${item.id}/update`" class="btn btn-warning btn-sm me-3" v-if="item.isGlobal">
              <i class="bi bi-pencil-square"></i>
            </router-link>
            <a @click="confirmDelete(item)" class="btn btn-danger btn-sm me-3">
              <i class="bi bi-ban"></i>
            </a>
          </td>
        </tr>
        </tbody>
      </table>
    </div>
    <div v-else class="text-center mt-3">
      <p>{{message}}</p>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import {useToast} from "vue-toastification";
import VoucherService from "@/services/voucher.service.js";
import {formatDate} from "@/helpers/time.utils.js";

export default {
  name: "VoucherList",
  mounted() {
    document.title='Danh sách khuyến mãi'
  },
  setup(){
    const toast = useToast()
    const vouchers = ref([])
    const message = ref()
    onMounted(async ()=>{
      await VoucherService.getAll().then(response=>{
        vouchers.value = response.data
      }).catch(err=>{
        message.value=err.response.data.detail
      })
    })
    return{
      toast,vouchers,message
    }
  },
  methods:{
    formatDate(datetime) {
      return formatDate(datetime);
    },
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn khóa tài khoản này không ?')) {
        this.deleteItem(item);
      }
    },
    async deleteItem(item) {
      let index = this.vouchers.indexOf(item);
      if (index > -1) {
        this.vouchers.splice(index, 1);
        try {
          const response = await VoucherService.deleteOne(item.id);
          if (response.status === 200) {
            this.toast.success(response.data.message)
          } else {
            this.toast.error("Xảy ra lỗi khi xóa công dụng.")
          }
        }catch (e) {
          this.toast.error(e.response.data.detail)
        }
      }else{
        this.toast.error("Xảy ra lỗi khi xóa công dụng.")
      }
    },
  }
}
</script>
<style scoped>
.bg-light {
  background-color: #f8f9fa !important;
}
</style>