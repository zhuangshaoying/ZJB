$(function () {
    getAppointment(1);
    $("#handleType_ul li").click(function () {
		$("#handleType_span").html($(this).html());
		$("#handleType").val($(this).attr("status"));
		$(".manage-list-content").hide();
		getAppointmentLog(1);
	});
});

function queryScheduleInfo() {
	top.loadingShow();
	window.location.href = "../schedule/getScheduleList.do?postType=" + $("#postType").val();
}

function showResult(rs) {
	art.dialog({
	    content : rs,
	    ok : true
	});
}

function selectAll() {
	if($("#reserveRel-all").attr("checked")){
		$("input[name=reserveRel][dis=1]").each(function(){
			$(this).attr("checked",true);
		});
	}else{
		$("input[name=reserveRel][dis=1]").each(function(){
			$(this).attr("checked",false);
		});
	}
}

function switchTab(thisObj, i) {
	$(thisObj).parent().addClass("selected");
	$(thisObj).parent().siblings().removeClass("selected");
	$("#postType").val(i);
	if(i != 0) {
		$(".sale-content-tip").show();
		$("#selectDiv").hide();
		$("#scheduleDiv").show();
		getAppointment(1);
	} else {
		$(".sale-content-tip").hide();
		$("#selectDiv").show();
		$("#scheduleDiv").hide();
		getAppointmentLog(1);
	}
}

