<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="cdkeylist.aspx.cs" Inherits="CL.Admin.admin.coupons.cdkeylist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>兑换码列表</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>兑换码列表</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        失效时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />-
                        <asp:TextBox ID="txtExpireTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="输入用户名" Style="margin-left: 60px;" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                        <asp:LinkButton ID="linkUrl" runat="server" PostBackUrl="~/admin/coupons/generatecdkeys.aspx" Text="生成兑换码"></asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>兑换用户</th>
                            <th>用户号码</th>
                            <th>生成时间</th>
                            <th>过期时间</th>
                            <th>合作方代码</th>
                            <th>兑换码</th>
                            <th>加密的兑换码</th>
                            <th>是否的兑换</th>
                            <th>兑换时间</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Eval("UserName")??"--" %></td>
                        <td align="center"><%# Eval("UserMobile")??"--" %></td>
                        <td align="center"><%# Eval("GenerateTime") %></td>
                        <td align="center"><%# Eval("ExpireTime") %></td>
                        <td align="center"><%# Eval("PartnerCode") %>(彩乐彩票)</td>
                        <td align="center"><%# Eval("CDKey") %></td>
                        <td align="center"><%# Eval("EncryptKey") %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsExchanger"))==true?"已兑换":"未兑换" %></td>
                        <td align="center"><%# Eval("ExchangerTime")??"--" %>
                            <input id="HidCouponsID" type="hidden" value='<%# Eval("CouponsID") %>' runat="server" />
                            <input id="HidCDKeyID" type="hidden" value='<%# Eval("CDKeyID") %>' runat="server" />
                            <input id="HidUserID" type="hidden" value='<%# Eval("ExchangerUserID") %>' runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"9\">暂无记录</td></tr>" : ""%>
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
