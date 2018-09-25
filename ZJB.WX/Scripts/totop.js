$(function(){
	$(".lr-gotop").click(function(){
		var ua = navigator.userAgent.toLowerCase();
		var iframeName = document.getElementsByTagName('iframe')[0].contentWindow;
		var ifmObj = "";
		if(ua.indexOf("chrome") > 0){
			ifmObj = iframeName.document.getElementsByTagName("body")[0];
		}else{
			ifmObj = iframeName.document.documentElement;
		}
		$(ifmObj).animate({scrollTop:0},1000);
	});	
	
	  $(".lr-qrcode").click(function(){
		  $("#qqcode-main").hide();
		  $(".lr-pop").toggle();
	  });
		
	 $(".lr-close").click(function(){
		 $(".lr-pop").hide();
	 });
 
	 	
	 $(".lr-qqcode").click(function(){
		 $(".lr-pop").hide();
		 $("#qqcode-main").toggle();
	 });
	 
	 $(".del-tip").click(function(){
		 $("#lrToolRb").hide();
	 });
});