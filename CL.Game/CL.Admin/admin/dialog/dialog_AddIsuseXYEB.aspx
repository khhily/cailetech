﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_AddIsuseXYEB.aspx.cs" Inherits="CL.Admin.admin.dialog.dialog_AddIsuseXYEB" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>批量添加期号</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        var api = top.dialog.get(window); //获取父窗体对象
        var W = api.data; //获取父对象
        //页面加载完成执行
        $(function () {
            //设置按钮及事件
            api.button([{
                value: '添加',
                callback: function () {
                    execAddIsuse();
                },
                autofocus: true
            }, {
                value: '取消',
                callback: function () { }
            }]);

            //初始化表单验证
            $("#form1").initValidform();
        });

        //添加导入的数据
        function execAddIsuse() {
            var s = $(".Validform_wrong");
            var days = $("#txtDays").val();
            if (days > 10) {
                top.dialog({
                    title: '提示',
                    content: '一次最多增加10天的期号！',
                    okValue: '确定',
                    ok: function () { }
                }).showModal(api);
                return false;
            }
            if (s.length > 0)
                return false;

            var para = {
                "LotteryCode": <%=LotteryCode %>,
                "DateValue": $("#txtDate").val(),
                "Days": $("#txtDays").val(),
                "Type": 5,
                "IsuseName":$("#txtIsuseName").val()
            };

            W.sendAjaxUrl(api, para);
            return false;
        }
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <div class="div-content">
            <div class="dl-attach-box">
                <dl>
                    <dt>开始期号</dt>
                    <dd>
                        <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input normal" errormsg="请输入开始期号" sucmsg=" " />
                    </dd>
                </dl>
                <dl>
                    <dt>开始时间</dt>
                    <dd>
                        <asp:TextBox ID="txtDate" runat="server" CssClass="input normal rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd'})" datatype="/^\s*$|^\d{4}\-\d{1,2}\-\d{1,2}$/" errormsg="请选择正确的日期" sucmsg=" " />
                    </dd>
                </dl>
                <dl>
                    <dt>增加天数</dt>
                    <dd>
                        <asp:TextBox ID="txtDays" runat="server" CssClass="input normal1" datatype="n" sucmsg=" ">10</asp:TextBox>
                    </dd>
                </dl>
                <dl>
                    <dt></dt>
                    <dd>高频性彩票，您只需要选择增加的天数，系统会自动在指定的天数内，增加每天的所有期号
                    </dd>
                </dl>
            </div>
        </div>
    </form>
</body>
</html>
