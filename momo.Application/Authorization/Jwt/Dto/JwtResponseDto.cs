using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Application.Authorization.Jwt.Dto
{
    public class JwtResponseDto
    {

        public int err_code { get; set; }
        /// <summary>
        /// 访问 Token 值
        /// </summary>
        public string Access { get; set; }

        /// <summary>
        /// 授权类型
        /// </summary>
        public string Type { get; set; } = "Bearer";

        /// <summary>
        /// 个人信息
        /// </summary>
        public Profile Profile { get; set; }


    }

    /// <summary>
    /// 个人信息
    /// </summary>
    public class Profile
    {
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 授权时间戳
        /// </summary>
        public long Auths { get; set; }

        /// <summary>
        /// 过期时间戳
        /// </summary>
        public long Expires { get; set; }
    }
}
