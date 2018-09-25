// JavaScript Document
function r(){
	var t=$(window).width();
	return 640>t?($("html").css("font-size",t/3.2),$("body").removeClass("iwjw-max")):($("html").css("font-size",200),$("body").addClass("iwjw-max"))
}
var i=$(window).width();
i>640?setTimeout(function(){r()},200):r(),$(window).resize(function(){r();});

//创建提示框
function createDiv(txt) {
	var html="";
	html+="<div class=\"mod-smallnote smallnote-mobile scale\"><div class=\"smallnote-text\">"+txt+"</div></div>";
	return html
}