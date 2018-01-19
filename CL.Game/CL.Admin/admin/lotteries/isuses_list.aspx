<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="isuses_list.aspx.cs" Inherits="CL.Admin.admin.lotteries.isuses_list" %>

<%@ Import Namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>期号列表</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <%--<script type="text/javascript" src="../../scripts/lhgdialog/lhgdialog.js?skin=idialog"></script>--%>
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
            <span>期号列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="add" href="#"><i></i><span>新增</span></a></li>
                            <li>
                                <div style="font-size: 14px; font-weight: bold; padding: 10px 5px;"><%=LastIsuse %></div>
                            </li>
                            <%--<li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                  <li><asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" onclick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>--%>
                        </ul>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server" datatype="*" errormsg="请选择彩种..." sucmsg=" " OnSelectedIndexChanged="ddlLotteryCode_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtDate" runat="server" OnTextChanged="txtDate_TextChanged" AutoPostBack="True" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" " />
                    </div>
                    <div class="r-list">
                        <asp:TextBox ID="txtKeywords" runat="server" placeholder="请输入期号" CssClass="keyword" />
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
                            <th>彩种</th>
                            <th>期号名称</th>
                            <th>开始时间</th>
                            <th>截止时间</th>
                            <th>开奖号码</th>
                            <th>状态</th>
                            <th>操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# SetLotteryName(Convert.ToInt32(Eval("LotteryCode"))) %></td>
                        <td align="center"><a href="isuses_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("IsuseID")%>"><%# Eval("IsuseName") %></a></td>
                        <td align="center"><%#string.Format("{0:G}",Eval("StartTime"))%></td>
                        <td align="center"><%#string.Format("{0:G}",Eval("EndTime"))%></td>
                        <td align="center"><%#Eval("OpenNumber") %></td>
                        <td align="center" class="isusestate"><%#Eval("IsuseState") %></td>
                        <td align="center">
                            <a href="isuses_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("IsuseID")%>">修改</a>
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
            //初始化导入
            $(".add").click(function () {
                showIsuseDialog();
            });

            $(".isusestate").each(function () {
                var isusestate = $(this);
                switch (isusestate.text()) {
                    case "0":
                        isusestate.text("未开启");
                        break;
                    case "1":
                        isusestate.text("开始");
                        break;
                    case "2":
                        isusestate.text("暂停");
                        break;
                    case "3":
                        isusestate.text("截止");
                        break;
                    case "4":
                        isusestate.text("期结");
                        break;
                    case "5":
                        isusestate.text("返奖");
                        break;
                    case "6":
                        isusestate.text("返奖结束");
                        break;
                }
            });
        });

        //初始化窗口
        function showIsuseDialog() {
            var LotteryCode = $("#ddlLotteryCode").find("option:selected").attr("value");
            if (LotteryCode == 0) {
                alert("请选择彩种");
                return;
            }

            var StrUrl = 'dialog/dialog_AddIsuse.aspx?LotteryCode=' + LotteryCode;
            if (LotteryCode == "801" || LotteryCode == "901")
                StrUrl = 'dialog/dialog_AddIsuseFC.aspx?LotteryCode=' + LotteryCode;
            else if (LotteryCode == "40010")
                StrUrl = 'dialog/dialog_AddIsuseXYEB.aspx?LotteryCode=' + LotteryCode;
            else if (LotteryCode == "40020")
                StrUrl = 'dialog/dialog_AddIsuseJNDEB.aspx?LotteryCode=' + LotteryCode;

            var attachDialog = top.dialog({
                id: 'AddIsuseId',
                title: "批量添加期号",
                url: StrUrl,
                width: 500,
                height: 250,
                data: window,
                onclose: function () {
                    var liHtml = this.returnValue; //获取返回值
                    if (liHtml.length > 0) {
                        alert(liHtml)
                        window.location.reload();
                    }
                }
            }).showModal();
        }

        //发送AJAX请求
        function sendAjaxUrl(winObj, para) {
            $.ajax({
                type: "post",
                url: "../../tools/admin_ajax.ashx?action=create_isuse",
                data: para,
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
                        setTimeout(function () {
                            d.close().remove();
                            location.reload(); //刷新页面
                        }, 1000);
                    } else {
                        top.dialog({
                            title: '提示',
                            content: '创建期号失败！',
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
