<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="template_edit.aspx.cs" Inherits="CL.Admin.admin.message.template_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>模板编辑</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
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
            <a href="template_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>编辑模板</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">模板基本信息</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content" style="min-height: 560px;">
            <dl>
                <dt>标题</dt>
                <dd>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-30" sucmsg=" " />
                    <span class="Validform_checktip"></span>
                </dd>
            </dl>
            <dl>
                <dt>消息类型</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlTemplateType" runat="server" datatype="*" errormsg="请选择类型..." sucmsg=" ">
                            <asp:ListItem Value="1">公告</asp:ListItem>
                            <asp:ListItem Value="2">提醒</asp:ListItem>
                            <asp:ListItem Value="3">私信</asp:ListItem>
                            <asp:ListItem Value="4">推送</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <%--<dt>模板变量</dt>
    <dd>
        <ul class="tags" style="width:100%;">
            <%foreach (Model.Entities.TemplateField item in FieldList)
                { %>
            <li data-value="<%=item.SetField %>"><%=item.FieldName %></li>
            <%} %>
        </ul>
    </dd>--%>
            </dl>
            <dl>
                <dt>内容</dt>
                <dd>
                    <asp:TextBox ID="txtContent" runat="server" CssClass="input normal" TextMode="MultiLine" Style="width: 50%; height: 100px;"></asp:TextBox>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
        <script>
            $(function () {
                $('.tags li').each(function (i) {
                    $(this).click(function () {
                        var field = $(this).attr("data-value");
                        var content = $("#txtContent").val();
                        if (content.indexOf(field) == -1)
                            $("#txtContent").val(content + ' {' + field + '} ');
                    });
                });
            });
        </script>
    </form>
</body>
</html>
