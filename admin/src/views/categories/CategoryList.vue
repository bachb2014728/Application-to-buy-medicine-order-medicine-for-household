<template>
  <main id="main" class="main">
    <div class="pagetitle">
      <h1>Danh sách danh mục</h1>
      <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/" style="text-decoration: none">Trang chủ</router-link></li>
          <li class="breadcrumb-item">Danh mục</li>
          <li class="breadcrumb-item active">Danh sách</li>
        </ol>
      </nav>
    </div>
    <section class="section">
      <div class="card">
        <div class="card-body">
          <table class="table table-bordered">
            <thead>
            <tr>
              <th scope="col">#</th>
              <th scope="col">Tên danh mục</th>
              <th class="col">Đường dẫn</th>
              <th scope="col">Danh mục cha</th>
              <th scope="col">Trạng thái</th>
              <th scope="col"></th>
            </tr>
            </thead>
            <tbody>
            <tr v-if="categories" v-for="(item, index) in categories" :key="index">
              <th scope="row">{{index+1}}</th>
              <td>{{item.name}}</td>
              <td>{{item.url}}</td>
              <td>{{ getParentName(item.categoryParent) }}</td>
              <td>
                <span v-if="item.isEnable" class="badge bg-success">Hoạt động</span>
                <span v-if="!item.isEnable" class="badge bg-danger">Tạm ẩn</span>
              </td>
              <td class="text-center">
                <router-link :to="`/categories/${item.url}`" class="btn btn-warning btn-sm me-3">
                  <i class="bi bi-pencil-square"></i>
                </router-link>
                <a @click="confirmDelete(item)" class="btn btn-danger btn-sm me-3">
                  <i class="bi bi-trash"></i>
                </a>
              </td>
            </tr>
            </tbody>
          </table>
        </div>
      </div>
    </section>
  </main>
</template>
<script>
import {onMounted, ref} from "vue";
import CategoryService from "@/services/category.service.js";
import {useToast} from "vue-toastification";
export default {
  name: "CategoryList",
  mounted() {
    document.title = 'Danh sách danh mục'
  },
  setup() {
    const toast = useToast();
    const categories = ref([]);
    const selectedItem = ref(null);
    const selectedIndex = ref(null);
    onMounted( async () => {
      const response = await CategoryService.getAll();
      categories.value = response.data
    });
    const selectItem = (item, index) => {
      selectedItem.value = item;
      selectedIndex.value = index;
    };
    return {
      toast,
      categories,
      selectedItem,
      selectedIndex,
      selectItem
    }
  },
  methods: {
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn xóa danh mục này không ?')) {
        this.deleteItem(item);
      }
    },
    getParentName(id) {
      const category = this.categories.find(cate => cate.id === id);
      return category ? category.name : '';
    },
    getChildName(id){
      const category = this.categories.find(cate => cate.categoryParent === id);
      return category ? category : null;
    },
    async deleteItem(item) {
      let index = this.categories.indexOf(item);
      if(this.getChildName(item.id) !== null){
        this.toast.warning("Không thể xóa khi tồn tại danh mục con.")
        return;
      }
      if (index > -1) {
        this.categories.splice(index, 1);
        this.selectedItem = null;
        try {
          const response = await CategoryService.deleteOne(item.id);
          if (response.status === 200) {
            this.toast.success(response.data.message)
          } else {
            this.toast.error("Xảy ra lỗi khi xóa danh mục.")
          }
        }catch (e) {
          this.toast.error(e.response.data.detail)
        }
      }else{
        this.toast.error("Xảy ra lỗi khi xóa danh mục.")
      }


    }
  }
}
</script>

<style scoped>

</style>