<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="openlottery_edit.aspx.cs" Inherits="CL.Admin.admin.lotteries.openlottery_edit" %>

<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>录入开奖信息</title>
    <link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../../scripts/datepicker/WdatePicker.js"></script>
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
    <form id="form2" runat="server">
        <!--导航栏-->
        <div class="location">
            <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
            <i class="arrow"></i>
            <span>录入开奖信息</span>
        </div>
        <div class="line10"></div>
        <!--/导航栏-->

        <!--内容-->
        <div id="floatHead" class="content-tab-wrap">
            <div class="content-tab">
                <div class="content-tab-ul-wrap">
                    <ul>
                        <li><a class="selected" href="javascript:;">期号基本信息</a></li>
                    </ul>
                </div>
            </div>
        </div>

        <div class="tab-content" style="min-height: 560px;">
            <dl>
                <dt>彩种</dt>
                <dd>
                    <div class="rule-single-select">
                        <asp:DropDownList ID="ddlLotteryCode" runat="server" datatype="*" errormsg="请选择彩种..." sucmsg=" "></asp:DropDownList>
                    </div>
                </dd>
            </dl>
            <dl>
                <dt>期号名称</dt>
                <dd>
                    <asp:TextBox ID="txtIsuseName" runat="server" CssClass="input normal" datatype="*1-20" sucmsg=" " />
                </dd>
            </dl>
            <dl>
                <dt>开奖号码</dt>
                <dd>
                    <asp:TextBox ID="txtWinNumber" runat="server" CssClass="input normal" datatype="*1-20" sucmsg=" " ReadOnly="true" />
                    <asp:HiddenField ID="hidWinNumer" runat="server" />
                    <span class="Validform_checktip"></span>
                </dd>
            </dl>
            <dl class="lowLottery" style="display:none">
                <dt>奖池滚存</dt>
                <dd>
                    <asp:TextBox ID="txtWinRollover" runat="server" CssClass="input small1" datatype="n" sucmsg=" " />
                </dd>
            </dl>
            <dl class="lowLottery" style="display:none">
                <dt>本期销售</dt>
                <dd>
                    <asp:TextBox ID="txtTotalSales" runat="server" CssClass="input small1" datatype="n" sucmsg=" " />
                </dd>
            </dl>
            <dl class="lowLottery" style="display:none">
                <dt>投注提示</dt>
                <dd>
                    <asp:TextBox ID="txtBettingPrompt" runat="server" CssClass="input normal" />
                </dd>
            </dl>
            <dl class="lowLottery" style="display:none">
                <dt>请输入各奖级的中奖金额</dt>
                <dd>
                    <table width="50%" border="0" cellspacing="0" cellpadding="0" class="ltable">
                        <tr>
                            <th>奖等</th>
                            <th>每注奖金</th>
                            <th>税后奖金</th>
                            <th>中奖注数</th>
                        </tr>
                    </table>
                    <input id="isusebonuses" type="hidden" value="" runat="server" />
                </dd>
            </dl>
            <dl>
                <dt></dt>
                <dd style="color:#ff0000;">
                    <asp:Label ID="lbMsg" runat="server" Text=""></asp:Label>
                </dd>
            </dl>
        </div>
        <!--/内容-->

        <!--工具栏-->
        <div class="page-footer">
            <div class="btn-wrap">
                <asp:Button ID="btnSubmit" runat="server" Text="提交保存" OnClientClick="return formHandle()" CssClass="btn" OnClick="btnSubmit_Click" />
                <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
            </div>
        </div>
        <!--/工具栏-->
    </form>

    <script>
        $(function () {
            $("#ddlLotteryCode").change(function () {
                $("#txtIsuseName").val('');
                $("#txtWinNumber").val('').attr("readonly", "readonly");
                $(".Validform_checktip").text('').removeClass("Validform_wrong");
                $("#txtWinRollover").val('');
                $("#txtTotalSales").val('');
                
            });
            $("#txtIsuseName").blur(function () {
                var lotterycode = $("#ddlLotteryCode").find("option:selected").attr("value");
                $.post("../../tools/admin_ajax.ashx?action=isusename_validate", { "LotteryCode": lotterycode, "param": $(this).val() }, function (data) {
                    var jsondata = JSON.parse(data);
                    if (jsondata.status == 0) {
                        if (!jsondata.OpenNumber) {
                            $("#txtWinNumber").removeAttr("readonly");
                            if ($("#ddlLotteryCode").val() == '801' || $("#ddlLotteryCode").val() == '901') { //低频彩显示奖等
                                var lotterycode = $(this).find("option:selected").attr("value");
                                $(".ltable").children().remove();

                                $.post("../../tools/admin_ajax.ashx?action=wintypes_list", { "LotteryCode": lotterycode }, function (data) {
                                    var jsondata = JSON.parse(data);
                                    if (jsondata.Code == 0) {
                                        var tHtml = "<tr><td align=\"center\">{0}<input name=\"tbID_{1}\" type=\"hidden\" value=\"{1}\"></td>" +
                                            "<td align=\"center\"><input name=\"tbMoney_{1}\" type=\"text\" value=\"{2}\" datatype=\"n\" class=\"input small\"></td>" +
                                            "<td align=\"center\"><input name=\"tbMoneyNoWithTax_{1}\" type=\"text\" value=\"{3}\" datatype=\"n\" class=\"input small\"></td>" +
                                            "<td align=\"center\"><input name=\"tbWinBet_{1}\" type=\"text\" datatype=\"n\" class=\"input small\"></td></tr>";
                                        var StrHtml = "";

                                        $.each(jsondata.Data, function (i, value) {
                                            StrHtml += tHtml.format(value["WinName"], value["WinID"], value["DefaultMoney"], value["DefaultMoneyNoWithTax"]);
                                        });
                                        $(".ltable").html("<tr><th>奖等</th><th>每注奖金</th><th>税后奖金</th><th>中奖注数</th></tr>" + StrHtml);
                                    }
                                });
                                $('.lowLottery').show();
                            }
                            else //高频彩隐藏奖等
                                $('.lowLottery').hide();
                        }
                        else {
                            $("#txtWinNumber").attr("readonly", "readonly");
                            $("#txtWinNumber").val(jsondata.OpenNumber);
                            //buxianshi jiajiang
                        }
                    } else {
                        $("#txtIsuseName").parents("dd").find(".Validform_checktip").text(jsondata.msg).addClass("Validform_wrong");
                    }
                });
            });
        });
        function formHandle() {
            var bright = true;
            var lotterycode = $("#ddlLotteryCode").find("option:selected").attr("value");
            $.ajax({
                type: "post",
                url: "../../tools/admin_ajax.ashx?action=isusename_validate",
                dataType: "json",
                data: { "LotteryCode": lotterycode, "param": $("#txtIsuseName").val() },
                async: false,
                success: function (jsondata) {
                    if (jsondata.status != 0) {
                        $("#txtIsuseName").parents("dd").find(".Validform_checktip").text(jsondata.msg).addClass("Validform_wrong");
                        bright = false;
                    }
                },
                error: function (xhr, status, error) {
                    alert(error);
                    bright = false;
                }
            });
            $("#hidWinNumer").val($("#txtWinNumber").val());
            if ($("#ddlLotteryCode").val() == '801' || $("#ddlLotteryCode").val() == '901') {
                var strVal = "";
                var index = 1;
                $(".ltable input").each(function (i, dom) {
                    if ((i % 4) == 3) {
                        strVal += $(dom).val() + "#";
                    } else {
                        if ((i % 4) == 2 || (i % 4) == 1)
                            strVal += $(dom).val() + "00,";
                        else
                            strVal += $(dom).val() + ",";
                    }
                });
                $("#isusebonuses").val(strVal.substring(0, strVal.length - 1));
            }
            return bright;
        };
    </script>
</body>
</html>
