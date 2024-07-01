<template>
  <div class="card" v-if="products">
    <h5 class="card-header d-flex justify-content-between" v-if="store">
      <span>Danh sách sản phẩm</span>
      <router-link :to="{ path: `/stores/${store.url}/products/create`}"
                   class="btn btn-sm btn-success" style="text-decoration: none">
        <span>Thêm sản phẩm</span>
      </router-link>
    </h5>
    <div class="card-body">
      <div class="table-responsive text-nowrap">
        <table class="table table-bordered">
          <thead>
          <tr>
            <th>tên sản phẩm</th>
            <th>hình ảnh</th>
            <th>giá bán</th>
            <th>khuyến mãi</th>
            <th>số lượng</th>
            <th></th>
          </tr>
          </thead>
          <tbody>
          <tr v-if="products && products.length" v-for="(product,index) in products" :key="index">
            <td>
              <span class="fw-medium">{{product.name}}</span>
            </td>
            <td v-if="images && images.length">
              <img v-if="images.some(image=>image.productId === product.id)"
                   :src="images.find(image=>image.productId === product.id).image" class="d-block rounded" height="100" alt="Ảnh sản phẩm">
              <img v-else src="@/assets/images/user.png" class="d-block rounded" height="100" alt="Ảnh sản phẩm">
            </td>
            <td>
              <div class="d-flex justify-content-center">
                <span class="me-1">{{FormatPrice(product.price)}}</span>
                <button type="button" class="btn btn-success btn-sm" data-bs-toggle="modal"
                        @click="editPrice = product" :data-bs-target="'#basicModal' + index">
                  <i class="bx bx-edit-alt me-1"></i>
                </button>
                <div class="modal fade" :id="'basicModal' + index" tabindex="-1" style="display: none;" aria-hidden="true">
                  <div class="modal-dialog" role="document">
                    <div class="modal-content">
                      <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel1">Cập nhật giá bán và khuyến mãi</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                      </div>
                      <div class="modal-body">
                        <div class="row" v-if="editPrice">
                          <div class="col mb-3">
                            <label for="price" class="form-label">Giá bán</label>
                            <input type="number" id="price" class="form-control" placeholder="Giá bán" v-model="editPrice.price">
                          </div>
                          <div class="col mb-3">
                            <label for="sale" class="form-label">Giá khuyến mãi</label>
                            <input type="number" id="sale" class="form-control" placeholder="Giá khuyến mãi" v-model="editPrice.sale">
                          </div>
                        </div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
                          Close
                        </button>
                        <button @click="updatePrice(editPrice)" type="button" data-bs-dismiss="modal"
                                class="btn btn-success">Cập nhật</button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </td>
            <td>{{FormatPrice(product.sale)}}</td>
            <td>
              <div class="d-flex justify-content-center">
                <span class="me-1">{{product.quantity}}</span>
                <button type="button" class="btn btn-success btn-sm" data-bs-toggle="modal"
                        @click="editQuantity = product" :data-bs-target="'#basicModal' + index +'quantity'">
                  <i class="bx bx-edit-alt me-1"></i>
                </button>
                <div class="modal fade" :id="'basicModal' + index+'quantity'" tabindex="-1" style="display: none;" aria-hidden="true">
                  <div class="modal-dialog" role="document">
                    <div class="modal-content">
                      <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLabel1">Cập nhật số lượng</h5>
                        <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                      </div>
                      <div class="modal-body">
                        <div class="row" v-if="editQuantity">
                          <div class="col mb-3">
                            <label for="price" class="form-label">Số lượng tồn kho</label>
                            <input type="number" id="price" class="form-control" placeholder="Số lượng tồn kho"
                                   v-model="editQuantity.quantity">
                          </div>
                        </div>
                      </div>
                      <div class="modal-footer">
                        <button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">
                          Close
                        </button>
                        <button @click="updateQuantity(editQuantity)" type="button" data-bs-dismiss="modal"
                                class="btn btn-success">Cập nhật</button>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </td>
            <td v-if="store">
              <div class="dropdown">
                <button type="button" class="btn p-0 dropdown-toggle hide-arrow" data-bs-toggle="dropdown">
                  <i class="bx bx-dots-vertical-rounded"></i>
                </button>
                <div class="dropdown-menu" >
                  <router-link :to="`/stores/${store.url}/products/${product.url}`" class="dropdown-item">
                    <span class="d-flex flex-row align-items-center"><i class="bx bx-show me-1"></i> Xem chi tiết</span>
                  </router-link>
                  <router-link :to="`/stores/${store.url}/products/${product.url}/update`" class="dropdown-item">
                    <span class="d-flex flex-row align-items-center"><i class="bx bx-edit-alt me-1"></i> Chỉnh sửa</span>
                  </router-link>
                  <a v-if="product.status" class="dropdown-item" @click="removeItem(product)">
                    <i class="bx bx-trash me-1"></i> Xóa
                  </a>
                  <a v-if="!product.status" class="dropdown-item" @click="changeStatus(product)">
                    Hiển thị
                  </a>
                </div>
              </div>
            </td>
          </tr>
          <tr v-else><td class="text-center" colspan="6">Không có sản phẩm nào.</td></tr>
          </tbody>
        </table>
      </div>
    </div>
  </div>
