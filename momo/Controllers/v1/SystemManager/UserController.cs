using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using momo.Application.Authorization.Jwt.Dto;
using momo.Application.Authorization.Secret.Dto;
using momo.Application.SystemManager.UserManager;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace momo.Controllers.v1.SystemManager
{
    public class UserController : BaseAPIController
    {
        private readonly IUserAppService _userAppService;
        // GET: api/<controller>
        public UserController(IUserAppService userAppService)
        {
            _userAppService = userAppService;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var userList = await _userAppService.GetAllUser();

            return Ok(new JwtResponseDto
            {
                err_code = 0, //0表示正常代码
                Data=userList,
                Access = "获取数据成功", //访问结果
                Type = "Bearer",
            });
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public async Task<ActionResult> Post(UserDto user)
        {
            var result = await _userAppService.AddUser(user);
            return Ok(new JwtResponseDto
            {
                Access = result == 1 ? "添加成功" : "添加失败",
                err_code = result,
                Type="Bearer"
            });
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(UserDto user)
        {
            
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
