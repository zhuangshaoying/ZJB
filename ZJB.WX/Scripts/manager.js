//
var checkWebArray = ['不发送', '删除旧房源再发送'];
var reRlsCheckWebArray = ['不发送', '先删除后发送', '强制发送'];
var sharHouseOrg;
$(function() {
	//生成日期
	var nowDates = $("#nowDate").val().split("-");
	var d = new Date(nowDates[0], (nowDates[1] - 1), nowDates[2]);
	
	for (var i = 0; i < 7; i++) {
		var year = d.getFullYear();
		var month = d.getMonth() + 1;
		var day = d.getDate();
		
		month = month < 10 ? "0" + month : month;
		day = day < 10 ? "0" + day : day;
		
		var date = year + "-" + month + "-" + day;
		if (i == 0) {
			$("#dateSpan_1").html(date);
			$("#dateSpan_2").html(date);
		}
		$("#dateUl_1").append('<li>' + date + '</li>');
		$("#dateUl_2").append('<li>' + date + '</li>');
		d.setDate(d.getDate() + 1);
	}
	
	initSelectUl('hourUl', 0, 23, 1);
	initSelectUl('minnuteUl', 0, 59, 1);
	initSelectUlDesc('appMinUl', 60, 1, 1);
	initSelectUlDesc('appNumUl', 10, 1, 1);
	
	function initSelectUl(id, begin, end, multiple) {
		var html = '';
		for (var i = begin; i <= end; i++) {
			var value = i * multiple;
			html += '<li value"' + value + '">' + value + '</li>';
		}
		$("#" + id).append(html);
	}
	
	function initSelectUlDesc(id, begin, end, multiple) {
		var html = '';
		for (var i = begin; i >= end; i--) {
			var value = i * multiple;
			html += '<li value"' + value + '">' + value + '</li>';
		}
		$("#" + id).append(html);
	}
	
	// 后台重定向时，左侧菜单联动
	function managerClick() {
		var postType = $('#postType').val();
		if (postType == 1) {
			id = "houseManagerSell";
		} else {
			id = "houseManagerRent";
		}
		parent.$("#" + id).click();
	}

 
	managerClick();
	
	var height = document.documentElement.clientHeight;
	$(".saleHouse-main").css("min-height",height-56);
	$("#title").placeholder();
	
	/*选择网站上一步*/
	$("#selectWebsite_on").click(function(){
		$(".selectHouse").show();
		$(".selectWebsite").hide();
	});
	
	/*继续推送*/
	$("#continue-sf").click(function(){
		$(".selectFinish").hide();
		$(".selectHouse").show();
	});
	
	/*继续推送*/
	$("#continue-set").click(function(){
		var checkbox = $(".saleManager-state-checkbox");
		$(".selectFinish").hide();
		$(".selectHouse").show();
		checkbox.each(function(){
			$(this).attr("checked",false);
		});
	});
	
	/*选择房源表格全选*/
	$("#saleManager-all").click(function(){
		$("#releaseDiv").hide();
		if($("#saleManager-all").attr("checked")){
			$(".saleManager-state-checkbox").each(function(){
				$(this).attr("checked",true);
			});
		}else{
			$(".saleManager-state-checkbox").each(function(){
				$(this).attr("checked",false);
			});
		}
	});

	/*选择网站表格全选*/
	$("#website-all").click(function(){
		if($("#website-all").attr("checked")){
			$(".website-accout input:checkbox").each(function(){
				if ($(this).attr("dis") != 1) {
					$(this).attr("checked",true);
				}
			});
		}else{
			$(".website-accout input:checkbox").each(function(){
				$(this).attr("checked",false);
			});
		}
	});
		
	$(".manage-time-content li").click(function(){
		var content = $(this).html();
		$("#sort").val($(this).val());
		$(".manage-time-tip span").html(content);
		$(".manage-list-content").hide();
		getHousebyCondition();
	});
	
	$(".manage-tags-content li").click(function(){
		var content = $(this).html();
		if(content == '不限'){
			$("#tagsTip").val("");
			$(".manage-tags-tip span").html("房源标签");
		}else{
			$("#tagsTip").val($(this).attr("tags"));
			$(".manage-tags-tip span").html(content);
		}
		$(".manage-list-content").hide();
		getHousebyCondition();
	});
	
	$(".select_li").die().live("click", function(){
		$(this).parent().parent().prev().find("input").val($(this).attr("liValue"));
		$(this).parent().hide();
		getHousebyCondition();
	});
	
	/*预约发送选择区*/
	$(".manage-date1-content li").click(function(){
		var content = $(this).html();
		$("#date1").val($(this).val());
		$(".manage-date1-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-date2-content li").click(function(){
		var content = $(this).html();
		$("#date2").val($(this).val());
		$(".manage-date2-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-time1-content li").click(function(){
		var content = $(this).html();
		$("#time1").val($(this).val());
		$(".manage-time1-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-time2-content li").click(function(){
		var content = $(this).html();
		$("#time2").val($(this).val());
		$(".manage-time2-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-appMin-content li").click(function(){
		var content = $(this).html();
		$(".manage-appMin-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-appNum-content li").click(function(){
		var content = $(this).html();
		$(".manage-appNum-content span").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-list-content").mouseleave(function() {
		$(".manage-list-content").hide();
		$(".manage-list-content").parent().prev().find("input").blur();
	});
	
	$("a[name=link_share_type]").live("click", function() {
		var shareLink = $(this);
		var cloneFrom = shareLink.attr("cloneFrom");
		if (cloneFrom > 0) {
			art.dialog.alert("克隆房源不可共享！");
			return;
		}
		var buildingId = shareLink.attr("buildingId");
		var shareOrgId = shareLink.attr("shareOrgId");
		sharHouseOrg = art.dialog.open("/House/GetHouesShareCompany?houseId=" +
				buildingId + "&shareOrgId=" + shareOrgId + "&from=0", {
			title: '修改共享范围',
			width: 280,
			height: 340,
			lock:true,
			esc:false
		});
	});
	
	$("#btn_share").click(function() {
		var tags = $("input[name=buildCheck]:checked");
		if (tags == null || tags.length == 0) {
			art.dialog.alert("请至少选择一条房源！");
			return;
		}
		
		var params = "?shareOrgId=0&from=0";
		$.each(tags, function(i, n) {
			params += ("&houseId=" + tags.eq(i).val());
		});
		
		sharHouseOrg = art.dialog.open("/House/GetHouesShareCompany" + params, {
			title: '修改共享范围',
			width: 265,
			height: 340,
			lock:true,
			esc:false
		});
	});
	
	$("a[name=link_tag]").live("click", function() {
		var tagLink = $(this);
		var buildingId = tagLink.attr("buildingId");
		var tags = tagLink.attr("tags");
	    //"/House/UpdateHouseTagsView?tags=" + tags, 
		art.dialog({
		    content: document.getElementById("updateTagsDiv"),
		    init:function(){
		        $("#updateTags").val(tags);
		        $.each($("input[name=SetingTag]"), function (i, a) {
		            $(this).attr("checked", false);
		        });
		        if (tags != null && tags.length > 0) {
		            var arr = tags.split(",");

		            $.each(arr, function (i, tag) {
		                $("#tag" + tag).attr("checked", true);
		            });
		        }
		    },
			width: 140,
			title: "设置标签",
			lock: true,
			padding:0,
			ok: function() {
				var params = [], tagStr = "";
				params.push({
					name: "HouseIds",
					value: buildingId
				});
				var tagStr = $("#updateTags").val();
				params.push({
					name: "tag",
					value: tagStr
				});
				
				$.ajax({
					url: $("#basePath").val() + "/House/batchUpdateHouseTags",
					type: "post",
					data: params,
					success: function() {
						var html = "";
						if (tagStr.length > 0) {
							var tagArr = tagStr.split(",");
							$.each(tagArr, function (i, tag) {
							    if (tagMap.get(tag) != null) {
							        html += '<span class="tags_pics"><img src="' + tagMap.get(tag) + '"></img></span>';
							    }
							});
						}
						$("#tags_pic_" + buildingId).html(html);
						tagLink.attr("tags", tagStr);
					}
				});
			},
			cancel: true
		});
	});
	///标签选择
	$("input[name=SetingTag]").change(function () {
	    var value = "";
	    var tagNum = 0;
	    $.each($("input[name=SetingTag]:checked"), function (i, obj) {
	        value += "," + $(this).val();
	        tagNum++;
	        if (i > 2) {
	            alert("最多选择3个标签");
	            $(this).attr("checked", false);
	        }
	    });
	    if (tagNum <= 3) {
	        if (value.length > 0) {
	            value = value.substring(1);
	        }
	        $("#updateTags").val(value);
	    }
	});

   
	$("#btn_import").click(function() {
		var postType = $(this).attr("pt");
		art.dialog.open("importBuilding.html?postType=" + postType, {
			title: "房源导入",
			width: 600,
			height: 400,
		    close: function () {
		    	getHousebyCondition();
		    }
		});
	});
	
});

/*弹出提示框*/
$(".saleManager-state-checkbox").die().live("click", function() {
	if ($("#isDraft").val() == 0) {
		/*计算选择条数*/
		var sum = 0;
		$(".saleManager-state-checkbox").each(function(){
			if($(this).attr("checked")){
				sum++;	
			}
		});
		$(".release-sum").html(sum);
		if($(this).attr("checked")){
			var X = $(this).offset().top;
			var Y = $(this).offset().left; 
			//$("#releaseDiv").show();
			//$("#releaseDiv").css({"top":X-32,"left":Y+25});
		}else{
			$(".saleManager-state-checkbox").each(function(){
				if($(this).attr("checked")){
					var X = $(this).offset().top;
					var Y = $(this).offset().left; 
					//$("#releaseDiv").show();
					//$("#releaseDiv").css({"top":X-32,"left":Y+25});
				}
			});
		}
		/*隐藏提示*/
		if(sum==0){
			$("#releaseDiv").hide();
		}
	}
});

/*房源类型tab切换*/
function switchTab(ProTag, freshFlag) {
	$("#tab" + ProTag).addClass("selected");
	$("#tab" + ProTag).siblings().removeClass("selected");
	$("#buildingType").val(ProTag);
	
	if (freshFlag != 0) {
		getHousebyCondition();
	}
	
	var status0 = 0;
	var status1 = 0;
	var status2 = 0;
	if (ProTag == 1) {
		status0 = value0;
		status1 = value10;
		status2 = value1;
	} else if (ProTag == 2) {
		status0 = value2;
		status1 = value11;
		status2 = value3;
	} else if (ProTag == 3) {
		status0 = value4;
		status1 = value12;
		status2 = value5;
	} else if (ProTag == 4) {
		status0 = value6;
		status1 = value13;
		status2 = value7;
	} else if (ProTag == 5) {
		status0 = value8;
		status1 = value14;
		status2 = value9;
	}
	
	$("#statusSpan").html(status0);
	$("#statusSpan1").html(status1);
	$("#statusSpan2").html(status2);
}

// 0:发送 1：预约发送
function releaseClick(flag, freshFlag){
	$("#releaseDiv").hide();
	if (checkBuildingNum()) {
		$(".selectHouse").hide();
		$(".selectWebsite").show();
		$("#isAppointment").val(flag);
		var isAppointment;
		if (flag == 0) {
			$(".selectWebsite-select").hide();
			$(".manager-rel").show();
			$(".manager-per").hide();
			isAppointment = 0;
		} else {
			$(".selectWebsite-select").show();
			$(".manager-rel").hide();
			$(".manager-per").show();
			isAppointment = 1;
			$("#hourSpan").html(roundHour());
			$("#minnuteSpan").html(roundMinute());
		}
		getWebAccount(isAppointment);
	}
}
	
/*房源状态tab切换*/
function switchStatusTab(ProTag, freshFlag) {
    for (var i = 0; i < 3; i++) {
        if ("statusLi" + i == ProTag) {
        	$("#" + ProTag).addClass("active");
        	$("#" + ProTag).siblings().removeClass("active");
        	$("#buttonDiv" + i).show();
        	$("#buttonDiv" + i).siblings().hide();
        	
        	if (i == 0) {
        		$("#updateTD").html('更新时间');
        		$("#buildingStatus").val(1);
        		$("#isDraft").val(0);
        		$("#cellDraft_Li").hide();
             	$("#cell_Li").show();
             	$(".tagsContent").show();
            } else if (i == 1) {
            	$("#buildingStatus").val(3);
             	$("#updateTD").html('删除时间');
             	$("#isDraft").val(0);
             	$("#cellDraft_Li").hide();
             	$("#cell_Li").show();
             	$(".tagsContent").hide();
            } else {
            	$("#updateTD").html('更新时间');
             	$("#buildingStatus").val(2);
             	$("#isDraft").val(1);
             	$("#cellDraft_Li").show();
             	$("#cell_Li").hide();
            	$(".tagsContent").hide();
            }
        	if (freshFlag != 0) {
        		getHousebyCondition();
        	}
        }
    }
    $(".manage-list-content").hide();
}

function redirectIndex(id){
	parent.$("#"+id).parents(".menu").addClass("menu_open");
	parent.$("#"+id).parents(".menu").siblings().removeClass("menu_open");
	parent.$("#"+id).click();
	parent.loadingShow();
}

function redirectLog(url, param) {
	var id = "releaseLog";
	redirectIndex(id);
	window.location.href = $("#basePath").val() + url + param;
}

function redirectAppointment(url, param) {
	var id = "appointment";
	redirectIndex(id);
	window.location.href = $("#basePath").val() + url + param;
}

function redirectImport(url, param) {
	var postType = $('#postType').val();
	var id;
	if (postType == 1) {
		id = "importSell";
		parent.$("#postTypeIndex").val("1");
	} else {
		id = "importRent";
		parent.$("#postTypeIndex").val("2");
	}
	redirectIndex(id);
	window.location.href = $("#basePath").val() + url + param;
}

function getWebManager() {
	var id = "webManager";
	redirectIndex(id);
	parent.hrefLink('/PersonManage/SiteManageView');
}

function getPersonSet() {
	var id = "personSet";
	redirectIndex(id);
	window.location.href = $("#basePath").val() + '/personConfig/getPersonConfig.do';
}

function enterSumbit() {
	var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异
	if (event.keyCode == 13){
		getHousebyCondition();
	}
}

var isCanSubmit = true;
// 通过条件获取房源信息
function getHousebyCondition(page) {
	page = (page == undefined) ? 1 : page;
	$("#page").val(page);
	var cell;
	if ($("#isDraft").val() == 1) {
		cell = $("#cellDraft").val();
	} else {
		cell = $("#cell").val();
	}
	cell = (cell == '小区') ? "" : cell;
	var tags = $("#tagsTip").val();
	$.ajax({
	    url: $("#basePath").val() + "/House/GetHouseList",
		type : "get",
		cache : false,
		data: {
			"postType" : $("#postType").val(),
			"buildingType" : $("#buildingType").val(),
			"buildingStatus" : $("#buildingStatus").val(),
			"cell" : cell,
			"sort" : $("#sort").val(),
			"title" : $("#title").val(),
			"isDraft" : $("#isDraft").val(),
			"tags" :tags,
			"page": page,
			"pageSize": $("#pageSize").val()
		},
		dataType: "json",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function (result) {
		    $("#houseTable").html(bulidHouseTableHtml(result));
			if ($("#isDraft").val() == 1) {
				$("#webNumTD").hide();
			} else {
				$("#webNumTD").show();
			}
			$("#saleManager-all").attr('checked', false);
		},
		error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
		},
		complete:function(XMLHttpRequest, textStatus) {
			parent.loadingHide();
		}
	});
	getHouseCommunityList();
}

function bulidHouseTableHtml(dataList)
{
    var html="";
    if (dataList.data.length > 0) {
        
        $.each(dataList.data, function (a, i) {
            var smallImagePath = i.HouseImgPath == "" ? "/images/null.png" : i.HouseImgPath;
            var bigImagePaht = smallImagePath;
            var pushTime = i.PushTime == "" || i.PushTime == "1900/1/1 0:00:00" || i.PushTime == "0001/1/1 0:00:00" ? "未推送" : i.PushTime;
            var tdWebCountHtml = "";
            //if ($("#buildingStatus").val() == "1"||$("#buildingStatus").val() =="3")
            //{
            //    tdWebCountHtml = '              <td width="45"><div id="' + i.HouseID + '">' + i.webCount + '</div>' +
            //                     '                <div><a href="javascript:getSuccessTip(\'' + i.HouseID + '\')">查看</a></div></td>';
            //}
            tdWebCountHtml = '              <td width="45"><div>' + i.Hits + '</div>' +
                                '                <div><a href="/v/' + i.HouseID + '.html"  target="_blank">查看</a></div></td>';
            var morePics = i.PicNum > 0 ? "<span class=\"more_pics\"></span>" : "";
            var yuyuePics = (i.OrderStatus >0 && i.OrderStatus<=2) ? "<span class=\"appointment_pics\"></span>" : "";
            var tagsHtml = "";
            if (i.Tag!=null&&i.Tag != ""&&i.Tag != "0") {
                var tagArr = i.Tag.split(',');
                $.each(tagArr, function (a, i) {
                    if (tagMap.get(i) != null) {
                        tagsHtml += '<span class="tags_pics"><img src="' + tagMap.get(i) + '"></img></span>';
                    }
                });
            }
            html += '<tr id="htr' + i.HouseID + '">' +
           '              <td width="60"><ul style="text-align:center">' +
           '                  <li>' +
           '                    <input class="saleManager-state-checkbox" type="checkbox" name="buildCheck" value="' + i.HouseID + '" id="bid' + i.HouseID + '" style="margin:0px">' +
           '                  </li>' +
           '                  <li>' +
           '                    <label for="bid' + i.HouseID + '">' + i.HouseID + '</label>' +
           '                  </li>' +
           '                </ul></td>' +
           '              <td width="480"><dl class="h_info">' +
           '                  <dt class="h_pic"> <a target="_blank" title="" href="' + bigImagePaht + '"> <img src="' + smallImagePath + '" width="80" height="60" original="' + smallImagePath + '"> </a> </dt>' +
           '                  <dd class="h_title"><a target="_blank" href="/v/' + i.HouseID + '.html"> <span title="' + i.Title + '"><span>' + i.Title + '</span> </span></a> </dd>' +
           '                  <dd class="eare"> <b style="color:#5b8b00; font-weight:normal"> <span class="span_title"> <span title="【' + i.DistrctName + '】' + i.CommunityName + '">【' + i.DistrctName + '】' + i.CommunityName + '</span> </span>' +
           '<div class="float-r">' + yuyuePics + '</div>' +
           '                    <div class="float-r">' + morePics + '</div>' +
           '                    <div  class="float-r" id="tags_pic_' + i.HouseID + '">' + tagsHtml + '</div>' +
           '                    </b> </dd>' +
           '                  <dd class="h_property"> <span class="float-l h-property-left"> <span>' + i.CurFloor + '/' + i.MaxFloor + '</span> / <span>' + i.Room + '室' + i.Hall + '厅</span> / ' + i.BuildArea + '㎡ / <b class="F_red">' + i.Price + '</b> ' + i.PriceUnit + ' </span> </dd>' +
           '                </dl></td>' +
           tdWebCountHtml+
           '              <td width="55">' + ($("#buildingStatus").val() == "3" ? i.DeleteTime : i.PostTime) + '</td>' +
           //'              <td width="55"> '+pushTime+' </td>' +
           '              <td width="70"><ul>' +
           '                  <li><a href="/House/GetHouse?postType=' + i.TradeType + '&&buildingType=' + i.BuildType + '&&buildingId=' + i.HouseID + '">修改</a></li>' +
           '                  <li><a name="link_share_type" buildingid="' + i.HouseID + '" clonefrom="' + i.BeColneHouseId + '" shareorgid="0">' + (i.IsShare == 1 ? "已共享" : "未共享") + '</a></li>' +
           '                  <li><a tags="'+(i.Tag==null?'':i.Tag)+'" name="link_tag" buildingid="' + i.HouseID + '" tags="">设置标签</a></li>' +
           '                </ul></td>' +
           '            </tr>';
        });
        html+= '<tr id="releaseDiv" class="push_buttom_quick" style="display:none;border:none;background-color:transparent">' +
        '              <td class="tip_inner" style="border:none;margin:0 20px;text-align:left;height:60px;line-height:60px;font-size:12px;background-color:transparent;padding:0 20px"><a class="close" onclick="closeReleaseDiv()"></a> 选中<b class="col-f60 release-sum">1</b>条，' +
        '                <input id="releaseNow" onclick="releaseClick(0)" type="button" value="发布" class="btn_o release-manager-btn">' +
        '                <input id="releaseNow" onclick="releaseClick(1)" type="button" value="预约发布" class="btn_o release-manager-yuyue"></td>' +
        '            </tr>';
        var pageSize=parseInt($("#pageSize").val());
        var pagecount = parseInt(dataList.totalSize / pageSize) + ((dataList.totalSize % pageSize) == 0 ? 0 : 1);
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
                getHousebyCondition($(".jPag-current").html());
            }
        });
    }
    else {
        html=' <tr><td colspan="6">' +
             '		<div class="null-tip saleManager-state-content con2">' +
             '			<img width="70" height="70" src="/images/icon_tip.png"></img>' +
             '			<p class="">没有满足条件的房源</p>' +
             '			<p>' +
             '				<a href="javascript:redirectImport(\'/House/GetHouse\', \'?posttype=' + $("#postType").val() + '&buildingType=' + $("#buildingType").val() + '\')" class="ui-btn"><span class="ui-btn-green"><em>立即加入</em></span></a>' +
             '			</p>' +
             '		</div>' +
             '		</td></tr>';
        $("#saleManager-fanye").html("");
    }
    return html;
}

///根据条件获取房源小区名称
function getHouseCommunityList()
{
    var postType=$("#postType").val();
    var budlingType=$("#buildingType").val();
    var budlingStatus=$("#buildingStatus").val();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/House/UserHouseCommunityList',
        data: { postType: postType, budlingType: budlingType, budlingStatus: budlingStatus },
        success:function(data){
            var html= "<li class=\"select_li\" liValue=\"小区\">不限</li>";
            $.each(data, function (a, i) {
                if (i.Name != "") {
                    html += "<li class=\"select_li\" livalue=\"" + i.Name + "\" title=\"" + i.Name + "\">" + i.Name + "</li>";
                }
            });
            if(budlingStatus!=2){
                //ul-cell-List
                $("#ul-cell-List").html(html);
            }
            else{
                //  ul-cellDraft-List
                $("#ul-cellDraft-List").html(html);
            }
        }
    });
}



// 获取可发送的网站

var sitePlaceStatus =new Map() ;///发布位置
sitePlaceStatus.put(2, [{ pid: 1, pname: "上架" },{ pid: 2, pname: "待上架" },{ pid: 3, pname: "发布低价房源" }]);//搜房
sitePlaceStatus.put(3, [{ pid: 1, pname: "不推广" }, { pid: 2, pname: "定价推广" }]);//安居客
sitePlaceStatus.put(4, [{ pid: 1, pname: "上架" }, { pid: 2, pname: "待上架" }, { pid: 3, pname: "发布低价房源" }]);//搜房无线
sitePlaceStatus.put(9, [{ pid: 1, pname: "未推广" }, { pid: 2, pname: "推广" }]);//赶集vip
sitePlaceStatus.put(10, [{ pid: 1, pname: "未推广" }, { pid: 2, pname: "推广" }]);//赶集放心房
function getWebAccount(isAppointment) {
	if (isCanSubmit) {
	    $.ajax({
		    url: $("#basePath").val() + "/House/GetEnableWebSite",
			type : "post",
			cache: false,
			data: { buildType: $("#buildingType").val(), isFilter: 1 },
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				parent.loadingShow();
				isCanSubmit = false;
			},
			success: function (result) {
			    var html = "";
			    $.each(result, function (a, i) {
                    /// 重复和库存上限html
			        var AppointMentStr = "";
			        var defaultOperation = "0_0";
			        if (window.localStorage && localStorage.getItem("siteId_" + i.SiteID)) {
			            defaultOperation = localStorage.getItem("siteId_" + i.SiteID);//本地存储
			        }
			        if (isAppointment == 0) {
			            AppointMentStr += '<td>';
			            if (i.SiteID != 2 && i.SiteID != 5) {
			                AppointMentStr += '<span><select class="webCheckSelect" id="' + i.SiteID + '_LimitOperate">' +
                                '<option value="0" ' + (i.LimitOperation == 0 ? 'selected="selected"' : '') + ' >删除旧房源再发送</option>' +
                                '<option value="1" ' + (i.LimitOperation == 1 ? 'selected="selected"' : '') + '>不发送</option>' +
                                '</select></span>';
			            }
			            AppointMentStr += '</td>';
			            AppointMentStr += '<td>';
			            if (i.SiteID >0) {
			                AppointMentStr += '<span><select class="webCheckSelect" id="' + i.SiteID + '_RepeateOperate">' +
                                '<option value="0" ' + (i.RepeatOperation == 0 ? 'selected="selected"' : '') + '>先删除后发送</option>' +
                                '<option value="1" ' + (i.RepeatOperation == 1 ? 'selected="selected"' : '') + '>不发送</option>' +
			                    '<option value="2" ' + (i.RepeatOperation == 2 ? 'selected="selected"' : '') + '>强制发送</option>' +
                                '</select></span>';
			            }
			            AppointMentStr += '</td>';
			        }
			        else if (isAppointment == 1) {
			            AppointMentStr += '<td><span></span></td>' +
                        '<td><span></span></td>';
			        }
                    ////位置 html
			        var placeStr = "<td><span>";
			        var itemSitePlace = sitePlaceStatus.get(i.SiteID);
			        placeStr += "<select class='webCheckSelect positionSelect-manager' id='" + i.SiteID + "_PlaceOperate'>";
			        if (itemSitePlace != null && itemSitePlace.length > 0) {
			           
			            for (var j = 0; j < itemSitePlace.length; j++) {
			                var selected = i.PlaceOperation == itemSitePlace[j].pid ? "selected='selected'" : "";
			                placeStr += "<option " + selected + " value='" + itemSitePlace[j].pid + "'>" + itemSitePlace[j].pname + "</option>";
			            }
			           
			        }
			        else {
			            placeStr += "<option  value='0'>上架</option>";
			        }
			        placeStr += "<select>";
			        placeStr += "</span></td>";
			        var accountInfo = "";
			        var nameClass = "";
			        if (i.IsBan == 1) {
			            nameClass = "col-f60";
			            accountInfo += "<span class='" + nameClass + "'>" + i.BanText + ",截止<font color='red'>" + i.BanTime + "</font>前不可发布</span><br/>";
			            
			        }
			        else {
			            accountInfo += '<input type="checkbox" name="webCheck" id="webId' + i.SiteID + '' + i.SiteUserName + '" value="' + i.SiteID + '" loginname="' + i.SiteUserName + '" title="' + i.SiteUserName + '">';
			        }
			        html+=  '<tr>' +
                            '<td width="120"><img width="100" height="50" src="' + i.Logo + '" title='+i.SiteName+'></td>' +
                            '<td class="website-accout" style="text-align:left;width:240px">' +
                            '<input type="hidden" id="webName_' + i.SiteID + '" value="' + i.SiteName + '">' +
                            '<span class="ml8">' +
                            accountInfo+
                            '<label style=" padding-left: 6px; "  class="'+nameClass+'" for="webId' + i.SiteID + '' + i.SiteUserName + '">' + i.SiteUserName + '</label>' +
                            '</span>' +
                            '</td>' +
                            AppointMentStr +// 库存 重复 
                            placeStr+//位置
                            '</tr>';
			    });

				$("#webAccountTable").html(html);
			},
			error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				parent.loadingHide();
				isCanSubmit = true;
			}
		});
	}
}

/*选择网站下一步*/
function nextClick() {
	var flag = $("#isAppointment").val();
	if (flag == 0) {
		releaseHouse();
	} else {
		appointmentRelease();
	}
}

function releaseHouse() {
	if (isCanSubmit && checkWebNum()) {
		var chk_value = [];
		var buildingNum = 0; webNum = 0;
		$('input[name="buildCheck"]:checked').each(function() {
			buildingNum++;
			chk_value.push( {
				name : 'buildCheck',
				value : $(this).val()
			});
		});
		var webCheckMap = new Map();
		$('input[name="webCheck"]:checked').each(function() {
			webNum++;
			if (webCheckMap.get($(this).val()) == null) {
				webCheckMap.put($(this).val(), $(this).attr("loginName"));
			} else {
				webCheckMap.put($(this).val(), webCheckMap.get($(this).val()) 
						+ "," + $(this).attr("loginName"));
			}
			
			chk_value.push( {
				name : 'position',
				value : $("#positionSelect" + $(this).val()).val()
			});
			chk_value.push({
			    name: 'WebOperate',
			    value: $(this).val() + "_" + $("#" + $(this).val() + "_RepeateOperate").val() + "_" + $("#" + $(this).val() + "_LimitOperate").val() + "_" + $("#" + $(this).val() + "_PlaceOperate").val()
			});
		});
		
		for (var i = 0; i < webCheckMap.size(); i++) {
			chk_value.push( {
				name : 'webCheck',
				//value: webCheckMap.arr[i].key + ":" + webCheckMap.arr[i].value
			    value : webCheckMap.arr[i].key
			});
		}
		
		chk_value.push( {
			name : 'buildingType',
			value : $("#buildingType").val()
		});
		
		chk_value.push({
		    name: 'releaseType',
		    value: 0
		});

		//var rlsNum = buildingNum * webNum;
		
		//if (rlsNum > $("#rlsRem").val()) {
		//	alert("可发送数量不足");
		//	return;
	//	}
		
		$.ajax({
		    url: $("#basePath").val() + "/House/ReleaseHouses",
			data : chk_value,
			type : "post",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				isCanSubmit = false;
				parent.loadingShow();
			},
			success: function(result) {
				//var tipMap = result.tipMap;
				var tipHtml = '';
				
				//var repeatNum = 0;
				//for (var buildingId in tipMap) {
				//	tipHtml += "房源编号 " + buildingId + ":";
				//	var webIdStr = tipMap[buildingId].split(",");
				//	for (var i = 0; i < webIdStr.length; i++) {
				//		repeatNum++;
				//		tipHtml += $("#webName_" + webIdStr[i]).val() + "  ";
				//	}
				//	tipHtml += '<br>';
				//}
				//if (tipHtml != '') {
				//	var contentHtml = "发送任务成功生成，其中以下房源在对应网站有重复房源，没有生成任务<br>" + tipHtml
				//		+ "若要发送重复房源，请到个人设置中修改设置";
				//	art.dialog({
				//		title : '提示',
				//		content : contentHtml,
				//		cancel : true
				//	});
				//}
				
				$(".house-select").html(buildingNum);
				$(".website-select").html(webNum);
				//rlsNum -= repeatNum;
				//$(".select-total").html(rlsNum);
				
				//var rlsRem = parseInt($("#rlsRem").val()) - rlsNum;
				//$("#rlsRem").val(rlsRem);
				//$("#rlsSpan").html(rlsRem);
				
				$(".selectWebsite").hide();
				$(".selectFinish").show();
				$(".sale-push").show();
				$(".sale-realsepush").hide();
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

function appointmentRelease() {
	var dateStr_1 = $("#dateSpan_1").html().split('-');
	var dateStr_2 = $("#dateSpan_2").html().split('-');
	
	var date_1 = new Date(dateStr_1[0], dateStr_1[1] - 1, dateStr_1[2],
			$("#hourSpan").html(), $("#minnuteSpan").html(), '0');
	var date_2 = new Date(dateStr_2[0], dateStr_2[1] - 1, dateStr_2[2],
			$("#hourSpan").html(), $("#minnuteSpan").html(), '0');
	
	if (isCanSubmit && checkTime(date_1, date_2) && checkWebNum()) {
		var chk_value = [];
		
		chk_value = getAppTimeList(chk_value);
		
		$('input[name="buildCheck"]:checked').each(function() {
			chk_value.push( {
				name : 'buildCheck',
				value : $(this).val()
			});
		});
		
		var webCheckMap = new Map();
		$('input[name="webCheck"]:checked').each(function() {
			if (webCheckMap.get($(this).val()) == null) {
				webCheckMap.put($(this).val(), $(this).attr("loginName"));
			} else {
				webCheckMap.put($(this).val(), webCheckMap.get($(this).val()) 
						+ "," + $(this).attr("loginName"));
			}
			
			chk_value.push( {
				name : 'position',
				value : $("#positionSelect" + $(this).val()).val()
			});
		});
		
		for (var i = 0; i < webCheckMap.size(); i++) {
			chk_value.push( {
				name : 'webCheck',
				//value: webCheckMap.arr[i].key + ":" + webCheckMap.arr[i].value
			    value : webCheckMap.arr[i].key 
			});
		}
		
//		$('input[name="webCheck"]:checked').each(function() {
//			chk_value.push( {
//				name : 'webCheck',
//				value : $(this).val()
//			});
//			chk_value.push( {
//				name : 'position',
//				value : $("#positionSelect" + $(this).val()).val()
//			});
//		});
		
		chk_value.push( {
			name : 'buildingType',
			value : $("#buildingType").val()
		});
		
		chk_value.push( {
			name : 'postType',
			value : $("#postType").val()
		});
		
		chk_value.push( {
			name : 'status',
			value : $("#status").val()
		});
		
		chk_value.push({
		    name: 'releaseType',
            value:1
		});
		$.ajax({
		    url: $("#basePath").val() + "/House/ReleaseHouses",
			data : chk_value,
			type : "post",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				isCanSubmit = false;
				parent.loadingShow();
			},
			success: function(result) {
				$(".selectWebsite").hide();
				$(".selectFinish").show();
				$(".sale-push").hide();
				$(".sale-realsepush").show();
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

// 删除房源
function deleteHouse(deleteType, buildingStatus, clear) {
	var chk_value = [];
	/*计算选择条数*/
	var sum = 0;
	var appBuildingIds = "";
	$(".saleManager-state-checkbox").each(function(){
		if($(this).attr("checked")){
			sum++;
			
			var app = $("#htr" + $(this).val()).find(".appointment_pics");
			if (app.length > 0) {
				appBuildingIds += "," + $(this).val();
			}
		}
	});
	
	if(sum == 0 && clear != 0){
		alert("请先选择房源");
		return ;
	}else{
		if(buildingStatus==0){
			if(!confirm("已选中"+sum+"条记录，是否确认还原？")){ return; }
		}else if(buildingStatus==1){
			var appTip = "";
			if (appBuildingIds != "") {
				appBuildingIds = appBuildingIds.substring(1);
				appTip = " \n 其中房源编号为 ：" + appBuildingIds
						+ "有预约任务 \n 确认删除时将同时删除预约任务";
			}
			if(!confirm(" 已选中"+sum+"条记录" + appTip + " \n 是否确认删除？")){ return; }
		}else{
			if(clear==0){
				if(!confirm("是否确认清空回收站？")){ return; }
			}else{
				if(!confirm("已选中"+sum+"条记录，是否确认彻底删除？")){ return; }
			}
		}
		$("#releaseDiv").hide();
	}
	if (isCanSubmit) {
		if (clear == 0) {
			chk_value.push( {
			    name: 'HouseID',
				value : 0
			});
		} else if (!checkBuildingNum()) {
			return;
		}
		var changeToStatus = 1;
		if (buildingStatus == 1)
		{
		    changeToStatus = 3;
		}
		else if (buildingStatus == 2)
		{
		    changeToStatus = 4;
		}
		//var houseList = [];
		$('input[name="buildCheck"]:checked').each(function() {
			chk_value.push( {
			    name: 'HouseID',
				value : $(this).val()
			});
		    //houseList.push({ HouseID: parseInt($(this).val()) });
		});
		
		chk_value.push( {
			name : 'deleteType',
			value : deleteType
		});
		
		chk_value.push( {
			name : 'changeToStatus',
			value: changeToStatus
		});
		
		chk_value.push( {
			name : 'clear',
			value : clear
		});
		
		chk_value.push( {
			name : 'appBuildingIds',
			value : appBuildingIds
		});
		
		$.ajax({
			url: $("#basePath").val() + "/House/DeleteHouses",
			data: chk_value,
			type : "post",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				parent.loadingShow();
				isCanSubmit = false;
			},
			success: function(result) {

			    location.reload();
				//var kcRem = parseInt($("#kcRem").val()) + parseInt(result.usedNum);
				//$("#kcRem").val(kcRem);
				//$("#kcSpan").html(kcRem);
				
				//var status0 = 0;
				//var status1 = parseInt($("#statusSpan1").html());
				//var status2 = 0;
				//if(buildingStatus == 0) {
				//	status0 = parseInt($("#statusSpan").html()) + sum;
				//	status2 = parseInt($("#statusSpan2").html()) - sum;
				//} else if(buildingStatus == 1) {
				//	status0 = parseInt($("#statusSpan").html()) - sum;
				//	status2 = parseInt($("#statusSpan2").html()) + sum;
				//} else{
				//	if (clear == 0) {
				//		sum = result.usedNum;
				//	}
				//	status0 = $("#statusSpan").html();
				//	status2 = parseInt($("#statusSpan2").html()) - sum;
					
				//	var typeSpanId;
				//	switch(parseInt($("#buildingType").val())) {
				//		case 1: typeSpanId = "typeSpan";break;
				//		case 2: typeSpanId = "typeSpan2";break;
				//		case 3: typeSpanId = "typeSpan3";break;
				//		case 4: typeSpanId = "typeSpan4";break;
				//		case 5: typeSpanId = "typeSpan5";break;
				//	}
				//	$("#" + typeSpanId).html(parseInt($("#" + typeSpanId).html()) - sum);
				//}
				//$("#statusSpan").html(status0);
				//$("#statusSpan2").html(status2);
				
				//getHousebyCondition();
				//changeValue(status0, status1, status2);

			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
//				if ($("#houseTable").children().html() == undefined) {
//					var emptyHtml = '<tr><td colspan="6"><div class="null-tip saleManager-state-content con2">'
//						+ '<img width="70" height="70" src="' + $("#basePath").val() + '/release/images/icon_tip.png"></img>'
//						+ '<p class="">没有满足条件的房源</p><p><a href="' + $("#basePath").val() + '/import/getimpview.do?'
//						+ 'postType=' +  $("#postType").val() + '&buildingType=' + $("#buildingType").val() + '"'
//						+ 'class="ui-btn"><span class="ui-btn-green"><em>立即加入</em></span></a></p></div></td></tr>';
//					$("#houseTable").html(emptyHtml);
//				}
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

function updateBuildingStatus(deleteType, buildingStatus, id) {
	if (isCanSubmit) {
		$.ajax({
			url: $("#basePath").val() + "/ajax/houman/deletehouses.do",
			data : {
				buildCheck : id,
				buildingStatus : buildingStatus,
				deleteType : deleteType
			},
			type : "post",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				parent.loadingShow();
				isCanSubmit = false;
			},
			success: function(result) {
				var kcRem = parseInt($("#kcRem").val()) + parseInt(result.usedNum);
				$("#kcRem").val(kcRem);
				$("#kcSpan").html(kcRem);
				sum = 1;
				var status0 = 0;
				var status1 = parseInt($("#statusSpan1").html());
				var status2 = 0;
				if(buildingStatus == 0) {
					status0 = parseInt($("#statusSpan").html()) + sum;
					status2 = parseInt($("#statusSpan2").html()) - sum;
				} else{
					status0 = $("#statusSpan").html();
					status2 = parseInt($("#statusSpan2").html()) - sum;
					
					var typeSpanId = null;
					switch(parseInt($("#buildingType").val())) {
						case 1: typeSpanId = "typeSpan";break;
						case 2: typeSpanId = "typeSpan2";break;
						case 3: typeSpanId = "typeSpan3";break;
						case 4: typeSpanId = "typeSpan4";break;
						case 5: typeSpanId = "typeSpan5";break;
					}
					$("#" + typeSpanId).html(parseInt($("#" + typeSpanId).html()) - sum);
				}
				$("#statusSpan").html(status0);
				$("#statusSpan2").html(status2);
				changeValue(status0, status1, status2);
				
				getHousebyCondition();
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

function deleteDraft(id) {
	if (isCanSubmit) {
		var chk_value = [];
		var num = 0;
		if (id == undefined) {
			$('input[name="buildCheck"]:checked').each(function() {
				chk_value.push( {
					name : 'buildCheck',
					value : $(this).val()
				});
				num++;
			});
			
			if(!confirm("已选中" + num + "条记录，是否确认彻底删除？")){
				return; 
			}
		} else {
			chk_value.push( {
				name : 'buildCheck',
				value : id
			});
			num++;
		}
		
		$.ajax({
			url: $("#basePath").val() + "/ajax/houman/deleteDraft.do",
			data : chk_value,
			type : "post",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				parent.loadingShow();
				isCanSubmit = false;
			},
			success: function(result) {
				var kcRem = parseInt($("#kcRem").val()) + num;
				$("#kcRem").val(kcRem);
				$("#kcSpan").html(kcRem);
				
				var status0 = parseInt($("#statusSpan").html());
				var status1 = parseInt($("#statusSpan1").html()) - num;
				var status2 = parseInt($("#statusSpan2").html());
				
				var typeSpanId;
				switch(parseInt($("#buildingType").val())) {
					case 1: typeSpanId = "typeSpan";break;
					case 2: typeSpanId = "typeSpan2";break;
					case 3: typeSpanId = "typeSpan3";break;
					case 4: typeSpanId = "typeSpan4";break;
					case 5: typeSpanId = "typeSpan5";break;
				}
				
				$("#" + typeSpanId).html(parseInt($("#" + typeSpanId).html()) - num);
				$("#statusSpan1").html(status1);
				changeValue(status0, status1, status2);
				
				getHousebyCondition();
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

function changeValue(status0, status1, status2) {
	var buildingType = $("#buildingType").val();
	if (buildingType == 1) {
		value0 = status0;
		value1 = status2;
		value10 = status1;
	} else if (buildingType == 2) {
		value2 = status0;
		value3 = status2;
		value11 = status1;
	} else if (buildingType == 3) {
		value4 = status0;
		value5 = status2;
		value12 = status1;
	} else if (buildingType == 4) {
		value6 = status0;
		value7 = status2;
		value13 = status1;
	} else if (buildingType == 5) {
		value8 = status0;
		value9 = status2;
		value14 = status1;
	}
}

// 验证是否选择房源
function checkBuildingNum() {
	var flag = false;
	$('input[name="buildCheck"]:checked').each(function() {
		flag = true;
	});
	if (flag) {
		return true;
	} else {
		alert("请先选择房源");
		return false;
	}
}

//验证是否选择网站
function checkWebNum() {
	var flag = false;
	$('input[name="webCheck"]:checked').each(function() {
		flag = true;
	});
	if (flag) {
		return true;
	} else {
		alert("请先选择发送的网站");
		return false;
	}
}

//验证预约时间
function checkTime(date_1, date_2) {
	var date = new Date();
	
	if (date_1.getTime() < date.getTime()) {
		alert("预约起始时间必须大于现在时间");
		return false;
	}
	if (date_1.getTime() > date_2.getTime()) {
		alert("预约起始时间必须小于预约结束时间");
		return false;
	}
	return true;
}

function dateToString(date) {
	var year = date.getFullYear();
	var month = date.getMonth() + 1;
	var day = date.getDate();
	var hour = date.getHours();
	var minute = date.getMinutes();
	var second = "00";
	
	month = month < 10 ? "0" + month : month;
	day = day < 10 ? "0" + day : day;
	hour = hour < 10 ? "0" + hour : hour;
	minute = minute < 10 ? "0" + minute : minute;
	
	return year + "-" + month + "-" + day + " " + hour + ":" + minute + ":" + second;
}

/*查询区*/
function selectOption(on){
	var obj = $("."+on).siblings("li").find(".manage-list-content");
	$("body").find(".manage-list-content").not(obj).hide();
	if(obj.css("display") == "block"){
        obj.hide();
    }else{
    	obj.show();
    }
}

//收缩/隐藏下拉框
$(".input_search img").die().live("click", function(){
	$(this).parent().next().find("ul").toggle();
});
//搜索下拉框失去焦点
$(".input_search input").live("blur", function(){
	if ($(this).val() == '') {
		$(this).val($(this).attr("inHtml"));
	}
});
//搜索下拉框获取焦点
$(".input_search input").live("focus", function(){
	if ($(this).val() == $(this).attr("inHtml")) {
		$(this).val("");
	}
	$(this).parent().next().find("ul").show();
});
//搜索下拉框输入进行搜索
$(".input_search input").live("keyup", function(){
	var searchVal = $(this).val();
	var objs = $(this).parent().next().find("ul").find("li");
	
	$.each(objs, function(i, obj){
		if ($(obj).attr("liValue").indexOf(searchVal) == -1) {
			$(obj).hide();
		} else {
			$(obj).show();
		}
	});
});
/**
 * 房源管理查看发布记录
 */
function getSuccessTip(id){
	 if (isCanSubmit) {
		$.ajax({
		    url: '/PostLog/GetPostLogList',
			data :{
			    "title": id,
			    "pageIndex": 1,
			    "pageSize": 30,
			    "status":1
			} ,
			type : "get",
			cache : false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				isCanSubmit = false;
				parent.loadingShow();
			},
			success: function (result) {

			    var resultHtml = BuildPostLogInHouseList(result.data);

				art.dialog({
					title: '发布记录',
					content: resultHtml,
					width: 500
					/*height: 400*/
				});
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, data) {
				parent.loadingHide();
				isCanSubmit = true;
			}
		});
	}
}

/**
 * 房源管理查看发布记录
 */
function BuildPostLogInHouseList(data)
{
    var realPostTypeMap = ["先删后发", "发布", "先删后发", "不发"];
    var html = "";
    if (data != null && data.length > 0) {
        html += '<div id="successTip" class="successTip">' +
                '		<div class="successTip-table">' +
                '			<table class="h_list" width="100%" cellspacing="0" cellpadding="0" style="margin:0">' +
                '				<thead><tr><td style="display:none;"><input type="checkbox" id="successTip-all" onclick="selectAllTip()"></td><td>发布网站</td><td>操作类型</td><td>发布状态</td><td>发布时间</td><td>本地操作</td></tr></thead>' +
                '				<tbody>';
        $.each(data, function (a, h) {
            var operateStr = h.Status==1?'<a style="display:none;" href="javascript:deleteTaskalone(' + h.ID + ')" class="successTip-btn">删除</a><a target="_blank" title="" href="' + h.TargetUrl + '" class="successTip-btn">浏览</a>':'';
            html += '						<tr id="htr' + h.ID + '">' +
                    '							<td width="60"  style="display:none;">' +
                    '										<input type="checkbox" name="successCheck" value="' + h.ID + '" infoid="' + h.TargetID + '" webid="' + h.SiteID + '" onclick="changeTr()">' +
                    '							</td>' +
                    '							<td>' + h.SiteName + '</td>' +
                    '							<td>' +
                    '									' + realPostTypeMap[h.PostType]+
                    '							</td>' +
                    '							<td title=' + h.Msg + '>' + h.Msg.substring(0, 20) + '</td>' +
                    '							<td>' + h.DateTime + '</td>' +
                    '<td>' +
                      operateStr +
                    '</td>'  
                    '						</tr>';
        });
        html += '				</tbody>' +
                '			</table>' +
                '		</div>' +
                '		<!-- 表格下面 -->' +
                '		<div class="saleManager-bottom" style="display:none;">' +
                '			<div class="float-l">' +
                '				<input type="button" class="btn_g" value="删除" id="successTip-del" onclick="checkSuccessNum()">' +
                '			</div>' +
                '		</div>' +
                '</div>';
    }
    else {
        html += '<div id="successTip" class="successTip">' +
                '	' +
                '		' +
                '			<span class="houseTitle-font">亲，此房源暂没有发布到网站，赶快发布一套吧^_^</span>' +
                '		' +
                '		' +
                '	' +
                '</div>';
    }
    return html;
}


/*全选*/
function selectAllTip(){
	var per = $("#successTip-all").attr("checked");
	var objs = $(".successTip-table").find("input[name=successCheck]");
	if(per){
		objs.each(function(){
			$(this).attr("checked",true);
		});
	}else{
		objs.each(function(){
			$(this).attr("checked",false);
		});
	}
}

$("#successTip-all").click(function(){
	var per = $(this).attr("checked");
	var objs = $(".successTip-table").find("input[name=successCheck]");
	if(per){
		objs.each(function(){
			$(this).attr("checked",true);
		});
	}else{
		objs.each(function(){
			$(this).attr("checked",false);
		});
	}
});

function changeTr(){
	var obj = $(".successTip-table").find("input[name=successCheck]");
	if(obj.length > 0 ){
		obj.each(function(){
			if(!($(this).attr("checked"))){
				$("#successTip-all").attr("checked",false);
			}
		});
	}else{
		$("#successTip-all").attr("checked",false);
	}
}

//验证是否选择房源
function checkSuccessNum() {
	var i = 0;
	$('input[name="successCheck"]:checked').each(function() {
		i++;
	});
	if (i > 0) {
		alertDelete(i);
	} else {
		art.dialog({
		    content : '请先选择需要删除的房源记录',
		    ok : true
		});
	}
}

function alertDelete(i) {
	art.dialog.confirm_3(
		"已选中" + i + "条记录<br>是：同步删除对应网站的发布记录<br>否：只删除本站记录",
		function(){
			deleteTask(1);
		},
		function(){
			deleteTask(0);
		},
		function(){
		}
	);
}

//批量删除房源
function deleteTask(deleateType) {
	if (isCanSubmit) {
		var chk_value = [];
		chk_value.push( {
			name : 'deleateType',
			value : deleateType
		});
		var infoTip = '';
		$('input[name="successCheck"]:checked').each(function(i) {
			chk_value.push( {
				name : 'taskId',
				value : $(this).val()
			});
			if($(this).attr("infoId") == null || $(this).attr("infoId") == '') {
				infoTip += (i + 1) + " ";
			}
		});
		
		if (infoTip != '') {
			art.dialog({
				title : "提示",
				content : "选中的第 " + infoTip + " 条房源因无法取得对应网站数据，无法进行全网删除。",
				ok : true
			});
		}
		
		$.ajax({
			type: "post",
			url: $("#basePath").val() + "/ajax/rellog/deleteTask.do",
			data : chk_value,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				isCanSubmit = false;
				parent.loadingShow();
			},
			success: function(result) {
				var tipListStr = "," + result.tipList + ",";
				var tipFlag = false;
				$('input[name="successCheck"]:checked').each(function(i) {
					if (tipListStr == null || tipListStr.indexOf("," + i + ",") == -1) {
						$("#htr" + $(this).val()).remove();
					} else {
						tipFlag = true;
					}
				});
				if (tipFlag) {
					art.dialog({
						title : "提示",
						content : "剩余选中房源因未绑定网站账号，无法进行全网删除。",
						ok : true
					});
				}
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

function deleteTaskalone(taskId){
	art.dialog.confirm_3(
			"是否要删除该条发布记录,<br>是：同步删除对应网站的发布记录,<br>否：只删除本站记录",
			function(){
				deleteTaskOne(1,taskId);
			},
			function(){
				deleteTaskOne(0,taskId);
			},
			function(){
			}
		);
}

//单条删除房源
function deleteTaskOne(deleateType,taskId) {
	if (isCanSubmit) {
		$.ajax({
			type: "post",
			url: $("#basePath").val() + "/ajax/rellog/deleteTask.do",
			data : {
				"taskId" : taskId,
				"deleateType" : deleateType
			},
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
				isCanSubmit = false;
				parent.loadingShow();
			},
			success: function(result) {
				$("#htr" + taskId).remove();
			},
			error: function(jqXHR) {
				alert($.parseJSON(jqXHR.responseText).msg);
			},
			complete:function(XMLHttpRequest, textStatus) {
				isCanSubmit = true;
				parent.loadingHide();
			}
		});
	}
}

/*提示框关闭*/
function closeReleaseDiv(){
	$("#releaseDiv").hide();
}

/**批量设置标签**/
function batchUpdateTags(){
	closeReleaseDiv();
	var sum = 0;
	var params = [];
	$("input[name=buildCheck]:checked").each(function(){
		sum++;
		params.push({
			name: "HouseIds",
			value: $(this).val()
		});
	});
	
	if(sum == 0){
		alert("请先选择房源");
		return ;
	}
	art.dialog({
	    content: document.getElementById("updateTagsDiv"),
	    init: function () {
	        $("#updateTags").val("");
	        $.each($("input[name=SetingTag]"), function (i, a) {
	            $(this).attr("checked", false);
	        });
	    },
		title: "设置标签",
		width: 140,
		lock: true,
		ok: function() {
			var tagStr = $("#updateTags").val();
			params.push({
				name: "tag",
				value: tagStr
			});
			$.ajax({
				url: $("#basePath").val() + "/House/batchUpdateHouseTags",
				type: "post",
				data: params,
				success: function() {
					$("input[name=buildCheck]:checked").each(function(){
						var buildingId = $(this).val();
						var html = "";
						if (tagStr.length > 0) {
							var tagArr = tagStr.split(",");
							$.each(tagArr, function (i, tag) {
							    if (tagMap.get(tag) != null) {
							        html += '<span class="tags_pics"><img src="' + tagMap.get(tag) + '"></img></span>';
							    }
							});
						}
						$("#tags_pic_" + buildingId).html(html);
						$("a[name=link_tag][buildingid="+buildingId+"]").attr("tags", tagStr);
					});
				}
			});
		},
		cancel: true
	});
}

/** 查看预约详情 */
var everyDay_appNum;
var total_appNum;
var app_webNum;
var app_buildNum;
function appDetail() {
	var chk_value = [];
	var list = getAppTimeList(chk_value);
	
	var html = '<ul style="font-size:14px;">';
	html += '<li><span>选择房源数：' + app_buildNum + '</span></li>';
	html += '<li><span>选择账号数：' + app_webNum + '</span></li>';
	html += '<li><span>每日预约推送：' + everyDay_appNum + ' 次</span></li>';
	html += '<li><span>共预约推送：' + total_appNum + ' 次</span></li>';
	
	
//	var html = '<ul style="font-size:14px;"><li><span style="padding:10px;">每日预约推送 ' + everyDay_appNum + '次,共预约推送 ' + total_appNum + '次</span></li>';
//	
//	for (var i = 0; i < list.length; i) {
//		html += '<li>';
//		for (var j = 0; j < 4; j++) {
//			if (i < list.length) {
//				html += '<span style="padding:10px;">' + list[i++].value + '</span>';
//			}
//		}
//		html += '</li>';
//	}
//	html += '</ul>';
	
	art.dialog({
		title : "预约详情",
		content : html,
		ok : true
	});
}

function getAppTimeList(chk_value) {
	var dateStr_1 = $("#dateSpan_1").html().split('-');
	var dateStr_2 = $("#dateSpan_2").html().split('-');
	
	var appNum = parseInt($("#appNumSpan").html());
	var appMin = parseInt($("#appMinSpan").html());
	
	var date_1 = new Date(dateStr_1[0], dateStr_1[1] - 1, dateStr_1[2],
			$("#hourSpan").html(), $("#minnuteSpan").html(), '0');
	var date_2 = new Date(dateStr_2[0], dateStr_2[1] - 1, dateStr_2[2],
			$("#hourSpan").html(), $("#minnuteSpan").html(), '0');
	
	var dayNum = 0;
	while(date_1.getTime() <= date_2.getTime()) {
		var nextDate = new Date(date_1.getFullYear(), date_1.getMonth(), date_1.getDate() + 1, 0, 0, 0);
		everyDay_appNum = 0;
		dayNum++;
		for (var i = 0; i < appNum; i++) {
			var date = new Date(date_1.getTime() + 1000 * 60 * appMin * i);
			
			if (date.getTime() < nextDate.getTime()) {
				chk_value.push( {
					name : 'time',
					value: date.getTime()
				});
				everyDay_appNum++;
			}
		}
		date_1.setDate(date_1.getDate() + 1);
	}
	
	app_webNum = 0;
	$('input[name="webCheck"]:checked').each(function() {
		app_webNum++;
	});
	app_buildNum = 0;
	$('input[name="buildCheck"]:checked').each(function() {
		app_buildNum++;
	});
	
	everyDay_appNum = everyDay_appNum * app_webNum * app_buildNum;
	total_appNum = everyDay_appNum * dayNum;
	
	return chk_value;
}

/**生成随机数**/
function roundHour(){
	var hourTime = parseInt(Math.random()*3)+9;
	return hourTime;
}

/**生成随机数**/
function roundMinute(){
	var minuteTime = parseInt(Math.random()*12)*5;
	return minuteTime;
}
