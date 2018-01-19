using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;

namespace CL.Game.DAL
{
    public class LotteriesDAL : DataRepositoryBase<LotteriesEntity>
    {
        public LotteriesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }
        /// <summary>
        /// 查询所有可用彩种
        /// </summary>
        /// <returns></returns>
        public List<LotteriesEntity> QueryLotterys()
        {
            return base.GetList(new { IsEnable = true }, "Sort DESC").ToList();
        }
        /// <summary>
        /// 插入彩种对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(LotteriesEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 是否存在该彩种编码
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public bool ExistsCode(int LotteryCode)
        {
            return base.RecordCount(new { LotteryCode = LotteryCode }) == 0 ? false : true;
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int LotteryID)
        {
            return base.RecordCount(new { LotteryID = LotteryID }) == 0 ? false : true;
        }
        /// <summary>
        /// 获得数据列表
        /// </summary>
        public List<LotteriesEntity> QueryEntitys(string strWhere)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select LotteryID,LotteryCode,LotteryName ");
            strSql.Append(" FROM CT_Lotteries ");
            if (strWhere.Trim() != "")
            {
                strSql.Append(" where " + strWhere);
            }

            SqlMapper.GridReader grid = base.QueryMultiple(strSql.ToString());
            List<LotteriesEntity> list = grid.Read<LotteriesEntity>().ToList();
            grid.Dispose();
            return list;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="LotteryID"></param>
        /// <returns></returns>
        public bool DelEntity(int LotteryID)
        {
            return base.Delete(LotteryID) > 0;
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<LotteriesEntity> QueryListByPage(string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" 1 = @Val ");
            object Paramters = new { Val = 1 };
            if (strName.Trim() != "")
            {
                Where.Append(" and LotteryName like @LotteryName ");
                Paramters = new { Val = 1, LotteryName = string.Format("%{0}%", strName) };
            }
            recordCount = base.GetIntSingle(string.Format("select count(1) from CT_Lotteries where {0}", Where.ToString()), Paramters);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), "LotteryID DESC", Paramters).ToList();
        }
    }
}
