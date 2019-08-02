using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using momo.Infrastructure.Dapper;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace momo.Middleware
{
    public class DapperMiddleware
    {
        private readonly ILogger _logger;

        private readonly IDataRepository _repository;

        private readonly RequestDelegate _request;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="repository"></param>
        /// <param name="logger"></param>
        /// <param name="request"></param>
        public DapperMiddleware(IDataRepository repository, ILogger<DapperMiddleware> logger, RequestDelegate request)
        {
            _repository = repository;
            _logger = logger;
            _request = request;
        }

        /// <summary>
        /// 注入中间件到HttpContext中
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task InvokeAsync(HttpContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //加载存储xml的dll
            _repository.LoadDataXmlStore();

            sw.Stop();
            TimeSpan ts = sw.Elapsed;

            _logger.LogInformation($"加载存储 XML 文件DLL，总共用时：{ts.TotalMinutes} 秒");

            await _request(context);
        }
    }
}
