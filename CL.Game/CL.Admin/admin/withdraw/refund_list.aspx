<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="refund_list.aspx.cs" Inherits="CL.Admin.admin.withdraw.refund_list" %>

<%@ Import Namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>退款记录</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
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
            <span>退款记录</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlType" runat="server" datatype="*" errormsg="请状态类型..." sucmsg=" " OnSelectedIndexChanged="ddlType_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="-1">所有</asp:ListItem>
                                <asp:ListItem Value="10">处理中</asp:ListItem>
                                <asp:ListItem Value="1">退款成功</asp:ListItem>
                                <asp:ListItem Value="2">退款失败</asp:ListItem>
                                <%--<asp:ListItem Value="3">失败可重复发</asp:ListItem>
                                <asp:ListItem Value="4">退款需要工人线下干预</asp:ListItem>--%>
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="r-list">
                        <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">查询</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th align="center" width="15%">用户账号</th>
                            <th align="center" width="10%">退款单号</th>
                            <th align="center" width="10%">退款交易号</th>
                            <th align="center" width="8%">退款金额</th>
                            <th align="center" width="8%">手续费</th>
                            <th align="center">处理结果</th>
                            <th align="center" width="10%">提交时间</th>
                            <th align="center" width="10%">完成时间</th>
                            <th width="8%">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("UserName") %>-(<%#Eval("UserMobile") %>)</td>
                        <td align="center"><%#Eval("RefundNo") %></td>
                        <td align="center"><%#Eval("RechargeNo") %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("Amount")) %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("FormalitiesFees")) %></td>
                        <td align="center" class="Status"><%#Eval("Result") %></td>
                        <td align="center"><%#string.Format("{0:g}", Eval("CreateTime"))%></td>
                        <td align="center"><%#string.Format("{0:g}", Eval("CompleteTime"))%></td>
                        <td align="center">
                            <%#OutputOperat(Convert.ToInt32(Eval("Result")), Convert.ToInt64(Eval("ReID"))) %>
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
    <script>
        $(function () {
            $(".Status").each(function () {
                var val = $(this).text();
                switch (val) {
                    case "0":
                        $(this).text("退款处理中");
                        break;
                    case "1":
                        $(this).text("退款成功");
                        break;
                    case "2":
                        $(this).text("退款失败");
                        break;
                    case "3":
                        $(this).text("退款失败,可重复退款");
                        break;
                    case "4":
                        $(this).text("退款需要工人线下干预");
                        break;
                }
            });

            $(".Refund").each(function () {
                $(this).click(function () {
                    var val = $(this).attr("data-value")
                    var para = {
                        "ReID": val
                    };
                    $.post("../../tools/admin_ajax.ashx?action=PostRefundQuery", para, function (data) {
                        var jsondata = JSON.parse(data);
                        if (jsondata.status == 0) {
                            alert(jsondata.msg);
                            if (jsondata.msg == "退款成功")
                                window.location.reload();
                        }
                        else {
                            alert(jsondata.msg);
                        }
                    });
                });
            });
        });
    </script>
</body>
</html>
