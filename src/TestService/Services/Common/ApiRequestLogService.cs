using Common.Model;
using IRepository.Common;
using IServices.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Common
{
    public class ApiRequestLogService : IApiRequestLogService
    {
        private readonly IApiRequestLogRepository<Sys_ApiRequestLog> repository;
        public ApiRequestLogService(IApiRequestLogRepository<Sys_ApiRequestLog> baseRepository)
        {
            repository = baseRepository;
        }

        public int Add(Sys_ApiRequestLog sys_Api)
        {
            return repository.Add(sys_Api);
        }
    }
}
