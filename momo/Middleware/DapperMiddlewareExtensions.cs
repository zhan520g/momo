using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace momo.Middleware
{
    public static class DapperMiddlewareExtensions
    {
        /// <summary>
        /// 调用中间件
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseDapper(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<DapperMiddleware>();
        }
    }
}
