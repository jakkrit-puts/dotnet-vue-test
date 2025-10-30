<template>
  <div class="flex min-h-full flex-1 flex-col justify-center px-6 py-12 lg:px-8">
    <div class="sm:mx-auto sm:w-full sm:max-w-sm">
      <div v-if="loading" class="loading">
        Loading profile...
      </div>

      <div v-else-if="error" class="error">
        Error: {{ error }}
      </div>

      <h2 class="mt-10 text-center  text-gray-900">Welcome User: {{ profile }} </h2>
    </div>
  </div>
</template>

<script setup>
import axios from 'axios';
import { ref, onMounted } from 'vue';

const profile = ref(null);
const loading = ref(true);
const error = ref(null);

onMounted(async () => {
  try {
    const response = await axios.get('http://localhost:5076/api/User/profile', {
      headers: {
        'Authorization': `Bearer ${localStorage.getItem('token')}`
      }
    });

    if (!response.status === 200) {
      throw new Error('Failed to fetch profile');
    }

    const data = await response.data;
    profile.value = data.user.username;
  } catch (err) {
    error.value = err.message;
    console.error('Profile fetch error:', err);
  } finally {
    loading.value = false;
  }
});


</script>