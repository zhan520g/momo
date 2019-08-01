using System;
using System.Collections.Generic;
using System.Text;

namespace momo.Application.Authorization.Jwt.Dto
{
    public class JwtAuthorizationDto
    {
        public object UserId { get; internal set; }
        public string Token { get; internal set; }
        public long Auths { get; internal set; }
        public long Expires { get; internal set; }
        public bool Success { get; internal set; }
    }
}
