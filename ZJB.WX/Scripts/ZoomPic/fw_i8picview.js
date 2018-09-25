function PicViewHandler() {

}
//define(function (require, exports) {
PicViewHandler.picviewInit = function (obj) {
    var $ = jQuery;
    $("body").addClass("bigPic_over");
    var $obj = $(obj); //.parent()
    var allimg = $obj.parents(".msg-attach-pics,.cattach-img-preview").find(".att-file-pic");
    var newbox = new popupDialog({ 'overlay': true, 'imgctrl': true });
    newbox.initBox();
    var container = newbox.container();
    var overlay = newbox.boxOverlay();
    var ctrllay = newbox.ctrlPanel();
    var bigpic = $obj.attr("src"); //.replace("/small/", "/"); //.find("img")
    if (bigpic.indexOf("?") > 0) {
        bigpic = bigpic.substring(0, bigpic.indexOf("?"));
    }
    var ImgData = {};
    ImgData.ImageMiddle = bigpic;
    var ImgIndex = $obj.attr("img-index");
    var load_tip = $('<span class="noattr-tips">努力加载中...</span>');
    container.append(load_tip);
    if (typeof ImgData == 'object') {
        imgReady(ImgData.ImageMiddle, function () {
            ctrllay.find(".ctlpanel-zoomout").hide();
            //            if (this.width < 680 && this.height < 510) {
            //                ctrllay.find(".ctlpanel-zoomout").hide();
            //            }
        }, function () {

        }, function () {

        });
        var firstImg = $('<img id="img_attachment_show" src="' + ImgData.ImageMiddle + '"/>').data(ImgData);
        container.empty().append(firstImg).data(ImgData);
    }
    ctrllay.siblings("a.vieworiginalfile").attr("href", ImgData.ImageMiddle).css({ 'display': 'inline-block' });
    cursorChange(overlay[0]);
    var img_attachment_show = $("#img_attachment_show");
    overlay.attr("show-index", ImgIndex).unbind("click").click(function () {
        var curIndex = parseInt($(this).attr("show-index"));
        if ($(this).hasClass("cursorRight")) {
            if (curIndex + 1 == allimg.length) {
                var span_tip = $('<span class="noattr-tips">已经是最后一张</span>');
                $(this).after(span_tip);
                setTimeout(function () { span_tip.remove(); }, 900);
            } else {
                $(this).attr("show-index", curIndex + 1);
                var curImg = allimg.get(curIndex + 1);
                var bigpic = curImg.src.replace("/small/", "/");
                bigpic = bigpic.substring(0, bigpic.indexOf("?"));
                var curData = {};
                curData.ImageMiddle = bigpic;
                ctrllay.siblings("a.vieworiginalfile").attr("href", curData.ImageMiddle).css({ 'display': 'inline-block' });
                //var curData = $.parseJSON(decodeURIComponent($(curImg).attr("img-data")));
                if (typeof curData == 'object') {
                    imgReady(curData.ImageMiddle, function () {
                        ctrllay.find(".ctlpanel-zoomout").hide();
                        //                        if (this.width < 680 && this.height < 510) {
                        //                            ctrllay.find(".ctlpanel-zoomout").hide();
                        //                        } else {
                        //                            ctrllay.find(".ctlpanel-zoomout").show();
                        //                        }
                    }, function () {
                        $("#img_attachment_show").replaceWith('<img id="img_attachment_show" src="' + curData.ImageMiddle + '"/>');
                        $("#img_attachment_show").attr("status", "normal").attr("degree", "0").data(curData);
                        $("#img_attachment_show").parents(".i8att-box").data(curData);
                    }, function () {

                    });

                }
            }
        } else if ($(this).hasClass("cursorLeft")) {
            if (curIndex - 1 < 0) {
                var span_tip = $('<span class="noattr-tips">已是第一张了</span>');
                $(this).after(span_tip);
                setTimeout(function () { span_tip.remove(); }, 900);
            } else {
                $(this).attr("show-index", curIndex - 1);
                var curImg = allimg.get(curIndex - 1);
                var bigpic = curImg.src.replace("/small/", "/");
                bigpic = bigpic.substring(0, bigpic.indexOf("?"));
                var curData = {};
                curData.ImageMiddle = bigpic;
                ctrllay.siblings("a.vieworiginalfile").attr("href", curData.ImageMiddle).css({ 'display': 'inline-block' });
                if (typeof curData == 'object') {
                    imgReady(curData.ImageMiddle, function () {
                        ctrllay.find(".ctlpanel-zoomout").hide();
                        //                        if (this.width < 680 && this.height < 510) {
                        //                            ctrllay.find(".ctlpanel-zoomout").hide();
                        //                        } else {
                        //                            ctrllay.find(".ctlpanel-zoomout").show();
                        //                        }
                    }, function () {
                        $("#img_attachment_show").replaceWith('<img id="img_attachment_show" src="' + curData.ImageMiddle + '"/>');
                        $("#img_attachment_show").attr("status", "normal").attr("degree", "0").data(curData);
                        $("#img_attachment_show").parents(".i8att-box").data(curData);
                    }, function () {

                    });

                }
            }
        }
    });
    $.getScript('/Scripts/ZoomPic/jquery-rotate-min.js', function () {
        $(".ctlpanel-rotate-left", ctrllay).click(function () {
            var oDegree = parseInt($("#img_attachment_show").attr("degree")) || 0;
            var newDegree = oDegree - 90;
            img_attachment_show.attr("degree", newDegree).rotate(newDegree);
        });
        $(".ctlpanel-rotate-right", ctrllay).click(function () {
            var oDegree = parseInt($("#img_attachment_show").attr("degree")) || 0;
            var newDegree = oDegree + 90;
            img_attachment_show.attr("degree", newDegree).rotate(newDegree);
        });
        $(".ctlpanel-zoomout", ctrllay).click(function () {
            var curImgData = $("#img_attachment_show").parents(".i8att-box").data();
            img_attachment_show.replaceWith('<img id="img_attachment_show" src="' + curImgData.ImageMiddle + '"/>');
            img_attachment_show.css({ 'position': 'relative' });
            overlay.hide();
            $(this).hide().siblings(".ctlpanel-zoomin").css({ 'display': 'inline-block' });
            //            $.getScript('/Js/ZoomPic/jquery-i8drag.min.js', function () {
            //                $("#img_attachment_show").draggable({ cursor: "move" });
            //            }); //ctrllay.siblings
            //            ctrllay.siblings("a.vieworiginalfile").attr("href", curImgData.ImageMiddle).css({ 'display': 'inline-block' });
        });
        $(".ctlpanel-zoomin", ctrllay).click(function () {
            var curImgData = img_attachment_show.parents(".i8att-box").data();
            img_attachment_show.replaceWith('<img id="img_attachment_show" src="' + curImgData.ImageMiddle + '"/>');
            img_attachment_show.removeAttr("style").addClass("ui-draggable");
            overlay.show();
            $(this).hide().siblings(".ctlpanel-zoomout").css({ 'display': 'inline-block' });
            $(".ctlpanel-zoomout", ctrllay).show();
            ctrllay.siblings("a.vieworiginalfile").css({ 'display': 'none' });
        });
    });
    //  });
}
function cursorChange(picLayer) {
    if ($.browser.msie) {
        picLayer.attachEvent("onmousemove", function (e) {
            if (e.offsetX < 340) {
                $(picLayer).removeClass("cursorRight").addClass("cursorLeft");
            } else {
                $(picLayer).removeClass("cursorLeft").addClass("cursorRight");
            }
        });
    } else {
        picLayer.addEventListener("mousemove", function (e) {
            if ((e.offsetX || e.layerX) < 340) {
                $(this).removeClass("cursorRight").addClass("cursorLeft");
            } else {
                $(this).removeClass("cursorLeft").addClass("cursorRight");
            }
        }, true);

    }
}
//};
var imgReady = (function () {
    var list = [], intervalId = null,
    // 用来执行队列
	tick = function () {
	    var i = 0;
	    for (; i < list.length; i++) {
	        list[i].end ? list.splice(i--, 1) : list[i]();
	    };
	    !list.length && stop();
	},
    // 停止所有定时器队列
	stop = function () {
	    clearInterval(intervalId);
	    intervalId = null;
	};

    return function (url, ready, load, error) {
        var onready, width, height, newWidth, newHeight,
			img = new Image();

        img.src = url;

        // 如果图片被缓存，则直接返回缓存数据
        if (img.complete) {
            ready.call(img);
            load && load.call(img);
            return;
        };
        width = img.width;
        height = img.height;
        // 加载错误后的事件
        img.onerror = function () {
            error && error.call(img);
            onready.end = true;
            img = img.onload = img.onerror = null;
        };
        // 图片尺寸就绪
        onready = function () {
            newWidth = img.width;
            newHeight = img.height;
            if (newWidth !== width || newHeight !== height ||
            // 如果图片已经在其他地方加载可使用面积检测
				newWidth * newHeight > 1024
			) {
                ready.call(img);
                onready.end = true;
            };
        };
        onready();

        // 完全加载完毕的事件
        img.onload = function () {
            // onload在定时器时间差范围内可能比onready快
            // 这里进行检查并保证onready优先执行
            !onready.end && onready();

            load && load.call(img);

            // IE gif动画会循环执行onload，置空onload即可
            img = img.onload = img.onerror = null;
        };

        // 加入队列中定期执行
        if (!onready.end) {
            list.push(onready);
            // 无论何时只允许出现一个定时器，减少浏览器性能损耗
            if (intervalId === null) intervalId = setInterval(tick, 40);
        };
    };
})();
//})