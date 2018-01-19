<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="withdrawreport.aspx.cs" Inherits="CL.Admin.admin.report.finance.withdrawreport" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>提现查询统计</title>
    <link href="../../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>提现查询统计</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        用户ID:<asp:TextBox ID="txtUserCode" runat="server" CssClass="input" />
                        用户账号:<asp:TextBox ID="txtUserName" runat="server" CssClass="input" />
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlPayOutStatus" runat="server">
                                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                                <asp:ListItem Value="0">申请</asp:ListItem>
                                <asp:ListItem Value="2">处理中</asp:ListItem>
                                <asp:ListItem Value="4">处理完成</asp:ListItem>
                                <asp:ListItem Value="6">提现失败</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container">
            充值总额：<%=TotalAmount/100 %>
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>平台订单编号</th>
                            <th>用户ID</th>
                            <th>用户账号</th>
                            <th>手机号码</th>
                            <th>充值方式</th>
                            <th>提交时间</th>
                            <th>支付金额</th>
                            <th>第三方订单号</th>
                            <th>订单状态</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# (Eval("OrderNo"))%></td>
                        <td align="center"><%# (Eval("UserID"))%></td>
                        <td align="center"><%# (Eval("UserName"))%></td>
                        <td align="center"><%# (Eval("UserMobile"))%></td>
                        <td align="center"><%# (Eval("PayType"))%></td>
                        <td align="center"><%# Eval("CreateTime")%></td>
                        <td align="center"><%# Convert.ToInt64(Eval("TradeAmount"))/100%></td>
                        <td align="center"><%# (Eval("RechargeNo"))%></td>
                        <td align="center"><%# (Eval("Result"))%></td>
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
