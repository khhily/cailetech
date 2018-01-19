<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scheme_list.aspx.cs" Inherits="CL.Admin.admin.lotteries.scheme_list" %>

<%@ Import Namespace="CL.Enum.Common" %>
<%@ Import Namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>投注查询</title>
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
            <span>投注查询</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlLotteryCode" runat="server" OnSelectedIndexChanged="ddlLotteryCode_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        </div>
                        <%--<div class="rule-single-select"> </div>--%>
                        <asp:DropDownList ID="ddlState" runat="server" OnSelectedIndexChanged="ddlState_SelectedIndexChanged" AutoPostBack="True" Visible="false">
                            <%--<asp:ListItem Value="0">全部</asp:ListItem>
                          <asp:ListItem Value="1">招募中</asp:ListItem>--%>
                            <asp:ListItem Value="2">出票中</asp:ListItem>
                            <asp:ListItem Value="3">已出票</asp:ListItem>
                            <%--<asp:ListItem Value="4">已撤单</asp:ListItem>
                          <asp:ListItem Value="5">已流单</asp:ListItem>--%>
                        </asp:DropDownList>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlWinState" runat="server" OnSelectedIndexChanged="ddlWinState_SelectedIndexChanged" AutoPostBack="True">
                                <asp:ListItem Value="0">全部</asp:ListItem>
                                <asp:ListItem Value="1">未开奖</asp:ListItem>
                                <asp:ListItem Value="2">未中奖</asp:ListItem>
                                <asp:ListItem Value="3">已中奖</asp:ListItem>
                                <asp:ListItem Value="4">已退款</asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                        <asp:TextBox ID="txtKeywords" runat="server" CssClass="input txt" placeholder="输入方案号" Style="margin-left: 30px;" />
                        <asp:TextBox ID="txtUserName" runat="server" CssClass="input txt" placeholder="输入用户名" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" OnClick="btnSearch_Click" Style="color: #ffffff;">查询</asp:LinkButton>
                    </div>
                </div>
                <br />
                <div class="l-list">
                    方案总金额：<span style='color: red; font-weight: bold;'><%=Utils.StrToDouble(SumMoney) %></span> 元	
                方案税后总奖金：<span style='color: red; font-weight: bold;'><%=Utils.StrToDouble(WinSumMoney) %></span> 元	
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <%--OnItemCommand="rptList_ItemCommand"--%>
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th width="8%">彩种</th>
                            <th width="10%">方案号</th>
                            <th width="8%">期号</th>
                            <th width="8%">发起人</th>
                            <th width="10%">玩法</th>
                            <th width="6%">方案总额</th>
                            <th width="6%">奖金金额</th>
                            <th width="10%">投注时间</th>
                            <th width="8%">状态</th>
                            <th width="10%">投注类型</th>
                            <th width="8%">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%# Eval("LotteryName") %></td>
                        <td align="center"><a href="<%#Convert.ToInt32(Eval("BuyType"))==1?"schemeChaseTask_edit":"scheme_edit"%>.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("SchemeID")%>"><%# Eval("SchemeNumber") %></a></td>
                        <td align="center"><%#Eval("IsuseName") %></td>
                        <td align="center"><%#Eval("UserName") %></td>
                        <td align="center"><%#Eval("PlayName") %></td>
                        <td align="center"><%#Utils.StrToDouble(Eval("SchemeMoney")??0) %></td>
                        <td align="center"><%#Convert.ToInt32(Eval("BuyType"))==1?0:Utils.StrToDouble(Eval("WinMoneyNoWithTax")??0) %></td>
                        <td align="center"><%#Eval("CreateTime") %></td>
                        <td align="center"><%#SetSchemeStatusName(Convert.ToInt16(Eval("SchemeStatus")))%></td>
                        <td align="center"><%#Convert.ToInt32(Eval("BuyType"))==1?"追号":Convert.ToInt32(Eval("BuyType"))==0?"标准":"跟单" %></td>
                        <%--<td>
                            <asp:HiddenField ID="hidId" Value='<%#Eval("SchemeID")%>' runat="server" />
                            <asp:LinkButton ID="lbtnIsRevoke" CommandName="lbtnIsRevoke" runat="server" Visible='<%#Convert.ToInt32(Eval("SchemeStatus")) == 4%>' Text='<%#Convert.ToInt32(Eval("SchemeStatus")) == 4 ? "手动撤单" : "" %>' />
                        </td>--%>
                        <td>
                            <a href="javascript:Syn_Client(<%#Eval("SchemeID")%>)">同步客户端</a>
                            <a href="javascript:QuashScheme_And_Syn_Client(<%#Eval("SchemeID")%>)"><%#Convert.ToInt32(Eval("SchemeStatus"))==4?"手动撤单":(Convert.ToInt32(Eval("SchemeStatus"))==19?"手动撤单":"") %></a>
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
            $(".setWinMoney").click(function () {
                setWinMoney(this);
            });
        });

        //初始化窗口
        function setWinMoney(obj) {
            var Arr = $(obj).attr("data-value").split(",");

            var setWinDialog = top.dialog({
                id: 'setWin',
                title: "设置中奖号码",
                url: 'dialog/dialog_SetWinMoney.aspx?SchemeID=' + Arr[0] + '&LotteryCode=' + Arr[1],
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
                url: "../../tools/admin_ajax.ashx?action=setwinmoney",
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
                            content: '设置成功！',
                            okValue: '确定',
                            ok: function () { }
                        }).showModal(winObj);
                    }
                }
            });
        }

        //撤单并同步客户端
        function QuashScheme_And_Syn_Client(Order) {
            $.ajax({
                type: "post",
                url: "../../tools/admin_ajax.ashx?action=quashScheme_and_syn_client&order=" + Order,
                data: null,
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    top.dialog({
                        title: '提示',
                        content: '错误信息：' + errorThrown,
                        okValue: '确定',
                        ok: function () { }
                    }).showModal(winObj);
                },
                success: function (data, textStatus) {
                    alert(data.msg);
                }
            });
        }

        //同步客户端
        function Syn_Client(Order) {
            $.ajax({
                type: "post",
                url: "../../tools/admin_ajax.ashx?action=syn_client&order=" + Order,
                data: null,
                dataType: "json",
                error: function (XMLHttpRequest, textStatus, errorThrown) {
                    top.dialog({
                        title: '提示',
                        content: '尝试同步失败，错误信息：' + errorThrown,
                        okValue: '确定',
                        ok: function () { }
                    }).showModal(winObj);
                },
                success: function (data, textStatus) {
                    alert(data.msg);
                }
            });
        }
    </script>
</body>
</html>
