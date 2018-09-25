var uploadFileHandler; //上传文件控件
var curLoignUserId = 0;
var curCircleId = 0;
jQuery(document).ready(function () {
    var groupId = 1;
    var kind = "dt";
    curLoignUserId = jQuery("#hdUserId").val();
    var lybHandler = new LybHandler({ Kind: kind, ShareTextArea: jQuery("#shareTextArea"), IsShowGroup: false, RefreshBtn: jQuery(".btn-refresh-list") });
    lybHandler.groupId = groupId;
   // var right = new RightHandler();
    lybHandler.GetList();
    jQuery(window).bind("scroll", function () {
        var totalheight = parseFloat(jQuery(window).height()) + parseFloat(jQuery(window).scrollTop()) + 80;
        if (jQuery(document).height() <= totalheight) {
            var ynLoad = lybHandler.GetScrollState();
            //加载数据
            if (ynLoad == "") {
                lybHandler.SetScrollState("1");
                lybHandler.GetList();
            }
        }
    });
});