function sendAjaxUrl() {
    $.ajax({
        type: "post",
        url: "/tools/admin_ajax.ashx?action=AgencyAccountList",
        data: { "key": "" },
        dataType: "json",
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            alert(errorThrown);
        },
        success: function (data, textStatus) {
            if (data.status == 0) {
                var tbodyhtml = '';
                for (var i = 0; i < data.msg.length; i++) {
                    tbodyhtml += "<tr><td><input id='radio_" + data.msg[i]["AccountID"] + "' type='radio'  name='Accountid' value='" + data.msg[i]["AccountID"] + "'></td><td>" + data.msg[i]["AccountCode"] + "</td><td>" + data.msg[i]["FullName"] + "</td><td>" + data.msg[i]["IdentityID"] + "</td><td>" + data.msg[i]["BankCard"] + "</td></tr>";
                }
                tbodyhtml += "<input type=\"hidden\" id=\"HIdAccountName\"/><input type=\"hidden\" id=\"HidAccountID\"/>";
                $("#tbodyconten").append(tbodyhtml);
            } else {
                alert(data.status);
            }
        }

    });
}

$(function () {
    $("#AgencyAcccount").click(function () {
        var winDialog = top.dialog({
            title: '选择用户',
            content: $("#SelectContent").html(),
            okValue: '确定',
            ok: function () {
                var currentCheckBox = $("input[type='radio']:checked", parent.document);
                if (0 < currentCheckBox.length) {
                    currentCheckBox = $(currentCheckBox[0]);
                    var pushid = currentCheckBox.val();
                    var username = currentCheckBox.parent().next().text();
                    $("#AgencyAcccount").val(username);
                    $("#HidAgencyID").val(pushid);
                }
                else {
                    alert("请选择用户!");
                }
            },
            cancelValue: '取消',
            cancel: function () {
            }
        }).showModal();
    });

    $("#form1").initValidform();
    sendAjaxUrl();
});

function verifyAccount() {
    var AccountId = $("#HidAgencyID").val();
    var UserName = $("#AgencyAcccount").val();
    var SaleAmount = $("#txtAmount").val();
    var RebateNum = $("#txtRebateNum").val();
    if (AccountId == '' || UserName == '') {
        alert("请选择代理用户");
        $("#HidAgencyID").val("");
        $("#AgencyAcccount").val("");
        return false;
    }
    if (RebateNum == '') {
        alert('返点数不能为空');
        return false;
    }
    if (SaleAmount == '') {
        return false;
    }
    return true
}

