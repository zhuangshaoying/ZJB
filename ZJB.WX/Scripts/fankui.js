// JavaScript Document
$(".msgeditor-bd > textarea").bind("click",function(){
	$(this).css("height","89px");
	$(this).parent().css("height","89px");
})
$(".msgeditor-bd > textarea").bind("focus",function(){
	if($(this).val()!="") {
		$("#mb-msgeditor-main").removeClass("cmpdisabled");	
	}
	else {
		$("#mb-msgeditor-main").addClass("cmpdisabled");	
	}
})
$(".msgeditor-bd > textarea").bind("keyup",function(){
	if($(this).val()!="") {
		$("#mb-msgeditor-main").removeClass("cmpdisabled");	
	}
	else {
		$("#mb-msgeditor-main").addClass("cmpdisabled");	
	}
})
$(".mb-cmteditor").find("textarea").bind("keyup",function(){
	if($(this).val()!="") {
		$(this).parent().parent().parent().removeClass("cmpdisabled");	
	}
	else {
		$(this).parent().parent().parent().addClass("cmpdisabled");	
	}
})
$(".mb-cmteditor").find("textarea").bind("focus",function(){
	if($(this).val()!="") {
		$(this).parent().parent().parent().removeClass("cmpdisabled");	
	}
	else {
		$(this).parent().parent().parent().addClass("cmpdisabled");	
	}
})
$(".msg-ctrl").find(".msg-ctrl-cmt").bind("click",function(){
	$(this).parent().parent().find(".mb-cmtbox").show();	
})
$(".msg-ctrl").find(".msg-ctrl-cmt").bind("click",function(){
	$(this).parent().parent().find(".mb-cmtbox").show();	
})
/*********表情*********/
$(".msgeditor-addons-smiley").bind("click",function(e){
	var obj=$(this);
	if($(this).attr("data-value")==0) {
		$(".mb-smiley-layer").remove();
		$(".msgeditor-addons-smiley").attr("data-value","0");
		var strhtml="";
		strhtml="<div style=\"z-index: 5050\" class=\"layer layer-shim br5 mb-smiley-layer\">";
		strhtml+="<div class=\"layer-main\"><span class=\"layer-arrow p1\"></span>";
		strhtml+="<div class=\"layer-bd\"><ul>";
		for (var i = 0; i < data.length; i++) {
			var onjJsonItem = data[i];
			strhtml += "<li fw=\"" + onjJsonItem.Faceword + "\">";
			strhtml += "<a> <img src=\"" + onjJsonItem.FaceUrl + "\"></a></li>";
		}                
		strhtml+="</ul></div>";
		strhtml+="</div>";
		strhtml+="</div>";
		$("body").append(strhtml);
		$(".mb-smiley-layer").css("top",$(this).offset().top+20+"px");
		$(".mb-smiley-layer").css("left",$(this).offset().left-30+"px");
		$(this).attr("data-value","1")
	}
	else {
		$(this).attr("data-value","0");
		$(".mb-smiley-layer").remove();
	}
	$(".layer-main").find("ul li").click(function (e) {
		var objLi = jQuery(this);
		var fw = objLi.attr("fw");
		if (fw == undefined || fw == "") return;
		$(".mb-smiley-layer").remove();
		$(".msgeditor-addons-smiley").attr("data-value","0");
		InsertString(obj.parent().parent().parent().find("textarea").attr("id"),"["+fw+"]");
		e.stopPropagation();
	});
	e.stopPropagation();
});
$(document).click(function() {
	$(".mb-smiley-layer").remove();
	$(".msgeditor-addons-smiley").attr("data-value","0");
});
function getStrLength(str) {
	var cArr = str.match(/[^\x00-\xff]/ig);
	return str.length + (cArr == null ? 0 : cArr.length);
}
function InsertString(id, str){
	var tb=document.getElementById(id);
    if (document.all){
        var r = document.selection.createRange();
        document.selection.empty();
        r.text = str;
        r.collapse();
        r.select();
    }
    else{
        var newstart = tb.selectionStart+str.length;
        tb.value=tb.value.substr(0,tb.selectionStart)+str+tb.value.substring(tb.selectionEnd);
        tb.selectionStart = newstart;
        tb.selectionEnd = newstart;
    }
	tb.focus();
} 
$(".msgeditor-addons .attachbtndoc").bind("click",function(){
	if($(this).parent().parent().parent().find(".cattachqueue-wrap").css("display")=="block") {
		$(this).parent().parent().parent().find(".cattachqueue-wrap").hide();	
	}
	else {
		$(this).parent().parent().parent().find(".cattachqueue-wrap").show();
	}
})