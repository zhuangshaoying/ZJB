
var loadingFlag = false;
$(function () {
    changeLoadFlag();
});

function changeLoadFlag() {
    //CKEDITOR.replace("houseDescribe");
    var ue = UE.getEditor('NoticeContent');
    loadingFlag = true;
}
function AddNotice()
{
    var ue = UE.getEditor('NoticeContent');
    var noticeId = $("#hideNoticeId").val();
    var noticeTitle=$("#NoticeTitle").val();
    var noticeContent = ue.getContent();
    var noticeContentTxt = ue.getContentTxt();
    var Type = $("#selectType").val();
    if (noticeContentTxt == "" || noticeTitle == "") {
        alert("标题或者内容不能为空");
        return false;
    }
    else {
        var alertContent = "确定添加公告？";
        if (noticeId > 0) {
            alertContent = "确定修改公告？";
        }
        if (confirm(alertContent))
        {
            $.ajax({
                type: 'post',
                dataType: 'json',
                url: "/Notice/NoticeNew",
                data: { NoticeId: noticeId, NoticeContent: noticeContent, Title: noticeTitle, Type: Type },
                success: function (result)
                {
                    if (result.status == 0) {
                        alert(result.msg);
                    }
                    else {
                        alert("成功");
                        setTimeout(function () {
                            location.href = "/Notice/NoticeList";
                        }, 2000);
                    }
                }
            });
        }
    }
}