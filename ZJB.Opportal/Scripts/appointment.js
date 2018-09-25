$(function(){
	handleTypeArray = ['A','UT','UB','D','E'];
	
	$("#handleType_ul li").click(function(){
		$("#handleType_span").html($(this).html());
		$("#handleType").val(handleTypeArray[$(this).val()]);
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
	$.ajax({
		url: $("#basePath").val() + "/ajax/appointment/getAppointment.do",
		type: "post",
		data: {
			"postType" : $("#postType").val(),
			"pageIndex" : pageIndex
		},
		dataType: "text",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function(result) {
			if (pageIndex > 1) {
				var schedDiv = $("#scheduleDiv");
				$("#saleReserve").html(schedDiv);
				$("#saleReserve").append(result);
			} else {
				$("#saleReserve").html(result);
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
    var data = result.data;
    var totalSize = result.totalSize;
    var html = "";
    html += '<table class="h_list clear" width="100%" cellspacing="0" cellpadding="0s">' +
            '	<thead><tr>'+
            '<td>房源编号</td>'+
            '<td id="td_1">操作时间</td>'+
            '<td id="td_2">预约时间</td>'+
            '<td id="td_3">状态</td>' +
            '<td>相关房源</td>'+
            '<td>相关网站</td>'+
            '</tr></thead>' +
            '<tbody id="appTable">';
    if (data.length > 0)
    {
        $.each(data, function (i, item) {
            html +=
                    '			<tr id="tr_'+item.HouseID+'">' +
                    '				<td>' + item.HouseID + '</td>' +
                    '				<td align="center">' +
                    '					<span>'+item.AddTime+'</span>' +
                    '				</td>' +
                    '				<td align="center">' +
                    '				<span class="reserveRel-href reserveRel-webSite" title="联合网">预约时间</span>' +
                    '					<span class="reserveRel-href  ml8 col-f60">共1条</span>' +
                    '					<a href="javascript:seeAbout(\'[联合网]\', 2)" class="reserveRel-href ml8">' +
                    '						查看' +
                    '					</a>' +
                    '				</td>' +
                    '				<td align="center">' +
                    '					<span>' +
                    '							新增' +
                    '					</span>' +
                    '				</td>' +
                    '				<td align="center">' +
                    '				</td>' +
                    '				<td style="width:320px;text-align:left">' +
                    '					<span class="reserveRel-href reserve-house">【1735998】【麦田认证房】水晶森林 南北通透 性价比高 总价最低 只有一套</span>' +
                    '					<span class="reserveRel-href  ml8 col-f60">共1条</span>' +
                    '					<a href="javascript:seeAbout(\'[【1735998】【麦田认证房】水晶森林 南北通透 性价比高 总价最低 只有一套]\', 1)" class="reserveRel-href ml8">' +
                    '						查看' +
                    '					</a>' +
                    '				</td>' +
                    '				<td style="width:150px;text-align:left">' +
                    '					<span class="reserveRel-href reserveRel-webSite" title="联合网">联合网</span>' +
                    '					<span class="reserveRel-href  ml8 col-f60">共1条</span>' +
                    '					<a href="javascript:seeAbout(\'[联合网]\', 2)" class="reserveRel-href ml8">' +
                    '						查看' +
                    '					</a>' +
                    '				</td>' +
                    '			</tr>';
        });
    }
    html+=         '		</tbody>' +
                   '</table>';
    return html;
}
///
///出租、出售预约html

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
		url: $("#basePath").val() + "/ajax/appointment/getAppointmentLog.do",
		type: "post",
		data: {
			"date" : $("#log_date").val(),
			"handleType" : $("#handleType").val(),
			"pageIndex" : pageIndex
		},
		dataType: "text",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function(result) {
			$("#saleReserve").html(result);
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

function deleteApp() {
	var chk_value = [];
	var buildingIdMap = new Map();
	$('input[name="reserveRel"]:checked').each(function() {
		chk_value.push({
			name : 'appointmentId',
			value : $(this).val()
		});
		
		var titles = $(this).attr("titles");
		titles = titles.replace("[", "");
		titles = titles.replace("]", "");
		var titleArray = titles.split(", 【");
		
		for (var i = 0; i < titleArray.length; i++) {
			var title = titleArray[i];
			var index = title.indexOf("】");
			var buildingId;
			if (i == 0) {
				buildingId = title.substring(1, index);
			} else {
				buildingId = title.substring(0, index);
			}
			if (buildingIdMap.get(buildingId) == null) {
				buildingIdMap.put(buildingId, 1);
			} else {
				buildingIdMap.put(buildingId, buildingIdMap.get(buildingId) + 1);
			}
		}
	});
	
	var buildingArray = buildingIdMap.arr;
	var size = buildingArray.length;
	for (var i = 0; i < size; i++) {
		var building = buildingArray[i];
		chk_value.push({
			name : 'buildingId',
			value : building.key + ":" + building.value
		});
	}
	
	if (chk_value == undefined || chk_value.length < 1) {
		art.dialog({
			content : "请选择需删除的预约任务",
			ok : true
		});
		return ;
	}
	
	$.ajax({
		url: $("#basePath").val() + "/ajax/appointment/deleteAppointment.do",
		type: "post",
		data: chk_value,
		dataType: "json",
		success: function(result) {
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
		
		
		title = title.length > 30 ? title.substring(0, 30) : title;
		html += title + "</span><br>";
	}
	
	art.dialog({
	    title : '查看',
	    content : html,
	    width:480,
		height:60,
		ok:true
	});
}
