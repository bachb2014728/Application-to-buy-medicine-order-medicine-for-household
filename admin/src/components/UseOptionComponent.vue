<template>
  <div class="card-body">
    <h5 class="card-title">Chọn công dụng</h5>
    <ul class="list-group scrollable-list">
      <li v-for="(item, index) in listItem" :key="index" class="list-group-item">
        <input class="form-check-input me-1" :checked="isSelected(item)" type="checkbox"
               @change="event => handleCheck(item,event,index)">
        {{item.name}}
      </li>
    </ul>
  </div>
</template>

<script>
import {onMounted, ref} from "vue";
import UseService from "@/services/use.service.js";

export default {
  name:"UseOptionComponent",
  props:['useChecked'],
  setup(props, { emit }) {
    const listItem = ref([]);
    const selectedItems = ref([]);

    const handleCheck = (item, event) => {
      const isChecked = event.target.checked;
      const index = selectedItems.value.findIndex(i => i.id === item.id && i.name === item.name && i.info === item.info);
      if (isChecked && index < 0) {
        selectedItems.value.push(item);
      } else if (!isChecked && index >= 0) {
        selectedItems.value.splice(index, 1);
      }
      emit('get-use-id', selectedItems.value);
    };
    const isSelected = (object) => {
      return props.useChecked.some(
          item => item.id === object.id && item.name === object.name && item.info === object.info)
    };

    onMounted( async () => {
      const response = await UseService.getAll();
      listItem.value = response.data;
      selectedItems.value = props.useChecked;
    });

    return {
      listItem,
      handleCheck,
      isSelected
    }
  },
}
</script>

<style scoped>
.scrollable-list {
  max-height: 30rem;
  overflow-y: auto;
}
</style>
