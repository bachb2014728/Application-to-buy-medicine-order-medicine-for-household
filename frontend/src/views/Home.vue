<template>
  <div class="row justify-content-start" v-if="products && products.length">
    <div class="col-3" v-for="(item,index) in products" :key="index">
      <ProductItemComponent :product = "item" />
    </div>
  </div>
</template>
<script>
import ProductItemComponent from "@/components/ProductItemComponent.vue";
import {onMounted, ref} from "vue";
import ProductClientService from "@/services/product.service.js";
import ProductService from "@/services/product.service.js";

export default {
  name: "Home",
  components: {ProductItemComponent},
  mounted(){
    document.title = 'Trang chủ'

  },
  setup(){
    const products = ref([])
    const message = ref()
    onMounted(async () =>{
      ProductService.getAll().then(response=>{
        for (let item of response.data){
          ProductService.getOne(item.id).then(res=>{
            products.value.push(res.data)
          })
        }
      }).catch(err=>{
        message.value = err.response.data.detail
      })
    })
    return{products}
  }
}
</script>
<style scoped>

</style>