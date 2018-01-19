<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="salepoint_audit.aspx.cs" Inherits="CL.Admin.admin.lotteries.salepoint_audit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>点位编辑</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <%--<script type="text/javascript" charset="utf-8" src="../../scripts/webuploader/webuploader.min.js"></script>--%>
    <%--<script type="text/javascript" charset="utf-8" src="../js/uploader.js"></script>--%>
    <%--<script type="text/javascript" charset="utf-8" src="../js/WebUploader.js"></script>--%>
    <link href="../../scripts/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../js/uploadify.js"></script>
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
            <a href="lotteries_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="lotteries_list.aspx"><span>点位审核</span></a>
            <i class="arrow"></i>
            <span>审核点位</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">点位信息</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>出票商</dt>
                <dd>
                    <asp:Label ID="labTicketSourceID" runat="server"></asp:Label>
                </dd>
            </dl>
            <dl>
                <dt>彩种</dt>
                <dd>
                    <asp:Label ID="labLotteryName" runat="server"></asp:Label>
                </dd>
            </dl>
            <div id="SaleDIV" runat="server">

            </div>
            <dl>
                <dt>点位变更生效时间</dt>
                <dd>
                    <asp:Label ID="labStartTime" runat="server"></asp:Label>
                </dd>
            </dl>
            <dl>
                <dt>点位变更描述</dt>
                <dd>
                    <asp:Label ID="labDescribe" runat="server"></asp:Label>
                </dd>
            </dl>
            <dl>
                <dt>附件</dt>
                <dd>
                    <asp:Repeater ID="rptList" runat="server">
                          <HeaderTemplate>
                          <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                          </HeaderTemplate>
                          <ItemTemplate>
                            <tr>
                              <td align="center"><%# Eval("FileName") %></td>
                              <td align="center"><%#Eval("FileEXT") %></td>
                              <td align="center">
                                  <asp:HiddenField ID="hidId" Value='<%#Eval("SalePointFileID")%>' runat="server" />
                                  <a href="../..<%#Eval("FileUrl") %>">下载</a>
                              </td>
                            </tr>
                          </ItemTemplate>
                          <FooterTemplate>
                            <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"20\">暂无文件记录</td></tr>" : ""%>
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
                <asp:Button ID="btnSubmit" runat="server" Text="审核通过" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->

    </form>
    
    <script>
        $(function () {
            var fileSign = $("#hidFileSign").val();
            initUploadify($('#myUploadFile'), '../../tools/upload_ajax.ashx?action=UpLoadifyFile&fileSign=' + fileSign);
        })
    </script>
</body>
</html>
