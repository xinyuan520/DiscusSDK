﻿using Microsoft.AspNetCore.Mvc.Controllers;

namespace Discus.Shared.Webapi.Registrar
{
    public abstract partial class AbstractWebApiDependencyRegistrar
    {
        /// <summary>
        /// 注册swagger组件
        /// </summary>
        protected virtual void AddSwaggerGen()
        {
            var openApiInfo = new OpenApiInfo { Title = ServiceInfo.ServiceName, Version = ServiceInfo.Version };
            Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(openApiInfo.Version, openApiInfo);

                c.CustomOperationIds(apiDesc =>
                {
                    var controllerAction = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                    return controllerAction.ControllerName + "-" + controllerAction.ActionName;
                });

                //采用bearer token认证
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme."
                });
                //设置全局认证
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
                c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{ServiceInfo.StartAssembly.GetName().Name}.xml"));
            });
        }
    }
}
