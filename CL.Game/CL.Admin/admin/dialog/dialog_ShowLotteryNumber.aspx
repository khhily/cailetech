<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dialog_ShowLotteryNumber.aspx.cs" Inherits="CL.Admin.admin.dialog.dialog_ShowLotteryNumber" %>
<%@ Import namespace="CL.Tools.Common" %>
<!DOCTYPE html>

<html>
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <meta name="viewport" content="width=device-width,minimum-scale=1.0,maximum-scale=1.0,initial-scale=1.0,user-scalable=no" />
    <meta name="apple-mobile-web-app-capable" content="yes" />
    <title>查看投注信息</title>
    <link href="../skin/default/style.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" charset="utf-8" src="../../scripts/jquery/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" charset="utf-8" src="../js/common.js"></script>
    <script type="text/javascript">
        var api = top.dialog.get(window); //获取父窗体对象
        //页面加载完成执行
        $(function () {
            //设置按钮及事件
            api.button([{
                value: '取消',
                callback: function () { }
            }]);
        });
    </script>
</head>
<body class="mainbody">
    <form id="form1" runat="server">
    <div class="div-content">
      <div class="dl-attach-box">
        <dl>
            <dt>彩种</dt>
            <dd>
                <%=LotteryName %>
            </dd>
          </dl>
        <dl>
        <dt>期号</dt>
        <dd>
            <%=IsuseName %>
        </dd>
        </dl>
        <dl>
        <dt>方案号</dt>
        <dd>
            <%=SchemeNumber %>
        </dd>
        </dl>
        <dl>
        <dt>投注时间</dt>
        <dd>
            <%=CreateTime %>
        </dd>
        </dl>
        <dl>
            <dt>投注明细</dt>
            <dd>
            <input type="hidden" id="lnum" value="<%=LotteryNumber %>" />
            <input type="hidden" id="arrplay" value="<%=ArrPlay %>"/>
            <table width="100%" border="0" cellspacing="0" cellpadding="0" id="ltable">
                <tr>
                    <th>玩法</th>
                    <th>注数</th>
                    <th>倍数</th>
                    <th>投注号</th>
                </tr>
            </table>
            </dd>
        </dl>
      </div>
    </div>
    </form>

    <script>
        var json = JSON.parse($("#lnum").val());
        var jsonData = JSON.parse(json);

        //var json = "[{\"playid\":10107,\"data\":[{\"multiple\":123,\"bet\":2,\"number\":\"4#5-6\"}]},{\"playid\":10106,\"data\":[{\"multiple\":123,\"bet\":3,\"number\":\"1-2-3\"}]}]";
        /*
        [{"playid":10107,"data":[{"multiple":123,"bet":2,"number":"4#5-6"}]},{"playid":10106,"data":[{"multiple":123,"bet":3,"number":"1-2-3"}]},]
        */

        var objplay = <%=ArrPlay %>;
        var tHtml="<tr><td align=\"center\">{0}</td><td align=\"center\">{1}</td><td align=\"center\">{2}</td><td align=\"center\">{3}</td></tr>";
        var sHtml="";

        $.each(jsonData, function () {
            sHtml += tHtml.format(objplay["" + this.playid + ""], this.data[0].bet, this.data[0].multiple, this.data[0].number);
        });
        $("#ltable").html("<tr><th>玩法</th><th>注数</th><th>倍数</th><th>投注号</th></tr>"+sHtml);
    </script>
</body>
</html>
