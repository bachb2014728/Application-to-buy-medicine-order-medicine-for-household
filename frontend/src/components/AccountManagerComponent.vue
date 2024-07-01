<template>
  <div class="row">
    <div class="col-md-12">
      <ul class="nav nav-pills flex-column flex-md-row mb-3">
        <li class="nav-item">
          <router-link to="/account/profile" class="nav-link" exact active-class="active">Tài khoản</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/account/notification" class="nav-link" exact active-class="active">Thông báo</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/account/order-manage" class="nav-link" exact active-class="active">Đơn hàng</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/account/receivers" class="nav-link" exact active-class="active">Địa chỉ nhận</router-link>
        </li>
        <li class="nav-item">
          <router-link to="/account/comments" class="nav-link" exact active-class="active">Sản phẩm</router-link>
        </li>
      </ul>
    </div>
  </div>
  <router-view></router-view>
</template>
<script>
export default {
  name: "AccountManagerComponent"
}
</script>
<style scoped>

</style>