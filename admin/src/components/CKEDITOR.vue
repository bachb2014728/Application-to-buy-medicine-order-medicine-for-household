<template>
  <div id="app">
    <ckeditor :editor="editor" v-model="editorData" :config="editorConfig" @input="handleInput"></ckeditor>
  </div>
</template>

<script>
import ClassicEditor from "@ckeditor/ckeditor5-build-classic";

export default {
  name: "CKEDITOR",
  props: ['data'],
  data() {
    return {
      editor: ClassicEditor,
      editorData: '',
      editorConfig: {}
    };
  },
  mounted() {
    this.editorData = this.data;
  },
  methods: {
    handleInput(editorData) {
      this.$emit('updateData', editorData);
    }
  }
}
</script>

<style scoped>

</style>
