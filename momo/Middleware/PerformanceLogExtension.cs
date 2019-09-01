using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using momo.Infrastructure.Watcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace momo.Middleware
{
    public static class PerformanceLogExtension
    {
        /// <summary>
        /// 调用中间件
        /// </summary>
        /// <param name="applicationBuilder"></param>
        /// <returns></returns>
        public static IApplicationBuilder UsePerformanceLog(this IApplicationBuilder applicationBuilder)
        {
            return applicationBuilder.UseMiddleware<PerformanceLogMiddleware>() ;

        }
    }
}
