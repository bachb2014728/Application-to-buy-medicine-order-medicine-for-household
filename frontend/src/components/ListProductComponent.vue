<template>
  <div class="row justify-content-start" v-if="products && products.length">
    <div class="col-3" v-for="(item, index) in products" :key="index">
      <div class="card mb-3 position-relative">
        <img v-if="images[index]" class="img-fluid d-flex mx-auto my-4 rounded square-image" :src="'data:image/jpeg;base64,'+images[index].file" alt="Card image cap">
        <div class="card-body">
          <p class="card-text">
            <span class="text-secondary fw-medium"> {{item.name}}</span><br>
            <small class="text-secondary fw-medium"> Giá bán : {{item.price}}</small><br>
          </p>
          <p class="card-text">
            <small class="text-muted"></small>
          </p>
          <div class="d-flex">
            <router-link :to="`/stores/${url}/products/${item.url}/update`" class="btn btn-sm btn-primary me-2">Cập nhật</router-link>
            <a href="" class="btn btn-sm btn-warning text-light me-2">Tạm ẩn</a>
            <a href="" class="btn btn-sm btn-danger">Xóa</a>
          </div>
        </div>
        <div class="position-absolute top-0 end-0 translate-middle-y" style="right: -50%;">
          <span class="badge bg-success me-2" v-if="item.status">Hiển thị</span>
          <span class="badge bg-warning me-2">Số lượng tồn kho : {{item.quantity}}</span>
        </div>
      </div>
    </div>
  </div>
  <div v-else>
    <p>Không có sản phẩm nào.</p>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import ImageService from "@/services/image.service.js";
import {useRoute} from "vue-router";

export default {
  name: "ListProductComponent",
  props:['listProduct'],
  setup(props){
    const route = useRoute();
    const products = ref(props.listProduct)
    const url = ref()
    const images = ref([])
    onMounted(async () => {
      url.value = route.params.url;
      for (let item of props.listProduct){
        ImageService.getOne(item.imageId).then(response=>{
          images.value.push(response.data)
        })
      }
      console.log(products)
    })
    return{
      products,images,url
    }
  }
}
</script>
<style scoped>
.square-image {
  object-fit: cover;
  width: auto;
  max-height: 15rem;
}
</style>