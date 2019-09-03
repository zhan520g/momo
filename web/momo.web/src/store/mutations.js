
const mutations = {
    //保存token
    COMMIT_TOKEN(state, token) {
        state.token = token;  //这里作者挖了一个坑, 用了对象 COMMIT_TOKEN(state, object.token )
    },
    //保存标签
    TAGES_LIST(state, arr) {
        state.tagsList = arr;
    },
    //是否奔溃
    IS_COLLAPSE(state, bool) {
        state.isCollapse = bool;
    },
    //保存用户
    COMMIT_ROLE(state, roles) {
        state.roles = roles
    },
    GET_LANGUAGE(state,lang){
        state.lang=lang
    },
    SET_BREAD(state,breadList){
        state.breadList=breadList
    }
}
export default mutations


//我将mutaions理解为store中的methods, mutations对象中保存着更改数据的回调函数,
//该函数名官方规定叫type, 第一个参数是state, 第二参数是payload, 也就是自定义的参数.
//改变state的值必须经过mutations

