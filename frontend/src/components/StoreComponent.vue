<template>
  <nav>
    <ol class="breadcrumb">
      <li class="breadcrumb-item">
        <router-link to="/" style="text-decoration: none">Trang chủ</router-link>
      </li>
      <li class="breadcrumb-item" v-for="(item, index) in $route.meta.breadcrumb" :key="index">
        <a v-if="item.href" :href="item.href" style="text-decoration: none">{{ item.name }}</a>
        <span v-else>{{ item.name }}</span>
      </li>
    </ol>
  </nav>
  <router-view></router-view>
</template>
<script>
export default {
  name: "StoreComponent"
}
</script>

<style scoped>

</style>