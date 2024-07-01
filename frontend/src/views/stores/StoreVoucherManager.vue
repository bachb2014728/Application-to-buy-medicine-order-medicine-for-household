<template>
  <div class="card" v-if="store">
    <h5 class="card-header d-flex justify-content-between" v-if="store">
      <span>Danh sách khuyến mãi</span>
      <router-link :to="{ path: `/stores/${store.url}/vouchers/create`, query: { value: store.id } }"
                   class="btn btn-sm btn-success" style="text-decoration: none">
        <span>Thêm mã khuyến mãi</span>
      </router-link>
    </h5>
    <div class="card-body">
      <div class="table-responsive text-nowrap">
        <table class="table table-bordered">
          <thead>
          <tr>
            <th>stt</th>
            <th>Tiêu đề</th>
            <th>Phúc lợi</th>
            <th>Ngày bắt đầu</th>
            <th>Ngày kết thúc</th>
            <th>Trạng thái</th>
            <th></th>
          </tr>
          </thead>
          <tbody>
          <tr v-for="(voucher,index) in vouchers" :key="index" v-if="vouchers && vouchers.length">
            <td>{{index + 1}}</td>
            <td>
              <span class="fw-medium">{{voucher.title}}</span>
            </td>
            <td>
              <span>Giảm theo phần trăm : {{voucher.discountPercentage}}%</span><br>
              <span>Giảm theo tiền cụ thể : {{voucher.discountAmount}}</span>
            </td>
            <td>{{FormatDate(voucher.startTime)}}</td>
            <td>{{FormatDate(voucher.endTime)}}</td>
            <td>
              <small class="text-primary fw-medium" v-if="voucher.remainingTime.status === 1">
                <span>Thời gian còn lại :</span><br> {{ voucher.remainingTime.time }}
              </small>
              <small class="text-danger fw-medium" v-if="voucher.remainingTime.status === 0">
                <span>Thời gian còn lại :</span><br> {{ voucher.remainingTime.time }}
              </small>
              <small class="text-success fw-medium" v-if="voucher.remainingTime.status === -1">
                <span>Khuyến mãi bắt đầu sau :</span><br> {{ voucher.remainingTime.time }}
              </small>
            </td>
            <td>
              <button type="button" class="btn btn-sm" data-bs-toggle="modal" data-bs-target="#basicModal"
                      @click="selectedItem = voucher">
                <i class="bx bx-edit-alt me-1"></i><span>Cập nhật</span>
              </button><br>
              <div class="modal fade" id="basicModal" tabindex="-1" aria-hidden="true">
                <div class="modal-dialog" role="document">
                  <div class="modal-content" v-if="selectedItem">
                    <div class="modal-header">
                      <h5 class="modal-title" id="exampleModalLabel1">Cập nhật khuyến mãi</h5>
                      <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                    </div>
                    <div class="modal-body">
                      <div class="row">
                        <div class="col mb-3">
                          <label for="title" class="form-label">Tiêu đề</label>
                          <input type="text" id="title" class="form-control" v-model="selectedItem.title">
                        </div>
                      </div>
                      <div class="row mb-3">
                        <div class="col-sm">
                          <label class="form-label" for="discountAmount">Số tiền được giảm (nếu có) : {{selectedItem.discountAmount}}</label>
                          <input type="number" class="form-control" id="discountAmount" name="discountAmount" v-model="selectedItem.discountAmount">
                        </div>
                        <div class="col-6">
                          <label class="form-label" for="discountPercentage">Phần trăm được giảm (nếu có): {{selectedItem.discountPercentage}}%</label>
                          <input type="number" min="0" max="100" class="form-control" id="discountPercentage" v-model="selectedItem.discountPercentage">
                        </div>
                      </div>
                      <div class="row mb-3">
                        <div class="col-sm-6">
                          <label class="form-label" for="startTime">Thời gian bắt đầu : {{FormatDate(selectedItem.startTime)}}<span class="badge bg-info"></span></label>
                          <input class="form-control" id="startTime" type="datetime-local" v-model="selectedItem.startTime">
                        </div>
                        <div class="col-sm-6">
                          <label class="form-label" for="endTime">Thời gian kết thúc : {{FormatDate(selectedItem.endTime)}}<span class="badge bg-success"></span></label>
                          <input class="form-control" id="endTime" type="datetime-local" v-model="selectedItem.endTime">
                        </div>
                      </div>
                    </div>
                    <div class="modal-footer">
                      <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
                        Close
                      </button>
                      <button @click="updateItem(selectedItem)" type="button" class="btn btn-primary">Lưu thay đổi</button>
                    </div>
                  </div>
                </div>
              </div>
              <button @click="confirmDelete(voucher)" class="btn btn-sm">
                <i class="bx bx-trash me-1"></i><span>Xóa</span>
              </button>
            </td>
          </tr>
          <tr v-else><td class="text-center" colspan="7">Không có mã khuyến mãi nào.</td></tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>

