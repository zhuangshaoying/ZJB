/*** 登录*/

$(function() {
	
    $("#phone").focus();
	//登录
	$("#login_btn").click(function(){
		login();
	});
	
	$("#userPwd").keydown(function(event){
		if(event.keyCode == 13){
			login();
		}
	});
	
});


var isCanSubmit = true;
function login(){
	if(checkNavigator()){
		return;
	}
	var contextPath = $("#contextPath").val();
	if ($("#phone").val() == '' || $("#userPwd").val() == '') {
		alert('用户名和密码为必填项！');
	} else if (isCanSubmit) {
		$.ajax({
		    url: "/User/DoLogin",
            type: "post",
            data: {
            	"phone" : $("#phone").val(),
            	"userPwd": $("#userPwd").val()
            },
            dataType: "json",
            beforeSend: function(XMLHttpRequest) {
                isCanSubmit = false;
                $("#login_btn").html("登录中...");
            },
            success: function(result) {
                if (result.status == 0) {
                    var gotoUrl=$("#tourl").val();
                    if (gotoUrl != "") {
                        window.location.href = gotoUrl;
                    }
                    else {
                        window.location.href = '/M';
                    }
                }
                else {
                    alert(result.msg);
                }
            },
            error: function(jqXHR) {
            	alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete: function(XMLHttpRequest, textStatus) {
                isCanSubmit = true;
                $("#login_btn").html("登录");
            }
        });
	}
}

function enterSumbit() {
	var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异
	if (event.keyCode == 13){
		login();
	}  
}

function checkNavigator(){
    var Sys = {};
    var ua = navigator.userAgent.toLowerCase();
    var s;
    (s = ua.match(/msie ([\d.]+)/)) ? Sys.ie = s[1] :
    (s = ua.match(/firefox\/([\d.]+)/)) ? Sys.firefox = s[1] :
    (s = ua.match(/chrome\/([\d.]+)/)) ? Sys.chrome = s[1] :
    (s = ua.match(/opera.([\d.]+)/)) ? Sys.opera = s[1] :
    (s = ua.match(/version\/([\d.]+).*safari/)) ? Sys.safari = s[1] : 0;

    if(Sys.ie == 6.0){
    	alert("房产盒子系统不支持IE6,在谷歌、火狐、及IE8以上的浏览器展现效果比较良好，建议使用相应的浏览器,谢谢合作!");
    	return true;
    }else{
    	return false;
    }
    //其它浏览器的版本判断
    /*if (Sys.firefox) document.write('Firefox: ' + Sys.firefox);
    if (Sys.chrome) document.write('Chrome: ' + Sys.chrome);
    if (Sys.opera) document.write('Opera: ' + Sys.opera);
    if (Sys.safari) document.write('Safari: ' + Sys.safari);*/
}
