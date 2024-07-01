<template>
  <div class="card">
    <h5 class="card-header">Danh sách sản phẩm</h5>
    <div class="table-responsive" v-if="products && products.length">
      <table class="table">
        <thead class="table-light">
        <tr>
          <th>Tên sản phẩm</th>
          <th>Hình ảnh</th>
          <th>Giá bán</th>
          <th>Cửa hàng</th>
          <th>Trạng thái</th>
          <th>Actions</th>
        </tr>
        </thead>
        <tbody class="table-border-bottom-0">
        <tr v-if="listImage" v-for="(item, index) in products" :key="index">
          <td><span class="fw-medium">{{item.name}}</span></td>
          <td>
            <img :src="'data:image/jpeg;base64,' + listImage.find(img=>img.productId === item.id)['image'].file" alt="Avatar" style="width: 3rem">
          </td>
          <td>{{item.price}}</td>
          <td>{{item.createdBy.name}}</td>
          <td>
            <span class="badge bg-success" v-if="item.status">Hoạt động</span>
          </td>
          <td>
            <router-link :to="`products/${item.url}`" class="btn btn-secondary btn-sm me-3">
              <i class="bi bi-eye"></i>
            </router-link>
            <router-link :to="`products/${item.id}/update`" class="btn btn-warning btn-sm me-3">
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
import ProductService from "@/services/product.service.js";
import ImageService from "@/services/image.service.js";

export default {
  name: "ProductList",
  mounted() {
    document.title='Danh sách sản phẩm'
  },
  setup(){
    const toast = useToast()
    const products = ref([])
    const message = ref()
    const listImage = ref([])
    onMounted(async ()=>{
      await ProductService.getAll().then(response=>{
        products.value = response.data
        for(let item of products.value){
          ImageService.getOne(item.imageId).then(res=>{
            listImage.value.push({image:res.data, productId: item.id})
          })
        }
      }).catch(err=>{
        if (err.response.data.status === 404){
          message.value = err.response.data.detail
        }else{
          this.toast.error("Lỗi khi tải dữ liệu.")
        }
      })
    })
    return{
      products,listImage,message, toast
    }
  },
  methods:{
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn xóa dữ liệu này không ?')) {
        this.deleteItem(item);
      }
    },
    async deleteItem(item) {
      let index = this.products.findIndex(pro=>pro.id === item.id)
      if (index > -1){
        ProductService.deleteOne(item.id).then(async response =>{
          this.products.splice(index,1);
          this.toast.success(response.data.message)
        }).catch(error=>{
          this.toast.error(error.response.data.message)
        })
      }
    }
  }
}
</script>
<style scoped>
.bg-light {
  background-color: #f8f9fa !important;
}
</style>