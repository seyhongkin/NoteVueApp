<template>
  <div class="w-72 py-6 px-8 mt-20 bg-white rounded shadow-xl text-gray-800">
    <form @submit.prevent="login">
      <div class="pb-2">
        <label for="username" class="block font-bold">Username</label>
        <input v-model="username"
               type="text"
               id="username"
               placeholder="username"
               class="w-full border border-gray-300 py-2 pl-3 rounded mt-2 outline-none focus:ring-indigo-600" />
      </div>
      <div class="pb-4">
        <label for="password" class="block font-bold">Password</label>
        <input v-model="password"
               type="password"
               id="password"
               placeholder="********"
               class="w-full border border-gray-300 py-2 pl-3 rounded mt-2 outline-none focus:ring-indigo-600" />
      </div>
      <button type="submit"
              :disabled="loading"
              class="cursor-pointer py-2 px-4 block mt-6 bg-indigo-500 text-white font-bold w-full text-center rounded">
        {{ loading ? "Logging in..." : "Login" }}
      </button>
      <p v-if="error" class="text-red-500 mt-2">{{ error }}</p>
    </form>
  </div>
</template>

<script>
import axios from "axios";

export default {
  data() {
    return {
      username: "",
      password: "",
      loading: false,
      error: null,
    };
  },
  methods: {
    async login() {
      this.loading = true;
      this.error = null;

      try {
        const response = await axios.post("https://localhost:7110/api/User/Login", {
          username: this.username,
          password: this.password,
        });

        // Assuming the API returns a token
        localStorage.setItem("authToken", response.data.token);

        // Redirect to dashboard
        this.$router.push("/");
      } catch (err) {
        this.error = err.response?.data?.message || "Login failed";
      } finally {
        this.loading = false;
      }
    },
  },
};
</script>
