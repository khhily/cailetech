<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reportcoupons.aspx.cs" Inherits="CL.Admin.admin.coupons.report.reportcoupons" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>彩券报表</title>
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
            <span>彩券报表</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlTimeType" runat="server">
                                <asp:ListItem Value="1">生成时间</asp:ListItem>
                                <asp:ListItem Value="2">失效时间</asp:ListItem>
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
                            <th width="100">发放个数</th>
                            <th width="100">发放金额</th>
                            <th width="100">失效个数</th>
                            <th width="100">失效金额</th>
                            <th width="100">消费总金额</th>
                            <th width="100">固定时段个数</th>
                            <th width="100">固定时段金额</th>
                            <th width="100">固定时长个数</th>
                            <th width="100">固定时长金额</th>
                            <th width="100">满减个数</th>
                            <th width="100">满减金额</th>
                            <th width="100">不过期个数</th>
                            <th width="100">不过期金额</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Eval("DTime") %></td>
                        <td align="center"><%# Eval("ReleaseCount") %></td>
                        <td align="center"><%# Eval("ReleaseMoney") %></td>
                        <td align="center"><%# Eval("ExpireCount") %></td>
                        <td align="center"><%# Eval("ExpireAmount") %></td>
                        <td align="center"><%# Eval("Amount") %></td>
                        <td align="center"><%# Eval("GDTimeCount") %></td>
                        <td align="center"><%# Eval("GDTimeAmount") %></td>
                        <td align="center"><%# Eval("GDCount") %></td>
                        <td align="center"><%# Eval("GDAmount") %></td>
                        <td align="center"><%# Eval("MJCount") %></td>
                        <td align="center"><%# Eval("MJAmount") %></td>
                        <td align="center"><%# Eval("NoExpireCount") %></td>
                        <td align="center"><%# Eval("NoExpireAmount") %></td>
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
