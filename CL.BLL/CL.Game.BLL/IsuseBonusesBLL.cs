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
using System.Collections.Generic;

namespace CL.Game.BLL
{

    /// <summary>
    ///IsuseBonuses info
    /// </summary>
    public class IsuseBonusesBLL
    {
        IsuseBonusesDAL dal = new IsuseBonusesDAL(DbConnectionEnum.CaileGame);
        /// <summary>
        /// 增加奖等信息
        /// </summary>
        public int InsertEntitys(int AdminID, int LotteryCode, string IsuseName, long WinRollover, string BettingPrompt, long TotalSales, string AllValues, ref string ReturnDescription)
        {
            return dal.InsertEntitys(AdminID, LotteryCode, IsuseName, WinRollover, BettingPrompt, TotalSales, AllValues, ref ReturnDescription);
        }
        /// <summary>
        /// 根据期号序列号查询对象集
        /// </summary>
        /// <param name="IsuseID"></param>
        /// <returns></returns>
        public List<IsuseBonusesEntity> QueryEntitysByIsuseID(long IsuseCode)
        {
            return dal.QueryEntitysByIsuseID(IsuseCode);
        }
    }
}
