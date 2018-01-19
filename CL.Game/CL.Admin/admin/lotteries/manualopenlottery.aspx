<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="manualopenlottery.aspx.cs" Inherits="CL.Admin.admin.lotteries.manualopenlottery" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>手动开奖</title>
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
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="lotteries_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="lotteries_list.aspx"><span>手动开奖</span></a>
            <%--<i class="arrow"></i>--%>
            <%--<span>编辑内容</span>--%>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <%--<div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server"></asp:DropDownList>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>


        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">彩种期次开奖(不同身份开奖同一期次，开奖内容相同则开奖成功)</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content" style="height: 500px;">
            <dl>
                <dt>彩种选择</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlLotteryCode" runat="server"></asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>期号</dt>
                <dd>
                    <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input normal" MaxLength="15" />
                </dd>
            </dl>
            <dl>
                <dt>开奖号码</dt>
                <dd>
                    <asp:TextBox ID="txtOpenNumber" runat="server" CssClass="input normal" MaxLength="30" /><span style="color: #dbdbdb;">(开奖号码之间请用一位空格隔开)</span>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="color:#ff0000;">
                    <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label>
                </dd>
            </dl>
            <!--/内容-->

            <!--工具栏-->
            <div class="page-footer">
                <div class="btn-wrap">
                    <asp:Button ID="btnSubmit" runat="server" Text="开奖" CssClass="btn" OnClick="btnSubmit_Click" />
                </div>
            </div>
            <!--/工具栏-->
        </div>
    </form>
</body>
</html>
