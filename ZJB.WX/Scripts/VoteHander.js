

var txtTimer; //定时器,计算字符用
var dialog;
var votesid = 0;
var voteuid = 0; //当前登录用户ID;
var pubIsMobile = false;
var voteuname = 0;
var isN = false;
var isPicVote = false;
var endtime = "";
var  defaultendtime = "";
var optionCount = 1;
var dcidArr = new Array();
var ListRefDc = new Array();
var txtitemType = "";
jQuery(function () {
    voteuid = jQuery("#hdUserId").val(); //当前登录用户ID;
    voteuname = jQuery("#txtUserName").val();
    var ismobile = document.getElementById("mVoteBox");
    if (ismobile) {
        pubIsMobile = true;
    }
    else {
        pubIsMobile = false;
        endtime = jQuery("#txtEndTime").val();
        defaultendtime = jQuery("#txtEndTime").val();
      
    }

});
var UserextVote = {
    Trim: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "").replace(/(^　*)|(　*$)/g, "");
    },
    SubmitVote: function (jixu) {
        var pv = "";
        var isN = false;
        var c = 1;
        pv = "t1=" + escape(jQuery("#inputtitle").val()) + "&";
        var i = 0;//选项填写的合格数量
        var isMobile = document.getElementById("mVoteBox");
        var voteAddParame = { SurveyName: "", EndTime: "", SurveyType: 1, ViewData: 0, OptionNameList: "" };
        var optionList = [];
        if (isMobile) {
            jQuery("#tpdiv .input_detail").each(function (a, b) {
                if (jQuery(this).val().length == 0) {
                    i += 1;
                }
                var tbox = UserextVote.Trim(jQuery(b).val());
                if (a < 2) {
                    if (tbox == "") {
                        isN = true;
                    }
                }
                if (a >= 0) {
                    var optionIndex = parseInt(a) + 1;
                    if (jQuery(b).val() != "" && jQuery(b).val() != "选项" + optionIndex) {
                        pv += "x" + c + "=" + escape(tbox) + "&";
                        c++;
                    }
                }
            });
        }
        else {
            jQuery("#tpdiv .W_input").each(function (a, b) {
                var optionIndex = parseInt(a) + 1;
                if (jQuery(b).val() != ""&& jQuery(b).val()!= "选项" + optionIndex) {
                    i++;
                }
                var tbox = UserextVote.Trim(jQuery(b).val());
                if (a < 2) {
                    if (tbox == "") {
                        isN = true;
                    }
                }
                if (a >= 0) {
                    if (jQuery(b).val() != "" && jQuery(b).val() != "选项" + optionIndex) {
                        optionList.push("{OptionName:'" + tbox + "'}");
                        c++;
                    }
                }
            });
            if (jQuery("#inputtitle").val().length == 0 &&i<2) {
                jQuery("#titleZsCount").css("display", "none");
                jQuery("#optionZsCount").css("display", "none");
                jQuery("#titleErrorTips").css("display", "inline-block");
                jQuery("#optionstip").css("display", "inline-block");
                jQuery("#inputtitle").focus();
                error("inputtitle", 3);
                if (isPicVote) {
                    error("optionstipPic", 3);
                }
                else {
                    error("optionstip", 3);
                }
                return false;
            }
            if (jQuery("#inputtitle").val().length == 0) {
                jQuery("#titleZsCount").css("display", "none");
                jQuery("#titleErrorTips").css("display", "inline-block");
                jQuery("#inputtitle").css("background-color", "#f7d6d6");
                return false;
            }
            if (i<2) {
                jQuery("#optionZsCount").css("display", "none");
                if (isPicVote) {
                    jQuery("#optionstipPic").css("display", "inline-block");
                }
                else {
                    jQuery("#optionstip").css("display", "inline-block");
                }
                return false;
            }
        }
        if (!isN) {
            var itemtype = jQuery("#Itemtype").attr("value");
            if (itemtype == "0") {
                itemtype = 1;
            }
            var picpath = "";
            var endtime = "";
            var viewdata = 0;
            if (isMobile) {
                picpath = jQuery("#showMPicPath").find("img").attr("src"); if (!picpath) { picpath = ""; }
                endtime = jQuery("#dcend_Year").val() + "-" + jQuery("#dcend_Month").val() + "-" + jQuery("#dcend_Day").val() + " " + jQuery("#dcend_Hour").val() + ":00";
                viewdata = jQuery("#selectViewdata").val();
            }
            else {
                endtime = jQuery("#txtEndTime").val();
                viewdata = jQuery("#selectViewdata .i8ctrl-radio").find("span.rdchecked").attr("vType");
            }
            voteAddParame.SurveyType = itemtype;
            voteAddParame.SurveyName = jQuery("#inputtitle").val();
            voteAddParame.EndTime = endtime;
            voteAddParame.ViewData = viewdata;
            voteAddParame.OptionNameList = "[" + optionList.join(',') + "]";
            jQuery.post("/Social/VoteAdd", voteAddParame, function (result) {
                if (result.status > 0) {
                    art.dialog.tips("发布成功！", 1.5, "success.gif");
                    InitvoteBox();
                    var lybHandler = new LybHandler({ Kind: "dt", ShareTextArea: jQuery("#shareTextArea"), IsShowGroup: false, RefreshBtn: jQuery(".btn-refresh-list") });
                    lybHandler.InitValue();
                    lybHandler.GetList();
                }
            });
        }
    }, //End
    AddItem: function () {

        var i = jQuery("#tpdiv dd").length;

        if (i < 10) {
            i++;
            jQuery("#tpdiv").append('<dd><div class="optionpicview"></div><input class="W_input" type="text" value="选项' + i + '"  onfocus="optionFoucs(this,' + i + ')"   onblur="optionBlur(this,' + i + ')"/></dd>');
            jQuery("#tpdiv dd a").addClass("W_ico12");
        } else {
            alert("最多只能添加10个选项");
        }
    }, //AddItem end
    AddMItem: function () {
        var i = jQuery("#tpdiv .ddss").length;

        if (i < 10) {
            i++;
            jQuery("#tpdiv").append('<div class="ddss"><textarea class="input_detail sstt_1 "  style="resize: none;height: 21px;" placeholder="选项' + i + '"></textarea></div>');
            var tempLast = jQuery("#tpdiv div.ddss:last-child textarea");
            jQuery("#tpdiv div.ddss:last-child textarea").val("");
            var liArr = jQuery("#tpdiv div.ddss");
            if (liArr.length < 10) {
                var tempLast = jQuery("#tpdiv div.ddss:last-child textarea");
                autoTextarea(tempLast[0]);
            }
        } else {
            alert("最多只能添加10个选项");
        }
    },
    DelItem: function (e) {
        var chs = jQuery('#tpdiv dd'),
             par = e.parent();

        if (chs.length < 3) {
            //alert("请至少保留两个选项");
            jQuery(".optiontips").css("display", "block");

            return false;
        }
        par.remove();
        var len = jQuery('#tpdiv dd').length;
        for (var i = 0; i < len; i++) {
            //jQuery("#tpdiv").append('<dd><span class="num">' + (i + 1) + '.</span><input class="W_input" name="vchoice"' + (i + 1) + ' type="text"><a class="del W_ico12 icon_close" href="javascript:;" onclick="return false;"></a></dd>');
            jQuery('#tpdiv dd').eq(i).children(".num").html(i + 1 + ".");
        }

        if (jQuery("#tpdiv .W_input").length <= 2) {
            jQuery("#tpdiv .W_input").siblings("a").removeClass("W_ico12");
        }

    }, //DelItem end  
    mlen: function (str) {
        var len = 0;
        for (var i = 0; i < str.length; i++) {
            if (str.charCodeAt(i) > 255) len += 2;
            else len++;
        }
        return len;
    },
    msub: function (str, slen) {
        var tmp = 0;
        var len = 0;
        var okLen = 0;
        for (var i = 0; i < slen; i++) {
            if (str.charCodeAt(i) > 255) {
                tmp += 2;
            } else {
                len += 1;
            }
            okLen += 1;
            if (tmp + len == slen) {
                return (str.substring(0, okLen));
            }
            if (tmp + len > slen) {
                return (str.substring(0, okLen - 1));
            }
        }
    },
    CheckLength: function (tar, len, t, ot) {
        tar.keyup(function () {
            if (UserextVote.mlen(tar.val()) > len) {
                tar.val(UserextVote.msub(tar.val(), len));
                alert("标题太长");
            }
        });
        tar.blur(function () {
            if (UserextVote.mlen(tar.val()) > len) {
                tar.val(UserextVote.msub(tar.val(), len));
                alert("标题太长");
            }
        });
    }
};


