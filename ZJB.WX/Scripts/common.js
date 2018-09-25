$(function(){
    $.ajaxSetup({
    	beforeSend: function(XMLHttpRequest){
    		loadingShow();
		}, 
		complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
			loadingHide();
		}    
    });
	
    /*iframe部分高度自适应*/
	var height = document.documentElement.clientHeight;
	$(".saleHouse").css("min-height",height-2);
	$(".saleHouse").css("_height",height-2);
});

/*正下加载展示*/
function loadingShow(){
	var height =$(document).height();
	$(".loading").css({"left":350,"top":(height-165)/2});
	$(".loading").show();
	$(".loading-bg").show();
}

/*正下加载隐藏*/
function loadingHide(){
	$(".loading-bg").hide();
	$(".loading").hide();
}