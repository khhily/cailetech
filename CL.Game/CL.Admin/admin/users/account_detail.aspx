<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="account_detail.aspx.cs" Inherits="CL.Admin.admin.users.account_detail" %>

<%@ Import namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>账户明细记录</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <%--<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>--%>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
          <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
          <i class="arrow"></i>
          <span>账户明细记录</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
                <div class="rule-single-select">
                    <asp:DropDownList ID="ddlTradeType" runat="server" OnSelectedIndexChanged="ddlTradeType_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="-1">全部</asp:ListItem>
                        <asp:ListItem Value="1">购彩消费</asp:ListItem>
                        <asp:ListItem Value="2">提现冻结</asp:ListItem>
                        <asp:ListItem Value="3">提现失败解冻</asp:ListItem>
                        <asp:ListItem Value="4">金豆兑换</asp:ListItem>
                        <asp:ListItem Value="5">中奖</asp:ListItem>
                        <asp:ListItem Value="11">用户撤单</asp:ListItem>
                        <asp:ListItem Value="12">系统撤单</asp:ListItem>
                        <asp:ListItem Value="13">追号撤单</asp:ListItem>
                        <asp:ListItem Value="14">投注失败退款</asp:ListItem>
                        <asp:ListItem Value="15">出票失败退款</asp:ListItem>
                        <asp:ListItem Value="16">充值退款冻结</asp:ListItem>
                        <asp:ListItem Value="17">退款失败返回金额</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})"/>
                <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" style="margin-left: 30px;"/>
                <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;margin-left: 60px;">查询</asp:LinkButton>
            </div>
              <div class="r-list">
                  <ul class="icon-list">
                      <li><span>合计：收入： <%=Utils.StrToDouble(SumMoneyAdd) %> 元，支出：<%=Utils.StrToDouble(SumMoneySub) %> 元，中奖金额：<%=Utils.StrToDouble(SumReward) %> 元</span></li>
                  </ul>
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
              <th width="10%">时间</th>
              <th width="10%">交易类型</th>
              <th>摘要</th>
              <th width="10%">金额(元)</th>
              <th width="10%">支出(元)</th>
              <th width="10%">余额</th>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><%#string.Format("{0:g}", Eval("CreateTime"))%></td>
              <td align="center"><%#SetTradeTypeName(Convert.ToInt16(Eval("TradeType")))%></td>
              <td align="center"><%#Eval("Memo")%></td>
              <td align="center"><%#Utils.StrToDouble(Eval("MoneyAdd")) %></td>
              <td align="center"><%#Utils.StrToDouble(Eval("MoneySub")) %></td>
              <td align="center"><%#Utils.StrToDouble(Eval("Balance")) %></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
          </table>
          </FooterTemplate>
          </asp:Repeater>
        </div>
        <!--/列表-->

        <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
          <div class="l-btns">
            <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
          </div>
          <div id="PageContent" runat="server" class="default"></div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>
