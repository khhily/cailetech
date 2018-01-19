using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;

namespace CL.Game.DAL
{
    public class WinTypesDAL : DataRepositoryBase<WinTypesEntity>
    {
        public WinTypesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int WinID)
        {
            return base.RecordCount(new { WinID = WinID }) == 0 ? false : true;
        }
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(WinTypesEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(WinTypesEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        /// <summary>
        /// 根据彩种编号查询
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<WinTypesEntity> QueryEntitysByLotteryCode(int LotteryCode)
        {
            return base.GetList(new { LotteryCode = LotteryCode }, "WinID desc").ToList();
        }
        /// <summary>
        /// 是否存在该玩法编码
        /// </summary>
        /// <param name="WinCode"></param>
        /// <returns></returns>
        public bool ExistsCode(int WinCode)
        {
            return base.RecordCount(new { WinCode = WinCode }) == 0 ? false : true;
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="WinID"></param>
        /// <returns></returns>
        public bool DelEntity(int WinID)
        {
            return base.Delete(WinID) > 0;
        }

        /// <summary>
        /// 获取奖等视图分页数据
        /// </summary>
        /// <param name="LotteryCode">彩种编号</param>
        /// <param name="strName">奖等名称(模糊搜索)</param>
        /// <param name="orderby">排序条件</param>
        /// <param name="pageSize">每页大小</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="recordCount">总条数</param>
        /// <returns></returns>
        public List<udv_WinTypes> QueryListByPage(int LotteryCode, string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            string Where = string.Format(" LotteryCode=@LotteryCode ");
            object Paramters = new { LotteryCode = LotteryCode };
            if (!string.IsNullOrEmpty(strName.Trim()))
            {
                Where += string.Format(" AND WinName like @WinName ");
                Paramters = new { LotteryCode = LotteryCode, WinName = string.Format("%{0}%", strName) };
            }
            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_WinTypes where {0}", Where), Paramters);
            List<udv_WinTypes> list = new DataRepositoryBase<udv_WinTypes>(DbConnectionEnum.CaileGame).GetListPaged(pageIndex, pageSize, Where, orderby, Paramters).ToList();
            return list;
        }
    }
}
