import { createRouter, createWebHistory } from 'vue-router';
import NotePage from './pages/NotePage.vue';
import LoginPage from './pages/LoginPage.vue';
import NoteFormPage from './pages/NoteFormPage.vue';

const routes = [
  {
    path: '/',
    component: NotePage,
    meta: { requiresAuth: true }
  },
  {
    path: '/note-form',
    component: NoteFormPage,
    meta: { requiresAuth: true }
  },
  {
    path: '/login',
    component: LoginPage,
    meta: { requiresGuest: true }
  },
  { path: '/:pathMatch(.*)*', redirect: '/' }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Function to check authentication status
const isAuthenticated = () => !!localStorage.getItem('authToken');

router.beforeEach((to, from, next) => {
  if (to.meta.requiresAuth && !isAuthenticated()) { // Call the function here
    next("/login");
  } else if (to.meta.requiresGuest && isAuthenticated()) {
    next("/");
  } else {
    next();
  }
});

export default router;
