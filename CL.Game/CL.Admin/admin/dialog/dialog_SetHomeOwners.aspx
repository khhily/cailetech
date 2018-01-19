<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_SetHomeOwners.aspx.cs" Inherits="CL.Admin.admin.dialog.dialog_SetHomeOwners" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>设置中奖信息</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
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
        });

        //添加导入的数据
        function execAddIsuse() {
            var para = {
                "id": "<%=id %>",
                "username": $("#txtUserName").val()
            };
            W.sendAjaxUrl(api, para);
            return false;
        }

    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <div class="div-content">
        <dl>
        <dt>用户账号</dt>
        <dd>
            <asp:TextBox ID="txtUserName" runat="server" CssClass="input normal"></asp:TextBox>
        </dd>
        </dl>
    </div>
    </form>
</body>
</html>