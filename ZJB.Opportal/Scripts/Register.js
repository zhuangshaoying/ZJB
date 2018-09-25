var isCanSubmit_Tel = $("#isCanSubmit_Tel").val()==1?true:false;
var isCanSubmit_Name = false;

function CheckRegister()
{
    if (checkTel() && checkName()&&isCanSubmit_Tel && isCanSubmit_Name  &&CheckPwd() && checkCity() && checkCompany() && checkCompanyStore())
    {
        var tel = $("#tel").val();
        var name = $("#name").val();
        var pwd = $("#userPwd").val();
        var cityid = $("#cityId").val();
        var companyId = $("#companyId").val();
        var companyStoreId = $("#companyStoreId").val();
        var districtId=$("#districtId").val();
        var regionId=$("#regionId").val();
        var vaildCode = $("#code").val();
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/DoRegister',
            data: { Tel: tel, Name: name, Password: pwd, CityID: cityid, CompanyId: companyId, StoreId: companyStoreId,DistrictId:districtId,RegionId:regionId, code: vaildCode, invitationCode: $("#InvitationCode").val() },
            success: function (result) {
                art.dialog.alert(result.msg);
                if (result.status == 0) { 
                    setTimeout(function () { window.location.href = "/Home"; }, 1000);
                }
            }
        });
    }
}
function getPhoneCode()
{
    var tel = $("#tel").val();
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
                    alert("验证码已发送");
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
                    art.dialog.alert("验证码获取失败");
                }
            }
        });
    }

}
function telFocus()
{
    $("#errorTel").html("");
    $("#errorTel").hide();
}
function InvitationFocus() {
    $("#errorInvitation").html("");
    $("#errorInvitation").hide();
}
function checkTel() {
    var tel = $("#tel").val();
    if (tel == "") {
        $("#tel").focus();
        $("#errorTel").html("请填写手机号");
        $("#errorTel").show();
        return false;
    }
    else if (!istel(tel)) {
        $("#errorTel").html("手机号码格式有误");
        $("#errorTel").show();
    }
    $("#errorTel").html("");
    $("#errorTel").hide();
    return true;
}
function checkTelExists()
{
    var tel = $("#tel").val();
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
                    isCanSubmit_Tel = false;
                    $("#errorTel").html("该手机号码已被注册");
                    $("#errorTel").show();
                }
                else {
                    $("#vcode").attr("disabled", false);
                    isCanSubmit_Tel = true;
                }
            }
        });
    }
    else {
        $("#errorTel").html("手机号码格式有误");
        $("#errorTel").show();
    }
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
function CheckPwd()
{
    var pwd1 = $("#userPwd").val();
    var pwd2 = $("#re_userPwd").val();
    if (pwd1.length < 6)
    {
        $("#userPwd").focus();
        $("#errorPwd").html("密码至少6位");
        $("#errorPwd").show();
        return false;
    }
    if (pwd1 != pwd2) {
        $("#re_userPwd").focus();
        $("#errorPwd").html("两次密码不一致");
        $("#errorPwd").show();
        return false;
    }
    $("#errorPwd").html("");
    $("#errorPwd").hide();
    return true;
}
function CheckInvitationRegister() {
    var invitation = $("#Invitation").val();
    if (invitation == "") {
        $("#Invitation").focus();
        art.dialog.alert("请填写邀请码");
        return false;
    }
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/User/CheckInvitation',
        data: { code: invitation },
        async: false,
        success: function(result) {
            if (result.exists == 0) {

                $("#errorInvitation").html("该邀请码已被使用");
                $("#errorInvitation").show();
         
            } else {
                setTimeout(function () { window.location.href = "/user/register?code=" 
+ invitation; }, 1000);
            }
        }
    });

    return true;
}
function nameFocus()
{
    $("#errorName").html("");
    $("#errorName").hide();
}
function checkName() {
    var name = $("#name").val();
    if (name == "") {
        $("#name").focus();
        $("#errorName").html("请填写用户名");
        $("#errorName").show();
        return false;
    }
    return true;
}
function checkNameExists()
{
    var name = $("#name").val();
    if (name!="") {
        $.ajax({
            type: 'post',
            dataType: 'json',
            url: '/User/CheckNameExists',
            data: { name: name },
            async: false,
            success: function (result) {
                if (result.exists == 1) {
                    isCanSubmit_Name = false;
                    $("#errorName").html("该用户名已被注册");
                    $("#errorName").show();
                }
                else {
                    isCanSubmit_Name = true;
                }
            }
        });
    }
    else {
        $("#errorName").html("请填写用户名");
        $("#errorName").show();
    }
}
function checkCity() {
    var cityId = $("#cityId").val();
    if (cityId <= 0) {
        $("#errorCity").html("请选择所在城市");
        $("#errorCity").show();
        return false;
    }
    $("#errorCity").html("");
    $("#errorCity").hide();
    return true;
}
function checkCompany() {
    var companyId = $("#companyId").val();
    if (companyId <= 0) {
        $("#errorCompany").html("请填写所在公司");
        $("#errorCompany").show();
        return false;
    }
    $("#errorCompany").html("");
    $("#errorCompany").hide();
    return true;
}
function checkCompanyStore() {
    var companyStoreId = $("#companyStoreId").val();
    var districtId = $("#districtId").val();
    var regionId = $("#regionId").val();
    if (companyStoreId <= 0) {
        if (districtId <= 0 || regionId <= 0) {
            $("#errorCompanyStore").html("请填写工作范围");
            $("#errorCompanyStore").show();
            return false;
        }
    }
    $("#errorCompanyStore").html("");
    $("#errorCompanyStore").hide();
    return true;
}





