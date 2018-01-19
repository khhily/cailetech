<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="systemconfig_edit.aspx.cs" Inherits="CL.Admin.admin.settings.systemconfig_edit" %>

<!DOCTYPE html>

<html>
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
<meta name="apple-mobile-web-app-capable" content="yes" />
<title>系统设置</title>
<link href="../../scripts/artdialog/ui-dialog.css" rel="stylesheet" type="text/css" />
<link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
<%--<script type="text/javascript" charset="utf-8" src="../../scripts/jquery/Validform_v5.3.2_min.js"></script>--%>
<script type="text/javascript" charset="utf-8" src="../../scripts/artdialog/dialog-plus-min.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/laymain.js"></script>
<script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
<%--<script type="text/javascript">
    $(function () {
        //初始化表单验证
        $("#form1").initValidform();
    });
</script>--%>
</head>
<body class="mainbody">
<form id="form1" runat="server">
<!--导航栏-->
<div class="location">
  <a href="../center.aspx" class="home"><i></i><span>首页</span></a>
  <i class="arrow"></i>
  <span>系统设置</span>
</div>
<div class="line10"></div>
<!--/导航栏-->

<!--内容-->
<div id="floatHead" class="content-tab-wrap">
  <div class="content-tab">
    <div class="content-tab-ul-wrap">
      <ul>
        <li><a class="selected" href="javascript:;">系统设置</a></li>
      </ul>
    </div>
  </div>
</div>

<input type="hidden" id="Configjson" value="" runat="server" />
<input type="hidden" id="SetKeyHidden" value="" runat="server" />
<input type="hidden" id="SetValueHidden" value="" runat="server"/>
<div class="tab-content" id="SetInfo"> 
</div>
<!--/内容-->

<!--工具栏-->
<div class="page-footer">
  <div class="btn-wrap">
    <asp:Button ID="btnSubmit" runat="server" Text="提交保存" CssClass="btn" onclick="btnSubmit_Click" />
    <input name="btnReturn" type="button" value="返回上一页" class="btn yellow" onclick="javascript: history.back(-1);" />
  </div>
</div>
<!--/工具栏-->
</form>

