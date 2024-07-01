<template>
  <div class="text-center mt-2">
    <h2 class="mb-2 mx-2">Cảm ơn bạn đã mua hàng!</h2>
    <p class="mb-4 mx-2">Sản phẩm sẽ được xet duyệt trong thời gian sớm nhất</p>
    <router-link to="/" class="btn btn-primary">Back to home</router-link>
  </div>
</template>
<script>
export default {
  name: "NotificationAfterOrder",
  mounted() {
    document.title='Đặt hàng thành công'
  }
}
</script>
<style scoped>

</style>