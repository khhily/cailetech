<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="user_recharge.aspx.cs" Inherits="CL.Admin.admin.users.user_recharge" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>账户充值</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
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
  <span>账户充值</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">账户充值</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content" style="min-height:560px;">
    <dl>
    <dt>用户账号</dt>
    <dd>
        <asp:TextBox ID="txtUserName" runat="server" CssClass="input normal" datatype="*2-30" sucmsg=" " ajaxurl="../../tools/admin_ajax.ashx?action=username_validate"/>
    </dd>
    </dl>
    <dl>
    <dt>充值金额</dt>
    <dd>
        <asp:TextBox ID="txtMoney" runat="server" CssClass="input small1" onkeydown="return checkMinusNumber(event);" datatype="*1-20" sucmsg=" "/>
    </dd>
    </dl>
    <dl>
    <dt>摘要</dt>
    <dd>
    <asp:TextBox ID="txtMemo" runat="server" CssClass="input normal"></asp:TextBox>
    </dd>
    </dl>
    <dl>
    <dt>提示</dt>
    <dd style="color:#cc0000;">
        可以通过输入负数进行充减金额
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
