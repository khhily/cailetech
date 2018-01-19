<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="notifyinfo_list.aspx.cs" Inherits="CL.Admin.admin.message.notifyinfo_list" %>

<%@ Import namespace="CL.Tools.Common" %>
<%@ Import namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>消息列表</title>
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
          <span>消息列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
                <ul class="icon-list">
                  <li><a class="add" href="notifyinfo_edit.aspx?action=<%=CaileEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
                  <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                  <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
                </ul>
                <div class="rule-single-select">
                    <asp:DropDownList id="ddlNotifyType" runat="server" datatype="*" errormsg="请选择消息类型..." sucmsg=" " OnSelectedIndexChanged="ddlNotifyType_SelectedIndexChanged" AutoPostBack="True">
                        <asp:ListItem Value="0">全部</asp:ListItem>
                        <asp:ListItem Value="1">公告</asp:ListItem>
                        <asp:ListItem Value="2">提醒</asp:ListItem>
                        <asp:ListItem Value="3">私信</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="rule-single-select">
                <asp:DropDownList id="ddlTargetType" runat="server" datatype="*" errormsg="请选择目标类型..." sucmsg=" " OnSelectedIndexChanged="ddlTargetType_SelectedIndexChanged" AutoPostBack="True">
                    <asp:ListItem Value="0">全部</asp:ListItem>
                    <asp:ListItem Value="1">所有客户端</asp:ListItem>
                    <asp:ListItem Value="2">App客户端</asp:ListItem>
                    <asp:ListItem Value="3">pc客户端</asp:ListItem>
                    <asp:ListItem Value="4">公众号客户端</asp:ListItem>
                </asp:DropDownList>
                </div>
              </div>
              <div class="r-list">
                <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
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
              <th width="8%">选择</th>
              <th align="center" width="10%">标题名称</th>
              <th align="center">内容</th>
              <th align="center" width="8%">消息类型</th>
              <th align="center" width="8%">目标类型</th>
              <th align="center" width="10%">创建时间</th>
              <th width="8%">操作</th>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center">
                <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
                <asp:HiddenField ID="hidId" Value='<%#Eval("ID") %>' runat="server" />
              </td>
              <td align="center"><%#Eval("Title") %></td>
              <td align="center"><%#Utils.CutString(Eval("Content").ToString(), 250) %></td>
              <td align="center" class="notifytype"><%#Eval("NotifyType") %></td>
              <td align="center" class="targettype"><%#Eval("TargetType") %></td>
              <td align="center"><%#string.Format("{0:g}", Eval("CreateTime")) %></td>
              <td align="center"><a href="#?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("ID") %>">重新发送</a></td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : "" %>
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

    <script>
        $(function () {
            $(".notifytype").each(function (i, d) {
                var text = $(this).text();
                switch (text) {
                    case "1":
                        $(this).text("公告");
                        break;
                    case "2":
                        $(this).text("提醒");
                        break;
                    case "3":
                        $(this).text("私信");
                        break;
                }
            });

            $(".targettype").each(function (i, d) {
                var text = $(this).text();
                switch (text) {
                    case "1":
                        $(this).text("客户端");
                        break;
                    case "2":
                        $(this).text("App客户端");
                        break;
                    case "3":
                        $(this).text("pc客户端");
                        break;
                    case "4":
                        $(this).text("公众号客户端");
                        break;
                }
            });
        });
    </script>
</body>
</html>
