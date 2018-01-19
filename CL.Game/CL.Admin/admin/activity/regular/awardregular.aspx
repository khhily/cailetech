<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="awardregular.aspx.cs" Inherits="CL.Admin.admin.activity.regular.awardregular" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>加奖规则</title>
    <link href="../../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>加奖规则</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li>
                            <label style="cursor: pointer;">加奖规则</label></li>
                    </ul>
                </div>
            </div>
            <div class="tab-content" style="min-height: 300px;">
                <dl>
                    <dt>活动类型</dt>
                    <dd>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlRegularType" runat="server">
                                <asp:ListItem Value="0">标准玩法加奖</asp:ListItem>
                                <asp:ListItem Value="1">追号加奖</asp:ListItem>
                                <asp:ListItem Value="2">胆拖玩法加奖</asp:ListItem>
                                <asp:ListItem Value="3">球队加奖</asp:ListItem>
                                <asp:ListItem Value="4">串关加奖</asp:ListItem>
                                <asp:ListItem Value="5">投注金额累计区间加奖</asp:ListItem>
                                <asp:ListItem Value="6">中奖金额区间加奖</asp:ListItem>
                                <asp:ListItem Value="7">活动期间投注金额累计名次加奖</asp:ListItem>
                                <asp:ListItem Value="8">活动期间中奖金额累计名次加奖</asp:ListItem>
                                <asp:ListItem Value="9">数字彩中球加奖</asp:ListItem>
                                <asp:ListItem Value="10">节假日加奖</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </dd>
                </dl>
                <dl>
                    <dt>活动彩种</dt>
                    <dd>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlRegularLottery" runat="server">
                                <asp:ListItem Value="101">红快3(吉林)</asp:ListItem>
                                <asp:ListItem Value="102">赢快3(江西)</asp:ListItem>
                                <asp:ListItem Value="201">华11选5(湖北)</asp:ListItem>
                                <asp:ListItem Value="202">老11选5(山东)</asp:ListItem>
                                <asp:ListItem Value="301">老时时彩(重庆)</asp:ListItem>
                                <asp:ListItem Value="801">双色球</asp:ListItem>
                                <asp:ListItem Value="901">超级大乐透</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                    </dd>
                </dl>
                <dl>
                    <dt></dt>
                    <dd style="color: red;">
                        <asp:Label ID="lbMsg" runat="server"></asp:Label>
                    </dd>
                </dl>
                <dl>
                    <dt></dt>
                    <dd>
                        <asp:LinkButton ID="lbtnRegularSubmit" runat="server" CssClass="btn" OnClick="lbtnRegularSubmit_Click" Style="color: #ffffff;">保存</asp:LinkButton>
                    </dd>
                </dl>
            </div>
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li>
                            <label id="regularadd" style="cursor: pointer;">添加详细规则</label>
                        </li>
                    </ul>
                </div>
            </div>
            <div class="tab-content" style="min-height: 724px;">
                <!--0-->
                <asp:Repeater ID="rptList_0" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>单次加奖金额</th>
                                <th>上限金额</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_0.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--1-->
                <asp:Repeater ID="rptList_1" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>中奖类型</th>
                                <th>单位(次或金额)</th>
                                <th>单次加奖金额</th>
                                <th>上限金额</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# Convert.ToInt32(Eval("RChaseType"))==1?"连续中奖次数":"累计中奖金额"%></td>
                            <td align="center"><%# (Eval("Unit"))%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_1.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--2-->
                <asp:Repeater ID="rptList_2" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>胆码数</th>
                                <th>拖码数</th>
                                <th>单次加奖金额</th>
                                <th>上限金额</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# Eval("DanNums")%></td>
                            <td align="center"><%# (Eval("TuoNums"))%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_2.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--5-->
                <asp:Repeater ID="rptList_5" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>区间规则</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# SetInterval(Eval("BetInterval"))%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_5.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--6-->
                <asp:Repeater ID="rptList_6" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>区间规则</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# SetInterval(Eval("AwardInterval"))%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_6.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--7-->
                <asp:Repeater ID="rptList_7" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>名次规则</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# SetRanking(Eval("BetRanking"))%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_7.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--8-->
                <asp:Repeater ID="rptList_8" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>名次规则</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# SetRanking(Eval("AwardRanking"))%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_8.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--9-->
                <asp:Repeater ID="rptList_9" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>选球类型</th>
                                <th>指定选号</th>
                                <th>单次加奖金额</th>
                                <th>上限金额</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# Convert.ToInt32(Eval("BallType"))==1?"指定红球数字":"指定蓝球数字"%></td>
                            <td align="center"><%# Eval("Ball")%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_9.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
                <!--10-->
                <asp:Repeater ID="rptList_10" runat="server" Visible="false">
                    <HeaderTemplate>
                        <table width="500" border="0" cellspacing="0" cellpadding="0" class="ltable">
                            <tr>
                                <th>加奖玩法</th>
                                <th>类型</th>
                                <th>单次加奖金额</th>
                                <th>单日上限金额</th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                            <td align="center"><%# SetHolidayType(Eval("HolidayType"))%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                            <td align="center"><%# Convert.ToInt64(Eval("TopLimitDay"))/100%></td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        <%#rptList_10.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                </table>
                    </FooterTemplate>
                </asp:Repeater>
            </div>
        </div>
        <!--/内容-->
        <style>
            .content-tab ul li label {
                display: block;
                float: left;
                padding: 0 25px;
                border-top: 1px solid #eee;
                border-right: 1px solid #eee;
                border-left: 1px solid #eee;
                line-height: 31px;
                font-size: 12px;
                color: #333;
                text-align: center;
                background: #fff;
                white-space: nowrap;
                word-break: break-all;
            }
        </style>
        <script type="text/javascript">
            $("#regularadd").click(function () {
                HandleScriptMothod(0);
            });
            function HandleScriptMothod(id)
            {
                var regularid = '<%=RegularID %>';
                var regulartype = $("#ddlRegularType").val();
                var regularname = $("#ddlRegularType").find("option:selected").text();;
                var contentid;
                switch (parseInt(regulartype)) {
                    case 0: //标准
                        contentid = "#Template_RegularNorm";
                        break;
                    case 1: //追号
                        contentid = "#Template_RegularChase";
                        break;
                    case 2: //胆拖
                        contentid = "#Template_RegularDanTuo";
                        break;
                    case 5: //投注区间
                        contentid = "#Template_RegularBetInterval";
                        break;
                    case 6: //中奖区间
                        contentid = "#Template_RegularAwardInterval";
                        break;
                    case 7: //投注名次
                        contentid = "#Template_RegularBetRanking";
                        break;
                    case 8: //中奖名次
                        contentid = "#Template_RegularAwardRanking";
                        break;
                    case 9: //中球
                        contentid = "#Template_RegularBall";
                        break;
                    case 10: //节假日
                        contentid = "#Template_RegularHoliday";
                        break;
                }
                var winDialog = top.dialog({
                    title: regularname,
                    content: $(contentid).html(),
                    okValue: '确定',
                    ok: function () {
                        var result = "";
                        switch (parseInt(regulartype)) {
                            case 0: //标准
                                var RNormID = $("#hidRNormID", parent.document).val();
                                var PlayCode = $("#ddlRegularNormPlayCode", parent.document).val();
                                var AwardMoney = $("#txtRegularNormAwardMoney", parent.document).val();
                                var TopLimit = $("#txtRegularNormTopLimit", parent.document).val();
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "AwardMoney": AwardMoney, "TopLimit": TopLimit, "RNormID": RNormID };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 1: //追号
                                var RChaseID = $("#hidRChaseID", parent.document).val();
                                var PlayCode = $("#ddlRegularChasePlayCode", parent.document).val();
                                var ChaseType = $("#ddlRegularChaseType", parent.document).val();
                                var ChaseUnit = $("#txtRegularChaseUnit", parent.document).val();
                                var AwardMoney = $("#txtRegularChaseAwardMoney", parent.document).val();
                                var TopLimit = $("#txtRegularChaseTopLimit", parent.document).val();
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "ChaseType": ChaseType, "Unit": ChaseUnit, "AwardMoney": AwardMoney, "TopLimit": TopLimit, "RChaseID": RChaseID };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 2: //胆拖
                                var RDanTuoID = $("#hidRDanTuoID", parent.document).val();
                                var AwardMoney = $("#txtRegularDanTuoAwardMoney", parent.document).val();
                                var TopLimit = $("#txtRegularDanTuoTopLimit", parent.document).val();
                                var DanNums = $("#txtDanNums", parent.document).val();
                                var TuoNums = $("#txtTuoNums", parent.document).val();
                                var PlayCode = $("#ddlRegularDanTuoPlayCode", parent.document).val();
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "AwardMoney": AwardMoney, "TopLimit": TopLimit, "DanNums": DanNums, "TuoNums": TuoNums, "RDanTuoID": RDanTuoID };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 5: //投注区间
                                var RBetIntervalID = $("#hidBetIntervalID", parent.document).val();
                                var PlayCode = $("#ddlRegularBetIntervalPlayCode", parent.document).val();
                                var $ul = $("#ul_BetInterval", parent.document);
                                var $li = $ul.find("li");
                                var length = $ul.find("li").length;
                                var parameter = "";
                                for (var i = 0; i < length; i++) {
                                    var pk = $li.eq(i).find(".label").text();
                                    var min = parseInt($li.eq(i).find(".min").val());
                                    var max = parseInt($li.eq(i).find(".max").val());
                                    var award = $li.eq(i).find(".award").val();
                                    if (min == '' || max == '' || $.trim(min) == 'NaN' || $.trim(max) == 'NaN') {
                                        alert("请正确填写投注金额区间值");
                                        return false;
                                    }
                                    if (min >= max) {
                                        alert("最小值必须比最大值小");
                                        return false;
                                    }
                                    if (award == null || award == '' || award == '0') {
                                        alert("请正确填写加奖金额");
                                        return false;
                                    }
                                    if (parameter != "")
                                        parameter += "|";
                                    parameter += "pk:" + pk + ",min:" + min + ",max:" + max + ",award:" + award;
                                }
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "RBetIntervalID": RBetIntervalID, "Param": parameter };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 6: //中奖区间
                                var RAwardIntervalID = $("#hidAwardIntervalID", parent.document).val();
                                var PlayCode = $("#ddlRegularAwardIntervalPlayCode", parent.document).val();
                                var $ul = $("#ul_AwardInterval", parent.document);
                                var $li = $ul.find("li");
                                var length = $ul.find("li").length;
                                var parameter = "";
                                for (var i = 0; i < length; i++) {
                                    var pk = $li.eq(i).find(".label").text();
                                    var min = parseInt($li.eq(i).find(".min").val());
                                    var max = parseInt($li.eq(i).find(".max").val());
                                    var award = $li.eq(i).find(".award").val();
                                    if (min == '' || max == '' || $.trim(min) == 'NaN' || $.trim(max) == 'NaN') {
                                        alert("请正确填写投注金额区间值");
                                        return false;
                                    }
                                    if (min >= max) {
                                        alert("最小值必须比最大值小");
                                        return false;
                                    }
                                    if (award == null || award == '' || award == '0') {
                                        alert("请正确填写加奖金额");
                                        return false;
                                    }
                                    if (parameter != "")
                                        parameter += "|";
                                    parameter += "pk:" + pk + ",min:" + min + ",max:" + max + ",award:" + award;
                                }
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "RAwardIntervalID": RAwardIntervalID, "Param": parameter };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 7: //投注名次
                                var RBetRankingID = $("#hidBetRankingID", parent.document).val();
                                var PlayCode = $("#ddlRegularBetRankingPlayCode", parent.document).val();
                                var $ul = $("#ul_BetRanking", parent.document);
                                var $li = $ul.find("li");
                                var length = $ul.find("li").length;
                                var parameter = "";
                                for (var i = 0; i < length; i++) {
                                    var placing = $li.eq(i).find(".label").text();
                                    var award = $li.eq(i).find(".award").val();
                                    if (award == null || award == '' || award == '0') {
                                        alert("请正确填写加奖金额");
                                        return false;
                                    }
                                    if (parameter != "")
                                        parameter += "|";
                                    parameter += "placing:" + placing + ",award:" + award;
                                }
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "RBetRankingID": RBetRankingID, "Param": parameter };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 8: //中奖名次
                                var RAwardRankingID = $("#hidAwardRankingID", parent.document).val();
                                var PlayCode = $("#ddlRegularAwardRankingPlayCode", parent.document).val();
                                var $ul = $("#ul_AwardRanking", parent.document);
                                var $li = $ul.find("li");
                                var length = $ul.find("li").length;
                                var parameter = "";
                                for (var i = 0; i < length; i++) {
                                    var placing = $li.eq(i).find(".label").text();
                                    var award = $li.eq(i).find(".award").val();
                                    if (award == null || award == '' || award == '0') {
                                        alert("请正确填写加奖金额");
                                        return false;
                                    }
                                    if (parameter != "")
                                        parameter += "|";
                                    parameter += "placing:" + placing + ",award:" + award;
                                }
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "RAwardRankingID": RAwardRankingID, "Param": parameter };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 9: //中球
                                var RBallID = $("#hidRBallID", parent.document).val();
                                var PlayCode = $("#ddlRegularBallPlayCode", parent.document).val();
                                var BallType = $("#ddlRegularBallType", parent.document).val();
                                var Ball = $("#txtRegularBall", parent.document).val();
                                var AwardMoney = $("#txtRegularBallAwardMoney", parent.document).val();
                                var TopLimit = $("#txtRegularBallTopLimit", parent.document).val();
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "AwardMoney": AwardMoney, "TopLimit": TopLimit, "BallType": BallType, "Ball": Ball, "RBallID": RBallID };
                                sendAjaxUrl(winDialog, postData);
                                break;
                            case 10: //节假日
                                var RHolidayID = $("#hidRHolidayID", parent.document).val();
                                var PlayCode = $("#ddlRegularHolidayPlayCode", parent.document).val();
                                var HoliadyType = $("#ddlRegularHoliadyType", parent.document).val();
                                var HoliadyAwardMoney = $("#txtRegularHoliadyAwardMoney", parent.document).val();
                                var HoliadyTopLimitDay = $("#txtRegularHoliadyTopLimitDay", parent.document).val();
                                var postData = { "RegularID": regularid, "RegularType": regulartype, "PlayCode": PlayCode, "AwardMoney": HoliadyAwardMoney, "TopLimit": HoliadyTopLimitDay, "HoliadyType": HoliadyType, "RHolidayID": RHolidayID };
                                sendAjaxUrl(winDialog, postData);
                                break;
                        }
                    },
                    cancelValue: '取消',
                    cancel: function () {

                    }
                }).showModal();
            }
            //发送AJAX请求
            function sendAjaxUrl(winObj, postData) {
                $.ajax({
                    type: "post",
                    url: "../../../tools/admin_ajax.ashx?action=regular",
                    data: postData,
                    dataType: "json",
                    error: function (XMLHttpRequest, textStatus, errorThrown) {
                        top.dialog({
                            title: '提示',
                            content: '尝试发送失败，错误信息：' + errorThrown,
                            okValue: '确定',
                            ok: function () { }
                        }).showModal(winObj);
                    },
                    success: function (data, textStatus) {
                        if (data.status == 0) {
                            winObj.close().remove();
                            var d = dialog({ content: data.msg }).show();
                            setTimeout(function () { d.close().remove(); location.reload(); }, 2000);
                        } else {
                            top.dialog({
                                title: '提示',
                                content: '错误提示：' + data.msg,
                                okValue: '确定',
                                ok: function () { }
                            }).showModal(winObj);
                        }
                    }
                });
            }
        </script>
        <script id="Template_RegularNorm" style="width: 500px; height: 260px;" type="text/template" title="标准玩法">
            <div class="content-tab-wrap" style="width: 500px; height: 220px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularNormPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidRNormID" Value="0" runat="server" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>单次加奖金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularNormAwardMoney" runat="server" CssClass="input" TextMode="Number" Text="2"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>上限金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularNormTopLimit" runat="server" CssClass="input" TextMode="Number" Text="0"></asp:TextBox>
                        </dd>
                    </dl>
                </div>
            </div>
        </script>
        <script id="Template_RegularChase" style="width: 500px; height: 320px;" type="text/template" title="追号玩法">
            <div class="content-tab-wrap" style="width: 500px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularChasePlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidRChaseID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>中奖类型</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularChaseType" runat="server">
                                    <asp:ListItem Value="1">连续中奖次数</asp:ListItem>
                                    <asp:ListItem Value="2">累计中奖金额</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单位(次或金额)</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularChaseUnit" runat="server" CssClass="input" TextMode="Number"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单次加奖金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularChaseAwardMoney" runat="server" CssClass="input" TextMode="Number" Text="2"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>上限金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularChaseTopLimit" runat="server" CssClass="input" TextMode="Number" Text="0"></asp:TextBox>
                        </dd>
                    </dl>
                </div>
            </div>
        </script>
        <script id="Template_RegularDanTuo" style="width: 500px; height: 320px;" type="text/template" title="胆拖玩法">
            <div class="content-tab-wrap" style="width: 500px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularDanTuoPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidRDanTuoID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>胆码数</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:TextBox ID="txtDanNums" runat="server" CssClass="input" TextMode="Number"></asp:TextBox>
                            </div>
                        </dd>
                    </dl>
                    <dl>
                        <dt>拖码数</dt>
                        <dd>
                            <asp:TextBox ID="txtTuoNums" runat="server" CssClass="input" TextMode="Number"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单次加奖金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularDanTuoAwardMoney" runat="server" CssClass="input" TextMode="Number" Text="2"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>上限金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularDanTuoTopLimit" runat="server" CssClass="input" TextMode="Number" Text="0"></asp:TextBox>
                        </dd>
                    </dl>
                </div>
            </div>
        </script>
        <script id="Template_RegularBetInterval" style="width: 600px; height: 320px;" type="text/template" title="投注区间加奖">
            <div class="content-tab-wrap" style="width: 600px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularBetIntervalPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidBetIntervalID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>区间规则</dt>
                        <dd>
                            <ul id="ul_BetInterval">
                                <li>序号：<label id="BetInterval_PK_1" class="label">1</label>&nbsp;&nbsp;投注金额区间：<input id="txtBetIntervalMin_1" class="input min" style="width: 45px;" type="text" />
                                    -
                                    <input id="txtBetIntervalMax_1" class="input max" style="width: 45px;" type="text" />&nbsp;&nbsp;加奖金额：<input id="txtBetIntervalAward_1" class="input award" style="width: 45px;" type="text" />&nbsp;&nbsp;<label id="BetInterval_AddLabel" style="cursor: pointer;">+</label></li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </div>
            <script type="text/javascript">
                $("#BetInterval_AddLabel").click(function () {
                    var $Obj = $("#ul_BetInterval");
                    var ul_length = parseInt($Obj.find("li").length);
                    if (ul_length >= 5) {
                        alert("最大支持五条记录");
                        return;
                    }
                    ul_length += 1;
                    var html = '<li>序号：<label id="BetInterval_PK_' + ul_length + '" class="label">' + ul_length + '</label>&nbsp;&nbsp;投注金额区间：<input id="txtBetIntervalMin_' + ul_length + '" class="input min" style="width: 45px;" type="text" /> - <input id="txtBetIntervalMax_' + ul_length + '" class="input max" style="width: 45px;" type="text" />&nbsp;&nbsp;加奖金额：<input id="txtBetIntervalAward_' + ul_length + '" class="input award" style="width: 45px;" type="text" /></li>';
                    $Obj.append(html);
                });
            </script>
        </script>
        <script id="Template_RegularAwardInterval" style="width: 600px; height: 320px;" type="text/template" title="中奖区间加奖">
            <div class="content-tab-wrap" style="width: 600px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularAwardIntervalPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidAwardIntervalID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>区间规则</dt>
                        <dd>
                            <ul id="ul_AwardInterval">
                                <li>序号：<label id="AwardInterval_PK_1" class="label">1</label>&nbsp;&nbsp;投注金额区间：<input id="txtAwardIntervalMin_1" class="input min" style="width: 45px;" type="text" />
                                    -
                                    <input id="txtAwardIntervalMax_1" class="input max" style="width: 45px;" type="text" />&nbsp;&nbsp;加奖金额：<input id="txtAwardIntervalAward_1" class="input award" style="width: 45px;" type="text" />&nbsp;&nbsp;<label id="AwardInterval_AddLabel" style="cursor: pointer;">+</label></li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </div>
            <script type="text/javascript">
                $("#AwardInterval_AddLabel").click(function () {
                    var $Obj = $("#ul_AwardInterval");
                    var ul_length = parseInt($Obj.find("li").length);
                    if (ul_length >= 5) {
                        alert("最大支持五条记录");
                        return;
                    }
                    ul_length += 1;
                    var html = '<li>序号：<label id="AwardInterval_PK_' + ul_length + '" class="label">' + ul_length + '</label>&nbsp;&nbsp;投注金额区间：<input id="txtAwardIntervalMin_' + ul_length + '" class="input min" style="width: 45px;" type="text" /> - <input id="txtAwardIntervalMax_' + ul_length + '" class="input max" style="width: 45px;" type="text" />&nbsp;&nbsp;加奖金额：<input id="txtAwardIntervalAward_' + ul_length + '" class="input award" style="width: 45px;" type="text" /></li>';
                    $Obj.append(html);
                });
            </script>
        </script>
        <script id="Template_RegularBetRanking" style="width: 600px; height: 320px;" type="text/template" title="投注排名加奖">
            <div class="content-tab-wrap" style="width: 600px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularBetRankingPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidBetRankingID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>排名规则</dt>
                        <dd>
                            <ul id="ul_BetRanking">
                                <li>第&nbsp;<label id="BetRanking_PK_1" class="label">1</label>&nbsp;名&nbsp;&nbsp;加奖金额：<input id="txtBetRankingAward_1" class="input award" style="width: 45px;" type="text" />&nbsp;&nbsp;<label id="BetRanking_AddLabel" style="cursor: pointer;">+</label></li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </div>
            <script type="text/javascript">
                $("#BetRanking_AddLabel").click(function () {
                    var $Obj = $("#ul_BetRanking");
                    var ul_length = parseInt($Obj.find("li").length);
                    if (ul_length >= 5) {
                        alert("最大支持前5名加奖");
                        return;
                    }
                    ul_length += 1;
                    var html = '<li>名&nbsp;<label id="BetRanking_PK_' + ul_length + '"  class="label">' + ul_length + '</label>&nbsp;次&nbsp;&nbsp;加奖金额：<input id="txtBetRankingAward_' + ul_length + '" class="input award" style="width: 45px;" type="text" /></li>';
                    $Obj.append(html);
                });
            </script>
        </script>
        <script id="Template_RegularAwardRanking" style="width: 600px; height: 320px;" type="text/template" title="中奖排名加奖">
            <div class="content-tab-wrap" style="width: 600px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularAwardRankingPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidAwardRankingID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>排名规则</dt>
                        <dd>
                            <ul id="ul_AwardRanking">
                                <li>第&nbsp;<label id="AwardRanking_PK_1" class="label">1</label>&nbsp;名&nbsp;&nbsp;加奖金额：<input id="txtAwardRankingAward_1" class="input award" style="width: 45px;" type="text" />&nbsp;&nbsp;<label id="AwardRanking_AddLabel" style="cursor: pointer;">+</label></li>
                            </ul>
                        </dd>
                    </dl>
                </div>
            </div>
            <script type="text/javascript">
                $("#AwardRanking_AddLabel").click(function () {
                    var $Obj = $("#ul_AwardRanking");
                    var ul_length = parseInt($Obj.find("li").length);
                    if (ul_length >= 5) {
                        alert("最大支持前5名加奖");
                        return;
                    }
                    ul_length += 1;
                    var html = '<li>名&nbsp;<label id="AwardRanking_PK_' + ul_length + '" class="label">' + ul_length + '</label>&nbsp;次&nbsp;&nbsp;加奖金额：<input id="txtAwardRankingAward_' + ul_length + '" class="input award" style="width: 45px;" type="text" /></li>';
                    $Obj.append(html);
                });
            </script>
        </script>
        <script id="Template_RegularBall" style="width: 500px; height: 320px;" type="text/template" title="数字彩中球玩法">
            <div class="content-tab-wrap" style="width: 500px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularBallPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidRBallID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>选球类型</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularBallType" runat="server">
                                    <asp:ListItem Value="1">指定红球数字</asp:ListItem>
                                    <asp:ListItem Value="2">指定蓝球数字(高频彩没有蓝球)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </dd>
                    </dl>
                    <dl>
                        <dt>指定选号</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularBall" runat="server" CssClass="input"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单次加奖金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularBallAwardMoney" runat="server" CssClass="input" TextMode="Number" Text="2"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>上限金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularBallTopLimit" runat="server" CssClass="input" TextMode="Number" Text="0"></asp:TextBox>
                        </dd>
                    </dl>
                </div>
            </div>
        </script>
        <script id="Template_RegularHoliday" style="width: 500px; height: 320px;" type="text/template" title="数字彩中球玩法">
            <div class="content-tab-wrap" style="width: 500px; height: 280px;">
                <div class="content-tab">
                    <div class="content-tab-ul-wrap">
                        <ul>
                            <li><a class="selected" href="javascript:;">详细规则</a></li>
                        </ul>
                    </div>
                </div>
                <div class="tab-content">
                    <dl>
                        <dt>加奖玩法</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularHolidayPlayCode" runat="server">
                                </asp:DropDownList>
                            </div>
                            <asp:HiddenField ID="hidRHolidayID" runat="server" Value="0" />
                        </dd>
                    </dl>
                    <dl>
                        <dt>节假日类型</dt>
                        <dd>
                            <div class="rule-single-select">
                                <asp:DropDownList ID="ddlRegularHoliadyType" runat="server">
                                    <asp:ListItem Value="1">周六加奖</asp:ListItem>
                                    <asp:ListItem Value="2">周日加奖</asp:ListItem>
                                    <asp:ListItem Value="3">周六日加奖</asp:ListItem>
                                    <asp:ListItem Value="4">指定时间段加奖(按活动开始结束时间加奖)</asp:ListItem>
                                </asp:DropDownList>
                            </div>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单次加奖金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularHoliadyAwardMoney" runat="server" CssClass="input" TextMode="Number" Text="2"></asp:TextBox>
                        </dd>
                    </dl>
                    <dl>
                        <dt>单日上限金额</dt>
                        <dd>
                            <asp:TextBox ID="txtRegularHoliadyTopLimitDay" runat="server" CssClass="input" TextMode="Number" Text="0"></asp:TextBox>
                        </dd>
                    </dl>
                </div>
            </div>
        </script>
    </form>
</body>
</html>
