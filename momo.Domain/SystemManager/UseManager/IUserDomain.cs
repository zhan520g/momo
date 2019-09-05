using momo.Entity.Premission;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace momo.Domain.SystemManager.UseManager
{
    public interface IUserDomain
    {
        #region APIs
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <returns></returns>
        Task<IList<IdentityUser>> GetAllUserAsync();

        Task<int> AddUser(IdentityUser user);

        Task<int> ModifyUser(IdentityUser user);

        Task<int> DeleteUser(Guid guid);
       #endregion
    }
}
