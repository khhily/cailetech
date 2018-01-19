<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_list.aspx.cs" Inherits="CL.Admin.admin.users.user_list" %>

<%@ Import namespace="CL.Enum.Common" %>
<%@ Import namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>用户列表</title>
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
          <span>用户列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
                  <ul class="icon-list">
                      <li><a class="add" href="user_edit.aspx?action=<%=CaileEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
                  </ul>
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
          <asp:Repeater ID="rptList" runat="server" onitemcommand="rptList_ItemCommand">
          <HeaderTemplate>
          <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
            <tr>
              <th>用户名</th>
              <th>真实姓名</th>
              <th>身份证</th>
              <th>手机号</th>
              <th>余额</th>
              <th>冻结</th>
              <th>关注数</th>
              <%--<th>粉丝数</th>--%>
              <th>注册时间</th>
              <th>状态</th>
              <th>操作</th>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><a href="user_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("UserID")%>"><%# Eval("UserName") %></a></td>
              <td align="center"><%#Eval("FullName")%></td>
              <td align="center"><%#Eval("IDNumber")%></td>
              <td align="center"><%#Eval("UserMobile")%></td>
              <td align="center"><%#Utils.StrToDouble(Eval("Balance")) %></td>
              <td align="center"><%#Utils.StrToDouble(Eval("Freeze"))%></td>
              <td align="center"><%#Eval("Idols") %></td>
              <%--<td align="center"><%#Eval("Fans") %></td>--%>
              <td align="center"><%#string.Format("{0:g}",Eval("CreateTime"))%></td>
              <td align="center">
                  <asp:HiddenField ID="hidId" Value='<%#Eval("UserID")%>' runat="server" />
                  <asp:LinkButton ID="lbtnIsShow" CommandName="lbtnIsShow" runat="server" style='<%#Convert.ToBoolean(Eval("IsCanLogin")) ? "" : "color:red;" %>' Text='<%#Convert.ToBoolean(Eval("IsCanLogin")) ? "允许登陆" : "禁止登陆" %>' />
              </td>
              <td align="center">
                  <a href="userscheme_list.aspx?UserName=<%#Eval("UserName")%>">方案明细</a> | 
                  <a href="account_detail.aspx?userid=<%#Eval("UserID")%>">账户明细</a> <%--| --%>
                  <%--<a href="commission_details.aspx?userid=<%#Eval("UserID")%>">佣金明细</a>--%>
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
