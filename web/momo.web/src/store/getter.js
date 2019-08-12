const getters = {
    roles:state=>{
        return state.roles
    }
}
export default getters

//我将getters属性理解为所有组件的computed属性,也就是计算属性。vuex的官方文档也是说到可以将getter理解为
//store的计算属性,getters的返回值会根据它的依赖被缓存起来，且只有当它的依赖值发生了改变才会被重新计算。