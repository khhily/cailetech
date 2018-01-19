using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.Game.DAL
{
    public class IsuseBonusesDAL : DataRepositoryBase<IsuseBonusesEntity>
    {
        public IsuseBonusesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        /// <summary>
        /// 根据期号序列号查询对象集
        /// </summary>
        /// <param name="IsuseID"></param>
        /// <returns></returns>
        public List<IsuseBonusesEntity> QueryEntitysByIsuseID(long IsuseCode)
        {
            return base.GetList(new { IsuseID = IsuseCode }, "ID asc").ToList();
        }
       
        /// <summary>
        /// 增加奖等信息
        /// </summary>
        public int InsertEntitys(int AdminID, int LotteryCode, string IsuseName, long WinRollover, string BettingPrompt, long TotalSales, string AllValues, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@AdminID", AdminID, DbType.Int32, null, 4);
            para.Add("@LotteryCode", LotteryCode, DbType.Int32, null, 4);
            para.Add("@IsuseName", IsuseName, DbType.String, null, 32);
            para.Add("@WinRollover", WinRollover, DbType.String, null, 50);
            para.Add("@BettingPrompt", BettingPrompt, DbType.String, null, 200);
            para.Add("@TotalSales", TotalSales, DbType.Int64, null, 8);
            para.Add("@AllValues", AllValues, DbType.String, null, 1024);
            para.Add("@ReturnValue", null, DbType.Int32, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            base.Execute("udp_AddAwardInfo", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            int iResult = para.Get<int>("@ReturnValue");
            return iResult;
        }

    }
}
