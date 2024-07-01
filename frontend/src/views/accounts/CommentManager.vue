<template>
  <div class="row">
    <div class="card col-sm-8">
      <h5 class="card-header">Danh sách sản phẩm đã mua</h5>
      <div class="table-responsive text-nowrap">
        <table class="table table-sm">
          <thead>
          <tr>
            <th>tên sản phẩm</th>
            <th>giá cả</th>
            <th>cửa hàng</th>
            <th>Status</th>
          </tr>
          </thead>
          <tbody class="table-border-bottom-0">
          <tr v-if="products && products.length" v-for="(product,index) in products" :key="index" @click="selectItem(product)">
            <td><span class="fw-medium">{{product.name}}</span></td>
            <td>{{product.price}}</td>
            <td>{{product.createdBy.name}}</td>
            <td>
              <span v-if="product.status" class="badge bg-label-primary me-1">Hoạt động</span>
              <span v-if="!product.status" class="badge bg-label-secondary me-1">Tạm ẩn</span>
            </td>
          </tr>
          <tr v-else class="text-center">
            <td colspan="4">Không có sản phẩm nào đã mua</td>
          </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="col-sm-4" v-if="selectedItem">
      <div class="card mb-2" style="height: 20rem">
        <h5 class="card-header">Bình luận</h5>
        <div class="card-body">
          <span class="nav-link" v-for="(cmt,num) in comments.find(x=>x.productId === selectedItem.id).comments">
              <div class="alert alert-secondary" role="alert">{{cmt.message}}</div>
          </span>
        </div>
      </div>
      <div class="card">
        <div class="card-body">
          <form @submit.prevent = "addComment(selectedItem)">
            <div class="mb-3">
              <label class="form-label">Message</label>
              <input type="text" class="form-control" name="message" v-model="comment.message">
            </div>
            <div class="text-center">
              <button class="btn btn-sm btn-success">Bình luận</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, reactive, ref} from "vue";
import OrderService from "@/services/order.service.js";
import ProductService from "@/services/product.service.js";
import CommentService from "@/services/comment.service.js";
import data from "bootstrap/js/src/dom/data.js";

export default {
  name: "CommentManager",
  computed: {
    data() {
      return data
    }
  },
  mounted(){
    document.title = 'Quản lý sản phẩm đã mua'
  },
  setup(){
    const products = ref([])
    const comments = ref([])
    const comment = reactive({
      message : '', productId :'', storeId : null
    })
    const selectedItem = ref()
    onMounted(async =>{
      OrderService.getAllOfCustomer().then(resOrder =>{
        const listId = [];
        for (let item of resOrder.data.filter(item=>item.orderStatus === 2)){
          listId.push(item.products.map(item=>item.productId))
        }
        const uniqueNumbers = new Set();

        listId.forEach(arr => {
          arr.forEach(number => {
            uniqueNumbers.add(number);
          });
        });

        const uniqueNumberArray = Array.from(uniqueNumbers);

        for (let productId of uniqueNumberArray){
          ProductService.getOne(productId).then(resProduct=>{
            CommentService.getAll(resProduct.data.id).then(resComment=>{
              let message = {comments: resComment.data , productId: resProduct.data.id}
              comments.value.push(message)
            })
            products.value.push(resProduct.data)
            selectedItem.value = products.value[0]
          })
        }

      })
    })
    return{products,comments,selectedItem,comment}
  },
  methods:{
    selectItem(product){
      this.selectedItem = product;
    },
    addComment(data){
      const formData = {message : this.comment.message , storeId : null, productId: data.id }
      CommentService.addComment(formData).then(response=>{
        let comment = this.comments.find(item=>item.productId === data.id)
        if ( comment === null){
          this.comments.push({comments: response.data , productId: data.id})
        }else{
          let indexCom = this.comments.indexOf(comment);
          this.comments[indexCom].comments.push(response.data)
        }
      })
    }
  }
}
</script>
<style scoped>

</style>