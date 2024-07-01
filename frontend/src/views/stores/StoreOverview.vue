<template>
  <div class="row mb-5" v-if="stores && stores.length">
    <div class="col-sm-3" v-for="(item,index) in stores" :key="index">
      <div class="card text-center mb-3">
        <div class="card-body">
          <h5 class="card-title">{{item.name}}</h5>
          <p class="card-text">Địa chỉ : {{item.address}}</p>
          <router-link :to="`/stores/${item.url}/overview`" class="btn btn-primary">Đến cửa hàng</router-link>
        </div>
      </div>
    </div>
  </div>
  <div v-else class="text-center">Không có tài khoản nhà thuốc nào.</div>
</template>
<script>
import {onMounted, ref} from "vue";
import StoreService from "@/services/store.service.js";

export default {
  name: "StoreOverview",
  mounted() {
    document.title = 'Danh sách cửa hàng'
  },
  setup(){
    const stores = ref([])
    onMounted(async () =>{
      StoreService.getMyStore().then(response =>{
        stores.value = response.data
        console.log(response.data)
      })
    })
    return{stores}
  }
}
</script>
<style scoped>

</style>