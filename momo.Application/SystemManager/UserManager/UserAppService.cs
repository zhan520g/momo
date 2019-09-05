using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using momo.Application.Authorization.Secret.Dto;
using momo.Domain.SystemManager.UseManager;
using momo.Entity.Premission;

namespace momo.Application.SystemManager.UserManager
{
    /// <summary>
    /// 服务层需要 啥,需要领域层
    /// </summary>
    public class UserAppService : IUserAppService  //显示实现接口便于 实现多个接口时,识别相同接口名的情况
    {
        #region initialize

        public IUserDomain _userDomain;

        private readonly IMapper _mapper;



        public UserAppService(IUserDomain userDomain ,IMapper mapper)
        {
            _userDomain = userDomain;
            _mapper = mapper;
        }


        #endregion

        #region APIs
        public async Task<IList<UserDto>> GetAllUser() 
        {
            var users = await _userDomain.GetAllUserAsync();

            var list = _mapper.Map<List<UserDto>>(users);

            return list;
        }

        public  async Task<int> AddUser(UserDto user)
        {
            var userParam = _mapper.Map<IdentityUser>(user);
            userParam.Id = Guid.NewGuid();
            userParam.Salt = Guid.NewGuid();
            return await _userDomain.AddUser(userParam);
        }

        #endregion
    }
}
