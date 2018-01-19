<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="StaticdataReport.aspx.cs" Inherits="CL.Admin.admin.report.StaticdataReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>平台数据报表</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>静态数据报表</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="r-list">
                        <asp:TextBox ID="txtDay" runat="server" CssClass="input txt" onfocus="WdatePicker({dateFmt:'yyyy-MM'})" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="lbtnSearch_Click">查询</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->
        <!--内容-->
        <div class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">统计</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content" style="min-height: 300px;">
            <dl>
                <dt>注册总数</dt>
                <dd style="color:red;">
                    <%=RecordUsers %>
                </dd>
                <dt>购彩总额</dt>
                <dd style="color:red;">
                    <%="￥"+(RecordBuy/100).ToString("N2") %>
                </dd>
                <dt>充值总额</dt>
                <dd style="color:red;">
                    <%="￥"+(RecordRecharge/100).ToString("N2") %>
                </dd>
                <dt>赠送总额</dt>
                <dd style="color:red;">
                    <%="￥"+(RecordLargess/100).ToString("N2") %>
                </dd>
                <dt>提现总额</dt>
                <dd style="color:red;">
                    <%="￥"+(RecordWithdraw/100).ToString("N2") %>
                </dd>
                <dt>中奖总额</dt>
                <dd style="color:red;">
                    <%="￥"+(RecordWin/100).ToString("N2") %>
                </dd>
            </dl>
        </div>
        <!--/内容-->
        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th align="center">日期</th>
                            <th align="center">注册会员</th>
                            <th align="center">充值金额</th>
                            <th align="center">线上充值</th>
                            <th align="center">线下充值</th>
                            <th align="center">赠送金额</th>
                            <th align="center">吉林快三购彩</th>
                            <th align="center">吉林快三中奖</th>
                            <th align="center">江西快三购彩</th>
                            <th align="center">江西快三中奖</th>
                            <th align="center">湖北11选5购彩</th>
                            <th align="center">湖北11选5中奖</th>
                            <th align="center">山东11选5购彩</th>
                            <th align="center">山东11选5中奖</th>
                            <th align="center">重庆时时彩购彩</th>
                            <th align="center">重庆时时彩中奖</th>
                            <th align="center">双色球购彩</th>
                            <th align="center">双色球中奖</th>
                            <th align="center">大乐透购彩</th>
                            <th align="center">大乐透中奖</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("dateday") %></td>
                        <td align="center"><%#(Convert.ToDecimal(Eval("users")))%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("recharge"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("online_recharge"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("offline_recharge"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("largess"))/100)%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_jlk"))/100)%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_jlk"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_jxk"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_jxk"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_hbsyydj"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_hbsyydj"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_sdsyydj"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_sdsyydj"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_cqssc"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_cqssc"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_ssq"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_ssq"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("buy_dlt"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("win_dlt"))/100) %></td>
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
