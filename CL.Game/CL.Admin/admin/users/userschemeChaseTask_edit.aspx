<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="userschemeChaseTask_edit.aspx.cs" Inherits="CL.Admin.admin.users.userschemeChaseTask_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>查看投注信息</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span>查看投注信息</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">投注信息</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content" style="min-height:1024px;">
        <dl>
        <dt>彩种</dt>
        <dd>
            <%=LotteryName %>
        </dd>
        </dl>
        <dl>
        <dt>玩法名称</dt>
        <dd>
            <%=PlayName %>
        </dd>
        </dl>
        <dl>
        <dt>玩法说明</dt>
        <dd>
            <%=ModuleName %>
        </dd>
        </dl>
        <dl>
        <dt>投注方式</dt>
        <dd>
            <%=BetType==1?"机选":"自选" %>
        </dd>
        </dl>
        <dl>
        <dt>投注号码</dt>
        <dd>
            <%=BetNumber %>
        </dd>
        </dl>
        <dl>
        <dt>注数</dt>
        <dd>
            <%=BetNum %>
        </dd>
        </dl>
        <dl>
        <dt>期数</dt>
        <dd>
            <%=IsuseCount %>
        </dd>
        </dl>
        <dl>
        <dt>追号开始时间</dt>
        <dd>
            <%=StartTime %>
        </dd>
        </dl>
        <dl>
        <dt>追号结束时间</dt>
        <dd>
            <%=EndTime %>
        </dd>
        </dl>
        <dl>
        <dt>追号终止方式</dt>
        <dd>
            <%=SetStopTypeName(Convert.ToInt64(StopTypeWhenWinMoney))%>
        </dd>
        </dl>
        <dl>
        <dt>追号状态</dt>
        <dd>
            <%=SetQuashStatusName(QuashStatus)%>
        </dd>
        </dl>
        <dl>
        <dt>方案号</dt>
        <dd>
            <%=SchemeNumber %>
        </dd>
        </dl>
        <dl>
        <dt>投注时间</dt>
        <dd>
            <%=CreateTime %>
        </dd>
        </dl>
        <dl>
            <dt>追号明细</dt>
            <dd>
                <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th align="center">期号</th>
                            <th align="center">开始时间</th>
                            <th align="center">截止时间</th>
                            <th align="center">追号金额</th>
                            <th align="center">奖金金额</th>
                            <th align="center">状态</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center"><%#Eval("IsuseName") %></td>
                        <td align="center"><%#Eval("StartTime") %></td>
                        <td align="center"><%#Eval("EndTime") %></td>
                        <td align="center"><%#ConvertMoney(Eval("Amount")) %></td>
                        <td align="center"><%#ConvertMoney(Eval("WinMoney")) %></td>
                        <td align="center"><%#SetTicketStatusName(Convert.ToInt32(Eval("TicketStatus"))) %></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"8\">暂无记录</td></tr>" : ""%>
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
            </dd>
        </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

<script>
</script>
</form>
</body>
</html>