</template>
<script>
import {useRoute} from "vue-router";
import {useToast} from "vue-toastification";
import {onMounted, onUnmounted, ref} from "vue";
import {formatDate} from "@/utils/time.utils.js";
import StoreService from "@/services/store.service.js";
import VoucherService from "@/services/voucher.service.js";

export default {
  name: "StoreVoucherManager",
  mounted() {
    document.title='Quản lý khuyến mãi'
  },
  setup(){
    const route =useRoute()
    const toast = useToast()
    const selectedItem = ref()
    const vouchers = ref([])
    const store = ref()
    let url = ref()
    onMounted(async ()=>{
      url = route.params.url
      StoreService.getOneByURL(url).then(response =>{
        store.value = response.data
        VoucherService.getAllVoucherOfStore(response.data.id).then(res=>{
          vouchers.value = res.data.map(item => ({
            ...item,
            remainingTime: '',
            intervalId: null,
            status: 0
          }))
          vouchers.value.forEach(item => {
            item.intervalId = setInterval(() => {
              item.remainingTime = countdown(item.startTime, item.endTime);
              if (item.remainingTime === "Khuyến mãi hết hạn") {
                clearInterval(item.intervalId);
              }
            }, 1000);
          });
        })
      })

    })
    const countdown = (startTime, endTime) => {
      let now = new Date().getTime();
      let start = new Date(startTime).getTime();
      let end = new Date(endTime).getTime();
      let distance;

      if (start > now) {
        distance = start - now;
        let days = Math.floor(distance / (1000 * 60 * 60 * 24));
        let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((distance % (1000 * 60)) / 1000);
        return {
          time : days + " ngày " + hours + " giờ " + minutes + " phút " + seconds + " giây ",
          status : -1
        }
      } else {
        distance = end - now;
        if (distance < 0) {
          return {
            time: "Khuyến mãi hết hạn",
            status: 0
          }
        }
        let days = Math.floor(distance / (1000 * 60 * 60 * 24));
        let hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
        let minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
        let seconds = Math.floor((distance % (1000 * 60)) / 1000);
        return {
          time : days + " ngày " + hours + " giờ " + minutes + " phút " + seconds + " giây ",
          status : 1
        }
      }
    };

    onUnmounted(() => {
      vouchers.value.forEach(item => {
        clearInterval(item.intervalId);
      });
    });
    return{store,toast,vouchers,selectedItem}
  },
  methods:{
    FormatDate(date){
      return formatDate(date)
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
        VoucherService.deleteOne(item.id).then(response=>{
          this.toast.success(response.data.message)
        }).catch(err=>{
          this.toast.error("Xảy ra lỗi khi xóa.")
        })
      }else{
        this.toast.error("Xảy ra lỗi khi xóa.")
      }
    },
    async updateItem(item) {
      let updatedItem = {
        title: this.selectedItem.title,
        discountAmount: this.selectedItem.discountAmount,
        discountPercentage: this.selectedItem.discountPercentage,
        startTime: this.selectedItem.startTime,
        endTime: this.selectedItem.endTime
      };
      try {
        const response = await VoucherService.update(this.selectedItem.id, updatedItem);
        if (response.status === 200) {
          this.toast.success("Cập nhật thành công.");
          let index = this.vouchers.indexOf(this.selectedItem);
          if (index > -1) {
            this.$set(this.vouchers, index, response.data);
          }
        } else {
          this.toast.error("Xảy ra lỗi khi cập nhật.");
        }
      } catch (e) {
        this.toast.error(e.response.data.detail);
      }
    },
  }
}
</script>
<style scoped>

</style>