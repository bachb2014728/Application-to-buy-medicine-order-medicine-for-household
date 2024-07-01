<template>
  <div class="row" v-if="store">
    <div class="row d-flex align-items-stretch mb-3">
      <div class="col-sm-5 h-100">
        <div class="card">
          <div class="text-center">
            <img class="card-img-top" v-if="imageBackground" :src="imageBackground" alt="Card image cap">
            <img class="card-img-top" v-else src="@/assets/images/background_2.png" alt="Card image cap">
          </div>
          <div class="card-body">
            <h5 class="card-title">{{store.name}}</h5>
            <p class="card-text">Địa chỉ cửa hàng : {{store.address}}</p>
            <p class="card-text">
              <small class="text-muted">{{ConvertDate(store.createdAt)}}</small>
            </p>
          </div>
        </div>
      </div>
      <div class="col-sm-7 h-100">
        <div class="card">
          <h5 class="card-header">Thông tin</h5>
          <div class="card-body">
            <div class="d-flex align-items-start align-items-sm-center gap-4">
              <img v-if="imageSrc" :src="imageSrc" alt="user-avatar" class="d-block rounded" height="100" width="100" id="uploadedAvatar">
              <img v-else src="@/assets/images/user.png" alt="user-avatar" class="d-block rounded" height="100" width="100" id="uploadedAvatar">
              <div class="button-wrapper">
                <label for="upload" class="btn btn-sm btn-primary me-2 mb-5" tabindex="0">
                  <span class="d-none d-sm-block">Thay đổi ảnh đại diện</span>
                  <i class="bx bx-upload d-block d-sm-none"></i>
                  <input type="file" id="upload" class="account-file-input" @change="handleFileUpload" hidden />
                </label>
                <label for="uploadBg" class="btn btn-sm btn-primary me-2 mb-5" tabindex="1">
                  <span class="d-none d-sm-block">Thay đổi ảnh nền</span>
                  <i class="bx bx-upload d-block d-sm-none"></i>
                  <input type="file" id="uploadBg" class="account-file-input" @change="handleFileUploadBackground" hidden />
                </label>
              </div>
            </div>
          </div>
          <div class="row">
            <div class="col-sm">
              <ul>
                <li class="nav-link mb-2"><i class="bi bi-box-seam"></i> Sản phẩm : {{store.products.length}} </li>
                <li class="nav-link mb-2" v-if="orders"><i class="bi bi-cart"></i> Số đơn hàng : {{orders.length}}</li>
                <li class="nav-link mb-2"><i class="bi bi-bar-chart"></i> Tỉ lệ duyệt đơn hàng : 0</li>
                <li class="nav-link mb-2"><i class="bi bi-person-add"></i> Số mã khuyến mãi : {{store.vouchers.length}} </li>
              </ul>
            </div>
            <div class="col-sm">
              <ul>
                <li class="nav-link mb-2"><i class="bi bi-person"></i> Người theo dõi : 0{{store.followers}}</li>
                <li class="nav-link mb-2"><i class="bi bi-chat-dots"></i> Đánh giá : </li>
                <li class="nav-link mb-2"><i class="bi bi-calendar-date"></i> Ngày thành lập: {{ConvertDate(store.createdAt)}}</li>
                <li class="nav-link mb-2">
                  <i class="bi bi-emoji-kiss"></i>
                  Trạng thái: <span v-if="store.status === 'ACTIVE'" class="badge bg-label-success me-1">Hoạt động</span>
                  <span v-if="store.status === 'BLOCK'" class="badge bg-label-danger me-1">Tạm khóa</span>
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
import ImageService from "@/services/image.service.js";
import StoreService from "@/services/store.service.js";
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import {useToast} from "vue-toastification";
import FilledTabComponent from "@/components/FilledTabComponent.vue";
import {formatDate} from "@/utils/time.utils.js";
import ListVoucherComponent from "@/components/ListVoucherComponent.vue";
import ListProductComponent from "@/components/ListProductComponent.vue";
import OrderService from "@/services/order.service.js";

export default {
  name: "StoreOverviewManager",
  components: {ListProductComponent, ListVoucherComponent},
  mounted() {
    document.title='Quản lý cửa hàng'
  },
  setup(){
    const toast = useToast();
    const store = ref();
    const orders = ref([]);
    const route = useRoute();
    const url = ref();
    let imageSrc = ref();
    let imageBackground = ref()
    onMounted(async () => {
      url.value = route.params.url;
      StoreService.getOneByURL(url.value).then(response => {
        store.value = response.data;
        if (store.value.avatar == null){
          imageSrc.value = null
        }else{
          ImageService.getOne(store.value.avatar).then(res=>{
            imageSrc.value= "data:image/jpeg;base64," + res.data.file
          })
        }
        if (store.value.background == null){
          imageBackground.value= null
        }else{
          ImageService.getOne(store.value.background).then(res=>{
            imageBackground.value="data:image/jpeg;base64," + res.data.file
          })
        }
        OrderService.getAllOrderOfStore(response.data.id).then(res=>{
          orders.value = res.data.filter(item=>item.orderStatus !== 1 || item.orderStatus !== 3)
        })
      }).catch(error =>{
        toast.error("Tài khoản bị khóa hoặc không tìm thấy.")
      })
    })
    const handleFileUpload = async event => {
      const file = event.target.files[0];
      if (file) {
        imageSrc.value = URL.createObjectURL(file);
        const formData = new FormData();
        formData.append('imageData', file);
        await ImageService.uploadImage(formData).then(async response =>{
          toast.success("Thay đổi ảnh đại diện thành công.")
          let id = response.data.id;
          const changeAvatar = new FormData();
          changeAvatar.append('image', id)
          const avatar = await StoreService.changeAvatar(changeAvatar,url.value);
        }).catch(error => {
          toast.error("Lỗi khi tải ảnh lên.");
        });
      }
    };
    const handleFileUploadBackground = async event => {
      const file = event.target.files[0];
      if (file) {
        imageBackground.value = URL.createObjectURL(file);
        const formData = new FormData();
        formData.append('imageData', file);
        ImageService.uploadImage(formData).then(response => {
          toast.success("Thay đổi ảnh bìa thành công.")
          let id = response.data.id;
          const changeImageBackground = new FormData();
          changeImageBackground.append('image', id)
          StoreService.changeBackground(changeImageBackground, url.value);
        }).catch(error => {
          toast.error("Lỗi khi tải ảnh lên.");
        });
      }
    };
    return {imageSrc, imageBackground, handleFileUpload, handleFileUploadBackground, store, toast,url,orders}
  },
  methods:{
    ConvertDate(data){
      return formatDate(data)
    }
  }
}
</script>
<style scoped>
.card {
  position: relative;
  min-height:20rem;
}
.card-img-top {
  height: 250px;
  width: 550px;
}
</style>