function code() {
    $.ajax({
        type: "post",
        url: "/Esf/IdentifyingCode",
        data: "tel=" + $("#txt_tel").val(),
        dataType: "json",
        success: function (data) {

        }
    });
}
var goback = function () {
    history.length >= 2 ? history.go(-1) : window.location = 'http://www.zhujia001.cn/esf/list';
}
function createDiv(txt) {
    var html = "";
    html += "<div class=\"mod-smallnote smallnote-mobile scale\"><div class=\"smallnote-text\">" + txt + "</div></div>";
    return html;
}