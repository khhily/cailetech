<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportcdkey.aspx.cs" Inherits="CL.Admin.admin.coupons.report.reportcdkey" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>兑换码报表</title>
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
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>兑换码报表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        合作商：<div class="rule-single-select">
                            <asp:DropDownList ID="ddlPartnerCode" runat="server">
                                <asp:ListItem Value="10">彩乐彩票</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlTimeType" runat="server">
                                <asp:ListItem Value="1">生成时间</asp:ListItem>
                                <asp:ListItem Value="2">失效时间</asp:ListItem>
                                <asp:ListItem Value="3">兑换时间</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="input rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM'})" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <%--OnItemCommand="rptList_ItemCommand"--%>
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="100">日期</th>
                            <th width="100">生成</th>
                            <th width="100">过期</th>
                            <th width="100">兑换</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Eval("DTime") %></td>
                        <td align="center"><%# Eval("GenerateCount") %></td>
                        <td align="center"><%# Eval("ExpireCount") %></td>
                        <td align="center"><%# Eval("ExchangerCount") %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
          </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <!--/列表-->


    </form>
</body>
</html>