function getAppointment(pageIndex) {
    $("#pageIndex").val(pageIndex);
    $("#reserveRel-all").attr("checked", false);
	$.ajax({
	    url: "/Appoint/GetAppointList",
		type: "get",
		data: {
		    "tradeType": $("#postType").val(),
            "status":1,
			"pi": pageIndex,
			"ps": $("#pageSize").val(),
		},
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function (result) {
		    var html = BuildAppointTradeLogHtml(result);
			if (pageIndex > 1) {
				var schedDiv = $("#scheduleDiv");
				$("#saleReserve").html(schedDiv);
				$("#saleReserve").append(html);
			} else {
			    $("#saleReserve").html(html);
			}
		},
		error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
		},
		complete:function(XMLHttpRequest, textStatus) {
			parent.loadingHide();
		}
	});
}
///预约日志html
function BuildAppointLogHtml(result)
{
    $("#OperateDiv").hide();
    var statusMap = ["已停止","预约","正在执行","完毕"];
    var data = result.data;
    var totalSize = result.totalSize;
    var html = "";
    html += 
            '<thead><tr>'+
            '<td>编号</td>'+
            '<td id="td_1">操作时间</td>'+
            '<td id="td_2">执行时间</td>'+
            '<td id="td_3">状态</td>' +
            '<td>相关房源</td>'+
            '<td>相关网站</td>'+
            '</tr></thead>' +
            '<tbody id="appTable">';
    if (data.length > 0)
    {
        $.each(data, function (i, item) {
            var orderTimeHtml = "";
            if (item.OrderTime.length > 1) {
                orderTimeHtml = '		<a href="javascript:seeAbout(\'' + item.OrderTime + '\', 2)">' + item.OrderTime.length + '条记录'
                    '					</a>';
            }
            else {
                orderTimeHtml = '<span>' + item.OrderTime[0] + '</span>';
            }
            var orderSiteHtml = "";
            if (item.OrderSites.length > 1){
                orderSiteHtml =  '		<a href="javascript:seeAbout(\'' + item.OrderSites + '\', 2)">'+ item.OrderSites.length + '条记录' +
                    '					</a>';
            }
            else {
                orderSiteHtml = '<span>' + item.OrderSites[0] + '</span>';
            }
            html +=
                    '			<tr id="tr_'+item.HouseID+'">' +
                    '				<td width="40">' + item.HouseID + '</td>' +
                    '				<td width="125">' +
                    '					<span>'+item.AddTime+'</span>' +
                    '				</td>' +
                    '				<td width="115">' +
                   orderTimeHtml+
                    '				</td>' +
                    '				<td width="60">' +
                    '					<span>' +
                    '							' + ((item.OrderStatus==-1)?"删除":statusMap[item.OrderStatus]) +
                    '					</span>' +
                    '				</td>' +
                    '				<td width="320">' +
                    '					<span>' + item.Title + '</span>' +

                    '				</td>' +
                    '				<td width="90">' +
                  orderSiteHtml+
                    '				</td>' +
                    '			</tr>';
        });
    }
    html += '		</tbody>';
    if (totalSize > 0) {
        var pageSize = parseInt($("#pageSize").val());
        var pagecount = parseInt(totalSize / pageSize) + ((totalSize % pageSize) == 0 ? 0 : 1);
        $("#saleManager-fanye").paginate({
            count: pagecount,
            start: $("#pageIndex").val(),
            display: 10,
            border: false,
            text_color: '#50b63f',
            text_hover_color: '#fff',
            background_color: '#fff',
            background_hover_color: '#50b63f',
            images: false,
            mouse: 'click',
            onChange: function () {
                getAppointmentLog($(".jPag-current").html());
            }
        });
    } else {
        $("#saleManager-fanye").html("");
    }
    return html;
}
///
///出租、出售预约html
function BuildAppointTradeLogHtml(result)
{
    $("#OperateDiv").show();
    var data = result.data;
    var totalSize = result.totalSize;
    var html = "";
    html+=      '<table class="h_list clear" width="100%" cellspacing="0" cellpadding="0s">' +
                '<thead><tr>'+
                '<td>编号</td>' +
                '<td>执行时间</td>' +
                '<td>相关房源</td>' +
                '<td>相关网站</td>'+
                '</tr></thead>' +
                '<tbody id="appTable">';
    if (data.length > 0) {
        $.each(data, function (i, item) {
            var orderTimeHtml = "";
            if (item.OrderTime.length > 1) {
                orderTimeHtml = '		<a href="javascript:seeAbout(\'' + item.OrderTime + '\', 2)" class="ml8">' + item.OrderTime.length + '条执行时间' +
                    '					</a>';
            }
            else {
                orderTimeHtml = '<span  id="dateSpan' + item.HouseID + '">' + item.OrderTime[0] + '</span>';
            }
            var orderSiteHtml = "";
            if (item.OrderSites.length > 1) {
                orderSiteHtml = '			<a href="javascript:seeAbout(\'' + item.OrderSites + '\', 2)" class="ml8">'+ item.OrderSites.length + '个相关网站'+
                    '					</a>';
            }
            else {
                orderSiteHtml = '<span>' + item.OrderSites[0] + '</span>';
            }
            html +=
    '						<tr id="tr_' + item.HouseID + '">' +
    '									<td width="60">' +
    '										<input class="appointManager-state-checkbox" type="checkbox" name="reserveRel" value="' + item.HouseID + '" titles="' + item.Title + '" id="' + item.HouseID + '" dis="1" ><br><label for="' + item.HouseID + '">' + item.HouseID + '</label>' +
   '									</td>' +
    '									<td width="120">' +
    orderTimeHtml +
    '									</td>' +
    '							<td width="400">' +
    '								<span>' + item.Title + '</span>' +
    '							</td>' +
    '							<td width="170">' +
    orderSiteHtml +
    '							</td>' +
    '						</tr>';
        });
        html += '<tr id="releaseDiv" class="push_buttom_quick" style="display:none;border:none;background-color:transparent">' +
   '              <td class="tip_inner" style="border:none;margin:0 20px;text-align:left;height:60px;line-height:60px;font-size:12px;background-color:transparent;padding:0 20px"><a class="close" onclick="closeReleaseDiv()"></a> 选中<b class="col-f60 release-sum">1</b>条，' +
   '                <input id="releaseNow" onclick="UpdatereleaseClick(0)" type="button" value="修改预约" class="btn_o release-manager-btn">' +
   '            </tr>';
        '				</tbody>' +
        '			</table>';
    }

    if (totalSize > 0) {
        var pageSize = parseInt($("#pageSize").val());
        var pagecount = parseInt(totalSize / pageSize) + ((totalSize % pageSize) == 0 ? 0 : 1);
        $("#saleManager-fanye").paginate({
            count: pagecount,
            start: $("#pageIndex").val(),
            display: 10,
            border: false,
            text_color: '#50b63f',
            text_hover_color: '#fff',
            background_color: '#fff',
            background_hover_color: '#50b63f',
            images: false,
            mouse: 'click',
            onChange: function () {
                getAppointmentLog($(".jPag-current").html());
            }
        });
    } else {
        $("#saleManager-fanye").html("");
    }
    return html;
}
///

function selectOption(on){
	$("."+on).mouseover(function(){
		$(this).find(".manage-list-content").show();
	});
	
	$("."+on).mouseout(function(){
		$(this).find(".manage-list-content").hide();
	});
}

function getAppointmentLog(pageIndex) {
	$("#pageIndex").val(pageIndex);
	$.ajax({
	    url: "/Appoint/GetAppointList",
		type: "get",
		data: {
			"time" : $("#log_date").val(),
			"status" : $("#handleType").val(),
			"pi": pageIndex,
			"ps": $("#pageSize").val()
		},
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function (result) {
		    var html = BuildAppointLogHtml(result);
		    $("#saleReserve").html(html);
		},
		error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
		},
		complete:function(XMLHttpRequest, textStatus) {
			parent.loadingHide();
		}
	});
}


