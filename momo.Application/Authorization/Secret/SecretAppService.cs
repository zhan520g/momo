using momo.Application.Authorization.Secret.Dto;
using momo.Domain.Authorization.Secret;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace momo.Application.Authorization.Secret
{

    public class SecretAppService : ISecretAppService
    {
        #region Initialize

        /// <summary>
        /// 领域接口
        /// </summary>
        private readonly ISecretDomain _secret;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="secret"></param>
        public SecretAppService(ISecretDomain secret)
        {
            _secret = secret;
        }

        #endregion

        #region API Implements

        /// <summary>
        /// 获取登录用户信息
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<UserDto> GetCurrentUserAsync(string account, string password)
        {
            var user = await _secret.GetUserForLoginAsync(account, password);

            //Todo：AutoMapper 做实体转换
            var userDto = new UserDto()
            {
                UserName = user.Name,
                Id = user.Id
            };
            return userDto;
        }

        #endregion
    }
}
