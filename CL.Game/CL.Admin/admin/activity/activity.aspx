<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="activity.aspx.cs" Inherits="CL.Admin.admin.activity.activity" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>发布活动</title>

    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>

    <link href="../../../scripts/uploadify/uploadify.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../../scripts/uploadify/jquery.uploadify.min.js"></script>
    <%--<script type="text/javascript" charset="utf-8" src="../../scripts/uploadify/uploadify.js"></script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="scheme_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>发布活动</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">发布活动</a></li>
                    </ul>
                </div>
            </div>
        </div>
        <div class="tab-content" style="min-height: 1024px;">
            <dl>
                <dt>活动类型</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlActivityType" runat="server">
                            <asp:ListItem Value="0">彩种官方活动</asp:ListItem>
                            <asp:ListItem Value="1">彩乐彩乐活动</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>活动主题</dt>
                <dd>
                    <asp:TextBox ID="txtActivitySubject" class="input" runat="server"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动开始时间</dt>
                <dd>
                    <asp:TextBox ID="txtStartTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>活动结束时间</dt>
                <dd>
                    <asp:TextBox ID="txtKeyEndTime" runat="server" CssClass="input txt rule-date-input" onfocus="WdatePicker({dateFmt:'yyyy-MM-dd HH:mm:ss'})" />
                </dd>
            </dl>
            <dl>
                <dt>活动金额</dt>
                <dd>
                    <asp:TextBox ID="txtActivityMoney" class="input" runat="server"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>活动参与币种</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlCurrencyUnit" runat="server">
                            <asp:ListItem Value="0">余额</asp:ListItem>
                            <asp:ListItem Value="1">元宝</asp:ListItem>
                            <asp:ListItem Value="2">游戏币</asp:ListItem>
                            <asp:ListItem Value="3">彩券</asp:ListItem>
                        </asp:DropDownList>
                        <asp:HiddenField ID="hidAdUrl" runat="server" />
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>活动落地页</dt>
                <dd>
                    <asp:TextBox ID="txtLandingPage" class="input" runat="server"></asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>上传客户端广告图片</dt>
                <dd>
                    <div id="fileQueue" style="width: 200px;"></div>
                    <input type="file" name="uploadify" id="uploadify" />
                    <input type="hidden" name="photo" id="photo" value="" />
                    <div id="pic_view_div" style="width: 150px; height: 180px">
                        <img runat="server" width="1020" height="225" id="pic_view" />
                    </div>
                </dd>
            </dl>
            <dl style="padding-top: 50px;">
                <dt>活动描述</dt>
                <dd>
                    <textarea id="areaActivityDescribe" class="input" runat="server" style="width: 300px; height: 180px;"></textarea>
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="color: red;">
                    <asp:Label ID="lbMsg" runat="server"></asp:Label>
                </dd>
            </dl>
        </div>
        <!--/内容-->
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="发布活动" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <script>
            $(function () {
                $("#uploadify").uploadify({
                    //'langFile': '/config/juploadify/uploadifyLang_cn.js',
                    'swf': '../../scripts/uploadify/uploadify.swf',
                    'uploader': '../../tools/upload_ajax.ashx?action=activity',
                    'queueID': 'fileQueue',
                    'auto': true,
                    'multi': true,
                    'buttonCursor': 'hand',
                    'fileObjName': 'uploadify',
                    'buttonText': '上传图片',
                    'height': '25',
                    'progressData': 'percentage',
                    'fileTypeDesc': '支持格式:jpg/gif/jpeg/png/bmp.',
                    'fileTypeExts': '*.jpg;*.gif;*.jpeg;*.png;*.bmp',//允许的格式  
                    onUploadSuccess: function (file, data, response) {
                        //设置图片预览  
                        if (data == '') {
                            alert('图片上传失败');
                        } else {
                            $("#pic_view").attr("src", data);
                            $("#hidAdUrl").val(data);
                        }
                    }
                });
            })
        </script>
        <!--/工具栏-->
    </form>
</body>
</html>
