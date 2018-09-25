function FuncTool() {
    
}

//阻止事件冒泡
FuncTool.prototype = {
    StopPP: function (e) {

        var evt = e || window.event;

        evt.stopPropagation ? evt.stopPropagation() : (evt.cancelBubble = true);

    },
    Trim: function (str) { //删除左右两端的空格
        if (str == undefined || str == null || str == "") return "";
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    ///去掉
    TrimBr: function (str) { //删除左右两端的换行符
        if (str == undefined || str == null || str == "") return "";
        return str.replace(/(^\n*)|(\n*$)/g, "");
    },
    ReplaceBr: function (str) {
        if (!str) return "";
        return str.replace(/(?:\r\n|\n|\r)/g, '<br>')
    },
    ReplaceStr: function (str, a, a1) {
        if (!str) return "";
        var reg = new RegExp(a, "g"); //创建正则RegExp对象 

        var newstr = str.replace(reg, a1);
        return newstr;

    },

    //截取字符串
    SubstrJS: function (str, start, len) {
        if (str.length > len) {
            return str.substr(start, len) + "...";
        } else {
            return str;
        }
    },
    IsCh: function (detail) {
        var reg = /([\u4E00-\u9FA5]|[\uFE30-\uFFA0])+/;
        return reg.test(detail);
    },
    //去掉所有的html标记
    DelHtmlTag: function (str) {
        if (!str) return "";
        return str.replace(/<\/?.+?>/g, "");
    },
    PatchTimes: function (re, s) {//参数1正则式，参数2字串
        re = eval("/" + re + "/ig")//不区分大小写，如须则去掉i,改为 re=eval("/"+re+"/g")
        var arr = s.match(re);
        if (arr == null) return 0;
        return arr.length;
    },
    //获取URL参数
    Request: function (paras) {
        var url = parent.location.href;

        var paraString = url.substring(url.indexOf("?") + 1, url.length).split("&");
        var paraObj = {}
        for (i = 0; j = paraString[i]; i++) {
            paraObj[j.substring(0, j.indexOf("=")).toLowerCase()] = j.substring(j.indexOf("=") + 1, j.length);
        }
        var returnValue = paraObj[paras.toLowerCase()];
        if (typeof (returnValue) == "undefined") {
            return "";
        } else {
            return returnValue;
        }
    },
    AddTxtCtrlFocusPos: function (id, str) {
        var obj = jQuery(("#" + id));
        var txt = obj.val();
        var pos = GetCaretPosition(obj[0]);
        if (pos == txt.length) {
            obj.focus();
            obj.val('');
            obj.val(txt + str + " ");
        }
        else {
            var substr = txt.substring(0, pos) + str;
            obj.val(substr + txt.substring(pos));
            SetCaretPositon(obj[0], substr.length);
        }
    },
    //设置input textarea 光标的位置
    SetCaretPositon: function (ctrl, pos) {
        if (ctrl.setSelectionRange) {
            ctrl.focus();
            ctrl.setSelectionRange(pos, pos);
        }
        else if (ctrl.createTextRange) {
            var range = ctrl.createTextRange();
            range.collapse(true);
            range.moveEnd('character', pos);
            range.moveStart('character', pos);
            range.select();
        }
    },
    GetCaretPosition: function (obj) {
        var result = 0;
        if (obj.selectionStart >= 0) { //非IE浏览器
            result = obj.selectionStart
        } else { //IE
            var rng;
            if (obj.tagName == "TEXTAREA") { //如果是文本域
                rng = event.srcElement.createTextRange();
                rng.moveToPoint(event.x, event.y);
            } else { //输入框
                rng = document.selection.createRange();
            }
            rng.moveStart("character", -event.srcElement.value.length);
            result = rng.text.length;
        }
        return result;
    },
    ByteLength: function (str) {
        var byteLen = 0, len = str.length;
        if (!str) return 0;
        for (var i = 0; i < len; i++)
            byteLen += str.charCodeAt(i) > 255 ? 2 : 1;
        return byteLen;
    },
    //转换容量单位
    ConvertToKBMBGB: function (length) {
        var temp = length / 1024;
        if (temp > 1024) {
            temp = temp / 1024;
        }
        else {
            return temp.toFixed(2) + "KB";
        }
        if (temp > 1024) {
            temp = temp / 1024;
        }
        else {
            return temp.toFixed(2) + "MB";
        }
        return temp.toFixed(2) + "GB";
    },
    DoubleTime: function (ms) {
        var date = new Date(ms);
        return date.getFullYear() + "-" + (date.getMonth() + 1) + "月" + date.getDate() + "日 " + date.getHours() + ":" + date.getMinutes() + ":" + date.getSeconds();
    },
    GetNowDateStr: function () {
        var isdate = arguments[0] == undefined ? false : arguments[0];
        var date = new Date();
        var month = date.getMonth() + 1;
        var day = date.getDate();
        var hour = date.getHours();
        var min = date.getMinutes();
        if (isdate) return date.getFullYear() + "-" + (month < 10 ? ("0" + month) : month) + "-" + (day < 10 ? ("0" + day) : day);
        return date.getFullYear() + "-" + (month < 10 ? ("0" + month) : month) + "-" + (day < 10 ? ("0" + day) : day) + " "
         + (hour < 10 ? ("0" + hour) : hour) + ":" + (min < 10 ? ("0" + min) : min);
    },
    GetDate: function (strDate) {
        var st = strDate;
        var a = st.split(" ");
        var b = a[0].split("-");
        var c = a[1].split(":");
        var date = new Date(b[0], b[1], b[2], c[0], c[1]);
        return date;
    },
    DateFormat: function (date, fmt) {
        if (arguments.length != 2) // 参数个数校验
            throw Error('arguments长度不合法');
        if (!date || (typeof date != 'object') || (date.constructor != Date)) // 参数合法性校验
            throw Error(arguments[0] + ':类型不为Date类型');
        if (/H+/.test(fmt) && /h+/.test(fmt))
            throw Error("小时格式错误，同类型只能连续出现一次！");
        /* 模板参数校验，正则验证方法 */
        var verify = function (Rex) {
            var arr = new RegExp(Rex).exec(fmt); // 获得匹配结果数组
            if (!arr) // 匹配失败返回
                return "";
            if (fmt.split(Rex).length > 2)  // 同一类型间隔出现多次
                throw Error("fmt格式错误：同类型只能连续出现一次！");
            return arr[0];
        };
        /**
        * 提供月、天、时、分、秒通用匹配替换
        * @param {对象o属性key} r
        * @param {r对应正则对象} rex
        **/
        var common = function (r, rex) {
            if (len != 1 && len != 2)
                throw Error("月份格式错误:M只能出现1/2次");
            len == 2 ? fmt = fmt.replace(rex, o[r].length == 1 ? "0" + o[r] : o[r]) : fmt = fmt.replace(rex, o[r]);
        }
        var o = { // 数据存储对象
            "y+": date.getFullYear() + "", // 年
            "q+": Math.floor((date.getMonth() + 3) / 3), // 季度
            "M+": date.getMonth() + 1 + "", // 月
            "d+": date.getDate() + "", // 日
            "H+": date.getHours() + "", // 24时
            "h+": date.getHours() + "", // 12时
            "m+": date.getMinutes() + "", // 分
            "s+": date.getSeconds() + "", // 秒
            "S+": date.getMilliseconds() // 毫秒
        }
        for (var r in o) {
            var rex, len, temp;
            rex = new RegExp(r);
            temp = verify(rex); // 匹配所得字符串
            len = temp.length; // 长度
            if (!len || len == 0)
                continue;
            if (r == "y+") {
                if (len != 2 && len != 4)
                    throw Error("年份格式错误:y只能出现2/4次");
                len == 2 ? fmt = fmt.replace(rex, o[r].substr(2, 3)) : fmt = fmt.replace(rex, o[r]);
            } else if (r == "q+") {
                if (len != 1)
                    throw Error("季度格式错误:q只能出现1次");
                fmt = fmt.replace(rex, o[r]);
            } else if (r == "h+") {
                if (len != 1 && len != 2)
                    throw Error("小时格式错误:h只能出现1/2次");
                var h = (o[r] > 12 ? o[r] - 12 : o[r]) + "";
                len == 2 ? fmt = fmt.replace(rex, h.length == 1 ? "0" + h : h) : fmt = fmt.replace(rex, h);
            } else if (r == "S+") {
                if (len != 1)
                    throw Error("毫秒数格式错误:S只能出现1次");
                fmt = fmt.replace(rex, o[r]);
            } else {    // (r=="M+" || r=="d+" || r=="H+" || r=="m+" || r=="s+")
                common(r, rex)
            }
        }
        return fmt;
    },
    JsonDateFormat: function (val, isShort) {
        if (val) {
            var date = new Date(parseInt(val.replace("/Date(", "").replace(")/", ""), 10));
            //月份为0-11，所以+1，月份小于10时补个0
            var month = date.getMonth() + 1 < 10 ? "0" + (date.getMonth() + 1) : date.getMonth() + 1;
            var day = date.getDate() < 10 ? "0" + date.getDate() : date.getDate();
            var h = date.getHours() < 10 ? "0" + date.getHours() : date.getHours();
            var m = date.getMinutes() < 10 ? "0" + date.getMinutes() : date.getMinutes();
            isShort = arguments[1] == undefined ? false : arguments[1];
            if (isShort) return date.getFullYear() + "-" + month + "-" + day;
            return date.getFullYear() + "-" + month + "-" + day + " " + h + ":" + m;
        }
        return "";
    },
    NullStr: function (v) {
        if (!v) return "";
        return v;
    }
}
///公用对象
var TsqFuncTool = new FuncTool();

LocalStorageUtil = function () {
    this.GetValue = function () {
        var key = arguments[0];
        return JSONUtil.decode(localStorage.getItem(key));
    };
    this.SetValue = function () {
        var key = arguments[0];
        var value = arguments[1];
        localStorage.setItem(key, JSONUtil.encode(value));
    },
    this.Remove = function () {
        var key = arguments[0];
        localStorage.removeItem(key);
    }
}
var TsqStorageUtil = new LocalStorageUtil();

function Dictionary() {
    this.data = new Array();

    this.put = function (key, value) {
        this.data[key] = value;
    };

    this.get = function (key) {
        return this.data[key];
    };

    this.remove = function (key) {
        this.data[key] = null;
    };

    this.isEmpty = function () {
        return this.data.length == 0;
    };

    this.size = function () {
        return this.data.length;
    };
}

///上传文件
function UploadFileHandler(param) {
    this.SuccessBack = param.Success;
    this.FailedBack = param.Failed;
    this.fileDiv = param.FileDiv;
    this.fileSize=param.FileSize==undefined?"10":param.FileSize;
    this.iframeId = param.IframeId == undefined ? "imgupload" : param.IframeId;
}
UploadFileHandler.prototype = {
    Init: function (param) {
        this.SuccessBack = param.Success;
        this.FailedBack = param.Failed;
        this.fileDiv = param.FileDiv;
        this.iframeId = param.IframeId == undefined ? "imgupload" : param.IframeId;
    },
    UploadClick: function () {
        var me = this;
        var contentWindow = jQuery("#" + me.iframeId)[0].contentWindow;
        jQuery(contentWindow.document).find("#filepic").trigger("click");
    },
    UploadComplete: function (data) {
        var me = this;
        jQuery("#" + me.iframeId).attr("src", "/ImageUpload/Index");
        if (me.fileDiv) {
            me.fileDiv.show();
        }
        if (this.SuccessBack) {
            this.SuccessBack(data);
        }
    },
    UploadFailed: function (error) {
        if (this.FailedBack) {
            this.FailedBack(error);
        }
        else {
            alert(error);
        }
    },
    //判断文件的大小
    FileChange: function (target) {
        var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
        var fileSize = 0;
        if (isIE && !target.files) {
            var filePath = target.value;
            var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
            var file = fileSystem.GetFile(filePath);
            fileSize = file.Size;
        } else {
            fileSize = target.files[0].size;
        }
        var size = fileSize / 1024;
        return size;
    },
    ValidateFileSize: function (target) {
        var size = this.FileChange(target);
        if (size > (this.fileSize * 1024)) {
            alert("文件大小不能超过" + this.fileSize + "M");
            return false;
        }
        else return true;
    }
}

function Global() {

}

Global.LinkUserMain = function () {
  
    window.location.href = "/user/userinfo?uid=" + arguments[0];
}

Global.LinkGroupMain = function () {
    window.location.href = "/Content/Group/CircleGroupMain.aspx?circleId=" + arguments[0];
}

Global.LinkDynamic = function () {
    window.location.href = "/Content/CircleDynamic/CircleDynamicDetail.aspx?id=" + arguments[0];
}

Global.LinkAnnouncement = function () {
    window.location.href = "/Content/Work/Announcement/AnnouncementDetail.aspx?id=" + arguments[0];
}