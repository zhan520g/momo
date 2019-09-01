using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using momo.Infrastructure.Watcher;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace momo.Middleware
{
    public class PerformanceLogMiddleware
    {

        private readonly ILogger _logger;

        private readonly RequestDelegate _request;

        public PerformanceLogMiddleware(ILogger<PerformanceLogMiddleware> logger,RequestDelegate requestDelegate)
        {
            _logger = logger;
            _request = requestDelegate;
        }

        /// <summary>
        /// 注入中间件到HttpContext中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            var profiler = new StopwatchProfiler();
            profiler.Start();
            await _request(context);
            profiler.Stop();
            _logger.LogInformation("TraceId:{TraceId}, RequestMethod:{RequestMethod}, RequestPath:{RequestPath}, ElapsedMilliseconds:{ElapsedMilliseconds}, Response StatusCode: {StatusCode}",
                                    context.TraceIdentifier, context.Request.Method, context.Request.Path, profiler.ElapsedMilliseconds, context.Response.StatusCode);

        }

    }
}
