<template>
  <nav class="mb-3">
    <ol class="breadcrumb">
      <li class="breadcrumb-item">
        <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
      </li>
      <li class="breadcrumb-item">Khuyến mãi</li>
    </ol>
  </nav>
  <div class="row justify-content-start" v-if="data && data.length">
    <div class="col-4 mb-3" v-for="(item, index) in data" :key="index">
      <div class="card mb-3 position-relative">
        <div class="row">
          <div class="col-4">
            <router-link to="/">
              <img class="card-img card-img-left m-1" src="@/assets/images/reduce-cost.gif" alt="Card image">
            </router-link>
          </div>
          <div class="col-8">
            <div class="card-body">
              <span class="fw-medium text-primary">{{item.title}}</span>
              <p class="card-text">
                <small class="text-secondary">Bắt đầu : {{formatDate(item.startTime)}}</small><br>
                <small class="text-info fw-medium" v-if="item.remainingTime.status === 1">
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
        <div class="position-absolute top-0 end-0 translate-middle-y" style="right: -50%;"
             v-if="!myVoucher.some(voucher=>voucher.id === item.id)">
          <a @click="AddToVoucher(item)" class="btn btn-sm btn-warning me-2">Thêm khuyến mãi</a>
        </div>
      </div>
    </div>
  </div>
  <div v-else>
    <p>Không có mã khuyến mãi nào.</p>
  </div>
</template>
<script>
import {onMounted, onUnmounted, ref} from "vue";
import VoucherService from "@/services/voucher.service.js";
import {formatDate} from "@/utils/time.utils.js";
import {useToast} from "vue-toastification";
import {useRouter} from "vue-router";

export default {
  name: "VoucherList",
  mounted() {
    document.title='Mã khuyến mãi'
  },
  setup(){
    const toast = useToast();
    const router = useRouter()
    const data = ref([]);
    const myVoucher =ref([])
    onMounted(async ()=>{
      VoucherService.getAll().then(response=>{
        data.value = response.data.map(item => ({
          ...item,
          remainingTime: '',
          intervalId: null,
          status: 0,
        }))
        data.value.forEach(item => {
          item.intervalId = setInterval(() => {
            item.remainingTime = countdown(item.startTime, item.endTime);
            if (item.remainingTime === "Khuyến mãi hết hạn") {
              clearInterval(item.intervalId);
            }
          }, 1000);
        });
      })
      VoucherService.myVoucher().then(response=>{
        myVoucher.value=response.data
      }).catch(err=>{
        myVoucher.value = []
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
      data.value.forEach(item => {
        clearInterval(item.intervalId);
      });
    });
    return{data,toast,myVoucher,router}
  },
  methods:{
    formatDate(datetime) {
      return formatDate(datetime);
    },
    AddToVoucher(item){
      let index = this.data.indexOf(item)
      const data = {voucherId : item.id}
      VoucherService.addToVoucher(data).then(response=>{
        this.myVoucher.push(item);
        this.toast.success("Thêm khuyến mãi thành công.")
      }).catch(err=>{
        this.toast.warning("Thêm khuyến mãi thất bại.")
        this.router.push("/login")
      })
    }
  }
}
</script>
<style scoped>

</style>