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

|-- momo.web 
  |-- .gitignore            //git项目忽视文件
  |-- babel.config.js       //babel 配置文件
  |-- package-lock.json     //记录安装包的具体版本号
  |-- package.json          //包的类型
  |-- README.md 
  |-- public                //项目打包后的目录
  |   |-- favicon.ico
  |   |-- index.html        //挂载页面
  |-- src                   //项目开发目录
      |-- App.vue           //主入口文件
      |-- main.js           //主入口文件
      |-- router.js         //vue-router文件
      |-- store.js          //vuex 状态管理
      |-- assets //静态文件
         |-- logo.png
      |-- components        //组件存放目录
        |-- HelloWorld.vue
      |-- views             //视图目录
        |-- About.vue
        |-- Home.vue


指令说明:
v-bind 可以用来在标签上绑定标签的属性（例如：img 的 src、title ）和样式
v-bind:href  缩写 :href
v-on:click  缩写 @click    vue 内置的 v-on 指令来替我们完成事件的绑定。
具名插槽 slot="footer"  占位符,替子组件
{
    插槽，也就是slot，是组件的一块HTML模板，这块模板显示不显示、以及怎样显示由父组件来决定。 实际上，一个slot最核心的两个问题这里就点出来了，是显示不显示和怎样显示。但是插槽显示的位置确由子组件自身决定，slot写在组件template的哪块，父组件传过来的模板将来就显示在哪块。
}
