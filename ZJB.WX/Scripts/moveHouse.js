$(function () {
    search();
	/*获取窗口高度*/
	var height = document.documentElement.clientHeight;
	$(".saleHouse").css("min-height",height-2);
	$(".saleHouse").css("_height",height-2);
	
	/*鼠标离开时隐藏*/
	$(".manage-list-content").mouseleave(function() {
		$(".manage-list-content").hide();
	});
	
	/*查询区*/
	$(".moveHouse-website-content li").die().live("click", function() {
		var content = $(this).html();
		$("#moveHouse-website-content").val($(this).attr("val"));
		$(".moveHouse-website span").html(content);
		$(".manage-list-content").hide();
		
		var areaLi = $(".moveHouse-area-content li").eq(0);
		$("#moveHouse-area-content").val(areaLi.attr("val"));
		$(".moveHouse-area span").html(areaLi.html());
		
		var buildingTypeLi = $(".moveHouse-houseType-content li").eq(0);
		$("#moveHouse-houseType-content").val(buildingTypeLi.attr("val"));
		$(".moveHouse-houseType span").html(buildingTypeLi.html());
		
		var postTypeLi = $(".moveHouse-saleType-content li").eq(0);
		$("#moveHouse-saleType-content").val(postTypeLi.attr("val"));
		$(".moveHouse-saleType span").html(postTypeLi.html());
		
		var statusLi = $(".moveHouse-status-content li").eq(0);
		$("#moveHouse-status-content").val(statusLi.attr("val"));
		$(".moveHouse-status span").html(statusLi.html());
		
		search();
	});
	
	$(".moveHouse-area-content li").die().live("click", function() {
		var content = $(this).html();
		$("#moveHouse-area-content").val($(this).attr("val"));
		$(".moveHouse-area span").html(content);
		$(".manage-list-content").hide();
		search();
	});
	
	$(".moveHouse-houseType-content li").die().live("click", function() {
		var content = $(this).html();
		$("#moveHouse-houseType-content").val($(this).attr("val"));
		$(".moveHouse-houseType span").html(content);
		$(".manage-list-content").hide();
		search();
	});
	
	$(".moveHouse-saleType-content li").die().live("click", function() {
		var content = $(this).html();
		$("#moveHouse-saleType-content").val($(this).attr("val"));
		$(".moveHouse-saleType span").html(content);
		$(".manage-list-content").hide();
		search();
	});
	
	$(".moveHouse-status-content li").die().live("click", function() {
		var content = $(this).html();
		$("#moveHouse-status-content").val($(this).attr("val"));
		$(".moveHouse-status span").html(content);
		$(".manage-list-content").hide();
		search();
	});
	
	/*全选*/
	$("#movehouse-chekboxAll").die().live("click", function() {
		var selectAll = $("#movehouse-chekboxAll").attr("checked");
		var obj = $("#moveHouseTable tr").find("input.yes");
		if(selectAll){
			$("#moveHouse-selected").html(obj.length);
			obj.each(function(){
				$(this).attr("checked",true);
			});
		}else{
			$("#moveHouse-selected").html(0);
			obj.each(function(){
				$(this).attr("checked",false);
			});
		}
	});
	
	$("#syncBtn").die().live("click", function() {
	    var buildingType = $("#moveHouse-houseType-content").val();
	    var postType = $("#moveHouse-saleType-content").val();
		$.ajax({
			url: "/house/sync",
			type: "get",
			data: {
			    webId: $("#moveHouse-website-content").val(),
			    tradeType: postType,
			    buildingType: buildingType
			},
			beforeSend: function(XMLHttpRequest){
			    $(".moveHouse-content").html(" <div class=\"houseTitle-font\">同步列表中...</div>");
				pageForbidden();
			}, 
			success: function(data) {
			    if (data == "") {
			        search();
					//setTimeout("search()", 3000);
				} else {
				    alert(unescape(data.replace(/\\u/g, '%u')));
				    var styleShibai = "<span class=\"shibai\"></span>";
				    $(".moveHouse-content").html(styleShibai);
					pageAllowed();
				}
			}, 
			error: function (XMLHttpRequest, textStatus, errorThrown) {
			    search();
				//setTimeout("search()", 3000);
//			},
//			complete:function(XMLHttpRequest, textStatus){                  
//				parent.loadingHide();
			}     
		});
	});
	
	$("#impBtn").die().live("click", function() {
		if ($("input[name=webBuildingId]:checked").length == 0) {
			alert("请至少选择一条房源！");
			return;
		}
		
		var params = [];
		$("input[name=webBuildingId]:checked").each(function() {
			params.push( {
			    name: "hids",
				value : $(this).val()
			});
		});
		params.push({
			name: "webId",
			value: $("#moveHouse-website-content").val()
		});
		
		$.ajax({
		    url: "/House/ImportHouseBatch",
			type: "post",
			data: params,
			beforeSend: function (XMLHttpRequest) {
			    $(".moveHouse-content").html(" <div class=\"houseTitle-font\">导入中...</div>");
//				parent.loadingShow();
				pageForbidden();
			}, 
			success: function(data) {
			    $(".moveHouse-content").html(" <div class=\"houseTitle-font\">成功导入" + data.count + "条房源</div>");
			    search();
			    //setTimeout("search()", 3000);
			}, 
			error: function (XMLHttpRequest, textStatus, errorThrown) {
			    search();
				//setTimeout("search()", 3000);
//			},
//			complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
//				parent.loadingHide();
			}    
		});
	});
	
	$("a[name=change_link]").die().live({
		mouseenter:function() {
			var id = $(this).attr("id");
			var tds = $(this).parent().parent().find("td");
			var webBuildingId = tds.eq(0).find("input").val();
			$.ajax({
				url: "ajax/showChange.html",
				type: "get",
				data: {
					webBuildingId: webBuildingId
				},
				dataType: "json", 
				beforeSend: function(XMLHttpRequest){
					parent.loadingShow();
				}, 
				success: function(data) {
					var content;
					if (data == null) {
						content = "房源不存在了！";
					} else {
						var priceUnit = (("出售" == tds.eq(1).html()) ? "万元" : "元/月");
						content = data.cell + "，" + data.price + priceUnit + "，" + data.room + "室" + 
						data.hall + "厅" + data.toilet + "卫，" + data.houseArea + "平";
					}
					art.dialog({
						id: "dlg_show_change", 
						follow: document.getElementById(id),
						lock: false,
						title: "原房源基本信息", 
					    content: content
					});
				},
				complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
					parent.loadingHide();
				}    
			});						  
		},
		mouseleave:function() {
			art.dialog({id: 'dlg_show_change'}).close();
		}
	});
	
	
	/*$("#moveHouseTable tr").die().live("click",function(e){
			var obj = $(this).find("input.yes");
			var check = $(this).find("input.yes").attr("checked");
			if(e.target == obj[0]){
				if(check){
					$(this).find("input.yes").attr("checked",true);
				}else{
					$(this).find("input.yes").attr("checked",false);
				}
			}else{
				if(check){
					$(this).find("input.yes").attr("checked",false);
				}else{
					$(this).find("input.yes").attr("checked",true);
				}
			}
			$("#moveHouse-selected").html($("input[name=webBuildingId]:checked").length);
		});*/
});

