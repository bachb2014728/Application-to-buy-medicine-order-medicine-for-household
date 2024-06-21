<template>
  <main id="main" class="main">
    <div class="pagetitle">
      <h1>Thêm danh mục</h1>
      <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/" style="text-decoration: none">Trang chủ</router-link></li>
          <li class="breadcrumb-item">Danh mục</li>
          <li class="breadcrumb-item active">Thêm danh mục</li>
        </ol>
      </nav>
    </div>
    <section class="section">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Nhập thông tin danh mục mới</h5>
          <p>Thêm <code>danh mục</code> thì không quá <code>3 level</code> trong cùng cây danh mục.</p>
          <form class="g-3" @submit.prevent="createItem" >
            <div class="row mb-3">
              <div class="col-sm-6">
                <input type="text" v-model="data.name" @input="updateSlug" class="form-control" placeholder="Tên danh mục">
              </div>
              <div class="col-sm-6">
                <input type="text" v-model="data.url" class="form-control" placeholder="Đường dẫn">
              </div>
            </div>
            <div class="row mb-3">
              <div class="col-sm-6">
                <div class="form-check">
                  <input class="form-check-input" type="checkbox" id="gridCheck" v-model="data.isEnable" checked>
                  <label class="form-check-label" for="gridCheck">
                    Hoạt động
                  </label>
                </div>
              </div>
              <div class="col-sm-6">
                <select id="categoryParent" name="categoryParent" class="form-select mb-3" v-model="data.categoryParent">
                  <option value="" disabled selected>Chọn danh mục cha</option>
                  <option value="0">Làm danh mục gốc</option>
                  <option v-for="(category, index) in listCategory" :value="category.id" :key="index" :disabled="category.level === 3">
                    {{ '-'.repeat(category.level) }} {{ category.name }}
                  </option>
                </select>
              </div>
            </div>
            <div class="text-center">
              <button type="submit" class="btn btn-sm btn-primary" style="margin-right: 5px">Thêm mới</button>
              <button type="button" class="btn btn-sm btn-secondary" @click="resetForm">Reset</button>
            </div>
          </form>

        </div>
      </div>
    </section>
  </main>
</template>
<script>
import {onMounted, reactive, ref} from "vue";
import CategoryService from "@/services/category.service.js";
import {useToast} from "vue-toastification";

export default {
  name: "CategoryCreate",
  mounted() {
    document.title='Thêm danh mục';
    this.getCategories();
  },
  setup(){
    const toast = useToast();
    const data = reactive({
      name: '', url: '', categoryParent:'', isEnable:true
    })
    onMounted(async () =>{});
    return{
      data,
      toast
    }
  },
  data() {
    return {
      categories: [], listCategory: []
    }
  },
  methods: {
    getCategories() {
      CategoryService.getAll().then(response => {
        this.categories = response.data;
        this.recursive(this.categories, 0, 1, this.listCategory);
      });
    },
    recursive(categories, parents = 0, level = 1, listCategory) {
      if (categories.length > 0) {
        for (let i = 0; i < categories.length; i++) {
          if (categories[i].categoryParent === parents) {
            categories[i].level = level;
            listCategory.push(categories[i]);
            let parent = categories[i].id;
            this.recursive(categories, parent, level + 1, listCategory);
          }
        }
      }
    },
    resetForm() {
      this.data.name = '';
      this.data.url = '';
      this.data.categoryParent = '';
      this.data.isEnable = true;
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
    async createItem(){
      CategoryService.create(this.data)
          .then(async response => {
            this.toast.success("Thêm mới danh mục thành công.")
            const updatedCategories = await CategoryService.getAll();
            this.categories = updatedCategories.data;
            this.listCategory = [];
            this.recursive(this.categories, 0, 1, this.listCategory);
          })
          .catch(error =>{
            this.toast.error(error.response.data.detail)
          })
    }
  },
}
</script>
<style scoped>

</style>