<template>
  <div class="col">
    <h6 class="text-muted">Cửa hàng</h6>
    <div class="nav-align-top mb-4" v-if="data">
      <ul class="nav nav-tabs nav-fill" role="tablist">
        <li class="nav-item" role="presentation">
          <button type="button" class="nav-link active" role="tab" data-bs-toggle="tab" data-bs-target="#navs-justified-product" aria-controls="navs-justified-product" aria-selected="true">
            <i class="tf-icons bx bx-home me-1"></i><span class="d-none d-sm-block">Sản phẩm</span>
            <span class="badge rounded-pill badge-center h-px-20 w-px-20 bg-label-danger ms-1">{{data.products.length}}</span>
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button type="button" class="nav-link" role="tab" data-bs-toggle="tab" data-bs-target="#navs-justified-voucher" aria-controls="navs-justified-voucher" aria-selected="false" tabindex="-1">
            <i class="tf-icons bx bx-user me-1"></i><span class="d-none d-sm-block">Mã khuyến mãi</span>
            <span class="badge rounded-pill badge-center h-px-20 w-px-20 bg-label-danger ms-1">{{data.vouchers.length}}</span>
          </button>
        </li>
        <li class="nav-item" role="presentation">
          <button type="button" class="nav-link" role="tab" data-bs-toggle="tab" data-bs-target="#navs-justified-info" aria-controls="navs-justified-info" aria-selected="false" tabindex="-1">
            <i class="tf-icons bx bx-message-square me-1"></i><span class="d-none d-sm-block"> Thông tin cửa hàng</span>
          </button>
        </li>
      </ul>
      <div class="tab-content">
        <div class="tab-pane fade active show" id="navs-justified-product" role="tabpanel" v-if="data.products">
          <ListProductComponent :list-product="data.products"/>
        </div>
        <div class="tab-pane fade" id="navs-justified-voucher" role="tabpanel" v-if="data.vouchers">
         <ListVoucherComponent :listVoucher="data.vouchers"/>
        </div>
        <div class="tab-pane fade" id="navs-justified-info" role="tabpanel">
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import ListVoucherComponent from "@/components/ListVoucherComponent.vue";
import {ref} from "vue";
import ListProductComponent from "@/components/ListProductComponent.vue";

export default {
  name: "FilledTabComponent",
  components: {ListProductComponent, ListVoucherComponent},
  props:['data'],
  setup(props){
    const data = ref(props.data);
    console.log(data)
    return{data}
  }
}
</script>
<style scoped>

</style>