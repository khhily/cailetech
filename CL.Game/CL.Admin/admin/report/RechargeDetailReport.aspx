<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RechargeDetailReport.aspx.cs" Inherits="CL.Admin.admin.report.RechargeDetailReport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>充值明细</title>
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
            <span>充值明细</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="输入用户名" />
                        <asp:TextBox ID="txtMobile" runat="server" CssClass="input txt" placeholder="输入手机号" />
                        <asp:TextBox ID="txtOrderNo" runat="server" CssClass="input txt" placeholder="输入系统订单号" />
                        <asp:TextBox ID="txtRechargeNo" runat="server" CssClass="input txt" placeholder="输入借口商订单号" />
                        <asp:TextBox ID="txtOutRechargeNo" runat="server" CssClass="input txt" placeholder="输入第三方订单号" />
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})"/>
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" style="margin-left: 30px;"/>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;margin-left: 60px;">查询</asp:LinkButton>
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
                            <th align="center">用户名</th>
                            <th align="center">手机号码</th>
                            <th align="center">实名认证</th>
                            <th align="center">交易金额</th>
                            <th align="center">账户余额</th>
                            <th align="center">系统订单号</th>
                            <th align="center">接口商订单号</th>
                            <th align="center">第三方订单号</th>
                            <th align="center">手续费</th>
                            <th align="center">支付类型</th>
                            <th align="center">是否机器人</th>
                            <th align="center">是否正常登录</th>
                            <th align="center">重置状态</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("UserName") %></td>
                        <td align="center"><%#Eval("UserMobile") %></td>
                        <td align="center"><%#IsTrueOrFalse(Eval("IsVerify")) %></td>
                        <td align="center"><%#ConvertMoney(Eval("TradeAmount")) %></td>
                        <td align="center"><%#ConvertMoney(Eval("Balance")) %></td>
                        <td align="center"><%#Eval("OrderNo") %></td>
                        <td align="center"><%#Eval("RechargeNo") %></td>
                        <td align="center"><%#Eval("OutRechargeNo") %></td>
                        <td align="center"><%#Eval("FormalitiesFees") %></td>
                        <td align="center"><%#Eval("PayType") %></td>
                        <td align="center"><%#IsTrueOrFalse(Eval("IsRobot")) %></td>
                        <td align="center"><%#IsTrueOrFalse(Eval("IsCanLogin")) %></td>
                        <td align="center"><%#SetResult(Eval("Result").ToString()) %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
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