function normal(id, times) {
    var obj = jQuery("#" + id);
    obj.css("background-color", "#FFF");
    if (times < 0) {
        return;
    }
    times = times - 1;
    setTimeout("error('" + id + "'," + times + ")", 150);
};
function error(id, times) {
    var obj = jQuery("#" + id);
    obj.css("background-color", "#F6CECE");
    times = times - 1;
    setTimeout("normal('" + id + "'," + times + ")", 150);
};


function InitvoteBox() {
        jQuery("#tpdiv").html("");
        UserextVote.AddItem();
        UserextVote.AddItem();
};
function initDcTime() {
    SelectDCEndYearOrMonthDays();
    var strhtml = GetHoursByDayDC();
    jQuery("#dcend_Hour").html(strhtml);
};

jQuery(".vote_tabStyle3 li").live("click", function () {
    var index = jQuery(this).index();
    if (index == 1) {
        isPicVote = true;
    }
    else {
        isPicVote = false;
    }
});
jQuery("#submit").live("click", function () {
    UserextVote.SubmitVote(true);
});
jQuery("#voteadd1").live("click", function () {
    UserextVote.AddItem();
});
jQuery("#tpdiv .del").live("click", function () {
    UserextVote.DelItem(jQuery(this));
});
jQuery("#inputtitle").live("keyup", function () {
    UserextVote.CheckLength(jQuery(this), 80, 'tips');
    jQuery("#titleZsCount").css("display", "inline-block");
    jQuery("#titleErrorTips").css("display", "none");
    if (jQuery("#inputtitle").val() != "") {
        $("#submit").addClass("send_btn").removeClass("disable");
    }
    else {
        $("#submit").addClass("disable").removeClass("send_btn");
    }
    if (!pubIsMobile) {
        //jQuery("#inputtitle").css("width", "643px");
    }

});
jQuery("#tpdiv .W_input").live("keyup", function () {
    UserextVote.CheckLength(jQuery(this), 80, 'tips');
    jQuery("#optionstip").css("display", "none");
    jQuery("#optionZsCount").css("display", "inline-block");

});

