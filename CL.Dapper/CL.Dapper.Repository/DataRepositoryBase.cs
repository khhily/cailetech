using CL.Enum.Common;
using Dapper;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace CL.Dapper.Repository
{
    public class DataRepositoryBase<T> : IRepositoryBase<T> where T : class, new()
    {
        public IDbConnection db { get; private set; }


        public DataRepositoryBase(DbConnectionEnum conenum, IDbConnection Db = null)
        {
            System.Threading.Tasks.Task.Factory.StartNew(() =>
            {
                GC.Collect();
            });
            if (Db == null)
            {
                var config = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                switch (conenum)
                {
                    case DbConnectionEnum.CaileSystem:
                        config = ConfigurationManager.ConnectionStrings["Connection_CaileSystem"];
                        break;
                    case DbConnectionEnum.CaileCoupons:
                        config = ConfigurationManager.ConnectionStrings["Connection_CaileCoupons"];
                        break;
                    case DbConnectionEnum.CaileGame:
                        config = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                        break;
                    default:
                        config = ConfigurationManager.ConnectionStrings["DefaultConnection"];
                        break;
                }
                switch (config.ProviderName)
                {
                    case "System.Data.MySqlClient":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
                        }
                        break;
                    case "System.Data.SQLite":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
                        }
                        break;
                    case "Npgsql":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
                        }
                        break;
                    default:
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
                        }
                        break;
                }

                var dbFactory = DbProviderFactories.GetFactory(config.ProviderName);
                Db = dbFactory.CreateConnection();
                if (Db != null) Db.ConnectionString = config.ConnectionString;
            }
            if (Db.State != ConnectionState.Open) Db.Open();
            this.db = Db;
        }

        public IDbConnection SqlConnection()
        {
            IDbConnection Db = null;

            if (Db == null)
            {
                var config = ConfigurationManager.ConnectionStrings["DefaultConnection"];


                switch (config.ProviderName)
                {
                    case "System.Data.MySqlClient":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
                        }
                        break;
                    case "System.Data.SQLite":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
                        }
                        break;
                    case "Npgsql":
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
                        }
                        break;
                    default:
                        {
                            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);
                        }
                        break;
                }

                var dbFactory = DbProviderFactories.GetFactory(config.ProviderName);
                Db = dbFactory.CreateConnection();
                if (Db != null) Db.ConnectionString = config.ConnectionString;
            }
            if (Db.State != ConnectionState.Open) Db.Open();
            return Db;
        }

        public int Delete(object id)
        {
            return SimpleCRUD.Delete<T>(db, id);
        }
        public int Delete(T entity)
        {
            return SimpleCRUD.Delete<T>(db, entity);
        }

        public int DeleteList(object conditions)
        {
            return SimpleCRUD.DeleteList<T>(db, conditions);
        }

        public T Get(object id)
        {
            return SimpleCRUD.Get<T>(db, id);
        }

        public IEnumerable<T> GetList(object whereConditions)
        {
            return SimpleCRUD.GetList<T>(db, whereConditions);
        }

        public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerPage, object whereConditions, string orderby)
        {
            return SimpleCRUD.GetListPaged<T>(db, pageNumber, rowsPerPage, whereConditions, orderby);
        }

        public int? Insert(T entity)
        {
            return SimpleCRUD.Insert<int>(db, entity);
        }
        public long? Insert_Long(T entity)
        {
            return SimpleCRUD.Insert<long>(db, entity);
        }

        public int RecordCount(object whereConditions)
        {
            return SimpleCRUD.RecordCount<T>(db, whereConditions);
        }

        public int Update(T entity)
        {
            return SimpleCRUD.Update(db, entity);
        }

        /// <summary>
        /// 查询数据，返回序列中的唯一元素(int)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public int GetIntSingle(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            //return SqlMapper.Query<int>(db, sql, paramters, commandType: cmdType).SingleOrDefault();
            return SqlMapper.QuerySingle<int>(db, sql, paramters, commandType: cmdType);
        }
        /// <summary>
        /// 查询数据，返回序列中的唯一元素(string)
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="paramters"></param>
        /// <param name="cmdType"></param>
        /// <returns></returns>
        public string GetStringSingle(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            //return SqlMapper.Query<string>(db, sql, paramters, commandType: cmdType).SingleOrDefault();
            return SqlMapper.QuerySingle<string>(db, sql, paramters, commandType: cmdType);
        }

        ///// <summary>
        ///// 执行多条语句，返回唯一元素
        ///// </summary>
        ///// <param name="sql"></param>
        ///// <param name="paramters"></param>
        ///// <param name="cmdType"></param>
        ///// <returns></returns>
        //public string GetSingle(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        //{
        //    return SqlMapper.Query<string>(db, sql, paramters, commandType: cmdType);
        //}

        #region 数据查询
        /// <summary>
        /// 查询数据，并返回第一个结果集
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="paramters">存储过程参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>第一个结果集</returns>
        public dynamic QueryFirstDynamic(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            return SqlMapper.QueryFirst(db, sql, paramters, commandType: cmdType);
        }


        /// <summary>
        /// 查询数据，并返回第一个对象
        /// </summary>
        /// <param name="spName">sql语句或者存储过程名称</param>
        /// <param name="paramters">sql命令参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>第一个对象</returns>
        public T QueryFirstT(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            return SqlMapper.QueryFirst<T>(db, sql, paramters, commandType: cmdType);
        }

        /// <summary>
        /// 查询数据，并返回单个结果集
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="paramters">存储过程参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>单个结果集</returns>
        public dynamic QuerySingleOrDefaultDynamic(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            return SqlMapper.QuerySingleOrDefault(db, sql, paramters, commandType: cmdType);
        }


        /// <summary>
        /// 查询数据，并返回指定的对象
        /// </summary>
        /// <param name="spName">sql语句或者存储过程名称</param>
        /// <param name="paramters">sql命令参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>单个指定的对象类型</returns>
        public T QuerySingleOrDefaultT(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            return SqlMapper.QuerySingleOrDefault<T>(db, sql, paramters, commandType: cmdType);
        }

        /// <summary>
        ///查询数据，并返回多个结果集 注意释放 GridReader, 不能并发使用 GridReader, (不建议使用)
        /// Example：
        ///              var p = new DynamicParameters();
        ///               p.Add("a", 11);
        ///               p.Add("r", dbType: DbType.Int32, direction: ParameterDirection.ReturnValue);
        ///               var grid = QueryMultiple("spDoList",p,commandType: CommandType.StoredProcedure);
        ///               var result1 = grid.Read<dynamic, dynamic, Tuple<dynamic, dynamic>>((a, b) => Tuple.Create((object)a, (object)b)).ToList();
        ///               var result2 = grid.Read<dynamic, dynamic, Tuple<dynamic, dynamic>>((a, b) => Tuple.Create((object)a, (object)b)).ToList();
        ///               int returnvalue = p.Get<int>("r");
        ///               grid.Dispose();   //注意释放
        /// </summary>
        /// <param name="spName">sql语句或者存储过程名称</param>
        /// <param name="paramters">sql命令参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>多个结果集</returns>
        public SqlMapper.GridReader QueryMultiple(string sql, object paramters = null, CommandType cmdType = CommandType.Text, IDbTransaction transaction = null)
        {
            return SqlMapper.QueryMultiple(db, sql, paramters, transaction: transaction, commandType: cmdType);
        }
        /// <summary>
        /// 查询数据，并返回结果集
        /// </summary>
        /// <param name="sql">sql语句或者存储过程名称</param>
        /// <param name="paramters">sql命令参数</param>
        /// <param name="cmdType">查询命令类型</param>
        /// <returns>结果集</returns>
        public IEnumerable<T> QueryList(string sql, object paramters = null, CommandType cmdType = CommandType.Text)
        {
            return SqlMapper.Query<T>(db, sql, paramters, commandType: cmdType).ToList();
        }

        #endregion
        #region 执行存储过程
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="parameters">存储过程参数</param>
        /// <returns>受影响的记录数</returns>
        public int Execute(string spName, object parameters = null)
        {
            return SqlMapper.Execute(db, spName, param: parameters, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql脚本</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteSql(string sql, object parameters = null)
        {
            return SqlMapper.Execute(db, sql, param: parameters, commandType: CommandType.Text);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <param name="spName">存储过程名称</param>
        /// <param name="paramters">存储过程参数</param>
        /// <returns>返回首行首列的值</returns>
        public object ExecScalar(string spName, object paramters)
        {
            return SqlMapper.ExecuteScalar(db, spName, paramters, commandType: CommandType.StoredProcedure);
        }
        /// <summary>
        /// 执行存储过程
        /// </summary>
        /// <typeparam name="T">返回的数据类型</typeparam>
        /// <param name="spName">存储过程名称</param>
        /// <param name="paramters">存储过程参数</param>
        /// <returns>返回首行首列的值</returns>
        //public T ExecScalar<T>(string spName, object paramters)
        //{
        //    return SqlMapper.ExecuteScalar<T>(db, spName, paramters, commandType: CommandType.StoredProcedure);
        //}
        #endregion

        /// <summary>
        /// 批量执行SQL语句,带事务
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public bool InsertBatch(string sql, object parameters = null)
        {
            using (IDbTransaction transaction = db.BeginTransaction())
            {
                try
                {
                    int flag = SqlMapper.Execute(db, sql, parameters, transaction, commandType: CommandType.Text);
                    transaction.Commit();
                    return flag > 0;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        #region 扩展信息 增加事物处理
        public int Delete(object id, IDbTransaction tran = null)
        {
            return SimpleCRUD.Delete<T>(db, id, tran);
        }
        public int Delete(T entity, IDbTransaction tran = null)
        {
            return SimpleCRUD.Delete<T>(db, entity, tran);
        }

        public int DeleteList(object conditions, IDbTransaction tran = null)
        {
            return SimpleCRUD.DeleteList<T>(db, conditions, tran);
        }

        public T Get(object id, IDbTransaction tran = null)
        {
            return SimpleCRUD.Get<T>(db, id, tran);
        }
        public T Get(object whereConditions, string OrderBy, IDbTransaction tran = null)
        {
            return SimpleCRUD.Get<T>(db, whereConditions, OrderBy, tran);
        }

        public IEnumerable<T> GetList(object whereConditions, string OrderBy, IDbTransaction tran = null)
        {
            return SimpleCRUD.GetList<T>(db, whereConditions, OrderBy, tran);
        }

        public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerPage, object whereConditions, string orderby, IDbTransaction tran = null)
        {
            return SimpleCRUD.GetListPaged<T>(db, pageNumber, rowsPerPage, whereConditions, orderby, tran);
        }
        public IEnumerable<T> GetListPaged(int pageNumber, int rowsPerPage, object whereConditions, string orderby, object param, IDbTransaction tran = null)
        {
            return SimpleCRUD.GetListPaged<T>(db, pageNumber, rowsPerPage, whereConditions, orderby, param, tran);
        }

        public int? Insert(T entity, IDbTransaction tran = null)
        {
            return SimpleCRUD.Insert<int>(db, entity, tran);
        }
        public long? Insert_Long(T entity, IDbTransaction tran = null)
        {
            return SimpleCRUD.Insert<long>(db, entity, tran);
        }

        public int RecordCount(object whereConditions, IDbTransaction tran = null)
        {
            return SimpleCRUD.RecordCount<T>(db, whereConditions, tran);
        }

        public int Update(T entity, IDbTransaction tran = null)
        {
            return SimpleCRUD.Update(db, entity, tran);
        }
        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql脚本</param>
        /// <param name="parameters">参数</param>
        /// <returns>受影响行数</returns>
        public int ExecuteSql(string sql, object parameters = null, IDbTransaction tran = null)
        {
            return SqlMapper.Execute(db, sql, param: parameters, transaction: tran, commandType: CommandType.Text);
        }
        #endregion

    }
}
