using CL.Enum.Common;
using CL.Game.BLL;
using CL.Game.BLL.View;
using CL.Game.Entity;
using CL.Tools.Common;
using CL.View.Entity.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CL.Admin.admin.dialog
{
    public partial class dialog_ShowLotteryNumber : UI.AdminPage
    {
        protected int SchemeID = 0;
        protected udv_Schemes model = new udv_Schemes();
        protected string LotteryName = string.Empty;
        protected string IsuseName = string.Empty;
        protected string SchemeNumber = string.Empty;
        protected string CreateTime = string.Empty;
        protected string LotteryNumber = string.Empty;
        protected string ArrPlay = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            SchemeID = QPRequest.GetQueryInt("id");
            if (SchemeID == 0)
            {
                JscriptMsg("传输参数不正确！", "back");
                return;
            }

            if (!Page.IsPostBack)
            {
                ChkAdminLevel("scheme_list", CaileEnums.ActionEnum.Add.ToString()); //检查权限

                model = new udv_SchemesBLL().QueryEntity(SchemeID);
                if (model != null)
                {
                    LotteryName = model.LotteryName;
                    IsuseName = model.IsuseName;
                    SchemeNumber = model.SchemeNumber;
                    CreateTime = model.CreateTime.ToString();
                    LotteryNumber = model.LotteryNumber;

                    PlayTypesBLL bll1 = new PlayTypesBLL();
                    List<PlayTypesEntity> playlist = bll1.QueryEntitysByLotteryCode(model.LotteryCode);
                    ArrPlay += "{";
                    foreach (PlayTypesEntity item in playlist)
                    {
                        ArrPlay += "\"" + item.PlayCode + "\":\"" + item.PlayName + "\",";
                    }
                    ArrPlay = ArrPlay.Substring(0, ArrPlay.Length - 1);
                    ArrPlay += "}";
                }
            }
        }
    }
}