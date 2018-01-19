using CL.Dapper.Repository;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;
using Dapper;
using CL.Tools.Common;

/// <summary>
/// 胆拖玩法加奖详细规则
/// </summary>
namespace CL.Game.DAL
{
    public class RegularDanTuoDAL : DataRepositoryBase<RegularDanTuoEntity>
    {
        public RegularDanTuoDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        public List<RegularDanTuoEntity> QueryEntitys(int RegularID)
        {
            return base.GetList(new { RegularID = RegularID }, "RDanTuoID DESC").ToList();
        }
        /// <summary>
        /// 查询加奖信息(胆拖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityDanTuo> QueryRegularNormAward(int LotteryCode)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@LotteryCode", LotteryCode, DbType.Int32);
            return base.db.Query<udv_IsAwardActivityDanTuo>("udp_QueryIsAwardActivityDanTuo", Parms, null, true, null, CommandType.StoredProcedure).ToList();
        }

        /// <summary>
        /// 胆拖玩法加奖
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool DanTuoAward(List<udv_Awards> Awards)
        {
            var Parms = new DynamicParameters();
            Parms.Add("@Awards", XmlHelper.Serializer(Awards.GetType(), Awards), DbType.Xml);
            var i = base.Execute("udp_AwardActivity", Parms);
            return i > 0;
        }
    }
}
