<template>
  <h1 class="font-bold text-lg mb-2">{{ noteId ? "Update Note" : "Create Note" }}</h1>
  <div>
    <label for="title" class="block mb-2 text-sm font-medium text-gray-900">
      Title
      <span class="text-red-500">*</span>
    </label>
    <input v-model="title" type="text" id="title" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full px-2.5 py-1.5"
           placeholder="E.g. Review meeting document" />
    <small class="text-red-500">{{errorMessage}}</small>

    <label for="content" class="block mb-1 text-sm font-medium text-gray-900 mt-3">Content</label>
    <textarea v-model="content" id="content" rows="4" class="block p-2.5 w-full text-sm text-gray-900 bg-gray-50 rounded-lg border border-gray-300 focus:ring-blue-500 focus:border-blue-500" placeholder="Write your thoughts here..."></textarea>

  </div>
  <div v-if="noteId" class="mt-3 grid grid-cols-1 md:grid-cols-2 gap-2">
    <div>
      <label for="title" class="block mb-2 text-sm font-medium text-gray-900">
        Created At
        <span class="text-red-500">*</span>
      </label>
      <input :value="createdAt" type="datetime" id="title" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full px-2.5 py-1.5"
             readonly/>
    </div>

    <div>
      <label for="title" class="block mb-2 text-sm font-medium text-gray-900">
        Updated At
        <span class="text-red-500">*</span>
      </label>
      <input :value="updatedAt" type="datetime" id="title" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full px-2.5 py-1.5"
             readonly/>
    </div>
  </div>



  <button @click="handleSave" type="button" class="flex items-center text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium cursor-pointer rounded-lg text-sm px-3 py-1.5 gap-2 me-2 my-2">
    <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
      <path stroke-linecap="round" stroke-linejoin="round" d="M16.5 3.75V16.5L12 14.25 7.5 16.5V3.75m9 0H18A2.25 2.25 0 0 1 20.25 6v12A2.25 2.25 0 0 1 18 20.25H6A2.25 2.25 0 0 1 3.75 18V6A2.25 2.25 0 0 1 6 3.75h1.5m9 0h-9" />
    </svg>
    Save
  </button>
</template>

<script setup>
  import { ref, onMounted } from "vue";
  import { useRoute, useRouter } from "vue-router";
  import axios from "axios";

  const route = useRoute();
  const router = useRouter();

  const noteId = route.params.id;
  const title = ref("");
  const content = ref("");
  const createdAt = ref(null);
  const updatedAt = ref(null);

  const errorMessage = ref("");

  const endPoint = "https://localhost:7110/api/Note";

  // Fetch note data if noteId exists (update mode)
  onMounted(async () => {
    if (noteId) {
      try {
        const response = await axios.get(`${endPoint}/${noteId}`, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
        });
        title.value = response.data.title;
        content.value = response.data.content;
        createdAt.value = formatDate(response.data.createdAt);
        updatedAt.value = response.data.updatedAt ? formatDate(response.data.updatedAt) : null;
      } catch (error) {
        router.push("/");
      }
    }
  });

  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString() + " " + date.toLocaleTimeString();
  };

  const handleSave = async () => {
    if (title.value.length == 0) {
      errorMessage.value = "Title is required";
      return;
    }

    const data = { title: title.value, content: content.value };
    try {
      if (noteId) {
        // If noteId exists, update the note (PUT request)
        await axios.put(`${endPoint}/${noteId}`, data, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
        });
        router.push("/");
      } else {
        // If noteId is null, create a new note (POST request)
        await axios.post(endPoint, data, {
          headers: {
            Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
          },
        });
        router.push("/");
      }
    } catch (error) {
      console.error("Failed to save note", error);
    }
  };
</script>
