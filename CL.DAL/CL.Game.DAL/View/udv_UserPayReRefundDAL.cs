using CL.Dapper.Repository;
using CL.View.Entity.Game;
using CL.Enum.Common;
using System.Data;
using System.Collections.Generic;
using System.Text;
using Dapper;
using System.Linq;

namespace CL.Game.DAL.View
{
    public class udv_UserPayReRefundDAL : DataRepositoryBase<udv_UserPayReRefund>
    {
        public udv_UserPayReRefundDAL(DbConnectionEnum conenum, IDbConnection Db = null) : base(conenum, Db)
        {
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public udv_UserPayReRefund GetPayReRefund(long ReID)
        {
            return base.Get(ReID);
        }
        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public List<udv_UserPayReRefund> QueryListByPage(string keywords, int iType, int pageSize, int pageIndex, ref int recordCount)
        {
            StringBuilder Where = new StringBuilder();
            Where.Append(" ReID > @ReID ");
            var Parms = new DynamicParameters();
            Parms.Add("@ReID", 0, DbType.Boolean, null, 1);

            if (keywords.Trim() != "")
            {
                Where.Append(" and (UserName like @keywords or UserMobile like @keywords or OrderNo like @keywords or RefundNo like @keywords ) ");
                Parms.Add("@keywords", string.Format("%{0}%", keywords), DbType.String, null, 64);
            }
            if (iType != -1)
            {
                if (iType == 2)
                    Where.Append(" and Result >= @Result ");
                else
                    Where.Append(" and Result = @Result ");
                Parms.Add("@Result", iType, DbType.Int16, null, 1);
            }

            recordCount = GetIntSingle(string.Format("select count(1) from udv_UserPayReRefund where {0}", Where.ToString()), Parms);
            return base.GetListPaged(pageIndex, pageSize, Where.ToString(), "ReID desc", Parms).ToList();
        }
    }
}
