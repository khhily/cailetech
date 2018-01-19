<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="scheme_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.scheme_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>查看投注信息</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
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
        <dt>期号</dt>
        <dd>
            <%=IsuseName %>
        </dd>
        </dl>
        <dl>
        <dt>开奖号码</dt>
        <dd style="color:#cc0000;">
            <%=OpenNumber %>
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
            <dt>投注明细</dt>
            <dd>
                <asp:Repeater ID="rptList" runat="server" OnItemDataBound="rptList_ItemDataBound">
                <HeaderTemplate>
                <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                <tr>
                    <th>玩法</th>
                    <th>注数</th>
                    <th>倍数</th>
                    <th>投注号</th>
                    <%if (SchemeStatus == 4 || SchemeStatus == 6 || SchemeStatus == 8) {%>
                    <th>设置奖等</th>
                     <%}
                        else {%>
                    <th>是否中奖</th>
                    <%} %>
                </tr>
                </HeaderTemplate>
                <ItemTemplate>
                <tr>
                    <td align="center"><%#Eval("PlayName") %></td>
                    <td align="center"><%#Eval("BetNum") %></td>
                    <td align="center"><%#Eval("Multiple") %></td>
                    <td align="center"><%#Eval("BetNumber") %></td>
                    <%if (SchemeStatus == 4 || SchemeStatus == 6 || SchemeStatus == 8)
                        {%>
                    <td align="center">
                        <asp:HiddenField ID="hidId" Value='<%#Eval("SDID")%>' runat="server" />
                        <asp:HiddenField ID="hidlcode" Value='<%#Eval("LotteryCode")%>' runat="server" />
                        <div class="rule-single-select">
                         <asp:DropDownList id="ddlWintypes" runat="server" datatype="*" errormsg="请选择奖等" sucmsg=" "></asp:DropDownList>
                        </div>
                    </td>
                    <%}
                        else {%>
                    <td align="center"><%#Convert.ToInt32(Eval("IsWin")) == 1 ? "<span style=\"color:#cc0000;\">中奖</span>" : "" %></td>
                    <%} %>
                </tr>
                </ItemTemplate>
                <FooterTemplate>
                </table>
                </FooterTemplate>
                </asp:Repeater>
            </dd>
        </dl>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
      <%if (SchemeStatus == 4 || SchemeStatus == 6 || SchemeStatus == 8) {%>
    <asp:Button ID="btnSubmit" runat="server" Text="提交中奖信息" CssClass="btn" onclick="btnSubmit_Click" />
      <%} %>
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

<script>
</script>
</form>
</body>
</html>
