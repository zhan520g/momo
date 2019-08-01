using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Entity.Premission
{
   public class IdentityUser : EntityBase
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 账户
        /// </summary>
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 加密参数
        /// </summary>
        public Guid Salt { get; set; }
    }
}
