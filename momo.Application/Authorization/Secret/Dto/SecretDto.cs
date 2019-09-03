using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Application.Authorization.Secret.Dto
{
   public class SecretDto
    {
        /// <summary>
        /// 账号名
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }
        /// <summary>
        /// 登录后授权的 Token
        /// </summary>
        public string Token { get; set; }
    }
}