</template>
<script>
import {useRoute} from "vue-router";
import {onMounted, ref} from "vue";
import {formatPrice} from "@/utils/convert.utils.js";
import ProductService from "@/services/product.service.js";
import StoreService from "@/services/store.service.js";
import ImageService from "@/services/image.service.js";
import {useToast} from "vue-toastification";
import toast from "bootstrap/js/src/toast.js";

export default {
  name: "StoreProductManager",
  mounted() {
    document.title = 'Quản lý sản phẩm'
  },
  setup(){
    const router = useRoute()
    const toast = useToast()
    let url = ref()
    const editPrice=ref()
    const editQuantity=ref()
    const images = ref([])
    const store = ref()
    const products = ref([])
    onMounted(async () =>{
      url = router.params.url;
      ProductService.getAllByStore(url).then(response =>{
        products.value = response.data
        for (let item of response.data){
          ImageService.getOne(item.imageId).then(res=>{
            images.value.push({
              image:"data:image/jpeg;base64," + res.data.file,
              productId:item.id
            })
          })
        }
      }).catch(err=>{console.log(err)})
      StoreService.getOneByURL(url).then(response =>{
        store.value = response.data
      })
    })
    return{products,store,images,editPrice,editQuantity,toast}
  },
  methods:{
    FormatPrice(data){
      return formatPrice(data)
    },
    updatePrice(data){
      if (data.price <0){this.toast.warning("Giá bán không hợp lệ."); return;}
      if (data.price < data.sale){this.toast.warning("Giá khuyến mãi lớn hơn giá gốc.");return;}
      let index = this.products.findIndex(item=>item.id === data.id);
      if (index !== -1){
        const dataForm = {productId:data.id, sale: data.sale, price:data.price}
        ProductService.updatePriceAndSale(dataForm,data.id).then(response=>{
          this.toast.success("Cập nhật giá bán thành công.")
        }).catch(err=>{
          this.toast.error("Cập nhật giá bán thất bại.");
        })
      }else{
        ProductService.getAllByStore(this.store.url).then(response =>{
          this.products = response.data
          for (let item of response.data){
            ImageService.getOne(item.imageId).then(res=>{
              this.images.push({
                image:"data:image/jpeg;base64," + res.data.file,
                productId:item.id
              })
            })
          }
        })
      }
    },
    updateQuantity(data){
      let index = this.products.findIndex(item=>item.id === data.id);
      if (index !== -1){
        const dataForm = {productId:data.id, quantity: data.quantity}
        ProductService.updateQuantity(dataForm,data.id).then(response=>{
          this.toast.success("Cập nhật số lượng thành công.")
        }).catch(err=>{
          this.toast.error("Cập nhật số lượng thất bại.");
        })
      }else{
        ProductService.getAllByStore(this.store.url).then(response =>{
          this.products = response.data
          for (let item of response.data){
            ImageService.getOne(item.imageId).then(res=>{
              this.images.push({
                image:"data:image/jpeg;base64," + res.data.file,
                productId:item.id
              })
            })
          }
        })
      }
    },
    removeItem(item) {
      if (window.confirm('Bạn có chắc chắn khóa tài khoản này không ?')) {
        this.deleteItem(item);
      }
    },
    deleteItem(item){
      let index = this.products.findIndex(x=>x.id === item.id);
      if (index>-1){
        ProductService.deleteOne(item.id).then(response=>{
          this.products[index] =  {
            ...this.products[index],
            status:false
          }
          this.toast.success("Ẩn dữ liệu thành công.")
        }).catch(err=>{
          this.toast.warning("Ẩn dữ liệu thất bại.")
        })
      }
    },
    changeStatus(item){
      let index = this.products.findIndex(x=>x.id===item.id);
      if (index>-1){
        const formData ={productId : item.id, status : true}
        ProductService.changeStatus(item.id,formData).then(response=>{
          this.products[index] = {
            ...this.products[index],
            status:true
          }
          this.toast.success("Thay đổi trạng thái thành công.");
        }).catch(err=>{
          this.toast.warning("Thay đổi thất bại");
        })
      }
    }
  }
}
</script>
<style scoped>

</style>