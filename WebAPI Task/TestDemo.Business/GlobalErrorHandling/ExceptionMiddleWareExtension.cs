using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Http;

namespace TestDemo.Business.GlobalErrorHandling
{
    public static class ExceptionMiddleWareExtension
    {

        public static void ConfigureGlobalExcpetionFilter(this IApplicationBuilder app, ILoggerManager logger)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(
                        async context =>
                        {
                            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                            context.Response.ContentType = "application/json";

                            var contextfeatureerror = context.Features.Get<IExceptionHandlerFeature>();

                            if (contextfeatureerror != null)
                            {
                                logger.LogError($"Something went wrong: {contextfeatureerror.Error}");
                                var err = $"<h1>Error: {contextfeatureerror.Error.Message}</h1>{contextfeatureerror.Error.StackTrace }";
                                await context.Response.WriteAsync(err).ConfigureAwait(false);
                            }
                        }
                );
            });
        }
    }
}
