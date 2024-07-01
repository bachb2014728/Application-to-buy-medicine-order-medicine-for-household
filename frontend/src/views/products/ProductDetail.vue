<template>
  <nav class="mb-3">
    <ol class="breadcrumb">
      <li class="breadcrumb-item">
        <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
      </li>
      <router-link to="/products" class="breadcrumb-item">Sản phẩm</router-link>
      <a class="breadcrumb-item" v-if="product">{{product.name}}</a>
    </ol>
  </nav>
  <div class="row" v-if="product">
    <div class="card col-sm-5">
      <div id="carouselExample" class="carousel slide" data-bs-ride="carousel" v-if="images && images.length">
        <div class="carousel-indicators">
          <button type="button" data-bs-target="#carouselExample" data-bs-slide-to="0" class="active" aria-label="Slide 1" aria-current="true"></button>
          <button type="button" data-bs-target="#carouselExample" data-bs-slide-to="1" aria-label="Slide 2" class=""></button>
          <button type="button" data-bs-target="#carouselExample" data-bs-slide-to="2" aria-label="Slide 3" class=""></button>
        </div>
        <div class="carousel-inner">
          <div class="carousel-item active" >
            <img class="d-block card-img-top" :src="'data:image/jpeg;base64,'+images[0].file" alt="First slide">
          </div>
          <div class="carousel-item"  v-for="(image,index) in images">
            <img class="d-block card-img-top" :src="'data:image/jpeg;base64,'+image.file" alt="First slide">
          </div>
        </div>
        <a class="carousel-control-prev" href="#carouselExample" role="button" data-bs-slide="prev">
          <span class="carousel-control-prev-icon" aria-hidden="true"></span>
          <span class="visually-hidden">Previous</span>
        </a>
        <a class="carousel-control-next" href="#carouselExample" role="button" data-bs-slide="next">
          <span class="carousel-control-next-icon" aria-hidden="true"></span>
          <span class="visually-hidden">Next</span>
        </a>
      </div>
      <div class="card-body">
        <h5 class="card-title">{{product.name}}</h5>
        <p class="card-text"></p>
        <p class="card-text">
          <small class="text-muted">{{formatDate(product.createdOn)}}</small>
        </p>
      </div>
    </div>
    <div class="col-sm">
      <div class="nav-align-top mb-4">
        <ul class="nav nav-pills mb-3" role="tablist">
          <li class="nav-item" role="presentation">
            <button type="button" class="nav-link active" role="tab" data-bs-toggle="tab" data-bs-target="#navs-pills-top-home" aria-controls="navs-pills-top-home" aria-selected="true">
              Thông tin chi tiết
            </button>
          </li>
          <li class="nav-item" role="presentation">
            <button type="button" class="nav-link" role="tab" data-bs-toggle="tab" data-bs-target="#navs-pills-top-profile" aria-controls="navs-pills-top-profile" aria-selected="false" tabindex="-1">
              Mô tả sản phẩm
            </button>
          </li>
          <li class="nav-item" role="presentation">
            <button type="button" class="nav-link" role="tab" data-bs-toggle="tab" data-bs-target="#navs-pills-top-messages" aria-controls="navs-pills-top-messages" aria-selected="false" tabindex="-1">
              Bình luận
            </button>
          </li>
        </ul>
        <div class="tab-content">
          <div class="tab-pane fade active show" id="navs-pills-top-home" role="tabpanel">
            <h4 class="mb-1">{{product.name}}</h4>
            <div class="mb-3">
              <span v-if="product.quantity > 1" class="badge btn-success">Còn hàng : {{product.quantity}}</span>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Thuộc danh mục</span>
              <ul>
                <li class="nav-link" v-for="(cate,num) in product.categories" :key="num">{{cate.name}}</li>
              </ul>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Sản phẩm của nhà thuốc : {{product.createdBy.name}}</span>
            </div>

            <div class="mb-3">
              <p v-if="product.sale===0" class="card-text fw-medium">Giá  : <span >{{convertPrice(product.price)}}</span></p>
              <p v-if="product.sale!==0" class="card-text fw-medium">Giá  :
                <span class="text-decoration-line-through me-2">{{convertPrice(product.price)}}</span>
                <span class="text-danger">{{convertPrice(product.sale)}}</span>
              </p>
            </div>
            <div class="d-flex justify-content-between">
              <label for="html5-number-input" class="col-form-label">Chọn số lượng</label>
              <input min="1" class="form-control" type="number" name="quantity" v-model="quantity" id="html5-number-input">
              <button class="btn btn-sm btn-success" @click="addToCart(product,quantity)">Thêm vào giỏ hàng</button>
            </div>
          </div>
          <div class="tab-pane fade" id="navs-pills-top-profile" role="tabpanel">
            <div class="mb-3">
              <span class="fw-medium">Công dụng : </span>
              <span class="badge bg-label-info me-2" v-for="(use,num) in product.uses" >{{use.name}}</span>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Dạng bào chế : </span>
              <span class="badge bg-label-secondary me-2" v-for="(con,num) in product.dosageForms">
                {{con.name}}
              </span>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Nhà sản xuất : </span>
              <span class="badge bg-label-success me-2">{{product.manufacturer.name}}</span>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Chống chỉ định : </span><br>
              <span class="badge bg-label-primary me-2" v-for="(con,num) in product.contraindications">{{con.name}}</span><br>
            </div>
            <div class="mb-3">
              <span class="fw-medium">Mô tả : </span><br>
              <p>{{product.content}}</p>
            </div>
          </div>
          <div class="tab-pane fade" id="navs-pills-top-messages" role="tabpanel">
            <div class="card-body">
              <ul class="p-0 m-0">
                <li class="d-flex mb-4 pb-1" v-if="comments" v-for="(cmt,nu) in comments" :key="nu">
                  <div class="avatar flex-shrink-0 me-3">
                    <img src="@/assets/images/user.png" alt="User" class="rounded">
                  </div>
                  <div class="d-flex w-100 flex-wrap align-items-center justify-content-between gap-2">
                    <div class="me-2">
                      <small class="text-muted d-block mb-1">{{cmt.customer.firstName}} {{cmt.customer.lastName}}</small>
                      <h6 class="mb-0">{{cmt.message}}</h6>
                    </div>
                    <div class="user-progress d-flex align-items-center gap-1">
                      <span class="text-muted">{{formatDate(cmt.createdOn)}}</span>
                    </div>
                  </div>
                </li>
              </ul>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import ProductService from "@/services/product.service.js";
