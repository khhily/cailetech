﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="salepointRecord.aspx.cs" Inherits="CL.Admin.admin.lotteries.salepointRecord" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>点位查询</title>
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
            <span>点位查询</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlTicketSource" runat="server" OnSelectedIndexChanged="ddlTicketSource_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="-1">全部</asp:ListItem>
                                <asp:ListItem Value="0">彩乐</asp:ListItem>
                                <asp:ListItem Value="1">华阳</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server" OnSelectedIndexChanged="ddlLotteryCode_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" style="margin-left:35px"/>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;margin-left:70px">查询</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server" >
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="7%">出票商</th>
                            <th width="7%">彩种</th>
                            <th width="10%">销售阶梯、点位</th>
                            <th width="7%">销售阶梯、点位（变更前）</th>
                            <th width="10%">生效时间</th>
                            <th width="7%">创建时间</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Convert.ToInt32(Eval("TicketSourceID"))==0?"彩乐":"华阳" %></td>
                        <td align="center"><%#SetLotteryName(Convert.ToInt32(Eval("LotteryCode"))) %></td>
                        <td align="center"><%#GetHTML(Eval("SalesRebate")) %></td>
                        <td align="center"><%#GetHTML(Eval("LastSalesRebate")) %></td>
                        <td align="center"><%#Eval("StartTime") %></td>
                        <td align="center"><%#Eval("CreateTime") %></td>
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

    <script>
        

    </script>
</body>
</html>
