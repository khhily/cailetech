<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="resultwinsetup.aspx.cs" Inherits="CL.Admin.admin.lotteries.resultwinsetup" %>

<%@ Import Namespace="CL.Tools" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>手动返奖</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <style>
        .ltable {
            width: 100%;
        }

            .ltable th, .ltable td {
                text-align: center;
            }
    </style>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>手动返奖</span>
        </div>
        <!--/导航栏-->
        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server"></asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input" placeholder="输入期号，如：20170306001"></asp:TextBox>
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;">获取接口返奖信息</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <span style="font-size: 14px; color: #686f7f">中奖电子票信息</span>
            <asp:CheckBoxList ID="cblNoResultWinSchemes" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow" Style="font-size: 14px; color: #686f7f; margin: 0 5px;">
                <asp:ListItem Value="0">派中奖方案</asp:ListItem>
            </asp:CheckBoxList>
            <asp:LinkButton ID="lbtnPayOutWin" runat="server" CssClass="btn" OnClick="lbtnPayOutWin_Click" Style="color: #ffffff;">重新派奖</asp:LinkButton>
        </div>
        <br />

        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table border="0" class="ltable">
                        <tr>
                            <th><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></th>
                            <th>用户昵称</th>
                            <th>绑定手机</th>
                            <th>方案编号</th>
                            <th>投注内容</th>
                            <th>投注倍数</th>
                            <th>投注金额</th>
                            <th>投注状态</th>
                            <th>接口返回状态</th>
                            <th>接口中奖金额</th>
                            <th>接口中奖金额(税前)</th>
                            <th>落地号</th>
                            <th>接口票号</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td>
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("SchemeETicketsID")%>' runat="server" />
                        </td>
                        <td><%# (Eval("UserName"))%></td>
                        <td><%# (Eval("UserMobile"))%></td>
                        <td><%# (Eval("SchemeNumber"))%></td>
                        <td><%# (Eval("Number"))%></td>
                        <td><%# (Eval("Multiple"))%></td>
                        <td><%# (Convert.ToInt64(Eval("TicketMoney"))/100).ToString("N")%></td>
                        <td><%# SetTicketStatus(Eval("TicketStatus"))%></td>
                        <td><%# InterfaceStatus(Eval("InterfaceStatus"))%></td>
                        <td><%# (Convert.ToInt64(Eval("WinMoneyNoWithTax"))/100).ToString("N")%></td>
                        <td><%# (Convert.ToInt64(Eval("WinMoney"))/100).ToString("N")%></td>
                        <td><%# (Eval("Identifiers"))%></td>
                        <td><%# (Eval("Ticket"))%></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"13\">暂无记录</td></tr>" : ""%>
                </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <!--/列表-->
        <!--内容底部-->
        <div class="line20"></div>
        <!--/内容底部-->
        <script>
            $(".all").click(function () {
                $(".checkall").attr("Checked", true);
            });
        </script>
    </form>
</body>
</html>
