<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="winfind_list.aspx.cs" Inherits="CL.Admin.admin.lotteries.winfind_list" %>

<%@ Import namespace="CL.Tools.Common" %>
<%@ Import namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>中奖查询</title>
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
          <span>中奖查询</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
                  <ul>
                      <li style="float:left;">
                          <div class="rule-single-select">
                              <asp:DropDownList id="ddlLotteryCode" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlLotteryCode_SelectedIndexChanged"></asp:DropDownList>
                          </div>
                          <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input txt" placeholder="输入期号" style="padding-left:30px;" />
                      </li>
                      <li style="float:left;">
                          购买时间：
                          <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                          <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                      </li>
                      <li style="float:left;">
                          <asp:TextBox ID="txtSchemeNumber" runat="server" CssClass="input txt" placeholder="方案编号" style="margin-left:35px;" />
                          <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="用户账号" style="padding-left:10px;" />
                      </li>
                  </ul>
                  <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" onclick="btnSearch_Click" style="color:#ffffff;">查询</asp:LinkButton>
              </div>
            </div>
          </div>
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        方案总金额：<span style='color:red; font-weight:bold;'><%=Utils.StrToDouble(SumMoney) %></span> 元	 
                        税后总金额：<span style='color:red; font-weight:bold;'><%=Utils.StrToDouble(WinSumMoney) %></span> 元	
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
              <th width="10%">用户名</th>
              <th width="10%">方案号</th>
              <th width="10%">时间</th>
              <th width="10%">方案金额</th>
              <th>中奖情况</th>
              <th width="10%">中奖金额</th>
              <th width="10%">税后金额</th>
              <%--<th width="10%">投注数</th>
              <th width="10%">倍数</th>--%>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><%# Eval("UserName") %></td>
              <td align="center"><a href="scheme_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("SchemeID")%>"><%#Eval("SchemeNumber")%></a></td>
              <td align="center"><%#string.Format("{0:g}",Eval("CreateTime"))%></td>
              <td align="center"><%#Utils.StrToDouble(Eval("SchemeMoney")) %></td>
              <td align="center"><%#Eval("Describe") %></td>
              <td align="center"><%#Utils.StrToDouble(Eval("WinMoney")) %></td>
              <td align="center"><%#Utils.StrToDouble(Eval("WinMoneyNoWithTax")) %></td>
              <%--<td align="center"><%#Eval("BetNum") %></td>
              <td align="center"><%#Eval("Multiple") %></td>--%>
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
