
$(function(){
	var contextPath = $("#contextPath").val();
	//网站管理
	$(".sale-tab-sub li").live("click",function(){
		$(".sale-tab-sub li").removeClass("selected");
		$(this).addClass("selected");
		var index = $(this).parent().find("li").index($(this));
		$(this).parents(".lay_card_panel").find(".sites_list_panel").children("div").each(function() {
			if (index == 0) {
				$(this).show();
			} else if (index == 1) {
			    if ($(this).attr("userId") > 0) {
					$(this).show();
				} else {
					$(this).hide();
				}
			} else if (index == 2) {
			    if ($(this).attr("userId") > 0) {
					$(this).hide();
				} else {
					$(this).show();
				}
			}
		});
	});
	
	$(".add_account_function").die().live("click",function(){
		var clickedBtn = $(this);
		var userWebId = $(this).attr("userWebId");
		var userWebCount = $(this).attr("userWebCount");
		var city = $("#city").val();
		if (city == 755) {
			if (userWebCount >= 3) {
			    art.dialog.alert("房产盒子暂时最多只能在每个网站上绑定三个账号，<br>如果要修改账号名请先删除旧账号然后再添加！");
				return;
			}
		} else {
			if (userWebId >= 1) {
				art.dialog.alert("房产盒子暂时最多只能在每个网站上绑定一个账号，<br>如果要修改账号名请先删除旧账号然后再添加！");
				return;
			}
		}
		//var accountAddUrl = contextPath + '../html/account_add.html';
		art.dialog({
		    title: '添加账号',
		    content: document.getElementById("add_account_win"),
			init: function () {
			 
			    $("#loginName").val("");
			    $("#loginPwd").val("");
			    $("#loginName").attr("disabled", true);
			    $("#loginName").removeAttr('disabled');
				//if (city != 755) {
					$("#setDef").hide();
					$("#setAccount").hide();
				//}
				$("#loginName").focus();
			},
			ok: function () {
			 
				var regexp = /(^\s*)|(\s*$)/g;
				var loginName = $("#loginName").val().replace(regexp, "");
				var loginPwd = $("#loginPwd").val().replace(regexp, "");
				var isDefault = $("#isDefaultCbx").val();

				if(! checkRangeWithHint(loginName.length, 1, 30, "用户名的长度")) {
					return false; 
				}
				if(! checkRangeWithHint(loginPwd.length, 1, 30, "密码的长度")) {
					return false; 
				}
				$.ajax({ 
				type: "post", 
				url: contextPath + "/PerSonManage/UserSiteAdd",
					data: "webBasicId=" + clickedBtn.attr("webBasicId") + "&loginName=" + loginName + 
							"&loginPwd=" + loginPwd + "&isDefault=" + isDefault,
					dataType: "json",
	                beforeSend: function(XMLHttpRequest) {
	                	parent.loadingShow();
	                },
					success: function (data) {
						 if(data.msg=="添加成功") {
				        		art.dialog.tips(data.msg);
				        		location.reload();
	                      } else {
	                    	  	art.dialog.alert(data.msg);
						  }	
					},
	                error: function(jqXHR) {
	                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
	                },
	                complete:function(XMLHttpRequest, textStatus){                     
	        			parent.loadingHide();
	        		} 
				});
			},
			cancel: true
		});
	});
	
	$(".register_detail_function").die().live("click",function(){
		art.dialog.load('soufun.html', {
			title: '规则说明',
			cancel: true
		});
	});	
	
	$(".site_del_function").die().live("click",function(){
		var userWebId = $(this).parents("dl").find("input[type=radio]:checked").attr('webbasicid');
		var isdef = $(this).parents("dl").find("input[type=radio]:checked").attr('isdef');
		var userWebCount = $(this).parents("dl").find("input[type=radio]:checked").attr('userWebCount');
		var city = $("#city").val();
		if(isdef == "true" && (userWebCount > 1) && (city == 755) ){
			art.dialog.alert("默认账号无法删除,若要删除,请先切换默认账号！");
		}else{
			art.dialog.confirm('您确定要删除账号？', function(){
				$.ajax({ 
			        type: "post", 
			        url: contextPath + "/PerSonManage/UserSiteDelete",
			        data: "SiteId=" + userWebId,
			        success: function (data) {
			        	//art.dialog.tips(data.msg);
						location.reload();
			        },
	                error: function(jqXHR) {
	                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
	                }
				});
			});
		}
	});
	
	$(".site_check_function").die().live("click",function(){
	
	    var userWebId = $(this).parents("dl").find("input[type=radio]:checked").attr('userWebId');
	   
		art.dialog.confirm('检测绑定的账号密码并激活成功账号?', function(){
			$.ajax({ 
		        type: "post", 
		        url: contextPath + "/PersonManage/CheckSite",
		        data: "siteId=" + userWebId,
		        beforeSend: function(XMLHttpRequest){
		    		parent.loadingShow();
				}, 
		        success: function (data) {
					if(data.msg=="账号验证成功并已经激活使用！") {
		        		art.dialog.tips(data.msg);
		        		location.reload();
                    } else {
                    	art.dialog.alert(data.msg);
				    }
		        },
                error: function(jqXHR) {
                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
                },
                complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
        			parent.loadingHide();
        		} 
			});
		});
	});
	
	$(".site_setDef_function").die().live("click",function(){
		var userWebId = $(this).parents("dl").find("input[type=radio]:checked").attr('userWebId');
		var loginName = $(this).parents("dl").find("input[type=radio]:checked").attr('loginName');
		art.dialog.confirm('将' + loginName + '设置位默认账号?', function(){
			$.ajax({ 
		        type: "post", 
		        url: contextPath + "/ajax/site/changeDefault.do", 
		        data: "userWebId=" + userWebId,
		        beforeSend: function(XMLHttpRequest){
		    		parent.loadingShow();
				}, 
		        success: function (data) {
                    art.dialog.alert(data.msg);
	        		location.reload();
		        },
                error: function(jqXHR) {
                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
                },
                complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
        			parent.loadingHide();
        		} 
			});
		});
	});
	
	$(".site_view_function").die().live("click",function(){
	    var userWebId = $(this).parents("dl").find("input[type=radio]:checked").attr('webBasicId');
		var flag = false;
		var dialog = art.dialog({
			title: "查看密码", 
			content: document.getElementById("site_view_win"),
			init: function () {
				$("#userPwd").val("");
				$("#userPwd").focus();
			},
			ok: function () {
				if(flag) return true;
				var userPwd = $("#userPwd").val();
				if(userPwd == ""){
					art.dialog.alert("登录密码不能为空");
					$("#userPwd").focus();
				}
				$.ajax({ 
				type: "post", 
				url: contextPath + "/PersonManage/UserSitePwdShow",
					data: "siteId=" + userWebId + "&userPwd=" + $("#userPwd").val(),
					success: function (data) {
					    flag = true;
					    if (data.msg == "登陆密码错误") {
					        dialog.content("<p>登陆密码错误</p>");
					    }
					    else {
					        dialog.content("<p>用户名:" + data.loginName +
                                    "<br>密码:" + data.loginPwd + "</p>");
					    }
					},
	                error: function(jqXHR) {
	                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
	    				$("#userPwd").val("");
	    				$("#userPwd").focus();
	                }
				});
				
				return false;
		    },
		    cancel: true 
		});
	});

	$(".site_pwd_edit_function").die().live("click",function(){
	    var userWebId = $(this).parents("dl").find("input[type=radio]:checked").attr('webBasicId');
		var loginName = $(this).parents("dl").find("input[type=radio]:checked").attr('loginName');

		var accountAddUrl = contextPath + '../html/account_add.html';
		art.dialog({
		    title: '修改密码',
		    content: document.getElementById("add_account_win"),
			init: function () {
				$("#loginName").val(loginName);
				$("#loginName").attr("disabled", "disabled");
				$("#setDef").hide();
				$("#loginPwd").focus();
				if (city != 755) {
					$("#setAccount").hide();
				}
			},
			ok: function () {
				var loginPwd = $("#loginPwd").val();
				if(! checkRangeWithHint(loginPwd.length, 1, 30, "密码的长度")) return false; 
				$.ajax({ 
					type: "post", 
					url: contextPath + "/PersonManage/UserSitePwdUpdate", 
					data: "SiteId=" + userWebId + "&loginPwd=" + loginPwd,
					beforeSend: function(XMLHttpRequest){
			    		parent.loadingShow();
					}, 
					success: function (data) {
						if(data.msg=="密码修改成功") {
			        		art.dialog.tips(data.msg);
			        		location.reload();
                        } else {
                        	art.dialog.alert(data.msg);
					    }	
					},
	                error: function(jqXHR) {
	                    art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
	                },
	                complete:function(XMLHttpRequest, textStatus){//通过XMLHttpRequest取得响应头，sessionstatus                     
	        			parent.loadingHide();
	        		} 
				});
			},
			cancel: true
		});
	});

	$(".account-input").die().live("click",function(){
		var dis = $(this).attr("dis");
	
		if (dis == 0) {
			$(this).parents("dl").find(".site_check_function").show();
		} else{
			$(this).parents("dl").find(".site_check_function").hide();
		}
	});
});

function gotoReg(url) {
	window.open(url);
}

function initAccount(){
	var objs = $(".account-input");
	objs.each(function(){
		var dis = $(this).attr("dis");
		var isDef = $(this).attr("isDef");
		var liIndex = $(this).attr("liIndex");
		if (liIndex == 1) {
			if (dis == 0) {
				$(this).parents("dl").find(".site_check_function").show();
			} else{
				$(this).parents("dl").find(".site_check_function").hide();
			}
			
			if (isDef == "false") {
				$(this).parents("dl").find(".site_setDef_function").show();
			} else {
				$(this).parents("dl").find(".site_setDef_function").hide();
			}
			$(this).attr("checked", "checked");
		}
	});
}

function loginWeb(that){
	var userWebId = $(that).parents("dl").find("input[type=radio]:checked").attr('userWebId');
	var url = '/PersonManage/SiteView?userWebId=' + userWebId;
	window.open(url);
}
