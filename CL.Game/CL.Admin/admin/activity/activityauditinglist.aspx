<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activityauditinglist.aspx.cs" Inherits="CL.Admin.admin.activity.activityauditinglist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>活动审核列表</title>
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
            <span>活动审核列表</span>
        </div>
        <!--/导航栏-->
        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>活动标题</th>
                            <th>活动类型</th>
                            <th>申请时间</th>
                            <th>开始时间</th>
                            <th>结束时间</th>
                            <th>活动预算</th>
                            <th>预算币种</th>
                            <th>加奖规则</th>
                            <th>操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# (Eval("ActivitySubject"))%></td>
                        <td align="center"><%# Convert.ToInt32(Eval("ActivityType"))==1?"彩种官方活动":"彩乐彩票活动"%></td>
                        <td align="center"><%# (Eval("CreateTime"))%></td>
                        <td align="center"><%# (Eval("StartTime"))%></td>
                        <td align="center"><%# (Eval("EndTime"))%></td>
                        <td align="center"><%# (Convert.ToInt64(Eval("ActivityMoney"))/100).ToString("C2")%></td>
                        <td align="center"><%# SetCurrencyUnit(Eval("CurrencyUnit"))%></td>
                        <td align="center"><%# (Eval("RegularCount")) + " 条"%></td>
                        <td>
                            <asp:HiddenField ID="hidActivityID" runat="server" Value='<%# Eval("ActivityID")%>' />
                            <asp:LinkButton ID="LinApply" runat="server" CommandName="Apply" Text="查看并审核"></asp:LinkButton>
                        </td>
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