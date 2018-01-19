<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="couponslist.aspx.cs" Inherits="CL.Admin.admin.coupons.couponslist" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>彩券列表</title>
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
            <span>彩券列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server">
                                <asp:ListItem Value="-1">全部彩种</asp:ListItem>
                                <asp:ListItem Value="0">全场通用</asp:ListItem>
                                <asp:ListItem Value="101">红快三</asp:ListItem>
                                <asp:ListItem Value="102">赢快三</asp:ListItem>
                                <asp:ListItem Value="201">华11选5</asp:ListItem>
                                <asp:ListItem Value="202">老11选5</asp:ListItem>
                                <asp:ListItem Value="301">老时时彩</asp:ListItem>
                                <asp:ListItem Value="801">双色球</asp:ListItem>
                                <asp:ListItem Value="901">超级大乐透</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlState" runat="server">
                                <asp:ListItem Value="-1">全部状态</asp:ListItem>
                                <asp:ListItem Value="0">未发放</asp:ListItem>
                                <asp:ListItem Value="1">已发放</asp:ListItem>
                                <asp:ListItem Value="2">未使用</asp:ListItem>
                                <asp:ListItem Value="3">已使用</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlType" runat="server">
                                <asp:ListItem Value="-1">全部类型</asp:ListItem>
                                <asp:ListItem Value="0">固定时间段</asp:ListItem>
                                <asp:ListItem Value="1">固定时长</asp:ListItem>
                                <asp:ListItem Value="2">满减</asp:ListItem>
                                <asp:ListItem Value="3">永不过期</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlSource" runat="server">
                                <asp:ListItem Value="-1">全部来源</asp:ListItem>
                                <asp:ListItem Value="1">平台活动发放</asp:ListItem>
                                <asp:ListItem Value="2">平台赠送</asp:ListItem>
                                <asp:ListItem Value="3">任务获得,游戏获得</asp:ListItem>
                                <asp:ListItem Value="4">线下推广二维码</asp:ListItem>
                                <asp:ListItem Value="5">会员赠送</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        失效时间：<asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />-
                        <asp:TextBox ID="txtExpireTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="输入用户名" Style="margin-left: 60px;" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="lbtnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                    </div>
                </div>
                <br />
            </div>
        </div>
        <!--/工具栏-->
        <!--列表-->
        <div class="table-container">
            彩券发放总额:<%=RecordFaceValue/100 %> &nbsp;&nbsp;&nbsp; 彩券使用总额:<%=RecordEmploy/100 %>
            <asp:Repeater ID="rptList" runat="server">
                <%--OnItemCommand="rptList_ItemCommand"--%>
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="100">活动标题</th>
                            <th width="100">用户昵称</th>
                            <th width="100">用户号码</th>
                            <th width="100">生成时间</th>
                            <th width="100">发放时间</th>
                            <th width="100">初次使用</th>
                            <th width="100">最后使用</th>
                            <th width="100">开始使用时间</th>
                            <th width="100">失效时间</th>
                            <th width="100">面值</th>
                            <th width="100">余额</th>
                            <th width="100">满减</th>
                            <th width="100">允许赠送</th>
                            <th width="100">允许追号</th>
                            <th width="100">叠加使用</th>
                            <th width="100">多次使用</th>
                            <th width="100">允许合买</th>
                            <th width="100">使用场景</th>
                            <th width="100">彩券状态</th>
                            <th width="100">彩券类型</th>
                            <th width="100">彩券来源</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Eval("ActivitySubject") %></td>
                        <td align="center"><%# Eval("UserName")??"--" %></td>
                        <td align="center"><%# Eval("UserMobile")??"--" %></td>
                        <td align="center"><%# Eval("GenerateTime") %></td>
                        <td align="center"><%# Eval("ReleaseTime")??"未发放" %></td>
                        <td align="center"><%# Eval("FirstTime")??"未使用" %></td>
                        <td align="center"><%# Eval("LastTime")??"未使用" %></td>
                        <td align="center"><%# Eval("StartTime") %></td>
                        <td align="center"><%# Eval("ExpireTime") %></td>
                        <td align="center"><%# Convert.ToInt64(Eval("FaceValue"))/100 %></td>
                        <td align="center"><%# Convert.ToInt64(Eval("Balance"))/100 %></td>
                        <td align="center"><%# Convert.ToInt64(Eval("SatisfiedMoney"))/100 %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsGive"))==true?"允许赠送":"禁止赠送" %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsChaseTask"))==true?"允许追号":"禁止追号" %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsSuperposition"))==true?"允许叠加":"禁止叠加" %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsTimes"))==true?"允许多次":"单次使用" %></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsJoinBuy"))==true?"允许合买":"禁止合买" %></td>
                        <td align="center"><%# SetLotteryCode(Convert.ToInt32(Eval("LotteryCode"))) %></td>
                        <td align="center"><%# SetCouponsStatus(Convert.ToInt32(Eval("CouponsStatus"))) %></td>
                        <td align="center"><%# SetCouponsType(Convert.ToInt32(Eval("CouponsType"))) %></td>
                        <td align="center"><%# SetCouponsSource(Convert.ToInt32(Eval("CouponsSource"))) %></td>
                        <td>
                            <input id="HidCouponsID" type="hidden" value='<%# Eval("CouponsID") %>' runat="server" />
                            <input id="HidActivityID" type="hidden" value='<%# Eval("ActivityID") %>' runat="server" />
                            <input id="HidUserID" type="hidden" value='<%# Eval("UserID") %>' runat="server" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"21\">暂无记录</td></tr>" : ""%>
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
