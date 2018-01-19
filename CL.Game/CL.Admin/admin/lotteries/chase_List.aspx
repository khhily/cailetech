<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chase_List.aspx.cs" Inherits="CL.Admin.admin.lotteries.chase_List" %>

<%@ Import Namespace="CL.Tools.Common" %>
<%@ Import Namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>追号查询</title>
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
            <span>追号查询</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul>
                            <li style="float: left;">
                                <div class="rule-single-select">
                                    <asp:DropDownList ID="ddlType" runat="server">
                                        <asp:ListItem Value="0">按彩种查询</asp:ListItem>
                                        <asp:ListItem Value="1">按用户名查询</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </li>
                            <li id="phIsuses" style="float: left; <%=iType ==0 ? "": "display:none" %>">
                                <div class="rule-single-select">
                                    <asp:DropDownList ID="ddlLotteryCode" runat="server"></asp:DropDownList>
                                </div>
                                <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input txt" placeholder="输入期号" />
                            </li>
                            <li id="phUserName" style="float: left; <%=iType ==1 ? "": "display:none" %>">
                                <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="输入用户名" />
                            </li>
                            <li style="float: left;">
                                <div class="rule-single-select">
                                    <asp:DropDownList ID="ddlChaseStatus" runat="server">
                                        <asp:ListItem Value="0">全部</asp:ListItem>
                                        <asp:ListItem Value="1">进行中</asp:ListItem>
                                        <asp:ListItem Value="2">已中止</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </li>
                            <li style="float: left;">
                                <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                                <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                            </li>
                            <li style="float: left;">
                                <asp:TextBox ID="txtSchemeNumber" runat="server" CssClass="input txt" placeholder="方案编号" Style="margin-left: 35px;" />
                            </li>
                        </ul>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                    </div>
                </div>
                <br />
                <div class="l-list">总金额：<span style='color: red; font-weight: bold;'><%=Utils.StrToDouble(SumMoney) %></span></div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="10%">用户名</th>
                            <th width="8%">方案编号</th>
                            <th width="10%">发起时间</th>
                            <th width="8%">彩种</th>
                            <th width="8%">投注总额</th>
                            <th width="8%">总期数</th>
                            <th width="8%">完成期数</th>
                            <th>中止条件</th>
                            <th width="10%">状态</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("UserName") %></td>
                        <td align="center"><a href="schemeChaseTask_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("SchemeID")%>"><%# Eval("SchemeNumber") %></a></td>
                        <td align="center"><%#string.Format("{0:g}",Eval("CreateTime"))%></td>
                        <td align="center"><%#Eval("LotteryName") %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("SchemeMoney")) %></td>
                        <td align="center"><%#Eval("IsuseCount") %></td>
                        <td align="center"><%#Eval("BuyedIsuseNum") %></td>
                        <td align="center">
                            <%#SetStopTypeName(Convert.ToInt64(Eval("StopTypeWhenWinMoney")))%>
                        </td>
                        <%--<td align="center" class="StopTypeWhenWinMoney" data-value="<%# Eval("StopTypeWhenWin") %>"><%#Eval("StopTypeWhenWinMoney") %></td>--%>
                        <td align="center"><%#Convert.ToInt32(Eval("SchemeStatus"))==19?"<font color='red'>追号中</font>":"追号完成" %></td>
                        <%--<td align="center" class="QuashStatus" data-value="<%#Eval("IsuseCount") %>,<%#Eval("BuyedIsuseNum") %>"><%#Eval("QuashStatus") %></td>--%>
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
        $(function () {
            $(".QuashStatus").each(function () {
                var QuashStatus = $(this);
                var val = $(this).text();
                var Arr = $(this).attr("data-value").split(',');
                var SumIsuseNum = Arr[0];
                var BuyedIsuseNum = Arr[1];

                if (val == "1") {
                    QuashStatus.text("已终止");
                }
                else {
                    if (SumIsuseNum == BuyedIsuseNum) {
                        QuashStatus.text("已完成");
                    } else {
                        QuashStatus.html("<font color='red'>进行中</font>");
                    }
                }
            });

            $(".StopTypeWhenWinMoney").each(function () {
                var val = $(this).val();
                var stoptype = $(this).attr("data-value");
                if (stoptype == 1 || val == 0)
                    $(this).text("完成方案");
                else {
                    $(this).text("单期中奖金额达到" + val + "元");
                }
            });

            $("#ddlType").change(function () {
                var itype = $(this).find("option:selected").attr("value");
                if (itype == 0) {
                    $("#phIsuses").show();
                    $("#phUserName").hide();
                } else {
                    $("#phIsuses").hide();
                    $("#phUserName").show();
                }
            });
        });
    </script>
</body>
</html>
