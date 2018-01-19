<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="buylotdetail.aspx.cs" Inherits="CL.Admin.admin.report.finance.detail.buylotdetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>彩票投注详情</title>
    <link href="../../../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../../../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../../js/common.js"></script>
    <style>
        input {
            width: 200px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../../../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>彩票投注详情</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">彩票投注详情</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content" style="min-height: 1024px;">
            <dl>
                <dt>彩种</dt>
                <dd>
                    <asp:TextBox ID="txtLottery" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>订单编号</dt>
                <dd>
                    <asp:TextBox ID="txtSchemesNumber" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>投注金额</dt>
                <dd>
                    <asp:TextBox ID="txtSchemeMoney" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>支付方式</dt>
                <dd>
                    <asp:TextBox ID="txtModePay" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>用户ID</dt>
                <dd>
                    <asp:TextBox ID="txtUserID" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>用户账号</dt>
                <dd>
                    <asp:TextBox ID="txtUserName" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>下单时间</dt>
                <dd>
                    <asp:TextBox ID="txtCreateTime" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>订单状态</dt>
                <dd>
                    <asp:TextBox ID="txtSchemeStatus" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>出票商</dt>
                <dd>
                    <asp:TextBox ID="txtSoure" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>出票详情列表</dt>
                <dd>
                    <asp:Repeater ID="rptList" runat="server">
                        <HeaderTemplate>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                                <tr>
                                    <th>电子票编号</th>
                                    <th>出票接口</th>
                                    <th>投注金额</th>
                                    <th>投注玩法</th>
                                    <th>投注倍数</th>
                                    <th>投注内容</th>
                                    <th>出票状态</th>
                                    <th>电子票号</th>
                                    <th>中奖金额</th>
                                </tr>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td align="center"><%# (Eval("SchemeETicketsID"))%></td>
                                <td align="center"><%# SetOutType(Eval("TicketSourceID"))%></td>
                                <td align="center"><%# Convert.ToInt64(Eval("TicketMoney"))/100%></td>
                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                <td align="center"><%# (Eval("Multiple"))%></td>
                                <td align="center"><%# (Eval("Number"))%></td>
                                <td align="center"><%# SetTicketStatus(Eval("TicketStatus"))%></td>
                                <td align="center"><%# (Eval("Ticket")??"--")%></td>
                                <td align="center"><%# Convert.ToInt64(Eval("WinMoney"))/100%></td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                        </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </dd>
            </dl>
        </div>
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
