<template>
  <div class="mb-3">
    <div class="mb-3">
      <label for="formFile" class="form-label">Ảnh sản phẩm</label>
    </div>
    <div class="row">
      <div class="card" v-for="(src, index) in selectedItems" :key="index">
        <img class="file-upload-image" :src="'data:image/jpeg;base64,'+src.file" alt="your image" />
        <div class="card-body">
          <small class="card-text">{{ src.name }}</small>
        </div>
        <div class="text-center p-2">
          <a class="btn btn-sm btn-danger m-2" @click="removeImage(index,src)" >Remove</a>
        </div>
      </div>
      <div class="card empty" v-for="i in (5 - selectedItems.length)" :key="i + selectedItems.length"></div>
    </div>
  </div>
  <div class="mb-3">
    <div class="mb-3">
      <label for="formFile" class="form-label">Chọn hình ảnh</label>
      <input class="form-control" type="file" id="formFile" ref="fileInput" @change="readURL" accept="image/*" multiple/>
    </div>
  </div>
</template>

<script>
import ImageService from "@/services/image.service.js";
import {onMounted, ref} from "vue";

export default {
  name: "UploadFileComponent",
  props:['updateImage'],
  setup(props, _) {
    const selectedItems = ref([]);
    let showOld = ref(false)
    onMounted( async () => {
      for (let i = 0; i < props.updateImage.length; i++) {
        await ImageService.getOne(props.updateImage[i]).then(response=>{
          selectedItems.value.push(response.data)
          showOld.value = true;
        })
      }
    });
    return {
      selectedItems,showOld
    }
  },
  data() {
    return {
      showContent:false,imageSrcs: [], fileNames: [], listId:[], files: []
    }
  },
  methods: {
    readURL(event) {
      if (event.target.files) {
        this.files = Array.from(event.target.files);
        for (let i = 0; i < this.files.length; i++) {
          let reader = new FileReader();
          reader.onload = (e) => {
            let formData = new FormData();
            formData.append('imageData', this.files[i]);
            ImageService.uploadImage(formData).then(response=>{
              this.showContent = true;
              this.selectedItems.push(response.data)

            })
          };
          reader.readAsDataURL(this.files[i]);
        }
        this.$emit('update-list-id', this.selectedItems);
      } else {
        this.removeUpload();
      }
    },
    removeImage(index,item) {
      ImageService.delete(item.id).then(response=>{
        this.selectedItems.splice(index, 1);
        if(this.selectedItems.length === 0) {
          this.showContent = false;
        }
        this.$emit('update-list-id', this.selectedItems);
      })
    },
  }
}
</script>

<style scoped>
.file-upload-image {
  width: 100%;
  height: auto;
  padding: 0.5rem;
}

.row {
  display: flex;
  flex-wrap: wrap;
  justify-content: space-between;
}

.card {
  flex: 1 0 calc(20% - 10px);
  margin: 5px;
}

.card.empty {
  visibility: hidden;
}
</style>
