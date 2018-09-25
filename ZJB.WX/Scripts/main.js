$(function(){
	/*高度与宽度自适应*/
	var height = document.documentElement.clientHeight;
	//var weight = window.screen.width; 
	var weight = document.documentElement.clientWidth;
	var ua = navigator.userAgent.toLowerCase();
	if(ua.indexOf('iphone') > 0 || ua.indexOf('ipad') > 0){
		$(".container_box").css("height",4000);
		$(".col_side").css("height",3998);
	}else{
		$(".main-html").css("overflow","hidden");
		$(".container_box").css("height",height-84);
		$(".col_side").css("height",height-86);
	}
	if(weight > 1024){
		$(".col_side").css("margin-left",((weight-1200)/2));
		$(".renewalTip").css("left",((weight-1200)/2));
	}
	
	/**切换菜单**/
	$(".menu_title").click(function(){
		if($(this).parent(".menu").hasClass("menu_open")){
			$(this).parent(".menu").removeClass("menu_open");
		}else{
			$(this).parent(".menu").addClass("menu_open");
			$(this).parent(".menu").siblings().removeClass("menu_open");
		}
	});
	
	$(".menu_item a").click(function(){
		$(this).addClass("selected");
		$(this).siblings("a").removeClass("selected");
		$(this).parents(".menu").siblings(".menu").find("a").removeClass("selected");
	});
	
	/**调用公告定时器方法**/
	startTimerTip();
	
    //window.onbeforeunload = function() {return ("确认关闭吗？");}
});


/**加载iframe内容**/
function hrefLink(url,name){
	var isCancel = false;
	isCancel = checkSave();
	
	if(!isCancel){
		loadingShow();
		if(name == '出售录入'){
			$("#postTypeIndex").val("1");
		} else if(name == '出租录入'){
			$("#postTypeIndex").val("2");
		} else {
			$("#postTypeIndex").val("0");
		}
		if (url.indexOf("?") > 0) url = url + "&tv=1"; else url = url + "?tv=1";
		$("#main-iframe").attr("src", url);
		//$("#ceshiid").val((document.getElementById("main-iframe").contentWindow.document).getElementById("saleHouse").offsetHeight);
		//$("#main-iframe").attr("height",(document.getElementById("main-iframe").contentWindow.document).getElementById("saleHouse").offsetHeight);
		//$(".container_box").css("height",(document.getElementById("main-iframe").contentWindow.document).getElementById("saleHouse").offsetHeight);
		//$(".col_side").css("height",(document.getElementById("main-iframe").contentWindow.document).getElementById("saleHouse").offsetHeight);
	}else{
		postType();
	}
}

/**数据考核跳转**/
function hrefBlank(url){
	var isCancel = false;
	isCancel = checkSave();
	
	if(!isCancel){
		if(name == '出售录入'){
			$("#postTypeIndex").val("1");
		} else if(name == '出租录入'){
			$("#postTypeIndex").val("2");
		} else {
			$("#postTypeIndex").val("0");
		}
		window.open(url);
	}else{
		postType();
	}
}

/*录入页面提示末保存*/
function checkSave(){
	var postTypeIndex = $("#postTypeIndex").val();
	if(postTypeIndex == "1"|| postTypeIndex == "2"){
		 var iframe = document.getElementsByTagName('iframe')[0].contentWindow;
		 var isChange =iframe.document.getElementById('isChange');
		 var isModify = $(isChange).val();
		if(isModify == 1){
			var isCancel = confirm("录入信息还没有保存，确认要放弃保存吗？");
			if(isCancel){
				return false;
			}else{
				return true;
			}
		}else{
			return false;
		}
	}else{
		return false;
	}
};

/**判断未保存的页面**/
function postType(){
	var iframe = document.getElementsByTagName('iframe')[0].contentWindow;
    var postType =iframe.document.getElementById('postType');
    var postTypeVal = $(postType).val();
    $(".menu").removeClass("menu_open");
    $(".menu_import").addClass("menu_open");
    if(postTypeVal == 0){
    	$("#importSell").addClass("selected");
    	$("#importSell").siblings("a").removeClass("selected");
		$("#importSell").parents(".menu").siblings(".menu").find("a").removeClass("selected");
    }else{
    	$("#importRent").addClass("selected");
    	$("#importRent").siblings("a").removeClass("selected");
		$("#importRent").parents(".menu").siblings(".menu").find("a").removeClass("selected");
    }
}

/**关闭联系客服层**/
function qqcodeHide(){
	$("#qqcode-main").hide();
}


/**关闭续费层**/
function renewalTipHide(){
	$(".renewalTip").hide();
}

