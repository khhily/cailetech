using CL.Dapper.Repository;
using CL.Enum.Common;
using CL.Game.Entity;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.DAL
{
    public class UsersPushDAL : DataRepositoryBase<UsersPushEntity>
    {
        public UsersPushDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 查询单个对象
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public UsersPushEntity QueryEntity(long UserCode)
        {
            return base.Get(UserCode);
        }
        /// <summary>
        /// 插入单个对象
        /// </summary>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public bool InsertEntity(UsersPushEntity Entity)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@UserId", Entity.UserId, DbType.Int64, null, 8);
            Parms.Add("@PushIdentify", Entity.PushIdentify, DbType.String, null, 100);
            Parms.Add("@Result", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_SavePushIdentify", Parms);
            int Result = Parms.Get<int>("@Result");
            return Result >= 0;
        }

        /// <summary>
        /// 推送查询
        /// </summary>
        /// <param name="UserName"></param>
        /// <returns></returns>
        public List<udv_UserPushList> QueryPushList(string UserName)
        {
            var para = new DynamicParameters();
            para.Add("@UserName", UserName);
            return new DataRepositoryBase<udv_UserPushList>(DbConnectionEnum.CaileGame).QueryList("udp_QueryPushList", para, CommandType.StoredProcedure).ToList();

        }
    }
}
