/**************头像操作*********************/
var defaultHeadImg = "/images/txDefault.png";
var imageW200H200Q85 = "?imageMogr2/strip|imageView2/1/w/200/h/200/q/85";
var imageW140H140Q85 = "?imageMogr2/strip|imageView2/1/w/140/h/140/q/85";
var imageW50H50Q85="?imageMogr2/strip|imageView2/1/w/50/h/50/q/85";
var oldHeadImage = $("#LeftHeadImg").attr("id") != undefined ? $("#LeftHeadImg").attr("src").replace(imageW200H200Q85, "") : defaultHeadImg;
var newHeadImage = oldHeadImage;
function UploadHeadImageSuccess(msg)
{
    if (msg.length > 0) {
        var data = msg[0];
        newHeadImage = data.url;
        var sizeLeft = imageW200H200Q85;
        var sizeSquare = imageW140H140Q85;
        var sizeRound = imageW140H140Q85;
        $("#LeftHeadImg").attr("src", newHeadImage + sizeLeft);
        $("#square_uploadFileSrc").attr("src", newHeadImage + sizeSquare);
        $("#round_uploadFileSrc").attr("src", newHeadImage + sizeRound);
    }
    else {
        jQuery.dialog.tips("上传图片太大", 1.5, "warning.png");
    }
}
function submitUploadHeadImg()
{
    if (newHeadImage != "" && newHeadImage != defaultHeadImg && newHeadImage != oldHeadImage) {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/HeadImgSave',
            data: { url: newHeadImage },
            success: function (result) {
                if (result.status > 0) {
                    jQuery.dialog.tips(result.msg, 1.5, "success.gif");
                    setTimeout(function () { location.href = "/User/BrokerInfo"; }, 1500);
                   
                }
                else {
                    jQuery.dialog.tips(result.msg, 1.5, "warning.png");
                }
            }
        });
    }
    else {
        jQuery.dialog.tips("请选择头像", 1.5, "warning.png");
    }
}
$("#UploadBtn").click(function () {
    uploadFileHandler = new UploadFileHandler({ Success: function (data) { UploadHeadImageSuccess(data); } }); //上传文件控件
    uploadFileHandler.UploadClick();
});
$("#img_submit").click(function () {
    submitUploadHeadImg();
});


/****************手机号操作*********************/
var isCanSubmit_Tel = false;
function checkTel() {
    var tel = $("#strMobile").val();
    if (tel == "") {
        $("#strMobile").focus();
        jQuery.dialog.tips("请填写手机号", 1.5, "warning.png");
        return false;
    }
    else if (!istel(tel)) {
        jQuery.dialog.tips("手机号码格式有误", 1.5, "warning.png");
        return false;
    }
    return true;
}
function istel(mobile) {
    var numberRegStr = /^\d{11}$/;
    var regNum = new RegExp(numberRegStr);
    if (regNum.test(mobile)) {
        var telRegStr = /^(13|14|15|17|18)\d{9}$/;
        var reg = new RegExp(telRegStr);
        if (reg.test(mobile)) {
            return true;
        }
        else {
            return false;
        }
    }
    else {
        return false;
    }
}
function checkTelExists() {
    var tel = $("#strMobile").val();
    if (istel(tel)) {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/CheckTelExists',
            data: { tel: tel },
            async: false,
            success: function (result) {
                if (result.exists == 1) {
                    $("#vcode").attr("disabled", true);
                    $("#strVcode").attr("disabled", true);
                    isCanSubmit_Tel = false;
                    $("#errorTel").html("该手机号码已被注册");
                    $("#errorTel").show();
                }
                else {
                    $("#strVcode").attr("disabled", false);
                    $("#vcode").attr("disabled", false);
                    $("#errorTel").html("");
                    $("#errorTel").hide();
                    isCanSubmit_Tel = true;
                }
            }
        });
    }
    else {
        $("#strVcode").attr("disabled", true);
        $("#vcode").attr("disabled", true);
        $("#errorTel").html("手机号码格式有误");
        $("#errorTel").show();
    }
}
function submitMobile()
{
    if (checkTel() && isCanSubmit_Tel) {
        var vaildCode = $("#strVcode").val();
        var tel=$("#strMobile").val();
        $.ajax({
            type: 'post',
            dataType: 'json',
            url:"/User/EditMobile",
            data: { tel: tel, code: vaildCode },
            success: function (result) {
                jQuery.dialog.tips(result.msg, 1.5, "success.gif");
                if (result.status == 0) {
                    setTimeout(function () { window.location.href = "/User/BrokerInfo"; }, 1500);
                }
            }
        });
    }
}
function getPhoneCode() {
    var tel = $("#strMobile").val();
    if (istel(tel)) {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/GetPhoneCode',
            data: { tel: tel },
            beforeSend: function () {
                $("#vcode").attr("disabled", true);
            },
            success: function (result) {
                if (result.status == 1) {
                    var miao = 120;
                    jQuery.dialog.tips("验证码已发送", 1.5, "warning.png");
                    var timehandler = setInterval(function () {
                        $("#vcode").val(miao + "秒后再获取");
                        miao--;
                        if (timehandler != null && miao <= 0) {
                            clearInterval(timehandler);
                            $("#vcode").attr("disabled", false);
                            $("#vcode").val("获取短信验证码");
                        }
                    }, 1000);
                }
                else {
                    jQuery.dialog.tips("验证码获取失败", 1.5, "warning.png");
                }
            }
        });
    }

}
$("#strMobile").blur(function () {
    checkTelExists();
});
$("#vcode").click(function () {
    getPhoneCode();
});
$("#mobile_submit").click(function () {
    submitMobile();
});


//************邮箱操作*******************/
function isEmail(email)
{
    return (/^[a-z0-9_+.-]+\@([a-z0-9-]+\.)+[a-z0-9]{2,4}$/i).test(email);
}
function checkEmail()
{
    var email = $("#email").val();
    if (!isEmail(email)) {
        jQuery.dialog.tips("邮箱格式有误", 1.5, "warning.png");
        return false;
    }
    return true;
}
function submitEmailEdit()
{
    if (checkEmail()) {
        var email = $("#email").val();
        $.ajax({
            type: 'post',
            dataType: 'json',
            url:'/User/EditEmail',
            data: { email: email },
            success: function (result) {
                jQuery.dialog.tips(result.msg, 1.5, "success.gif");
                if (result.status == 0) {
                    setTimeout(function () { window.location.href = "/User/BrokerInfo"; }, 1500);
                }
            }
        });
    }
}
$("#email_submit").click(function () {
    submitEmailEdit();
});
//**************修改真实姓名*****************/
function submitEnrolnNameEdit()
{
    var name = $("#enrolnName").val();
    if (name.length < 2 || name.length > 6) {
        jQuery.dialog.tips("真实姓名必须为2~6个字", 1.5, "warning.png");
        return false;
    }
    if (name != "")
    {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/EnrolnNameSave',
            data: { name: name },
            success: function (result) {
                if (result.status == 0) {
                    jQuery.dialog.tips(result.msg, 1.5, "success.gif");
                    setTimeout(function () { window.location.href = "/User/BrokerInfo"; }, 1500);
                }
                else {
                    jQuery.dialog.tips(result.msg, 1.5, "warning.png");
                }
            }
        });
    }
    else {
        $("#enrolnName").focus();
    }
}
$("#enrolnName_submit").click(function () {
    submitEnrolnNameEdit();
});