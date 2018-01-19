using Dapper;

namespace CL.Game.Entity
{
    [Table("CT_SystemStaticdata")]
    public class SystemStaticdataEntity
    {
        [Key]
        public string dateday { set; get; }
        public long recharge { set; get; }
        public long online_recharge { set; get; }
        public long offline_recharge { set; get; }
        public long withdraw { set; get; }
        public int users { set; get; }
        public int largess { set; get; }
        public long buy_jlk { set; get; }
        public long win_jlk { set; get; }
        public long buy_jxk { set; get; }
        public long win_jxk { set; get; }
        public long buy_hbsyydj { set; get; }
        public long win_hbsyydj { set; get; }
        public long buy_sdsyydj { set; get; }
        public long win_sdsyydj { set; get; }
        public long buy_cqssc { set; get; }
        public long win_cqssc { set; get; }
        public long buy_ssq { set; get; }
        public long win_ssq { set; get; }
        public long buy_dlt { set; get; }
        public long win_dlt { set; get; }
    }
}
