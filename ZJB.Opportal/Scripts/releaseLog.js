
var tradeTypeMap = ["", "出售", "求购", "出租", "求租"];
var buildTypeMap = ["", "住宅", "别墅", "商铺", "写字楼", "厂房"];
$(function () {

    if ($("#index").val() != '') {
        switchTab($("#index").val());
    }
  
    
    $("#cellUl li").live("click", function () {
        var cell = $(this).html();

        if (cell == '全部小区') {
            $("#cell").val('');
        } else {
            $("#cell").val(cell);
        }

        $("#cellSpan").html(cell);
        $("#cellUl").hide();
        ajaxSub();
    });

    $("#webUl li").live("click", function () {
        $("#webId").val($(this).val());
        $("#webSapn").html($(this).html());
        $("#webUl").hide();
        ajaxSub();
    });

    $("#statusUl li").click(function () {
        $("#status").val($(this).val());
        $("#statusSapn").html($(this).html());
        $("#statusUl").hide();
        ajaxSub();
    });

    $(".manage-time-content li").click(function () {
        var content = $(this).html();
        $("#sort").val($(this).val());
        $(".manage-time-tip span").html(content);
        $(".manage-list-content").hide();
        getHousebyCondition();
    });

    $(".manage-list-content").mouseleave(function () {
        $(".manage-list-content").hide();
    });
    ajaxSub(1, 1);
    GetEnableWebSite();
    getHouseCommunityList();
});

/*tab切换*/
function switchTab(i) {
    if (i == 0 || i == 1||i==5) {
        $("#selectDiv").show();
        if (i == 0 || i == 5) {
            $("#statusLi").hide();
        } else {
            $("#statusLi").show();
        }
    } else {
        $("#selectDiv").hide();
    }
    if (i != 1) {
        $('body').stopTime();//停止定时器
    }
    $("#tab" + i).addClass("selected");
    $("#tab" + i).siblings().removeClass("selected");
}

function enterSumbit() {
    var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异
    if (event.keyCode == 13) {
        ajaxSub();
    }
}

function resetSelect(i) {
    $("#index").val(i);
    if (i == 0) {
        $("#status").val(1);
    } else if (i == 1) {
        $("#status").val(0);
        $("#statusSapn").html("全部状态");
    }
    $("#cellSpan").html("全部小区");
    $("#cell").val('');
    $("#websapn").html("全部网站");
    $("#webid").val(0);

    $("#time").val('');
    $("#title").val('');
}

