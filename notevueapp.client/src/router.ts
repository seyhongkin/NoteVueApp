import { createRouter, createWebHistory } from 'vue-router';
import Login from './pages/Login.vue';
import Signup from './pages/Signup.vue';
import Home from './pages/Home.vue';
import Note from './pages/Note.vue';
import axios from 'axios';

const endPoint = "https://localhost:7110/api/User/Verify";

const routes = [
  {
    path: '/',
    component: Home,
    meta: { requiresAuth: true }
  },
  {
    path: '/Note',
    component: Note,
    meta: { requiresAuth: true }
  },
  {
    path: '/Note/:id',
    component: Note,
    meta: { requiresAuth: true }
  },
  {
    path: '/login',
    component: Login,
    meta: { requiresGuest: true }
  },
  {
    path: '/signup',
    component: Signup,
    meta: { requiresGuest: true }
  },
  { path: '/:pathMatch(.*)*', redirect: '/' }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

router.beforeEach(async (to, from, next) => {
  const token = localStorage.getItem('accessToken');

  if (to.meta.requiresAuth && !token) {
    return next('/login');
  }

  if (to.meta.requiresGuest && token) {
    return next('/');
  }

  // If a token exists, verify it
  if (token) {
    try {
      const response = await axios.post(endPoint, {}, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      // If the response status is 401, redirect to login
      if (response.status === 401) {
        localStorage.removeItem('accessToken');
        return next('/login');
      }
    } catch (error) {
      console.error('Error verifying token:', error);

      // In case of any error, remove the token and redirect to login
      localStorage.removeItem('accessToken');
      return next('/login');
    }
  }

  next();
});

export default router;
