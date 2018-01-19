<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userscheme_list.aspx.cs" Inherits="CL.Admin.admin.users.userscheme_list" %>

<%@ Import namespace="CL.Tools.Common" %>
<%@ Import namespace="CL.Enum.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>方案明细</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
          <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
          <i class="arrow"></i>
          <span>方案明细</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
          <div class="toolbar">
            <div class="box-wrap">
              <a class="menu-btn"></a>
              <div class="l-list">
                  <div class="rule-single-select">
                      <asp:DropDownList id="ddlLotteryCode" runat="server" OnSelectedIndexChanged="ddlLotteryCode_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                  </div>
                  <div class="rule-single-select">
                      <asp:DropDownList id="ddlWinState" runat="server" OnSelectedIndexChanged="ddlWinState_SelectedIndexChanged" AutoPostBack="True">
                          <asp:ListItem Value="0">全部</asp:ListItem>
                          <asp:ListItem Value="1">未开奖</asp:ListItem>
                          <asp:ListItem Value="2">未中奖</asp:ListItem>
                          <asp:ListItem Value="3">已中奖</asp:ListItem>
                          <asp:ListItem Value="4">已退款</asp:ListItem>
                      </asp:DropDownList>
                  </div>
                  <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                  <asp:TextBox ID="txtEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                  <asp:TextBox ID="txtKeywords" runat="server" CssClass="input txt" placeholder="输入方案号" style="margin-left:30px;" />
                  <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn" onclick="btnSearch_Click" style="color:#ffffff;">查询</asp:LinkButton>
              </div>
            </div>
            <br />
            <div class="l-list">
                方案总金额：<span style='color:red; font-weight:bold;'><%=SumMoney / 100 %></span> 元	
                方案税后总奖金：<span style='color:red; font-weight:bold;'><%=WinSumMoney / 100 %></span> 元	
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
              <th width="8%">彩种</th>
              <th width="10%">方案号</th>
              <th width="8%">期号</th>
              <th width="20%">玩法</th>
              <th width="6%">总金额</th>
              <th width="6%">税后金额</th>
              <th>方案描述</th>
              <th width="10%">投注时间</th>
              <th width="8%">状态</th>
            </tr>
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><%# Eval("LotteryName") %></td>
              <td align="center"><a href="<%#Convert.ToInt32(Eval("BuyType"))==1?"userschemeChaseTask_edit":"userscheme_edit"%>.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("SchemeID")%>"><%# Eval("SchemeNumber") %></a></td>
              <td align="center"><%#Eval("IsuseName") %></td>
              <td align="center"><%#Eval("PlayName") %></td>
              <td align="center"><%#Convert.ToInt64(Eval("SchemeMoney")) / 100 %></td>
              <td align="center"><%#Convert.ToInt64(Eval("WinMoneyNoWithTax")) / 100 %></td>
              <td align="center"><%#Utils.CutString(Eval("Describe").ToString(), 10)%></td>
              <td align="center"><%#Eval("CreateTime") %></td>
              <td align="center"><%#SetSchemeStatusName(Convert.ToInt32(Eval("SchemeStatus"))) %></td>
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

    </script>
</body>
</html>