var isCanSubmit = true;
function ajaxSub(i, page) {
    if (isCanSubmit) {
        if (page == undefined) {
            if (i == undefined) {
                i = $("#index").val();
            } else {
                resetSelect(i);
            }
            page = 1;
            $("#buildingId").val(0);
        }
        $("#page").val(page);

        var url = $("#basePath").val();
        var callBack = BuildLogHtml;
        if (i == 0) {
            url = "/PostLog/GetPostLogList";
            $("#status").val(1);
            callBack = BuildLogHtml;
        } else if (i == 1) {
            url = "/PostLog/GetPostLogList";
            callBack = BuildLogHtml;
        } else if (i == 2) {
            url = "/PostLog/GetLogByHouses";
            callBack = BuildLogHtmlByHouse;
        } else if (i == 3) {
            url = "/PostLog/GetLogByWebSite";
            callBack = BuildLogByWeiSite;
        } else if (i == 4) {
            url = "/PostLog/GetLogByStat";
        }
        else if (i == 5) {
            url = "/PostLog/GetPostLogList";
            $("#status").val(-1);
            callBack = BuildLogHtml;
        }
        $.ajax({
            url: url,
            data: {
                "pageIndex": page,
                "pageSize": $("#pageSize").val(),
                "siteId": $("#webId").val(),
                "communityName": $("#cell").val(),
                "status": $("#status").val(),
                "title": $("#title").val(),
                "time": $("#time").val(),
                "houseId": $("#buildingId").val()
            },
            type: 'get',
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                isCanSubmit = false;
                parent.loadingShow();
            },
            success: function (result) {
                var resultHtml = callBack != null ? callBack(result) : "";
                var totalSize = result.totalSize;
                $("#logDiv").html(resultHtml);
                if (totalSize > 0) {
                    var pageSize = parseInt($("#pageSize").val());
                    var pagecount = parseInt(totalSize / pageSize) + ((totalSize % pageSize) == 0 ? 0 : 1);
                    $("#saleManager-fanye").paginate({
                        count: pagecount,
                        start: $("#page").val(),
                        display: 10,
                        border: false,
                        text_color: '#50b63f',
                        text_hover_color: '#fff',
                        background_color: '#fff',
                        background_hover_color: '#50b63f',
                        images: false,
                        mouse: 'click',
                        onChange: function () {
                            ajaxSub(i, $(".jPag-current").html());
                        }
                    });
                }
                else {
                    $("#saleManager-fanye").html("");
                }
                switchTab(i);
            },
            error: function (jqXHR) {
                //					alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete: function (XMLHttpRequest, textStatus) {
                isCanSubmit = true;
                parent.loadingHide();
            }
        });
    }
}
/*发布记录和成功记录的html显示*/
function BuildLogHtml(datas) {
    var data = datas.data;
    var html = "";
    html += '<div id="release-success" class="houseLog lookAsLog con1">' +
               '	<table class="h_list clear" width="100%" cellspacing="0" cellpadding="0">' +
               '		<thead><tr><td>房源编号</td><td>基本信息</td><td>目标网站</td><td>操作类型</td><td>发布账号</td><td>状态</td><td>操作时间</td></tr></thead>' +
               '		<tbody>';
    if (data != null && data.length > 0) {
        $.each(data, function (a, b) {
            var titleAHtml = "";
            var titleHtmlUp = '【<span class="postTypeSpan">' + tradeTypeMap[b.TradeType] + '</span>】<span>' + b.Title + '</span>';
            var titleHtmlDown = '【<span class="buildingTypeSpan">' + buildTypeMap[b.BuildType] + '</span>】<span>' + b.CommunityName + '</span>&nbsp;&nbsp;<span>' + b.BuildArea + '㎡</span>&nbsp;&nbsp;<span>' + b.CurFloor + '/' + b.MaxFloor + '</span>&nbsp;&nbsp;<span>' + b.Price + '</span>&nbsp;' + b.PriceUnit + '';
            html += '				<tr id="htr' + b.ID + '">' +
                    '					<td width="60">' +
                    '						<ul style="text-align:center">' +
                    '								<li style="display:none;"><input type="checkbox" name="taskCheck" class="saleManager-state-checkbox" value="' + b.ID + '" id="bid' + b.ID + '" infoid="' + b.TargetID + '" webid="' + b.SiteID + '" style="margin:0px"></li>' +
                    '						<li><label for="bid' + b.ID + '">' + b.InfoID + '</label></li>' +
                    '						</ul>' +
                    '					</td>' +
                    '					<td style="text-align:left">' +
                    '						<ul class="task-title">' +
                    '							<li>' + (b.Status == 1 ? ('<a target="_blank" title="' + b.Title + '" href="' + b.TargetUrl + '">' + titleHtmlUp + '</a>') : titleHtmlUp) + '</li>' +
                    '							<li>' + (b.Status == 1 ? ('<a target="_blank" title="' + b.Title + '" href="' + b.TargetUrl + '">' + titleHtmlDown + '</a>') : titleHtmlDown) + '</li>' +
                    '						</ul>' +
                    '					</td>' +
                    '					<td><span class="webSpan" title="' + b.SiteName + '">' + b.SiteName + '</span></td>' +
                    '					<td width="65">' +
                    '						' +
                    '							先删后发' +
                    '					</td>' +
                    '					<td><span class="loginName" title="' + b.SiteUserName + '">' + b.SiteUserName + '</span></td>' +
                    '					<td><span  class="loginName" title="' + b.Msg + '">' + (b.Status == 1 ? b.Msg : "<font color='red' >"+b.Msg+"</font>") + '</span>' +
                    '<a href="javascript:void(0)" onclick="seeAbout(\'' + escape(b.RealyMsg) + '\',1)">查看</a>' +
                    '					</td>' +
                    '					<td>' + (b.Status == 2 ? b.ShortBetinTime : b.ShortDateTime) + '</td>' +
                    '				</tr>';
        });

    }
    html += '		</tbody>' +
                  '	</table>' +
                  '	<!-- 表格下面 -->' +
                  '	<div class="saleManager-bottom">' +
                  '		<div class="qx" style="display:none;"><input type="checkbox" id="houseLog-all"><label for="houseLog-all">全选</label></div>' +
                  '		<div class="float-l" style="display:none;">' +
                  '			<input type="button" class="btn_g" value="删除" onclick="checkBuildingNum()" id="del_button">' +
                  '		</div>' +
                  '		<div class="float-r">' +
                  '			<div id="saleManager-fanye" class="commom-fanye jPaginate" style="padding-left: 64px;"></div>' +
                  '		</div>' +
                  '	</div>' +
                  '</div>';

    return html;
}

