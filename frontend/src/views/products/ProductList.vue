<template>
  <nav class="mb-3">
    <ol class="breadcrumb">
      <li class="breadcrumb-item">
        <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
      </li>
      <li class="breadcrumb-item">Sản phẩm</li>
    </ol>
  </nav>
  <div class="row mb-3">
    <div class="col-sm">
      <div class="mb-3">
        <label for="functionSelect" class="form-label">Chức năng</label>
        <select class="form-select" id="functionSelect" v-model="selectedUse"  @change="filterProducts">
          <option value="null" disabled selected>Chọn chức năng</option>
          <option v-for="(use,index) in uses" :key="index" :value="use">{{use.name}}</option>
        </select>
      </div>
    </div>
    <div class="col-sm">
      <div class="mb-3">
        <label for="categorySelect" class="form-label">Danh mục</label>
        <select class="form-select" id="categorySelect" v-model="selectedCategory" @change="filterProducts">
          <option value="null" disabled selected>Chọn danh mục</option>
          <option v-for="(item,index) in categories" :key="index" :value="item">{{item.name}}</option>
        </select>
      </div>
    </div>
    <div class="col-sm">
      <div class="mb-3">
        <label for="priceSelect" class="form-label">Giá bán</label>
        <select class="form-select" id="priceSelect" v-model="selectedPriceRange" @change="filterProducts">
          <option value="null" disabled selected>Chọn khoản giá</option>
          <option value="0">Dưới 100k</option>
          <option value="1">Từ 100k đến 200k</option>
          <option value="2">Trên 200k</option>
        </select>
      </div>
    </div>
    <div class="col-sm">
      <div class="mb-3">
        <label for="manufacturerSelect" class="form-label">Nhà sản xuất</label>
        <select class="form-select" id="manufacturerSelect" v-model="selectedManufacturer" @change="filterProducts">
          <option value="null" disabled selected>Chọn nhà sản xuất</option>
          <option v-for="(item,index) in manufacturers" :key="index" :value="item">{{item.name}}</option>
        </select>
      </div>
    </div>
    <div class="col-sm">
      <label class="form-label">Làm mới</label>
      <button class="btn btn-secondary form-control" @click="resetFilters">Làm mới</button>
    </div>
  </div>
  <div class="row">
    <div class="row justify-content-start" v-if="results && results.length">
      <div class="col-3 me-2 mb-3" v-for="(product,index) in results" :key="index">
        <div class="card h-100 mb-3 position-relative" v-if="product">
          <div class="card-body d-flex flex-column justify-content-between">
            <h5 class="card-title">{{product.name}}</h5>
            <h6 class="card-subtitle text-muted">{{product.createdBy.name}}</h6>
            <router-link :to="`/products/${product.url}`">
              <img v-if="images.find(image=>image.productId === product.id)" class="img-fluid d-flex mx-auto my-4 rounded"
                   :src="'data:image/jpeg;base64,'+images.find(image=>image.productId === product.id)['image'].file" alt="Card image cap">
            </router-link>
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
      </div>
    </div>
    <div class="text-center" v-else>
      {{ message }}
    </div>
  </div>
</template>
<script>
import {computed, onMounted, ref} from "vue";
import ProductItemComponent from "@/components/ProductItemComponent.vue";
import ProductService from "@/services/product.service.js";
import CategoryService from "@/services/category.service.js";
import ManufacturerService from "@/services/manufacturer.service.js";
import UseService from "@/services/use.service.js";
import {formatPercentage, formatPrice} from "@/utils/convert.utils.js";
import ImageService from "@/services/image.service.js";
import CartService from "@/services/cart.service.js";
import {useToast} from "vue-toastification";
import {useRouter} from "vue-router";
import {useStore} from "vuex";
import StoreService from "@/services/store.service.js";

export default {
  name: "ProductList",
  components: { ProductItemComponent },
  mounted() {
    document.title='Sản phẩm'
  },
  setup() {
    const products = ref([]);
    const toast = useToast()
    const router = useRouter()
    const message = ref();
    const store = useStore();
    const images = ref([])
    const categories = ref([]);
    const manufacturers = ref([]);
    const uses = ref([]);
    const cart = computed(()=>store.state.cart)
    const results = ref([])
    const selectedUse = ref(null);
    const selectedCategory = ref(null);
    const selectedPriceRange = ref(null);
    const selectedManufacturer = ref(null);

    onMounted(async () => {
      await Promise.all([
        ProductService.getAll().then(response=>{
          for (let item of response.data){
            ProductService.getOne(item.id).then(resProduct=>{
              ImageService.getOne(resProduct.data.images[0]).then((resImage) => {
                images.value.push({image:resImage.data,productId : item.id});
              });
              results.value.push(resProduct.data)
              products.value.push(resProduct.data)
            })

          }

        }).catch(err=>{
          message.value = err.response.data.detail
        }),
        CategoryService.getAll().then((response) => {
          categories.value = response.data;
        }),
        ManufacturerService.getAll().then((response) => {
          manufacturers.value = response.data;
        }),
        UseService.getAll().then((response) => {
          uses.value = response.data;
        }),
      ]);
    });

    const filterProducts = async () => {
      try {
        let filteredProducts = [...products.value];

        if (selectedUse.value) {
          filteredProducts = filteredProducts.filter(
              (product) => (product.uses.map(item=>item.id)).includes(selectedUse.value.id)
          );
        }

        if (selectedCategory.value) {
          filteredProducts = filteredProducts.filter(
              (product) => (product.categories.map(item=>item.id)).includes(selectedCategory.value.id)
          );
        }
        if (selectedPriceRange.value !== null) {
          switch (selectedPriceRange.value) {
            case "0":
              filteredProducts = filteredProducts.filter(
                  (product) => product.price < 100000
              );
              break;
            case "1":
              filteredProducts = filteredProducts.filter(
                  (product) => product.price >= 100000 && product.price <= 200000
              );
              break;
            case "2":
              filteredProducts = filteredProducts.filter(
                  (product) => product.price > 200000
              );
              break;
            default:
              break;
          }
        }

        if (selectedManufacturer.value) {
          filteredProducts = filteredProducts.filter(
              (product) => product.manufacturer.id === selectedManufacturer.value.id
          );
        }
        if (filteredProducts.length ===0){
          message.value = "Không tim thấy sản phẩm"
        }
        results.value = filteredProducts;
      } catch (error) {
        message.value = error.response.data.detail;
      }
    };
    const resetFilters = () => {
      selectedUse.value = null;
      selectedCategory.value = null;
      selectedPriceRange.value = null;
      selectedManufacturer.value = null;
      filterProducts();
    };
    return {
      results, message, categories, manufacturers, uses, selectedUse,images,resetFilters,toast,router,store,cart,
      selectedCategory, selectedPriceRange, selectedManufacturer, filterProducts
    };
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
      CartService.create(data).then(response=> {
        CartService.getAll().then(async res => {
          await this.store.dispatch('setCart',res.data.length)
          this.toast.success("Thêm vào giỏ hàng thành công.")
        })
      }).catch(err=>{
        this.toast.warning("không thể thêm vào giỏ hàng");
        this.router.push("/login")
      })
    }
  }
};
</script>
