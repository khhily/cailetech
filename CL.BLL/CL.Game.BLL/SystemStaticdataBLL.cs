using CL.Enum.Common;
using CL.Game.DAL;
using CL.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class SystemStaticdataBLL
    {
        SystemStaticdataDAL dal = new SystemStaticdataDAL(DbConnectionEnum.CaileGame);

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="Month"></param>
        /// <returns></returns>
        public List<SystemStaticdataEntity> QueryEntitys(string Month, ref long RecordBuy, ref long RecordWin, ref long RecordUsers, ref long RecordRecharge, ref long RecordLargess, ref long RecordWithdraw)
        {
            return dal.QueryEntitys(Month, ref RecordBuy, ref RecordWin, ref RecordUsers, ref RecordRecharge, ref RecordLargess, ref RecordWithdraw);
        }
    }
}
