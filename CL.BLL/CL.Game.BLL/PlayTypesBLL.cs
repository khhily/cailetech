//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由T4模板自动生成
//     2017-04-26 11:36:00 Created by LEON
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using CL.View.Entity.Game;
using System.Collections.Generic;

namespace CL.Game.BLL
{

    /// <summary>
    ///PlayTypes info
    /// </summary>
    public class PlayTypesBLL
    {
        PlayTypesDAL dal = new PlayTypesDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 是否存在该玩法编码
        /// </summary>
        /// <param name="PlayCode"></param>
        /// <returns></returns>
        public bool ExistsCode(int PlayCode)
        {
            return dal.ExistsCode(PlayCode);
        }
        /// <summary>
        /// 根据彩种编号查询彩种玩法信息
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<PlayTypesEntity> QueryEntitysByLotteryCode(int LotteryCode)
        {
            return dal.QueryEntitysByLotteryCode(LotteryCode);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(int PlayID)
        {
            return dal.Exists(PlayID);
        }
        /// <summary>
        /// 插入彩种玩法对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int InsertEntity(PlayTypesEntity entity)
        {
            return dal.InsertEntity(entity);
        }
        
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool ModifyEntity(PlayTypesEntity entity)
        {
            return dal.ModifyEntity(entity) > 0;
        }
        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PlayTypesEntity QueryEntity(int id)
        {
            return dal.QueryEntity(id);
        }
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="PlayID"></param>
        /// <returns></returns>
        public bool DelEntity(int PlayID)
        {
            return dal.DelEntity(PlayID);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        public List<udv_PlayTypes> QueryListByPage(int LotteryCode, string strName, string orderby, int pageSize, int pageIndex, ref int recordCount)
        {
            return dal.QueryListByPage(LotteryCode, strName, orderby, pageSize, pageIndex, ref recordCount);
        }
    }
}