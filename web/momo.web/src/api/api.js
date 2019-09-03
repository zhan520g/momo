import {get,post} from './request';
//登陆, 这是一个委托.....
export const  login= (login)=>post('/api/v1.0/Secret/token',login)
//上传
export const  upload=(upload)=>get('/api/get/upload',upload)