

//重定向
function redirect(id) {
	href = parent.$("#" + id).attr("href");
	url = href.substring(href.indexOf("'") + 1, href.lastIndexOf("'"));
	parent.hrefLink(url);
	/*parent.$("#" + id).parent().prev().click();*/
	parent.$("#"+id).parents(".menu").addClass("menu_open");
	parent.$("#"+id).parents(".menu").siblings().removeClass("menu_open");
	parent.$("#" + id).click();
  
}