/*修改时间*/
function modifyDateTime(id){
	dateTimeStr = $("#dateSpan" + id).html();
	i = dateTimeStr.indexOf(" ");
	j = dateTimeStr.indexOf(":");
	dStr = dateTimeStr.substring(0, i);
	hStr = dateTimeStr.substring(i + 1, j);
	mStr = dateTimeStr.substring(j + 1, dateTimeStr.length);
	
	var html = '<select id="dateSelect">';
	
	//生成日期
	var d = new Date();
	
	for (var i = 0; i < 7; i++) {
		var year = d.getFullYear();
		var month = d.getMonth() + 1;
		var day = d.getDate();
		month = month < 10 ? "0" + month : month;
		day = day < 10 ? "0" + day : day;
		
		var date = year + "-" + month + "-" + day;
		
		html += '<option value="' + date + '">' + date + '</option>';
		d.setDate(d.getDate() + 1);
	}
	
	html += '</select>&nbsp;<select id="hourSelect">';
	//生成小时
	var hour = '';
	for (var i = 0; i < 24; i++) {
		if (i < 10) {
			hour = "0" + i;
		} else {
			hour = i;
		}
		html += '<option value="' + hour + '">' + hour + '</option>';
	}
	html += '</select>时&nbsp;<select id="minuteSelect">';
	//生成分钟
	var minute ='';
	for (var i = 0; i < 60; i++) {
		if (i < 10) {
			minute = "0" + i;
		} else {
			minute = i;
		}
		html += '<option value="' + minute + '">' + minute + '</option>';
	}
	html += '</select>分';

	art.dialog({
		title:'修改时间',
		content : html,
		init : function () {
			$("#dateSelect").val(dStr);
			$("#hourSelect").val(hStr);
			$("#minuteSelect").val(mStr);
		},
		width:280,
		height:60,
		ok : function(){
			dateStr = $("#dateSelect").val();
			hourStr = $("#hourSelect").val();
			minuteStr = $("#minuteSelect").val();
			timeStr = hourStr + ":" + minuteStr;
			var d = dateStr.split("-");
			
			var date_1 = new Date(d[0], d[1] - 1, d[2], hourStr, minuteStr , '0');
			var date_2 = new Date();
			if (date_1.getTime() < date_2.getTime()) {
				art.dialog({
					content : "预约时间必须大于当前时间",
					ok : true
				});
			} else {
				updateTime(id, dateStr, timeStr);
			}
		},
		cancel:true
	});
}

function updateTime(id, dateStr, timeStr) {
	date = dateStr + " " + timeStr + ":00";
	$.ajax({
		url: $("#basePath").val() + "/ajax/appointment/updateTime.do",
		type: "post",
		data: {
			"appointmentId" : id,
			"date" : date
		},
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function(result) {
			$("#dateSpan" + id).html(dateStr + " " + timeStr);
		},
		error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
		},
		complete:function(XMLHttpRequest, textStatus) {
			parent.loadingHide();
		}
	});
}
///删除预约
function deleteApp() {
	var chk_value = [];
	var buildingIdMap = new Map();
	$('input[name="reserveRel"]:checked').each(function() {
		chk_value.push({
		    name: 'HouseIds',
			value : $(this).val()
		});
	});
	

	
	if (chk_value == undefined || chk_value.length < 1) {
		art.dialog({
			content : "请选择需删除的预约任务",
			ok : true
		});
		return ;
	}
	
	$.ajax({
	    url: "/Appoint/DeleteAppoint",
		type: "post",
		data: chk_value,
		dataType: "json",
		success: function (result) {
		    getAppointment(1);
		},
		error: function(jqXHR) {
			alert($.parseJSON(jqXHR.responseText).msg);
		}
	});
}

/*查看房源*/
function seeAbout(content, flag){
	content = content.replace("[","");
	content = content.replace("]","");
	var str;
	if (flag == 1) {
		str = content.split(", 【");
	} else {
		str = content.split(",");
	}
	
	var html = '';
	for (var i = 0; i < str.length; i++) {
		var title = "<span style='display:block;line-height:24px'>";
		if (flag == 1) {
			if (i > 0) {
				title = "" + str[i];
			} else {
				title = str[i];
			}
		} else {
		    title += "" + str[i] + "</span>";
		}
		//title = title.length > 30 ? title.substring(0, 30) : title;
		html += title;
	}
	art.dialog({
	    title : '查看执行时间',
	    content : html,
	    width:160,
		height:60,
		ok:true
	});
}
