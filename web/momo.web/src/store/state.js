const state = {
    token: '',
    roles: [], //用户角色
    tagsList: [], //打开的标签页个数,
    isCollapse: false, //侧边导航是否折叠
    lang:'zh',//默认语言
    breadList:['home']//面包屑导航
}
export default state
//state就是Vuex中的公共的状态, 我是将state看作是所有组件的data, 用于保存所有组件的公共数据.
