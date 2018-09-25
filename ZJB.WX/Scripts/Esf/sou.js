var url = website;
////用窗口变化来判断键盘是否弹出
//var h=document.body.clientHeight;
////android终端或者uc浏览器
//var u = navigator.userAgent, app = navigator.appVersion;
//var isAndroid = u.indexOf('Android') > -1 || u.indexOf('Linux') > -1;
//if(isAndroid){
//    //   用窗口变化来判断键盘是否弹出
//    window.onresize=function(){
//        var h1=document.body.clientHeight;
//        if(h1<h){
//            $('.formBox').css('top','0')
//        }
//        if(h1>=h){
//            $('.formBox').css('top','31%')
//        }
//    };
//}

/*if (navigator.geolocation && location.href.indexOf("result") < 0 && !isWeiXin()) {
 navigator.geolocation.getCurrentPosition(function (p) {
 var latitude = p.coords.latitude;//纬度
 var longitude = p.coords.longitude;//经度
 createmap(latitude, longitude);
 }, function (e) {//错误信息
 var aa = e.code + "\n" + e.message;
 //alert(aa);
 //alert('无法获取您当前位置');
 getCityId("北京");
 }
 );
 }
 function createmap(a, b) {
 //alert("latitude="+a+";longitude="+b);
 $.ajaxJSONP({
 url: "http://apis.map.qq.com/ws/geocoder/v1/?location=" + a + "," + b + "&key=HJCBZ-CVZR3-BKG37-3LJWA-TZFYF-RTFBU&get_poi=0&output=jsonp&callback=?",
 success: function (data) {
 //alert(JSON.stringify(data));
 var cityName = data.result.address_component.city;
 if (cityName == '北京市') {
 cityName = "北京";
 } else if (cityName == '上海市') {
 cityName = "上海";
 } else if (cityName == '广州市') {
 cityName = "广州";
 } else if (cityName == '深圳市') {
 cityName = "深圳";
 } else {
 cityName = "北京";
 }
 getCityId(cityName);
 $('#city').text(cityName);

 //console.log(data);
 //console.log(JSON.stringify(data));
 }
 });
 }*/
function getCityId(city_name) {
    var text = $("#" + city_name).text();
    $("#" + city_name).addClass('select');
    $("#" + city_name).siblings().removeClass('select');
    $('.city').text(text);
    $('.cityList').addClass('none');
    $('.drop-toggle').removeClass('up');
    $('.icon').removeClass('icon_t');
    //alert(222);
    $.post(url + "/home/search/getCityId", {udc_name: city_name}, function (data) {
        localStorage.setItem("city_id", data);
    });
    $('.tishi').html('&nbsp;');
    $('.tishi').removeClass("alert");
}
$('.btn-share').click(function () {
    $('.share').show(300)
});
$('.share').click(function () {
    $('.share').hide(600)
});

//<!-- 模拟下拉框-->
$('.city,.drop-toggle').on('click', function () {
    $('.cityList').toggleClass('none');
    $('.drop-toggle').toggleClass('up');
    $('.houseList').addClass('none');
});
var isajax = true;

$('#estate').bind("input propertychange", function () {
    var sv = $(this).val().trim();
    var city_id = localStorage.getItem("city_id");
    $('#estateVal').val("");
    if (isajax) {
        $.ajax({
            type: 'get',
            url:  "/Community/SearchByName",
            data: {city_id: city_id, name: sv},
            dataType: 'json',
            beforeSend: function () {
                isajax = false;
            },
            success: function (data) {
                //data = data.substring(1,data.length-1);
                //console.log(data)
                if (data.length > 0) {
                    var html = "";
                    for (var i = 0; i < data.length; i++) {
                        html += '<li onclick="setEstate(' + data[i].house_id + ')" id="ete' + data[i].house_id + '">' + data[i].house_name + '</li>';
                    }
                } else {
                    var html = '<div class="nothis">找不到这个小区</div>';
                }
                $('.houseList').html(html);
                $('.houseList').removeClass('none');
                isajax = true;
            }
        });
        $('.tishi').html('&nbsp;');
        $('.tishi').removeClass("alert");
        //$('.nothis').show();
        //$('.houseList li').hide().each(function(){
        //    if($(this).text().indexOf(sv) > -1){
        //        $(this).show();
        //        $('.nothis').hide();
        //
        //    }
        //});
    }
});
$("#priceVal").bind("input propertychange", function () {
    $('.tishi').html('&nbsp;');
    $('.tishi').removeClass("alert");
});
//$('body').click(function(e){
//    if(e.target.className != 'city' || e.target.className != 'drop-toggle')
//    {
//        $('.List').addClass('none');
//        $('.drop-toggle').removeClass('up');
//    }
//});
function setEstate(i) {
    console.log(i);
    $('#estate').val($('#ete' + i).text());
    $('#estateVal').val(i);
    $('.houseList').addClass('none');
    localStorage.setItem('house_name', $('#ete' + i).text());
    localStorage.setItem('house_id', i);
    $('.tishi').html('&nbsp;');
    $('.tishi').removeClass("alert");
}


//--买家卖家按钮
$('#buyer,#seller').click(function () {
    if ($('#estateVal').val().trim() == '') {
        $('.tishi').text('请输入小区名/从列表中选择小区');
        $('.tishi').addClass("alert");
        return false;
    } else {
        var city_id = localStorage.getItem("city_id");
        var house_id = localStorage.getItem("house_id");
        location.href = "/Community/Detail?city_id=" + city_id + "&communityId=" + house_id;
    }
});