/*发布日志->房源查看的html显示*/
function BuildLogHtmlByHouse(datas) {
    var data = datas.data;
    var html = "";

    html += '<div class="houseLog lookAsHouse con2">' +
          '	<table class="h_list" width="100%" cellspacing="0" cellpadding="0s">' +
          '		<thead><tr><td>房源编号</td><td>标题</td><td>次数</td><td>面积</td><td>价格</td><td>楼层</td></tr></thead>' +
          '		<tbody>';
    if (data != null && data.length > 0) {
        $.each(data, function (a, b) {
            html += '				<tr>' +
                    '					<td width="60">' +
                    b.InfoID +
                    '					</td>' +
                    '					<td style="text-align:left" class="building-title">' +
                    '						<a title="(' + b.CommunityName + ')' + b.Title + '" href="javascript:buildingToLog(' + b.InfoID + ')">' +
                    '							(' + b.CommunityName + ')<span>' + b.Title + '</span>' +
                    '						</a>' +
                    '					</td>' +
                    '					<td><span>' +
                    b.PostCount +
                    '					次</span></td>' +
                    '					<td>' + b.BuildArea + '㎡</td>' +
                    '						<td>' + b.Price + '' + b.PriceUnit + '</td>' +
                    '					<td>' + b.CurFloor + '/' + b.MaxFloor + '</td>' +
                    '				</tr>';
        });
    }
    html += '		</tbody>' +
            '	</table>' +
            '	<!-- 表格下面 -->' +
            '	<div class="saleManager-bottom">' +
            '		<div class="float-r">' +
            '			<div id="saleManager-fanye" class="commom-fanye jPaginate" style="padding-left: 64px;"></div>' +
            '		</div>' +
            '	</div>' +
            '</div>';

    return html;
}

/*发布日志->网站查看的html显示*/
function BuildLogByWeiSite(datas) {
    var data = datas.data;
    var timeList = datas.timeList;
    var html = "";
    html += '<div class="houseLog lookAsWebsite con3">' +
            '  <table class="h_list" width="100%" cellspacing="0" cellpadding="0s">' +
            '    <thead>' +
            '      <tr>' +
            '        <td>网站列表</td>';
    $.each(timeList, function (a, b) {
        var timesplit = b.split('-');
        html += '    <td>' + timesplit[0] + '月' + timesplit[1] + '日</td>';
    });
    html += '        <td>合计</td>' +
            '      </tr>' +
            '    </thead>' +
            '    <tbody>';
    if (data != null && data.length > 0) {
        $.each(data, function (siteindex, itemSite) {
            html += '  <tr>' +
                    '  <td style="text-align:left;padding-left:10px;width:160px"><a class="lookAsWebsite-link" href="javascript:webToLog(' + itemSite.SiteID + ', \'' + itemSite.SiteName + '\')"><span class="website-img"><img width="54" height="24" src="' + itemSite.Logo + '"></span><span class="website-list">' + itemSite.SiteName + '</span></a></td>';
            $.each(timeList, function (timeindex, itemTime) {
                html += '<td>' + findTimeCount(itemSite.SiteTimeLogList, itemTime) + ' </td>';
            });
            html += '        <td>' + itemSite.TimeAllCount + '</td>' +
                    '      </tr>';
        });
    }
    html += '    </tbody>' +
            '  </table>' +
            '</div>';
    return html;
}
function findTimeCount(timeStatList, time) {
    var count = 0;
    for (var i = 0; i < timeStatList.length; i++) {
        if (timeStatList[i].time == time) {
            count = timeStatList[i].count;
            break;
        }
    }
    return count;
}



function webToLog(webId, webName) {
    $("#webId").val(webId);
    $("#webSapn").html(webName);
    ajaxSub(1,1);
}

function buildingToLog(buildingId) {
    $("#buildingId").val(buildingId);
    $("#title").val(buildingId);
    $("#index").val(1);
    ajaxSub(1, 1);
}

//验证是否选择房源
function checkBuildingNum() {
    var i = 0;
    $('input[name="taskCheck"]:checked').each(function () {
        i++;
    });
    if (i > 0) {
        alertDelete(i);
    } else {
        art.dialog({
            content: '请先选择需要删除的房源记录',
            ok: true
        });
    }
}

function alertDelete(i) {
    art.dialog.confirm_3(
		"已选中" + i + "条记录<br>是：同步删除对应网站的发布记录<br>否：只删除本站记录",
		function () {
		    deleteTask(1);
		},
		function () {
		    deleteTask(0);
		},
		function () {
		}
	);
}

