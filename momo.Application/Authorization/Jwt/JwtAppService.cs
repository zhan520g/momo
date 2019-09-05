using momo.Application.Authorization.Jwt.Dto;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using momo.Application.Authorization.Secret.Dto;
using System.IdentityModel.Tokens.Jwt;

namespace momo.Application.Authorization.Jwt
{
    /// <summary>
    /// JSON WEB TOKEN
    /// </summary>
    public class JwtAppService : IJwtAppService
    {
        #region Initialize

        /// <summary>
        /// 已授权的 Token 信息集合
        /// </summary>
        private static ISet<JwtAuthorizationDto> _tokens = new HashSet<JwtAuthorizationDto>();

        /// <summary>
        /// 分布式缓存
        /// </summary>
        private readonly IDistributedCache _cache;

        /// <summary>
        /// 配置信息
        /// </summary>
        private readonly IConfiguration _configuration;

        /// <summary>
        /// 获取 HTTP 请求上下文
        /// </summary>
        private readonly IHttpContextAccessor _httpContextAccessor;


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="cache"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="configuration"></param>
        public JwtAppService(IDistributedCache cache, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _cache = cache;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }


        #endregion


        #region API Implements

        /// <summary>
        ///  Token 创建
        /// </summary>
        /// <param name="dto">用户信息数据传输对象</param>
        /// <returns></returns>
        public JwtAuthorizationDto Create(UserDto dto)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecurityKey"]));

            DateTime authTime = DateTime.UtcNow;
            DateTime expiresAt = authTime.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]));

            //将用户信息添加到 Claim 中,制作身份证
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);

            //添加用户信息
            IEnumerable<Claim> claims = new Claim[] {
              new Claim(ClaimTypes.Name,dto.UserName),
              new Claim(ClaimTypes.Role,dto.Role.ToString()),
              new Claim(ClaimTypes.Email,dto.Email),
              new Claim(ClaimTypes.Expiration,expiresAt.ToString())
            };
            identity.AddClaims(claims);
            var authProperties = new AuthenticationProperties
            {
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(24),//24
            };

            //制作
            var principal = new ClaimsPrincipal(identity);
            //签发一个加密后的用户信息凭证，用来标识用户的身份
            _httpContextAccessor.HttpContext.SignInAsync(JwtBearerDefaults.AuthenticationScheme, principal, authProperties);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),//创建声明信息
                Issuer = _configuration["Jwt:Issuer"],//Jwt token 的签发者
                Audience = _configuration["Jwt:Audience"],//Jwt token 的接收者
                Expires = expiresAt,//过期时间
                SigningCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256)//创建 token,使用的hash算法，如：HMAC SHA256或RSA
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            //存储 Token 信息
            var jwt = new JwtAuthorizationDto
            {
                UserId = dto.Id,
                Token = tokenHandler.WriteToken(token),
                Auths = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                Expires = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                Success = true
            };
            _tokens.Add(jwt);

            return jwt;
        }


        /// <summary>
        /// token刷新
        /// </summary>
        /// <param name="token"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<JwtAuthorizationDto> RefreshAsync(string token, UserDto dto)
        {
            var jwtOld = GetExistenceToken(token);
            if (jwtOld == null)
            {
                return new JwtAuthorizationDto()
                {
                    Token = "未获取到当前 Token 信息",
                    Success = false
                };
            }

            var jwt = Create(dto);

            //停用修改前的 Token 信息
            await DeactivateCurrentAsync();

            return jwt;
        }


        /// <summary>
        /// 判断是否存在当前 Token
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private JwtAuthorizationDto GetExistenceToken(string token)
        => _tokens.SingleOrDefault(x => x.Token == token);

        /// <summary>
        /// Token停用 
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        public async Task DeactivateAsync(string token)
        => await _cache.SetStringAsync(GetKey(token),
                " ", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow =
                        TimeSpan.FromMinutes(Convert.ToDouble(_configuration["Jwt:ExpireMinutes"]))
                });

        /// <summary>
        /// 停用当前 Token
        /// </summary>
        /// <returns></returns>
        public async Task DeactivateCurrentAsync()
        => await DeactivateAsync(GetCurrentAsync());

        /// <summary>
        /// 设置缓存中过期 Token 值的 key
        /// </summary>
        /// <param name="token">Token</param>
        /// <returns></returns>
        private static string GetKey(string token)
            => $"deactivated token:{token}";

        /// <summary>
        /// 获取 HTTP 请求的 Token 值
        /// </summary>
        /// <returns></returns>
        private string GetCurrentAsync()
        {
            //http header
            var authorizationHeader = _httpContextAccessor
                .HttpContext.Request.Headers["authorization"];

            //token
            return authorizationHeader == StringValues.Empty
                ? string.Empty
                : authorizationHeader.Single().Split(" ").Last();// bearer tokenvalue
        }

        /// <summary>
        /// token是否有效
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> IsActiveAsync(string token)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 当前token是否有效
        /// </summary>
        /// <returns></returns>
        public Task<bool> IsCurrentActiveTokenAsync()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
