<template>
  <div class="row">
    <div class="col-sm-8">
      <div class="card" v-if="data">
        <div class="card-body">
          <div class="card-title">Thông báo</div>
          <div class="demo-inline-spacing mt-3">
            <div class="list-group list-group-flush">
              <div v-for="(item,index) in data" :key="index"
                   class="list-group-item list-group-item-action alert alert-secondary mb-3" role="alert">
                {{item.message}} : {{FormatDate(item.createdOn)}}
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
<script>
import {onMounted, ref} from "vue";
import NotificationService from "@/services/notification.service.js";
import {formatDate} from "@/utils/time.utils.js";

export default {
  name: "Notification",
  mounted() {
    document.title='Quản lý thông báo'
  },
  setup(){
    const data = ref([])
    onMounted(async () =>{
      NotificationService.getAll().then(response=>{
        data.value = response.data
      })
    })
    return{data}
  },
  methods:{
    FormatDate(date){
      return formatDate(date);
    }
  }
}
</script>
<style scoped>

</style>