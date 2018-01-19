<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ETicketsReport_add.aspx.cs" Inherits="CL.Admin.admin.report.ETicketsReport_add" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>出票明细导入</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <link href="../../scripts/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/uploadify/jquery.uploadify.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/uploadify.js"></script>


</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="lotteries_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="ETicketsReport.aspx"><span>出票明细查询</span></a>
            <i class="arrow"></i>
            <span>出票明细导入</span>
        </div>
        <div class="line10"></div>

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">出票明细导入</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>出票商</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlMerchantCode" runat="server" AutoPostBack="True">
                            <asp:ListItem Value="0">--请选择--</asp:ListItem>
                            <asp:ListItem Value="1">华阳</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl id="HuaYang" style="display:none">
                <dt>接口商出票数据</dt>
                <dd id="uploadDD">
                    <span class="Validform_checktip">*请上传华阳出票明细Excel</span>
                    <input type="file" id="myUploadFile"/>
                    <p id="queue"></p>
                </dd>
            </dl>
            <div class="page-footer">
            <div class="btn-wrap">
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        </div>
        <!--/内容-->

        <!--工具栏-->
        
        <!--/工具栏-->

        
        
    </form>

    <script>
        $(function () {
            if ($('#ddlMerchantCode').val() != 0)
                $('#HuaYang').show();
            else
                $('#HuaYang').hide();
            initUploadify($('#myUploadFile'), '../../tools/upload_ajax.ashx?action=UpLoadifyExcelTOLst');
        })
    </script>
</body>
</html>
