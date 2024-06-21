<template>
  <div class="row">
    <div class="card col-sm-8 ">
      <div class="card-body" v-if="item">
        <h5 class="card-title">Thông tin sản phẩm</h5>
        <form class="g-3">
          <div class="mb-3">
            <p>Tên sản phẩm : {{ item.name }}</p>
          </div>
          <div class="mb-3">
            <p>Tên đường dẫn : {{ item.url }}</p>
          </div>
          <div class="d-flex mb-3">
            <div class="flex-fill">
              <p>Số lượng còn lại trong kho: <span class="badge bg-warning">{{ item.quantity}}</span></p>
            </div>
            <div class="flex-fill">
              <p>Giá gốc: <span class="badge bg-primary">{{ item.price }}đ</span></p>
            </div>
            <div class="flex-fill">
              <p>Giá khuyến mãi: <span class="badge bg-success">{{ item.sale }}đ</span></p>
            </div>
          </div>
          <div class="mb-3">
            Trạng thái : <span class="badge bg-success" v-if="item.status">Hoạt động</span> <span class="badge bg-dark" v-if="!item.status">Tạm ẩn</span>
          </div>
          <div class="mb-3">
            Danh mục : <span v-for="(item,index) in item.categories" :key="index"> <span class="badge bg-secondary m-1">{{item.name}}</span> </span>
          </div>
          <div class="mb-3">
            Công dụng : <span v-for="(item,index) in item.uses" :key="index"> <span class="badge bg-secondary m-1">{{item.name}}</span> </span>
          </div>
          <div class="mb-3">
            Dạng bào chế : <span v-for="(item,index) in item.dosageForms" :key="index"> <span class="badge bg-secondary m-1">{{item.name}}</span> </span>
          </div>
          <div class="mb-3">
            Chống chỉ định : <span v-for="(item,index) in item.contraindications" :key="index"> <span class="badge bg-secondary m-1">{{item.name}}</span> </span>
          </div>
          <div class="mb-3">
            Nhà sản xuất : <span class="badge bg-secondary m-1">{{item.manufacturer.name}}</span>
          </div>
          <div class="mb-3">
            Ngày tạo : <span class="badge bg-primary">{{formatDate(item.createdOn)}}</span>
          </div>
          <div class="mb-3">
            Nhà thuốc : <span class="badge bg-info">{{item.createdBy.name}}</span>
          </div>
        </form>
      </div>
    </div>
    <div class="card col-sm-4">
      <h5 class="card-title">Hình ảnh sản phẩm</h5>
      <div class="grid-container">
        <div class="grid-item p-2" v-for="(image,index) in listImage">
          <img :src="'data:image/jpeg;base64,'+image.file" alt="" id="uploadedAvatar" />
        </div>
      </div>
    </div>
  </div>
  <div class="row">
    <div class="card col-sm-8" v-if="item">
      <div class="card-body">
        <h5 class="card-title">Giới thiệu thuốc : {{item.name}}</h5>
        <p v-if="item.content">{{item.content}}</p>
      </div>
    </div>
    <div class="card col-sm-4">
      <h5 class="card-title">Bình luận</h5>
      <span>Chưa có bình luận nào</span>
    </div>
  </div>
</template>
<script>

import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import ProductService from "@/services/product.service.js";
import ImageService from "@/services/image.service.js";
import { formatDate } from '@/helpers/time.utils.js';

export default {
  name: "ProductOverview",
  setup(){
    const item = ref()
    const route = useRoute();
    const listImage = ref([])
    let url = ref();
    onMounted(async ()=>{
      url = route.params.url;
      await ProductService.getOneByUrl(url).then(response=>{
        item.value = response.data
        for(let image of response.data.images){
          ImageService.getOne(image).then(res=>{
            listImage.value.push(res.data)
          })
        }
      })
    })
    return{
      item,listImage
    }
  },
  methods: {
    formatDate(datetime) {
      return formatDate(datetime);
    },
  },

}
</script>
<style scoped>
.grid-container {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 10px;
}
.grid-item img {
  width: 100%;
  height: auto;
}

</style>