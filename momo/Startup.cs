using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using momo.Handles;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.IdentityModel.Tokens;
using momo.Infrastructure.Dapper;
using momo.Middleware;
using AutoMapper;

namespace momo
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            #region  授权服务
            string issuer = Configuration["Jwt:Issuer"];
            string audience = Configuration["Jwt:Audience"];
            string expire = Configuration["Jwt:ExpireMinutes"];
            TimeSpan expiration = TimeSpan.FromMinutes(Convert.ToDouble(expire));
            SecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:SecurityKey"]));
            services.AddAuthorization(options =>
            {
                //1、Definition authorization policy, 一旦标识了Permission,将挂起请求,直到认证授权通过
                options.AddPolicy("Permission",
                   policy => policy.Requirements.Add(new PolicyRequirement()));
            }).AddAuthentication(s =>
            {
                //2、Authentication
                s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(s =>
            {
                //3、Use Jwt bearer 
                s.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = key,
                    ClockSkew = expiration,
                    ValidateLifetime = true
                };
                s.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        //Token expired
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    }
                };
            });

            //DI handler process function
            services.AddSingleton<IAuthorizationHandler, PolicyHandler>();
            #endregion

            services.AddMvcCore().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonFormatters();
            services.AddSwaggerGen(s =>
            {

                //Generate api description doc
                var provider = services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();

                //swagger界面概要
                foreach (var description in provider.ApiVersionDescriptions)
                {
                    s.SwaggerDoc(description.GroupName, new Info
                    {
                        Contact = new Contact
                        {
                            Name = "zc",
                            Email = "172610800@qq.com",
                            Url = "https://yuiter.com"
                        },
                        Description = "A front-background project build by ASP.NET Core 2.2 and Vue",
                        Title = "momo",
                        Version = description.ApiVersion.ToString(),
                        License = new License
                        {
                            Name = "MIT",
                            Url = "https://mit-license.org/"
                        }
                    });
                }
                //Show the api version in url address
                s.DocInclusionPredicate((version, apiDescription) =>
                {
                    if (!version.Equals(apiDescription.GroupName))
                        return false;
                    var values = apiDescription.RelativePath
                   .Split('/')
                   .Select(v => v.Replace("v{version}", apiDescription.GroupName)); apiDescription.RelativePath = string.Join("/", values);
                    return true;

                });

                //接口说明配置
                //Add comments description
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);//get application located directory
                var apiPath = Path.Combine(basePath, "momo.xml");
                //var dtoPath = Path.Combine(basePath, "Grapefruit.Application.xml");
                s.IncludeXmlComments(apiPath, true);
                //s.IncludeXmlComments(dtoPath, true);

                //Add Jwt Authorize to http header
                //配置 Swagger 从而使 Swagger 可以支持我们的权限验证方式。
                s.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//Jwt default param name
                    In = "header",//Jwt store address
                    Type = "apiKey"//Security scheme type
                });
                //Add authentication type
                s.AddSecurityRequirement(new Dictionary<string, IEnumerable<string>>
        {
            { "Bearer", new string[] { } }
        });
            });
            services.AddApiVersioning(o =>
            {
                o.ReportApiVersions = true; //return versions in a response header
                o.DefaultApiVersion = new ApiVersion(1, 0);//default version select 
                o.AssumeDefaultVersionWhenUnspecified = true; //if not specifying an api version,show the default version
            })
            //添加api版本服务浏览
            .AddVersionedApiExplorer(option =>
            {
                option.GroupNameFormat = "'v'VVVV";//api group name
                option.AssumeDefaultVersionWhenUnspecified = true;//whether provide a service API version
            });

            //采用反射的方式，批量的将程序集内的接口与其实现类进行注入。
            string assemblies = Configuration["Assembly:FunctionAssembly"];
            if (!string.IsNullOrEmpty(assemblies))
            {
                foreach (var item in assemblies.Split('|'))
                {
                    Assembly assembly = Assembly.Load(item);
                    foreach (var implement in assembly.GetTypes())
                    {
                        Type[] interfaceType = implement.GetInterfaces();
                        foreach (var service in interfaceType)
                        {
                            services.AddTransient(service, implement);
                        }
                    }
                }
            }

            //在停用 token 的代码中，我们使用了 Redis 去保存停用的 token 信息，因此，我们需要配置我们的 Redis 连接。
            services.AddDistributedRedisCache(r =>
            {
                r.Configuration = Configuration["Redis:ConnectionString"];
            });

            services.AddAutoMapper();
            //DI Sql Data
            services.AddTransient<IDataRepository, DataRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
            //record interface call time;
            app.UsePerformanceLog();

            //Enable Authentication
            app.UseAuthentication();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();

            //Load Sql Data
            app.UseDapper();

            //使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s =>
            {
                //版本控制
                // s.SwaggerEndpoint("/swagger/v1/swagger.json", "momo API V1.0");
                foreach (var description in provider.ApiVersionDescriptions.Reverse())
                {
                    s.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                        $"momo API {description.GroupName.ToUpperInvariant()}");
                }
            });
        }
    }
}
