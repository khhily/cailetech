<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="isuses_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.isuses_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>玩法编辑</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();
        });
    </script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="isuses_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span>编辑期号</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">期号基本信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content" style="min-height:560px;">
    <dl>
    <dt>期号名称</dt>
    <dd>
        <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input normal" datatype="*2-30" sucmsg=" " />
        <span class="Validform_checktip"></span>
    </dd>
    </dl>
    <dl>
    <dt>开始时间</dt>
    <dd>
        <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
    </dd>
    </dl>
    <dl>
    <dt>结束时间</dt>
    <dd>
        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
    </dd>
    </dl>
    <dl>
    <dt>开奖号码</dt>
    <dd>
    <asp:TextBox ID="txtOpenNumber" Enabled="false" runat="server" CssClass="input normal"></asp:TextBox>(<span style="color:red">开奖号码不可编辑</span>)
    </dd>
    </dl>
    <dl>
    <dt>状态</dt>
    <dd>
        <div class="rule-single-select">
        <asp:DropDownList id="ddlIsuseState" runat="server" datatype="*" errormsg="请选择状态" sucmsg=" ">
            <asp:ListItem Value="0">未开启</asp:ListItem>
            <asp:ListItem Value="1">开始</asp:ListItem>
            <asp:ListItem Value="2">暂停</asp:ListItem>
            <asp:ListItem Value="3">截止</asp:ListItem>
            <asp:ListItem Value="4">期结</asp:ListItem>
            <asp:ListItem Value="5">返奖</asp:ListItem>
            <asp:ListItem Value="6">返奖结束</asp:ListItem>
        </asp:DropDownList>
        </div>
    </dd>
    </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
