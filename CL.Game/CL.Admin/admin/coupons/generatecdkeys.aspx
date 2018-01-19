<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="generatecdkeys.aspx.cs" Inherits="CL.Admin.admin.coupons.generatecdkeys" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>生成兑换码</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>生成兑换码</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">生成兑换码</a></li>
                    </ul>
                </div>
            </div>
        </div>


        <div class="tab-content" style="min-height: 1024px;">
            <dl>
                <dt>活动</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlActivity" runat="server">
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>合作商</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlProxy" runat="server">
                            <asp:ListItem Value="10">彩乐彩票</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>支持彩种</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlLotteryCode" runat="server">
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
                </dd>
            </dl>
            <dl>
                <dt>彩券类型</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlType" runat="server">
                            <asp:ListItem Value="0">固定时间段</asp:ListItem>
                            <asp:ListItem Value="1">固定时长</asp:ListItem>
                            <asp:ListItem Value="2">满减</asp:ListItem>
                            <asp:ListItem Value="3">永不过期</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl id="divSatisfiedMoney" style="display: none;">
                <dt>满减金额</dt>
                <dd>
                    <asp:TextBox ID="txtSatisfiedMoney" CssClass="input txt" placeholder="" runat="server" Text="100"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>彩券面值</dt>
                <dd>
                    <asp:TextBox ID="txtFaceValue" CssClass="input txt" runat="server" Text="5"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>有效开始时间</dt>
                <dd>
                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>兑换码失效时间</dt>
                <dd>
                    <asp:TextBox ID="txtKeyExpireTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>彩券失效时间</dt>
                <dd>
                    <asp:TextBox ID="txtExpireTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>是否允许赠送</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsGive" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否允许追号</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsChaseTask" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否可以叠加使用</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsSuperposition" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否允许多次使用</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsTimes" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否允许参与合买</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsJoinBuy" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>批量生成彩券个数</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlCouponsCount" runat="server">
                            <asp:ListItem Value="1">1</asp:ListItem>
                            <asp:ListItem Value="5">5</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="50">50</asp:ListItem>
                            <asp:ListItem Value="100">100</asp:ListItem>
                            <asp:ListItem Value="200">200</asp:ListItem>
                            <asp:ListItem Value="500">500</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <span style="color: red;">
                        <asp:Label ID="lbCouponsCount" runat="server" Text="大约1分钟内生成完成"></asp:Label>
                    </span>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="color: red;">
                    <asp:Label ID="lbMsg" runat="server"></asp:Label>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="开始生成兑换码" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->

        <script>
            $("#ddlType").change(function () {
                var item = parseInt($("#ddlType").val());
                if (item == 2) {
                    $("#divSatisfiedMoney").attr("style", "display:block");
                } else {
                    $("#divSatisfiedMoney").attr("style", "display:none");
                }
            });
            function verifySubmit() {
                var ActivityID = $("#ddlActivity").val();
                var PacketType = parseInt($("#ddlType").val());
                var SatisfiedMoney = parseInt($("#txtSatisfiedMoney").val());
                var FaceValue = parseInt($("#txtFaceValue").val());
                if (ActivityID == 0) {
                    $("#lbMsg").text("请选择活动");
                    return false;
                }
                if (PacketType == 2 && SatisfiedMoney == 0) {
                    $("#lbMsg").text("请填写满减金额");
                    return false;
                }
                if (FaceValue == 0) {
                    $("#lbMsg").text("请填写彩券面值");
                    return false;
                }
                $("#lbMsg").text("请稍候，兑换码生成中. . . ");
                return true;
            }
            $("#ddlCouponsCount").change(function () {
                var item = parseInt($("#ddlCouponsCount").val());
                if (item == 20) {
                    $("#lbCouponsCount").text("大约1分钟内生成完成");
                } else if (item == 50) {
                    $("#lbCouponsCount").text("大约需要2分钟左右生成时间");
                } else if (item == 100) {
                    $("#lbCouponsCount").text("大约需要3分钟左右生成时间");
                } else if (item == 500) {
                    $("#lbCouponsCount").text("大约需要4分钟左右生成时间");
                } else if (item == 1000) {
                    $("#lbCouponsCount").text("大约需要5分钟左右生成时间");
                }
            });
        </script>
    </form>
</body>
</html>
