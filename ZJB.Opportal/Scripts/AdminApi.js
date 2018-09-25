
var loadingFlag = false;
$(function () {
    changeLoadFlag();
});

function changeLoadFlag() {
    //CKEDITOR.replace("houseDescribe");
    var ue = UE.getEditor('ApiWordContent');
    loadingFlag = true;
}
function AddApi()
{
    var ue = UE.getEditor('ApiWordContent');
    var apiWordId = $("#hideApiWordId").val();
    var apiTitle = $("#ApiTitle").val();
    var apiurl = $("#ApiUrl").val();
    var apiWordContent = ue.getContent();
    var apiWordContentTxt = ue.getContentTxt();
    var Type = $("#selectType").val();
    var IsLogin = $("#IsLogin").val();
    var METHODType = $("#METHODType").val();
    if (apiWordContentTxt == "" || apiTitle == "") {
        alert("标题或者内容不能为空");
        return false;
    }
    else {
        var alertContent = "确定添加";
        if (apiWordId > 0) {
            alertContent = "确定修改？";
        }
        if (confirm(alertContent))
        {
            $.ajax({
                type: 'post',
                dataType: 'json',
                url: "/Apis/apiNew",
                data: { ApiWordId: apiWordId, ApiWordContent: apiWordContent, Title: apiTitle, Type: Type, Method: METHODType, Url: apiurl, IsLogin: IsLogin },
                success: function (result)
                {
                    if (result.status == 0) {
                        alert(result.msg);
                    }
                    else {
                        alert("成功");
                        setTimeout(function () {
                            location.href = "/Apis/apiList";
                        }, 2000);
                    }
                }
            });
        }
    }
}