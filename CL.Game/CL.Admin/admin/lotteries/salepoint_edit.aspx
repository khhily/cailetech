<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="salepoint_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.salepoint_edit" %>

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
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <link href="../../scripts/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/uploadify/jquery.uploadify.min.js"></script>
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
            <a href="lotteries_list.aspx"><span>点位查询</span></a>
            <i class="arrow"></i>
            <span>编辑点位</span>
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
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblTicketSourceID" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">彩乐</asp:ListItem>
                            <asp:ListItem Value="1">华阳</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>彩种</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlLottery" runat="server">
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd><span class="btn" onclick="addLine()">插入</span><span class="btn" onclick="removeLine()">删除</span></dd>
            </dl>
            <div id="Sale">
                <dl>
                    <dt>销售阶梯1</dt>
                    <dd>
                        <input type="text" class="input normal" datatype="n" value="0">元
                    <span class="Validform_checktip">*</span>
                    </dd>
                </dl>
                <dl>
                    <dt>销售点位1</dt>
                    <dd>
                        <input type="text" class="input small" datatype="/^[+-]?\d+(\.\d+)?$|^$|^(\d+|\-){7,}$/" value="0">%
                        <span class="Validform_checktip">*</span>
                    </dd>
                </dl>
            </div>
            <input id="issalepoint" type="hidden" value="" runat="server" />
            <dl>
                <dt>点位变更生效时间</dt>
                <dd>
                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" />
                </dd>
            </dl>
            <dl>
                <dt>点位变更描述</dt>
                <dd>
                    <asp:TextBox ID="txtDescribe" runat="server" CssClass="input normal" TextMode="MultiLine" Style="width: 50%; height: 80px;"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>附件</dt>
                <dd id="uploadDD">
                    <span class="Validform_checktip">*请上传变更函</span>
                    <input type="file" id="myUploadFile" />
                    <p id="queue"></p>
                </dd>
            </dl>
            <asp:HiddenField ID="hidFileSign" runat="server" />
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交审核" CssClass="btn" OnClick="btnSubmit_Click" OnClientClick="return formHandle()" />
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

        var num = 2;
        function addLine() {
            var html = "<div><dl>"
                    + "<dt>销售阶梯" + num + "</dt>"
                    + "<dd>"
                        + "<input type=\"text\" class=\"input normal\" datatype=\"n\" value=\"0\">"
                        + "<span class=\"Validform_checktip\">*</span>"
                    + "</dd>"
                + "</dl>"
                + "<dl>"
                    + "<dt>销售点位" + num + "</dt>"
                    + "<dd>"
                        + '<input type="text" class="input small" datatype="\/^[+-]?\\d+(\\.\\d+)?$|^$|^(\\d+|\\-){7,}$\/" value="0">'
                        + "<span class=\"Validform_checktip\">*整数或小数</span>"
                   + "</dd>"
                + "</dl></div>";
            $("#Sale").append(html);
            num++;
        }

        function removeLine() {
            var jqObj = $("#Sale");
            if (!jqObj.has('div').length) {
                return;
            }
            $("#Sale div").eq(-1).remove();
            num--;
        }


        function formHandle() {
            var strVal = "";
            $("#Sale input").each(function (i, dom) {
                if (i % 2 == 0) {
                    strVal += $(dom).val() + "#";
                } else {
                    strVal += $(dom).val() + ",";
                }
            });
            $("#issalepoint").val(strVal.substring(0, strVal.length - 1));
        };
    </script>
</body>
</html>
