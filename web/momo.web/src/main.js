import Vue from 'vue'
import App from './App.vue'
import router from './router/router'
import store from './store/store'
import i18n from './lang'
import './plugins/element.js'
import './directive/premissionBtn'
import './assets/css/public.css'  
import './element-variables.scss'   

//表单设计
import FormMaking from '@/views/formDesign/index'
//vue特效
import VueParticles from 'vue-particles'
//复制粘贴功能
import VueClipboard from 'vue-clipboard2'
import { messages } from './assets/js/common'
// 引入字体文件,图标
import './assets/icon/iconfont.css'
import './assets/icon/iconfont.js'

Vue.use(VueParticles)
Vue.use(FormMaking)
Vue.use(VueClipboard)

//全局挂载提示框
Vue.prototype.$message = messages
Vue.config.productionTip = false
new Vue({
  i18n,
  router,
  store,
  render: h => h(App)  //Vue的渲染逻辑——Render函数 , 渲染App这个元素
}).$mount('#app')