jQuery(".W_close").live("click", function () {

    jQuery(".W_layer").css("display", "none");
    jQuery("#inputtitle").val("");
    jQuery("#tpdiv .W_input").val("");
    jQuery("#optionZsCount").css("display", "none");
    jQuery("#optionstip").css("display", "none");
    jQuery("#optionstipPic").css("display", "none");
    jQuery("#titleZsCount").css("display", "none");
    jQuery("#titleErrorTips").css("display", "none");
});
jQuery(".UserAddSurvey").live("click", function () {
    jQuery(".W_layer").css("display", "block");
    InitvoteBox();
});

jQuery(".retract").live("click", function () {
    //var dcid = jQuery(this).attr("bin");
    jQuery(this).parents(".showVoteResult").css("display", "none");
    jQuery(this).parents(".showVoteResult").prevAll(".getVoteResult").css("display", "block");
});
//投票说明
jQuery("#new_vote_desc_btn").live("click", function () {
    jQuery(".vote_textarea_holder").toggle();
    jQuery(".vote_textarea").val("");
});
//标题传图
jQuery("#target_vote_uploadify").live("click", function () {
    jQuery("#vote_uploadify").click();
});

jQuery(".voteDeletePreview a").live("click", function () {
    jQuery(".vote_uploadrprview").html("");
    jQuery("#votehdImageUrl").val("");
});
jQuery(".vote_uploadrprview img").live("mouseover", function () {
    jQuery(".voteDeletePreview").removeClass("votedeletehide").addClass("votedeleteshow");
});
jQuery(".vote_uploadrprview img").live("mouseout", function () {
    setTimeout(function () { jQuery(".voteDeletePreview").removeClass("votedeleteshow").addClass("votedeletehide"); }, 2000);
});
jQuery("#inputtitle").live("mouseover", function () {
    jQuery(".vote_item_img_op").removeClass("votedeletehide").addClass("votedeleteshow");
    if (!pubIsMobile) {
        //jQuery("#inputtitle").css("width", "643px");
    }
});
jQuery("#inputtitle").live("mouseout", function () {
    setTimeout(function () { jQuery(".vote_item_img_op").removeClass("votedeleteshow").addClass("votedeletehide"); }, 2000);
    if (!pubIsMobile) {
       // jQuery("#inputtitle").css("width", "643px");
    }
});
//标题传图end
//**********正常投票 开始
function NormalSubmit(e) {
    var thisobj = jQuery(e);
    if (thisobj.parent("a").hasClass("W_btn_vote")) {
        thisobj.parent(".W_btn_vote").removeClass("W_btn_vote").addClass("W_btn_a_disable");
        var sid = jQuery(e).attr("bin");
        var checkarr = [];
        var nid = jQuery(e).attr("nid");
        var optionName = "";
        var optionList = [];
        var parame = { VoteId: sid ,options:"",dynamicContent:""};
        
        thisobj.parents(".btn").prevAll(".vote_text_list").each(function (a, b) {
            var obj = jQuery(b).find('input:checked');
            for (var i = 0; i < obj.length; i++) {
                optionList.push("{OptionId:'" + obj.eq(i).val() + "'}");
                var nametext = obj.eq(i).attr("optionName");
                optionName += nametext + ";";
            }
        });

        var surveyname = thisobj.parents(".W_btn_vote").attr("votetitle");
 
        var terminal = 0;
        if (browser.versions.iPhone || browser.versions.ios || browser.versions.android || browser.versions.iPad) {
            terminal = 1;
        }
        var sourceurl = location.href;
        parame.options = "[" + optionList.join(',') + "]";
        var txtContent = "我投了:" + optionName + "你呢?";
        parame.dynamicContent = txtContent;
        jQuery.post("/Social/SubmitVote", parame, function (data) {
            if (data.status > 0) {
                var ran = parseInt(1000000 * Math.random());
                getDcData(ListRefDc, dcidArr);
                var me = new ReplyHandler();
                me.GetReplyList(nid);
                ///发表评论end
            }
            if (data.status == -100) {
                alert("你已经投票过，已经不能在投票了");
                jQuery(".inputSel").attr("checked", false);
                return false;
            }
            if (data.status == -200) {
                alert("投票失败，因为投票已结束");
                jQuery(".inputSel").attr("checked", false);

                return false;
            }
        });
    }
};



