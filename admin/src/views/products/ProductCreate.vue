<template>
  <div class="row">
    <div class="card col-sm-8 ">
      <div class="card-body">
        <h5 class="card-title">Nhập thông tin sản phẩm mới</h5>
        <form class="g-3" @submit.prevent="createItem">
          <div class="mb-3">
            <input type="text" class="form-control" id="name" @input="updateSlug" placeholder="Tên sản phẩm" v-model="data.name">
          </div>
          <div class="mb-3">
            <input type="text" class="form-control" placeholder="Đường dẫn" v-model="data.url">
          </div>
          <div class="row mb-3">
            <div class="col-sm">
              <input type="number" class="form-control" id="quantity" placeholder="Số lượng" v-model="data.quantity">
            </div>
            <div class="col-sm">
              <input type="number" class="form-control" id="price" placeholder="Giá tiền" v-model="data.price">
            </div>
            <div class="col-sm">
              <input type="number" class="form-control" id="sale" placeholder="Giảm giá" v-model="data.sale">
            </div>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none"
               @click="selectedComponent = 'component1'">
              {{ listUse.length === 0 ? 'Thêm công dụng' : 'Thay đổi công dụng'}}
            </a>
            <ol v-if="listUse" class="breadcrumb" :class="{'border p-1 m-2' : listUse.length>0}">
              <li class="breadcrumb-item active" v-for="(use,index) in listUse" :key="index">
                {{use.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component2'">
              {{ listCategory.length === 0 ? 'Thêm danh mục' : 'Thay đổi danh mục'}}
            </a>
            <ol v-if="listCategory" class="breadcrumb" :class="{'border p-1' : listCategory.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in listCategory" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component3'">
              {{ listDosage.length === 0 ? 'Thêm dạng bào chế' : 'Thay đổi dạng bào chế'}}
            </a>
            <ol v-if="listDosage" class="breadcrumb" :class="{'border p-1' : listDosage.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in listDosage" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <a class="text-primary" style="text-decoration: none" @click="selectedComponent = 'component4'">
              {{ listContraindication.length === 0 ? 'Thêm chống chỉ định' : 'Thay đổi chống chỉ định'}}
            </a>
            <ol v-if="listContraindication" class="breadcrumb" :class="{'border p-1' : listContraindication.length>0}">
              <li class="breadcrumb-item active" v-for="(item,index) in listContraindication" :key="index">
                {{item.name}}
              </li>
            </ol>
          </div>
          <div class="mb-3">
            <select class="form-select" aria-label="Default select example" v-model="data.manufacturerId">
              <option selected="">Chọn nhà sản xuất</option>
              <option v-for="(item, index) in listManufacturers" :value="item.id" :key="index" >
                {{item.name}}
              </option>
            </select>
          </div>
          <UploadFileComponent :updateImage="data.images" @update-list-id="uploadListImage"/>
          <div class="text-center mb-3">
            <button type="submit" class="btn btn-sm btn-primary">Thêm mới</button>
          </div>
        </form>
      </div>
    </div>
    <div class="card col-sm-4">
      <UseOptionComponent :useChecked="listUse" @get-use-id="getListUse" v-if="selectedComponent === 'component1'"/>
      <CategoryOptionComponent :categoryChecked="listCategory" @get-category-id="getListCategory" v-if="selectedComponent === 'component2'"/>
      <DosageFormOptionComponent :dosageChecked="listDosage" @get-dosage-id="getListDosage" v-if="selectedComponent === 'component3'"/>
      <ContraindicationOptionComponent :contraindicationChecked="listContraindication" @get-contraindication-id="getListContraindication" v-if="selectedComponent === 'component4'"/>
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

export default {
  name: "ProductCreate",
  components: {
    DosageFormOptionComponent,
    ContraindicationOptionComponent, CategoryOptionComponent, UseOptionComponent, UploadFileComponent},
  mounted() {
    document.title = 'Thêm sản phẩm';
    this.data.storeId=parseInt(this.$route.query.value);
  },
  setup(){
    const listManufacturers = ref([])
    const toast = useToast()
    const data = reactive({
      name:'',url:'',quantity:'',price:'',sale:'',content:'',uses:[],
      manufacturerId:[],contraindications:[],dosageForms:[],categories:[],images:[],storeId: ''
    })
    onMounted( async () => {
      const response = await ManufacturerService.getAll();
      listManufacturers.value=response.data
    });
    return{
      listManufacturers,toast, data
    }
  },
  data() {
    return {
      selectedComponent: null,listUse:[],listCategory:[],listDosage:[],listContraindication:[],listImages:[]
    };
  },
  methods: {
    uploadListImage(newListId) {
      this.listImages=newListId;
    },
    getListUse(listUse){
      this.listUse = [...listUse];
    },
    getListCategory(listCategory){
      this.listCategory=[...listCategory]
    },
    getListDosage(listDosage){
      this.listDosage=[...listDosage]
    },
    getListContraindication(listContraindication){
      this.listContraindication=[...listContraindication]
    },
    updateSlug() {
      this.data.url = this.slugify(this.data.name);
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
    },
    createItem(){
      this.data.categories=this.listCategory.map(item => item.id);
      this.data.uses=this.listUse.map(item => item.id);
      this.data.contraindications=this.listContraindication.map(item => item.id);
      this.data.dosageForms=this.listDosage.map(item => item.id);
      this.data.images=this.listImages.map(item=>item.id);
      this.data.status = true;
      try {
        ProductService.create(this.data).then(response=>{
          this.toast.success("Thêm sản phẩm thành công.")
        }).catch(err=>{
          console.log(err.response.data);
        })
      }catch (e) {
        this.toast.error("Thêm sản phẩm thất bại")
      }
    }
  }
}
</script>

<style scoped>

</style>