/*城市 公司下拉操作 */

jQuery("#select_boxer").click(function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    jQuery("#citylist").toggle();
    jQuery("#searchlistblock").hide();
    jQuery("#searchlistarea").hide()
});
jQuery("#citytab a").click(function () {
    jQuery(this).siblings().removeClass("on");
    jQuery(this).addClass("on");
    var B = jQuery("#citytab a").index(this);
    jQuery("#citylist ul").hide();
    jQuery("#city" + B).show()
});
jQuery("#citylist ul li a").click(function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    jQuery("#selectcityname").html(jQuery(this).text());
    jQuery("#cityId").val(jQuery(this).attr("cityid"));
    jQuery("#citylist").hide();

    ///公司信息清掉
    $("#companyId").val(0);
    $("#companytext").val("输入并选择所属公司");
    $("#companylist").html("");
    $("#companylist").hide("");
    ///分店信息情掉
    $("#liCompanyStore").hide();
    $("#companyStoreId").val(0);
    $("#companyStoreText").val("输入并选择所属分店");
    $("#companyStorelist").html("");
    $("#companyStorelist").hide();
    ///工作范围清掉

    $("#liWorkRange").hide();
    $("#districtId").val(0);
    $("#districtText").html("请选择区域");
    $("#districtList").html("");
    $("#districtList").hide();
    $("#regionId").val(0);
    $("#regionText").html("请选择板块");
    $("#regionList").html("");
    $("#regionList").hide();
    
    
})
jQuery("#companytext").unbind("focus").bind("focus", function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    if ((this.value == "") || (this.value == "输入并选择所属公司")) {
        if (this.value == "输入并选择所属公司") {
            jQuery(this).val("")
        }
        var B = "";
        jQuery("#companyId").val(0);
        jQuery("#storeid").val("");
        B += '<dl class="match"><dt>请输入您所在的公司</dt></dl><ul class="searchlist  h173"></ul>';
        // jQuery("#companylist").html(B);
        //jQuery("#companylist").show()
    }
});
jQuery("#companytext").unbind("blur").bind("blur", function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    if (jQuery("#companyId").val() == 0) {
        jQuery("#errorCompany").html("请填写所属公司");
        jQuery("#errorCompany").show()
    }
    jQuery("#errorCompany").html("");
    jQuery("#errorCompany").hide()
});
jQuery("#companytext").unbind("keyup").bind("keyup", function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    jQuery("#companyId").val(0);
    jQuery("#companyStoreId").val(0);
    jQuery("#liCompanyStore").hide();
    jQuery("#companyStoreText").val("");
    jQuery("#companyStorelist").html("");
    jQuery("#companyStorelist").hide();
    if (this.value != "") {
        jQuery.getJSON("/User/GetUserCompanyByKey", { key: this.value, cityId: $("#cityId").val() }, function (C)
        {
            var E = "";
            var D = "";
            E += '<dl class="match"><dt>请点击匹配公司</dt></dl>';
            jQuery.each(C.data, function (G, F) {
                D += '<li><a companyid="' + F.CompanyId + '" href="javascript:void(0);">' + F.Name + "</a></li>"
            });
            E += '<ul class="searchlist h173">' + D + "</ul>";
            jQuery("#companylist").html(E);
            jQuery("#companylist").show()
        })
    }
});
jQuery("#companylist a").die("click").live("click", function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    var B = jQuery(this).attr("companyid");
    jQuery("#companyId").val(B);
    jQuery("#companytext").val(jQuery(this).text());
    jQuery("#companylist").hide();
    jQuery("#liCompanyStore").show();
    
});


