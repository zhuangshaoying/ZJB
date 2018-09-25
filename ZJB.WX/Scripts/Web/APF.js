var APF = {
    log: function(a) {}
};
APF.Namespace = {
    register: function(d) {
        var c = d.split(".");
        var a = window;
        for (var b = 0; b < c.length; b++) {
            if (typeof a[c[b]] == "undefined") {
                a[c[b]] = new Object();
            }
            a = a[c[b]];
        }
    }
};
APF.Utils = {
    getWindowSize: function() {
        var b = 0,
        a = 0;
        if (typeof(window.innerWidth) == "number") {
            b = window.innerWidth;
            a = window.innerHeight;
        } else {
            if (document.documentElement && (document.documentElement.clientWidth || document.documentElement.clientHeight)) {
                b = document.documentElement.clientWidth;
                a = document.documentElement.clientHeight;
            } else {
                if (document.body && (document.body.clientWidth || document.body.clientHeight)) {
                    b = document.body.clientWidth;
                    a = document.body.clientHeight;
                }
            }
        }
        return {
            width: b,
            height: a
        };
    },
    getScroll: function() {
        var b = 0,
        a = 0;
        if (typeof(window.pageYOffset) == "number") {
            a = window.pageYOffset;
            b = window.pageXOffset;
        } else {
            if (document.body && (document.body.scrollLeft || document.body.scrollTop)) {
                a = document.body.scrollTop;
                b = document.body.scrollLeft;
            } else {
                if (document.documentElement && (document.documentElement.scrollLeft || document.documentElement.scrollTop)) {
                    a = document.documentElement.scrollTop;
                    b = document.documentElement.scrollLeft;
                }
            }
        }
        return {
            left: b,
            top: a
        };
    },
    setCookie: function(c, e, a, h, d, g) {
        var b = new Date();
        b.setTime(b.getTime());
        if (a) {
            a = a * 1000 * 60 * 60 * 24;
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
                    e = unescape(b[1].replace(/^\s+|\s+$/g, ""));
                }
                return e;
                break;
            }
            b = null;
            d = "";
        }
        if (!c) {
            return null;
        }
    },
    deleteCookie: function(a, c, b) {
        if (this.getCookie(a)) {
            document.cookie = a + "=" + ((c) ? ";path=" + c : "") + ((b) ? ";domain=" + b : "") + ";expires=Thu, 01-Jan-1970 00:00:01 GMT";
        }
    },
    setScrollTop: function(a) {
        if (document.body) {
            document.body.scrollTop = a;
            if (document.body.scrollTop == 0) {
                if (document.documentElement) {
                    document.documentElement.scrollTop = a;
                }
            }
        } else {
            if (document.documentElement) {
                document.documentElement.scrollTop = a;
            }
        }
    },
    getScrollTop: function() {
        return document.body ? document.body.scrollTop || document.documentElement.scrollTop : document.documentElement.scrollTop;
    },
    gotoScrollTop: function(f, d) {
        var b = APF.Utils.getScrollTop(),
        h = 0,
        g = 0;
        var d = d || 1;
        var f = f || 0;
        var a = b > f ? 1 : 0;
        (function() {
            b = APF.Utils.getScrollTop();
            h = a ? b - f : f - b;
            g = a ? b - h / 15 * d : b + 1 + h / 15 * d;
            APF.Utils.setScrollTop(g);
            if (h <= 0 || b == APF.Utils.getScrollTop()) {
                return;
            }
            setTimeout(arguments.callee, 10);
        })();
    }
}; (function($) {
    var _aui = {};
    _aui.nameSpace = function(ns) {
        var nsParts = ns.split(".");
        var root = window;
        for (var i = 0; i < nsParts.length; i++) {
            if (typeof root[nsParts[i]] == "undefined") {
                root[nsParts[i]] = {};
            }
            root = root[nsParts[i]];
        }
    };
    _aui.inherit = function(my, classParent, args) {
        classParent.apply(my, args || []);
        $.extend(my.constructor.prototype, classParent.prototype)
    };
    _aui.Observer = function() {
        this._ob = {};
    };
    _aui.Observer.prototype.on = function(eventNames, callback) {
        var _events = eventNames.split(" ");
        var _eventKeys = {};
        for (var i = 0; i < _events.length; i++) {
            if (!this._ob[_events[i]]) {
                this._ob[_events[i]] = [];
            }
            var _key = this._ob[_events[i]].push(callback);
            _eventKeys[_events[i]] = _key - 1;
        }
        return _eventKeys;
    };
    _aui.Observer.prototype.off = function(eventName, keys) {
        if ( !! keys && !$.isArray(keys)) {
            keys = [keys];
        }
        if (this._ob[eventName]) {
            for (var i = 0; i < this._ob[eventName].length; i++) {
                if (!keys || $.inArray(i, keys)) {
                    this._ob[eventName][i] = undefined;
                }
            }
        }
    };
    _aui.Observer.prototype.trigger = function(eventName, args) {
        var r;
        if (!this._ob[eventName]) {
            return r;
        }
        var _arg = args || [];
        for (var i = 0; this._ob[eventName] && i < this._ob[eventName].length; i++) {
            if (!this._ob[eventName][i]) {
                continue;
            }
            var _r = this._ob[eventName][i].apply(this, _arg);
            r = (r === false) ? r : _r;
        }
        return r;
    };
    _aui.Observer.prototype.one = function(eventName, callback) {
        var self = this;
        var key = self.on(eventName,
            function() {
                callback.apply(this, arguments);
                self.off(eventName, key);
            });
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
            daVal.push("data." + i);
        }
        var _tp = tpl.replace(new RegExp(_op.open, "g"), _op.open + _op.val);
        _fnBuf = _tp.split(new RegExp(_op.open + "|" + _op.close, "g"));
        for (var i = 0; i < _fnBuf.length; i++) {
            if (new RegExp("^" + _op.val + _op.exp).test(_fnBuf[i])) {
                _fnBuf[i] = _fnBuf[i].replace(new RegExp("^" + _op.val + _op.exp), "");
            } else {
                if (_fnBuf[i].length > 0) {
                    if (new RegExp("^" + _op.val).test(_fnBuf[i])) {
                        _fnBuf[i] = "_buf.push(" + _fnBuf[i].replace(new RegExp("^" + _op.val), "") + ");";
                    } else {
                        _fnBuf[i] = "_buf.push('" + _fnBuf[i] + "');";
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
        return eval(efn.join(""));
    };
    _aui.render._options = {
        open: "{%",
        close: "%}",
        exp: ">",
        val: "="
    };
    _aui.nameSpace("XF");
    window.XF = _aui;
})(jQuery); (function($) {
    XF.nameSpace("XF.Vars");
    XF.nameSpace("XF.Validate");
    XF.nameSpace("XF.WindowsOpen");
    XF.Validate.phoneMobile = function(data) {
        return /^1[3|4|5|7|8]\d{9}$/.test(data);
    };
    XF.Validate.phoneArea = function(data) {
        return /^0\d{2,3}$/.test(data);
    };
    XF.Validate.phonePlane = function(data) {
        return /^[2-9]\d{6,7}$/.test(data);
    };
    XF.Validate.email = function(data) {
        return /^(\w)+(\.\w+)*@(\w)+((\.\w{2,3}){1,3})$/.test(data);
    };
    XF.WindowsOpen.redirect = function(url) {
        if (!/*@cc_on!@*/0) {
            window.open(url, "_blank");
        } else {
            var a = document.createElement("a");
            a.href = url;
            a.target = "_blank";
            document.body.appendChild(a);
            a.click()
        }
    }
})(jQuery);
(function (a) {
    a.fn.extend({
        drawLine: function(f) {
            var f, g;
            f = a.extend({
                    colors: ["#44a1f7", "#F8CE5D", "#339966"],
                    ydata: [],
                    xdata: null,
                    formatterFn: function() {
                    }
                },
                f || {});

            function b() {
                var m, l, n = [],
                    k = [];
                l = arguments.length;
                for (m = 0; m < l; m++) {
                    n.push(arguments[m]);
                }
                if (k.length === 0) {
                    k = n[0];
                }
                for (m = 1; m < l; m++) {
                    k = h(k, n[m]);
                }
                return k;
            }

            function h(t, r) {
                var n, m, l, p, o, q, s = [];
                p = t.length;
                o = r.length;
                q = p + o;
                for (n = 0, m = 0, l = 0; l < q; l++) {
                    if (n < p && (m >= o || t[n] < r[m])) {
                        s.push(t[n++]);
                    } else {;
                        s.push(r[m++]);
                    }
                }
                return s;
            }

            function c() {
                var l = f.ydata[0].data;
                var m = f.ydata[1].data;
                var o = f.ydata[2].data;
                var n = b(l, m, o);
                var k;
                k = a.inArray(0, n);
                if (k == -1) {
                    f.setMin = null;
                } else {
                    f.setMin = 0;
                }
            }

            c();
            var j = f.ydata[2].data.reverse();
            for (var d in j) {
                if (j[d] == null) {
                    continue;
                }
                j[d] = {
                    y: j[d],
                    marker: {
                        symbol: "url(/images/self-flat.png)"
                    }
                };
                break;
            }
            f.ydata[2].data = j.reverse();
            var e = new Highcharts.Chart({
                colors: f.colors,
                chart: {
                    renderTo: this.attr("id"),
                    type: "line"
                },
                title: {
                    text: ""
                },
                subtitle: {
                    text: ""
                },
                xAxis: {
                    categories: f.xdata,
                    tickmarkPlacement: "on",
                    labels: {
                        style: {
                            fontSize: "14px",
                            fontFamily: "Microsoft YaHei"
                        },
                        y: 25
                    }
                },
                yAxis: {
                    title: "",
                    gridLineColor: "#D9D9D9",
                    opposite: true,
                    labels: f.labels,
                    min: f.setMin
                },
                tooltip: {
                    crosshairs: true,
                    useHTML: true,
                    borderWidth: 2,
                    borderColor: "#999999",
                    borderRadius: 0,
                    backgroundColor: "#FFFFFF",
                    style: {
                        padding: "8px"
                    },
                    shared: true,
                    formatter: f.formatterFn
                },
                legend: {
                    enabled: false
                },
                plotOptions: {
                    line: {
                        fillOpacity: f.Opacity,
                        marker: {
                            symbol: "circle",
                            radius: 5,
                            lineWidth: 1
                        }
                    }
                },
                series: f.ydata
            });
        }
    });
})(jQuery);
XF.nameSpace("XF.Soj.send");
XF.nameSpace("XF.Soj.param");(function(a) {
    XF.nameSpace("XF.Modal");
    XF.Modal = function(c) {
        var b = this;
        XF.inherit(b, XF.Observer);
        b.op = a.extend({},
        XF.Modal._default, c);
        b.op.ie6 = /MSIE 6/.test(navigator.userAgent)
    };
    XF.Modal._default = {
        modalClass: "",
        con: "",
        hd: "",
        title: "",
        bd: "",
        ft: "",
        width: 560,
        height: "",
        pos: {
            top: undefined,
            left: undefined
        },
        mask: true,
        duration: 200
    };
    XF.Modal.prototype._create = function() {
        var j = this,
        e = j.op;
        e.modal = a('<div class="xf-modal"></div>');
        e.modalMask = a('<div class="modal-mask"></div>');
        e.modalIfr = a('<iframe class="modal-ifr"></iframe>');
        e.modalCover = a('<div class="modal-cover"></div>');
        var g = a('<div class="shadow"></div>'),
        h = a('<a href="javascript:" class="close"></a>'),
        b = a('<div class="con"></div>').append(a(e.con).show()),
        d = a('<div class="hd"></div>').append(a(e.hd).show()),
        f = a('<div class="bd"></div>').append(a(e.bd).show()),
        c = a('<div class="ft"></div>').append(a(e.ft).show()); ! e.hd && e.title && d.html('<h3 class="title">' + e.title + "</h3>");
        e.con || a().add((e.hd || e.title) && d).add(e.bd && f).add(e.ft && c).appendTo(b);
        b.add(h).appendTo(e.modal);
        e.modalClass && e.modal.addClass(e.modalClass);
        b.css({
            width: e.width,
            height: e.height
        });
        e.modalCover.append(e.modal).appendTo("body");
        h.on("click.modal",
        function() {
            j.close();
        });
        var k = a(document).height();
        e.mask && e.modalMask.css({
            height: k
        }).appendTo("body");
        e.ie6 && e.modalIfr.css({
            height: k
        }).appendTo("body");
    };
    XF.Modal.prototype.center = function(c, d) {
        var j = this,
        f = j.op,
        g = c.outerWidth(),
        b = c.outerHeight(),
        e = a(window).height();
        if (f.ie6 || b > e) {
            a("html").css({
                overflow: "hidden"
            });
            f.modalCover.css({
                overflow: "auto"
            });
        } else {
            a("html").css({
                overflow: ""
            });
            f.modalCover.css({
                overflow: "hidden"
            });
        }
        if (f.pos.top === undefined || f.pos.left === undefined) {
            var k = -parseInt(g / 2, 10),
            h = -parseInt(b / 2, 10);
            if (b > e) {
                h = -parseInt(e / 2, 10);
            }
            c.animate({
                    "margin-top": h,
                    "margin-left": k
                },
                d);
        } else {
            c.animate(f.pos, d);
        }
    };
    XF.Modal.prototype.open = function() {
        var b = this,
        e = b.op,
        d = window.undefined;
        function c(f) {
            e.ie6 && e.modalCover.css({
                top: a(window).scrollTop()
            });
            b.center(e.modal, f);
        }
        if (a(b).trigger("openBefore") !== false) { (e.modal === d) && b._create();
            c(0);
            e.modalCover.add(e.modalMask).add(e.modalIfr).css({
                visibility: "visible"
            });
            a(window).off("resize.modal").on("resize.modal",
            function() {
                e.timer && clearTimeout(e.timer);
                e.timer = setTimeout(function() {
                    c(e.duration);
                },
                    200);
            });
            a(b).trigger("openAfter");
        }
    };
    XF.Modal.prototype.close = function() {
        var b = this,
            c = b.op;
        if (a(b).trigger("closeBefore") !== false) {
            if (c.modal !== undefined) {
                c.modalCover.css({
                    overflow: "hidden"
                }).add(c.modalMask).add(c.modalIfr).css({
                    visibility: "hidden"
                });
                a("html").css({
                    overflow: ""
                });
                a(window).off("resize.modal");
                a(b).trigger("closeAfter");
            }
        }
    };
})(jQuery);(function(a) {
    XF.nameSpace("XF.Select");
    XF.Select = function(c) {
        var b = this;
        b.op = a.extend({},
        XF.Select._default, c);
        b.op.ie6 = /MSIE 6/.test(navigator.userAgent);
        b._init();
    };
    XF.Select._default = {
        selectST: ".xf-select",
        focusClass: "xf-select-fo",
        hoverClass: "option-hv",
        disableClass: "option-dis",
        adaptive: true
    };
    XF.Select.prototype._init = function() {
        var b = this,
            c = b.op;
        c.$select = a(c.selectST);
        c.$select.each(function() {
            var j = a(this),
                n = j.find(".text"),
                m = n.find("input"),
                k = n.find("span"),
                e = j.find("ul"),
                h = e.find("li").not("." + c.disableClass),
                d = h.filter("." + c.hoverClass).removeClass(c.hoverClass).eq(0);
            d.length && l(d.text(), d.data("code"));
            if (c.ie6 && c.adaptive) {
                var p = parseInt(e.css("height")),
                    g = e.css("height", "auto").outerHeight() - 2,
                    o = n.outerWidth() - parseInt(h.css("padding-left")) - 2;
                if (g > p) {
                    e.css("height", p);
                    h.css("width", o - 17);
                } else {
                    h.css("width", o);
                }
                e.css("width", n.outerWidth());
            }
            n.on("click.select",
                function(r) {
                    r.stopPropagation();
                    j.hasClass(c.focusClass) ? q() : f();
                });
            a(document).on("click.select",
                function() {
                    q();
                });
            h.on({
                mouseenter: function() {
                    a(this).addClass(c.hoverClass);
                },
                mouseleave: function() {
                    a(this).removeClass(c.hoverClass);
                },
                click: function() {
                    l(a(this).text(), a(this).data("code"));
                    //alert(a(this).data("code"));
                }
            });
            m.on({
                change: function() {
                    if (a(this).val() == "" || a(this).val() == 0 || isNaN(a(this).val())) {
                        a(this).val(80);
                    }
                    my(a(this).val());
                    //alert(a(this).data("code"));
                }
            });

            function f() {
                c.$select.not(j.addClass(c.focusClass)).removeClass(c.focusClass);
            }

            function q() {
                j.removeClass(c.focusClass);
            }

            function l(s, r) {
                if (m.val() != (r || s)) {
                    m.val(r || s);
                    k.text(s);
                    a(b).trigger("change", [m]);
                }
            }

            function my(s) {
                a(b).trigger("change", [m]);
            }
        });
    };
})(jQuery);
$.fn.extend({
    drawPie: function(b) {
        var b = $.extend({
            colors: ["#23A0E2", "#FC6300"],
            Suffix: "",
            databox: []
        },
        b || {});
        var a = new Highcharts.Chart({
            colors: b.colors,
            chart: {
                renderTo: this.attr("id"),
                type: "pie",
                height: b.height,
                backgroundColor: "#f9f9f9"
            },
            title: {
                text: ""
            },
            credits: {
                enabled: false
            },
            yAxis: {
                title: ""
            },
            tooltip: {
                enabled: false
            },
            plotOptions: {
                pie: {
                    allowPointSelect: true,
                    cursor: "pointer",
                    dataLabels: {
                        enabled: false
                    },
                    shadow: false
                }
            },
            series: b.databox
        });
    }
}); (function($) {
    XF.nameSpace("XF.View.Calculator");
    XF.nameSpace("XF.Soj.Calculator.param");
    XF.View.Calculator = function(op) {
        var self = this;
        self.totalPrice = 0;
        self.loanPrice = 0;
        self.op = $.extend({},
        XF.View.Calculator._default_settings, op);
        self.initialize();
    };
    XF.View.Calculator._default_settings = {
        valHouseType: "",
        lblTotalPrice: "",
        valLoanRatio: "",
        valLoanType: "",
        valLoanMonth: "",
        valLoanFund: "",
        valLoanBussness: "",
        lblScaleTotalPrice: "",
        subContentSclae: "",
        subContentChart: "",
        intUnitPrice: 0,
        btnStartup: "",
        intCalType: 1,
        dw: "元/平方米"
    };
    XF.View.Calculator.prototype.initialize = function() {
        var self = this,
        op = self.op;
        self.init_form(op);
    };
    XF.View.Calculator.prototype.init_form = function(option) {//初始化
        var self = this;
        var objSelect = new XF.Select({
            selectST: ".calculator-mod .xf-select"
        });
        $(objSelect).on("change",
        function(evt, input) {
     
            switch ("#" + input.attr("id")) {
            case option.valHouseType:
            case option.valLoanRatio:
                self.update_totalprice();
                break;
            case option.valLoanType:
                if (input.val() == 3) {
                    $(option.subContentSclae).show();
                    window.parent.document.getElementById("calIframe").style.height = 482 + $(option.subContentSclae).height() + "px";

                } else {
                    $(option.subContentSclae).hide();
                    window.parent.document.getElementById("calIframe").style.height = "463px";

                }
                break;
            default:
            }
        });
        self.update_totalprice();
        $(option.btnStartup).click(function() {
            if (self.check_data() == true) {
                self.calculate();
                var data = "{from:calculator}";
                var op = XF.Soj.Calculator.param;
            }
        });
        if (self.check_data() == true) {
            self.calculate();
        }
    };
    XF.View.Calculator.prototype.isNumber = function(s) {
        var regu = /^(\d*\.)?\d+$/;
        var re = new RegExp(regu);
        return re.test(s);
    };
    XF.View.Calculator.prototype.update_totalprice = function() {
        var self = this;
        var iscalculator = ($("#hdpriceunit").val() == "万元/套" ? true : false);
        var houseTypeData = $(self.op.valHouseType).val();
		self.op.intUnitPrice = self.op.intUnitPrice;
		var strTotalPrice = "";
        if (self.op.dw == "1") {

            self.totalPrice = self.op.intUnitPrice;
            var jj= Math.round(self.op.intUnitPrice*10000/houseTypeData);//均价

            self.loanPrice = Math.round(self.op.intUnitPrice * $(self.op.valLoanRatio).val() / 100);

             strTotalPrice = "<p><strong>" + self.op.intUnitPrice + "</strong>万元<span>（均价<em>" + jj + "</em>" + $("#hdpriceunit").val() + "）</span></p>";
            

            var strLoanPrice = "（贷款总额" + self.loanPrice + "万）";
        $(self.op.lblTotalPrice).html(strTotalPrice);
        $(self.op.lblScaleTotalPrice).html(strLoanPrice);
        } else {
                 self.totalPrice = Math.round(houseTypeData * self.op.intUnitPrice / 10000);
                 self.loanPrice = Math.round(self.totalPrice * $(self.op.valLoanRatio).val() / 100);
            if (iscalculator) {
                strTotalPrice = "<p><strong>" + self.op.intUnitPrice + "</strong>万元<span></span></p>";
            } else {
                strTotalPrice = "<p><strong>" + self.totalPrice + "</strong>万元<span>（均价<em>" + self.op.intUnitPrice + "</em>" + $("#hdpriceunit").val() + "）</span></p>";
                
            }
            var strLoanPrice = "（贷款总额" + self.loanPrice + "万）";
        $(self.op.lblTotalPrice).html(strTotalPrice);
        $(self.op.lblScaleTotalPrice).html(strLoanPrice);
        }
   
    };
    XF.View.Calculator.prototype.check_data = function(bolDispErr) {
        var self = this,
        intResult = true;
        if ($(self.op.valHouseType).val() <= 0) {
            intResult = false;
        }
        if ($(self.op.valLoanType).val() == 3) {
            var FunVal = $(self.op.valLoanFund).val().replace(/(^\s*)|(\s*$)/g, "");
            var BussnessVal = $(self.op.valLoanBussness).val().replace(/(^\s*)|(\s*$)/g, "");
            if (FunVal != "" && !self.isNumber(FunVal)) {
                $(self.op.valLoanFund).addClass("int-err");
                $(self.op.valLoanFund).parent().find(".com-msg").css("display", "inline-block").find("span").text("金额错误!");
                $(self.op.valLoanBussness).removeClass("int-err");
                $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "none");
                return false;
            }
            if (BussnessVal != "" && !self.isNumber(BussnessVal)) {
                $(self.op.valLoanBussness).addClass("int-err");
                $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "inline-block").text("金额错误!");
                $(self.op.valLoanFund).removeClass("int-err");
                $(self.op.valLoanFund).parent().find(".com-msg").css("display", "none");
                return false;
            }
            if (FunVal == "" && BussnessVal == "") {
                $(self.op.valLoanFund).addClass("int-err");
                $(self.op.valLoanFund).parent().find(".com-msg").css("display", "inline-block").find("span").text("不能为空！");
                $(self.op.valLoanBussness).addClass("int-err");
                $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "inline-block").find("span").text("不能为空！");
                intResult = false;
            } else {
                if (FunVal != "" && BussnessVal != "") {
                    if (parseFloat(FunVal) + parseFloat(BussnessVal) != self.loanPrice) {
                        $(self.op.valLoanFund).addClass("int-err");
                        $(self.op.valLoanFund).parent().find(".com-msg").css("display", "inline-block").find("span").text("总额错误！");
                        $(self.op.valLoanBussness).addClass("int-err");
                        intResult = false;
                    } else {
                        $(self.op.valLoanFund).removeClass("int-err");
                        $(self.op.valLoanFund).parent().find(".com-msg").css("display", "none");
                        $(self.op.valLoanBussness).removeClass("int-err");
                        $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "none");
                    }
                } else {
                    if (FunVal != "" && BussnessVal == "") {
                        if (self.loanPrice - parseFloat(FunVal) < 0) {
                            $(self.op.valLoanFund).addClass("int-err");
                            $(self.op.valLoanFund).parent().find(".com-msg").css("display", "inline-block").find("span").text("金额错误!");
                            $(self.op.valLoanBussness).addClass("int-err");
                            intResult = false;
                        } else {
                            $(self.op.valLoanFund).val(parseFloat(FunVal).toFixed(0));
                            $(self.op.valLoanBussness).val((self.loanPrice - parseFloat(FunVal)).toFixed(0));
                            $(self.op.valLoanFund).removeClass("int-err");
                            $(self.op.valLoanFund).parent().find(".com-msg").css("display", "none");
                            $(self.op.valLoanBussness).removeClass("int-err");
                            $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "none");
                        }
                    } else {
                        if (BussnessVal != "" && FunVal == "") {
                            if (self.loanPrice - parseFloat(BussnessVal) < 0) {
                                $(self.op.valLoanBussness).addClass("int-err");
                                $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "inline-block").text("金额错误!");
                                $(self.op.valLoanFund).addClass("int-err");
                                intResult = false;
                            } else {
                                $(self.op.valLoanBussness).val(parseFloat(BussnessVal).toFixed(0));
                                $(self.op.valLoanFund).val((self.loanPrice - parseFloat(BussnessVal).toFixed(0)).toFixed(0));
                                $(self.op.valLoanFund).removeClass("int-err");
                                $(self.op.valLoanFund).parent().find(".com-msg").css("display", "none");
                                $(self.op.valLoanBussness).removeClass("int-err");
                                $(self.op.valLoanBussness).parent().find(".com-msg").css("display", "none");
                            }
                        }
                    }
                }
            }
        }
        return intResult;
    };
    XF.View.Calculator.prototype.calculate = function() {
        var self = this;
        var rates = {
            "date": "2015\u5e745\u670811\u65e5",
            "commerce": {
                "1": {
                    "title": "\u9996\u5957\u623f\u4f18\u60e0(15%)\u5229\u7387",
                    "rate": {
                        "1": "0.04335",
                        "4": "0.04675",
                        "6": "0.048025"
                    },
                    "selected": false
                },
                "2": {
                    "title": "\u57fa\u51c6\u5229\u7387",
                    "rate": {
                        "1": "0.0510",
                        "4": "0.0550",
                        "6": "0.0490"
                    },
                    "selected": true
                },
                "3": {
                    "title": "\u7b2c\u4e8c\u5957\u623f\u4e0a\u6d6e\u5229\u7387",
                    "rate": {
                        "1": "0.0561",
                        "4": "0.0605",
                        "6": "0.06215"
                    },
                    "selected": false
                }
            },
            "fund": {
                "rate": {
                    "1": "0.03250",
                    "6": "0.03250"
                }
            }
        };
        var intLoanBussiness = 0;
        var intLoanFund = 0;
        var intRateBussiness = 0;
        var intRateFund = 0;
        var intMonths = $(self.op.valLoanMonth).val();
        var floatMonthlyRepayBuss = 0;
        var floatMonthlyRepayFund = 0;
        var floatRateFund = rates.fund["rate"]["6"];
        var floatRateBussiness = eval("rates['commerce']['2']['rate']['6']");
        if (intMonths <= 5 * 12) {
            floatRateFund = rates.fund["rate"]["1"];
            floatRateBussiness = eval("rates['commerce']['2']['rate']['4']")
        }
        if (intMonths <= 3 * 12) {
            floatRateBussiness = eval("rates['commerce']['2']['rate']['1']")
        }
        var floatMonthlyRateFund = floatRateFund / 12;
        var floatMonthlyRateBussiness = floatRateBussiness / 12;
        switch ($(self.op.valLoanType).val()) {
        case "1":
            intLoanFund = 0;
            intLoanBussiness = self.loanPrice;
            break;
        case "2":
            intLoanFund = self.loanPrice;
            intLoanBussiness = 0;
            break;
        case "3":
            intLoanFund = $(self.op.valLoanFund).val();
            intLoanBussiness = $(self.op.valLoanBussness).val();
            break;
        }
        floatMonthlyRepayFund = 10000 * intLoanFund * floatMonthlyRateFund * [Math.pow((1 + floatMonthlyRateFund), intMonths)] / [Math.pow((1 + floatMonthlyRateFund), intMonths) - 1];
        floatMonthlyRepayBuss = 10000 * intLoanBussiness * floatMonthlyRateBussiness * [Math.pow((1 + floatMonthlyRateBussiness), intMonths)] / [Math.pow((1 + floatMonthlyRateBussiness), intMonths) - 1];
        var arrResult = {
            intMonthlyRepay: Math.round(floatMonthlyRepayFund + floatMonthlyRepayBuss),
            intFirstPay: Math.round((self.totalPrice - self.loanPrice)),
            intLoanPrice: self.loanPrice,
            floatInterest: parseFloat(((floatMonthlyRepayFund + floatMonthlyRepayBuss) * intMonths / 10000 - self.loanPrice).toFixed(1)),
            intFirstPayRatio: 10 - $(self.op.valLoanRatio).val() / 10,
            intLoanRatio: $(self.op.valLoanRatio).val() / 10,
            floatRateFund: (floatRateFund * 100).toFixed(2),
            floatRateBussiness: (floatRateBussiness * 100).toFixed(2)
        };
        this.render_result(arrResult);
    };
    XF.View.Calculator.prototype.render_result = function(objResult) {
        var self = this;
        $(self.op.subContentChart).parent().find(".price").html(objResult.intMonthlyRepay);
        $(self.op.subContentChart).parent().find(".legend-pay").find("span").html("参考首付：" + (objResult.intFirstPay) + "万（" + objResult.intFirstPayRatio + "成）");
        $(self.op.subContentChart).parent().find(".legend-price").find("span").html("贷款金额：" + objResult.intLoanPrice + "万（" + objResult.intLoanRatio + "成）");
        $(self.op.subContentChart).parent().find(".legend-rate").find("span").html("支付利息：" + objResult.floatInterest + "万<em><br />（利率公积金" + objResult.floatRateFund + "%，商业性" + objResult.floatRateBussiness + "%）</em>");
        var arrPieData = [{
            name: "参考首付",
            y: objResult.intFirstPay,
            events: {
                mouseOver: function() {
                    $(".legend-pay").addClass("curr");
                },
                mouseOut: function() {
                    $(".legend-pay").removeClass("curr");
                }
            }
        },
        {
            name: "贷款金额",
            y: objResult.intLoanPrice,
            events: {
                mouseOver: function() {
                    $(".legend-price").addClass("curr");
                },
                mouseOut: function() {
                    $(".legend-price").removeClass("curr");
                }
            }
        },
        {
            name: "支付利息",
            y: objResult.floatInterest,
            events: {
                mouseOver: function() {
                    $(".legend-rate").addClass("curr");
                },
                mouseOut: function() {
                    $(".legend-rate").removeClass("curr");
                }
            }
        }];
        $(self.op.subContentChart).drawPie({
            colors: ["#2F69BF", "#A2BF2F", "#BF5A2F"],
            databox: [{
                data: arrPieData,
                innerSize: "47%"
            }]
        });
        if (objResult.intFirstPay == 0) {
            $(self.op.subContentChart).html("");
        }
    }
})(jQuery); (function (a) {
    //alert($(".calculator-mod").find(".tools-item5 input.int-text").html());
    $(".calculator-mod").find(".tools-item5 input.int-text").on("keyup.calcu",
        function () {
            $(this).val($(this).val().replace(/[^0-9]/, ""));
        });
})(jQuery);