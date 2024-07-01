<template>
  <ckeditor
      :editor="editor"
      v-model="editorData"
      @ready="onReady"
      @input="onChange"
  ></ckeditor>
</template>

<script>
import ClassicEditor from '@ckeditor/ckeditor5-build-classic';
import CKEditor from '@ckeditor/ckeditor5-vue';
import CKEditorInspector from '@ckeditor/ckeditor5-inspector';
import {ref} from "vue";

export default {
  name: 'ConvertCKEDITOR',
  props:['data'],
  components: {
    ckeditor: CKEditor.component,
  },
  setup(props){
    const data = ref(props.data)
    return {data}
  },
  data() {
    return {
      editor: ClassicEditor,
      editorData: this.data
    };
  },
  methods: {
    onReady(editor) {
      CKEditorInspector.attach(editor);
    },
    onChange(data) {
    },
  },
};
</script>