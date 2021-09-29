using Common.Helper;
using Common.Model;
using Microsoft.AspNetCore.Http;
using Repository.Common;
using Services.Common;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace TestService.Filter
{
    public class GlobalApiLoggingMiddleware : IMiddleware
    {
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            //在这里我们来拦截请求，收集日志
            Stream originalBody = context.Response.Body;
            try
            {
                // var reqlogflag = Appsettings.app(new string[] { "RequestLog" }).ToString();
                var reqlogflag = "1";
                if (reqlogflag.Equals("1"))
                {
                    using var memStream = new MemoryStream();
                    context.Response.Body = memStream;
                    await next.Invoke(context);
                    //reuqest支持buff,否则body只能读取一次
                    context.Request.EnableBuffering();
                    //这里不要释放stream,否则后续读取request.body会报错
                    var reader = new StreamReader(context.Request.Body, Encoding.UTF8);
                    var requestStr = await reader.ReadToEndAsync();
                    var request = context.Request;

                    var HttpMethod = request.Method;
                    var Request = requestStr;
                    var RequestUrl = request.Path.Value;
                    var QueryString = request.QueryString.Value;
                    var ServerIp = request.Host.Value;
                    var StatusCode = context.Response.StatusCode;
                    var ClientIp = context.Connection.RemoteIpAddress.ToString();
                    var ClientPort = context.Connection.RemotePort.ToString();
                    memStream.Position = 0;
                    var Response = await new StreamReader(memStream).ReadToEndAsync();

                    memStream.Position = 0;
                    await memStream.CopyToAsync(originalBody);
                    var log = new Sys_ApiRequestLog
                    {
                        CreateTime = DateTime.Now,
                        HttpMethod = HttpMethod,
                        Request = Request,
                        RequestUrl = RequestUrl,
                        QueryString = QueryString,
                        ServerIp = ServerIp,
                        StatusCode = StatusCode,
                        ClientIp = ClientIp,
                        ClientPort = ClientPort,
                        Response = Response
                    };
                    ApiRequestLogService apiRequestLog = new ApiRequestLogService(new ApiRequestLogRepository());
                    apiRequestLog.Add(log);
                }
            }
            finally
            {
                //重新给response.body赋值，用于返回
                context.Response.Body = originalBody;
            }
            await next.Invoke(context);
        }
    }
}
