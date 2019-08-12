const actions={
    
}
export default actions


//actions 类似于 mutations，不同在于：
// 1.actions提交的是mutations而不是直接变更状态
// 2.actions中可以包含异步操作, mutations中绝对不允许出现异步
// 3.actions中的回调函数的第一个参数是context, 是一个与store实例具有相同属性和方法的对象

