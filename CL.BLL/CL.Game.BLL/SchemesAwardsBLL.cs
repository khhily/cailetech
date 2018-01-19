using CL.Game.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class SchemesAwardsBLL
    {
        SchemesAwardsDAL dal = new SchemesAwardsDAL(Enum.Common.DbConnectionEnum.CaileGame);
    }
}
