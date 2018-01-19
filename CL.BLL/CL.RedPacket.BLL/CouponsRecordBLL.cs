using CL.Coupons.DAL;

namespace CL.Coupons.BLL
{
    public class CouponsRecordBLL
    {
        CouponsRecordDAL dal = new CouponsRecordDAL(Enum.Common.DbConnectionEnum.CaileCoupons);
    }
}