//**********正常投票 结束

///************改变投票 开始
function ChangeVoteSubmit(e) {
    var thisobj = jQuery(e);
    if (thisobj.parent("a").hasClass("W_btn_d")) {
        thisobj.parent(".W_btn_d").removeClass("W_btn_d").addClass("W_btn_a_disable");
        var sid = thisobj.attr("bin");
        var checkarr = [];
        var nid = thisobj.attr("nid");
        var optionName = "";
        var optionList = [];
        var parame = { VoteId: sid, options: "", dynamicContent: "" };
     
        thisobj.parents(".btn").prevAll(".vote_text_list").each(function (a, b) {
            var obj = jQuery(b).find('input:checked');
            for (var i = 0; i < obj.length; i++) {
                optionList.push("{OptionId:'" + obj.eq(i).val() + "'}");
                var nametext = obj.eq(i).attr("optionName");
                optionName += nametext + ";";
            }
        });

        var surveyname = thisobj.parents(".W_btn_vote").attr("votetitle");
        
        var terminal = 0;
        if (browser.versions.iPhone || browser.versions.ios || browser.versions.android || browser.versions.iPad) {
            terminal = 1;
        }
        var sourceurl = location.href;
        parame.options = "[" + optionList.join(',') + "]";
        var txtContent = "我重新投了:" + optionName + "你呢?";
        parame.dynamicContent = txtContent;
        parame.isChange = 1;
        jQuery.post("/Social/SubmitVote", parame, function (data) {

            if (data.status > 0) {
                var ran = parseInt(1000000 * Math.random());
                getDcData(ListRefDc, dcidArr);
                var me = new ReplyHandler();
                me.GetReplyList(nid);
                ///发表评论end
            }
            if (data.status == -100) {
                alert("你已经投票过，已经不能在投票了");
                jQuery(".inputSel").attr("checked", false);
                return false;
            }
            if (data.status == -200) {
                alert("投票失败，因为投票已结束");
                jQuery(".inputSel").attr("checked", false);

                return false;
            }
        });
    }
};
///************改变投票 结束





var optionFoucs = function (obj,i) {
    if (jQuery(obj).val() =="选项"+i) {
        jQuery(obj).val("");
    }

};
var optionBlur = function (obj, i) {
    if (jQuery(obj).val() == "" || jQuery(".W_input").val() == null) {
        jQuery(obj).val("选项"+i);
    }
};

jQuery("#tpdiv .W_input:last").live("click", function () {
        UserextVote.AddItem();
    });
 jQuery("#tpdiv .ddss:last").live("click", function () {
        UserextVote.AddMItem();
    });
jQuery("#Itemtype").live("click", function () {
    jQuery("#selItemtypevalue").show();
});
jQuery("#selectItemtype").live("click", function () {
    jQuery("#selItemtypevalue").show();
});

