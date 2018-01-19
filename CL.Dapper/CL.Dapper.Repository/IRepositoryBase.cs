using System.Collections.Generic;

namespace CL.Dapper.Repository
{
    public interface IRepositoryBase<T> where T : class, new()
    {
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns></returns>
        T Get(object id);

        /// <summary>
        /// 列表查询
        /// </summary>
        /// <param name="whereConditions">查询表达式</param>
        /// <returns></returns>
        IEnumerable<T> GetList(object whereConditions);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="pageNumber">当前页</param>
        /// <param name="rowsPerPage">页大小</param>
        /// <param name="whereConditions">查询表达式</param>
        /// <param name="orderby">排序</param>
        /// <returns></returns>
        IEnumerable<T> GetListPaged(int pageNumber, int rowsPerPage, object whereConditions, string orderby);

        /// <summary>
        /// 查询记录统计
        /// </summary>
        /// <param name="expr">查询表达式</param>
        /// <returns></returns>
        int RecordCount(object whereConditions);

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity">插入对象</param>
        /// <returns>The ID (primary key) of the newly inserted record if it is identity using the int? type, otherwise null</returns>
        int? Insert(T entity);

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="entity">更新对象</param>
        /// <returns>受影响的记录数</returns>
        int Update(T entity);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns>受影响的记录数</returns>
        int Delete(T entity);

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="id">对象ID</param>
        /// <returns>受影响的记录数</returns>
        int Delete(object id);

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="conditions">查询表达式</param>
        /// <returns>受影响的记录数</returns>
        int DeleteList(object conditions);
    }
}
