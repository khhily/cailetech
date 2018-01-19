using CL.Dapper.Repository;
using CL.Game.Entity;
using System.Text;
using CL.Enum.Common;
using System.Data;
using Dapper;
using CL.Tools.Common;

namespace CL.Game.DAL
{
    public class UsersExtendDAL : DataRepositoryBase<UsersExtendEntity>
    {
        public UsersExtendDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 根据用户编号查询用户衍生对象
        /// </summary>
        /// <param name="UserCode">用户编号</param>
        /// <returns></returns>
        public UsersExtendEntity QueryEntityByUserCode(long UserCode)
        {
            return base.Get(new { UserID = UserCode }, "UserID desc");
        }


        /// <summary>
        /// 集成登陆查询
        /// </summary>
        /// <param name="IntegrateType">集成登录类型</param>
        /// <param name="IntegrateOpenID">集成登陆Token</param>
        /// <returns></returns>
        public UsersExtendEntity QueryEntityByUserCode(short IntegrateType, string IntegrateOpenID)
        {
            switch (IntegrateType)
            {
                case 1:
                    return base.Get(new { QQID = IntegrateOpenID }, "UserID desc");
                case 2:
                    return base.Get(new { WechatID = IntegrateOpenID }, "UserID desc");
                case 3:
                    return base.Get(new { AliPayID = IntegrateOpenID }, "UserID desc");
                default:
                    return base.Get(new { QQID = IntegrateOpenID }, "UserID desc");
            }
        }
        
        /// <summary>
        /// 修改对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int ModifyEntity(UsersExtendEntity entity)
        {
            return base.Update(entity);
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public bool InsertInfo(UsersExtendEntity entity)
        {
            #region 废弃代码
            //using (var tran = base.db.BeginTransaction())
            //{
            //    try
            //    {
            //        StringBuilder strSQL = new StringBuilder();
            //        strSQL.Append("INSERT INTO dbo.CT_UsersExtend ");
            //        strSQL.Append("(UserID ,NickName ,UserLevel ,SpecialLevel ,AvatarAddress ,Email ,BindType ,CreateTime ,BindTime ,SourceType ,IsBindTel,WechatToken,QQToken,AliPayToken,WechatID,QQID,AliPayID) ");
            //        strSQL.Append("VALUES ");
            //        strSQL.Append("(@UserID ,@NickName ,@UserLevel ,@SpecialLevel ,@AvatarAddress ,@Email ,@BindType ,@CreateTime ,@BindTime ,@SourceType ,@IsBindTel,@WechatToken,@QQToken,@AliPayToken,@WechatID,@QQID,@AliPayID) ");

            //        var Paramters = new DynamicParameters();
            //        Paramters.Add("@UserID", entity.UserID);
            //        Paramters.Add("@NickName", entity.NickName);
            //        Paramters.Add("@UserLevel", entity.UserLevel);
            //        Paramters.Add("@SpecialLevel", entity.SpecialLevel);
            //        Paramters.Add("@AvatarAddress", entity.AvatarAddress);
            //        Paramters.Add("@Email", entity.Email);
            //        Paramters.Add("@BindType", entity.BindType);
            //        Paramters.Add("@CreateTime", entity.CreateTime);
            //        Paramters.Add("@BindTime", entity.BindTime);
            //        Paramters.Add("@SourceType", entity.SourceType);
            //        Paramters.Add("@IsBindTel", entity.IsBindTel);
            //        Paramters.Add("@WechatToken", entity.WechatToken);
            //        Paramters.Add("@QQToken", entity.QQToken);
            //        Paramters.Add("@AliPayToken", entity.AliPayToken);
            //        Paramters.Add("@WechatID", entity.WechatID);
            //        Paramters.Add("@QQID", entity.QQID);
            //        Paramters.Add("@AliPayID", entity.AliPayID);
            //        base.ExecuteSql(strSQL.ToString(), Paramters, tran);
            //        tran.Commit();
            //        return true;
            //    }
            //    catch
            //    {
            //        tran.Rollback();
            //        return false;
            //    }
            //}
            #endregion

            var Parms = new DynamicParameters();
            Parms.Add("@UsersExtendTable", XmlHelper.Serializer(entity.GetType(), entity).Replace("false", "0").Replace("true", "1"));
            Parms.Add("@Rec", null, DbType.Int32, ParameterDirection.Output, 4);
            base.Execute("udp_UsersExtend", Parms);
            int iResult = Parms.Get<int>("@Rec");
            return iResult == 0 ? true : false;
        }
    }
}