/*公告提示定时器*/
function startTimerTip() {
    //$('body').stopTime();//停止定时器
    noticeContentTip();
  //  NewHouseNoticeCount();
    var time = (1000 * 60 * 10);
    var time2 = (1000 * 30);
    setInterval(noticeContentTip, time);
  //  setInterval(NewHouseNoticeCount, time2);
    
	//$('body').everyTime(time + 's', noticeContentTip);//启动定时器
}

/**获取公告内容**/
function noticeContentTip() {
	var sum = 0;
		$.ajax({
			url: $("#contextPath").val() + "/Notice/GetNoticeContentTip",
			type : "get",
			cache: false,
			dataType: "json",
			beforeSend: function(XMLHttpRequest) {
			},
			success: function(result) {
			    var noticeList = result.data;
				var html = "";
				$("#noticeTip").html("");
				if (noticeList.length > 0) {
				    $("#tsMsgIcon").addClass("tsicon");
				    html += '<div class="noticeTip"><div class="noticeTip-header">' +
							'<span class="noticeTipLog"></span>' +
							'<span class="noticeHeader-del"></span></div>';
				    for (var i = 0; i < noticeList.length; i++) {
				        sum++;
				        noticeContentList = dStripHtml(noticeList[i].NoticeContent);
				        noticeContentList = noticeContentList.replace(/&nbsp;/ig, "");
				        noticeContentList = trimAll(noticeContentList, "g");
				        var noticeContent = noticeContentList.length > 68 ? noticeContentList.substring(0, 68) + "..." : noticeContentList;
				        if (i > 0) {
				            html += '<div style="display:none" id="noticeList_' + i + '" class="noticeList" index="' + i + '">';
				            html += '<h5 class="noticeTip-title"><a href="javascript:hrefLink(\'/Notice/DetailView?noticeId=' + noticeList[i].NoticeId + '\')">' + noticeList[i].Title + '</a></h5>';
				            html += '<div class="noticeTip-content"><a href="javascript:hrefLink(\'/Notice/DetailView?noticeId=' + noticeList[i].NoticeId + '\')">' + noticeContent + '</a></div>';
				            html += '</div>';
				        } else {
				            html += '<div id="noticeList_' + i + '" class="noticeList" index="' + i + '">';
				            html += '<h5 class="noticeTip-title"><a href="javascript:hrefLink(\'/Notice/DetailView?noticeId=' + noticeList[i].NoticeId + '\')">' + noticeList[i].Title + '</a></h5>';
				            html += '<div class="noticeTip-content"><a href="javascript:hrefLink(\'/Notice/DetailView?noticeId=' + noticeList[i].NoticeId + '\')">' + noticeContent + '</a></div>';
				            html += '</div>';
				        }
				    }
				    if (sum > 1) {
				        html += '<div class="noticeTip-bottom">' +
                        '<a class="noticeTip-more innort float-l")">忽略全部</a>' +
                        '<span class="notice-bottom-font">' +
						'<span class="notice-bottom-sum">1</span>/' + noticeList.length + '</span>' +
						'<span class="noticeTip-bottom-per" onclick="toper()"></span>' +
						'<span class="noticeTip-bottom-next" onclick="tonext(' + noticeList.length + ')"></span>' +
						'<a class="noticeTip-more float-r" href="javascript:hrefLink(\'/Notice/NoticeList?type=-1\')">查看更多</a></div></div>';
				    } else {
				        html += '<div class="noticeTip-bottom">' +
								'<a class="noticeTip-more innort float-l")">忽略</a>' +
								'<a class="noticeTip-more float-r" href="javascript:hrefLink(\'/Notice/NoticeList?type=-1\')">查看更多</a></div></div>';
				    }
				    $("#noticeTip").append(html);
				    $("#noticeTip").fadeIn(3000);
				}
				else {
				    $("#tsMsgIcon").removeClass("tsicon");
				}
				
				$(".noticeHeader-del").click(function(){
					noticeTipHide();
				});
				$(".noticeTip-content").click(function(){
					noticeTipHide();
				});
				$(".noticeTip-title").click(function(){
					noticeTipHide();
				});
				$(".noticeTip-more").click(function(){
					noticeTipHide();
				});
				$(".innort").click(function () {
				    noticeTipHide();
				   
				});
			},
			error: function(jqXHR) {
			},
			complete:function(XMLHttpRequest, textStatus) {
			}
		});
	}


/*公告隐藏*/
function noticeTipHide() {
    $("#tsMsgIcon").removeClass("tsicon");
    $("#noticeTip").fadeOut(3000);
    $.ajax({
        type: 'post',
        url: '/Notice/NoticeSetAllIsRead'
    });
}

