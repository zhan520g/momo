# momo.web

## Project setup
```
npm install
```

### Compiles and hot-reloads for development
```
npm run serve
```

### Compiles and minifies for production
```
npm run build
```

### Run your tests
```
npm run test
```

### Lints and fixes files
```
npm run lint
```

### Customize configuration
See [Configuration Reference](https://cli.vuejs.org/config/).


Vue.js 的核心是一个允许采用简洁的模板语法来声明式地将数据渲染进 DOM 的系统：

|-- momo.web 
  |-- .gitignore            //git项目忽视文件
  |-- babel.config.js       //babel 配置文件
  |-- package-lock.json     //记录安装包的具体版本号
  |-- package.json          //包的类型
  |-- README.md 
  |-- public                //项目打包后的目录
  |   |-- favicon.ico       //浏览器tag图标
  |   |-- index.html        //SPA的挂载页面 —— Index.html
  |-- src                   //项目开发目录
      |-- App.vue           //App入口文件---App.vue —— 页面所有路由对应组件的容器
      |-- main.js           //主配置文件--- main.js —— 入口文件，初始化vue实例并使用需要的插件
      |-- router.js         //vue-router文件--- router.js —— 路由文件，配置着 url 路径 和 页面的关系
      |-- store.js          //vuex 状态管理 ----Vuex store配置文件
      |-- assets //静态文件
         |-- logo.png
      |-- components        //组件存放目录
        |-- HelloWorld.vue
      |-- views             //视图目录
        |-- About.vue
        |-- Home.vue


指令说明:
v-bind 可以用来在标签上绑定标签的属性（例如：img 的 src、title ）和样式
v-bind:href  缩写 :href   比如:   <el-option v-for="item in opentions" :label="item.label" :value="item.value" :key="item.key"></el-option>
v-on:click  缩写 @click    vue 内置的 v-on 指令来替我们完成事件的绑定。
具名插槽 slot="footer"  占位符,替子组件
{
    插槽，也就是slot，是组件的一块HTML模板，这块模板显示不显示、以及怎样显示由父组件来决定。 实际上，一个slot最核心的两个问题这里就点出来了，是显示不显示和怎样显示。但是插槽显示的位置确由子组件自身决定，slot写在组件template的哪块，父组件传过来的模板将来就显示在哪块。
}


vue.config.js  配置四个东西，第一个就是目录别名alias (@api就是在这里配置)，另一个是项目启动时自动打开浏览器，最后一个就是处理引入的全局scss文件 . webpack打包 有关的配置 


三、vue-i18n 数据渲染的模板语法
我们知道 vue 中对于文字数据的渲染，有以‘’{{}}‘’或者 v-text、v-html等的形式，同样的使用国际化后，依旧可以沿用，但需要一点修改。 （不国际化，不用进行修改。)

v-text:
<span v-text="$t('m.music')"></span>

{{}}:
<span>{{$t('m.music')}}</span>


四、$ 在vue中的作用
1. 除了数据属性，Vue 实例还暴露了一些有用的实例属性与方法。它们都有前缀 $，以便与用户定义的属性区分开来。在vue文档的API中还可以看到完整的实力属性和方法。

2. <input type="text" ref="input1"/>
一般来讲，获取DOM元素，需document.querySelector（".input1"）获取这个dom节点，然后在获取input1的值。
但是用ref绑定之后，我们就不需要在获取dom节点了，直接在上面的input上绑定input1，然后$refs里面调用就行。
然后在javascript里面这样调用：this.$refs.input1这样就可以减少获取dom节点的消耗了
<script>
new Vue({
  el: "#app",
  methods:{
  add:function(){
    this.$refs.input1.value ="test"; //this.$refs.input1 减少获取dom节点的消耗
    }
  }
})
</script>

通过 this.$route.query或者 this.$route.params接收router-link传的参数
$route为当前router跳转对象里面可以获取name、path、query、params等
$router为VueRouter实例，想要导航到不同URL，则使用$router.push方法
返回上一个history也是使用$router.go方法