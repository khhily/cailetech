<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_SetWinMoney.aspx.cs" Inherits="CL.Admin.admin.dialog.dialog_SetWinMoney" %>

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

            //初始化表单验证
            $("#form1").initValidform();

            $("#ddlWintypes").change(function () {
                var WinID = $(this).find("option:selected").attr("value");
                $.post("../../tools/admin_ajax.ashx?action=wintypes_list", { "WinID": WinID }, function (data) {
                    var jsondata = JSON.parse(data);
                    if (jsondata.status == 0) {
                        $("#txtDefaultMoney").val(jsondata.DefaultMoney);
                        $("#txtDefaultMoneyNoWithTax").val(jsondata.DefaultMoneyNoWithTax);
                    }
                });
            });
        });

        //添加导入的数据
        function execAddIsuse() {
            var s = $(".Validform_wrong");
            if (s.length > 0)
                return false;

            var para = {
                "SchemeID": "<%=SchemeID %>",
                "WinMoney": $("#txtDefaultMoney").val(),
                "WinMoneyTax": $("#txtDefaultMoneyNoWithTax").val()
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
            <dt>开始时间</dt>
            <dd>
                <div class="rule-single-select">
                    <asp:DropDownList id="ddlWintypes" runat="server" datatype="*" errormsg="请选择奖等" sucmsg=" "></asp:DropDownList>
                </div>
            </dd>
          </dl>
        <dl>
        <dt>中奖金额</dt>
        <dd>
            <asp:TextBox ID="txtDefaultMoney" runat="server" CssClass="input normal1" datatype="n" sucmsg=" "></asp:TextBox>
        </dd>
        </dl>
        <dl>
            <dt>税后金额</dt>
            <dd>
                <asp:TextBox ID="txtDefaultMoneyNoWithTax" runat="server" CssClass="input normal1" datatype="n" sucmsg=" "></asp:TextBox>
            </dd>
        </dl>
      </div>
    </div>
    </form>
</body>
</html>
