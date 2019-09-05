<template>
    <div class="main">
       <el-table ref="userTable" :data="tableData" @selection-change="checkboxChanged" empty-text="暂无数据,请检查系统配置"  style="width:100%">
       <el-table-column type="selection" width="55">
          </el-table-column>
           <el-table-column type="index" label="序号" width="70">
          </el-table-column>
           <el-table-column prop="userName" label="用户名">
          </el-table-column>
          <el-table-column prop="phone" label="手机号码">
          </el-table-column>
           <el-table-column prop="email" label="地址">
          </el-table-column>
        </el-table>
    </div>
</template>

<script>
import {get} from '@/api/request';

export default {
    data(){
      return{
        tableData:[]
      }
      },
       created(){
          this.init();
        },
      methods: {
       async init () {
             const {err_code, data}= await get('/api/v1.0/User');
               if(err_code===0) {
                 this.tableData = data;  
               }
            }
        }
}
</script>
<style scoped>
.main {
  margin-top: 30px;
}
</style>