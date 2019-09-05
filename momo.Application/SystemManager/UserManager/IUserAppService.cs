using momo.Application.Authorization.Secret.Dto;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace momo.Application.SystemManager.UserManager
{
   public interface IUserAppService
    {
       Task<IList<UserDto>> GetAllUser();

        Task<int> AddUser(UserDto user);
    }
}
