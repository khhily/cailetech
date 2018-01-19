using CL.Dapper.Repository;
using CL.Game.Entity;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.DAL
{
    public class UsersBanksDAL : DataRepositoryBase<UsersBanksEntity>
    {
        public UsersBanksDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }

        /// <summary>
        /// 插入对象
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public long InertEntity(UsersBanksEntity entity)
        {
            return base.Insert_Long(entity) ?? 0;
        }
        
        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="BankID"></param>
        /// <returns></returns>
        public bool DelEntity(long BankID)
        {
            return base.Delete(BankID) > 0;
        }
        /// <summary>
        /// 查询对象
        /// </summary>
        /// <param name="UserCode"></param>
        /// <param name="CardNum"></param>
        /// <returns></returns>
        public UsersBanksEntity QueryEntityByBankNum(long UserCode, string CardNum)
        {
            return base.Get(new { UserID = UserCode, CardNumber = CardNum }, "BankID DESC");
        }
        /// <summary>
        /// 查询对象集
        /// </summary>
        /// <param name="UserCode"></param>
        /// <returns></returns>
        public List<UsersBanksEntity> QueryEntitys(long UserCode)
        {
            return base.GetList(new { UserID = UserCode }, "BankID DESC").ToList();
        }
    }
}
