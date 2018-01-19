<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="JiGuangPush.aspx.cs" Inherits="CL.Admin.admin.push.JiGuangPush" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>极光推送</title>

    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>极光推送</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">极光推送</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div id="sdsds" class="tab-content" style="min-height: 1024px;">
            <dl style="display: none;">
                <dt>目标平台</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlEquipment" runat="server">
                            <asp:ListItem Value="1">IOS开发环境</asp:ListItem>
                            <asp:ListItem Value="2">IOS生成环境</asp:ListItem>
                            <asp:ListItem Value="3">Android</asp:ListItem>
                            <asp:ListItem Value="4">WinPhone</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>目标人群</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlPush" runat="server">
                            <asp:ListItem Value="1">广播(所有人)</asp:ListItem>
                            <asp:ListItem Value="2">Registration ID</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl id="dl_Registration" style="display: none;">
                <dt></dt>
                <dd>
                    <textarea id="TaRegistration" disabled="disabled" class="input" style="margin: 0px; width: 361px; height: 65px;" runat="server"></textarea><span style="color: red">每次最多发送二十个用户(广播除外)</span>
                    <input id="hidRegistration" runat="server" type="hidden" />
                </dd>
            </dl>
            <dl>
                <dt>推送内容
                </dt>
                <dd>
                    <textarea id="TaContent" class="input" style="margin: 0px; width: 361px; height: 135px;" runat="server"></textarea>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="color: red;">
                    <asp:Button ID="btnSubmit" CssClass="btn" OnClientClick="return verifypush()" OnClick="btnSubmit_Click" runat="server" Text="推送" />
                </dd>
            </dl>
        </div>
        <!--/内容-->
        <script>
            $("#ddlPush").change(function () {
                var pushVal = $("#ddlPush").val();
                if (pushVal == 2) {
                    $("#dl_Registration").attr("style", "display: block;");
                    var winDialog = top.dialog({
                        title: '选择用户',
                        content: $('#SelectContent').html(),
                        okValue: '确定',
                        ok: function () {
                            var username = $("#TaRegistration").text();
                            var pushid = $("#hidRegistration").val();
                            var tbody = $("#tbodycontent", parent.document).html();
                            $(tbody).each(function () {
                                username += $(this).find("td").eq(0).text() + ",";
                                pushid += $(this).find("td").eq(1).text() + ",";
                            });

                            $("#TaRegistration").text(username);
                            $("#hidRegistration").val(pushid);
                            return true;
                        },
                        cancelValue: '取消',
                        cancel: function () {

                        }
                    }).showModal();
                } else {
                    $("#dl_Registration").attr("style", "display: none;");
                    $("#TaRegistration").text("");
                    $("#hidRegistration").val("");
                }
            });
            function verifypush() {
                var Content = $("#TaContent").val();
                if ($("#ddlPush").val() == 2) {
                    var RegistrationIds = $("#hidRegistration").val();
                    var UserName = $("#TaRegistration").text();
                    if (RegistrationIds == '' || UserName == '') {
                        alert("请选择推送用户");
                        $("#hidRegistration").val("");
                        $("#TaRegistration").text("");
                        return false;
                    }
                }
                if (Content == '') {
                    alert("请填写推送内容");
                    return false;
                }
                return true
            }

            $(function () {
                var pushVal = $("#ddlPush").val();
                if (pushVal == 2) {
                    $("#dl_Registration").attr("style", "display: block;");
                } else {
                    $("#dl_Registration").attr("style", "display: none;");
                }
            });
        </script>


        <script id="SelectContent" type="text/template">
            <div class="toolbar-wrap">
                <div class="r-list">
                    <input id="txtKeys" type="text" class="keyword" placeholder="输入用户名" />
                    <input id="txtSearch" type="button" class="btn-search" value="查询" />
                </div>
            </div>
            <div class="table-container">
                <table id="tabcontent" border="1" style="width: 350px;" class="input">
                    <thead>
                        <tr>
                            <td>用户名
                            </td>
                            <td>RegistrationID
                            </td>
                        </tr>
                    </thead>
                    <tbody id="tbodycontent">
                    </tbody>
                </table>
            </div>
            <script type="text/javascript">
                $(function () {
                    sendAjaxUrl();
                });
                $("#txtSearch").click(function () {
                    sendAjaxUrl();
                });

                function sendAjaxUrl() {
                    $.ajax({
                        type: "post",
                        url: "../../tools/admin_ajax.ashx?action=userpushlist",
                        data: { "key": $("#txtKeys").val() },
                        dataType: "json",
                        error: function (XMLHttpRequest, textStatus, errorThrown) {
                            alert(errorThrown);
                            $("#tbodycontent").html("");
                        },
                        success: function (data, textStatus) {
                            if (data.status == 0) {
                                var tbodyhtml = '';
                                for (var i = 0; i < data.msg.length; i++) {
                                    tbodyhtml += "<tr><td>" + data.msg[i]["UserName"] + "</td><td>" + data.msg[i]["PushIdentify"] + "</td></tr>";
                                }
                                $("#tbodycontent").html(tbodyhtml);
                            } else {
                                alert(data.status);
                                $("#tbodycontent").html("");
                            }
                        }

                    });
                }
            </script>
        </script>
    </form>
</body>
</html>
