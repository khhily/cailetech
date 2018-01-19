<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="programa_edit.aspx.cs" Inherits="CL.Admin.admin.settings.programa_edit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
     <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>栏目编辑</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="programa_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="programa_list.aspx"><span>栏目管理</span></a>
            <i class="arrow"></i>
            <span>栏目编辑</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">栏目编辑</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>上级导航</dt>
                <dd>
                    <%--<asp:TreeView ID="treeView1" runat="server" ShowCheckBoxes="All"></asp:TreeView>--%>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlMenuType" runat="server" datatype="*" errormsg="请选择上级菜单..." sucmsg=" "></asp:DropDownList>
                    </div>
                </dd>
            </dl>
              <dl>
                <dt>调用别名</dt>
                <dd>
                    <asp:TextBox ID="txtName" runat="server" CssClass="input normal"  datatype="/^[a-zA-Z0-9\-\_]{2,50}$/" errormsg="请填写正确的别名"  sucmsg=" " /><span class="Validform_checktip">权限控制名称，只允许字母、数字、下划线</span>
                </dd>
            </dl>
            <dl>
                <dt>标 题</dt>
                <dd>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-50" sucmsg=" " /><span class="Validform_checktip">*导航中文标题，100字符内</span>
                </dd>
            </dl>
            <dl>
                <dt>链接地址</dt>
                <dd>
                    <asp:TextBox ID="txtLinkUrl" runat="server" CssClass="input normal" MaxLength="100" datatype="*2-50" sucmsg=" " />
                </dd>
            </dl>     
             <dl>
                <dt>排序数字</dt>
                <dd>
                    <asp:TextBox ID="txtSortID" runat="server" CssClass="input normal" MaxLength="100" datatype="*2-50" sucmsg=" " /><span class="Validform_checktip">*数字，越小越向前</span>
                </dd>
            </dl> 
             <dl>
                <dt>是否隐藏</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlIsLock" runat="server">
                            <asp:ListItem Value="False">否</asp:ListItem>
                            <asp:ListItem Value="True">是</asp:ListItem>
                        </asp:DropDownList>
                        <span class="Validform_checktip">*隐藏后不显示在界面导航菜单中。</span>
                    </div>
                </dd>
            </dl>          
            <dl>
                <dt>备注说明</dt>
                <dd>
                     <asp:TextBox ID="txtRemark" runat="server" CssClass="input" TextMode="MultiLine"></asp:TextBox>
                     <span class="Validform_checktip">非必填，可为空</span>
                </dd>
            </dl>
             <dl>
                <dt>方法类型</dt>
                <dd>
                     <asp:TextBox ID="txtActionType" runat="server" CssClass="input normal" MaxLength="100" datatype="*2-50" sucmsg=" " />
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

    </form>
</body>
</html>
