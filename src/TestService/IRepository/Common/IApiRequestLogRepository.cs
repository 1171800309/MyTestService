namespace IRepository.Common
{
    public interface IApiRequestLogRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// 添加实体
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        int Add(TEntity model);
    }
}