jQuery("#selItemtypevalue em").live("click", function () {
    
    jQuery("#Itemtype").attr("value", jQuery(this).attr("value"));
    jQuery("#Itemtype").html(jQuery(this).attr("itemname"));
    jQuery("#selItemtypevalue").hide();

});
function DcoptionClick(e) {
    var parentobj = jQuery(e).parents(".vote_text_list");
    var count = jQuery(e).parents(".vote_text_list").length; //题目数
    var len = parentobj.find(".inputSel[type=radio]:checked").length; //单选题数
    var _cnum = jQuery(e).parents(".vote_text_list").find('input[type=checkbox]:checked').length;
    if (_cnum > 0) {
        jQuery(e).parents(".vote_text_list").addClass("HaveChecked");
    }
    else {
        jQuery(e).parents(".vote_text_list").removeClass("HaveChecked");

    }
    //var len2 = parentobj.find(".HaveChecked").length; //多选题数
    if ((len + _cnum) >= count) {
        jQuery(e).parents(".normalClass").nextAll(".btn").find(".btnVote").parent("a").removeClass("W_btn_a_disable").addClass("W_btn_vote");
        jQuery(e).parents(".changeClass").nextAll(".btn").find(".btnVote").parent("a").removeClass("W_btn_a_disable").addClass("W_btn_d");
    }
    else {
        jQuery(e).parents(".normalClass").nextAll(".btn").find(".btnVote").parent("a").removeClass("W_btn_vote").addClass("W_btn_a_disable");
        jQuery(e).parents(".changeClass").nextAll(".btn").find(".btnVote").parent("a").removeClass("W_btn_d").addClass("W_btn_a_disable");
    }
    controlOptionByOption(jQuery(e));
}


function controlOptionByOption(optionobj) {
    var ids = optionobj.attr("controloptions");
    if (optionobj.attr("checked")) {
        if (ids != null && ids != undefined && ids != "") {
            var len = ids.split(',').length;
            for (var i = 0; i < len; i++) {

                jQuery("#control_" + ids.split(',')[i]).siblings("p").hide();
                jQuery("#control_" + ids.split(',')[i]).siblings("p").children(".hdTxt").attr("status", "false");
                jQuery("#control_" + ids.split(',')[i]).show();
                jQuery("#control_" + ids.split(',')[i]).children(".hdTxt").attr("status", "true");
            }
        } else {
            optionobj.siblings("p").hide();
            optionobj.siblings("p").children(".hdTxt").attr("status", "false");
        }
    } else {
        if (ids != null && ids != undefined && ids != "") {
            var len = ids.split(',').length;
            for (var j = 0; j < len; j++) {
                jQuery("#control_" + ids.split(',')[j]).hide();
                jQuery("#control_" + ids.split(',')[j]).children(".hdTxt").attr("status", "false");
            }
        } 
    }
}
///可见范围
function ShowPublishGroup_DC() {
    jQuery("#rbqzqd").attr("onclick", "SelectGroupDC();");
    jQuery("#lbqzqx").attr("onclick", "SelectGroupDC();");
    HideAllDivPage('divGroupListSel');
    return false;
};
function SelectGroupDC() {
    var xzItem = jQuery("#divGroupList .com_center_public_selected_option");
    if (xzItem.length == 0) {
        onclickchecked_DCEx(jQuery("#divGroupList").find("[gid='all']")[0]);
    }
    jQuery("#spangroup_dc").html(jQuery("#spangroup_rw").html());
    HideAllDivPage('mVoteBox');
};
//获得结束时间
function SelectDCEndYearOrMonthDays() {
    var myDate = new Date();
    var curyear = myDate.getFullYear(); //获取完整的年份(4位,1970-????)
    var curmonth = myDate.getMonth() + 1; //获取当前月份(0-11,0代表1月)
    var day = myDate.getDate() + 7; //获取当前日(1-31)
    // myDate.getDay(); //获取当前星期X(0-6,0代表星期天)
    var h = myDate.getHours(); //获取当前小时数(0-23)
    var year = jQuery("#dcend_Year").val(); //.find("option[selected]");
    var month = jQuery("#dcend_Month").val();
    var days;
    if (year == "" || month == "") { days = getNumberDaysOfMonth(curyear, curmonth); }
    else { days = getNumberDaysOfMonth(year, month); }
    while (days < day) {
        curmonth += 1; if (curmonth > 12) { curmonth = 1; } day = day - days; if (year == "" || month == "") { days = getNumberDaysOfMonth(curyear, curmonth); }
        else { days = getNumberDaysOfMonth(year, month); } 
    }
    if (year == "") {
        jQuery("#dcend_Year").val(curyear);
        jQuery("#dcend_Month").val(curmonth);
        year = curyear;
    }
    if (month == "") {
        jQuery("#dcend_Year").val(curyear);
        jQuery("#dcend_Month").val(curmonth);
        month = curmonth;
    }
    jQuery("#dcend_Day").html("");
    var strhtml = "";
    strhtml = "<option value=\"\" selected=\"\">日</option>";
    for (var i = 1; i <= days; i++) {
        if (i == day) {
            strhtml += "<option value=\"" + i + "\" selected=\"\">" + i + "日</option>";
        }
        else {
            strhtml += "<option value=\"" + i + "\">" + i + "日</option>";
        }
    }
    jQuery("#dcend_Day").html(strhtml);
}


