<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="audit.aspx.cs" Inherits="CL.Admin.admin.withdraw.audit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>提现审核</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
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
            <span>管理员列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                            <li>
                                <asp:LinkButton ID="btnAudit" runat="server" OnClick="btnAudit_Click" CssClass="audit"><i></i><span>审核</span></asp:LinkButton></li>
                        </ul>
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
                            <th width="8%">选择</th>
                            <th align="left">用户名</th>
                            <th align="left">真实姓名</th>
                            <th align="left">身份证号</th>
                            <th align="left">银行名称</th>
                            <th align="left">银行卡号</th>
                            <th align="left">提现金额</th>
                            <th align="left">申请时间</th>
                            <th align="left">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Enabled='<%#Eval("PayOutID").ToString() != "1" %>' Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("PayOutID")%>' runat="server" />
                        </td>
                        <td><%# Eval("UserName") %></td>
                        <td><%# Eval("FullName") %></td>
                        <td><%# Eval("IDNumber") %></td>
                        <td><%# GetEnumValue(Convert.ToInt32(Eval("BankType"))) %></td>
                        <%--<td><%#CL.Enum.Common.GetDescription((CL.Enum.BankType)Eval("BankType")) %></td>--%>
                        <td><%# Eval("CardNumber") %></td>
                        <td><%# GetAmount(Convert.ToInt64(Eval("Amount"))) %></td>
                        <td><%# Eval("CreateTime") %></td>
                        <td align="center">
                            <a href="javascript:sendUrl(<%#Eval("PayOutID")%>)">审核</a>
                            <a href="#" class="Refuse" data-value="<%#Eval("PayOutID") %>">拒绝</a>
                            <input id="Remark" type="hidden" value="<%#Eval("Remark") %>" />
                        </td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"9\">暂无记录</td></tr>" : ""%>
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
        $(".Refuse").each(function () {
            var PayOutID = $(this).attr("data-value");
            $(this).click(function () {
                var winDialog = top.dialog({
                    title: '拒绝原因',
                    content: '<textarea id="txtRemark" name="txtRemark" rows="2" cols="20" class="input">' + $("#Remark").val() + '</textarea>',
                    okValue: '确定',
                    ok: function () {
                        var Remark = $("#txtRemark", parent.document).val();
                        if (Remark == "") {
                            alert("请输入拒绝原因")
                            return false;
                        }
                        var postData = { "PayOutID": PayOutID, "Remark": Remark };
                        //发送AJAX请求
                        sendAjaxUrl(winDialog, postData, "../../tools/admin_ajax.ashx?action=withdraw&IsRefuse=2");
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

        //发送AJAX请求
        function sendUrl(id) {
            $.ajax({
                type: "get",
                url: "../../tools/admin_ajax.ashx?action=withdraw&IsRefuse=1&PayOutID=" + id,
                data: null,
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    top.dialog({
                        title: '提示',
                        content: '尝试发送失败，错误信息：' + errorThrown,
                        okValue: '确定',
                        ok: function () { }
                    });
                },
                success: function (data, textStatus) {
                    if (data.status == 0) {
                        var d = dialog({ content: data.msg }).show();
                        setTimeout(function () { d.close().remove(); location.reload(); }, 2000);
                    } else {
                        top.dialog({
                            title: '提示',
                            content: '错误提示：' + data.msg,
                            okValue: '确定',
                            ok: function () { }
                        });
                    }
                }
            });
        }
    </script>
</body>
</html>
