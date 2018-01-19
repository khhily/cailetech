<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="center.aspx.cs" Inherits="CL.Admin.admin.center" %>


<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>管理首页</title>
    <link href="skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="js/layindex.js"></script>
    <script type="text/javascript" charset="utf-8" src="js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>管理中心</span>
        </div>
        <!--/导航栏-->

        <!--内容-->
        <div class="line10"></div>
        <div class="nlist-1">
            <ul>
                <li>本次登录IP：<asp:Literal ID="litIP" runat="server" Text="-" /></li>
                <li>上次登录IP：<asp:Literal ID="litBackIP" runat="server" Text="-" /></li>
                <li>上次登录时间：<asp:Literal ID="litBackTime" runat="server" Text="-" /></li>
            </ul>
        </div>
        <div class="tab-content" style="height: 300px; width: 35%; margin-top: 20px; border: solid 1px #eee; float: left; margin-left: 10%;">
            <ul class="nlist-1">
                <li>今日注册会员：<span style="font-weight:bold;"><%=Day_RecordUsers %></span></li>
                <li>今日投注金额：<span style="font-weight:bold;"><%=Day_RecordBuy %></span></li>
                <li>今日提现金额：<span style="font-weight:bold;"><%=Day_RecordWithdraw %></span></li>
                <li>今日赠送金额：<span style="font-weight:bold;"><%=Day_RecordLargess %></span></li>
                <li>今日中奖金额：<span style="font-weight:bold;"><%=Day_RecordWin %></span></li>
                <li>今日充值金额：<span style="font-weight:bold;"><%=Day_RecordRecharge %></span></li>
            </ul>
        </div>
        <div class="tab-content" style="height: 300px; width: 35%; margin-top: 20px; border: solid 1px #eee; float: left;margin-left: 10%;">
            <ul class="nlist-1" style="float:left;">
                <li>本周注册会员：<span style="font-weight:bold;"><%=Week_RecordUsers %></span></li>
                <li>本周投注金额：<span style="font-weight:bold;"><%=Week_RecordBuy %></span></li>
                <li>本周提现金额：<span style="font-weight:bold;"><%=Week_RecordWithdraw %></span></li>
                <li>本周赠送金额：<span style="font-weight:bold;"><%=Week_RecordLargess %></span></li>
                <li>本周中奖金额：<span style="font-weight:bold;"><%=Week_RecordWin %></span></li>
                <li>本周充值金额：<span style="font-weight:bold;"><%=Week_RecordRecharge %></span></li>
            </ul>
        </div>
        <div class="tab-content" style="height: 300px; width: 35%; margin-top: 20px; border: solid 1px #eee; float: left; margin-left: 10%;">
            <ul class="nlist-1">
                <li>本月注册会员：<span style="font-weight:bold;"><%=Month_RecordUsers %></span></li>
                <li>本月投注金额：<span style="font-weight:bold;"><%=Month_RecordBuy %></span></li>
                <li>本月提现金额：<span style="font-weight:bold;"><%=Month_RecordWithdraw %></span></li>
                <li>本月赠送金额：<span style="font-weight:bold;"><%=Month_RecordLargess %></span></li>
                <li>本月中奖金额：<span style="font-weight:bold;"><%=Month_RecordWin %></span></li>
                <li>本月充值金额：<span style="font-weight:bold;"><%=Month_RecordRecharge %></span></li>
            </ul>
        </div>
        <div class="tab-content" style="height: 280px; width: 35%; margin-top: 20px; border: solid 1px #eee; float: left; margin-left: 10%;">
            <ul class="nlist-1">
                <li>会员总数：<span style="font-weight:bold;"><%=RecordUsers %></span></li>
                <li>投注总额：<span style="font-weight:bold;"><%=RecordBuy %></span></li>
                <li>提现总额：<span style="font-weight:bold;"><%=RecordWithdraw %></span></li>
                <li>赠送总额：<span style="font-weight:bold;"><%=RecordLargess %></span></li>
                <li>中奖总额：<span style="font-weight:bold;"><%=RecordWin %></span></li>
                <li>充值总额：<span style="font-weight:bold;"><%=RecordRecharge %></span></li>
            </ul>
        </div>
    </form>
</body>
</html>
