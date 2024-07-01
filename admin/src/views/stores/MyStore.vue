<template>
  <div class="card">
    <h5 class="card-header">Danh sách nhà thuốc </h5>
    <div class="table-responsive text-nowrap" v-if="listItem && listItem.length">
      <table class="table">
        <thead class="table-light">
          <tr>
            <th>STT</th>
            <th>Tên nhà thuốc</th>
            <th>Ảnh đại diện</th>
            <th>Trạng thái</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody class="table-border-bottom-0">
        <tr v-for="(item, index) in listItem" :key="index">
          <td>{{index+1}}</td>
          <td><span class="fw-medium">{{item.name}}</span></td>
          <td >
            <img v-if="images[index]" :src="'data:image/jpeg;base64,' + images[index].file"
                 alt="Avatar" class="rounded-circle" style="width: 1.5rem">
            <img v-else src="@/assets/image/user.png" alt="Avatar" class="rounded-circle" style="width: 1.5rem">
          </td>
          <td>
            <span v-if="item.status === 'ACTIVE'" class="badge bg-label-success me-1">Hoạt động</span>
            <span v-if="item.status === 'BLOCK'" class="badge bg-label-danger me-1">Tạm khóa</span>
          </td>
          <td>
            <router-link :to="`/stores/${item.url}`" class="btn btn-secondary btn-sm me-3">
              <i class="bi bi-eye"></i>
            </router-link>
            <router-link :to="`/stores/my-store/${item.url}/update`" class="btn btn-warning btn-sm me-3">
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
      <p>Không có tài khoản nào</p>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import StoreService from "@/services/store.service.js";
import ImageService from "@/services/image.service.js";
import {useToast} from "vue-toastification";
import ProductService from "@/services/product.service.js";
import OrderService from "@/services/order.service.js";

export default {
  name: "MyStore",
  mounted() {
    document.title='Danh sách của thuốc của tôi'
  },
  setup(){
    const listItem = ref([])
    const images = ref([])
    const toast = useToast()
    onMounted(async ()=>{
      StoreService.getMyStore().then(response=>{
        listItem.value = response.data
        for(let i = 0 ; i < response.data.length ; i++){
          let id = response.data[i].avatar;
          ImageService.getOne(id).then(res=>{
            images.value.push(res.data)
          })
        }
      }).catch(err=>{
        if (err.response.status === 404){
          message.value = err.response.data.detail
        }else {
          this.toast.success("Lỗi khi tải dữ liệu.")
        }
      })
    })
    return{
      listItem, toast, images
    }
  },
  methods:{
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn khóa tài khoản này không ?')) {
        this.deleteItem(item);
      }
    },
    async deleteItem(item) {
      await StoreService.deleteOne(item.id).then(async response =>{
        console.log(response)
        this.toast.success(response.data.message)
        let list = await StoreService.getAll();
        this.listMyStore = list.data
      }).catch(error=>{
        this.toast.error(error.response.data.message)
      })
    }
  }
}
</script>
<style scoped>

</style>