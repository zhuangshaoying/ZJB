var type = 0;
var firstIn = true;
var completeCount = 0;
function switchTab(ProTag, freshFlag) {
    type = ProTag;
    $("#tab" + ProTag).addClass("selected");
    $("#tab" + ProTag).siblings().removeClass("selected");
    $("#task_" + ProTag).show();
    $("#task_" + ProTag).siblings().hide();
    GetUserTaskList();
}
$(function () {
    GetUserTaskList();
})
$(".modal-close-icon").bind("click", function () {
    $(".modal").animate({ top: "0px" }, 100).css("opacity", "0");
    $(".modal-backdrop").removeClass("in")
})
function GetUserTaskList()
{
    if (type == 0 || type == 1) {
        $.ajax({
            type: 'get',
            dataType: 'json',
            url: '/Task/GetUserTask',
            data: { type: type },
            beforeSend: function (XMLHttpRequest) {
                if (parent) {
                    parent.loadingShow();
                }
            },
            success: function (result) {
                var data = result.data;
                var html = BuildTaskHtml(data);
                $("#task_" + type).html(html);
                if (firstIn && completeCount > 0) {
                    firstIn = false;
                    $(".modal-backdrop").addClass("in").show();
                     $("#firstInShow").html("恭喜，你已经完成 "+completeCount+" 项任务，感觉简单？再来几个！");
                     $(".modal").animate({ top: "25%" }, 150).css("opacity", "1").show();
                }
            },
            complete: function (XMLHttpRequest, textStatus) {
                if (parent) {
                    parent.loadingHide();
                }
            }
        });
    }

}
function BuildTaskHtml(data)
{
    completeCount = 0;
    var html = '<table class="task-table" style="padding: 0px;">';
    $.each(data, function (i, n) {
        var processTxt = "未完成";
        var processClass = "unfinished";
        var btnDrawClass = "ui-btn-disabled";
        var btnDrawName = "领取奖励";
        var btnDo = "去完成";
        var btnTaskDraw = "";
        if (type == 0 ) {
            if (n.TaskStatus == 0) {
                completeCount++;
                processTxt = "未领取";
                btnDrawClass = "";
                btnTaskDraw = "TaskDraw(" + n.TaskId + ")";
            }
            if (n.Type == 3) {
                btnDrawName = "已完成" + n.SpecialTaskCount + "次";
                processTxt = "";

            }
        }
        else if( type == 1){
            processTxt = "已完成";
            btnDrawName = "已完成";
            processClass = "finished";
            btnDo = "去看看";
        }
        var display = "none";
        if (n.TaskUrl != '' && n.TaskUrl != null) {
            display = "inline";
        }
        html +=
'          <tbody>' +
'            <tr>' +
'              <td class="task-content"><div class="task-content-wrap">' +
'                  <span class="task_'+n.Type+'"></span><h4 class="task-title">' + n.TaskName + '</h4>' +
'                  <ul>' +
'                    <li>' +
'                      <label>条件：</label>' +
'                      <span>' + (n.TaskDescription == null ? "<br/>" : n.TaskDescription) + '</span> </li>' +
'                    <li>' +
'                      <label>奖励：</label>' +
'                      <span> <em>+' + n.Points + '</em> 积分 </span> </li>' +
'                    <li>' +
'                      <label>进度：</label>' +
'                      <span> <span class="' + processClass + '">' + processTxt + '</span> </span> </li>' +
'                  </ul>' +
'                </div></td>' +
'              <td class="task-operate"><a href="javascript:' + btnTaskDraw + ';" class="ui-btn ' + btnDrawClass + '">' + btnDrawName + '</a> <a href="' + n.TaskUrl + '" style="display:' + display + ';" class="ui-btn" >' + btnDo + '</a></td>' +
'            </tr>' +
'            <tr class="searation-row">' +
'              <td colspan="2"></td>' +
'            </tr>' +
'          </tbody>';
    });
    
    html += '        </table>';
    return html;
}
function TaskDraw(taskId)
{
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Task/UserTaskDraw',
        data: { taskId: taskId },
        beforeSend: function (XMLHttpRequest) {
            if (parent) {
                parent.loadingShow();
            }
        },
        success: function (result) {
            jQuery.dialog.tips(result.msg, 1.5, "success.gif");
            if (result.status > 0) {
                $.each($(".myPoints"), function (i, n) {
                    var newPoints = parseInt($(n).html()) + result.status;
                    $(n).html(newPoints)
                });
            }
            GetUserTaskList();
        },
        complete: function (XMLHttpRequest, textStatus) {
            if (parent) {
                parent.loadingHide();
            }
        }
    });
}

function TipFadeOut()
{
    $(".modal-backdrop").removeClass("in").hide();
    $(".modal").animate({ top: "0%" }, 50).css("opacity", "0");
    setTimeout(function () { $(".modal").hide(); }, 1000);
}