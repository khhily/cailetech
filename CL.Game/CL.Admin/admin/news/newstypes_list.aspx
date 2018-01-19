<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="newstypes_list.aspx.cs" Inherits="CL.Admin.admin.news.newstypes_list" %>
<%@ Import namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>信息类别管理</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
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
  <span>信息类别管理</span>
</div>
<!--/导航栏-->

<!--工具栏-->
<div id="floatHead" class="toolbar-wrap">
  <div class="toolbar">
    <div class="box-wrap">
      <a class="menu-btn"></a>
      <div class="l-list">
        <ul class="icon-list">
          <li><a class="add" href="newstypes_edit.aspx?action=<%=CaileEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
          <li><asp:LinkButton ID="btnSave" runat="server" CssClass="save" onclick="btnSave_Click"><i></i><span>保存</span></asp:LinkButton></li>
          <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
          <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete','本操作会删除本导航及下属子导航，是否继续？');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
        </ul>
      </div>
    </div>
  </div>
</div>
<!--/工具栏-->

<!--列表-->
<div class="table-container">
  <asp:Repeater ID="rptList" runat="server" onitemdatabound="rptList_ItemDataBound">
  <HeaderTemplate>
  <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
    <tr>
      <th width="6%">选择</th>
      <th align="left" width="6%">编号</th>
      <th align="left" width="12%">栏目名称</th>
      <th align="left" width="12%">状态</th>
      <th align="left" width="12%">排序</th>
      <th>操作</th>
    </tr>
  </HeaderTemplate>
  <ItemTemplate>
    <tr>
      <td align="center">
        <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" style="vertical-align:middle;" />
        <asp:HiddenField ID="hidId" Value='<%#Eval("TypeID")%>' runat="server" />
        <asp:HiddenField ID="hidLayer" Value='<%#Eval("ClassLayer") %>' runat="server" />
      </td>
      <td><%#Eval("TypeID")%></td>
      <td>
        <asp:Literal ID="LitFirst" runat="server"></asp:Literal>
        <a href="newstypes_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("TypeID")%>"><%#Eval("TypeName")%></a>
      </td>
      <td><%#Convert.ToInt32(Eval("IsShow")) == 0 ? "禁用" : "使用" %></td>
      <td><asp:TextBox ID="txtSortId" runat="server" Text='<%#Eval("Sort")%>' CssClass="sort" onkeydown="return checkNumber(event);" /></td>
      <td align="center">
        <a href="newstypes_edit.aspx?action=<%#CaileEnums.ActionEnum.Add %>&id=<%#Eval("TypeID")%>">添加子类</a>
        <a href="newstypes_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("TypeID")%>">修改</a>
      </td>
    </tr>
  </ItemTemplate>
  <FooterTemplate>
    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"6\">暂无记录</td></tr>" : ""%>
  </table>
  </FooterTemplate>
  </asp:Repeater>
</div>
<!--/列表-->

</form>
</body>
</html>
