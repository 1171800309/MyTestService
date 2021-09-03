using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Model
{
    ///<summary>
    ///接口访问日志
    ///</summary> 
    public partial class Sys_ApiRequestLog
    {
        public Sys_ApiRequestLog()
        {


        }
        /// <summary>
        /// Desc:唯一标识
        /// Default:
        /// Nullable:False
        /// </summary>            
        public int ID { get; set; }

        /// <summary>
        /// Desc:创建时间
        /// Default:
        /// Nullable:False
        /// </summary>           
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// Desc:请求方式
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string HttpMethod { get; set; }

        /// <summary>
        /// Desc:请求body
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Request { get; set; }

        /// <summary>
        /// Desc:url
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string RequestUrl { get; set; }

        /// <summary>
        /// Desc:url参数
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string QueryString { get; set; }

        /// <summary>
        /// Desc:服务器IP
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ServerIp { get; set; }

        /// <summary>
        /// Desc:响应编码
        /// Default:
        /// Nullable:True
        /// </summary>           
        public int? StatusCode { get; set; }

        /// <summary>
        /// Desc:客户端IP
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ClientIp { get; set; }

        /// <summary>
        /// Desc:客户端请求端口
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string ClientPort { get; set; }

        /// <summary>
        /// Desc:接口响应内容
        /// Default:
        /// Nullable:True
        /// </summary>           
        public string Response { get; set; }

    }
}
