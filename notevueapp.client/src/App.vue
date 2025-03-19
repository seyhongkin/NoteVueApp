<template>
  <header class="sticky inset-0 z-50 border-b border-slate-100 bg-white/80 backdrop-blur-lg">
    <nav class="mx-4 lg:mx-auto flex max-w-6xl gap-8 transition-all duration-200 ease-in-out py-4">
      <div class="relative flex items-center">
        <router-link to="/" class="font-bold text-xl">
          NoteApp
        </router-link>
      </div>
      <div class="flex-grow"></div>

      <div v-if="!isAuthenticated" class="items-center justify-center gap-6 flex">
        <router-link to="/login" class="font-dm text-sm font-medium text-slate-700">Sign in</router-link>
        <router-link to="/signup"
                     class="rounded-md bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
          Sign up
        </router-link>
      </div>
      <div v-if="isAuthenticated" class="items-center justify-center gap-6 flex">
        <p class="capitalize flex items-center gap-1">
          <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="size-4">
            <path stroke-linecap="round" stroke-linejoin="round" d="M15.75 6a3.75 3.75 0 1 1-7.5 0 3.75 3.75 0 0 1 7.5 0ZM4.501 20.118a7.5 7.5 0 0 1 14.998 0A17.933 17.933 0 0 1 12 21.75c-2.676 0-5.216-.584-7.499-1.632Z" />
          </svg>

          {{ userData?.name }}
        </p>
        <div @click="logout" class="cursor-pointer rounded-md bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
          Logout
        </div>
      </div>
    </nav>
  </header>
  <div class="max-w-6xl mx-4 lg:mx-auto pt-2">
    <router-view></router-view>
  </div>
</template>

<script setup>
  import { ref, computed, watchEffect } from 'vue';
  import { useRouter } from 'vue-router';
  import { jwtDecode } from "jwt-decode";

  const router = useRouter();
  const userData = ref(null);

  const accessToken = ref(localStorage.getItem('accessToken'));

  const isAuthenticated = computed(() => !!accessToken.value);

  watchEffect(() => {
    accessToken.value = localStorage.getItem('accessToken');

    if (isAuthenticated.value) {
      try {
        userData.value = jwtDecode(accessToken.value);
      } catch (error) {
        console.error("Invalid token:", error);
        userData.value = null;
        logout();
      }
    } else {
      userData.value = null;
    }
  });

  function logout() {
    localStorage.removeItem('accessToken');
    accessToken.value = null;
    userData.value = null;
    router.push('/login');
  }
</script>
