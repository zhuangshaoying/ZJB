var imageW50H50Q85 = "?imageMogr2/strip|imageView2/1/w/50/h/50/q/100";
var imageW100H00 = "?imageMogr2/strip|imageView2/2/w/100/h/100/q/100";
var imageW30H30Q85 = "?imageMogr2/strip|imageView2/1/w/30/h/30/q/100";
function LybHandler(param) {
    this.kind = param.Kind == undefined ? "dt" : param.Kind;
    this.ownerUserId = param.ownerUserId == undefined ? 0 : param.ownerUserId;
    this.shareTextArea = param.ShareTextArea;// == undefined ? "dt" : param.ShareTextArea;
    this.LybTextLength = 140*2; //主贴默认显示的最多字符
    this.replyHandler;
    this.lastId = 0;
    this.lastTime = 0;
    this.pageSize = 10;
    this.groupId = 1;
    this.scrollState = jQuery("#hdState");
    this.lybList = jQuery(".dynamicListBox");
    this.defaultIcon = "/images/txDefault.png";
    this.defaultCommentSize = 5;
    this.shareGroupId = 1;
    this.searchInput = param.SearchInput;
    this.searchBtn = param.SearchBtn;
    this.keyword = "";
    this.isShowGroup = param.IsShowGroup == undefined ? true : param.IsShowGroup;
    this.pageMoreBox = jQuery(".paginator-more");
    this.RefreshBtn = param.RefreshBtn;///刷新动态列表的按钮
    this.CurrentMenu = param.CurrentMenu == null ? "ShareDynamic" : param.CurrentMenu;
    this.MenuGroupClass = "home-editor-tab-group";
    this.IsFirstComming = 1;
    this.Init();
}
LybHandler.prototype = {
    Init: function () {
        var me = this;
        me.IsFirstComming = 1;
        if (this.kind != "search") this.keyword = "";
        else {
            this.keyword = decodeURI(TsqFuncTool.Request("keyword"));
            this.keyword = TsqFuncTool.Trim(this.keyword);
        }
        me.replyHandler = new ReplyHandler();
        if (this.shareTextArea) {
            var shareHandler = new ShareHandler({
                textArea: me.shareTextArea,
                fileDiv: jQuery("#fileListDiv"),
                mbMain: jQuery("#mb-msgeditor-main"), //
                hdImageUrl: jQuery("#hdImageUrl"),
                isShowGroup: me.isShowGroup,
                callBack: function (msg) { me.AddLyb(msg); }
            });
        }
        if (this.searchBtn) {
            this.searchBtn.click(function () {
                me.keyword = me.searchInput.val();
                me.kind = "search";
                me.InitValue();
                me.GetList();
            })
        }
        if (this.RefreshBtn)
        {
            this.RefreshBtn.click(function () {
                me.InitValue();
                me.GetList();
            });
        }
        me.InitMenuChange();
    },
    GetScrollState: function () {
        return this.scrollState.val();
    },
    SetScrollState: function (v) {
        this.scrollState.val(v);
    },
    InitValue: function () {
        this.lastId = 0;
        this.lastTime = 0;
        this.lybList.find(".tsq_zt_bs").remove();
        this.IsFirstComming = 1;
    },
    GetList: function () {
        var me = this;
        var pageSize = this.pageSize;
        var pageMore = this.pageMoreBox;
        var parame={
            type: me.kind,
            keyword: me.keyword,
            lastTime: this.lastTime,
            pageSize: this.pageSize,
            firstComming: this.IsFirstComming
        };
        jQuery.getJSON("/Social/DynamicList", parame, function (result) {
            var msg = result.data;
            me.IsFirstComming = 0;
            me.FormateLybList(msg);
            var state = "";
            if (msg.length < pageSize) {
                state = "1";
                pageMore.show();
            }
            me.SetScrollState(state);
        });
    },
    FormateLybList: function (msg) {
        var me = this;
        var bulider = new StringBulider();
        var ids = [];
        var length = msg.length;
        var ListRefDc = new Array();//关联投票
        var dcidArr = new Array();
        jQuery.each(msg, function (i, n) {
            bulider.append(me.FormateLyb(n));
            ids.push(n.Id);
            if (n.VoteId > 0) {
                dcidArr.push(n.VoteId);
                ListRefDc.push({ dcid: n.VoteId, nid: n.Id });
            }
            if (length == (i + 1)) {
                me.lastId = 0; // n.Id;
                me.lastTime = n.LastCommentTime;
            }
        });
        if (this.kind == "refresh") {
            this.lybList.html(bulider.toString());
        }
        else {
            this.lybList.append(bulider.toString());
        }
       
        jQuery.each(msg, function (i, n) {
            if (n.SupportList.length > 0) {
                me.FormateSupportList(n);
            }
        });
        this.lybList.find(".mb-view-detail[data-switch='msg-content-limit']").find("a").off("click").on("click", function () {
            me.ShowMore(this);
        });
        me.BindBtnEvent();
        //me.GetZanList(ids.join());
        me.replyHandler.GetReplyList(ids.join());
        if (typeof getDcData != 'undefined') {
            getDcData(ListRefDc, dcidArr);
        }
    },
    ///显示评论框
    ShowReplyTextArea: function (obj) {
        var curObj = jQuery(obj);
        var ft = curObj.parent(".tsq_lyb_dbcd").parent();
        var cmtbox = ft.find(".mb-cmtbox");
        var cmteditor = cmtbox.find(".mb-cmteditor");
        if (cmteditor.length > 0) return;
        var lyb = ft.parent();
        this.replyHandler.CreateReplyTextArea(cmtbox.show(), lyb.attr('data-id'));
    },
    //查看更多
    ShowMore: function (targetObj) {
        var me = this;
        var obj = jQuery(targetObj);
        var objParent = obj.parent();
        var className = objParent.attr("data-switch");
        var msgContent = objParent.siblings(".msg-content");
        if (msgContent.hasClass(className)) {
            msgContent.removeClass(className);
            objParent.contents().first().remove();
            obj.html('<br>收起');
        }
        else {
            msgContent.addClass(className);
            obj.html('<br>查看更多').before("......");
        }
    },
    FormateLyb: function (n) {
        var me = this;
        var bulider = new StringBulider();
        bulider.append('<div class=\"twt-type- mb-twt msg kcmp tsq_zt_bs\" data-id="', n.Id, '" data-username="', n.UserName, '" data-userId="', n.UserId, '" data-lastCommentId="0">');
        var icon = n.Portrait == "" ? me.defaultIcon : n.Portrait;
        bulider.append('				<a href="javascript:Global.LinkUserMain(', n.UserId, ')" class="namecard avatarbig"><img alt="头像" onerror="this.src=\'', me.defaultIcon, '\'" UserName="this.src=\'', me.defaultIcon, '\'" src="', icon, imageW100H00, '" class="avatar"></a>');
        bulider.append('				<div class="msg-bd">' );
        bulider.append('				<div class="msg-main">');
        if (n.IsTop == 1) {
            bulider.append('<span class="setTopIco">置顶</span>');
        }
        bulider.append('<a href="javascript:Global.LinkUserMain(', n.UserId, ')" class="mb-twt-username namecard">', n.UserName, '<span style="color:#999;font-size:12px;margin-left:3px"></span></a>');
        bulider.append('<br/>');
        var noWrapContent = n.WrapComment.replace(/<\/?.+?>/g, "");//WrapContent.replace(/<\/?.+?>/g, "");
        var remindUsers = [], remindUserHtml = [];
        if (n.RemindUsers.length > 0) {
            jQuery.each(n.RemindUsers, function (i, r) {
                remindUsers.push(r.UserName);
                remindUserHtml.push("<a href=\"javascript:Global.LinkUserMain(',r.UserId,')\" >@" + r.UserName + "</a>");
            });
        }
        var temp = noWrapContent + (remindUsers.length > 0 ? " " + remindUsers.join("@") : "");
        var content = me.AddAUserHtml(n.WrapComment, n.RemindUsers, remindUserHtml); // n.WrapContent + (remindUserHtml.length > 0 ? " " + remindUserHtml.join(" ") : "");
        content = TsqFuncTool.ReplaceBr(content);
        if (me.kind == "search" && me.keyword != "") {
            if (content) {
                content = TsqFuncTool.ReplaceStr(content, me.keyword, "<em class=\"highlight\">" + me.keyword + "</em>");
            }
        }
        if (n.DynamicContent && TsqFuncTool.ByteLength(temp) > me.LybTextLength) {
            bulider.append('<span class="msg-content msg-content-limit">', content, '</span>');
            //若超出140*2个字则显示更多
            bulider.append('<span class="mb-view-detail" data-switch="msg-content-limit">......<a href="javascript:void(0);"><br>查看更多</a></span>');
        }
        else {
            bulider.append('<span class="msg-content">', content, '</span>');
        }
        bulider.append('</div>');
        if(n.VoteId > 0)
        {
            bulider.append("<div detailId=\"" + n.Id + "\" id=\"showVoteResult" + n.VoteId + "\" class=\"Vote_Box WB_media_expand SW_fun2 S_line1 S_bg1 showVoteResult\">");
            bulider.append("</div>");
        }
        bulider.append('<div class="msg-attach">').append(me.FormateImage(n.ImageList)).append("</div>"); //主贴图片的显示
        bulider.append('</div>');
        bulider.append('<div class="msg-ft">');
        bulider.append(me.FormateBtn(n)); //主贴的操作按钮
        bulider.append((n.SupportList == null || n.SupportList.length == 0 ? "" : me.CreateLybZanDiv(n.SupportList.length))); //主贴的赞列表
        bulider.append(me.FormateReplyInfo(n)); //对于页面初始化加载时有评论的微博添加恢复框
        bulider.append('</div>'); //ft                          
        bulider.append('</div>');
        return bulider.toString();
    },
    AddAUserHtml: function (content, remindUsers, remindHtml) {
        if (!content) return;
        var arr = [];
        var length = content.length;
        for (var i = 0; i < remindUsers.length; i++) {
            var aUser = "@" + remindUsers[i].UserName;
            var aUserLength = aUser.length;
            if (content.indexOf(aUser) >= 0) {
                content = TsqFuncTool.ReplaceStr(content, aUser, remindHtml[i] + " "); // content.replace(aUser, remindHtml[i] + " ");
            }
            //            else if (content.substring(length - aUserLength - 1, aUserLength) == aUser) {
            //                content = content.replace(aUser, remindHtml[i]);
            //            }
            else
                arr.push(remindHtml[i]);
        }
        return content + arr.join(" ");
    },
    ///格式化主贴图片
    FormateImage: function (pics) {
        if (pics == undefined || pics == null || pics.length == 0) return "";
        var arr = pics; // pics.split(';');
        var length = arr.length;
        if (length == 0) return "";
        var bulider = new StringBulider();
        bulider.append('<div class="msg-attach-pics ">');
        for (var i = 0; i < length; i++) {
            var picObj = arr[i];
            var pic = picObj.ImageUrl + imageW100H100;
            bulider.append('<a  href="javascript:void(0);" class="zoom-min " style="display: inline-block;">');
            bulider.append('<img img-index="', i, '" alt="" src="', pic, '" title="" class="att-file-pic" ><img src="/images/s-left-bg-png8.png" class="block"/></a>');
        }
        bulider.append('</div>');
        return bulider.toString();
    },
    ///格式化操作按钮
    FormateBtn: function (n) {
        var bulider = new StringBulider();
        bulider.append('<div class="msg-ctrl mb10 tsq_lyb_dbcd">'); //主贴的底部菜单
        var zanBtnTxt = n.IsSupport ? "取消赞" : "赞";
        bulider.append('<a href="javascript:" data-zan="', n.IsSupport, '" class="msg-ctrl-orlike">', zanBtnTxt, '</a> <a class="msg-ctrl-cmt">回复</a>');
      // var collectionBtnTxt = n.IsCollection ? "取消收藏" : "收藏";
        //bulider.append('<a href="javascript:" data-fav="', n.IsCollection, '" class="msg-ctrl-fav">', collectionBtnTxt, '</a>');
        //bulider.append('<a href="/Content/CircleDynamic/CircleDynamicDetail.aspx?id=', n.Id, '">详情</a>');
        if (curLoignUserId == n.UserId && curLoignUserId > 0) {
            bulider.append('<a href="javascript:" class="msg-ctrl-del">删除</a>');
        }
        bulider.append('<a href="javascript:" class="msg-ctrl-ext">', n.AddTime, '</a>');
        bulider.append('</div>');
        return bulider.toString();
    },
    BindBtnEvent: function () {
        var me = this;
        var tsq_lyb_dbcd = this.lybList.find(".tsq_lyb_dbcd");
        //直接评论
        tsq_lyb_dbcd.find(".msg-ctrl-cmt").off("click").on("click", function () {
            me.ShowReplyTextArea(this);
        });
        //赞
        tsq_lyb_dbcd.find(".msg-ctrl-orlike").off("click").on("click", function () {
            me.ZanLyb(this);
        });
        //收藏
        tsq_lyb_dbcd.find(".msg-ctrl-fav").off("click").on("click", function () {
            me.DynamicCollection(this);
        });
        //删除
        tsq_lyb_dbcd.find(".msg-ctrl-del").off("click").on("click", function () {
            var obj = this;
            jQuery.dialog.confirm('你确定要删除这掉消息吗？', function () {
                me.DelteLyb(obj);
            }, function () {

            });
        });
        var cmtbox_more = this.lybList.find(".mb-cmtbox-more");
        cmtbox_more.find("a").off("click").on("click", function () {
            me.GetMoreDynamicComment(this);
        });
    },
    ///对于页面初始化加载时有评论的微博添加恢复框
    FormateReplyInfo: function (n) {
        var bulider = new StringBulider();
        bulider.append('<div class="mb-cmtbox none">');
        var size = parseInt(n.CommentNum) - this.defaultCommentSize;
        var className = size > 0 ? "" : " none";
        bulider.append('<div class="mb-cmtbox-more', className, '"><span></span>');
        bulider.append('<a data-id="', n.Id, '" href="javascript:"><i class="icon icon-comment"></i>还有<b>', size, '</b>条较早的回复 »</a>').append('</div>');
        bulider.append('<div class="mb-cmtpanel msgpanel "><div class="msgpanel-bd"></div></div></div>');
        return bulider.toString();
    },
    GetMoreDynamicComment: function (obj) {
        var curObj = jQuery(obj);
        var id = curObj.attr("data-id");
        var lybDiv = this.lybList.find(".tsq_zt_bs[data-id='" + id + "']"); //curObj.parentsUntil(".tsq_zt_bs");
        if (id < 1) return;
        var plgdDiv = lybDiv.find(".tsq_plgd_bs");
        
        if (plgdDiv.length > 0) {
            plgdDiv.toggle();
            if (plgdDiv.is(":visible")) {
                curObj.data("tip", curObj.html()).html("<i class=\"icon icon-comment\"></i>收起 »");
            }
            else {
                curObj.html(curObj.data("tip"));
            }
            return;
        }
        this.replyHandler.lastId = lybDiv.attr("data-lastCommentId");
        //this.replyHandler.pageSize = 10000;
        this.replyHandler.GetReplyListById(id, true);
        curObj.data("tip", curObj.html()).html("<i class=\"icon icon-comment\"></i>收起 »");
    },
    AddLybBack: function (msg) {
        var me = this;
        var bulider = new StringBulider();
        jQuery.each(msg, function (i, n) {
            bulider.append(me.FormateLyb(n));
        });
        this.lybList.prepend(bulider.toString());
        this.lybList.find(".mb-view-detail[data-switch='msg-content-limit']").find("a").off("click").on("click", function () {
            me.ShowMore(this);
        });
        me.BindBtnEvent();
    },
    AddLyb: function (param) {
        var me = this;
        jQuery.post("/Social/AddDynamic", param, function (result) {
            var msg = result.status;
            var data = result.data;
            if (msg == -1) {
                jQuery.dialog.tips("请随便说点什么！");
                return;
            }
            else if (msg == 0) {
                jQuery.dialog.tips("发布失败！");
                return;
            }
            art.dialog.tips("发布成功！", 1.5, "success.gif");
            var datalist = [];
            datalist.push(data);
            me.AddLybBack(datalist);
            if (param.callBack) {
                param.callBack();
            }
        });
    },
    FormateSupportList: function (obj) {
        var me = this;
        var lybid = obj.Id;
        var bulider = new StringBulider();
        var length = obj.SupportList.length;
        var dh = "、";
        var myStr = "";
        var arr = [];
        jQuery.each(obj.SupportList, function (i, n) {
            if (n.UserId == curLoignUserId) {
                myStr = '<span class="iand"><a href="javascript:Global.LinkUserMain(' + n.UserId + ')" class="like-number-flag">我' + (length > 1 ? dh : "") + '</a></span>';
            }
            else {
                arr.push('<a href="javascript:Global.LinkUserMain(' + n.UserId + ')" class="like-number-flag">' + n.UserName + ' </a>');
            }
        });
        var lyb = me.lybList.find(".tsq_zt_bs[data-id='" + lybid + "']");
        lyb.find(".msg-ft .msg-likeinf .like-text").before(myStr + arr.join(dh));
    },
    DelteLyb: function (obj) {
        var curObj = jQuery(obj).hide();
        var ft = curObj.parent(".tsq_lyb_dbcd").parent();
        var likeinf = ft.find(".msg-likeinf");
        var lyb = ft.parent();
        var lybid = lyb.attr('data-id');
        jQuery.post("/Social/DeleteDynamic", {
            dynamicId: lybid
        }, function (result) {
            var msg = result.status;
            if (msg == 1) {
                jQuery.dialog.tips("删除成功！", 1.5, "success.gif");
                lyb.remove();
            }
            else {
                jQuery.dialog.tips("删除失败！");
            }
        });
    },
    ZanLyb: function (obj) {
        var curObj = jQuery(obj).hide();
        var iszan = curObj.attr("data-zan");
        iszan = iszan == "false" ? false : true;
        var ft = curObj.parent(".tsq_lyb_dbcd").parent();
        var likeinf = ft.find(".msg-likeinf");
        var lyb = ft.parent();
        var lybid = lyb.attr('data-id');
        if (likeinf.length < 1) {
            ft.find(".mb-cmtbox").before(this.CreateLybZanDiv());
            likeinf = ft.find(".msg-likeinf");
        }
        if (!iszan) {
            if (likeinf.find("a").length == 0) {
                likeinf.find(".icon-like").after('<span class="iand"><a href="#" class="like-number-flag">我 </a></span>');
            }
            else {
                likeinf.find(".icon-like").after('<span class="iand"><a href="#" class="like-number-flag">我</a> 、 </span>');
            }
        }
        else {
            likeinf.find('span.iand').remove();
        }
        if (likeinf.find("a").length == 0) {
            likeinf.remove();
        }
        var isSupport = iszan ? 0 : 1;
        // curObj.hide();
        curObj.html((iszan ? "赞" : "取消赞")).attr("data-zan", !iszan).show();
        jQuery.post("/Social/DynamicSupportAdd", {
            type: "support",
            dynamicId: lybid,
            isSupport: isSupport
        }, function (result) {
            var msg = result.status;
            if (msg > 0) {
                var str = iszan ? "取消赞成功" : "赞成功";
                jQuery.dialog.tips(str, 1.5, "success.gif");
            }
        });

    },
    CreateLybZanDiv: function (supportNum) {
        var bulider = new StringBulider();
        bulider.append('<div class="msg-likeinf ellipsis">');
        bulider.append('<i class="icon icon-like"></i>');
        // bulider.append('<span class="iand"><a href="#" class="like-number-flag">我</a></span>');
        bulider.append('<span class="like-text">等 ', supportNum, ' 人觉得这很赞</span>');
        bulider.append('</div>');
        return bulider.toString();
    },
    DynamicCollection: function (obj) {
        var curObj = jQuery(obj).hide();
        var isfav = curObj.attr("data-fav");
        isfav = isfav == "false" ? false : true;
        var ft = curObj.parent(".tsq_lyb_dbcd").parent();
        var lyb = ft.parent();
        var lybid = lyb.attr('data-id');
        var state = 0;
        if (!isfav) {
            state = 1;
        }
        if (lybid < 1) {
            jQuery.dialog("请选择要收藏的主贴");
            return;
        }
        //curObj.hide();
        curObj.html(isfav ? "收藏" : "取消收藏").attr("data-fav", !isfav).show();
        jQuery.post("/AjaxAshx/Dynamic/AddDynamicCollection.ashx", {
            type: "add",
            dynamicId: lybid,
            iscollection: state
        }, function (msg) {
            if (msg > 0) {
                var str = isfav ? "取消收藏成功" : "收藏成功";
                jQuery.dialog.tips(str, 1.5, "success.gif");
            }
            //            else {
            //                curObj.show();
            //            }
        });
    }
    ,InitMenuChange: function () {
        var thisClass = this.MenuGroupClass;
        jQuery("." + thisClass).click(function () {
            var allTab = jQuery("." + thisClass);
            allTab.removeClass("current");
            jQuery(this).addClass("current");
            jQuery.each(allTab, function (i, n) {
                var refTabId = jQuery(n).attr("refTabId");
                jQuery("#" + refTabId).hide()
            });
            var thisrefTabId = jQuery(this).attr("refTabId");
            jQuery("#" + thisrefTabId).show();
            if (thisrefTabId == "W_layer") {
                jQuery("#xzup").addClass("home-editor-tab-arrow").removeClass("home-editor-tab-arrow_1");
                jQuery("#xzdown").addClass("home-editor-tab-arrow_v").removeClass("home-editor-tab-arrow_w");
                InitvoteBox();
            }
            else {
                jQuery("#xzup").addClass("home-editor-tab-arrow_1").removeClass("home-editor-tab-arrow");
                jQuery("#xzdown").addClass("home-editor-tab-arrow_w").removeClass("home-editor-tab-arrow_v");   
            }
        });
    }
}
function ReplyHandler() {
    this.LybTextLength = 140*2; //主贴默认显示的最多字符
    this.lybList = jQuery(".dynamicListBox");
    this.pageSize = 5;
    this.maxPageSize = 10000;
    this.lastId = 0;
    this.defaultIcon = "/images/txDefault.png";
    this.Init();

}
ReplyHandler.prototype = {
    Init: function () {

    },
    GetReplyList: function (ids) {
        if (ids == undefined || ids == null || jQuery.trim(ids) == "") return;
        var me = this;
        jQuery.getJSON("/Social/DynamicReplayList", { dynamicIds: ids, pageSize: me.pageSize },
        function (result) {
            var msg = result.data;
            jQuery.each(msg, function (i, n) {
                me.FormateReplyRecord(n);
            });
        });
    },
    GetReplyListById: function (id, ismore) {
        if (id == undefined || id == null || id < 1) return;
        ismore = ismore == undefined ? false : ismore;
        var me = this;
        if (!this.lastId) this.lastId = 0;
        var ran = Math.random() * 100000;
        jQuery.get("/Social/DynamicReplayList", {
            dynamicIds: id,
            lastId: this.lastId,
            pageSize: this.maxPageSize,
            ran: ran
        },
        function (result) {
            var msg = result.data;
            me.lastId = 0;
            jQuery.each(msg, function (i, n) {
                me.FormateReplyRecord(n, ismore);
            });
            var lybDiv = jQuery(".tsq_zt_bs[data-id='" + id + "']");
            var cmt_more = lybDiv.find(".mb-cmtbox-more");
            cmt_more.removeClass("none");
        });
    },
    FormateReplyRecord: function (obj, ismore) {
        var me = this;
        ismore = ismore == undefined ? false : ismore;
        if (obj.CommentList==null||obj.CommentList.length == 0) return;
        var bulider = new StringBulider();
        var replyId = obj.DynamicId;
        var length = obj.CommentList.length;
        var tempLastId = 0;
        jQuery.each(obj.CommentList, function (i, n) {
            var pic = TsqFuncTool.NullStr(n.Portrait) == "" ? "/images/txDefault.png" : n.Portrait;
            bulider.append('<div class="mb-cmt msg kcmp" data-id="', n.Id, '" data-username="', n.UserName, '" data-userId="', n.UserId, '">');
            bulider.append('<a href="javascript:Global.LinkUserMain(', n.UserId, ')" class="namecard"><img  onerror="this.src=\'', me.defaultIcon, '\'" alt="头像" src="', pic, imageW30H30Q85, '" class="avatar" ></a>');
            bulider.append('<div class="msg-bd">');
            bulider.append('<div class="msg-main">');
            bulider.append('<a href="javascript:Global.LinkUserMain(', n.UserId, ')" class="mb-cmt-username namecard">', n.UserName, '：</a>');
            if (n.ReplyCommentId > 0 && n.ReplayUserId > 0) {
                bulider.append('回复 <a href="javascript:Global.LinkUserMain(', n.ReplayUserId, ')" class="mb-cmt-username namecard">', n.ReplayUserName, '：</a>')
            }
            bulider.append('<span>', n.WrapComment, '</span>');
            bulider.append('</div></div>');
            bulider.append('<div class="msg-ft">');
            bulider.append('<div class="msg-ctrl">');
            bulider.append('<a  class="msg-ctrl-cmt" data-id="', n.Id, '">回复</a>');
            if (curLoignUserId == n.UserId) {
                bulider.append('<a href="javascript:" class="msg-ctrl-del" data-id="', n.Id, '">删除</a>');
            }
            bulider.append('<span class="msg-meta">', n.AddTime, '</span>');
            bulider.append('</div></div></div>');
            if (length == (i + 1)) tempLastId = n.Id;
        });
        var lyb = this.lybList.find(".tsq_zt_bs[data-id='" + replyId + "']");
        lyb.attr("data-lastCommentId", tempLastId);
        var cmtbox = lyb.find(".mb-cmtbox").removeClass("none");
        var cmtpanel = cmtbox.find(".mb-cmtpanel");
        if (ismore) {
            var tsq_plgd_bs = cmtpanel.find(".tsq_plgd_bs");
            if (tsq_plgd_bs.length == 0) {
                cmtpanel.append("<div class=\"tsq_plgd_bs\">" + bulider.toString() + "</div>");
            }
            else {
                tsq_plgd_bs.append(bulider.toString());
            }
        }
        else {
            cmtpanel.html(bulider.toString());
        }
        var me = this;
        cmtpanel.find(".msg-ctrl-cmt").off("click").on("click", function () {
            me.BeforeReply(this, lyb);
        });
        cmtpanel.find(".msg-ctrl-del").off("click").on("click", function () {
            var obj = this;
            jQuery.dialog.confirm("确定删除吗？", function () {
                me.DelReply(obj, lyb);
            }, function () {

            });
        });
        var cmteditor = cmtbox.find(".mb-cmteditor");
        if (cmteditor.length > 0) return;
        me.CreateReplyTextArea(cmtbox, replyId);

    },
    CreateReplyTextArea: function (cmtbox, replyId) {
        var me = this;
        var textAreaStr = this.FormateReplyTextArea();
        cmtbox.append(textAreaStr);
        var shareHandler = new ShareHandler({
            textArea: cmtbox.find(".msgeditor-bd-container textarea"),
            fileDiv: cmtbox.find('.cattachqueue-wrap'),
            mbMain: cmtbox.find(".mb-cmteditor"), // .msgeditor-ft
            hdImageUrl: jQuery("#hdImageUrl"),
            replyId: replyId,
            isShowTopic: false,
            isShowGroup: false,
            callBack: function (param) { me.AddReply(param); }
        });
    },
    BeforeReply: function (obj, lyb) {
        var curObj = jQuery(obj);
        var id = curObj.attr("data-id");
        var reply = lyb.find(".mb-cmt[data-id='" + id + "']");
        var hdl = lyb.find(".mb-cmteditor .msgeditor-hd .msgeditor-hdl").show();
        var span = hdl.find("span").show();
        var em = hdl.find("em").css("display", "inline-block"); //.show();
        span.html("回复 " + reply.attr("data-username") + ":").attr("data-id", id);
    },
    FormateReplyTextArea: function () {
        var bulider = new StringBulider();
        bulider.append('<div class="mb-cmteditor msgeditor  kcmp cmpdisabled">');
        bulider.append('<div class="msgeditor-hd clearfix">');
        bulider.append('<div class="msgeditor-hdl float-l">');
        bulider.append('<span style="display: none;"></span><em style="display: none;"></em>').append('</div>');
        bulider.append('<div class="msgeditor-hdr float-r">');
        bulider.append('<i>输入超过<b>10</b>行</i><q>已超出<strong></strong>字</q><span>还可输入<em>1000</em>字</span></div>');
        bulider.append('</div>');
        bulider.append('<div class="msgeditor-bd-container">');
        bulider.append('<div class="msgeditor-bd h5uploadercontainer kcmp">');
        bulider.append('<textarea placeholder=""></textarea>');
        bulider.append('</div></div>');
        bulider.append('<div class="msgeditor-ft">');
        bulider.append('<div class="msgeditor-addons">');
        bulider.append('<a href="javascript:" class="msgeditor-addons-smiley"><i class="icon icon-smiley"></i></a>');
        //bulider.append('<a data-uploaderwidth="30" href="javascript:" class="attachbtn attachbtndoc msgeditor-addons-doc kcmp"><i class="icon icon-attach"></i></a>');
        bulider.append('</div>');
        bulider.append('<div class="msgeditor-ctrl">');
        bulider.append('<a href="javascript:" class="btn em msgeditor-ctrl-post"><em>回复</em></a>');
        bulider.append('</div></div>');
        bulider.append('<div class="cattachqueue-wrap br5 kcmp" style="display: none;">');
        bulider.append('<ul class="cattachqueue"></ul>');
        bulider.append('<ul class="cattach-img-preview clearfix"></ul>');
        bulider.append('</div></div>');
        return bulider.toString();
    },
    AddReply: function (param) {
        var me = this;
        jQuery.post("/Social/AddDynamicComment", param, function (result) {
            var dialog = jQuery.dialog;
            if (result.status>0) {
                dialog.tips(result.msg, 1.5, "success.gif");
                if (param.callBack) {
                    param.callBack();
                }
                // me.pageSize = 5;
                me.lastId = me.lybList.find(".tsq_zt_bs[data-id='" + param.replyId + "']").attr("data-lastCommentId");
                // me.FormateReplyRecord(result.comment[0], true);
                //me.GetReplyListById(param.replyId, true);
                me.GetReplyList(param.replyId);
            }
        });
    },
    DelReply: function (obj) {
        var me = this;
        var curObj = jQuery(obj);
        var commentId = curObj.attr("data-id");
        var reply = jQuery(".mb-cmt[data-id='" + commentId + "']");
        var userId = reply.attr("data-userId");
        jQuery.post("/Social/DeleteDynamicComment", {
            commentId: commentId
        }, function (result) {
            var msg = result.status;
            if (msg ==1) {
                jQuery.dialog.tips("删除成功", 1.5, "success.gif");
                reply.hide();
            }
            else {
                jQuery.dialog.tips("删除失败");
            }
        });
    }
};
function RightHandler() {
    this.Home_Admin_Widget = jQuery(".home-admin-widget");
    this.Home_Invite_Widget = jQuery(".home-invite-widget");
    this.defaultIcon = "/images/txDefault.png";
    this.role = jQuery("#hdUserRole").val();
    this.Init();
}
RightHandler.prototype = {
    Init: function () {
        this.GetAttentionCircleUsers();
        this.GetAdminWidget();
        this.GetAnnouncementList();
    },
    GetAnnouncementList: function () {
        var ran = Math.random() * 1000000;
        var me = this;
        jQuery.getJSON("/AjaxAshx/Work/Announcement/AnnouncementHandler.ashx", {
            type: "list",
            pageSize: 5,
            pageIndex: 1,
            ran: ran
        }, function (msg) {
            var bulider = new StringBulider();
            if (msg.length == 0) {
                bulider.append('<li>暂无公告，管理员可点击<a href="/Content/Work/Announcement/Announcement.aspx" class="publicLink">发布</a></li>');
            }
            else {
                jQuery.each(msg, function (i, n) {
                    var title = n.Title;
                    if (title.length> 20) title=title.substring(0,20)+"...";
                    bulider.append('<li class="bulletin-title"><a href="javascript:Global.LinkAnnouncement(', n.Id, ')">', title, '</a></li>');
                });
            }
            jQuery("#bulletin-main").html(bulider.toString());
        });
    },
    GetAttentionCircleUsers: function () {
        var ran = Math.random() * 1000000;
        var me = this;
        jQuery.get("/AjaxAshx/User/UserContacts.ashx", {
            type: "list",
            maxTime: "",
            lastId: 0,
            pageSize: 5,
            ran: ran
        }, function (msg) {
            var obj = eval('(' + msg + ')');
            var bulider = new StringBulider();
            jQuery.each(obj.data, function (i, n) {
                var pic = TsqFuncTool.NullStr(n.Icon) == "" ? me.defaultIcon : n.Icon;
                bulider.append('<li><a class="namecard" href="javascript:">');
                bulider.append('<img width="32" height="32" src="', pic, '"></a>');
                bulider.append('<div class="detail-infor">');
                bulider.append('<p class="name">', n.UserName, '</p>');
                bulider.append('<p class="ellipsis"></p>');
                bulider.append('</div></li>');
            });
            jQuery(".right .mb-widget-recently-joined ul").html(bulider.toString());
        });
    },
    GetAdminWidget: function () {
        //if (this.role < 1) return;
        var me = this;
        var ran = Math.random() * 1000000;
        jQuery.getJSON("/AjaxAshx/Circle/CircleHandler.ashx", {
            type: "circledetail",
            ran: ran
        }, function (msg) {
            if (msg == -100) {
                window.location.href = "/login.aspx";
            }
            if (msg.length > 0) {
                var n = msg[0];
                if (me.role > 0) {
                    var bulider = new StringBulider();
                    bulider.append('<h2 class="widget-hd">');
                    bulider.append('    管理<a href="/Content/Admin/CircleSet.aspx" class="widget-hd-more">更多</a></h2>');
                    bulider.append('<p>');
                    bulider.append('    今日微博总数 : <span class="mb-total-num">', n.TodayDynamicNum, '</span></p>');
                    bulider.append('<p>');
                    bulider.append('    成员总数 : <span class="member-total-num">', n.AttentionNum, '</span></p>');
                    bulider.append('<a class="admin-board-btn" target="_self" href="/Content/Admin/CircleSet.aspx">进入管理看板<em class="join-admin icon"></em></a>');
                    me.Home_Admin_Widget.html(bulider.toString());
                }
                if (n.InviteType == 0) {
                    if (me.role < 1) {
                        me.Home_Admin_Widget.hide();
                        me.Home_Invite_Widget.remove();
                    }
                }
            }
            else {
                me.Home_Admin_Widget.remove();
                me.Home_Invite_Widget.remove();
            }
        });
    }
}