<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="news_list.aspx.cs" Inherits="CL.Admin.admin.news.news_list" %>

<%@ Import Namespace="CL.Enum.Common" %>
<%@ Import Namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>新闻列表</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <link href="../../css/pagination.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../../scripts/artdialog/dialog-plus-min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="javascript:history.back(-1);" class="back"><i></i><span>返回上一页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>新闻列表</span>
        </div>
        <!--/导航栏-->

        <!--工具栏-->
        <div id="floatHead" class="toolbar-wrap">
            <div class="toolbar">
                <div class="box-wrap">
                    <a class="menu-btn"></a>
                    <div class="l-list">
                        <ul class="icon-list">
                            <li><a class="add" href="news_edit.aspx?action=<%=CaileEnums.ActionEnum.Add %>"><i></i><span>新增</span></a></li>
                            <li><a class="all" href="javascript:;" onclick="checkAll(this);"><i></i><span>全选</span></a></li>
                            <li>
                                <asp:LinkButton ID="btnDelete" runat="server" CssClass="del" OnClientClick="return ExePostBack('btnDelete');" OnClick="btnDelete_Click"><i></i><span>删除</span></asp:LinkButton></li>
                        </ul>
                        <div class="rule-single-select">
                            <asp:DropDownList ID="ddlNewsType" runat="server" datatype="*" errormsg="请选择类型..." sucmsg=" " OnSelectedIndexChanged="ddlNewsType_SelectedIndexChanged" AutoPostBack="True"></asp:DropDownList>
                        </div>
                    </div>
                    <div class="r-list">
                        <asp:TextBox ID="txtKeywords" runat="server" CssClass="keyword" />
                        <asp:LinkButton ID="lbtnSearch" runat="server" CssClass="btn-search" OnClick="btnSearch_Click">查询</asp:LinkButton>
                    </div>
                </div>
            </div>
        </div>
        <!--/工具栏-->

        <!--列表-->
        <div class="table-container">
            <asp:Repeater ID="rptList" runat="server">
                <HeaderTemplate>
                    <table width="100%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th align="center">选择</th>
                            <th align="center">标题</th>
                            <th align="center">关键字</th>
                            <th align="center">内容</th>
                            <th align="center">分类</th>
                            <th align="center">关联彩种</th>
                            <th align="center">推荐号码</th>
                            <th align="center">作者</th>
                            <th align="center">来源</th>
                            <th align="center">推荐</th>
                            <th align="center">显示设备</th>
                            <th align="center">审核状态</th>
                            <th align="center">是否删除</th>
                            <th align="center">操作</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                    <tr>
                        <td align="center">
                            <asp:CheckBox ID="chkId" CssClass="checkall" runat="server" Style="vertical-align: middle;" />
                            <asp:HiddenField ID="hidId" Value='<%#Eval("NewsID")%>' runat="server" />
                        </td>
                        <td align="center"><%# Utils.CutString(Eval("Title").ToString(),10)%></td>
                        <td align="center"><%# Utils.CutString(Eval("Keys").ToString(),10)%></td>
                        <td align="center"><%# Utils.CutString(Eval("PlainText").ToString(),10)%></td>
                        <td align="center"><%# (Eval("TypeName"))%></td>
                        <td align="center"><%# SetLottery(Eval("LotteryCode"))%></td>
                        <td align="center"><%# (Eval("LotNumber"))%></td>
                        <td align="center"><%# (Eval("Author"))%></td>
                        <td align="center"><%# (Eval("Source"))%></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsRecommend"))==true?"推荐":"不推荐"%></td>
                        <td align="center"><%# SetEquipment(Eval("Equipment"))%></td>
                        <td align="center"><%# SetStatus(Eval("AuditingStatus"))%></td>
                        <td align="center"><%# Convert.ToBoolean(Eval("IsDel"))==true?"删除":"正常"%></td>
                        <td align="center"><a href="news_edit.aspx?action=<%#CaileEnums.ActionEnum.Edit %>&id=<%#Eval("NewsID")%>">修改</a></td>
                    </tr>
                </ItemTemplate>
                <FooterTemplate>
                    <%#rptList.Items.Count == 0 ? "<tr><td align=\"center\" colspan=\"14\">暂无记录</td></tr>" : ""%>
          </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
        <!--/列表-->

        <!--内容底部-->
        <div class="line20"></div>
        <div class="pagelist">
            <div class="l-btns">
                <span>显示</span><asp:TextBox ID="txtPageNum" runat="server" CssClass="pagenum" onkeydown="return checkNumber(event);" OnTextChanged="txtPageNum_TextChanged" AutoPostBack="True"></asp:TextBox><span>条/页</span>
            </div>
            <div id="PageContent" runat="server" class="default"></div>
        </div>
        <!--/内容底部-->
    </form>
</body>
</html>
