<template>
  <div class="row justify-content-start" v-if="listItems">
    <div class="col-4 mb-3" v-for="(item, index) in listItems" :key="index">
      <div class="card mb-3 position-relative">
        <div class="row">
          <div class="col-4">
            <router-link to="/vouchers">
              <img class="card-img card-img-left m-1" src="../assets/image/reduce-cost.gif" alt="Card image">
            </router-link>
          </div>
          <div class="col-8">
            <div class="card-body">
              <small class="fw-medium">{{item.title}}</small>
              <p class="card-text">
                <small class="text-secondary">Bắt đầu : {{formatDate(item.startTime)}}</small><br>
                <small class="text-primary fw-medium" v-if="item.remainingTime.status === 1">
                  Thời gian còn lại : {{ item.remainingTime.time }}
                </small>
                <small class="text-danger fw-medium" v-if="item.remainingTime.status === 0">
                  Thời gian còn lại : {{ item.remainingTime.time }}
                </small>
                <small class="text-success fw-medium" v-if="item.remainingTime.status === -1">
                  Khuyến mãi bắt đầu sau : {{ item.remainingTime.time }}
                </small>
              </p>
            </div>
          </div>
        </div>
        <div class="position-absolute top-0 end-0 translate-middle-y" style="right: -50%;">
          <button type="button" class="btn btn-primary btn-sm me-2"
                  data-bs-toggle="modal" data-bs-target="#basicModal" @click="selectedItem = item">
            Cập nhật
          </button>
          <a @click="confirmDelete(item)" class="btn btn-sm btn-danger me-2">Xóa</a>
        </div>
      </div>
    </div>
  </div>
  <div class="modal fade" id="basicModal" tabindex="-1" aria-hidden="true" style="display: none;" >
    <div class="modal-dialog modal-xl" role="document">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="exampleModalLabel1">Cập nhật khuyến mãi</h5>
          <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
        </div>
        <div class="modal-body" v-if="selectedItem">
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
              <input type="range" min="0" max="100" class="form-range" id="discountPercentage" v-model="selectedItem.discountPercentage">
            </div>
          </div>
          <div class="row mb-3">
            <div class="col-sm-6">
              <label class="form-label" for="startTime">Thời gian bắt đầu : {{formatDate(selectedItem.startTime)}}<span class="badge bg-info"></span></label>
              <input class="form-control" id="startTime" type="datetime-local" v-model="selectedItem.startTime">
            </div>
            <div class="col-sm-6">
              <label class="form-label" for="endTime">Thời gian kết thúc : {{formatDate(selectedItem.endTime)}}<span class="badge bg-success"></span></label>
              <input class="form-control" id="endTime" type="datetime-local" v-model="selectedItem.endTime">
            </div>
          </div>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
            Close
          </button>
          <button @click="updateItem(item)" type="button" class="btn btn-primary">Lưu thay đổi</button>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import { ref, onMounted, onUnmounted } from "vue";
import { formatDate } from "@/helpers/time.utils.js";
import VoucherService from "@/services/voucher.service.js";
import {useToast} from "vue-toastification";

export default {
  name: "ListVoucherComponent",
  props:['listVoucher'],
  setup(props) {
    const toast = useToast()
    const selectedItem = ref();
    const listItems = ref(props.listVoucher.map(item => ({
      ...item,
      remainingTime: '',
      intervalId: null,
      status: 0
    })));

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

    onMounted(() => {
      listItems.value.forEach(item => {
        item.intervalId = setInterval(() => {
          item.remainingTime = countdown(item.startTime, item.endTime);
          if (item.remainingTime === "Khuyến mãi hết hạn") {
            clearInterval(item.intervalId);
          }
        }, 1000);
      });
    });

    onUnmounted(() => {
      listItems.value.forEach(item => {
        clearInterval(item.intervalId);
      });
    });

    return { listItems,toast,selectedItem };
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
      let index = this.listItems.indexOf(item);
      if (index > -1) {
        this.listItems.splice(index, 1);
        try {
          const response = await VoucherService.deleteOne(item.id);
          if (response.status === 200) {
            this.toast.success(response.data.message)
          } else {
            this.toast.error("Xảy ra lỗi khi xóa.")
          }
        }catch (e) {
          this.toast.error(e.response.data.detail)
        }
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
          let index = this.listItems.indexOf(this.selectedItem);
          if (index > -1) {
            this.$set(this.listItems, index, response.data);
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