//删除房源
function deleteTask(deleateType) {
    if (isCanSubmit) {
        var chk_value = [];
        chk_value.push({
            name: 'deleateType',
            value: deleateType
        });
        var infoTip = '';
        $('input[name="taskCheck"]:checked').each(function (i) {
            chk_value.push({
                name: 'taskId',
                value: $(this).val()
            });
            if ($(this).attr("infoId") == null || $(this).attr("infoId") == '') {
                infoTip += (i + 1) + " ";
            }
        });

        if (infoTip != '') {
            art.dialog({
                title: "提示",
                content: "选中的第 " + infoTip + " 条房源因无法取得对应网站数据，无法进行全网删除。",
                ok: true
            });
        }

        $.ajax({
            type: "post",
            url: $("#basePath").val() + "/ajax/rellog/deleteTask.do",
            data: chk_value,
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                isCanSubmit = false;
                parent.loadingShow();
            },
            success: function (result) {
                var tipListStr = "," + result.tipList + ",";
                var tipFlag = false;
                $('input[name="taskCheck"]:checked').each(function (i) {
                    if (tipListStr == null || tipListStr.indexOf("," + i + ",") == -1) {
                        $("#htr" + $(this).val()).remove();
                    } else {
                        tipFlag = true;
                    }
                });
                if (tipFlag) {
                    art.dialog({
                        title: "提示",
                        content: "剩余选中房源因未绑定网站账号，无法进行全网删除。",
                        ok: true
                    });
                }
            },
            error: function (jqXHR) {
                alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete: function (XMLHttpRequest, textStatus) {
                isCanSubmit = true;
                parent.loadingHide();
            }
        });
    }
}

//显示错误原因
function getFailreason(id) {
    $.ajax({
        type: "post",
        url: $("#basePath").val() + "/ajax/rellog/getFailreason.do",
        data: {
            failreasonId: id
        },
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            parent.loadingShow();
        },
        success: function (result) {
            var content = "";
            if (result == null) {
                content = "未知原因";
            } else {
                content = result.failReason;
            }

            art.dialog({
                content: content,
                ok: true
            });
        },
        error: function (jqXHR) {
            //				alert($.parseJSON(jqXHR.responseText).msg);
        },
        complete: function (XMLHttpRequest, textStatus) {
            parent.loadingHide();
        }
    });
}

function redirectImport(url, param, postType) {
    var id;
    if (postType == 0) {
        id = "importSell";
        parent.$("#postTypeIndex").val("1");
    } else {
        id = "importRent";
        parent.$("#postTypeIndex").val("2");
    }
    parent.$("#" + id).parents(".menu").addClass("menu_open");
    parent.$("#" + id).parents(".menu").siblings().removeClass("menu_open");
    parent.$("#" + id).click();
    parent.loadingShow();
    window.location.href = $("#basePath").val() + url + param + "&postType=" + postType;
}

/*查询区*/
function selectOption(on) {
    var obj = $("." + on).siblings("li").find(".manage-list-content");
    $("body").find(".manage-list-content").not(obj).hide();
    if (obj.css("display") == "block") {
        obj.hide();
    } else {
        obj.show();
    }
}

function GetEnableWebSite() {
    $.ajax({
        type: 'post',
        url: '/House/GetEnableWebSite',
        success: function (data) {
            var html = ' <li value="0">全部网站</li>';
            $.each(data, function (a, b) {
                html += ' <li value="' + b.SiteID + '" title="' + b.SiteName + '">' + b.SiteName + '</li>';
                if ($("#webId").val() == b.SiteID)
                {
                    $("#webSapn").html(b.SiteName);
                }
            });
            $("#webUl").html(html);
            
        }
    });
}
///根据条件获取房源小区名称
function getHouseCommunityList() {
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/House/UserHouseCommunityList',
        success: function (data) {
            var html = "<li value=\"\">全部小区</li>";
            $.each(data, function (a, i) {
                if (i.Name != "") {
                    html += "<li  value=\"" + i.Name + "\" title=\"" + i.Name + "\">" + i.Name + "</li>";
                }
            });
            $("#cellUl").html(html);

        }
    });
}

/*查看房源*/
function seeAbout(content, flag) {
    content = unescape(content);
    content = content.replace("[", "");
    content = content.replace("]", "");
    var str;
    if (flag == 1) {
        str = content.split(", 【");
    } else {
        str = content.split(", ");
    }

    var html = '';
    for (var i = 0; i < str.length; i++) {
        var title = "<span>";
        if (flag == 1) {
            if (i > 0) {
                title = "【" + str[i];
            } else {
                title = str[i];
            }
        } else {
            title = "【" + str[i] + "】";
        }
        html += title + "</span><br>";
    }

    art.dialog({
        title: '查看',
        content: html,
        width: 480,
        height: 60,
        ok: true
    });
}
