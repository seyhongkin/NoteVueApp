<template>
  <section>
    <div class="flex flex-col text-gray-700 items-center justify-center px-6 py-8 mx-auto mt-24 lg:py-0">
      <div class="w-full bg-white rounded-lg shadow md:mt-0 sm:max-w-md xl:p-0 text-gray-700">
        <div class="p-6 space-y-4 md:space-y-6 sm:p-8">
          <h1 class="text-xl font-bold leading-tight tracking-tight text-gray-900 md:text-2xl">
            Login
          </h1>
          <form class="space-y-4" @submit.prevent="handleLogin">
            <div>
              <label for="username" class="block mb-2 text-sm font-medium text-gray-900">Username</label>
              <input v-model="username" type="text" name="username" id="username" class="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full py-1.5 px-2.5" placeholder="Username">
            </div>
            <div>
              <label for="password" class="block mb-2 text-sm font-medium text-gray-900">Password</label>
              <input v-model="password" type="password" name="password" id="password" class="bg-gray-50 border border-gray-300 text-gray-900 rounded-lg focus:ring-primary-600 focus:border-primary-600 block w-full py-1.5 px-2.5" placeholder="********">
            </div>
            <button type="submit" class="w-full py-2 rounded-md bg-gradient-to-br from-green-600 to-emerald-400 px-3 py-1.5 font-dm text-sm font-medium text-white shadow-md shadow-green-400/50 transition-transform duration-200 ease-in-out hover:scale-[1.03]">
              Sign in
            </button>
            <p v-if="errorMessage" class="text-red-500 text-sm mt-2">{{ errorMessage }}</p>
          </form>
        </div>
      </div>
    </div>
  </section>
</template>

<script>
  import { ref } from "vue";
  import axios from "axios";

  export default {
    setup() {
      const username = ref("");
      const password = ref("");
      const errorMessage = ref("");

      const endPoint = "https://localhost:7110/api/User/Login";

      const handleLogin = async () => {
        if (!username.value || !password.value) {
          errorMessage.value = "Username and password are required";
          return;
        }

        try {
          const response = await axios.post(endPoint, {
            username: username.value,
            password: password.value,
          });

          if (response.status == 403) {
            errorMessage.value = "Invalid credentials";
          }

          if (response.data.token) {
            localStorage.setItem("accessToken", response.data.token);
            window.location.href = "/";
          }
        } catch (error) {
          if (error.response && error.response.status === 403) {
            errorMessage.value = "Username or password is not correct";
          } else {
            errorMessage.value = "An unexpected error occurred";
          }
        }
      };

      return { username, password, errorMessage, handleLogin };
    },
  };
</script>
