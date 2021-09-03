using Common.Model;
using IRepository.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Repository.Common
{
    public class ApiRequestLogRepository :  IApiRequestLogRepository<Sys_ApiRequestLog>
    {
        public int Add(Sys_ApiRequestLog model)
        {
            return 0;
        }
    }
}
