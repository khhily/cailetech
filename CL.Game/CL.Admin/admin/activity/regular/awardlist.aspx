<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="awardlist.aspx.cs" Inherits="CL.Admin.admin.activity.regular.award" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>活动规则列表</title>
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
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>活动规则列表</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        活动主题：<asp:Label ID="lbActivitySubject" runat="server"></asp:Label>
                    </div>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlRegularType" runat="server">
                                <asp:ListItem Value="-1">全部规则</asp:ListItem>
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
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlRegularLottery" runat="server">
                                <asp:ListItem Value="-1">全部彩种</asp:ListItem>
                                <asp:ListItem Value="101">红快3(吉林)</asp:ListItem>
                                <asp:ListItem Value="102">赢快3(江西)</asp:ListItem>
                                <asp:ListItem Value="201">华11选5(湖北)</asp:ListItem>
                                <asp:ListItem Value="202">老11选5(山东)</asp:ListItem>
                                <asp:ListItem Value="301">老时时彩(重庆)</asp:ListItem>
                                <asp:ListItem Value="801">双色球</asp:ListItem>
                                <asp:ListItem Value="901">超级大乐透</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlRegularStatus" runat="server">
                                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                                <asp:ListItem Value="0">初始化规则</asp:ListItem>
                                <asp:ListItem Value="1">规则作废</asp:ListItem>
                                <asp:ListItem Value="2">规则开始并生效</asp:ListItem>
                                <asp:ListItem Value="3">活动截止并开始加奖</asp:ListItem>
                                <asp:ListItem Value="4">活动结束并销毁</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                        <asp:LinkButton ID="linkUrl" runat="server" OnClick="linkUrl_Click" Text="添加活动规则"></asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>规则类型</th>
                            <th>活动彩种</th>
                            <th>已累计派奖</th>
                            <th>规则状态</th>
                            <th>操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# SetType(Eval("RegularType"))%></td>
                        <td align="center"><%# SetLot(Eval("LotteryCode"))%></td>
                        <td align="center"><%# SetMoney(Eval("TotalAwardMoney"))%></td>
                        <td align="center"><%# SetStatus(Eval("RegularStatus"))%></td>
                        <td>
                            <asp:HiddenField ID="hidRegularID" runat="server" Value='<%# Eval("RegularID")%>' />
                            <asp:LinkButton ID="LinRegular" runat="server" CommandName="Regular" Text="查看编辑"></asp:LinkButton>
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
          </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <!--/列表-->
    </form>
</body>
</html>
