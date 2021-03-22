import Vue from 'vue'
import VueRouter from 'vue-router'
import YoubikeLog from './views/YoubikeLog.vue'

Vue.use(VueRouter)

const router = new VueRouter({
    routes: [
        { path: '/', component: YoubikeLog }
    ]
})

export default router