<template>
  <div>
    <header class="sticky inset-0 z-50 border-b border-slate-100 bg-white/80 backdrop-blur-lg">
      <nav class="mx-auto flex gap-8 px-6 transition-all duration-200 ease-in-out lg:px-12 py-4">
        <div class="relative flex items-center">
          <a href="/">
            <img src="https://www.svgrepo.com/show/499831/target.svg" loading="lazy" style="color:transparent" width="32" height="32">
          </a>
        </div>
        <ul class="items-center justify-center gap-6 md:flex">
          <li class="pt-1.5 font-dm text-sm font-medium text-slate-700">
            <router-link to="/">Home</router-link>
          </li>
        </ul>
        <div class="flex-grow"></div>
        <div class="items-center justify-center gap-6 md:flex">
          <!-- Show Register and Sign In buttons if not authenticated -->
          <button v-if="!authToken"
                  class="rounded-md cursor-pointer bg-gradient-to-br from-blue-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
            Register
          </button>
          <button v-if="!authToken"
                  @click="redirectToLogin"
                  class="rounded-md cursor-pointer bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
            Sign in
          </button>

          <!-- Show Logout button if authenticated -->
          <button v-if="authToken"
                  @click="logout"
                  class="rounded-md cursor-pointer bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
            Logout
          </button>
        </div>
      </nav>
    </header>
    <router-view></router-view>
  </div>
</template>

<script>
  export default {
    data() {
      return {
        // Store auth token reactively
        authToken: localStorage.getItem('authToken') || null
      };
    },
    methods: {
      logout() {
        // Remove auth token from localStorage
        localStorage.removeItem("authToken");

        // Update the reactive authToken
        this.authToken = null;

        // Redirect to login page
        this.$router.push("/login");
      },
      redirectToLogin() {
        // Redirect to login page when clicking the "Sign in" button
        this.$router.push("/login");
      }
    }
  };
</script>
