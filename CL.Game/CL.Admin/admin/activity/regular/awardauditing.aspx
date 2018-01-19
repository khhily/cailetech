<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="awardauditing.aspx.cs" Inherits="CL.Admin.admin.activity.regular.awardauditing" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>活动规则审核</title>
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
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>活动规则审核</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap" style="min-height: 1024px;">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li>
                            <label style="cursor: pointer;">活动详情</label></li>
                    </ul>
                </div>
            </div>
            <div class="tab-content" style="min-height: 300px;">
                <dl>
                    <dt>活动类型
                    </dt>
                    <dd>
                        <label id="lbActivityType" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>活动主题
                    </dt>
                    <dd>
                        <label id="lbActivitySubject" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>活动预算
                    </dt>
                    <dd>
                        <label id="lbActivityMoney" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>预算币种
                    </dt>
                    <dd>
                        <label id="lbCurrencyUnit" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>活动落地页
                    </dt>
                    <dd>
                        <a id="linkLandingPage" target="_blank" runat="server"></a>
                    </dd>
                </dl>
                <dl>
                    <dt>申请时间
                    </dt>
                    <dd>
                        <label id="lbCreateTime" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>活动开始时间
                    </dt>
                    <dd>
                        <label id="lbStartTime" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>活动结束时间
                    </dt>
                    <dd>
                        <label id="lbEndTime" runat="server"></label>
                    </dd>
                </dl>
                <dl>
                    <dt>应用广告图
                    </dt>
                    <dd>
                        <asp:Image ID="imgADUrl" runat="server" Width="1020" Height="225" />
                    </dd>
                </dl>
                <dl>
                    <dt>加奖活动规则
                    </dt>
                    <dd>
                        <asp:Repeater ID="rptRegularList" runat="server" OnItemDataBound="rptRegularList_ItemDataBound" OnItemCommand="rptRegularList_ItemCommand">
                            <HeaderTemplate>
                                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                                    <tr style="color: #808080;">
                                        <th>规则类型</th>
                                        <th>活动彩种</th>
                                        <th>累计派奖</th>
                                        <th>操作</th>
                                    </tr>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <tr>
                                    <td align="center"><%# SetType(Eval("RegularType"))%></td>
                                    <td align="center"><%# SetLot(Eval("LotteryCode"))%></td>
                                    <td align="center"><%# SetMoney(Eval("TotalAwardMoney"))%></td>
                                    <td>
                                        <asp:HiddenField ID="hidRegularID" runat="server" Value='<%# Eval("RegularID")%>' />
                                        <asp:HiddenField ID="hidArgument" runat="server" Value="Open" />
                                        <asp:LinkButton ID="LinRegular" runat="server" ForeColor="Blue" CommandName="Regular" Text="展开规则详情"></asp:LinkButton>
                                    </td>
                                </tr>
                                <tr>
                                    <!--0-->
                                    <asp:Repeater ID="rptList_0" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>单次加奖金额</th>
                                                <th>上限金额</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--1-->
                                    <asp:Repeater ID="rptList_1" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>中奖类型</th>
                                                <th>单位(次或金额)</th>
                                                <th>单次加奖金额</th>
                                                <th>上限金额</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# Convert.ToInt32(Eval("RChaseType"))==1?"连续中奖次数":"累计中奖金额"%></td>
                                                <td align="center"><%# (Eval("Unit"))%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--2-->
                                    <asp:Repeater ID="rptList_2" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>胆码数</th>
                                                <th>拖码数</th>
                                                <th>单次加奖金额</th>
                                                <th>上限金额</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# Eval("DanNums")%></td>
                                                <td align="center"><%# (Eval("TuoNums"))%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--5-->
                                    <asp:Repeater ID="rptList_5" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>区间规则</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# SetInterval(Eval("BetInterval"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--6-->
                                    <asp:Repeater ID="rptList_6" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>区间规则</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# SetInterval(Eval("AwardInterval"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--7-->
                                    <asp:Repeater ID="rptList_7" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>名次规则</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# SetRanking(Eval("BetRanking"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--8-->
                                    <asp:Repeater ID="rptList_8" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>名次规则</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# SetRanking(Eval("AwardRanking"))%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--9-->
                                    <asp:Repeater ID="rptList_9" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>选球类型</th>
                                                <th>指定选号</th>
                                                <th>单次加奖金额</th>
                                                <th>上限金额</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# Convert.ToInt32(Eval("BallType"))==1?"指定红球数字":"指定蓝球数字"%></td>
                                                <td align="center"><%# Eval("Ball")%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("TopLimit"))/100%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                    <!--10-->
                                    <asp:Repeater ID="rptList_10" runat="server" Visible="false">
                                        <HeaderTemplate>
                                            <tr>
                                                <th>加奖玩法</th>
                                                <th>类型</th>
                                                <th>单次加奖金额</th>
                                                <th>单日上限金额</th>
                                            </tr>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <tr>
                                                <td align="center"><%# SetPlayCode(Eval("PlayCode"))%></td>
                                                <td align="center"><%# SetHolidayType(Eval("HolidayType"))%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("AwardMoney"))/100%></td>
                                                <td align="center"><%# Convert.ToInt64(Eval("TopLimitDay"))/100%></td>
                                            </tr>
                                        </ItemTemplate>
                                    </asp:Repeater>
                                </tr>
                            </ItemTemplate>
                            <FooterTemplate>
                                <%#rptRegularList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
                            </table>
                            </FooterTemplate>
                        </asp:Repeater>
                    </dd>
                </dl>
            </div>
        </div>
        <!--/内容-->
        <!--工具栏-->
        <div class="page-footer" style="margin-bottom: auto;">
            <div class="btn-wrap">
                <asp:Button ID="btnAuditing" runat="server" Text="通过" CssClass="btn" OnClick="btnAuditing_Click" />
                <asp:Button ID="btnRefuse" runat="server" Text="拒绝" CssClass="btn yellow" OnClick="btnRefuse_Click" />
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
