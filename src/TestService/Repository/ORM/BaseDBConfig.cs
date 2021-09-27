using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.ORM
{
    public class BaseDBConfig
    {
        /// <summary>
        /// 常规数据库连接字符串
        /// </summary>
        public static string ConnectionString { get; set; }

        /// <summary>
        /// 日志数据库连接字符串
        /// </summary>
        public static string LogConString { get; set; }

        /// <summary>
        /// 只读数据库连接字符串
        /// </summary>
        public static string ReadConStr { get; set; }

        /// <summary>
        /// 报表数据库连接字符串
        /// </summary>
        public static string ReportConStr { get; set; }

        /// <summary>
        /// 客户主数据库连接字符串
        /// </summary>
        public static string MainConStr { get; set; }
    }
}