jQuery("#companyStoreText").unbind("focus").bind("focus", function (event) {
    //取消事件冒泡  
    event.stopPropagation();  
    if ((this.value == "") || (this.value == "输入并选择所属分店")) {
        if (this.value == "输入并选择所属分店") {
            jQuery(this).val("")
        }
        var B = ""; jQuery("#companyStoreId").val(0);
        B += '<dl class="match"><dt>输入并选择所属分店</dt></dl><ul class="searchlist  h173"></ul>';
        //jQuery("#companyStorelist").html(B);
        //jQuery("#companyStorelist").show()
    }
});

jQuery("#companyStoreText").unbind("blur").bind("blur", function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    if (jQuery("#companyStoreId").val() == 0) {
        jQuery("#errorCompanyStore").html("请填写所属分店");
        jQuery("#errorCompanyStore").show()
    }
    jQuery("#errorCompanyStore").html("");
    jQuery("#errorCompanyStore").hide()
});

jQuery("#companyStoreText").unbind("keyup").bind("keyup", function (event) {
    event.stopPropagation();
    jQuery("#companyStoreId").val(0);
    if (this.value != "") {
        jQuery.getJSON("/User/GetUserCompanyByKey", { key: this.value, act: "companyStore", companyId: $("#companyId").val(),cityId: $("#cityId").val() }, function (C) {
    var E = "";
    var D = "";
    E += '<dl class="match"><dt>请点击所属分店</dt></dl>';
    if (C.data.length > 0) {
        jQuery.each(C.data, function (G, F) {

            D += '<li><a companyid="' + F.CompanyId + '" href="javascript:void(0);">' + F.Name + "</a></li>"
        });
    }
    else {
        D += '<li><a companyid="0" href="javascript:void(0);">其他公司</a></li>';
    }
    E += '<ul class="searchlist h173">' + D + "</ul>";
    jQuery("#companyStorelist").html(E);
    jQuery("#companyStorelist").show()
})
    }
});

jQuery("#companyStorelist a").die("click").live("click", function (event) {
    event.stopPropagation();
    var B = jQuery(this).attr("companyid");
    jQuery("#companyStoreId").val(B);
    jQuery("#companyStoreText").val(jQuery(this).text());
    jQuery("#companyStorelist").hide();
    if (B == 0) {
        $("#liWorkRange").show();
    }
    else {
        ///工作范围清掉

        $("#liWorkRange").hide();
        $("#districtId").val(0);
        $("#districtText").html("请选择区域");
        $("#districtList").html("");
        $("#districtList").hide();
        $("#regionId").val(0);
        $("#regionText").html("请选择板块");
        $("#regionList").html("");
        $("#regionList").hide();
    }
});

jQuery("#select_district").click(function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/User/GetRegionList',
        data: { cityId: $("#cityId").val(),districtId:0 },
        success: function (result) {
            var html = '<ul class="searchlist h129">';
            $.each(result.data, function (a, b) {
                html += '<li><a href="javascript:void(0)" regionId='+b.RegionID+'>'+b.Name+'</a></li>';
            });
            html += '</ul>';
            $("#districtList").html(html);
            $("#districtList").toggle();
        }
    });
    
});
jQuery("#districtList ul li a").live("click",function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    $("#districtText").html($(this).text());
    $("#districtId").val($(this).attr("regionId"));
    $("#districtList").hide();

    $("#regionId").val(0);
    $("#regionText").html("请选择板块");
    $("#regionList").html("");
    $("#regionList").hide();
});


jQuery("#select_region").click(function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    if (parseInt($("#districtId").val()) > 0) {
        $.ajax({
            type: 'get',
            dataType: 'json',
            url: '/User/GetRegionList',
            data: { cityId: $("#cityId").val(), districtId: $("#districtId").val() },
            success: function (result) {
                var html = '<ul class="searchlist h129">';
                $.each(result.data, function (a, b) {
                    html += '<li><a href="javascript:void(0)" regionId=' + b.RegionID + '>' + b.Name + '</a></li>';
                });
                html += '</ul>';
                $("#regionList").html(html);
                $("#regionList").toggle();
            }
        });
    }
});

jQuery("#regionList ul li a").live("click",function (event) {
    //取消事件冒泡  
    event.stopPropagation();
    $("#regionText").html($(this).text());
    $("#regionId").val($(this).attr("regionId"));
    $("#regionList").hide();
});


$(document).click(function (event) {
    $('#companylist').hide();
    $("#companyStorelist").hide();
    $("#citylist").hide();
    $("#districtList").hide();
    $("#regionList").hide();
});