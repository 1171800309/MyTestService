using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Hosting;

namespace TestService.Filter
{
    public class GlobalExceptionsFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _env; 

        public GlobalExceptionsFilter(IHostEnvironment env )
        {
            _env = env; 
        }

        public void OnException(ExceptionContext context)
        {
            var json = new JsonErrorResponse();
            json.Message = context.Exception.Message;//错误信息
            if (_env.IsDevelopment())
            {
                json.DevelopmentMessage = context.Exception.StackTrace;//堆栈信息
            }
            context.Result = new InternalServerErrorObjectResult(json);
             

        }

        public class InternalServerErrorObjectResult : ObjectResult
        {
            public InternalServerErrorObjectResult(object value) : base(value)
            {
                StatusCode = StatusCodes.Status500InternalServerError;
            }
        }
        //返回错误信息
        public class JsonErrorResponse
        {
            /// <summary>
            /// 生产环境的消息
            /// </summary>
            public string Message { get; set; }
            /// <summary>
            /// 开发环境的消息
            /// </summary>
            public string DevelopmentMessage { get; set; }
        }

    }
}
