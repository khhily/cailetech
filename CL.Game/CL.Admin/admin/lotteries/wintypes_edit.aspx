<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="wintypes_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.wintypes_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>奖等编辑</title>
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
  <a href="wintypes_list.aspx?LotteryCode=<%=LotteryCode %>" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="wintypes_list.aspx?LotteryCode=<%=LotteryCode %>"><span>奖等列表</span></a>
  <i class="arrow"></i>
  <span>编辑奖等</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">奖等基本信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>奖等名称</dt>
    <dd>
        <asp:TextBox ID="txtWinName" runat="server" CssClass="input normal" datatype="*2-30" sucmsg=" " />
        <span class="Validform_checktip">*奖等名称最多30个字符</span>
    </dd>
  </dl>
  <dl>
    <dt>奖等代码</dt>
    <dd>
        <asp:TextBox ID="txtWinCode" runat="server" CssClass="input small1" datatype="n" sucmsg=" " ajaxurl="../../tools/admin_ajax.ashx?action=wincode_validate">0</asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>默认奖金</dt>
    <dd>
      <asp:TextBox ID="txtDefaultMoney" runat="server" CssClass="input small1" datatype="n" sucmsg=" ">0</asp:TextBox>
    </dd>
  </dl>
   <dl>
    <dt>默认税后奖金</dt>
    <dd>
      <asp:TextBox ID="txtDefaultMoneyNoWithTax" runat="server" CssClass="input small1" datatype="n" sucmsg=" " Text="0" />
    </dd>
  </dl>
  <dl>
    <dt>是否和值</dt>
    <dd>
        <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblIsSumValue" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
        <asp:ListItem Value="1">是</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>中奖号码</dt>
    <dd>
        <asp:TextBox ID="txtWinNumber" runat="server" CssClass="input normal" TextMode="MultiLine" style="width:50%; height:80px;"></asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>排序数字</dt>
    <dd>
      <asp:TextBox ID="txtSortId" runat="server" CssClass="input small1" datatype="n" sucmsg=" ">99</asp:TextBox>
      <span class="Validform_checktip">*数字，越小越向前</span>
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
