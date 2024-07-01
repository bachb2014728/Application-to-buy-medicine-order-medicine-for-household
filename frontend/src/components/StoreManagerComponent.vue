<template>
<div class="row" v-if="url">
  <div class="col-md-12">
    <ul class="nav nav-pills flex-column flex-md-row mb-3">
      <li class="nav-item">
        <router-link :to="`/stores/${url}/overview`" class="nav-link" exact active-class="active">
          <i class="bx bx-store me-1"></i>Cửa hàng
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/statistical`" class="nav-link" exact active-class="active">
          <i class='bx bx-trending-up me-1' ></i>Thống kê
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/notification`" class="nav-link" exact active-class="active">
          <i class="bx bx-bell me-1"></i>Thông báo
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/products`" class="nav-link" exact active-class="active">
          <i class="bx bx-data me-1"></i>Sản phẩm
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/vouchers`" class="nav-link" exact active-class="active">
          <i class="bx bx-party me-1"></i>Khuyến mãi
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/order-confirm`" class="nav-link" exact active-class="active">
          <i class="bx bx-cart me-1"></i>Duyệt đơn hàng
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/orders`" class="nav-link" exact active-class="active">
          <i class="bx bx-cart me-1"></i>Đơn hàng
        </router-link>
      </li>
      <li class="nav-item">
        <router-link :to="`/stores/${url}/members`" class="nav-link" exact active-class="active">
          <i class="bx bx-user me-1"></i>Khách hàng
        </router-link>
      </li>
    </ul>
  </div>
  <div class="col-12">
    <router-view></router-view>
  </div>
</div>
</template>
<script>
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";

export default {
  name: "StoreManagerComponent",
  setup(){
    const url = ref()
    const route = useRoute()
    onMounted(async()=>{
      url.value = route.params.url;
    })
    return{url}
  }
}
</script>
<style scoped>

</style>