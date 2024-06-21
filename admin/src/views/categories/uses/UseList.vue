<template>
  <div class="row">
    <div class="card col-sm-8">
      <h5 class="card-title">Danh sách</h5>
      <div class="card-body">
        <table class="table table-bordered">
          <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">Công dụng</th>
            <th class="col">Thông tin chi tiết </th>
            <th scope="col"></th>
          </tr>
          </thead>
          <tbody>
          <tr v-if="uses" v-for="(item, index) in uses" :key="index" @click="selectItem(item,index)">
            <th scope="row">{{index+1}}</th>
            <td>{{item.name}}</td>
            <td>{{item.info}}</td>
            <td class="text-center">
              <a @click="confirmDelete(item)" class="btn btn-danger btn-sm me-3">
                <i class="bi bi-trash"></i>
              </a>
            </td>
          </tr>
          </tbody>
        </table>
      </div>
    </div>
    <div class="col-sm-4">
      <div class="card">
        <div class="card-body">
          <h5 class="card-title">Thêm công dụng</h5>
          <form class="g-3" @submit.prevent="createItem" >
            <div class="mb-3">
              <input type="text" v-model="use.name" class="form-control" placeholder="Tên công dụng">
            </div>
            <div class="mb-3">
              <input type="text" v-model="use.info" class="form-control" placeholder="Thông tin (nếu có)">
            </div>
            <div class="text-center">
              <button type="submit" class="btn btn-sm btn-primary" style="margin-right: 5px">Thêm mới</button>
              <button type="button" class="btn btn-sm btn-secondary" @click="resetForm">Reset</button>
            </div>
          </form>
        </div>
      </div>
      <div class="card" >
        <div class="card-body">
          <h5 class="card-title">Cập nhật công dụng</h5>
          <form class="g-3" @submit.prevent="updateItem" >
            <div class="mb-3">
              <input type="text" v-model="data.name" class="form-control" placeholder="Tên công dụng">
            </div>
            <div class="mb-3">
              <input type="text" v-model="data.info" class="form-control" placeholder="Thông tin (nếu có)">
            </div>
            <div class="text-center">
              <button type="submit" class="btn btn-sm btn-primary" style="margin-right: 5px">Cập nhật</button>
            </div>
          </form>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {useToast} from "vue-toastification";
import {onMounted, reactive, ref} from "vue";
import UseService from "@/services/use.service.js";

export default {
  name: "UseList",
  mounted() {
    document.title = 'Danh sách công dụng'
  },
  setup() {
    const toast = useToast();
    const use = reactive({name:'',info:''})
    const data = reactive({id:'',name:'',info:''})
    const uses = ref([]);
    onMounted( async () => {
      const response = await UseService.getAll();
      uses.value = response.data
    });
    const selectItem = (item,index) => {
      data.id = item.id;
      data.name = item.name;
      data.info = item.info;
    };
    return {toast, uses, selectItem,use,data}
  },
  methods: {
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn xóa công dụng này không ?')) {
        this.deleteItem(item);
      }
    },
    resetForm() {
      this.use.name = '';
      this.use.info = '';
    },
    async deleteItem(item) {
      let index = this.uses.indexOf(item);
      if (index > -1) {
        this.uses.splice(index, 1);
        try {
          const response = await UseService.deleteOne(item.id);
          if (response.status === 200) {
            this.data = {id: '', name: '', info: ''};
            this.toast.success(response.data.message)
          } else {
            this.toast.error("Xảy ra lỗi khi xóa công dụng.")
          }
        }catch (e) {
          this.toast.error(e.response.data.detail)
        }
      }else{
        this.toast.error("Xảy ra lỗi khi xóa công dụng.")
      }
    },
    async createItem(){
      await UseService.create(this.use).then( async response =>{
        this.toast.success("Thêm mới công dụng thành công.")
        this.uses.push(response.data)
      }).catch(err=>{
        this.toast.error("Lỗi khi thêm công dụng.");
      })
    },
    async updateItem(){
      let index = this.uses.findIndex(item => item.id === this.data.id);
      try {
        const response = await UseService.update(this.data.id, this.data);
        this.uses.splice(index, 1);
        if (response.status === 200) {
          this.uses.splice(index,0,response.data)
          this.toast.success("Cập nhật công dụng thành công.")
        } else {
          this.toast.error(response.data.detail)
        }
      }catch (e) {
        this.toast.error(e.response.data.detail)
      }
    }
  }
}
</script>
<style scoped>
.card-body {
  max-height: 25rem;
  overflow-y: auto;
}
</style>
