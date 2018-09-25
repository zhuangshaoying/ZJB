
var pageIndex = 1;
var PageSize = 10;
var canGet = true;
var wh = $(window);
onScrollEvent=function (callBack) {
    var a = document.documentElement.scrollTop || window.pageYOffset || document.body.scrollTop;
    var scrollh = $(document).height();
    var bua = navigator.userAgent.toLowerCase();
    if (bua.indexOf('iphone') != -1 || bua.indexOf('ios') != -1) {
        scrollh = scrollh - 140;
    } else {
        scrollh = scrollh - 80;
    }

    if (canGet != false && ($(document).scrollTop() + wh.height()) >= scrollh) {
        pageIndex++
        callBack(pageIndex);
    }
}
