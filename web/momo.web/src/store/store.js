import Vue from 'vue'
import Vuex from 'vuex'
import state from "./state";
import mutations from "./mutations";
import actions from "./action";
import getters from "./getter";
//引入vuex 数据持久化插件
import createPersistedState from "vuex-persistedstate"
Vue.use(Vuex)

export default new Vuex.Store({
  state,
  mutations,
  actions,
  getters,
  plugins: [createPersistedState()]
})

//store.js是vuex模块整合文件,由于刷新页面会造成vuex数据丢失，所以这里引入了一个vuex数据持久话插件，
//将state里面的数据保存到localstorage。
