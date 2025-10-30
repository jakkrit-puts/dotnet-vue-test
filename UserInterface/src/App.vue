<template>
  <nav className="bg-dark_green py-5 sticky top-0 z-50 backdrop-blur-md border-b-2 border-black">
    <div class="text-center font-bold">
       {{ currentTitle }}
    </div>
  </nav>
  <router-view />
</template>

<script>

export default {
  computed: {
    currentTitle() {
      return this.$route.meta.title || 'Default Title';
    }
  }
};
</script>