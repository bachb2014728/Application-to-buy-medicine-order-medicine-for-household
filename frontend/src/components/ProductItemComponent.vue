<template>
  <div class="card h-100 mb-3 position-relative" v-if="product">
    <div class="card-body d-flex flex-column justify-content-between">
      <h5 class="card-title">{{product.name}}</h5>
      <h6 class="card-subtitle text-muted">{{product.createdBy.name}}</h6>
      <a href="">
        <img v-if="image['file']" class="img-fluid d-flex mx-auto my-4 rounded" :src="'data:image/jpeg;base64,'+image.file" alt="Card image cap">
      </a>
      <div class="mt-3 mb-3">
        <p v-if="product.sale===0" class="card-text fw-medium">Giá  : <span >{{convertPrice(product.price)}}</span></p>
        <p v-if="product.sale!==0" class="card-text fw-medium">Giá  :
          <span class="text-decoration-line-through me-2">{{convertPrice(product.price)}}</span>
          <span class="text-danger">{{convertPrice(product.sale)}}</span>
        </p>
      </div>
      <div class="d-flex">
        <button class="btn btn-sm btn-primary" @click="addToCart(product)">Thêm vào giỏ hàng</button>
      </div>
    </div>
    <div class="position-absolute top-0 end-0 translate-middle-y" style="right: -50%;">
      <span v-if="getPercent(product.price,product.sale)" class="badge bg-danger me-2" >Giảm giá : {{getPercent(product.price,product.sale)}}</span>
      <span class="badge bg-warning me-2">Số lượng tồn kho : {{product.quantity}}</span>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import ImageService from "@/services/image.service.js";
import CartService from "@/services/cart.service.js";
import {formatPercentage, formatPrice} from "@/utils/convert.utils.js"
import {useToast} from "vue-toastification";
import {useStore} from "vuex";
import ProductService from "@/services/product.service.js";
import {useRouter} from "vue-router";
export default {
  name: "ProductItemComponent",
  props:['product'],
  setup(props){
    const product = ref()
    const toast = useToast()
    const store = useStore()
    const router = useRouter()
    const image = ref([])
    onMounted(async () => {
      let item = props.product;
      ProductService.getOne(item.id).then(response=>{
        product.value=response.data
        ImageService.getOne(response.data.images[0]).then((res) => {
          image.value = res.data;
        });
      })
    })
    return{product,image,toast,store,router}
  },
  methods:{
    convertPrice(price){
      return formatPrice(price)
    },
    getPercent(price,sale){
      return formatPercentage(price,sale)
    },
    async addToCart(product){
      const data = {productId:product.id,quantity: 1}
      CartService.create(data).then(async response=> {
        this.toast.success("Thêm vào giỏ hàng thành công.")
      }).catch(err=>{
        this.toast.warning("không thể thêm vào giỏ hàng");
        this.router.push("/login")
      })
    }

  }
}
</script>
<style scoped>

</style>