using CL.View.Entity.Game;
using System.Collections.Generic;
using System.Linq;

namespace CL.Game.BLL.Tools
{
    public class Bet
    {

        //计算注数公式
        //
        public static int CheckData(List<udv_SchemeBetData> Model, long BetAmount, int BuyType, udv_Tasks udv, ref bool IsSplit, ref List<udv_Parameter> RetuList)
        {
            long Amount = 0;
            long Price = 0;
            RetuList = new List<udv_Parameter>();
            foreach (udv_SchemeBetData obj in Model)
            {
                if (obj.PlayCode == 90102) //追加玩法
                    Price = 300;
                else if(obj.PlayCode== 20213) //山东11选5乐选3
                    Price = 600;
                else if (obj.PlayCode == 20214) //山东11选5乐选4
                    Price = 1000;
                else if (obj.PlayCode == 20215)  //山东11选5乐选5
                    Price = 1400;
                else
                    Price = 200;
                foreach (udv_SchemeBetDataDetail objchild in obj.Data)
                {
                    if (objchild.Multiple <= 0 || objchild.Bet <= 0)
                        return 2;
                    else
                    {
                        Amount += objchild.Bet * objchild.Multiple * Price;
                    }
                    RetuList.Add(new udv_Parameter()
                    {
                        Amount = objchild.Bet * objchild.Multiple * Price,
                        Bet = objchild.Bet,
                        Multiple = objchild.Multiple,
                        Number = objchild.Number,
                        IsNorm = objchild.Number.Contains("#"),
                        PlayCode = obj.PlayCode
                    });
                }
            }
            if (BetAmount != Amount)
            {
                if (BuyType != 1)
                {
                    RetuList = new List<udv_Parameter>();
                    return 1; // 订单金额与投注数不一致
                }
                else
                {
                    if (BetAmount != udv.Data.Sum(s => s.Amount))
                    {
                        RetuList = new List<udv_Parameter>();
                        return 1; // 订单金额与投注数不一致
                    }
                }
            }
            if (Amount > 20000)
            {
                IsSplit = true;
            }
            return 0;
        }
    }
}
