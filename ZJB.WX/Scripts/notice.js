

$(function(){
	$("input[name=userType]").click(function(){
		var obj = $("#user").attr("checked");
		if(obj){
			$("#userText").show();
		}else{
			$("#userText").hide();
		}
	});
	
	/*根据城市获取公司*/
	$("select[name=city]").change(function(){
		var city = $("select[name=city]").val();
		var html = '<option selected="selected" value="company">所有公司</option>';
		if(city != "city"){
			$.ajax({
				url: "../getCompanyByCity.do",
				type: "post",
				data : {
					"city" : city,
				},
				success: function(data) {
					var storeList = data;
					if(storeList.length > 0){
						for(var i = 0; i < storeList.length; i++){
							html += '<option value="'+storeList[i].orgId+'">'+storeList[i].orgName+'</option>';
						}
					}
					$("select[name=company]").html(html);
				}
			});
		}else{
			$("select[name=company]").html(html);
		}
	});
});

/*保存公告*/
var noticeFlag = true;
function saveNoticeView() {
	if(noticeFlag){
		var scopeType = "";
		var scopeId = "";
		var noticeTitle = $("input[name=noticeTitle]").val();
		var noticeType = $("select[name=noticeType]").val();
		var userType = $("input[name=userType]:checked").val();
		var company = $("select[name=company]").val();
		var city = $("select[name=city]").val();
		var noticeContent = CKEDITOR.instances.noticeContent.getData();
		
		/**去html与空格**/
		var noticeContentHtml = dStripHtml(noticeContent);
		noticeContentHtml = noticeContentHtml.replace(/&nbsp;/ig,"");
		noticeContentHtml = trimAll(noticeContentHtml,"g");
		if(noticeTitle.length < 1){
			alert("公告标题不能为空");
			return;
		}
		if(noticeTitle.length > 128){
			alert("公告标题超过限制,最多只能输入128个字符");
			return;
		}
		if(noticeContentHtml.length < 1){
			alert("公告内容不能为空");
			return;
		}
		if(userType == "user"){
			scopeType = "user";
			scopeId = $("#userText").val();
			scopeId = dStripHtml(scopeId);
			scopeId = scopeId.replace(/&nbsp;/ig,"");
			scopeId = trimAll(scopeId,"g");
			if(scopeId.length < 1){
				alert("指定用户不能为空");
				return;
			}
		}else{
			if(company == "company"){
				if(city == "city"){
					scopeType = "all";
					scopeId = "0";
				}else{
					scopeType = "city";
					scopeId =  $("select[name=city]").val();
				}
			}else{
				scopeType = "company";
				scopeId =  $("select[name=company]").val();
			}
		}
		$.ajax({
			url: "../notice/saveNotice.do",
			type: "post",
			data : {
				"noticeTitle" : noticeTitle,
				"noticeContent" : noticeContent,
				"noticeType" : noticeType,
				"scopeType" : scopeType,
				"scopeId" : scopeId
			},
			beforeSend: function(XMLHttpRequest) {
				noticeFlag = false;
			},
			success: function(data) {
				if(data==0){
					alert("保存成功");
				}else if(data==1){
					alert("保存失败");
				}else{
					alert("用户名"+data+"不对,保存失败,请重新输入");
				}
			},
			complete:function(XMLHttpRequest, textStatus) {
				noticeFlag = true;
			}
		});
	}
}




function getNotice(pageIndex) {
	$.ajax({
		url: "getNoticeListDiv.html",
		type: "get",
		data: {
			"noticeType" : $("#noticeType").val(),
			"pageIndex" : pageIndex
		},
		dataType: "text",
		beforeSend: function(XMLHttpRequest) {
			parent.loadingShow();
		},
		success: function(result) {
			$(".noticeList-main").html(result);
		},
		error: function(jqXHR) {
//				alert($.parseJSON(jqXHR.responseText).msg);
		},
		complete:function(XMLHttpRequest, textStatus) {
			parent.loadingHide();
		}
	});
}

function dStripHtml(str) {
	str = str.replace(/<\/?[^>]*>/g,''); //去除HTML tag
	str.value = str.replace(/[ | ]*\n/g,'\n'); //去除行尾空白
	return str;
}

function trimAll(str,is_global) {
    var result;
    result = str.replace(/(^\s*)|(\s*$)/g,"");
    if(is_global.toLowerCase()=="g") {
        result = result.replace(/\s/g,"");
    }
    return result;
}