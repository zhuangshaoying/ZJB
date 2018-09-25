var changeFlag = 0;
$(document).ready(function () {
    var showflag = "1";
    $("[name*='ide']").each(function (i) {
        $(this).click(function () {
            var a = $(this).attr("class") == null ? "" : $(this).attr("class");
            var b = $(this).attr("name").split(" ")[0];
            if (a.indexOf("active") < 0) {
                hideActive();
                $(this).attr("class", "active");
                hideChioce();
                $("." + b).show();
                if (b == 'allChioce') {
                    $("#allIn").show();
                }
                $("#sift").attr("class", "tabSX");
                $(".float").show();
                IScroll.refresh("#" + b);


                if (showflag == "1") {
                    if (true) {
                        $("#districtChioce").show();
                        IScroll.refresh("#districtChioce");
                    }
                    if (false) {
                        $("#districts_jimei").show();
                        IScroll.refresh("#districts_jimei");
                    }
                    if (false) {
                        $("#subwayChioce").show();
                        IScroll.refresh("#subwayChioce");
                    }
                    if (false) {
                        var ele = $("[name*='']").parent().parent();
                        var id = $(ele).attr("id");
                        $(ele).show();
                        IScroll.refresh("#" + id);
                    }
                    showflag = "0";
                }
            } else {
                $(this).attr("class", "");
                hideAllOut();
                hideChioce();
                window.location.href = "#sift";
                $(".float").hide();
            };
        });
    });
    $(".float").click(function () {
        $("[name*='ide']").attr("class", "");
        hideAllOut();
        hideChioce();
        $(".float").hide();
    });
    $(".btnBack").click(function () {
        hideAllOut();
        $("#allIn").show();
    });
    $("[name*='neIde']").each(function (i) {
        $(this).click(function () {
            $("section.column3").hide();
            $("section.column2").hide();
            $(this).addClass("active").siblings().removeClass("active");
            $(".stations").hide();
            $(".districts").hide();
            var a = $(this).attr("name").split(" ")[0];
            
            $("#" + a).show();
            a == "districtChioce" ? $("#subwayChioce").hide() : $("#districtChioce").hide();
            IScroll.refresh("#" + a);
        });
    });
    $("[name*='twIde']").each(function (i) {
        $(this).click(function () {
            var a = $(this).attr("name").split(" ")[0];
            $("#allIn").hide();
            hideAllOut();
            $("#" + a).show().prev().show();
            $(".btnBack").show();
            IScroll.refresh("#" + a);
        })
    });
    $("#allOut").find("dd").each(function (i) {
        var a = $(this).parent().parent().attr("id");
        var b = $(this).attr("class") == null ? "" : $(this).attr("class");
        if (b.indexOf("active") != -1) {
            $("[name*='" + a + "']").attr("value", $(this).attr("value"));
        }
        $(this).click(function () {
            if (a == "saleDateTw") {
                return false
            }
            else {
                $("[name*='" + a + "']").attr("value", $(this).attr("value")).find("span").text($(this).text());
                $(this).parent().find("dd.active").attr("class", "");
                $(this).attr("class", "active");
                hideAllOut();
                $("#allIn").show();
            }
        });
    });
    $("[name*='thIde']").each(function (i) {
        $(this).click(function () {
            var a = $(this).attr("name").split(" ")[0];
            $("#allIn").hide();
            hideAllOut();
            $("#ccRes").show();
            $("#" + a).show();
        });
    });


});

function hideChioce() {
    $("#sift").attr("class", "");
    var a = $(".lbTab").children();
    for (var i = 1; i < a.size() ; i++) {
        a.eq(i).hide();
    }
}
function hideActive() {
    var a = $("ul.flexbox").children();
    for (var i = 0; i < a.size() ; i++) {
        a.eq(i).attr("class", "");
    }
}
function cleanAllChioce() {
    $("#wapxfsy_D02_01").find("span").html("区域");
    $("#wapxfsy_D02_02").find("span").html("出售");
    $("#wapxfsy_D02_03").find("span").html("类别");
    changeFlag = 1;
    var a = $("span.rt");
    for (var i = 0; i < a.size() ; i++) {
        a.eq(i).text("不限");
    }
    $("#districtTh").attr("value", "");
    $("#subwayTh").attr("value", "");
    var b = $("dl.all").children();
    for (var i = 0; i < b.size() ; i++) {
        b.eq(i).attr("value", "");
    }
}
function hideAllOut() {
    var a = $("#allOut").children();
    for (var i = 0; i < a.size() ; i++) {
        a.eq(i).hide();
    }
}