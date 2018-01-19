<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lookauditing.aspx.cs" Inherits="CL.Admin.admin.news.lookauditing" ValidateRequest="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>新闻资讯审核</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
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
<body>
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="news_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="news_list.aspx"><span>新闻管理</span></a>
            <i class="arrow"></i>
            <span>新闻资讯审核</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->
        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">新闻编辑</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>新闻类型</dt>
                <dd>
                    <asp:TextBox ID="txtNewsType" runat="server" CssClass="input normal" />

                </dd>
            </dl>
            <dl>
                <dt>标 题</dt>
                <dd>
                    <asp:TextBox ID="txtTitle" runat="server" CssClass="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>关键字</dt>
                <dd>
                    <asp:TextBox ID="txtKeys" runat="server" CssClass="input normal" MaxLength="100" />
                </dd>
            </dl>
            <dl>
                <dt>作 者</dt>
                <dd>
                    <asp:TextBox ID="txtAuthor" runat="server" CssClass="input normal" MaxLength="100" />
                </dd>
            </dl>
            <dl>
                <dt>来 源</dt>
                <dd>
                    <asp:TextBox ID="txtSource" runat="server" CssClass="input normal" MaxLength="200" /></dd>
            </dl>
            <dl>
                <dt>显示设备</dt>
                <dd>
                    <asp:TextBox ID="txtEquipment" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>关联彩种</dt>
                <dd>
                    <asp:TextBox ID="txtLottery" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>推荐号码</dt>
                <dd>
                    <asp:TextBox ID="txtLotNumber" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>是否推荐</dt>
                <dd>
                    <asp:TextBox ID="txtIsRecommend" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>发布人</dt>
                <dd>
                    <asp:TextBox ID="txtPublish" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>发布时间</dt>
                <dd>
                    <asp:TextBox ID="txtPublishTime" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>修改人</dt>
                <dd>
                    <asp:TextBox ID="txtModify" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>修改时间</dt>
                <dd>
                    <asp:TextBox ID="txtModifyTime" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>排序</dt>
                <dd>
                    <asp:TextBox ID="txtSort" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>阅读数</dt>
                <dd>
                    <asp:TextBox ID="txtReadNum" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>支持数</dt>
                <dd>
                    <asp:TextBox ID="txtSupportNum" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>反对数</dt>
                <dd>
                    <asp:TextBox ID="txtOpposeNum" runat="server" CssClass="input normal" MaxLength="200" />
                </dd>
            </dl>
            <dl>
                <dt>文章内容</dt>
                <dd>
                    <textarea id="txtContent" class="editor" style="visibility: hidden;" runat="server"></textarea>
                </dd>
            </dl>
        </div>
        <!--/内容-->
        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnAdopt" runat="server" Text="通过" CssClass="btn" OnClick="btnAdopt_Click" />
                <asp:Button ID="btnRefuse" runat="server" Text="拒绝" CssClass="btn" OnClick="btnRefuse_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
    </form>
</body>
</html>
