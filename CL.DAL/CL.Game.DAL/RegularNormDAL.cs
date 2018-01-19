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
using CL.View.Entity.Game;
using CL.Tools.Common;

/// <summary>
/// 标准玩法加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularNormDAL : DataRepositoryBase<RegularNormEntity>
    {
        public RegularNormDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 根据规则编号查询详细规则
        /// </summary>
        /// <param name="RegularID"></param>
        /// <returns></returns>
        public List<RegularNormEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RNormID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(标准玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityNorm> QueryRegularNormAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityNorm>("udp_QueryIsAwardActivity", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 标准玩法加奖
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool NormAward(List<udv_Awards> Awards)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@Awards", XmlHelper.Serializer(Awards.GetType(), Awards), DbType.Xml);
            var i = base.Execute("udp_AwardActivity", Parms);
            return i > 0;
        }
    }
}
