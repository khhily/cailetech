<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="article_edit.aspx.cs" Inherits="CL.Admin.admin.news.article_edit" ValidateRequest="false" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>文章编辑</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../editor/kindeditor-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        $(function () {
            //初始化表单验证
            $("#form1").initValidform();

            //初始化编辑器
            var editor = KindEditor.create('.editor', {
                width: '100%',
                height: '350px',
                resizeType: 1,
                uploadJson: '../../tools/upload_ajax.ashx?action=EditorFile&IsWater=1',
                fileManagerJson: '../../tools/upload_ajax.ashx?action=ManagerFile',
                allowFileManager: true
            });
        });
    </script>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="article_list.aspx" class="back"><i></i><span>返回列表页</span></a>
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <a href="article_list.aspx"><span>文章管理</span></a>
  <i class="arrow"></i>
  <span>文章编辑</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">文章编辑</a></li>
      </ul>
    </div>
  </div>
</div>

<div class="tab-content">
  <dl>
    <dt>文章名称</dt>
    <dd>
        <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" datatype="*2-50" sucmsg=" " />
    </dd>
  </dl>
  <dl>
    <dt>关键字</dt>
    <dd>
        <asp:TextBox ID="txtKeyWord" runat="server" CssClass="input normal" maxlength="100" />
    </dd>
  </dl>
  <dl>
    <dt>描述</dt>
    <dd>
        <asp:TextBox ID="txtDescript" runat="server" CssClass="input normal" TextMode="MultiLine" style="width:50%; height:100px;"></asp:TextBox>
    </dd>
  </dl>
  <dl>
    <dt>文章内容</dt>
    <dd>
        <textarea id="txtContent" class="editor" style="visibility:hidden;" runat="server"></textarea>
    </dd>
  </dl>
  <%--<dl>
    <dt>是否显示</dt>
    <dd>
        <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblIsShow" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0">否</asp:ListItem>
        <asp:ListItem Value="1" Selected="True">是</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>是否热点</dt>
    <dd>
        <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblIsHot" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
        <asp:ListItem Value="1">是</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>是否置顶</dt>
    <dd>
        <div class="rule-multi-radio">
        <asp:RadioButtonList ID="rblIsTop" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
        <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
        <asp:ListItem Value="1">是</asp:ListItem>
        </asp:RadioButtonList>
      </div>
    </dd>
  </dl>
  <dl>
    <dt>阅读次数</dt>
    <dd>
      <asp:TextBox ID="txtReadCount" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
    </dd>
  </dl>--%>
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript:history.back(-1);" />
  </div>
</div>
<!--/工具栏-->

</form>
</body>
</html>
