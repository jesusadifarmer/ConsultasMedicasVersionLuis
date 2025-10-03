<template>
  <div class="grid gap-6 md:grid-cols-2">
    <article class="rounded-3xl bg-white/5 p-6 shadow-xl shadow-black/20 ring-1 ring-white/10">
      <h2 class="font-display text-xl font-semibold text-white">Próximas consultas</h2>
      <p class="mt-3 text-sm text-slate-300">
        Aún no tienes consultas agendadas para esta semana. Programa una nueva consulta para mantenerte al día con tu salud.
      </p>
    </article>
    <article class="rounded-3xl bg-white/5 p-6 shadow-xl shadow-black/20 ring-1 ring-white/10">
      <h2 class="font-display text-xl font-semibold text-white">Resumen rápido</h2>
      <ul class="mt-4 space-y-3 text-sm text-slate-200">
        <li class="flex items-center justify-between rounded-2xl bg-white/5 px-4 py-3">
          <span>Citas completadas este mes</span>
          <span class="font-semibold text-white">3</span>
        </li>
        <li class="flex items-center justify-between rounded-2xl bg-white/5 px-4 py-3">
          <span>Pacientes en seguimiento activo</span>
          <span class="font-semibold text-white">8</span>
        </li>
        <li class="flex items-center justify-between rounded-2xl bg-white/5 px-4 py-3">
          <span>Alertas pendientes</span>
          <span class="font-semibold text-white">0</span>
        </li>
      </ul>
    </article>
  </div>
</template>

<script setup lang="ts"></script>
