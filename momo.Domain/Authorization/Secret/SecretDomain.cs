using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using momo.Entity.Premission;
using momo.Infrastructure.Dapper;

namespace momo.Domain.Authorization.Secret
{
    public class SecretDomain : ISecretDomain
    {
        #region Initialize

        /// <summary>
        /// 仓储接口
        /// </summary>
        private readonly IDataRepository _repository;

       
        // <summary>
        // ctor
        // </summary>
        // <param name = "repository" ></ param >
        public SecretDomain(IDataRepository repository)
        {
            _repository = repository;
        }

        #endregion

        #region API Implements

        /// <summary>
        /// 根据帐户名、密码获取用户实体信息
        /// </summary>
        /// <param name="account">账户名</param>
        /// <param name="password">密码</param>
        /// <returns></returns>
        public async Task<IdentityUser> GetUserForLoginAsync(string account, string password)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(_repository.GetCommandSQL("Secret_GetUserByLoginAsync"));
            string sql = strSql.ToString();

            return await DBManager.MsSQL.ExecuteAsync<IdentityUser>(sql, new
            {
                account,
                password
            });
        }

        #endregion
    }
}

