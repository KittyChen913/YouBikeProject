<template>
  <div id="app">
    <a-layout>
      <a-layout-header :style="{ position: 'fixed', zIndex: 1, width: '100%' }">
        <a-row>
          <a-col :span="6">
            <div class="logo">
              <img src="./assets/bike.png" />
            </div>
          </a-col>
          <a-col :span="18"> </a-col>
        </a-row>
      </a-layout-header>
      <router-view />
    </a-layout>
  </div>
</template>

<script lang="ts">
import { Vue } from 'vue-property-decorator'

export default class App extends Vue {}
</script>

<style>
</style>
