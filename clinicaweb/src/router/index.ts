import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '@/views/HomeView.vue'
import { useAuthStore } from '@/stores/auth'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/login',
      name: 'login',
      component: () => import('@/views/LoginView.vue'),
    },
    {
      path: '/register',
      name: 'register',
      component: () => import('@/views/RegisterView.vue'),
    },
    {
      path: '/recover',
      name: 'recover',
      component: () => import('@/views/RecoverView.vue'),
    },
    {
      path: '/dashboard',
      name: 'dashboard',
      component: () => import('@/views/DashboardView.vue'),
      meta: { requiresAuth: true },
      children: [
        {
          path: '',
          name: 'dashboard.overview',
          component: () => import('@/views/dashboard/DashboardOverview.vue'),
          meta: {
            title: 'Resumen general',
            description:
              'Consulta un panorama general de tu actividad reciente y mantente al tanto de tus próximos compromisos clínicos.',
          },
        },
        {
          path: 'consultas/crear',
          name: 'dashboard.consultations.create',
          component: () => import('@/views/dashboard/ConsultationsCreateView.vue'),
          meta: {
            title: 'Crear nueva consulta',
            description:
              'Registra una nueva consulta capturando los datos del paciente, la valoración médica y las recomendaciones iniciales.',
          },
        },
        {
          path: 'consultas/historial',
          name: 'dashboard.consultations.history',
          component: () => import('@/views/dashboard/ConsultationHistoryView.vue'),
          meta: {
            title: 'Historial de consultas',
            description:
              'Revisa el detalle de las atenciones realizadas y filtra rápidamente para encontrar la información que necesitas.',
          },
        },
        {
          path: 'administracion/usuarios',
          name: 'dashboard.users.catalog',
          component: () => import('@/views/dashboard/UsersCatalogView.vue'),
          meta: {
            title: 'Catálogo de usuarios',
            description:
              'Administra las cuentas de acceso del personal administrativo y médico de la clínica desde un solo lugar.',
          },
        },
        {
          path: 'administracion/medicos',
          name: 'dashboard.doctors.catalog',
          component: () => import('@/views/dashboard/DoctorsCatalogView.vue'),
          meta: {
            title: 'Catálogo de médicos',
            description:
              'Gestiona el directorio médico, actualiza especialidades y mantiene los datos de contacto siempre disponibles.',
          },
        },
      ],
    },
  ],
})

router.beforeEach((to) => {
  const authStore = useAuthStore()
  const routeName = (to.name as string | undefined) ?? ''

  if (to.meta.requiresAuth && !authStore.isAuthenticated) {
    return {
      name: 'login',
      query: to.fullPath ? { redirect: to.fullPath } : undefined,
    }
  }

  if (['login', 'register', 'recover'].includes(routeName) && authStore.isAuthenticated) {
    return { name: 'dashboard.overview' }
  }

  return true
})

export default router
