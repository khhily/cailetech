<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="pay_list.aspx.cs" Inherits="CL.Admin.admin.withdraw.pay_list" %>

<%@ Import Namespace="CL.Tools.Common" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>充值记录</title>
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
            <span>充值记录</span>
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
                                <asp:ListItem Value="0">未成功</asp:ListItem>
                                <asp:ListItem Value="1">已成功</asp:ListItem>
                                <asp:ListItem Value="2">已退款</asp:ListItem>
                                <asp:ListItem Value="3">退款处理中</asp:ListItem>
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
                            <th align="center" width="10%">订单号</th>
                            <th align="center">接口交易号</th>
                            <th align="center" width="8%">充值类型</th>
                            <th align="center" width="8%">金额</th>
                            <th align="center" width="8%">手续费</th>
                            <th align="center" width="8%">状态</th>
                            <th align="center" width="10%">创建时间</th>
                            <th align="center" width="10%">处理时间</th>
                            <th width="8%">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("UserName") %>-(<%#Eval("UserMobile") %>)</td>
                        <td align="center"><%#Eval("OrderNo") %></td>
                        <td align="center"><%#Eval("RechargeNo") %></td>
                        <td align="center"><%#Eval("PayType") %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("Amount")) %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("FormalitiesFees")) %></td>
                        <td align="center" class="Status"><%#Eval("Result") %></td>
                        <td align="center"><%#string.Format("{0:g}", Eval("CreateTime"))%></td>
                        <td align="center"><%#string.Format("{0:g}", Eval("CompleteTime"))%></td>
                        <td align="center">
                            <a href="#" class="Replenishment" style="<%#Convert.ToInt32(Eval("Result")) == 0 ? "": "display:none;" %>" data-value="<%#Eval("PayID") %>">补单</a>
                            <%#OutputOperat(Convert.ToInt32(Eval("Result")), Eval("PayType").ToString(), Eval("UserID").ToString() + "," + Eval("PayID").ToString() + "," + Eval("Amount").ToString()) %>
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
                        $(this).text("未成功");
                        break;
                    case "1":
                        $(this).text("已成功");
                        break;
                    case "2":
                        $(this).text("已退款");
                        break;
                    case "3":
                        $(this).text("退款处理中");
                        break;
                }
            });

            $(".Refund").each(function () {
                $(this).click(function () {
                    var arr = $(this).attr("data-value").split(',');
                    var para = {
                        "UserID": arr[0],
                        "PayID": arr[1],
                        "RefundFee": arr[2]
                    };
                    $.post("../../tools/admin_ajax.ashx?action=Refund", para, function (data) {
                        var jsondata = JSON.parse(data);
                        if (jsondata.status == 0) {
                            alert(jsondata.msg);
                            window.location.reload();
                        }
                        else {
                            alert(jsondata.msg);
                        }
                    });
                });
            });
        });
        //补单
        $(".Replenishment").each(function () {
            var PayID = $(this).attr("data-value");
            $(this).click(function () {
                var winDialog = top.dialog({
                    title: '交易信息',
                    content: '接口交易号：&nbsp;&nbsp;<input id="txtRechargeNo" /></br>第三方交易号：&nbsp;&nbsp;<input id="txtOutRechargeNo" />',
                    okValue: '确定',
                    ok: function () {
                        var RechargeNo = $("#txtRechargeNo", parent.document).val();
                        var OutRechargeNo = $("#txtOutRechargeNo", parent.document).val();
                        if (RechargeNo == "") {
                            alert("请填写交易号")
                            return false;
                        }
                        if (OutRechargeNo == "") {
                            alert("请填写第三方交易号")
                            return false;
                        }
                        var postData = { "PayID": PayID, "RechargeNo": RechargeNo, "OutRechargeNo": OutRechargeNo };
                        //发送AJAX请求
                        sendAjaxUrl(winDialog, postData, "../../tools/admin_ajax.ashx?action=replenishment");
                        return false;
                    },
                    cancelValue: '取消',
                    cancel: function () { }
                }).showModal();
            });
        });
        //发送AJAX请求
        function sendAjaxUrl(winObj, postData, sendUrl) {
            $.ajax({
                type: "post",
                url: sendUrl,
                data: postData,
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    top.dialog({
                        title: '提示',
                        content: '尝试发送失败，错误信息：' + errorThrown,
                        okValue: '确定',
                        ok: function () { }
                    }).showModal(winObj);
                },
                success: function (data, textStatus) {
                    if (data.status == 0) {
                        alert(data.msg);
                        winObj.close().remove();
                        var d = dialog({ content: data.msg }).show();
                        setTimeout(function () { d.close().remove(); location.reload(); }, 2000);
                    } else {
                        top.dialog({
                            title: '提示',
                            content: '错误提示：' + data.msg,
                            okValue: '确定',
                            ok: function () { }
                        }).showModal(winObj);
                    }
                }
            });
        }
    </script>

</body>
</html>
