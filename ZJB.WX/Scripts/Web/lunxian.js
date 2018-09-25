var APF = {
    log: function(a) {}
};
APF.Namespace = {
    register: function(d) {
        var c = d.split(".");
        var a = window;
        for (var b = 0; b < c.length; b++) {
            if (typeof a[c[b]] == "undefined") {
                a[c[b]] = new Object()
            }
            a = a[c[b]]
        }
    }
};
APF.Utils = {
    getWindowSize: function() {
        var b = 0,
        a = 0;
        if (typeof(window.innerWidth) == "number") {
            b = window.innerWidth;
            a = window.innerHeight
        } else {
            if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                b = document.documentElement.clientWidth;
                a = document.documentElement.clientHeight
            } else {
                if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                    b = document.body.clientWidth;
                    a = document.body.clientHeight
                }
            }
        }
        return {
            width: b,
            height: a
        }
    },
    getScroll: function() {
        var b = 0,
        a = 0;
        if (typeof(window.pageYOffset) == "number") {
            a = window.pageYOffset;
            b = window.pageXOffset
        } else {
            if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
                a = document.body.scrollTop;
                b = document.body.scrollLeft
            } else {
                if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
                    a = document.documentElement.scrollTop;
                    b = document.documentElement.scrollLeft
                }
            }
        }
        return {
            left: b,
            top: a
        }
    },
    setCookie: function(c, e, a, h, d, g) {
        var b = new Date();
        b.setTime(b.getTime());
        if (a) {
            a = a * 1000 * 60 * 60 * 24
        }
        var f = new Date(b.getTime() + (a));
        document.cookie = c + "=" + escape(e) + ((a) ? ";expires=" + f.toGMTString() : "") + ((h) ? ";path=" + h: "") + ((d) ? ";domain=" + d: "") + ((g) ? ";secure": "")
    },
    getCookie: function(a) {
        var f = document.cookie.split(";");
        var b = "";
        var d = "";
        var e = "";
        var c = false;
        for (i = 0; i < f.length; i++) {
            b = f[i].split("=");
            d = b[0].replace(/^\s+|\s+$/g, "");
            if (d == a) {
                c = true;
                if (b.length > 1) {
                    e = unescape(b[1].replace(/^\s+|\s+$/g, ""))
                }
                return e;
                break
            }
            b = null;
            d = ""
        }
        if (!c) {
            return null
        }
    },
    deleteCookie: function(a, c, b) {
        if (this.getCookie(a)) {
            document.cookie = a + "=" + ((c) ? ";path=" + c: "") + ((b) ? ";domain=" + b: "") + ";expires=Thu, 01-Jan-1970 00:00:01 GMT"
        }
    },
    setScrollTop: function(a) {
        if (document.body) {
            document.body.scrollTop = a;
            if (document.body.scrollTop == 0) {
                if (document.documentElement) {
                    document.documentElement.scrollTop = a
                }
            }
        } else {
            if (document.documentElement) {
                document.documentElement.scrollTop = a
            }
        }
    },
    getScrollTop: function() {
        return document.body ? document.body.scrollTop || document.documentElement.scrollTop: document.documentElement.scrollTop
    },
    gotoScrollTop: function(f, d) {
        var b = APF.Utils.getScrollTop(),
        h = 0,
        g = 0;
        var d = d || 1;
        var f = f || 0;
        var a = b > f ? 1 : 0; (function() {
            b = APF.Utils.getScrollTop();
            h = a ? b - f: f - b;
            g = a ? b - h / 15 * d: b + 1 + h / 15 * d;
            APF.Utils.setScrollTop(g);
            if (h <= 0 || b == APF.Utils.getScrollTop()) {
                return
            }
            setTimeout(arguments.callee, 10)
        })()
    }
}; (function($) {
    var _aui = {};
    _aui.nameSpace = function(ns) {
        var nsParts = ns.split(".");
        var root = window;
        for (var i = 0; i < nsParts.length; i++) {
            if (typeof root[nsParts[i]] == "undefined") {
                root[nsParts[i]] = {}
            }
            root = root[nsParts[i]]
        }
    };
    _aui.inherit = function(my, classParent, args) {
        classParent.apply(my, args || []);
        $.extend(my.constructor.prototype, classParent.prototype)
    };
    _aui.Observer = function() {
        this._ob = {}
    };
    _aui.Observer.prototype.on = function(eventNames, callback) {
        var _events = eventNames.split(" ");
        var _eventKeys = {};
        for (var i = 0; i < _events.length; i++) {
            if (!this._ob[_events[i]]) {
                this._ob[_events[i]] = []
            }
            var _key = this._ob[_events[i]].push(callback);
            _eventKeys[_events[i]] = _key - 1
        }
        return _eventKeys
    };
    _aui.Observer.prototype.off = function(eventName, keys) {
        if ( !! keys && !$.isArray(keys)) {
            keys = [keys]
        }
        if (this._ob[eventName]) {
            for (var i = 0; i < this._ob[eventName].length; i++) {
                if (!keys || $.inArray(i, keys)) {
                    this._ob[eventName][i] = undefined
                }
            }
        }
    };
    _aui.Observer.prototype.trigger = function(eventName, args) {
        var r;
        if (!this._ob[eventName]) {
            return r
        }
        var _arg = args || [];
        for (var i = 0; this._ob[eventName] && i < this._ob[eventName].length; i++) {
            if (!this._ob[eventName][i]) {
                continue
            }
            var _r = this._ob[eventName][i].apply(this, _arg);
            r = (r === false) ? r: _r
        }
        return r
    };
    _aui.Observer.prototype.one = function(eventName, callback) {
        var self = this;
        var key = self.on(eventName,
        function() {
            callback.apply(this, arguments);
            self.off(eventName, key)
        })
    };
    _aui.render = function(tpl, data, op) {
        var daName = [],
        daVal = [],
        efn = [],
        _fnBuf,
        _op = $.extend({},
        _aui.render._options, op || {});
        for (var i in data) {
            daName.push(i);
            daVal.push("data." + i)
        }
        var _tp = tpl.replace(new RegExp(_op.open, "g"), _op.open + _op.val);
        _fnBuf = _tp.split(new RegExp(_op.open + "|" + _op.close, "g"));
        for (var i = 0; i < _fnBuf.length; i++) {
            if (new RegExp("^" + _op.val + _op.exp).test(_fnBuf[i])) {
                _fnBuf[i] = _fnBuf[i].replace(new RegExp("^" + _op.val + _op.exp), "")
            } else {
                if (_fnBuf[i].length > 0) {
                    if (new RegExp("^" + _op.val).test(_fnBuf[i])) {
                        _fnBuf[i] = "_buf.push(" + _fnBuf[i].replace(new RegExp("^" + _op.val), "") + ");"
                    } else {
                        _fnBuf[i] = "_buf.push('" + _fnBuf[i] + "');"
                    }
                }
            }
        }
        efn.push("(function(");
        efn.push(daName.join(","));
        efn.push("){");
        efn.push("var _buf = [];");
        efn.push(_fnBuf.join(""));
        efn.push('return _buf.join("")');
        efn.push("})(");
        efn.push(daVal.join(","));
        efn.push(")");
        return eval(efn.join(""))
    };
    _aui.render._options = {
        open: "{%",
        close: "%}",
        exp: ">",
        val: "="
    };
    _aui.nameSpace("XF");
    window.XF = _aui
})(jQuery); (function(a) {
    XF.nameSpace("XF.Switch");
    XF.Switch = function(c) {
        var b = this;
        b.op = a.extend({},
        XF.Switch._default, c);
        b._init()
    };
    XF.Switch._default = {
        switchST: "",
        clipST: ".clip",
        conST: ".con",
        itemST: ".item",
        prevST: ".prev",
        nextST: ".next",
        pnavST: ".pnav",
        effect: "slide",
        event: "click",
        current: "cur",
        circle: false,
        vertical: false,
        auto: false,
        start: 0,
        duration: 400,
        interval: 5000,
        switchNum: 1,
        clipNum: 1
    };
    XF.Switch.prototype._init = function() {
        var c = this,
        e = c.op;
        e.sw = a(e.switchST);
        e.clip = e.sw.find(e.clipST);
        e.con = e.clip.find(e.conST).css({
            position: "relative"
        });
        e.item = e.con.find(e.itemST);
        e.prev = e.prevST == ".prev" ? e.sw.find(e.prevST) : a(e.prevST);
        e.next = e.nextST == ".next" ? e.sw.find(e.nextST) : a(e.nextST);
        e.pnav = e.pnavST == ".pnav" ? e.sw.find(e.pnavST) : a(e.pnavST);
        e.itemLen = e.item.length;
        e.switchNum > e.clipNum && (e.switchNum = e.clipNum);
        e.itemLen < e.clipNum && (e.itemLen = e.clipNum);
        if (e.effect != "slide") {
            e.switchNum = 1;
            e.clipNum = 1
        }
        e.prevDisClass = a.trim(e.prevST).match(/\w\S*$/) + "-dis";
        e.nextDisClass = a.trim(e.nextST).match(/\w\S*$/) + "-dis";
        e.start = parseInt(e.start, 10);
        e.start = (e.start >= 0 && e.start < e.itemLen) ? e.start: 0;
        if (e.effect == "slide") {
            e.vertical || e.item.css({
                "float": "left"
            });
            e.leftOrTop = e.vertical ? "top": "left";
            e.widthOrHeight = e.vertical ? e.item.outerHeight(true) : e.item.outerWidth(true);
            e.conSize = e.widthOrHeight * e.itemLen;
            e.vertical ? e.con.css({
                height: e.conSize
            }) : e.con.css({
                width: e.conSize
            })
        } else {
            if (e.effect == "fade") {
                e.item.not(e.item.eq(e.start).show()).hide().css({
                    position: "absolute"
                })
            } else {
                e.item.not(e.item.eq(e.start).show()).hide();
                e.effect = "none";
                e.duration = 0
            }
        }
        function b() {
            e.timer = setInterval(function() {
                e.showpage >= e.itemLen - e.clipNum ? c.switchTo(0) : c.next()
            },
            e.interval)
        }
        function d() {
            clearInterval(e.timer)
        }
        d();
        if (e.itemLen <= e.clipNum) {
            e.stopRun = true;
            c.switchTo(0);
            return
        }
        c.switchTo(e.start);
        e.prev.off("click.switch").on("click.switch",
        function() {
            a(this).hasClass(e.prevDisClass) || c.prev()
        });
        e.next.off("click.switch").on("click.switch",
        function() {
            a(this).hasClass(e.nextDisClass) || c.next()
        });
        e.pnav.each(function(f) {
            a(this).off(e.event + ".switch").on(e.event + ".switch",
            function() {
                c.switchTo(f)
            })
        });
        if (e.auto) {
            b();
            e.sw.off("mouseenter.switch mouseleave.switch").on({
                "mouseenter.switch": function() {
                    d()
                },
                "mouseleave.switch": function() {
                    b()
                }
            })
        }
    };
    XF.Switch.prototype._play = function(d, j, g) {
        var c = this,
        h = c.op,
        f = null,
        e = {},
        b = 0;
        if (a(c).trigger("playBefore") !== false) {
            if (d === null) {
                d = j ? h.showpage - h.switchNum: h.showpage + h.switchNum
            } else {
                d = isNaN(d) ? 0 : d;
                if (d == h.showpage) {
                    return
                }
            }
            d < 0 && (d = 0);
            d > h.itemLen - h.clipNum && (d = h.itemLen - h.clipNum);
            d == 0 ? h.prev.addClass(h.prevDisClass) : h.prev.removeClass(h.prevDisClass);
            d == h.itemLen - h.clipNum ? h.next.addClass(h.nextDisClass) : h.next.removeClass(h.nextDisClass);
            for (; b < h.clipNum + h.switchNum; b++) {
                if (d + b >= h.itemLen) {
                    break
                }
                c._changeSrc(d + b)
            }
            if (h.effect == "slide") {
                e[h.leftOrTop] = -h.widthOrHeight * d;
               // h.con.stop().animate(e, h.duration)
			   h.con.stop().show();
            } else {
                if (h.effect == "fade" || h.effect == "none") {
                    f = h.item.eq(d);
                   // h.item.not(f).stop().fadeOut(h.duration);
                   //f.fadeIn(h.duration)
				   h.item.not(f).stop().hide();
				   f.show()
                }
            }
            h.pnav.removeClass(h.current);
            h.pnav.eq(Math.ceil(d / h.switchNum)).addClass(h.current);
            h.showpage = d;
            a(c).trigger("playAfter")
        }
    };
    XF.Switch.prototype._changeSrc = function(d) {
        var b = this,
        f = b.op,
        c = f.item.eq(d).find("img"),
        e = 0;
        for (; e < c.length; e++) {
            c.eq(e).attr("src") || c.eq(e).attr("src", c.eq(e).data("src"))
        }
    };
    XF.Switch.prototype.switchTo = function(b) {
        this._play(b, false, false)
    };
    XF.Switch.prototype.prev = function() {
        this._play(null, true, false)
    };
    XF.Switch.prototype.next = function() {
        this._play(null, false, true)
    }
})(jQuery)
function Fid(id) {
    return document.getElementById(id);
}
function Fempty(v) {
    if (v != null && (typeof(v) == 'object' || typeof(v) == 'function')) return false;
    return (("" == v || undefined == v || null == v) ? true: false);
}
function show_left_words(name, lorm) {
    if (lorm == 'more') {
        document.getElementById(name + 'more').style.display = "none";
        document.getElementById(name + 'less').style.display = "block";
	document.getElementById('down').style.display = "block";
    } else if (lorm == 'less') {
        document.getElementById(name + 'less').style.display = "none";
        document.getElementById(name + 'more').style.display = "block";
	document.getElementById('down').style.display = "none";
    }
}
function showMenu(baseID, divID) {
    baseID = document.getElementById(baseID);
    divID = document.getElementById(divID);
    if (showMenu.timer) clearTimeout(showMenu.timer);
    hideCur();
    divID.style.display = 'block';
    showMenu.cur = divID;
    if (!divID.isCreate) {
        divID.isCreate = true;
        divID.onmouseover = function() {
            if (showMenu.timer) clearTimeout(showMenu.timer);
            hideCur();
            divID.style.display = 'block';
        };
        function hide() {
            showMenu.timer = setTimeout(function() {
                divID.style.display = 'none';
            },
            500);
        }
        divID.onmouseout = hide;
        baseID.onmouseout = hide;
    }
    function hideCur() {
        showMenu.cur && (showMenu.cur.style.display = 'none');
    }
}
function move(direction, num, id, step) {
    var movecontent = Fid(id);
    var current = parseInt(movecontent.style.left);
    if (direction == 'left') {
        if (current <= -(num - 1) * step) {
            return false;
        }
        movecontent.style.left = current - step + 'px';
    } else {
        if (current >= 0) {
            return false;
        }
        movecontent.style.left = current + step + 'px';
    }
} (function() {
    var cur_unit_type = 0;
    var cur_unit = 0;
    if (!Fempty(Fid('huxing_title'))) {
        var as = Fid('huxing_title').getElementsByTagName('a');
        if (Fempty(as)) {
            return false;
        }
        var offset = parseInt(Fid('div_offset').value);
        for (var i = 0,
        len = as.length; i < len; i++) { (function(i) {
                as[i].onclick = function() {
                    for (var j = 0; j < len; j++) {
                        as[j].style.color = '';
                    }
                    as[cur_unit].className = "";
                    as[i].className = "on";
                    cur_unit = i;
                    var id = as[i].id;
                    if (id == 'unit_pic_show_all') {
                        Fid('pic_hx_show_right').onclick = function() {
                            move('rigth', Fid('div_num').value, 'movecontent', offset);
                        };
                        Fid('pic_hx_show_left').onclick = function() {
                            move('left', Fid('div_num').value, 'movecontent', offset);
                        };
                        if (!Fempty(Fid('movecontent_' + cur_unit_type))) {
                            Fid('movecontent_' + cur_unit_type).style.display = 'none';
                            Fid('movecontent_' + cur_unit_type).style.left = '0px';
                        }
                        Fid('movecontent').style.display = 'block';
                        cur_unit_type = 0;
                    } else {
                        var unit_type_id = id.match(/unit_pic_show_(\d+)/);
                        unit_type_id = unit_type_id[1];
                        id = 'unit_pic_show_' + unit_type_id;
                        Fid('pic_hx_show_right').onclick = function() {
                            move('rigth', Fid('div_num_' + unit_type_id).value, 'movecontent_' + unit_type_id, offset);
                        };
                        Fid('pic_hx_show_left').onclick = function() {
                            move('left', Fid('div_num_' + unit_type_id).value, 'movecontent_' + unit_type_id, offset);
                        };
                        if (cur_unit_type == 0) {
                            Fid('movecontent').style.display = 'none';
                            Fid('movecontent').style.left = '0px';
                        } else {
                            Fid('movecontent_' + cur_unit_type).style.display = 'none';
                            Fid('movecontent_' + cur_unit_type).style.left = '0px';
                        }
                        Fid('movecontent_' + unit_type_id).style.display = 'block';
                        cur_unit_type = unit_type_id;
                    }
                    return false;
                }
            })(i)
        }
    }
})()
