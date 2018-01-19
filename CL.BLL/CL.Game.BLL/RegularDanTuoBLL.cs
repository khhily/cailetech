using CL.Game.DAL;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CL.Game.BLL
{
    public class RegularDanTuoBLL
    {
        Log log = new Log("RegularDanTuoBLL");
        RegularDanTuoDAL dal = new RegularDanTuoDAL(Enum.Common.DbConnectionEnum.CaileGame);

        public int ModifyEntity(RegularDanTuoEntity Entity)
        {
            return dal.Update(Entity);
        }
        public int InsertEntity(RegularDanTuoEntity Entity)
        {
            return dal.Insert(Entity) ?? 0;
        }
        /// <summary>
        /// 查询加奖信息(胆拖玩法)
        /// 不包含阶梯加奖或等级加奖
        /// </summary>
        /// <param name="LotteryCode"></param>
        /// <returns></returns>
        public List<udv_IsAwardActivityDanTuo> QueryRegularNormAward(int LotteryCode)
        {
            return dal.QueryRegularNormAward(LotteryCode);
        }

        /// <summary>
        /// 胆拖玩法加奖
        /// </summary>
        /// <param name="Awards"></param>
        /// <returns></returns>
        public bool DanTuoAward(List<udv_Awards> Awards)
        {
            return dal.DanTuoAward(Awards);
        }
        public List<RegularDanTuoEntity> QueryEntitys(int RegularID)
        {
            return dal.QueryEntitys(RegularID);
        }



        #region 计算加奖
        /// <summary>
        /// 加奖计算
        /// </summary>
        /// <param name="Entitys"></param>
        /// <param name="Chase_Entitys"></param>
        /// <returns></returns>
        public bool CalculateAward(List<udv_ComputeTicket> ComputeTickets, int LotteryCode)
        {
            try
            {
                //加奖内容
                List<udv_Awards> Awards = new List<udv_Awards>();
                if (ComputeTickets != null && ComputeTickets.Count > 0)
                {
                    List<udv_IsAwardActivityDanTuo> AwardActivitys = this.QueryRegularNormAward(LotteryCode);
                    if (AwardActivitys != null && AwardActivitys.Count > 0)
                    {
                        AwardActivitys.ForEach((Entity) =>
                        {
                            var Tickets = ComputeTickets.Where(w => w.PlayCode == Entity.PlayCode).ToList();
                            if (Tickets != null && Tickets.Count > 0)
                            {
                                Tickets.ForEach((Ticket) =>
                                {
                                    //查询胆码数量
                                    var SchemeEntity = new SchemesBLL().QueryEntity(Ticket.SchemeID);
                                    var LotteryNumber = string.Empty;
                                    if (SchemeEntity != null)
                                        LotteryNumber = SchemeEntity.LotteryNumber;
                                    if (!string.IsNullOrEmpty(LotteryNumber))
                                    {
                                        List<udv_SchemeBetData> SchemeBetDatas = Newtonsoft.Json.JsonConvert.DeserializeObject<List<udv_SchemeBetData>>(LotteryNumber);
                                        SchemeBetDatas.ForEach((Scheme) =>
                                        {
                                            var NumEntity = Scheme.Data.Where(w => w.Number.Contains("#")).ToList();
                                            if (NumEntity != null && NumEntity.Count > 0)
                                            {
                                                NumEntity.ForEach((item) =>
                                                {
                                                    string[] Nums = item.Number.Split('|')[0].Split('#');
                                                    if (Nums.Length == 2)
                                                    {
                                                        int DanNumbers = Nums[0].Split(',').Length;
                                                        int TuoNumbers = Nums[1].Split(',').Length;
                                                        if (DanNumbers == Entity.DanNums && TuoNumbers == Entity.TuoNums)
                                                        {
                                                            Awards.Add(new udv_Awards()
                                                            {
                                                                tid = Ticket.SchemeETicketsID,   //方案电子票
                                                                oid = Ticket.SchemeID,           //方案标识
                                                                rid = Entity.RegularID,          //规则标识
                                                                at = Entity.ActivityType,        //加奖类型
                                                                am = Entity.AwardMoney           //加奖金额
                                                            });
                                                        }
                                                    }
                                                });
                                            }
                                        });
                                    }


                                });
                            }
                        });
                        //执行加奖操作
                        this.DanTuoAward(Awards);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                log.Write(string.Format("加奖计算错误：{0}", ex.Message), true);
                return false;
            }
        }


        #endregion
    }
}
