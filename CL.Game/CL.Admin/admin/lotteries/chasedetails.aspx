<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="chasedetails.aspx.cs" Inherits="CL.Admin.admin.lotteries.chasedetails" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>投注查询</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();
        });
    </script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="chase_List.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="chase_List.aspx"><span>投注查询</span></a>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">方案明细</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content" style="min-height:560px;">
  <dl>
    <dt>方案标题</dt>
    <dd>
        <asp:Label ID="labtitle" runat="server"></asp:Label>
    </dd>
  </dl>
  <dl>
    <dt>任务类型</dt>
    <dd>
        <asp:Label ID="lablotteryname" runat="server"></asp:Label>
        <asp:Label ID="labplayname" runat="server"></asp:Label>
    </dd>
  </dl>
  <dl>
    <dt>追号描述</dt>
    <dd>
        <asp:Label ID="labdescription" runat="server"></asp:Label>
    </dd>
  </dl>
   <dl>
    <dt>所追期号</dt>
    <dd>
      <asp:Repeater ID="rptList" runat="server" onitemcommand="rptList_ItemCommand">
          <HeaderTemplate>
          <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
          </HeaderTemplate>
          <ItemTemplate>
            <tr>
              <td align="center"><%# Eval("IsuseName") %>期</td>
              <td align="center"><%#Eval("SchemeNumber") %></td>
              <td align="center"><%# Eval("LotteryNumber") %></td>
              <td align="center">倍数<%# Eval("Multiple") %></td>
              <td align="center"><%#Eval("Amount") %></td>
              <td align="center" class="statusname" data-value="<%#Eval("QuashStatus") %>,<%#Eval("SchemeID") %>"></td>
              <td align="center">
                  <asp:HiddenField ID="hidId" Value='<%#Eval("ID")%>' runat="server" />
                  <asp:LinkButton ID="lbtndStatus" CommandName="lbtndStatus" runat="server" style='<%#Convert.ToInt32(Eval("SchemeID")) > 0 ? "display:none" : ""%>'>取消此期</asp:LinkButton>
              </td>
            </tr>
          </ItemTemplate>
          <FooterTemplate>
            <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无记录</td></tr>" : ""%>
          </table>
          </FooterTemplate>
          </asp:Repeater>
    </dd>
  </dl>
   <dl>
    <dt>任务执行</dt>
    <dd>
        <asp:Label ID="Label1" runat="server"></asp:Label>
        <asp:Button ID="btnSubmit" runat="server" Text="中止任务" CssClass="btn" onclick="btnSubmit_Click" />
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

</form>

    <script>
        $(function () {
            $(".statusname").each(function () {
                var Arr = $(this).attr("data-value").split(',');
                var QuashStatus = Arr[0];
                var SchemeID = Arr[1];

                if (QuashStatus != 0) {
                    if (QuashStatus == 2)
                        $(this).text("系统撤单");
                    else
                        $(this).text("用户撤单");
                } else {
                    if (SchemeID > 0) {
                        $(this).text("已完成");
                    } else {
                        $(this).text("进行中");
                    }
                }
            });
        });
    </script>
</body>
</html>
