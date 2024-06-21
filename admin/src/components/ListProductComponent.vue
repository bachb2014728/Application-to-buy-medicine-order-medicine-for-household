<template>
  <div class="row justify-content-start" v-if="products">
    <div class="col-3" v-for="(item, index) in products" :key="index">
      <div class="card mb-3 position-relative">
        <img v-if="images[index]" class="card-img-top square-image" :src="'data:image/jpeg;base64,'+images[index].file" alt="Card image cap">
        <div class="card-body">
          <p class="card-text">
            <span class="text-secondary fw-medium"> {{item.name}}</span><br>
            <small class="text-secondary fw-medium"> Giá bán : {{item.price}}</small><br>
          </p>
          <p class="card-text">
            <small class="text-muted"></small>
          </p>
          <div class="d-flex">
            <a href="" class="btn btn-sm btn-primary me-2">Cập nhật</a>
            <a href="" class="btn btn-sm btn-warning text-light me-2">Tạm ẩn</a>
            <a href="" class="btn btn-sm btn-danger">Xóa</a>
          </div>
        </div>
        <div class="position-absolute top-0 end-0 translate-middle-y" style="right: -50%;">
          <span class="badge bg-warning me-2">Số lượng tồn kho : {{item.quantity}}</span>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import ImageService from "@/services/image.service.js";

export default {
  name: "ListProductComponent",
  props:['listProduct'],
  setup(props){
    const products = ref(props.listProduct)
    const images = ref([])
    onMounted(async () => {
      for (let item of props.listProduct){
        ImageService.getOne(item.imageId).then(response=>{
          images.value.push(response.data)
        })
      }
    })
    return{
      products,images
    }
  }
}
</script>
<style scoped>
.square-image {
  object-fit: cover;
  width: auto;
  height: 15rem;
}
</style>