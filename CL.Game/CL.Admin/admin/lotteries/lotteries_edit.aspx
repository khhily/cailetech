<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lotteries_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.lotteries_edit" ValidateRequest="false" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>彩种编辑</title>
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
            <a href="lotteries_list.aspx" class="back"><i></i><span>返回列表页</span></a>
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <a href="lotteries_list.aspx"><span>内容管理</span></a>
            <i class="arrow"></i>
            <span>编辑内容</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">彩种基本信息</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content">
            <dl>
                <dt>彩种名称</dt>
                <dd>
                    <asp:TextBox ID="txtLotterryName" runat="server" CssClass="input normal" datatype="*2-30" sucmsg=" " />
                    <span class="Validform_checktip">*彩种名称最多30个字符</span>
                </dd>
            </dl>
            <dl>
                <dt>名称拼音</dt>
                <dd>
                    <asp:TextBox ID="txtShorthand" runat="server" CssClass="input normal" MaxLength="30" />
                </dd>
            </dl>
            <dl>
                <dt>彩种代码</dt>
                <dd>
                    <asp:TextBox ID="txtLotteryCode" runat="server" CssClass="input small" datatype="n" sucmsg=" " ajaxurl="../../tools/admin_ajax.ashx?action=lotterycode_validate">0</asp:TextBox>
                </dd>
            </dl>
            <dl>
                <dt>彩票类型</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblTypeID" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="1">福彩</asp:ListItem>
                            <asp:ListItem Value="2">体彩</asp:ListItem>
                            <asp:ListItem Value="3">足彩</asp:ListItem>
                            <asp:ListItem Value="4" Selected="True">高频彩</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>副标题</dt>
                <dd>
                    <asp:TextBox ID="txtSubhead" runat="server" CssClass="input normal" datatype="*2-250" sucmsg=" " />
                    <span class="Validform_checktip">相当于小广告，如“两元博五百万”</span>
                </dd>
            </dl>
            <dl>
                <dt>最大追号期数</dt>
                <dd>
                    <asp:TextBox ID="txtMaxChaseIsuse" runat="server" CssClass="input normal" MaxLength="30" />
                </dd>
            </dl>
            <dl>
                <dt>开奖号码格式</dt>
                <dd>
                    <asp:TextBox ID="txtWinNumberExemple" runat="server" CssClass="input normal" MaxLength="30"></asp:TextBox>
                    <span class="Validform_checktip">如果有括号代表可能有特别号码（如·14年双色球增加的幸运蓝球）</span>
                </dd>
            </dl>
            <dl>
                <dt>开奖周期规则</dt>
                <dd>
                    <asp:TextBox ID="txtIntervalType" runat="server" CssClass="input normal" MaxLength="30"></asp:TextBox>
                    <span class="Validform_checktip">格式：10@分@79@08:30-21:30、1@天@1@08:00-23:00、(时间间隔@间隔单位@周期局数@开始时间-结束时间)</span>
                </dd>
            </dl>
            <dl>
                <dt>出票方式</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlPrintOutType" runat="server">
                            <asp:ListItem Value="0">本地出票</asp:ListItem>
                            <asp:ListItem Value="1">华阳电子票</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>单注金额</dt>
                <dd>
                    <asp:TextBox ID="txtPrice" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*</span>
                </dd>
            </dl>
            <dl>
                <dt>投注最大倍数</dt>
                <dd>
                    <asp:TextBox ID="txtMaxMultiple" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*能投注的最大倍数</span>
                </dd>
            </dl>
            <dl>
                <dt>截止投注时间</dt>
                <dd>
                    <asp:TextBox ID="txtOffTime" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*截止投注时间，单位 秒，秒</span>
                </dd>
            </dl>
            <dl>
                <dt>延迟执行</dt>
                <dd>
                    <asp:TextBox ID="txtChaseExecuteDeferMinute" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*追号延迟执行间隔时间，秒</span>
                </dd>
            </dl>
            <dl>
                <dt>撤销延迟执行</dt>
                <dd>
                    <asp:TextBox ID="txtQuashExecuteDeferMinute" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*撤销追号提前时间间隔，秒</span>
                </dd>
            </dl>
            <dl>
                <dt>彩种介绍</dt>
                <dd>
                    <textarea id="txtKinformation" class="editor" style="visibility: hidden;" runat="server"></textarea>
                </dd>
            </dl>
            <dl>
                <dt>重点推荐</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblIsEmphasis" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否加奖</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblIsAddaward" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>是否启用</dt>
                <dd>
                    <div class="rule-multi-radio">
                        <asp:RadioButtonList ID="rblIsEnable" runat="server" RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="0" Selected="True">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>游戏版本号</dt>
                <dd>
                    <asp:TextBox ID="txtModuleVersion" runat="server" CssClass="input normal" />
                </dd>
            </dl>
            <dl>
                <dt>排序数字</dt>
                <dd>
                    <asp:TextBox ID="txtSortId" runat="server" CssClass="input small" datatype="n" sucmsg=" ">99</asp:TextBox>
                    <span class="Validform_checktip">*数字，越小越向前</span>
                </dd>
            </dl>
            <dl>
                <dt>预售时间</dt>
                <dd>
                    <asp:TextBox ID="txtPresellTime" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*(分钟为单位) 0表示正常销售  高频彩当天最后一期结束预售第二天第一期</span>
                </dd>
            </dl>
            <dl>
                <dt>期号提前结束时间</dt>
                <dd>
                    <asp:TextBox ID="txtAdvanceEndTime" runat="server" CssClass="input small" datatype="n" sucmsg=" ">0</asp:TextBox>
                    <span class="Validform_checktip">*(分钟为单位) 0表示正常结期</span>
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
