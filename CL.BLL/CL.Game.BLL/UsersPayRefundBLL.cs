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

namespace CL.Game.BLL
{

    /// <summary>
    ///UsersPayRefund info
    /// </summary>
    public class UsersPayRefundBLL
    {
        UsersPayRefundDAL dal = new UsersPayRefundDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 更新退款申请记录
        /// </summary>
        /// <returns></returns>
        public bool ModifyPayRefund(long ReID, short iResult)
        {
            return dal.ModifyPayRefund(ReID, iResult);
        }
        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertEntity(UsersPayRefundEntity entity)
        {
            return dal.InsertEntity(entity);
        }
    }
}