/*公告翻页*/
/*向前*/
var index = "";
function toper(){
	var objs = $(".noticeList");
	objs.each(function(){
		if($(this).css("display") == 'block'){
			index = $(this).attr("index");
		}
	});
	if(index > 0){
		$("#noticeList_"+ index).hide();
		index--;
		$(".notice-bottom-sum").html(index+1);
		$("#noticeList_"+ index).show();
	}
}

/*向后*/
function tonext(size){
	var objs = $(".noticeList");
	objs.each(function(){
		if($(this).css("display") == 'block'){
			index = $(this).attr("index");
		}
	});
	if(index < size - 1){
		$("#noticeList_"+ index).hide();
		index++;
		$(".notice-bottom-sum").html(index+1);
		$("#noticeList_"+ index).show();
	}
}

function dStripHtml(str) {
	str = str.replace(/<\/?[^>]*>/g,''); //去除HTML tag
	str.value = str.replace(/[ | ]*\n/g,'\n'); //去除行尾空白
	//str = str.replace(/\n[\s| | ]*\r/g,'\n'); //去除多余空行
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

/**定时修改提醒状态**/
var loop = 0;
function startModifyTip() {
	$('title').everyTime(1 + "s", modifyTip);//启动定时器
}

function colseModifyTip(){
	loop = 0;
	$('title').stopTime();
	document.title = '我的盒子--房产盒子';
	$(".newHouse").hide();
}

var changeTip = 0;
function modifyTip(){
	if (changeTip == 0) {
		changeTip = 1;
		document.title='【个人房源 】';
		$("#newHouse-content").html("个人房源");
	} else {
		changeTip = 0;
		document.title='【有新房源，请关注 】';
		$("#newHouse-content").html("有新房源，请关注");
	}
	
	if(loop < 3){
		playSound($("#contextPath").val() + '/voice.wav');
	}
	loop++;
}

function  playSound(sound)
{
      if(navigator.appName == "Microsoft Internet Explorer")
      {
        var snd = document.createElement("bgsound");
        document.getElementsByTagName("body")[0].appendChild(snd);
        snd.src = sound;
     }
     else
     {
            var obj = document.createElement("object");
            obj.width="0px";
            obj.height="0px";
            obj.type = "audio/x-wav";
            obj.data = sound;            
            var body = document.getElementsByTagName("body")[0];
            body.appendChild(obj);
     }
        
}


/**监听浏览器关闭事件**/

function addFeedback() {
    var content = $("#FeedbackContent").val();
    if (content != "") {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Home/AddFeedback',
            data: { FeedbackContent: content },
            success: function (data) {
                if (data.status > 0) {
                    $("#FeedbackContent").val("");
                    art.dialog.alert("非常感谢您的意见和反馈！");
                }
                else {
                    art.dialog.alert("抱歉！系统发生错误，请重试！！");
                }
            }
        });
    }
    else {
        $("#FeedbackContent").focus();
    }
}

var handler = null;
var mymsg="";
/*function UpdateTitle(str1, str2, interval) {
    if (handler != null) return;
    handler = setInterval(function () {
        //document.title = (document.title == str2) ? str1 : str2;
		mymsg=str1;
		mymsg=mymsg.substring(0,mymsg.length)+mymsg.substring(0,1);   
   		document.title = mymsg;
		}, interval);
}*/
function UpdateTitle() {
	mymsg=mymsg.substring(1,mymsg.length)+mymsg.substring(0,1);   
   	document.title = mymsg;
}
/**获取新房源统计数**/
function NewHouseNoticeCount() {
    var sum = 0;
    $.ajax({
        url: "/Notice/NewHouseNoticeCount",
        type: "get",
        cache: false,
        dataType: "json",
        beforeSend: function(XMLHttpRequest) {
        },
        success: function (result) {
            if (parseInt(result) > 0) {
                $("#newhouseCount").show();
                $("#newhouseCount").html("<a href=\"javascript:hrefLinkNewHouseNotice()\">" + result + "</a>");
				//UpdateTitle("【新房源】房产盒子", "【　　　】房产盒子", 1000);
				//UpdateTitle("收到" + result + "条新房源...", "【　　　】房产盒子", 500);
				mymsg="收到" + result + "条新房源...";
				if (handler != null) return;
    			handler = setInterval("UpdateTitle()",1000);
                
            } else {
                $("#newhouseCount").hide();
                $("#newhouseCount").html();
                if (handler != null) {
                    clearInterval(handler);
                    handler = null;
                    document.title = "我的盒子--房产盒子";
                }
            }
        }
    });
}

function hrefLinkNewHouseNotice() {
    $("#newhouseCount").hide();
    hrefLink('/Notice/NewHouseNotice'); 
	if (handler != null) {
			clearInterval(handler);
			handler = null;
			document.title = "我的盒子--房产盒子";  
		}   
}
