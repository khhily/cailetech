<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="arena_list.aspx.cs" Inherits="CL.Admin.admin.users.arena_list" %>


<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>擂台玩家排行</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
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
          <span>擂台玩家排行</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
              </div>
              <div class="r-list">
                <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" placeholder="输入用户名"/>
                <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" onclick="btnSearch_Click">查询</asp:LinkButton>
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
              <th>排名</th>
              <th>用户名</th>
              <th>投注次数</th>
              <th>命中次数</th>
              <th>中奖次数</th>
              <th>总积分</th>
              <th>总奖金</th>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><%# Eval("RowID") %></a></td>
              <td align="center"><%#Eval("UserName")%></td>
              <td align="center"><%#Eval("BetCount")%></td>
              <td align="center"><%#Eval("WinCount")%></td>
              <td align="center"><%#Eval("SumMoney")%></td>
              <td align="center"><%#Eval("SumScore")%></td>
              <td align="center"><%#Eval("TotalWinMoney") %></td>
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
