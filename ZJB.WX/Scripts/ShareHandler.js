///分享处理
var imageW100H100 = "?imageMogr2/strip|imageView2/2/w/100/h/100";
function ShareHandler(param) {
    this.textArea = param.textArea; //  arguments[0] == undefined ? "" : arguments[0];
    this.faceBtn;
    this.uploadBtn;
    this.topicBtn;
    this.sendBtn;
    this.shareDropdown = jQuery(".msgeditor-share .share-dropdown");
    this.shareToBtn = jQuery(".msgeditor-ctrl .dropdown-btn p").eq(0);
    this.shareToEm = this.shareToBtn.find(".msgeditor-share-to");
    this.timerShareGroupDropdown;
    this.fileDiv = param.fileDiv;  //jQuery("#fileListDiv");
    this.msgeditMainElem = param.mbMain;  //jQuery("#mb-msgeditor-main");
    this.hdImageUrl = param.hdImageUrl;  //jQuery("#hdImageUrl");
    this.callBack = param.callBack; //添加后的回调
    this.maxInputLength = param.maxLength == undefined ? 10000 : param.maxLength; ///最多可以输入的字符数
    this.className = "cmpdisabled"; //提交按钮灰色
    this.replyId = param.replyId == undefined ? 0 : param.replyId; //回复的主贴Id
    this.replyWhoEm; //删除回复谁
    this.replyWhoSpan; //显示回复谁
    this.replyHdr; //回复框的右边用于显示输入多少给字
    this.IsShowTopic = param.isShowTopic == undefined ? true : param.isShowTopic;
    this.isShowGroup = param.isShowGroup == undefined ? true : param.isShowGroup;
    this.groupShowType = param.groupShowType == undefined ? 1 : param.groupShowType;
    this.textChangeCtr;
    this.Init();
}
ShareHandler.prototype = {
    Init: function () {
        var me = this;
        me.sendBtn = me.msgeditMainElem.find(".msgeditor-ctrl-post");
        ///分享
        this.textChangeCtr = new TextChangeUtil({ input: me.textArea,
            btn: me.sendBtn,
            isShowTopic: me.IsShowTopic
        });
        this.textArea.click(function () {
            me.textArea.height(89).parent().height(89);
            me.msgeditMainElem.find(".msgeditor-ft").show().find(".msgeditor-addons,.msgeditor-ctrl").show();
        }).keyup(function () {
            var areaValue = me.textArea.val();
            if (jQuery.trim(areaValue) != "") {
                me.RemoveCommitDisabled();
            }
            else {
                me.SetCommitDisabled();
            }
            me.textChangeCtr.Change();
            me.ShowInputLength();
        })
        ///表情按钮
        me.faceBtn = me.msgeditMainElem.find(".msgeditor-addons-smiley");
        me.faceBtn.click(function (e) {
            var faceHanler = new FaceHandler({ targetObj: me.faceBtn, inputObj: me.textArea, callBack: function () { me.RemoveCommitDisabled(); } });
            e.stopPropagation();
        });
        ///上传按钮
        me.uploadBtn = me.msgeditMainElem.find(".msgeditor-addons-doc");
        me.uploadBtn.click(function () {
            uploadFileHandler = new UploadFileHandler({ FileDiv: me.fileDiv, Success: function (data) { me.UploadComplete(data); } }); //上传文件控件
            uploadFileHandler.UploadClick();
        });
        if (me.IsShowTopic) {
            ///话题按钮
            me.topicBtn = me.msgeditMainElem.find(".mb-twteditor-addons-topic"); // 
            me.topicBtn.click(function (e) {
                var topicHandler = new TopicHandler(me.textArea, me.topicBtn);
                e.stopPropagation();
            });
        }
        if (me.replyId > 0) {
            me.replyWhoSpan = me.msgeditMainElem.find(".msgeditor-hdl span"); //显示回复谁
            me.replyWhoEm = me.msgeditMainElem.find(".msgeditor-hdl em"); //删除回复谁
            me.replyWhoEm.click(function () {
                me.replyWhoSpan.attr("data-id", 0).html("").parent().hide();
            });
        }
        me.replyHdr = me.msgeditMainElem.find(".msgeditor-hdr");
        me.sendBtn.click(function () {
            me.Commit();
        });
        if (this.isShowGroup) {
            this.groupHandler = new CircleGroupPublish({
                dropdownContent: me.shareDropdown.find(".dropdown-content"),
                checkedEm: me.shareToEm,
                showType: me.groupShowType
            });
            this.groupHandler.GetJoinList();
            this.shareToBtn.mouseover(function () {
                if (me.timerShareGroupDropdown)
                    clearTimeout(me.timerShareGroupDropdown);
                me.shareDropdown.show();

            }).mouseleave(function () {
                me.timerShareGroupDropdown = setTimeout(function () { me.shareDropdown.hide() }, 500);
            });
            this.shareDropdown.mouseover(function () {
                if (me.timerShareGroupDropdown)
                    clearTimeout(me.timerShareGroupDropdown);
            }).mouseleave(function () {
                me.shareDropdown.hide();
            });
        }
        //点击文档时，隐藏语言列表
        jQuery(document).click(function () {
            jQuery(".mb-smiley-layer").hide();
            jQuery(".mb-addtopic-layer").hide();
        });
    },
    GetShareGroupId: function () {
        var groupId = this.shareToEm.attr("data-groupid");
        if (groupId == undefined || groupId == null || groupId == "" || groupId < 1)
            groupId = 1;
        var hdGroupEm = jQuery("#hdGroupId");
        if (hdGroupEm.length > 0) {
            var checkedGroupId = hdGroupEm.val();
            if (checkedGroupId > 0) groupId = checkedGroupId;
        }
        return groupId;
    },
    ShowInputLength: function () {
        var me = this;
        if (me.replyHdr) {
            var length = me.textArea.val().length;
            if (length > me.maxInputLength) {
                var q = me.replyHdr.find("q").show();
                q.siblings().hide();
                q.find("strong").html(length - me.maxInputLength);
                me.SetCommitDisabled();
            }
            else {
                var q = me.replyHdr.find("span").show();
                q.siblings().hide();
                q.find("em").html(me.maxInputLength - length);
            }
        }
    },
    Commit: function () {
        
        if (this.IsCommitDiabled()) return;
        var me = this;
        var detail = me.textArea.val();
        if (jQuery.trim(detail) == "") {
            alert('说说你在做什么，想什么，随便写点东西吧！');
            me.textArea.focus();
            return;
        }
        detail = TsqFuncTool.ReplaceStr(TsqFuncTool.ReplaceStr(TsqFuncTool.ReplaceStr(detail, "<", " &lt;"), ">", "&gt;"), "'", "&acute;");
        var imgUrl = me.GetImageUrlByImg();
        me.SetCommitDisabled();
        var replySpan = me.msgeditMainElem.find(".msgeditor-hdl span");
        var replyCommentId = 0, replyCommentWho = "";
        if (replySpan.length > 0) {
            replyCommentId = replySpan.attr("data-id");
            // replyCommentWho = replySpan.html();
        }
        if (replyCommentId == undefined || replyCommentId == "" || replyCommentId == null) replyCommentId = 0;
        var submitParame = { detail: replyCommentWho + detail, ImageList: imgUrl, replyId: me.replyId, replyCommentId: replyCommentId, callBack: function () { me.InitField(); } };
        // detail: replyCommentWho + detail, pic: imgUrl, groupId: me.GetShareGroupId(), replyId: me.replyId, replyCommentId: replyCommentId,
       // remindUsers: me.textChangeCtr.GetAUserIds(),
        // callBack: function () { me.InitField(); }
        me.callBack(submitParame);
        me.fileDiv.hide();
    },
    UploadComplete: function (data) {
        var url = "";
        if (data.length == 0) return;
        var me = this;
        var imgObj = { url: "", width: 0, height: 0 };
        var imgObjArr = [];
        var imgLength = this.fileDiv.find(".cattach-img-preview").find("img").length;
        var str = "", imgStr = "", imgArr = [];
        var sizeConst = imageW100H100;
        for (var i = 0; i < data.length; i++) {
            imgObj = { url: "", width: 0, height: 0 };
            var n = data[i];
            var key= ""+new Date().getTime()+Math.random() * 100000;
            str += "<li class=\"uploader_attachment_doc\" guid=\"" + key + "\">";
            str += "<em>" + n.name + "</em><i>" + TsqFuncTool.ConvertToKBMBGB(n.size) + "</i>";
            str += "<b style=\"border-left-width: 200px; width: 0px; display: none;\"></b>";
            str += "<span>(已完成)<a class=\"cattachqueue-remove\" guid=\"" + key + "\">删除</a></span></li>";
            var pic = n.url;
            imgStr += "<li guid=\"" + key + "\" class=\"cxl_upload_img\"><span class=\"upload_img\"><img img-index=\"" + (parseInt(imgLength) + i) + "\" w=\"0\" h=\"0\" src=\"" + pic +sizeConst+ "\"  class=\"att-file-pic\"></span></li>";
            imgArr.push(pic);
        }
        me.fileDiv.find(".cattachqueue").append(str);
        me.fileDiv.find(".cattach-img-preview").append(imgStr);
        me.fileDiv.find(".cattachqueue-remove").off("click").on("click", function () {
            me.DeleteFile(this);
        });
        me.AddImgUrl(imgArr.join(';'));
    },
    DeleteFile: function (obj) {
        var me = this;
        var curObj = jQuery(obj);
        var guid = curObj.attr('guid');
        var imgUrl = me.fileDiv.find(".cxl_upload_img[guid='" + guid + "']").find("img").attr("src");
        me.fileDiv.find("li[guid='" + guid + "']").remove();
        me.RemoveImgUrl(imgUrl);
        if (me.fileDiv.find(".uploader_attachment_doc").length == 0) {
            me.fileDiv.hide();
        }
        var imgs = this.fileDiv.find(".cattach-img-preview").find("img");
        jQuery.each(imgs, function (i, n) {
            jQuery(n).attr("img-index", i);
        });
    },
    AddImgUrl: function (str) {
        var history = this.hdImageUrl.val();
        if (history == "") history = str;
        else history = history + ";" + str
        this.hdImageUrl.val(history);
    },
    RemoveImgUrl: function (str) {
        var history = this.hdImageUrl.val();
        if (history == "") {
            return;
        }
        var arr = history.split(';');
        for (var i = 0; i < arr.length; i++) {
            if (arr[i] == str) {
                arr.splice(i, 1);
                break;
            }
        }
        this.hdImageUrl.val(arr.join(';'));
    },
    GetImageUrlByImg: function () {
        var imgs = this.fileDiv.find("li.cxl_upload_img img");
        var imgObjArr = [];
        jQuery.each(imgs, function (i, n) {
            var imgEl = jQuery(n);
            var obj = { url: "", witdh: 0, hight: 0 };
            obj.url = imgEl.attr("src");
            if (obj.url!=null&&obj.url != "") {
                obj.url= obj.url.replace(imageW100H100, "");
            }
            obj.width = imgEl.attr("w");
            obj.height = imgEl.attr("h");
            imgObjArr.push("{ImageUrl:'" + obj.url + "',ImageWidth:" + obj.width + ",ImageHeight:" + obj.height + "}");
        });
        return "[" + imgObjArr.join(',') + "]";
    },
    GetImageUrl: function () {
        return this.hdImageUrl.val();
    }, ///清除回复谁
    HideReplyWhoSpan: function () {
        if (this.replyId < 1) return;
        this.replyWhoSpan.attr("data-id", 0).html("").hide().parent().hide();
        this.replyWhoEm.css("display", "none");
    },
    InitField: function () {
        this.RemoveCommitDisabled();
        this.textArea.val("");
        this.ShowInputLength();
        this.HideReplyWhoSpan();
        this.hdImageUrl.val("");
        this.fileDiv.find(".cattachqueue").html("");
        this.fileDiv.find(".cattach-img-preview").html("");
        if (this.groupHandler) {
            this.groupHandler.MoveFirst();
        }
    },
    IsCommitDiabled: function () {
        if (this.msgeditMainElem.hasClass(this.className)) return true; //.parent()
        return false;
    },
    SetCommitDisabled: function () {
        this.msgeditMainElem.addClass(this.className); //.parent()
    },
    RemoveCommitDisabled: function () {
        this.msgeditMainElem.removeClass(this.className); //.parent()
    }
};
///话题处理
function TopicHandler() {
    this.textArea = arguments[0];
    this.curTargetObj = arguments[1];
    this.topicDiv = jQuery(".mb-addtopic-layer");
    this.curTopic;
    this.Show();
}
TopicHandler.prototype = {
    Init: function () {
        var me = this;
        this.topicDiv.find(".addtopic-btn-box .btn-green").click(function () {
            me.curTopic = jQuery(this);
            me.CheckedTopic();
        });
    },
    CheckedTopic: function () {
        var me = this;
        var obj = me.curTopic;
        var topic = obj.attr("data-topic"); //"#请在这里输入自定义话题#";
        var txtVal = me.textArea.val();
        if (txtVal.indexOf(topic) < 0) {
            //me.textArea.val(topic);
            me.textArea.setValueAutoFocus(topic);
        }
        txtVal = me.textArea.val();
        var start = txtVal.indexOf(topic) + 1;
        var end = start + topic.length - 2;
        //me.textArea.val("#请在这里输入自定义话题#");
        me.textArea.setSelection(start, end);
        me.topicDiv.hide();
    },
    Create: function () {
        var strhtml = "<!--话题开始-->";
        strhtml += "<div class=\"layer layer-shim br5 mb-addtopic-layer\" style=\"z-index: 5020; left: 434px; top: 308px;\">";
        strhtml += "<div class=\"layer-main\" style=\"\">";
        strhtml += "<span class=\"layer-arrow p2\"></span>";
        strhtml += "<div class=\"layer-bd\">";
        strhtml += "<div class=\"addtopic-btn-box\">";
        strhtml += "<a class=\"btn btn-green\" data-topic=\"#请在这里输入自定义话题#\">";
        strhtml += "<em>插入话题</em></a></div>";
        strhtml += "<h5>热门话题</h5><ul>";
        strhtml += "</ul>";
        strhtml += "<a class=\"addtopic-close\" href=\"#\"></a>";
        strhtml += "</div></div>";
        strhtml += "</div>";
        strhtml += "<!--话题结束-->";
        jQuery("body").append(strhtml);
        this.topicDiv = jQuery(".mb-addtopic-layer");
        this.Init();
        this.GetHotList();
    },
    GetHotList: function () {
        var ran = Math.random() * 100000;
        var me = this;
        jQuery.getJSON("/AjaxAshx/Topic/TopicHandler.ashx", { type: "hotlist", ran: ran },
            function (msg) {
                var strhtml = "";
                jQuery.each(msg, function (i, n) {
                    strhtml += "<li class=\"ellipsis \">";
                    strhtml += "<a href=\"javascript:\" data-topic=\"" + n + "\">" + n + "</a></li>";
                });
                me.topicDiv.find(".layer-main ul").html(strhtml);
                me.topicDiv.find("ul .ellipsis a").off("click").on("click", function () {
                    me.curTopic = jQuery(this);
                    me.CheckedTopic();
                });
            });
    },
    Show: function () {
        var me = this;
        if (this.topicDiv.length == 0) {
            this.Create();
        }
        else if (this.topicDiv.is(":visible")) {
            this.topicDiv.hide();
            return;
        }
        var offset = this.curTargetObj.offset();
        this.topicDiv.css({ "top": (offset.top + me.curTargetObj.height() + 10) + "px", "left": (offset.left - 115) + "px" }).show();
    }
}