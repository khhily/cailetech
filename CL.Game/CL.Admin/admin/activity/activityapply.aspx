<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activityapply.aspx.cs" Inherits="CL.Admin.admin.activity.activityapply" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>变更活动</title>

    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>变更活动</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">变更活动</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content" style="min-height: 1024px;">
            <dl>
                <dt>活动类型</dt>
                <dd>
                    <asp:TextBox ID="txtActivityType" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动主题</dt>
                <dd>
                    <asp:TextBox ID="txtActivitySubject" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动开始时间</dt>
                <dd>
                    <asp:TextBox ID="txtStartTime" CssClass="input grayr" TextMode="DateTime" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动结束时间</dt>
                <dd>
                    <asp:TextBox ID="txtEndTime" CssClass="input grayr" TextMode="DateTime" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动金额</dt>
                <dd>
                    <asp:TextBox ID="txtActivityMoney" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动币种</dt>
                <dd>
                    <asp:TextBox ID="txtCurrencyUnit" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>审核状态</dt>
                <dd>
                    <asp:TextBox ID="txtActivityApply" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动落地页</dt>
                <dd>
                    <asp:TextBox ID="txtLandingPage" CssClass="input grayr" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动描述</dt>
                <dd>
                    <asp:TextBox ID="txtActivityDescribe" CssClass="input grayr" TextMode="MultiLine" runat="server" Enabled="false"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd>
                    
                </dd>
            </dl>
            <dl>
                <dt>变更时间</dt>
                <dd>
                    <asp:TextBox ID="txtModifyTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>变更金额</dt>
                <dd>
                    <asp:TextBox ID="txtModifyMoney" runat="server" CssClass="input" TextMode="Number"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>变更描述</dt>
                <dd>
                    <asp:TextBox ID="txtModifyDescribe" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
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
                <asp:Button ID="btnSubmit" runat="server" Text="发布活动" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->

    </form>
</body>
</html>
