<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activitylist.aspx.cs" Inherits="CL.Admin.admin.activity.activitylist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>活动列表</title>
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
            <span>活动列表</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlActivityType" runat="server">
                                <asp:ListItem Value="-1">全部类型</asp:ListItem>
                                <asp:ListItem Value="0">官方活动</asp:ListItem>
                                <asp:ListItem Value="1">彩乐平台活动</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlIsModify" runat="server">
                                <asp:ListItem Value="-1">全部活动</asp:ListItem>
                                <asp:ListItem Value="0">非变更活动</asp:ListItem>
                                <asp:ListItem Value="1">变更活动</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlActivityApply" runat="server">
                                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                                <asp:ListItem Value="0">申请中</asp:ListItem>
                                <asp:ListItem Value="1">审核通过</asp:ListItem>
                                <asp:ListItem Value="2">审核拒绝</asp:ListItem>
                                <asp:ListItem Value="3">变更申请</asp:ListItem>
                                <asp:ListItem Value="4">变更审核通过</asp:ListItem>
                                <asp:ListItem Value="5">变更审核拒绝</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlCurrencyUnit" runat="server">
                                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                                <asp:ListItem Value="0">余额</asp:ListItem>
                                <asp:ListItem Value="1">元宝</asp:ListItem>
                                <asp:ListItem Value="2">游戏币</asp:ListItem>
                                <asp:ListItem Value="3">彩券</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        申请时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />-
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtKeys" runat="server" CssClass="input txt" placeholder="输入关键字" Style="margin-left: 60px;" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                        <asp:LinkButton ID="linkUrl" runat="server" PostBackUrl="~/admin/activity/activity.aspx" Text="申请活动"></asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server" OnItemCommand="rptList_ItemCommand">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>活动主题</th>
                            <th>活动类型</th>
                            <th>落地页</th>
                            <th>审核状态</th>
                            <th>赠送币种</th>
                            <th>活动总额</th>
                            <th>变更活动</th>
                            <th>申请时间</th>
                            <th>开始时间</th>
                            <th>结束时间</th>
                            <th>变更时间</th>
                            <th>变更总额</th>
                            <th>申请说明</th>
                            <th>变更说明</th>
                            <th>操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Sub(Eval("ActivitySubject"))%></td>
                        <td align="center"><%# SetType(Eval("ActivityType"))%></td>
                        <td align="center"><%# Eval("LandingPage")%></td>
                        <td align="center"><%# SetApply(Eval("ActivityApply"))%></td>
                        <td align="center"><%# SetUnit(Eval("CurrencyUnit"))%></td>
                        <td align="center"><%# SetMoney(Eval("ActivityMoney"))%></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsModify")) == true ? "变更活动" : "正常活动"%></td>
                        <td align="center"><%# Eval("CreateTime")%></td>
                        <td align="center"><%# Eval("StartTime")%></td>
                        <td align="center"><%# Eval("EndTime")%></td>
                        <td align="center"><%# Eval("ModifyTime") ?? "--"%></td>
                        <td align="center"><%# SetMoney(Eval("ModifyMoney"))%></td>
                        <td align="center"><%# Sub(Eval("ActivityDescribe"))%></td>
                        <td align="center"><%# Sub(Eval("ModifyDescribe"))%></td>
                        <td align="center">
                            <asp:HiddenField ID="hidActivityID" runat="server" Value='<%# Eval("ActivityID")%>' />
                            <asp:LinkButton ID="LinApply" runat="server" Visible='<%# Convert.ToInt32(Eval("ActivityApply")) == 1?true:false%>' CommandName="Apply" CommandArgument='<%# Eval("ActivityApply")%>' Text="申请活动变更"></asp:LinkButton>
                            <asp:LinkButton ID="LinLook" runat="server" CommandName="Look" CommandArgument='<%# Eval("IsModify")%>' Text="查看活动"></asp:LinkButton>
                            <asp:LinkButton ID="LinRegular" runat="server" CommandName="Regular" CommandArgument='<%# Eval("ActivitySubject")%>' Text="查看规则"></asp:LinkButton>
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
