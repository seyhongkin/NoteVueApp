<template>
  <div>
    <div class="flex justify-between">
      <h1 class="font-bold text-2xl mb-4 text-gray-800">My Notes</h1>

      <section>
        <router-link to="/Note" class="bg-green-500 hover:bg-green-700 cursor-pointer text-white p-1 rounded flex justify-between items-center gap-1 px-3">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
            <path stroke-linecap="round" stroke-linejoin="round" d="M12 4.5v15m7.5-7.5h-15" />
          </svg>
          New Note
        </router-link>
      </section>
    </div>

    <!-- filter -->
    <div class="mb-2 flex gap-2">
      <div class="w-full md:max-w-sm">
        <label for="default-search" class="mb-2 text-sm font-medium text-gray-900 sr-only">Search</label>
        <div class="relative">
          <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
            <svg class="w-4 h-4 text-gray-500 " aria-hidden="true" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 20 20">
              <path stroke="currentColor" stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="m19 19-4-4m0-7A7 7 0 1 1 1 8a7 7 0 0 1 14 0Z" />
            </svg>
          </div>
          <input type="search" v-model="search" @input="handleSearch" id="default-search" class="block w-full px-4 py-1.5 ps-10 text-sm text-gray-900 border border-gray-300 rounded-lg bg-gray-50 focus:ring-blue-500 focus:border-blue-500" placeholder="Search Notes title..." required />
        </div>
      </div>
      <button @click="handleToggleSort" type="button" class="flex justify-between items-center gap-1 py-1.5 px-3 me-2 mb-2 text-sm font-medium text-gray-900 focus:outline-none bg-white rounded-lg border border-gray-200 hover:bg-gray-100 hover:text-blue-700 focus:z-10 focus:ring-4 focus:ring-gray-100">
        Sort

        <svg v-if="sortAsc" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
          <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 6.75 12 3m0 0 3.75 3.75M12 3v18" />
        </svg>

        <svg v-if="!sortAsc" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
          <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 17.25 12 21m0 0-3.75-3.75M12 21V3" />
        </svg>


      </button>
    </div>

    <div v-if="loading" class="text-center text-gray-600">Loading notes...</div>
    <div v-if="errorMessage" class="text-red-500">{{ errorMessage }}</div>

    <div v-if="notes.length" class="grid grid-cols-1 md:grid-cols-3 gap-4">
      <!-- Note items -->
      <div v-for="note in notes" :key="note.id" class="relative block p-3 px-4 bg-white border border-gray-300 rounded-lg shadow-sm hover:bg-gray-100 text-gray-800">
        <h5 class="text-xl font-bold tracking-tight">{{ note.title }}</h5>
        <p class="font-normal text-gray-700">
          {{ formatDate(note.createdAt) }}
        </p>

        <div class="absolute bottom-2 right-2 flex justify-center gap-2">
          <router-link :to="`/Note/${note.id}`" class="bg-blue-500 hover:bg-blue-700 cursor-pointer text-white p-1 rounded">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
              <path stroke-linecap="round" stroke-linejoin="round" d="m16.862 4.487 1.687-1.688a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
            </svg>
          </router-link>

          <div @click="deleteNote(note.id)" class="bg-red-500 hover:bg-red-700 cursor-pointer text-white p-1 rounded">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5">
              <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
            </svg>
          </div>
        </div>
      </div>
      <!-- End Note items -->
    </div>

    <div v-else-if="!loading" class="text-gray-600 text-center mt-4">
      No notes available.
    </div>
  </div>
</template>

<script>
  import { ref, onMounted } from "vue";
  import axios from "axios";

  export default {
    setup() {
      const notes = ref([]);
      const loading = ref(false);
      const errorMessage = ref("");
      const search = ref("");
      const sortAsc = ref(true);

      const endPoint = "https://localhost:7110/api/Note";

      // Fetch Notes Function
      const fetchNotes = async () => {
        loading.value = true;
        errorMessage.value = "";

        try {
          const response = await axios.get(`${endPoint}?search=${search.value}&isSortAsc=${sortAsc.value}`, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
            },
          });
          notes.value = response.data;
        } catch (error) {
          errorMessage.value = "Failed to fetch notes. Please try again.";
          console.error(error);
        } finally {
          loading.value = false;
        }
      };

      const handleSearch = async () => {
        await fetchNotes();
      };

      const handleToggleSort = async () => {
        sortAsc.value = !sortAsc.value;
        await fetchNotes();
      }

      const deleteNote = async (noteId) => {
        loading.value = true;
        errorMessage.value = "";

        try {
          const response = await axios.delete(endPoint + `/${noteId}`, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("accessToken")}`,
            },
          });

          // Remove the deleted note from the local state
          notes.value = notes.value.filter(note => note.id !== noteId);
        } catch (error) {
          errorMessage.value = "Failed to delete note. Please try again.";
          console.error(error);
        } finally {
          loading.value = false;
        }
      }

      // Fetch notes automatically when the component is mounted
      onMounted(fetchNotes);

      // Format Date Helper
      const formatDate = (dateString) => {
        const date = new Date(dateString);
        return date.toLocaleDateString() + " " + date.toLocaleTimeString();
      };

      return { notes, loading, errorMessage, search, sortAsc, fetchNotes, deleteNote, formatDate, handleSearch, handleToggleSort };
    },
  };
</script>
