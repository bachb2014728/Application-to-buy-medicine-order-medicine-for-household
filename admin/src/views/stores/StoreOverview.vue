<template>
  <div v-if="store" class="card" >
    <img :src="imageBackground" class="card-img-top" alt="user-background">
    <div class="card-body">
      <div class="d-flex align-items-start align-items-sm-center gap-4">
        <img :src="imageSrc" alt="user-avatar" class="d-block rounded avatar" id="uploadedAvatar" />
        <label for="upload" class="btn btn-sm btn-primary me-2 mb-4" tabindex="0">
          <span class="d-none d-sm-block">Thay đổi ảnh đại diện</span>
          <i class="bx bx-upload d-block d-sm-none"></i>
          <input type="file" id="upload" class="account-file-input" @change="handleFileUpload" hidden />
        </label>
        <label for="uploadBg" class="btn btn-sm btn-primary me-2 mb-4" tabindex="1">
          <span class="d-none d-sm-block">Thay đổi ảnh nền</span>
          <i class="bx bx-upload d-block d-sm-none"></i>
          <input type="file" id="uploadBg" class="account-file-input" @change="handleFileUploadBackground" hidden />
        </label>
        <router-link :to="{ path: '/products/create', query: { value: store.id } }"
                     class="btn btn-sm btn-primary me-2 mb-4" style="text-decoration: none" v-if="store.isBoss">
          <span>Thêm sản phẩm</span>
        </router-link>
        <router-link :to="{ path: '/vouchers/create', query: { value: store.id } }"
                     class="btn btn-sm btn-primary me-2 mb-4" style="text-decoration: none" v-if="store.isBoss">
          <span>Thêm mã khuyến mãi</span>
        </router-link>
      </div>
      <h5 class="fw-medium">{{store.name}}</h5>
      <div class="row">
        <div class="col-sm">
          <ul>
            <li><i class="bi bi-box-seam"></i> Sản phẩm : {{store.products.length}} </li>
            <li><i class="bi bi-cart"></i> Số đơn hàng : </li>
            <li><i class="bi bi-bar-chart"></i> Tỉ lệ duyệt đơn hàng :</li>
            <li><i class="bi bi-person-add"></i> Đang theo dõi : </li>
          </ul>
        </div>
        <div class="col-sm">
          <ul>
            <li><i class="bi bi-person"></i> Người theo dõi : {{store.followers}}</li>
            <li><i class="bi bi-chat-dots"></i> Đánh giá : </li>
            <li><i class="bi bi-calendar-date"></i> Ngày thành lập: {{store.createdAt}}</li>
            <li>
              <i class="bi bi-emoji-kiss"></i>
              Trạng thái: <span v-if="store.status === 'ACTIVE'" class="badge bg-label-success me-1">Hoạt động</span>
              <span v-if="store.status === 'BLOCK'" class="badge bg-label-danger me-1">Tạm khóa</span>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
  <div class="card" v-if="store">
    <div class="card-body">
      <h5 class="card-title">Giới thệu nhà thuốc :</h5>
      <p>{{store.info}}</p>
    </div>
  </div>
  <div v-if="store">
    <ListVoucherComponent :listVoucher="store.vouchers"/>
  </div>
  <div v-if="store">
    <ListProductComponent :listProduct="store.products"/>
  </div>
</template>
<script>
import {useToast} from "vue-toastification";
import {onMounted, ref} from "vue";
import {useRoute} from "vue-router";
import StoreService from "@/services/store.service.js";
import ImageService from "@/services/image.service.js";
import ListVoucherComponent from "@/components/ListVoucherComponent.vue";
import ListProductComponent from "@/components/ListProductComponent.vue";

export default {
  name: "StoreOverview",
  components: {ListProductComponent, ListVoucherComponent},
  mounted() {
    document.title='Xem chi tiết nhà thuốc'
  },
  setup(){
    const toast = useToast();
    const store = ref();
    const route = useRoute();
    let url = ref();
    let imageSrc = ref();
    let imageBackground = ref()
    onMounted(async () => {
      url = route.params.url;
      await StoreService.getOneByURL(url).then(response => {
        store.value = response.data;
        if (store.value.avatar == null){
          imageSrc.value = "require('@/assets/image/user.png')"
        }else{
          ImageService.getOne(store.value.avatar).then(res=>{
            imageSrc.value= "data:image/jpeg;base64," + res.data.file
          })
        }
        if (store.value.background == null){
          imageBackground.value="require('@/assets/image/background.jpg')"
        }else{
          ImageService.getOne(store.value.background).then(res=>{
            imageBackground.value="data:image/jpeg;base64," + res.data.file
          })
        }
      }).catch(error =>{
        toast.error("Lỗi hệ thống.")
      })
    })
    const handleFileUpload = async event => {
      const file = event.target.files[0];
      if (file) {
        imageSrc.value = URL.createObjectURL(file);
        const formData = new FormData();
        formData.append('imageData', file);
        await ImageService.uploadImage(formData).then(async response =>{
          toast.success(response.data.message)
          let id = response.data.id;
          const changeAvatar = new FormData();
          changeAvatar.append('image', id)
          const avatar = await StoreService.changeAvatar(changeAvatar,url);
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
        await ImageService.uploadImage(formData).then(async response => {
          toast.success(response.data.message)
          let id = response.data.id;
          const changeImageBackground = new FormData();
          changeImageBackground.append('image', id)
          await StoreService.changeBackground(changeImageBackground, url);
        }).catch(error => {
          toast.error("Lỗi khi tải ảnh lên.");
        });
      }
    };
    return {
      imageSrc,
      imageBackground,
      handleFileUpload,
      handleFileUploadBackground,
      store,
      toast
    }
  },
}
</script>

<style scoped>
.avatar {
  position: absolute;
  left: 10px;
  top: 50%;
  transform: translateY(-50%);
  height: 5rem;
  width: 5rem;
  border: 5px solid white;
  border-radius: 50%;
}
.card {
  position: relative;
  min-height:20rem;
}
.card-img-top {
  background-position: center;
  background-repeat: no-repeat;
  height: 20rem;
  width: 100%;
}

</style>

