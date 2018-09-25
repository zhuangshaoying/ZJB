$(function(){
	$("#btn_upd_pwd").click(function() {
		var bValid = true;
		var oldPwd = $("#oldPassword").val();
		var newPwd = $("#newPassword").val();
		var enterPwd = $("#enterPassword").val();
		bValid = bValid && oldPwd.length >= 6;
		bValid = bValid && newPwd.length >= 6;
		bValid = bValid && enterPwd.length >= 6;
		if (bValid) {
		    bValid = bValid && (newPwd == enterPwd);
		}
		else {
		    art.dialog.alert("密码至少6位");
		    return;
		}
		if (!bValid) {
			art.dialog.alert("两次输入的密码不一致！");
			return;
		}
		
		$.ajax({
		    url: "/PersonManage/UserUpdatePwd",
			type: "post",
			dataType : "json",
			data: {
			    "oldPwd": oldPwd,
			    "newPwd": newPwd
			},
			success: function(data) {
				art.dialog.alert(data.msg);
			}
		});
	});
	
	$("#btn_reset_pwd").click(function() {
	    $("#oldPassword").val("");
		$("#newPassword").val("");
		$("#enterPassword").val("");
	});
});




