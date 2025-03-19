<template>
  <header class="sticky inset-0 z-50 border-b border-slate-100 bg-white/80 backdrop-blur-lg">
    <nav class="mx-4 lg:mx-auto flex max-w-6xl gap-8 transition-all duration-200 ease-in-out py-4">
      <div class="relative flex items-center">
        <router-link to="/" class="font-bold">
          NoteApp
        </router-link>
      </div>
      <div class="flex-grow"></div>
      <div class="items-center justify-center gap-6 flex">
        <router-link to="/login" class="font-dm text-sm font-medium text-slate-700">Sign in</router-link>
        <router-link to="/signup"
                     class="rounded-md bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
          Sign up
        </router-link>
      </div>
    </nav>
  </header>
  <div class="max-w-6xl mx-4 lg:mx-auto pt-2">
    <router-view></router-view>
  </div>
</template>

<script setup>

</script>