import {onMounted, ref} from "vue";
import {useRoute, useRouter} from "vue-router";
import ImageService from "@/services/image.service.js";
import {formatPercentage, formatPrice} from "@/utils/convert.utils.js";
import CartService from "@/services/cart.service.js";
import {useToast} from "vue-toastification";
import {formatDate} from "@/utils/time.utils.js";
import CommentService from "@/services/comment.service.js";

export default {
  name: "ProductDetail",
  props:['product'],
  mounted() {
    document.title='Xem chi tiết sản phẩm'
  },
  setup(){
    let productURL = ref()
    const route = useRoute()
    const router = useRouter()
    const comments =ref([])
    const quantity = ref()
    const toast =useToast()
    const product = ref()
    const images = ref([])
    onMounted(async ()=>{
      productURL = route.params.productURL
      ProductService.getOneByURL(productURL).then(resProduct=>{
        product.value=resProduct.data
        for (let item of resProduct.data.images){
          ImageService.getOne(item).then(resImage=>{
            images.value.push(resImage.data)
          })
        }
        CommentService.getAll(resProduct.data.id).then(resComment=>{
          comments.value = resComment.data
        }).catch(err=>{console.log(err.response.data)})
      }).catch(err=>{console.log(err.response.data)})
      quantity.value = 1
    })
    return{product,images,toast,quantity,comments,route,router}
  },
  methods:{
    formatDate,
    convertPrice(price){
      return formatPrice(price)
    },
    getPercent(price,sale){
      return formatPercentage(price,sale)
    },
    async addToCart(product){
      const data = {productId:product.id,quantity: this.quantity}
      CartService.create(data).then(async response=> {
        console.log(response)
        this.toast.success("Thêm vào giỏ hàng thành công.")
      }).catch(err=>{
        this.toast.warning("Không thể thêm vào giỏ hàng.")
        this.router.push("/login")
      })
    }
  }
}
</script>
<style scoped>

</style>