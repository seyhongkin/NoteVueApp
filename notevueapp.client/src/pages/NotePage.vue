<template>
  <div class="px-6 md:px-10 pt-4">
    <h1 class="text-2xl font-bold mb-4 pb-2 text-white">My Notes</h1>

    <div class="flex md:flex-row flex-col gap-2">
      <!-- Search -->
      <div class="relative w-full w-full md:w-md">
        <div class="absolute inset-y-0 start-0 flex items-center ps-3 pointer-events-none">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4 text-gray-500">
            <path stroke-linecap="round" stroke-linejoin="round" d="m21 21-5.197-5.197m0 0A7.5 7.5 0 1 0 5.196 5.196a7.5 7.5 0 0 0 10.607 10.607Z" />
          </svg>

        </div>
        <input type="text" id="voice-search" @change="handleSearchChange" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full ps-10 px-2.5 py-1.5  dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
               placeholder="Search..." />
      </div>

      <!-- Sort -->
      <button @click="toggleSortDirection" class="flex justify-center items-center rounded-md cursor-pointer bg-gradient-to-br from-red-600 to-orange-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-orange-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
        Sort

        <svg v-if="sortAsc" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
          <path stroke-linecap="round" stroke-linejoin="round" d="M8.25 6.75 12 3m0 0 3.75 3.75M12 3v18" />
        </svg>

        <svg v-if="!sortAsc" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
          <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 17.25 12 21m0 0-3.75-3.75M12 21V3" />
        </svg>

      </button>

      <button @click="showCreateModal"
              class="rounded-md cursor-pointer bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
        Create New Note
      </button>
    </div>

    <p v-if="notes.length == 0" class="text-2xl pt-4">No note found.</p>

    <div class="grid md:grid-cols-2 lg:grid-cols-4 xl:grid-cols-6 gap-4 pt-4">
      <div v-for="note in notes" :key="note.id"
           class="relative block p-6 bg-white border border-gray-200 rounded-lg shadow-sm hover:bg-gray-100 dark:bg-gray-800 dark:border-gray-700 dark:hover:bg-gray-700">

        <h5 class="mb-2 text-2xl font-bold tracking-tight text-gray-900 dark:text-white">{{ note.title }}</h5>
        <p class="font-normal text-gray-700 dark:text-gray-400">{{ formatDate(note.createdAt) }}</p>

        <div class="absolute top-1 right-1 gap-2">
          <!-- Edit Button -->
          <div class="hover:bg-gray-600 cursor-pointer rounded inline-flex p-1" @click="showEditModal(note)">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5 text-blue-500">
              <path stroke-linecap="round" stroke-linejoin="round" d="M16.862 4.487 18.55 2.8a1.875 1.875 0 1 1 2.652 2.652L10.582 16.07a4.5 4.5 0 0 1-1.897 1.13L6 18l.8-2.685a4.5 4.5 0 0 1 1.13-1.897l8.932-8.931Zm0 0L19.5 7.125M18 14v4.75A2.25 2.25 0 0 1 15.75 21H5.25A2.25 2.25 0 0 1 3 18.75V8.25A2.25 2.25 0 0 1 5.25 6H10" />
            </svg>
          </div>

          <!-- Delete Button -->
          <div class="hover:bg-gray-600 cursor-pointer rounded inline-flex p-1" @click="deleteNote(note.id)">
            <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-5 text-red-500">
              <path stroke-linecap="round" stroke-linejoin="round" d="m14.74 9-.346 9m-4.788 0L9.26 9m9.968-3.21c.342.052.682.107 1.022.166m-1.022-.165L18.16 19.673a2.25 2.25 0 0 1-2.244 2.077H8.084a2.25 2.25 0 0 1-2.244-2.077L4.772 5.79m14.456 0a48.108 48.108 0 0 0-3.478-.397m-12 .562c.34-.059.68-.114 1.022-.165m0 0a48.11 48.11 0 0 1 3.478-.397m7.5 0v-.916c0-1.18-.91-2.164-2.09-2.201a51.964 51.964 0 0 0-3.32 0c-1.18.037-2.09 1.022-2.09 2.201v.916m7.5 0a48.667 48.667 0 0 0-7.5 0" />
            </svg>
          </div>
        </div>
      </div>
    </div>

    <!-- Create/Edit Note Modal -->
    <div v-if="showModal" class="fixed inset-0 flex items-center justify-center backdrop-blur-sm text-gray-800">
      <div class="bg-white p-6 rounded-lg shadow-lg w-96">
        <h3 class="text-xl font-bold mb-4">{{ isEditMode ? 'Edit' : 'Create' }} Note</h3>

        <form @submit.prevent="isEditMode ? updateNote() : createNote()">
          <div class="mb-4">
            <label for="title" class="block text-gray-700">Title</label>
            <input type="text" id="title" v-model="newNote.title" class="w-full p-2 border border-gray-300 rounded" required />
          </div>

          <div class="mb-4">
            <label for="content" class="block text-gray-700">Content</label>
            <textarea id="content" v-model="newNote.content" class="w-full p-2 border border-gray-300 rounded"></textarea>
          </div>

          <div class="flex justify-between">
            <button @click="closeCreateModal" type="button" class="px-4 py-2 bg-gray-300 rounded">Cancel</button>
            <button type="submit" class="px-4 py-2 bg-blue-600 text-white rounded">{{ isEditMode ? 'Update' : 'Create' }}</button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script>
  import axios from "axios";

  export default {
    data() {
      return {
        notes: [],
        showModal: false,
        search: '',
        sortAsc: true,
        isEditMode: false,  // To track if we are in edit mode
        newNote: {
          id: null,
          title: "",
          content: ""
        }
      };
    },
    created() {
      this.fetchUsers();
    },
    methods: {
      async fetchUsers() {
        try {
          const response = await axios.get(`https://localhost:7110/api/Note?search=${this.search}&isSortAsc=${this.sortAsc}`, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("authToken")}`  // Use Bearer token
            }
          });
          this.notes = response.data;
        } catch (error) {
          console.error("Error fetching notes:", error);
        }
      },

      async toggleSortDirection(){
        this.sortAsc = !this.sortAsc;
        await this.fetchUsers();
      },

      async handleSearchChange(event){
        this.search = event.target.value;
        await this.fetchUsers();
      },

      async deleteNote(noteId) {
        console.log(noteId);
        const isConfirmed = confirm("Are you sure you want to delete this note?");
        if (isConfirmed) {
          try {
            const response = await axios.delete(`https://localhost:7110/api/Note/${noteId}`, {
              headers: {
                Authorization: `Bearer ${localStorage.getItem("authToken")}`
              }
            });
            this.notes = this.notes.filter(note => note.id !== noteId);
          } catch (error) {
            console.error("Error deleting note:", error);
          }
        } else {
          console.log("Deletion cancelled.");
        }
      },

      // Open Create Note Modal
      showCreateModal() {
        this.isEditMode = false;
        this.showModal = true;
        this.newNote = { title: "", content: "" }; // Reset form
      },

      // Open Edit Note Modal
      showEditModal(note) {
        this.isEditMode = true;
        this.showModal = true;
        this.newNote = { ...note }; // Pre-fill form with selected note data
      },

      // Close Create/Edit Modal
      closeCreateModal() {
        this.showModal = false;
        this.newNote = { id: null, title: "", content: "" }; // Reset form fields
      },

      // Submit New Note
      async createNote() {
        try {
          const response = await axios.post("https://localhost:7110/api/Note", this.newNote, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("authToken")}`
            }
          });
          this.notes.push(response.data);
          this.closeCreateModal();
        } catch (error) {
          console.error("Error creating note:", error);
        }
      },

      // Submit Updated Note
      async updateNote() {
        try {
          const response = await axios.put(`https://localhost:7110/api/Note/${this.newNote.id}`, this.newNote, {
            headers: {
              Authorization: `Bearer ${localStorage.getItem("authToken")}`
            }
          });
          // Update the note in the notes list
          const index = this.notes.findIndex(note => note.id === this.newNote.id);
          this.notes[index] = response.data;
          this.closeCreateModal();
        } catch (error) {
          console.error("Error updating note:", error);
        }
      },

      formatDate(date) {
        const d = new Date(date);
        return d.toLocaleString('en-GB', {
          year: 'numeric',
          month: '2-digit',
          day: '2-digit',
          hour: '2-digit',
          minute: '2-digit',
          hour12: false,  // 24-hour format
        }).replace(',', '');
      },
    }
  };
</script>
