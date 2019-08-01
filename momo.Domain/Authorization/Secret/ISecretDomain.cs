using momo.Entity.Premission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace momo.Domain.Authorization.Secret
{
  public interface ISecretDomain
    {
        #region APIs

        /// <summary>
        /// 根据帐户名、密码获取用户实体信息
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        Task<IdentityUser> GetUserForLoginAsync(string account, string password);

        #endregion
    }
}