function search(pageIndex) {
    var pi = pageIndex == undefined ? 1 : pageIndex;
	var webId = $("#moveHouse-website-content").val();
	var buildingType = $("#moveHouse-houseType-content").val();
	var postType = $("#moveHouse-saleType-content").val();
	var status = $("#moveHouse-status-content").val();
	var cell = $("#moveHouse-area-content").val();
	$.ajax({
	    url: "/house/SnatchWeb",
		type: "get",
		data: {
			SiteId: webId,
			BuildingType: buildingType,
			TradeType: postType,
			status: status,
			CommunityId: cell,
			PageIndex: pi
		},
		beforeSend: function (XMLHttpRequest) {
		    $(".moveHouse-content").html(" <div class=\"houseTitle-font\">列表获取中...</div>");
		},
		success: function (data) {
		    if (data == "登陆失败") {
		        var styleShibai = "<span class=\"shibai\"></span>";
		        $(".moveHouse-content").html(styleShibai);
		    }
		    else {
		        $(".moveHouse-content").html(data);
		    }
		    pageAllowed();
//			if (parseInt($("#moveHouse-doing").html()) > 0) {
//				setTimeout("search()", 3000);
//			}
		}
	});
}

var forbidden = false;
function pageForbidden() {
	$("#syncBtn").attr("disabled", true);
	$("#impBtn").attr("disabled", true);
	$(".moveHouseSelect-right .btn_g").addClass("btn_disable");
	$(".moveHouseSelect-right .btn_g").removeClass("btn_g");
	$(".manage-tip").css("border","1px solid #e8e8e8");
	forbidden = true;
	
	parent.loadingShow();
}

function pageAllowed() {
	$("#syncBtn").attr("disabled", false);
	$("#impBtn").attr("disabled", false);
	$(".moveHouseSelect-right .btn_disable").addClass("btn_g");
	$(".moveHouseSelect-right .btn_disable").removeClass("btn_disable");
	$(".manage-tip").css("border","1px solid #64ac58");
	forbidden = false;
	
	parent.loadingHide();
}

/*查询区*/
function selectOption(on) {
	if (forbidden) return;
	var obj = $("."+on).siblings("li").find(".manage-list-content");
	$("body").find(".manage-list-content").not(obj).hide();
	if(obj.css("display") == "block"){
        obj.hide();
    }else{
    	obj.show();
    }
}

/*去掉全选*/
function changeTr(){
	var obj = $("#moveHouseTable").find("input.yes");
	if(obj.length > 0 ){
		obj.each(function(){
			if(!($(this).attr("checked"))){
				$("#movehouse-chekboxAll").attr("checked",false);
			}
			$("#moveHouse-selected").html($("input[name=webBuildingId]:checked").length);
		});
	}else{
		$("#movehouse-chekboxAll").attr("checked",false);
	}
}

