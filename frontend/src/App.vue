<template>
  <Nav/>
  <div class="container mt-3">
    <router-view/>
  </div>
</template>
<script>
import Nav from "@/components/NavbarComponent.vue";
export default {
  components: {Nav}
}
</script>