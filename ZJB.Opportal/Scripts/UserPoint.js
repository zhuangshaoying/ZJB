var artPoint;
function ShowUserPoint(uid) {
    $.ajax({
        type: 'get',
        url: '/User/GetUserDetailInfo',
        data: { id: uid },
        success: function (result) {
            var html = '<div>当前积分：' + result.Points + '</div>';
            html += '<div>';
            html += '<select onchange="onSelectTaskTypeChange()" id="taskType"><option value="0">普通操作</option><option value="13">意见采纳</option></select>';
            html += '<select id="pointType"><option value="0">加分</option><option value="1">扣分</option></select>';
            html += '<input type="text" id="pointTxt" placeHolder="分数"></input>';
            html += '<input type="text" id="pointDesc" placeHolder="备注说明"></input>';
            html += '<input type="button" id="pointTxt" onclick="AddUserPoint(' + uid + ')" value="确定"></input>';
            html += '</div>';
            $("#UserPointBox").html(html);
            artPoint=art.dialog({
                content: document.getElementById('UserPointBox'),
                title: result.Name + '积分操作'
            });
            
        }
    });

}
function onSelectTaskTypeChange()
{
    var taskType=$("#taskType").val();
    if (taskType > 0) {
        $("#pointType").hide();
        $("#pointTxt").hide();
    }
    else {
        $("#pointType").show();
        $("#pointTxt").show();
    }
}
    function AddUserPoint(uid) {
        var pointType = $("#pointType").val();
        var points = $("#pointTxt").val();
        var pointDesc = $("#pointDesc").val();
        var taskType = $("#taskType").val();
        if (pointType == 1) {
            points = -points;
        }
        $.ajax({
            type: 'post',
            url: '/User/AddUserPoint',
            data: { id: uid, points: points, pointDesc: pointDesc, taskType: taskType },
            success: function (result) {
                alert(result.msg);
                if (result.code > 0) {
                    $("#UserPointBox").html("");
                    artPoint.close();
                }
            }
        });
    }