<script>
    function IsNum(s) {
        if (s) return !isNaN(s);
        return false;
    }
    function IsUrl(s) {
        var reg = /^http(s)?:\/\/([\w-]+\.)+[\w-]+(V[\w-.\/?%&=]*)?/;
        var regs = new RegExp(reg);
        return regs.test(s) ? true : false;
    }
    function IsDate(s) {
        var reg = /^(\d{4})-(\d{2})-(\d{2})$/;
        return reg.test(s) ? true : false;
    }

    var StrHtml = "<dl><dt>{0}</dt><dd>{1}</dd></dl>";
    var sHtml="";
    var json = JSON.parse($("#Configjson").val());
    
    $.each(json, function () {
        switch (this.DataType) {
            case 0:
                var input = "<input type=\"text\" class=\"input normal\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" datatype=\"String\"/>".format(this.SetKey, this.SetValue);
                sHtml += StrHtml.format(this.SetName, input);
                break;
            case 1:
                var input = "<input type=\"text\" class=\"input normal\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" msg=\"{2}\" datatype=\"Int\"/>".format(this.SetKey, this.SetValue, this.SetName);
                sHtml += StrHtml.format(this.SetName, input);
                break;
            case 2:
                var input = "{0}<input type=\"checkbox\" name=\"{1}\" value=\"{2}\" {3} />&nbsp;&nbsp;";
                var strcb = "";
                if (this.DataValue) {
                    var ArrList = this.DataValue.split(',');
                    var ArrVal = this.SetValue.split(',');
                    var sName = this.SetKey;
                    $.each(ArrList, function (i, v) {
                        if (v) {
                            var arr = v.split('.');
                            if (arr.length > 0) {
                                strcb += input.format(arr[1], sName, arr[0], ArrVal.indexOf(arr[0]) > -1 ? "checked='checked'" : "");
                            }
                        }
                    });
                }
                strcb += "<input type=\"text\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" style=\"display:none;\"/>".format(this.SetKey, this.SetValue);
                sHtml += StrHtml.format(this.SetName, strcb);
                break;
            case 3:
                var input = "{0}<input type=\"radio\" name=\"{1}\"value=\"{2}\" {3}/>&nbsp;&nbsp;";
                var strra = "";
                if (this.DataValue) {
                    var ArrList = this.DataValue.split(',');
                    var ArrVal = this.SetValue.split(',');
                    var sName = this.SetKey;
                    $.each(ArrList, function (i, v) {
                        if (v) {
                            var arr = v.split('.');
                            if (arr.length > 0) {
                                strra += input.format(arr[1], sName, arr[0], ArrVal.indexOf(arr[0]) > -1 ? "checked='checked'" : "");
                            }
                        }
                    });
                }
                strra += "<input type=\"text\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" style=\"display:none;\"/>".format(this.SetKey, this.SetValue);
                sHtml += StrHtml.format(this.SetName, strra);
                break;
            case 4:
                var input = "<input type=\"text\" class=\"input normal\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" msg=\"{2}\" datatype=\"Url\"/>".format(this.SetKey, this.SetValue, this.SetName);
                sHtml += StrHtml.format(this.SetName, input);
                break;
            case 5:
                var input = "<input type=\"text\" class=\"input normal\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" msg=\"{2}\" datatype=\"Date\"/>".format(this.SetKey, this.SetValue, this.SetName);
                sHtml += StrHtml.format(this.SetName, input);
                break;
            default:
                var input = "<input type=\"text\" class=\"input normal\" name=\"setVal\" id=\"setvalue_{0}\" value=\"{1}\" datatype=\"String\"/>".format(this.SetKey, this.SetValue);
                sHtml += StrHtml.format(this.SetName, input);
                break;
        }
    });

    $("#SetInfo").html(sHtml);

    $("input[type='radio']").click(function () {
        $("#setvalue_" + $(this).attr("name")).val($(this).val());
    });

    $("input[type='checkbox']").click(function () {
        var strName = $(this).attr("name");
        var checkVal = "";
        $("input[name=" + strName + "]").each(function (k, v) {
            if (v.checked)
                checkVal += $(this).val() + ",";
        });
        $("#setvalue_" + strName).val(checkVal.length > 0 ? checkVal.substring(0, checkVal.length - 1) : "");
    });

    $("#form1").submit(function () {
        var bret = true;
        var msg = "";

        //判断数字类型逻辑
        $.each($("input[datatype='Int']"), function () {
            if (!IsNum($(this).val())) {
                alert($(this).attr("msg") + "必须填写数字");
                bret = false;
                $(this).focus();
                return false;
            }
        })
        if (!bret) return false;
        //判断URL格式是否合法
        $.each($("input[datatype='Url']"), function () {
            if (!IsUrl($(this).val())) {
                alert($(this).attr("msg") + "地址格式不对");
                bret = false;
                $(this).focus();
                return false;
            }
        });
        if (!bret) return false;
        //日期格式验证
        $.each($("input[datatype='Date']"), function () {
            if (!IsDate($(this).val())) {
                alert($(this).attr("msg") + "日期格式不对");
                bret = false;
                $(this).focus();
                return false;
            }
        });
        if (!bret) return false;

        var idStr = "";
        var valStr = "";
        $("input[type='text'][name='setVal']").each(function (k, v) {
            idStr += $(v).attr("id") + "*";
            valStr += $(v).val() + "*";
        });
        if (idStr == "") return false;
        $("#SetKeyHidden").val(idStr.length > 0 ? idStr.substring(0, idStr.length - 1) : "");
        $("#SetValueHidden").val(valStr.length > 0 ? valStr.substring(0, valStr.length - 1) : "");
        return true;
    });

</script>
</body>
</html>
