<template>
  <div class="row">
    <div class="card col-sm-8 ">
      <div class="card-body">
        <h5 class="card-title">Cập nhật thông tin sản phẩm </h5>
        <form class="g-3" @submit.prevent="uploadItem" v-if="product">
          <div class="mb-3">
            <input type="text" class="form-control" @input="updateSlug" id="name" placeholder="Tên sản phẩm" v-model="product.name">
          </div>
          <div class="mb-3">
            <input type="text" class="form-control" placeholder="Đường dẫn" v-model="product.url">
          </div>
          <div class="row mb-3">
            <div class="col-sm">
              <input type="number" class="form-control" id="quantity" placeholder="Số lượng" v-model="product.quantity">
            </div>
            <div class="col-sm">
              <input type="number" class="form-control" id="price" placeholder="Giá tiền" v-model="product.price">
            </div>
            <div class="col-sm">
              <input type="number" class="form-control" id="sale" placeholder="Giảm giá" v-model="product.sale">
            </div>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none"
               @click="selectedComponent = 'component1'">
              {{ product.uses.length === 0 ? 'Thêm công dụng' : 'Thay đổi công dụng'}}
            </a>
            <ol v-if="product.uses" class="breadcrumb" :class="{'border p-1' : product.uses.length>0}">
              <li class="breadcrumb-item active" v-for="(use,index) in product.uses" :key="index">
                {{use.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component2'">
              {{ product.categories.length === 0 ? 'Thêm danh mục' : 'Thay đổi danh mục'}}
            </a>
            <ol v-if="product.categories" class="breadcrumb" :class="{'border p-1' : product.categories.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in product.categories" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component3'">
              {{ product.dosageForms.length === 0 ? 'Thêm dạng bào chế' : 'Thay đổi dạng bào chế'}}
            </a>
            <ol v-if="product.dosageForms" class="breadcrumb" :class="{'border p-1' : product.dosageForms.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in product.dosageForms" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component4'">
              {{ product.contraindications.length === 0 ? 'Thêm chống chỉ định' : 'Thay đổi chống chỉ định'}}
            </a>
            <ol v-if="product.contraindications" class="breadcrumb" :class="{'border p-1' : product.contraindications.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in product.contraindications" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3" v-if="listManufacturers">
            <select class="form-select" aria-label="Default select example" v-model="product.manufacturer.id">
              <option disabled selected>Chọn nhà sản xuất</option>
              <option v-for="(item, index) in listManufacturers" :value="item.id" :key="index">
                {{item.name}}
              </option>
            </select>
          </div>
          <UploadFileComponent :updateImage="product.images" @update-list-id="uploadListImage"/>
          <div class="text-center mb-3">
            <button type="submit" class="btn btn-sm btn-primary">Thêm mới</button>
          </div>
        </form>
      </div>
    </div>
    <div class="card col-sm-4">
      <UseOptionComponent :useChecked="product.uses" @get-use-id="getListUse" v-if="selectedComponent === 'component1'"/>
      <CategoryOptionComponent :categoryChecked="product.categories" @get-category-id="getListCategory" v-if="selectedComponent === 'component2'"/>
      <DosageFormOptionComponent :dosageChecked="product.dosageForms" @get-dosage-id="getListDosage" v-if="selectedComponent === 'component3'"/>
      <ContraindicationOptionComponent :contraindicationChecked="product.contraindications" @get-contraindication-id="getListContraindication" v-if="selectedComponent === 'component4'"/>
    </div>
  </div>
</template>
<script>
import UploadFileComponent from "@/components/UploadFileComponent.vue";
import UseOptionComponent from "@/components/UseOptionComponent.vue";
import CategoryOptionComponent from "@/components/CategoryOptionComponent.vue";
import ContraindicationOptionComponent from "@/components/ContraindicationOptionComponent.vue";
import DosageFormOptionComponent from "@/components/DosageFormOptionComponent.vue";
import {onMounted, reactive, ref} from "vue";
import ManufacturerService from "@/services/manufacturer.service.js";
import ProductService from "@/services/product.service.js";
import {useToast} from "vue-toastification";
import {useRoute} from "vue-router";

export default {
  name: "ProductUpdate",
  components: {
    DosageFormOptionComponent,
    ContraindicationOptionComponent, CategoryOptionComponent, UseOptionComponent, UploadFileComponent},
  mounted() {
    document.title = 'Cập nhật sản phẩm';
  },
  setup(){
    const listManufacturers = ref([])
    const toast = useToast()
    let id = ref()
    const route = useRoute()
    const product = ref()
    const data = reactive({
      name:'',url:'',quantity:'',price:'',sale:'',content:'',uses:[],status:'',
      manufacturer:'',contraindications:[],dosageForms:[],categories:[],images:[]
    })
    onMounted( async () => {
      id = route.params.id;
      ProductService.getOne(id).then(res=>{
        product.value = res.data
      })
      ManufacturerService.getAll().then(response=>{
        listManufacturers.value=response.data
      });
    });
    return{
      listManufacturers,toast,data,product
    }
  },
  data() {
    return {
      selectedComponent: null,listImage:[]
    };
  },
  methods: {
    uploadListImage(newListId) {
      this.listImage=newListId;
    },
    getListUse(listUse){
      this.data.uses = [...listUse];
    },
    getListCategory(listCategory){
      this.data.categories=[...listCategory]
    },
    getListDosage(listDosage){
      this.data.dosageForms=[...listDosage]
    },
    getListContraindication(listContraindication){
      this.data.contraindications=[...listContraindication]
    },
    uploadItem(){
      this.data = {name:this.product.name,url:this.product.url,quantity:this.product.quantity,price:this.product.price,
      sale:this.product.sale,status:this.product.status,manufacturer:this.product.manufacturer.id}
      this.data.categories=this.product.categories.map(item => item.id);
      this.data.uses=this.product.uses.map(item => item.id);
      this.data.contraindications=this.product.contraindications.map(item => item.id);
      this.data.dosageForms=this.product.dosageForms.map(item => item.id);
      if (this.listImage.length === 0){
        this.data.images = this.product.images
      }else{
        this.data.images=this.listImage.map(item=>item.id);
      }
      console.log(this.data)
      ProductService.update(this.product.id,this.data).then(response=>{
        this.toast.success("Cập nhật thành công.")
      }).catch(er=>{
        this.toast.error("Lỗi khi cập nhật sản phẩm.")
      })
    },
    updateSlug() {
      this.product.url = this.slugify(this.product.name);
    },
    slugify(title) {
      var slug;
      slug = title.toLowerCase();
      slug = slug.replace(/á|à|ả|ạ|ã|ă|ắ|ằ|ẳ|ẵ|ặ|â|ấ|ầ|ẩ|ẫ|ậ/gi, 'a');
      slug = slug.replace(/é|è|ẻ|ẽ|ẹ|ê|ế|ề|ể|ễ|ệ/gi, 'e');
      slug = slug.replace(/i|í|ì|ỉ|ĩ|ị/gi, 'i');
      slug = slug.replace(/ó|ò|ỏ|õ|ọ|ô|ố|ồ|ổ|ỗ|ộ|ơ|ớ|ờ|ở|ỡ|ợ/gi, 'o');
      slug = slug.replace(/ú|ù|ủ|ũ|ụ|ư|ứ|ừ|ử|ữ|ự/gi, 'u');
      slug = slug.replace(/ý|ỳ|ỷ|ỹ|ỵ/gi, 'y');
      slug = slug.replace(/đ/gi, 'd');
      slug = slug.replace(/\`|\~|\!|\@|\#|\||\$|\%|\^|\&|\*|\(|\)|\+|\=|\,|\.|\/|\?|\>|\<|\'|\"|\:|\;|_/gi, '');
      slug = slug.replace(/ /gi, "-");
      slug = slug.replace(/\-\-\-\-\-/gi, '-');
      slug = slug.replace(/\-\-\-\-/gi, '-');
      slug = slug.replace(/\-\-\-/gi, '-');
      slug = slug.replace(/\-\-/gi, '-');
      slug = '@' + slug + '@';
      slug = slug.replace(/\@\-|\-\@|\@/gi, '');
      return slug;
    }
  }
}
</script>

<style scoped>

</style>