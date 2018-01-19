<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_edit.aspx.cs" Inherits="CL.Admin.admin.users.user_edit" %>

<%@ Import namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>用户信息</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
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
  <a href="user_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="user_list.aspx"><span>用户列表</span></a>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">用户信息</a></li>
      </ul>
    </div>
  </div>
</div>
<%if (action == CaileEnums.ActionEnum.Add.ToString()){%>
<div class="tab-content" style="min-height:560px;">
  <dl>
    <dt>用户名</dt>
    <dd>
        <asp:TextBox ID="txtUserName" runat="server" CssClass="input normal" datatype="/^[a-zA-Z0-9\-\_]{2,50}$/" sucmsg=" " ajaxurl="../../tools/admin_ajax.ashx?action=manager_validate"></asp:TextBox> <span class="Validform_checktip">*字母、下划线，不可修改</span>
    </dd>
  </dl>
  <dl>
    <dt>登录密码</dt>
    <dd>
        <asp:TextBox ID="txtPassword" runat="server" CssClass="input normal" TextMode="Password" datatype="*6-20" nullmsg="请设置密码" errormsg="密码范围在6-20位之间" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*</span>
    </dd>
  </dl>
 <dl>
    <dt>手机号码</dt>
    <dd>
      <asp:TextBox ID="txtUserMobile" runat="server" CssClass="input normal" datatype="*" sucmsg=" "></asp:TextBox> <span class="Validform_checktip">*</span>
    </dd>
  </dl>
 <dl>
    <dt>用户余额</dt>
    <dd>
         <asp:TextBox ID="txtBalance" runat="server" CssClass="input small1" datatype="n" sucmsg=" ">0</asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>允许登陆</dt>
    <dd>
        <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblIsCanLogin" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0">否</asp:ListItem>
        <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
</div>
<%}else{%>
<div class="tab-content" style="min-height:560px;">
  <dl>
    <dt>用户名</dt>
    <dd>
        <asp:Label ID="labUserName" runat="server"></asp:Label>
    </dd>
  </dl>
  <dl>
    <dt>用户昵称</dt>
    <dd>
        <asp:Label ID="labNickName" runat="server"></asp:Label>
    </dd>
  </dl>
  <dl>
    <dt>真实姓名</dt>
    <dd>
        <asp:Label ID="labFullName" runat="server"></asp:Label>
    </dd>
  </dl>
  <dl>
    <dt>身份证号</dt>
    <dd>
        <asp:Label ID="labIDNumber" runat="server"></asp:Label>
    </dd>
  </dl>
 <dl>
    <dt>手机号码</dt>
    <dd>
      <asp:Label ID="labUserMobile" runat="server"></asp:Label>
    </dd>
  </dl>
 <dl>
    <dt>用户余额</dt>
    <dd>
        <asp:Label ID="labBalance" runat="server"></asp:Label>
    </dd>
  </dl>
 <dl>
    <dt>冻结金额</dt>
    <dd>
        <asp:Label ID="labFreeze" runat="server"></asp:Label>
    </dd>
  </dl>
 <dl>
    <dt>注册时间</dt>
    <dd>
        <asp:Label ID="labCreateTime" runat="server"></asp:Label>
    </dd>
  </dl>
 <dl>
    <dt>关注数</dt>
    <dd>
        <asp:Label ID="labIdols" runat="server"></asp:Label>
    </dd>
  </dl>
 <%--<dl>
    <dt>粉丝数</dt>
    <dd>
        <asp:Label ID="labFans" runat="server"></asp:Label>
    </dd>
  </dl>--%>
</div>
<%} %>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <%if (action == CaileEnums.ActionEnum.Add.ToString()) {%>
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <%} %>
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->
</form>
</body>
</html>
