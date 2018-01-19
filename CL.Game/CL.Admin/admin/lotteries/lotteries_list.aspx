<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lotteries_list.aspx.cs" Inherits="CL.Admin.admin.lotteries.lotteries_list" %>

<%@ Import Namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>彩种列表</title>
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
            <span>彩种列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="add" href="lotteries_edit.aspx?action=<%=CaileEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
                            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                            <li>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" OnClick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
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
                            <th width="5%">选择</th>
                            <th align="left">彩种名称</th>
                            <th align="left" width="10%">彩种代码</th>
                            <th align="left" width="10%">最大追号期数</th>
                            <th align="left" width="10%">彩票类型</th>
                            <th align="left" width="10%">出票方式</th>
                            <th width="8%">重点推荐</th>
                            <th width="8%">是否加奖</th>
                            <th width="8%">是否停销</th>
                            <th width="8%">热门推荐</th>
                            <th width="5%">状态</th>
                            <th width="10%">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("LotteryID")%>' runat="server" />
                        </td>
                        <td><a href="lotteries_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("LotteryID")%>"><%# Eval("LotteryName") %></a></td>
                        <td><%# Eval("LotteryCode") %></td>
                        <td><%#Eval("MaxChaseIsuse") %></td>
                        <td class="typeid"><%#Eval("TypeID") %></td>
                        <td class="printouttype"><%#Eval("PrintOutType")%></td>
                        <td align="center"><%#Convert.ToBoolean(Eval("IsEmphasis")) ? "是" : "否"%></td>
                        <td align="center"><%#Convert.ToBoolean(Eval("IsAddaward")) ? "是" : "否"%></td>
                        <td align="center">
                            <a href="#" class="IsStop" data-value="<%#Eval("LotteryID") %>"><%#!Convert.ToBoolean(Eval("IsStop")) ? "正常" : "停销"%></a>
                            <input id="StopReason" type="hidden" value="<%#Eval("StopReason") %>" />
                        </td>
                        <td>
                            <a href="#" class="IsHot" data-value="<%#Eval("LotteryID") %>"><%#!Convert.ToBoolean(Eval("IsHot")) ? "非热门" : "热门推荐"%></a>
                            <input id="IsHot" type="hidden" value="<%#Eval("IsHot") %>" />
                        </td>
                        <td align="center"><%#Convert.ToBoolean(Eval("IsEnable")) ? "正常" : "禁用"%></td>
                        <td align="center">
                            <a href="lotteries_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("LotteryID")%>">修改</a> |
                  <a href="palytypes_list.aspx?LotteryCode=<%#Eval("LotteryCode")%>">玩法管理</a> |
                  <a href="wintypes_list.aspx?LotteryCode=<%#Eval("LotteryCode")%>">奖等管理</a>
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
            $(".typeid").each(function () {
                var typeid = $(this);
                switch (typeid.text()) {
                    case "1":
                        typeid.text("福彩");
                        break;
                    case "2":
                        typeid.text("体彩");
                        break;
                    case "3":
                        typeid.text("足彩");
                        break;
                    case "4":
                        typeid.text("高频彩");
                        break;
                }
            });

            $(".printouttype").each(function () {
                var printouttype = $(this);
                switch (printouttype.text()) {
                    case "0":
                        printouttype.text("本地出票");
                        break;
                    case "1":
                        printouttype.text("华阳电子票");
                        break;
                }
            });

            $(".IsStop").each(function () {
                var LotteryID = $(this).attr("data-value");
                $(this).click(function () {
                    var winDialog = top.dialog({
                        title: '停售原因',
                        content: '<textarea id="txtStopReason" name="txtStopReason" rows="2" cols="20" class="input">' + $("#StopReason").val() + '</textarea>',
                        okValue: '确定',
                        ok: function () {
                            var StopReason = $("#txtStopReason", parent.document).val();
                            //if (StopReason == "") {
                            //    alert("请输入停售原因")
                            //    return false;
                            //}
                            var postData = { "LotteryID": LotteryID, "StopReason": StopReason };
                            //发送AJAX请求
                            sendAjaxUrl(winDialog, postData, "../../tools/admin_ajax.ashx?action=Lotteries_IsStop");
                            return false;
                        },
                        cancelValue: '取消',
                        cancel: function () { }
                    }).showModal();
                });
            });
            //热门推荐
            $(".IsHot").each(function () {
                var LotteryID = $(this).attr("data-value");
                var IsHot = $('#IsHot').val();
                $(this).click(function () {
                    var winDialog = top.dialog({
                        title: "设置",
                        content: '',
                        okValue: '确定',
                        ok: function () {
                            var postData = { "LotteryID": LotteryID };
                            //发送AJAX请求
                            sendAjaxUrl(winDialog, postData, "../../tools/admin_ajax.ashx?action=Lotteries_IsHot");
                            return false;
                        },
                        cancelValue: '取消',
                        cancel: function () { }
                    }).showModal();
                });
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

    </script>
</body>
</html>
