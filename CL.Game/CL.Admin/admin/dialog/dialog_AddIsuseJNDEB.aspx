<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_AddIsuseJNDEB.aspx.cs" Inherits="CL.Admin.admin.dialog.dialog_AddIsuseJNDEB" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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
            if (s.length > 0)
                return false;

            var para = {
                "LotteryCode": <%=LotteryCode %>,
                "DateValue": 0,
                "Days": 0,
                "Type": 6,
                "IsuseName":0
            };

            W.sendAjaxUrl(api, para);
            return false;
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="div-content">
            <div class="dl-attach-box">
                <dl>
                    <dt></dt>
                    <dd>高频性彩票，维护当前彩种时请检查服务器是否能访问当前网址：lotto.bclc.com
                    </dd>
                </dl>
            </div>
        </div>
    </form>
</body>
</html>
