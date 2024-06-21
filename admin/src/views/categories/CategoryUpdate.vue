<template>
  <main id="main" class="main">
    <div class="pagetitle">
      <h1>Cập nhật danh mục</h1>
      <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/" style="text-decoration: none">Trang chủ</router-link></li>
          <li class="breadcrumb-item">Danh mục</li>
          <li class="breadcrumb-item active">Cập nhật danh mục</li>
        </ol>
      </nav>
    </div>
    <section class="section">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Nhập thông tin danh mục</h5>
          <p>Cập nhật <code>danh mục</code> thì không quá <code>3 level</code> trong cùng cây danh mục.</p>
          <form class="g-3" @submit.prevent="updateItem" v-if="category">
            <div class="row mb-3">
              <div class="col-sm-6">
                <input type="text" v-model="category.name" @input="updateSlug" class="form-control" placeholder="Tên danh mục">
              </div>
              <div class="col-sm-6">
                <input type="text" v-model="category.url" class="form-control" placeholder="Đường dẫn">
              </div>
            </div>
            <div class="row mb-3">
              <div class="col-sm-6">
                <div class="form-check">
                  <input class="form-check-input" type="checkbox" id="gridCheck" v-model="category.isEnable" :checked="category">
                  <label class="form-check-label" for="gridCheck">
                    Hoạt động
                  </label>
                </div>
              </div>
              <div class="col-sm-6">
                <select v-if="category" id="categoryParent" name="categoryParent" class="form-select mb-3" v-model="category.categoryParent">
                  <option value="" disabled selected>Chọn danh mục cha</option>
                  <option value="0">Làm danh mục gốc</option>
                  <option v-for="(item, index) in listCategory" :value="item.id" :key="index" :disabled="item.level === 3 || category.id === item.id">
                    {{ '-'.repeat(item.level) }} {{ item.name }}
                  </option>
                </select>
              </div>
            </div>
            <div class="text-center">
              <button type="submit" class="btn btn-sm btn-primary" style="margin-right: 5px">Cập nhật</button>
            </div>
          </form>

        </div>
      </div>
    </section>
  </main>
</template>
<script>
import {useRoute} from "vue-router";
import {onMounted, ref} from "vue";
import CategoryService from "@/services/category.service.js";
import {useToast} from "vue-toastification";

export default {
  name: "CategoryUpdate",
  async mounted(){
    document.title = 'Cập nhật sản phẩm';
    this.getCategories();
  },
  setup(){
    const toast = useToast();
    const category = ref();
    const route = useRoute();
    onMounted(async () => {
      const url = route.params.url;
      await CategoryService.getOneByURL(url).then(response => {
        category.value = response.data
      }).catch(error =>{
        toast.error("Lỗi hệ thống.")
      })
    })
    return{
      category,
      toast
    }
  },
  data() {
    return {
      categories: [], listCategory: []
    }
  },
  methods:{
    getDescendantIds(id) {
      let ids = [];
      for (let category of this.categories) {
        if (category.categoryParent === id) {
          ids.push(category.id);
          ids = ids.concat(this.getDescendantIds(category.id));
        }
      }
      return ids;
    },
    getCategoriesWithoutDescendants(id) {
      const descendantIds = this.getDescendantIds(id);
      // descendantIds.push(id);
      return this.categories.filter(category => !descendantIds.includes(category.id));
    },
    getCategories() {
      CategoryService.getAll().then(response => {
        this.categories = response.data;
        this.recursive(this.getCategoriesWithoutDescendants(this.category.id), 0, 1, this.listCategory);
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
    updateSlug() {
      this.category.url = this.slugify(this.category.name);
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
    async updateItem(){
      CategoryService.update(this.category.id,this.category)
          .then(async response => {
            this.toast.success("Cập nhật danh mục thành công.")
            const updatedCategories = await CategoryService.getAll();
            this.categories = updatedCategories.data;
            this.listCategory = [];
            this.recursive(this.getCategoriesWithoutDescendants(this.category.id), 0, 1, this.listCategory);
          })
          .catch(error =>{
            this.toast.error(error.response.data.detail)
          })
    }
  }
}
</script>
<style scoped>

</style>