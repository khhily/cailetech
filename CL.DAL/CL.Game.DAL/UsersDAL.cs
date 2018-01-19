using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System.Text;
using System.Collections.Generic;
using Dapper;
using System.Linq;

namespace CL.Game.DAL
{
    public class UsersDAL : DataRepositoryBase<UsersEntity>
    {
        public UsersDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {

        }

        /// <summary>
        /// 根据用户编号查询用户对象
        /// </summary>
        /// <param name="UserCode">用户编号</param>
        /// <returns></returns>
        public UsersEntity QueryEntityByUserCode(long UserCode)
        {
            return base.Get(UserCode);
        }

        /// <summary>
        /// 根据手机号码查询用户对象
        /// </summary>
        /// <param name="UserCode">用户编号</param>
        /// <returns></returns>
        public UsersEntity QueryEntityByUserMobile(string Mobile)
        {
            return base.Get(new { UserMobile = Mobile }, "UserMobile desc");
        }

        /// <summary>
        /// 插入用户对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InsertEntity(UsersEntity entity)
        {
            return base.Insert_Long(entity) ?? 0;
        }

        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(UsersEntity entity)
        {
            return base.Update(entity);
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(string UserName)
        {
            return base.RecordCount(new { UserName = UserName }) == 0 ? false : true;
        }
        /// <summary>
        /// 是否存在该记录
        /// </summary>
        public bool Exists(long UserID)
        {
            return base.RecordCount(new { UserID = UserID }) == 0 ? false : true;
        }

        /// <summary>
        /// 获取用户数据列表
        /// </summary>
        public List<udv_UserInfo> QueryListByPage(string UserName, string StartTime, string EndTime, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder strWhere = new StringBuilder();
            if (UserName.Length > 0)
                strWhere.Append(" and a.UserName like '%" + UserName + "%' or a.UserMobile like '%" + UserName + "%' or b.NickName like '%" + UserName + "%' ");
            if (StartTime.Length > 0)
                strWhere.Append(" AND b.CreateTime >= '" + StartTime + "' ");
            if (EndTime.Length > 0)
                strWhere.Append(" AND b.CreateTime <= '" + EndTime + "' ");

            string strSql = @" SELECT   a.UserID, a.UserName, a.UserMobile, a.UserPassword, a.PayPassword, a.Balance, a.GoldBean, a.Freeze,
                a.IsRobot, a.IsCanLogin, 
                b.BindType, b.CreateTime, b.BindTime, b.RelationID, 
                b.IsVerify, b.IsBindTel, b.Idols, 
                b.IDNumber, b.FullName, b.AvatarAddress, b.NickName
                FROM      dbo.CT_Users AS a LEFT OUTER JOIN
                dbo.CT_UsersExtend AS b ON a.UserID = b.UserID
                WHERE a.UserID>0 {0}";

            strSql = string.Format(strSql, strWhere);
            recordCount = base.GetIntSingle(PagingHelper.CreateCountingSql(strSql));
            SqlMapper.GridReader grid = base.QueryMultiple(PagingHelper.CreatePagingSql(recordCount, pageSize, pageIndex, strSql, " a.UserID desc "));
            List<udv_UserInfo> list = grid.Read<udv_UserInfo>().ToList();
            grid.Dispose();
            return list;
        }

        /// <summary>
        ///通过身份证号码获取注册会员标识
        /// </summary>
        public string GetUserID(string idCards)
        {

            StringBuilder strWhere = new StringBuilder();
            string strSql = @"SELECT a.UserID FROM  dbo.CT_Users a JOIN dbo.CT_UsersExtend b ON b.UserID = a.UserID  {0}";
            if (idCards.Length > 0)
            {
                strWhere.Append(" WHERE b.IDNumber ='" + idCards + "'");
            }
            strSql = string.Format(strSql, strWhere);
            SqlMapper.GridReader grid = base.QueryMultiple(strSql);
            List<udv_UserInfo> list = grid.Read<udv_UserInfo>().ToList();
            grid.Dispose();
            if (list != null && list.Count > 0)
            {
                return list.FirstOrDefault().UserID.ToString();
            }
            else
            {
                return "";
            }

        }



        /// <summary>
        /// 后台手动充值
        /// </summary>
        /// <returns></returns>
        public long AddUserBalanceManual(int AdminID, string UserName, long Money, string Memo, ref string ReturnDescription)
        {
            var para = new DynamicParameters();
            para.Add("@AdminID", AdminID, DbType.Int32, null, 4);
            para.Add("@UserName", UserName, DbType.String, null, 32);
            para.Add("@OrderNo", Utils.GetRamCode(), DbType.String, null, 32);
            para.Add("@Money", Money, DbType.Int64, null, 8);
            para.Add("@Memo", Memo, DbType.String, null, 1024);
            para.Add("@ReturnValue", null, DbType.Int64, ParameterDirection.Output, 4);
            para.Add("@ReturnDescription", null, DbType.String, ParameterDirection.Output, 100);

            base.Execute("udp_BackstagePay", para);
            ReturnDescription = para.Get<string>("@ReturnDescription");
            long iResult = para.Get<long>("@ReturnValue");
            return iResult;
        }
        /// <summary>
        /// 设置静态数据
        /// </summary>
        /// <param name="DateDay">当天日期</param>
        /// <param name="StaticdataType">静态数据类型 1今日充值总额 2今日线上充值 3今日线下充值 4今日提现总额 5今日新增会员 6今日赠送金额 7彩种投注 8彩种中奖</param>
        /// <param name="Amount">静态数据金额</param>
        /// <param name="UsersCount">新增用户</param>
        /// <param name="LotteryCode">彩种</param>
        /// <returns></returns>
        public bool SetSystemStaticdata(string DateDay, int StaticdataType, long Amount, int UsersCount, int LotteryCode)
        {
            var para = new DynamicParameters();
            para.Add("@DateDay", DateDay);
            para.Add("@StaticdataType", StaticdataType);
            para.Add("@Amount", Amount);
            para.Add("@Users", UsersCount);
            para.Add("@LotteryCode", LotteryCode);
            return base.Execute("udp_SetSystemStaticdata", para) > 0;
        }

        /// <summary>
        /// 彩豆兑换
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="Amount"></param>
        /// <param name="Bean"></param>
        /// <returns></returns>
        public int ExchangeCaileBean(long UserCode, long Amount, long Bean)
        {
            var para = new DynamicParameters();
            para.Add("@UserCode", UserCode);
            para.Add("@Amount", Amount);
            para.Add("@Bean", Bean);
            para.Add("@RecordValue", 0, DbType.Int32, ParameterDirection.Output);
            base.Execute("udp_ExchangeCaileBean", para);
            return para.Get<int>("@RecordValue");
        }
    }
}
