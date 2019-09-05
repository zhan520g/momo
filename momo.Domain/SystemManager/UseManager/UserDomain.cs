using momo.Entity.Premission;
using momo.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace momo.Domain.SystemManager.UseManager
{
    public class UserDomain : IUserDomain
    {
        private readonly IDataRepository _repository;


        public UserDomain(IDataRepository repository)
        {
            _repository = repository;
        }

        public async Task<int> AddUser(IdentityUser user)
        {
            var strSQL = _repository.GetCommandSQL("Sys_AddUserInfoAsync");
            return await DBManager.MsSQL.ExecuteNonQueryAsync(strSQL, user);
        }

        #region API Implements
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        public async Task<IList<IdentityUser>> GetAllUserAsync()
        {
            string strSql = _repository.GetCommandSQL("Sys_GetAllUserInfoAsync");
            return await DBManager.MsSQL.ExecuteIListAsync<IdentityUser>(strSql,null);
        }

        public Task<int> ModifyUser(IdentityUser user)
        {
            throw new NotImplementedException();
        }

        public Task<int> DeleteUser(Guid guid)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
