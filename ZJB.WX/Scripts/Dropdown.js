function DropdownUtil(param) {
    this._autoShow = param.autoShow;
    this._store = param.store || [];
    this._textFormat = param.textFormat || "{text}";
    this._valueFormat = param.valueFormat || "{id}";
    this._title = param.title;
    this._info = param.info;
    this._pos = param.position;
    this._onSelect = param.onSelect || __noop;
    this._resetZIndex = (typeof param.resetZIndex === "undefined") ? true : param.resetZIndex;
    this._type = param.type;
    this._render();
}

DropdownUtil.prototype = {
    $: jQuery,
    ITEM_FOCUS_CLS: "hover",
    _$currItem: null,
    _undef: function (a) {
        return typeof a === "undefined"
    },
    _tpl: function () {
        var builder = new StringBulider(); //拼接字符串
        builder.append('<div class="dropdown-layer">');
        if (!this._undef(this._title)) {
            builder.append("<h2>", this._title, "</h2>")
        }
        if (!this._undef(this._info)) {
            builder.append("<p>", this._info, "</p>")
        }
        builder.append("<ul></ul>").append("</div>");
        return builder.toString();

    },
    _topicsItemsTpl: function () {
        var builder = new StringBulider(); //拼接字符串
        var i = 0; length = this._store.length;
        for (i = 0; i < length; i++) {
            builder.append('<li data-index="', i, '" class="dropdown-topic-item cxl_tsq_dropdown">', this._store[i]);
//            if (this._store[i].Activity > 0) {
//                builder.append(" (", this._store[i].Activity, ")")
//            }
            builder.append("</li>")
        }
        return builder.toString();
    },
    _itemsTpl: function () {
        var builder = new StringBulider(); //拼接字符串
        var me = this;
        var i = 0; length = this._store.length;
        var className = "dropdown-group-split cxl_tsq_dropdown";
        for (var i = 0; i < length; i++) {
            var e = me._store[i];
            var pic = e.Icon;
            if (!pic) pic = "/Images/txDefault.png";
            builder.append('<li data-index="', i, '" class="', className, '">').append('<img class="dropdown-item-img" src="', pic, '" />');
            builder.append('<ul class="dropdown-item-info">').append('<li class="dropdown-item-info-section1"><span class="dropdown-name ellipsis">', e.NickName, "", '</span><br/><span class="dropdown-jobtitle ellipsis">', e.Phone, '</span></li>');
            builder.append('<li class="dropdown-item-info-section2"><span class="dropdown-dept">', e.Dept, '</span></li>').append("</ul>");
            builder.append("</li>");
        }
        return builder.toString();
    },
    _render: function () {
        var b = this.$,
			a = this;
        this.$root = this.$(this._tpl()).appendTo("body").delegate(".cxl_tsq_dropdown", "click mouseover mouseout", function (d) {
            var c = b(this);
            switch (d.type) {
                case "click":
                    a.select();
                    break;
                case "mouseover":
                    a._$currItem.removeClass(a.ITEM_FOCUS_CLS);
                    c.addClass(a.ITEM_FOCUS_CLS);
                    a._$currItem = c;
                    break
            }
        }).click(function () {
            return false
        });
        this.$list = this.$root.find("ul").first();
        this.$info = this.$root.find("p").first();
        this._renderItems();
        this.position(this._pos);
        if (this._autoShow === true) {
            this.show()
        }
    },
    _renderItems: function () {
        var a;
        this.$list.empty();
        if (this._store.length != 0) {
            if (this._type == "topic") {
                a = this._topicsItemsTpl()
            } else {
                a = this._itemsTpl()
            }
            this._$items = this.$(a).appendTo(this.$list);
            this._$currItem = this._$firstItem = this._$items.first().addClass(this.ITEM_FOCUS_CLS);
            this._$lastItem = this._$items.last()
        }
    },
    _navigate: function (b) {
        if (this._store.length == 0) {
            return
        }
        var a;
        if (b === true) {
            if (this._$currItem.is(":last-child")) {
                a = this._$firstItem
            } else {
                a = this._$currItem.next()
            }
        } else {
            if (this._$currItem.is(":first-child")) {
                a = this._$lastItem
            } else {
                a = this._$currItem.prev()
            }
        }
        this._$currItem.removeClass(this.ITEM_FOCUS_CLS);
        a.addClass(this.ITEM_FOCUS_CLS);
        this._$currItem = a
    },
    onSelect: function (a) {
        this._onSelect = a
    },
    select: function () {
        var a = this._$currItem == null ? null : this._store[this._$currItem.data("index")];
        this._onSelect(a);
        this.hide()
    },
    info: function (a) {
        if (a === false && this.$info.length != 0) {
            this.$info.hide();
            return
        }
        if (this.$info.length == 0) {
            this.$info = this.$("<p>" + a + "</p>").insertBefore(this.$list)
        } else {
            this.$info.html(a)
        }
        this.$info.show()
    },
    store: function (a) {
        if (this._undef(a)) {
            return this._store
        }
        this._store = a;
        this._renderItems()
    },
    position: function (c) {
        if (this._undef(c)) {
            return this._pos
        }
        this._pos = c;
        if (this._undef(this._pos.rel)) {
            this.$root.css({
                left: this._pos.x + "px",
                top: this._pos.y + "px"
            })
        } else {
            var a = this.$(this._pos.rel),
				b = a.offset();
            this.$root.css({
                left: b.left + this._pos.x + "px",
                top: b.top + this._pos.y + "px"
            })
        }
        if (this._resetZIndex === true) {
            this.$root.css("z-index", 99999);
        }
    },
    show: function (b) {
        var a = this;
        this.position(b);
        this.$root.show();
        this.$(document).one("click", function () {
            a.hide()
        })
    },
    hide: function () {
        this.$root.hide();
        this._store = [];
        this.$list.empty()
    },
    isVisible: function () {
        return this.$root.is(":visible")
    },
    up: function () {
        this._navigate(false)
    },
    down: function () {
        this._navigate(true)
    },
    title: function (a) {
        this.$(".dropdown-layer h2").title(a)
    }

}
function TextChangeUtil(param) {
    this.input = param.input;
    this.btn = param.btn;
    this.type = "";
    this.isShow = false;
    this.topicFlag = "#";
    this.isShowTopic = param.isShowTopic;
    this.callBack = param.callBack;
    this.queryType = param.queryType == undefined ? "search" : param.queryType;
    this.aFlag = "@";
    this.cursorPosition = 0;
    this.cursorSearch = "";
    this.info = "未找到匹配项";
    this.topicDropdown;
    this.auserDropdown;
    this.writeType = 0;
    this.timeUserNameList;
    this.timeTopicList;
    this.curUserName = "";
    this.aUserArr = [];
}
TextChangeUtil.prototype = {
    Change: function () {
        var me = this;
        var v = me.input.val();
        var pos = me.input.getCaretPosition();
        var position = { x: pos.left, y: pos.top + 5, rel: me.input };
        if (me.IsAuser()) {
            me.type = "auser";
            me.isShow = true;
        }
        else {
            me.isShow = false;
        }
        if (me.isShow) {
            if (typeof me.auserDropdown == "undefined") {
                me.auserDropdown = new DropdownUtil({
                    autoShow: true,
                    store: [],
                    title: "@用户",
                    //info: "未找到匹配项",
                    position: position,
                    type: me.type,
                    onSelect: function (obj) {
//                         else {
                            me.OnUserSelect(obj);
//                        }
                    }
                });
            }
            clearTimeout(me.timeUserNameList);
            me.timeUserNameList = setTimeout(function () { me.GetAuserList(position); }, 500);
            return;
        }
        else {
            if (!(typeof me.auserDropdown == "undefined")) {
                me.auserDropdown.hide();
            }
        }
        if (!me.isShowTopic) return;
        if (me.IsTopic()) {
            me.type = "topic";
            me.isShow = true;
        }
        else {
            me.isShow = false;
        }
        if (me.isShow) {
            if (typeof me.topicDropdown == "undefined") {
                me.topicDropdown = new DropdownUtil({
                    autoShow: true,
                    store: [],
                    title: "想用什么话题？",
                    //info: "未找到匹配项",
                    position: position,
                    type: me.type,
                    onSelect: function (obj) { me.OnSelect(obj); }
                });
            }
            clearTimeout(me.timeTopicList);
            timeTopicList = setTimeout(function () { me.GetTopicList(position) }, 500);
        }
        else {
            if (!(typeof me.topicDropdown == "undefined")) {
                me.topicDropdown.hide();
            }
        }
    },
    //@用户列表
    GetAuserList: function (position) {
        var me = this;
        if (me.cursorSearch.length > 10) {
            return;
        }
        jQuery.getJSON("/AjaxAshx/User/AuserHandler.ashx", { type: me.queryType, key: me.cursorSearch.replace("@", "") },
            function (msg) {
                me.auserDropdown.store(msg);
                if (msg.length == 0) {
                    me.auserDropdown.info(me.info);
                }
                else {
                    me.auserDropdown.info("");
                }
                me.auserDropdown.show(position);
            }
         );
    },
    //话题列表
    GetTopicList: function (position) {
        var me = this;
        var ran = Math.random() * 1000000;
        var type = "search";
        if (me.cursorSearch == undefined || me.cursorSearch == "") {
            //me.topicDropdown.store([]);
            //me.topicDropdown.show(position);
            type = "hotlist";
            // return;
        }
        if (me.cursorSearch.length > 20) {
            return;
        }
        jQuery.getJSON("/AjaxAshx/Topic/TopicHandler.ashx", { type: type, keyword: me.cursorSearch, ran: ran },
            function (msg) {
                me.topicDropdown.store(msg);
                if (msg.length == 0) {
                    me.topicDropdown.info(me.info);
                }
                else {
                    me.topicDropdown.info("");
                }
                me.topicDropdown.show(position);
            }
        );
    },
    IsAuser: function () {
        var me = this;
        var v = me.input.val();
        var length = v.length;
        if (length == 0) return false;
        me.cursorPosition = me.input.getCursorPosition();
        var tempStr = v.substring(0, me.cursorPosition);
        var lastIndex = tempStr.lastIndexOf(me.aFlag);
        if (lastIndex < 0) return false;
        me.cursorSearch = v.substring(lastIndex, me.cursorPosition);
        if (me.curUserName != "") {
            if (me.cursorSearch.indexOf(me.curUserName) > -1) {
                me.auserDropdown.hide();
                return false;
            }
        }
        else {
            var emptyStrIndex = me.cursorSearch.indexOf(' ');
            if (me.cursorSearch != "" && me.cursorSearch.length > 1 && emptyStrIndex > 0) {
                me.curUserName = me.cursorSearch.substring(0, emptyStrIndex);
                me.auserDropdown.hide();
                return false;
            }
        }
        return true;
    },
    IsTopic: function () {
        var me = this;
        var v = me.input.val();
        var length = v.length;
        if (length == 0) return false;
        var topicFlagCount = TsqFuncTool.PatchTimes(me.topicFlag, v);
        if (topicFlagCount == 0) return false;
        var lastIndenxOf = v.lastIndexOf(me.topicFlag) + 1;
        me.cursorPosition = me.input.getCursorPosition();
        if (length == me.cursorPosition) {
            if (topicFlagCount % 2 != 0) {
                me.cursorSearch = v.substring(lastIndenxOf, me.cursorPosition);
                me.writeType = 1;
                return true;
            }
        }
        else {
            if (me.cursorPosition > 1) {
                if (me.cursorPosition < lastIndenxOf) {
                    var arr = me.GetFlagArr(v.substring(0, lastIndenxOf));
                    for (var i = 0; i < arr.length; i++) {
                        if (me.cursorPosition > arr[i].start && me.cursorPosition <= arr[i].end) {
                            me.cursorSearch = v.substring(arr[i].start, me.cursorPosition);
                            if (me.cursorSearch.indexOf(me.aFlag) >= 0) return false;
                            me.writeType = 2;
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    },
    GetFlagArr: function (v) {
        var me = this;
        var length = v.length; //{start:1,end:7}
        var arr = [];
        var count = 0;
        var obj = {};
        for (var i = 0; i < length; i++) {
            var ch = v[i];
            if (ch == me.topicFlag) {
                count++;
                if (count == 1) obj.start = i + 1;
                else obj.end = i + 1;
                if (count % 2 == 0) {
                    count = 0;
                    arr.push(obj);
                    obj = {};
                }
                if (length % 2 != 0 && i == length - 1) {
                    obj.end = me.cursorPosition;
                }
            }
        }
        return arr;
    },
    ///话题选择
    OnSelect: function (obj) {
        var me = this;
        var v = obj;
        if (jQuery.isPlainObject(obj)) {
            v = obj.Name;
        }
        if (me.writeType == 1) {
            v = v.substring(1, v.length)
            v = v.replace(me.cursorSearch, "");
        }
        else {
            v = v.substring(1, v.length - 1);
            v = v.replace(me.cursorSearch, "");
        }
        this.input.setValueAutoFocus(v + " ");
    },
    OnUserSelect: function (obj) {
        var me = this;
        var v = obj.NickName;
        me.curUserName = v;
        v = v.replace(me.cursorSearch.replace(me.aFlag, ""), "");
        me.input.setValueAutoFocus(v + " ");
        if (!me.IsAuserSelected(obj.UserId)) {
            me.aUserArr.push(obj);
        }
        if (me.callBack) {
            me.callBack(obj);
        }
    },
    IsAuserSelected: function (userId) {
        for (var i = 0; i < this.aUserArr.length; i++) {
            if (this.aUserArr[i].UserId == userId) return true;
        }
        return false;
    },
    InitaUserArr:function(){
        var v=arguments[0]==undefined?[]:arguments[0];
        this.aUserArr=v;
    },
    GetAUserIds: function () {
        var arr = [];
        var content = this.input.val();
        for (var i = 0; i < this.aUserArr.length; i++) {
            var obj = this.aUserArr[i];
            if (content.indexOf("@" + obj.NickName + " ") >= 0) {
                arr.push(obj.UserId);
            }
        }
        return arr.join();
    },
    GetAUserNames: function () {
        var arr = [];
        var content = this.input.val();
        for (var i = 0; i < this.aUserArr.length; i++) {
            var obj = this.aUserArr[i];
           // if (content.indexOf("@" + obj.NickName + " ") >= 0) {
                arr.push("@" + obj.NickName + " ");
           // }
        }
        return arr.join("");
    },
    GetAUsers: function () {
        var arr = [];
        var content = this.input.val();
        for (var i = 0; i < this.aUserArr.length; i++) {
            var obj = this.aUserArr[i];
            if (content.indexOf("@" + obj.NickName + " ") >= 0) {
                arr.push(obj);
            }
        }
        return arr;
    }
}
