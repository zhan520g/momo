using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
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
                        Description = "A front-background project build by ASP.NET Core 2.1 and Vue",
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

                //接口说明
                //Add comments description
                var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);//get application located directory
                var apiPath = Path.Combine(basePath, "momo.xml");
                //var dtoPath = Path.Combine(basePath, "Grapefruit.Application.xml");
                s.IncludeXmlComments(apiPath, true);
                //s.IncludeXmlComments(dtoPath, true);

               
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
            }); ;
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApiVersionDescriptionProvider provider)
        {
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

            //使用Swagger
            app.UseSwagger();
            app.UseSwaggerUI(s=>
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
