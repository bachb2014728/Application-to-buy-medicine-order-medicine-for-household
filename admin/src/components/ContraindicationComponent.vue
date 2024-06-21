<template>
  <main id="main" class="main">
    <div class="pagetitle">
      <h1>Quản lý chống chỉ định</h1>
      <nav>
        <ol class="breadcrumb">
          <li class="breadcrumb-item"><router-link to="/" style="text-decoration: none">Home</router-link></li>
          <li class="breadcrumb-item" v-for="(item, index) in $route.meta.breadcrumb" :key="index">
            <a v-if="item.href" :href="item.href" style="text-decoration: none">{{ item.name }}</a>
            <span v-else>{{ item.name }}</span>
          </li>
        </ol>
      </nav>
    </div>
    <section class="section">
      <router-view></router-view>
    </section>
  </main>
</template>
<script>
export default {
  name: "ContraindicationComponent"
}
</script>
<style scoped>

</style>