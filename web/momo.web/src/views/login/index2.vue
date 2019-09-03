<template>
    <div class="login-container">
        <vue-particles color="#fff" :particlesNumber='60' :moveSpeed='1.5' lineOpacity='0.5'  class="bg"></vue-particles>
        <div class="login-form">
            <el-row :gutter="20">
              <el-col :lg="6" :sm="10" class="sa">
                  <h3>{{$t('login.system')}}</h3>
                  <el-form
                     class="login-ruleForm"
                     :model="ruleForm2"
                     status-icon
                     :rules="rules2"
                     ref="ruleForm2"
                     label-width="100px"
                  >
                  <el-form-item :label="$t('login.username')" prop="username">
                      <el-input v-model="ruleForm2.username"></el-input>
                  </el-form-item>
                  <el-form-item :label="$t('login.password')" prop="password">
                      <el-input type="password" v-model="ruleForm2.password" autocomplete="off" show-password></el-input>
                  </el-form-item>
                  <el-form-item >
                      <el-button type="primary" @click="submitForm(ruleForm2)">登陆</el-button>
                      <el-button @click="resetForm('ruleForm2')" >重置</el-button>
                  </el-form-item>
                  </el-form>
              </el-col>
            </el-row>
        </div>
    </div>
</template>

<script>
import {login} from "@api"
import {message} from "@assets/js/common.js"
export default {
  name:"login",
  data(){
      var validatePass=(rule,value,callback)=>
      {
          if(value===""){
              callback(new Error("请输入密码"));
          }
          else{
            if(this.ruleForm2.checkPass!=null){
                this.$refs.ruleForm2.validatePass("checkPass");
            }
            callback();
          }
      };
      var validateName=(rule,value,callback)=>{
          if(value=== ""){
              callback(new Error("请输入用户名"));
          }
          else{
             callback();
          }
      };
      return {
          ruleForm2:{
              username:'zs',
              password:'123456'
          },
          rule2:{
              password:[{validator:validatePass,trigger:'blur'}],
              username:[{validator:validateName,trigger:'blur'}]
          }
      };
  },
  methods:{
      submitForm(formName){
          console.log(this.$refs[formName]);
          //this.$refs[formName]
          this.$refs.ruleForm2.validate((valid)=>{
            if(vaild){
                //这里模拟管理员以及用户两种权限,一般都是登陆后接口传过来.
                let roles=[]
                roles.push(this.ruleForm2.username)
                this.$store.commit("COMMIT_ROLE",role);
                this.$router.push({
                    path:"/home"
                });
           login(this.ruleForm2) 
             .then(res => {
               console.log(res)
               //提交数据到vuex
               this.$store.commit("COMMIT_TOKEN", res);
               this.$message('success',res.message)
               this.$router.push({
               path: "/"
              });
             })
             .catch(err => {
               this.$message("error", err.message);
             });
            } else{
                return false;
            }
          });
      },
      resetForm(formName)
      {
          this.$refs.ruleForm2.resetFields();
      }
  }
};
</script>
<style lang="scss" scoped>
.bg{
    position: fixed;
    z-index: -1;
    width: 100%;
    height: 100%;
} 
.login-container{
    background:#000000;
    width: 100%;
    height: 100%;
    position: fixed;
    display: table;
    .login-form{
        display: table-cell;
        vertical-align: middle;
        text-align: center;
        color:white;
        font-size: 18px;
        .aa{
            margin:auto;
            float:none;
        }
        h3{
            line-height: 60px;
            margin-left: 60px;
        }
        .accout{
            text-align:right;
        }
    }
}

</style>
