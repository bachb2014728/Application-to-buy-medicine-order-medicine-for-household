<template>
  <div class="row">
    <div class="card col-sm-8">
      <h5 class="card-title">Danh sách</h5>
      <div class="card-body">
        <table class="table table-bordered">
          <thead>
          <tr>
            <th scope="col">#</th>
            <th scope="col">dạng bài chế </th>
            <th class="col">Thông tin chi tiết </th>
            <th scope="col"></th>
          </tr>
          </thead>
          <tbody>
          <tr v-if="dosages" v-for="(item, index) in dosages" :key="index" @click="selectItem(item,index)">
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
          <h5 class="card-title">Thêm dạng bài chế </h5>
          <form class="g-3" @submit.prevent="createItem" >
            <div class="mb-3">
              <input type="text" v-model="dosage.name" class="form-control" placeholder="Tên dạng bài chế ">
            </div>
            <div class="mb-3">
              <input type="text" v-model="dosage.info" class="form-control" placeholder="Thông tin (nếu có)">
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
          <h5 class="card-title">Cập nhật dạng bài chế </h5>
          <form class="g-3" @submit.prevent="updateItem" >
            <div class="mb-3">
              <input type="text" v-model="data.name" class="form-control" placeholder="Tên dạng bài chế ">
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
import DosageService from "@/services/dosage.service.js";

export default {
  name: "DosageFormList",
  mounted() {
    document.title = 'Danh sách dạng bài chế '
  },
  setup() {
    const toast = useToast();
    const dosage = reactive({name:'',info:''})
    const data = reactive({id:'',name:'',info:''})
    const dosages = ref([]);
    onMounted( async () => {
      const response = await DosageService.getAll();
      dosages.value = response.data
    });
    const selectItem = (item,index) => {
      data.id = item.id;
      data.name = item.name;
      data.info = item.info;
    };
    return {toast, dosages, selectItem,dosage,data}
  },
  methods: {
    confirmDelete(item) {
      if (window.confirm('Bạn có chắc chắn xóa dạng bài chế  này không ?')) {
        this.deleteItem(item);
      }
    },
    resetForm() {
      this.dosage.name = '';
      this.dosage.info = '';
    },
    async deleteItem(item) {
      let index = this.dosages.indexOf(item);
      if (index > -1) {
        this.dosages.splice(index, 1);
        try {
          const response = await DosageService.deleteOne(item.id);
          if (response.status === 200) {
            this.data = {id: '', name: '', info: ''};
            this.toast.success(response.data.message)
          } else {
            this.toast.error("Xảy ra lỗi khi xóa dạng bài chế .")
          }
        }catch (e) {
          this.toast.error(e.response.data.detail)
        }
      }else{
        this.toast.error("Xảy ra lỗi khi xóa dạng bài chế .")
      }
    },
    async createItem(){
      await DosageService.create(this.dosage).then( async response =>{
        this.toast.success("Thêm mới dạng bài chế  thành công.")
        this.dosages.push(response.data)
      }).catch(err=>{
        this.toast.error("Lỗi khi thêm dạng bài chế .");
      })
    },
    async updateItem(){
      let index = this.dosages.findIndex(item => item.id === this.data.id);
      try {
        const response = await DosageService.update(this.data.id, this.data);
        this.dosages.splice(index, 1);
        if (response.status === 200) {
          this.dosages.splice(index,0,response.data)
          this.toast.success("Cập nhật dạng bài chế  thành công.")
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