//上传图片
jQuery("#mDcUppic").click(function () {
    //jQuery("#imgType").val(11); //表示调查上传图片
    jQuery(jQuery("#imgupload")[0].contentWindow.document).find("#filepic").click();
});

var browser = {
    versions: function () {
        var u = navigator.userAgent, app = navigator.appVersion;
        return {//移动终端浏览器版本信息                                 
            trident: u.indexOf('Trident') > -1, //IE内核                                 
            presto: u.indexOf('Presto') > -1, //opera内核                                 
            webKit: u.indexOf('AppleWebKit') > -1, //苹果、谷歌内核                                 
            gecko: u.indexOf('Gecko') > -1 && u.indexOf('KHTML') == -1, //火狐内核                                
            mobile: !!u.match(/AppleWebKit.*Mobile.*/)
					|| !!u.match(/AppleWebKit/), //是否为移动终端                                 
            ios: !!u.match(/\(i[^;]+;( U;)? CPU.+Mac OS X/), //ios终端                 
            android: u.indexOf('Android') > -1 || u.indexOf('Linux') > -1, //android终端或者uc浏览器                                 
            iPhone: u.indexOf('iPhone') > -1 || u.indexOf('Mac') > -1, //是否为iPhone或者QQHD浏览器                    
            iPad: u.indexOf('iPad') > -1, //是否iPad       
            webApp: u.indexOf('Safari') == -1, //是否web应该程序，没有头部与底部
            google: u.indexOf('Chrome') > -1
        };
    } ()
};
function isSecurity(v) {
    var lv = -1;
    if (v.match(/[a-z]/ig)) { lv++; }
    if (v.match(/[0-9]/ig)) { lv++; }
    if (v.match(/(.[^a-z0-9])/ig)) { lv++; }
    if (v.length < 6 && lv > 0) { lv--; }
    return lv;
};
///改变投票显示
function WangToChange(sid) {
    jQuery("#voteNormal" + sid).toggle();
    jQuery("#voteChange" + sid).toggle();
};

///投票详情控制
jQuery("#selectViewdata .i8ctrl-radio").live("click", function () {
    jQuery("#selectViewdata .i8ctrl-radio").find("span.btn-radio").removeClass("rdchecked");
    jQuery(this).find("span.btn-radio").addClass("rdchecked");
});



///html
function getDcData(RefDcList, dcidList) {
    ListRefDc = RefDcList;
    dcidArr = dcidList;
    if (dcidArr.length > 0) {
        jQuery.ajax({
            type: 'post',
            dataType: 'json',
            url: '/Social/GetVoteResult',
            data: { ids: dcidArr.toString()},
            success: function (data) {
                for (var i = 0; i < data.length; i++) {
                    var s = SetSurveyNameHtml(data[i]);
                    jQuery("#showVoteResult" + data[i].VoteId).html(s);
                    var lineList = jQuery("#showVoteResult" + data[i].VoteId).find(".line");
                    var itemtypeobj = jQuery("#showVoteResult" + data[i].VoteId).find("span.spanitemtype").html(txtitemType);
                    var lenOption = lineList.length;
                    for (var j = 0; j < lenOption; j++) {
                        var sildeObj = lineList.eq(j).find("i");
                        var percent = sildeObj.attr("maxpercent");
                        if (parseInt(percent) < 1) { percent = 1; }
                        sildeObj.animate({ width: percent + "px" }, 1000);
                    }
                }
            }
        });
    }
}
function SetSurveyNameHtml(n) {
    var reval = "";
    reval += "<div class=\"mmvote\">";
    reval += "<div class=\"S_txt2 infos\"><s></s>";
    if (n.isGuoqi == 0) {
        reval += "<span node-type=\"timeLeft\">还有<span node-type=\"time\">" + n.countdown + "结束</span></span>";
    }
    else {
        reval += "<span node-type=\"timeLeft\">结束</span>";
    }
    reval += "<span class=\"vline\">|</span> <span class=\"spanvotenum\">" + n.VoteNum + "</span>人参与<em class=\"W_vline\">|</em>";
    reval += "<span class=\"spanitemtype\"></span>";
    reval += "</div>";

    reval += SetItemTableHtml(n);
    
    reval += "</div>";
    reval += "</div>";
    return reval;
}
function SetItemTableHtml(s) {
    var oneRefInfo = GetOneRefByDcid(ListRefDc, s.VoteId);
    var reval = "";
    //*****************加载图片start
    if (s.PicPath != null && s.PicPath != "" && s.PicPath != "undefined") {
        reval += "  <div class=\"vote_pic_info\" style=\"display: block; \"><div class=\"vote_big_pic\" id=\"lybLPic" + oneRefInfo.nid + "\"><img id=\"imgL" + oneRefInfo.nid + "\" onclick=\"javascript:if(typeof OnClickFDPicHD!='undefined'){OnClickFDPicHD('" + oneRefInfo.nid + "',1)}else{ShowImageByDiv('" + s.PicPath.replace("small", "") + "',this);}\"  src=\"" + s.PicPath + "\" alt=\"\" style=\"cursor:url('/Images/weipanImg/big.cur'),pointer;max-width:150px; max-height:150px;\"></div></div>";
    }
    //*******************加载图片end
    var normalHtml = "";
    var changeHtml = "";
    //********************不改变投票的div End
    normalHtml += "<div class=\"vote_text_list normalClass\" >";
    changeHtml += "<div class=\"vote_text_list changeClass\" >";
    if (s.SurveyType == 1) { txtitemType = "单选"; }
    else if (s.SurveyType == 2) { txtitemType = "多选"; }

    optionCount = 1;
    for (var l = 0; l < s.OptionList.length; l++) {
        var rejson = SetOptionNameHtml(s, s.OptionList[l]);
            normalHtml += rejson.normal;
            changeHtml += rejson.change;
            optionCount++;
        }
        normalHtml += "</div>";
        changeHtml += "</div>";
        if (s.isGuoqi == 1) {
            normalHtml += "<a class=\"W_btn_a_disable\"  href=\"javascript:void(0)\"><span>已到期</span></a>";
        }
        else if (s.isSubmit == 1) {
            normalHtml += "<a class=\"W_btn_a_disable\"  href=\"javascript:void(0)\"><span>已投票</span></a><a class=\"VoteChangeToggle\" onclick=\"WangToChange(" + s.VoteId + ")\"  href=\"javascript:void(0)\"><span>更改投票</span></a>";
            changeHtml += "<div class=\"btn\"> <a href=\"javascript:void(0);\" class=\"W_btn_a_disable\"  votetitle='" + s.SurveyName + "'><span class=\"btnVote\"  bin=\"" + s.VoteId + "\"  nid=\"" + oneRefInfo.nid + "\"   onclick=\"ChangeVoteSubmit(this)\">投票</span></a><a class=\"VoteChangeToggle\" onclick=\"WangToChange(" + s.VoteId + ")\"  href=\"javascript:void(0)\"><span>取消</span></a>";
        }
        else {
            normalHtml += "<div class=\"btn\"> <a href=\"javascript:void(0);\" class=\"W_btn_a_disable\"  votetitle='" + s.SurveyName + "' ><span class=\"btnVote\"  bin=\"" + s.VoteId + "\"  nid=\"" + oneRefInfo.nid + "\"  onclick=\"NormalSubmit(this)\" >投票</span></a>";
        }

        //********************不改变投票的div End
    var normalStrBuffer = "";
    var changeStrBuffer = "";
    normalStrBuffer = "<div id=\"voteNormal" + s.VoteId + "\" class=\"alltpList\">" + normalHtml + "</div>";
    changeStrBuffer = "<div id=\"voteChange" + s.VoteId + "\" class=\"alltpList\" style=\"display:none;\">" + changeHtml + "</div>";
    reval += normalStrBuffer;
    reval += changeStrBuffer;
    reval += "<ul class=\"result_list\">";
    reval += "</ul>";
    return reval;
}
function SetOptionNameHtml(s, n) {
    var normalHtml = "";
    var changeHtml = "";
    var initWidth = "1";
    if (n.percent == "0") { initWidth = "1"; }
    else { initWidth = n.percent; }
    normalHtml += "<div tval=\"\"  bin=\"" + n.OptionId + "\" >";
    normalHtml += "<label >";
    changeHtml += "<div tval=\"\"  bin=\"" + n.OptionId + "\" >";
    changeHtml += "<label >";
    if (n.PicPath != null && n.PicPath != "" && n.PicIsDisplay == 1) {
        normalHtml += "<img src='" + n.PicPath + "' style='width:100px;height:100px;'/>";
        changeHtml += "<img src='" + n.PicPath + "' style='width:100px;height:100px;'/>";
    }
    //********************正常div 开始
    if (n.voteThis == 1) {
        normalHtml += "<span class=\"icon_succS\"></span>"; //投过该选项 则提示
    }
    else {
        normalHtml += "<span class=\"icon_none\">";
        if (s.isGuoqi == 0 && s.isSubmit == 0) {
            if (s.SurveyType == 2) {
                normalHtml += "<input  optionName=\"" + n.OptionName + "\" type=\"checkbox\"     name=\"MulItemType_" + n.OptionId + "\" value=\"" + n.OptionId + "\"  class=\"inputSel optionid_" + n.OptionId + "\"  onclick=\"DcoptionClick(this)\"/>";
            }
            else if (s.SurveyType == 1) {
                normalHtml += "<input optionName=\"" + n.OptionName + "\" type=\"radio\"    name=\"SingleItemType_" + s.VoteId + "\" value=\"" + n.OptionId + "\" class=\"inputSel  optionid_" + n.OptionId + "\" onclick=\"DcoptionClick(this)\"/>";
            }
        }

        normalHtml += "</span>";
    }
    //********************正常div 结束
    //********************改变投票 开始
    changeHtml += "<span class=\"icon_none\">";
    if (s.SurveyType == 2) {
        changeHtml += "<input  optionName=\"" + n.OptionName + "\" type=\"checkbox\"    name=\"MulItemType_" + n.OptionId + "\" value=\"" +  n.OptionId + "\"  class=\"inputSel optionid_" + n.OptionId + "\" onclick=\"DcoptionClick(this)\"/>";
    }
    else if (s.SurveyType == 1) {
        changeHtml += "<input optionName=\"" + n.OptionName + "\" type=\"radio\"  name=\"SingleItemType_" + s.VoteId + "\" value=\"" +  n.OptionId + "\" class=\"inputSel  optionid_" + n.OptionId + "\" onclick=\"DcoptionClick(this)\"/>";
    }
    changeHtml += "</span>";
    //******************改变投票 结束
    normalHtml += "<span class='voteOptionName' >" + n.OptionName + "</span>";
    changeHtml += "<span class='voteOptionName' >" + n.OptionName + "</span>";
    if (s.ViewData == 0)///显示结果百分比
    {
        if (s.isSubmit == 1 || s.isGuoqi == 1) {
            normalHtml += " <span class=\"line\" class=\"" + "line" + n.OptionId + "\"><i style=\"width:" + 1 + "px;\" class=\"linecolor lc" + (optionCount % 10) + "\" maxpercent='" + initWidth + "'></i><em class=\"S_txt2\">" + n.VoteNum + "(" + n.percent + "%)</em></span>";
        }
    }
    else if (s.ViewData == 1) {
        normalHtml += " <span class=\"line\" class=\"" + "line" + n.OptionId + "\"><i style=\"width:" + 1 + "px;\" class=\"linecolor lc" + (optionCount % 10) + "\" maxpercent='" + initWidth + "'></i><em class=\"S_txt2\">" + n.VoteNum + "(" + n.percent + "%)</em></span>";
    }
    else if (s.ViewData == 2) {
        normalHtml += " <span class=\"line\" class=\"" + "line" + n.OptionId + "\"><i style=\"width:" + 1 + "px;\" class=\"linecolor lc" + (optionCount % 10) + "\" maxpercent='" + initWidth + "'></i><em class=\"S_txt2\">" + n.VoteNum + "(" + n.percent + "%)</em></span>";
    }
    normalHtml += "</label>";
    normalHtml += "</div>";
    changeHtml += "</label>";
    changeHtml += "</div>";
    return { normal: normalHtml, change: changeHtml };
}
function GetOneRefByDcid(InObj, dcid) {
    var oneRef;
    for (var i = 0; i < InObj.length; i++) {
        if (InObj[i].dcid == dcid) {
            oneRef = InObj[i];
            break;
        }
    }
    return oneRef;
}

function HideAllDivPageDc(id) {
    jQuery("#mVoteBox").hide();
    jQuery("#divGroupListSel").hide();
    jQuery("#" + id).show();
};
function OnAutoHeightDc(obj) {
    var tip = '';
    var txtDesc = jQuery(obj);
    if (txtDesc.length < 1) return;
    autoTextarea(txtDesc[0]); // 调用
    txtDesc.value = tip;

    txtDesc.onfocus = function () {
        if (txtDesc.value === tip)
            txtDesc.value = '';
    };
    txtDesc.onblur = function () {
        if (txtDesc.value === '')
            txtDesc.value = tip;
    };
};

function GetHoursByDayDC() {
    var hour = "";
    for (var i = 0; i < 24; i++) {
        var temp = i;
        if (i < 10) temp = "0" + i;
        hour += "<option value=\"" + temp + "\">" + temp + ":00</option>"; //<option value=\"" + temp +":30"+"\">"+temp+":30</option>
    }
    return hour;
};

