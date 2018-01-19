using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CL.Enum.Common;
using System.Data;
using CL.View.Entity.Game;

namespace CL.Game.DAL
{
    public class PlayTypesDAL : DataRepositoryBase<PlayTypesEntity>
    {
        public PlayTypesDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入彩种玩法对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(PlayTypesEntity entity)
        {
            return base.Insert(entity) ?? 0;
        }
        
        public int ModifyEntity(PlayTypesEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 根据彩种编号查询彩种玩法信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<PlayTypesEntity> QueryEntitysByLotteryCode(int LotteryCode)
        {
            return base.GetList(new { LotteryCode = LotteryCode }, " PlayCode asc").ToList();
        }
        /// <summary>
        /// 是否存在该玩法编码
        /// </summary>
        /// <param name="PlayCode"></param>
        /// <returns></returns>
        public bool ExistsCode(int PlayCode)
        {
            return base.RecordCount(new { PlayCode = PlayCode }) == 0 ? false : true;
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PlayID)
        {
            return base.RecordCount(new { PlayID = PlayID }) == 0 ? false : true;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlayTypesEntity QueryEntity(int id)
        {
            return base.Get(new { PlayID = id }, "PlayID desc");
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="PlayID"></param>
        /// <returns></returns>
        public bool DelEntity(int PlayID)
        {
            return base.Delete(PlayID) > 0;
        }

        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_PlayTypes> QueryListByPage(int LotteryCode, string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" LotteryCode=@LotteryCode ");
            object Paramters = new { LotteryCode = LotteryCode };
            if (!string.IsNullOrEmpty(strName.Trim()))
            {
                Where.Append(" AND PlayName like @PlayName ");
                Paramters = new { LotteryCode = LotteryCode, PlayName = string.Format("%{0}%", strName) };
            }
            recordCount = base.GetIntSingle(string.Format("select count(1) from udv_PlayTypes where {0}", Where.ToString()), Paramters);

            return new DataRepositoryBase<udv_PlayTypes>(DbConnectionEnum.CaileGame).GetListPaged(pageIndex, pageSize, Where.ToString(), orderby, Paramters).ToList();
        }
    }
}
