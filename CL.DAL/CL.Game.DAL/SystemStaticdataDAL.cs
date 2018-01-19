using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.Game.DAL
{
    public class SystemStaticdataDAL : DataRepositoryBase<SystemStaticdataEntity>
    {
        public SystemStaticdataDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        public List<SystemStaticdataEntity> QueryEntitys(string Month, ref long RecordBuy, ref long RecordWin, ref long RecordUsers, ref long RecordRecharge, ref long RecordLargess, ref long RecordWithdraw)
        {
            var para = new DynamicParameters();
            para.Add("@Month", Month);
            para.Add("@RecordBuy", RecordBuy, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordWin", RecordWin, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordUsers", RecordUsers, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordRecharge", RecordRecharge, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordLargess", RecordLargess, DbType.Int64, ParameterDirection.Output);
            para.Add("@RecordWithdraw", RecordWithdraw, DbType.Int64, ParameterDirection.Output);
            var Entitys = base.QueryList("udp_QuerySystemStaticdata", para, CommandType.StoredProcedure).ToList();
            RecordBuy = para.Get<long>("@RecordBuy");
            RecordWin = para.Get<long>("@RecordWin");
            RecordUsers = para.Get<long>("@RecordUsers");
            RecordRecharge = para.Get<long>("@RecordRecharge");
            RecordLargess = para.Get<long>("@RecordLargess");
            RecordWithdraw = para.Get<long>("@RecordWithdraw");
            return Entitys;
        }
    }
}
