<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CheckingReport.aspx.cs" Inherits="CL.Admin.admin.report.CheckingReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>统计报表</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>统计报表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="r-list">
                        <%--<asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />--%>
                        <asp:TextBox ID="txtDay" runat="server" CssClass="input txt" onfocus="WdatePicker({dateFmt:'yyyy-MM'})" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="lbtnSearch_Click">查询</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th align="center">日期</th>
                            <th align="center">充值金额</th>
                            <th align="center">消费金额</th>
                            <th align="center">提现解冻</th>
                            <th align="center">提现失败解冻</th>
                            <th align="center">金豆兑换</th>
                            <th align="center">中奖金额</th>
                            <th align="center">用户撤单</th>
                            <th align="center">系统撤单</th>
                            <th align="center">追号撤单</th>
                            <th align="center">消费失败退款</th>
                            <th align="center">出票失败退款</th>
                            <th align="center">退款冻结</th>
                            <th align="center">退款失败</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("Day") %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("Pay"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("BuyLot"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("Freeze"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("UnFreeze"))/100)%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("Imazamox"))/100)%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("WinMoney"))/100)%></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("UserRevoke"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("SystemRevoke"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("ChaseRevoke"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("BetRevoke"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("TicketRevoke"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("RefundFreeze"))/100) %></td>
                        <td align="center">￥<%#(Convert.ToDecimal(Eval("RefundFailure"))/100) %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%if (rptList.Items.Count == 0)
                        { %>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
                    <%}
    else
    { %>
                    <tr>
                        <th align="center">日期</th>
                        <th align="center">充值金额</th>
                        <th align="center">消费金额</th>
                        <th align="center">提现解冻</th>
                        <th align="center">提现失败解冻</th>
                        <th align="center">金豆兑换</th>
                        <th align="center">中奖金额</th>
                        <th align="center">用户撤单</th>
                        <th align="center">系统撤单</th>
                        <th align="center">追号撤单</th>
                        <th align="center">消费失败退款</th>
                        <th align="center">出票失败退款</th>
                        <th align="center">退款冻结</th>
                        <th align="center">退款失败</th>
                    </tr>
                    <%} %>
          </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </form>
</body>
</html>
