//滚动到页面底部时，自动加载更多
window.addEventListener("scroll", function scrollHandler() {

    var scrollh = $(document).height();
    var bua = navigator.userAgent.toLowerCase();
    if (bua.indexOf('iphone') != -1 || bua.indexOf('ios') != -1) {
        scrollh = scrollh - 140;
    } else {
        scrollh = scrollh - 80;
    }
    var c = document.documentElement.clientHeight || document.body.clientHeight, t = $(document).scrollTop();
    if (k != false && ($(document).scrollTop() + w.height()) >= scrollh) {
        load();
    }
}, 100);