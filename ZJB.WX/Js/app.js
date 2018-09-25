$(function() {
    var a, b, c, d, e;
    return a = $("body"),
    d = $(".modal-back"),
    b = $(".modal-card"),
    c = $(".modal-back, .cancel-handler, .close-handler"),
    $(".modal-handler").on("click",
    function(c) {
        return c.preventDefault(),
        a.addClass("open-modal"),
        $(".confirm-handler").html(teambitionI18n.confirm),
        b.delay(100).fadeIn(250),
        d.fadeIn(350)
    }),
    c.on("click",
    function() {
        return e()
    }),
    a.on("keydown",
    function(a) {
        return 27 === a.keyCode ? e() : void 0
    }),
    e = function() {
        return b.animate({
            opacity: 0,
            top: "-=200"
        },
        150,
        function() {
            return b.attr({
                style: "display:none"
            })
        }),
        d.delay(100).fadeOut(350,
        function() {
            return a.removeClass("open-modal")
        })
    }
}),
$(function() {
    return $(".setlocale").on("click",
    function(a) {
        var b, c;
        return a.preventDefault(),
        c = $(this).data("locale"),
        b = {
            path: "/",
            domain: "." + location.hostname.split(".").slice(1).join(".")
        },
        $.cookie("lang", c, b),
        location.href = $(".tbsite-article").length ? this.href + "?p=setlocale&" + location.search.match(/_id=([^&]*)/)[0] : this.href + "?p=setlocale"
    })
}),
$(function() {
    var a, b, c, d;
    return d = $(".tbsite-article"),
    a = {
        appid: "",
        title: document.title,
        desc: $('meta[name="description"]').attr("content"),
        image: $(".navbar-brand").data("circle"),
        link: location.href,
        callback: function() {}
    },
    d.length ? (b = d.find(".modal-wechat .qrcode"), b.qrcode({
        text: b.data("src"),
        width: 250,
        height: 250
    }), c = $.extend({},
    a, {
        title: d.find(".title").text(),
        desc: b.data("desc"),
        image: d.find(".topbanner").data("bg"),
        link: b.data("src")
    })) : c = a,
    $(document).on("WeixinJSBridgeReady",
    function() {
        return WeixinJSBridge.on("menu:share:appmessage",
        function() {
            return WeixinJSBridge.invoke("sendAppMessage", {
                appid: c.appid,
                title: c.title,
                desc: c.desc,
                img_url: c.image,
                link: c.link
            },
            c.callback)
        }),
        WeixinJSBridge.on("menu:share:timeline",
        function() {
            return WeixinJSBridge.invoke("shareTimeline", {
                title: c.title,
                desc: c.desc,
                img_url: c.image,
                link: c.link
            },
            c.callback)
        })
    })
}),
$(function() {
    var a, b;
    return a = $(".csr-form #submit"),
    b = !1,
    $(".csr-form .type").on("click",
    function() {
        return $(".type").removeClass("active"),
        $(this).addClass("active")
    }),
    a.on("click",
    function(c) {
        var d;
        return c.preventDefault(),
        d = {
            email: $("#teambition-account").val(),
            type: $(".active").data("type"),
            name: $("#organization-name").val(),
            description: $("#organization-description").val(),
            contact: $("#organization-contact-info").val()
        },
        b === !1 ? $.post("/api/philanthropy", d,
        function() {
            return a.html(teambitionI18n.applySuccess + ' <img src="https://dn-site.oss.aliyuncs.com/v0.3.x/images/csr/heart@2x.png">'),
            b = !0
        }).fail(function() {
            return a.html(teambitionI18n.pleaseRetry + ' <img src="https://dn-site.oss.aliyuncs.com/v0.3.x/images/csr/heart@2x.png">')
        }) : a.html(teambitionI18n.submitted + ' <img src="https://dn-site.oss.aliyuncs.com/v0.3.x/images/csr/heart@2x.png">').addClass("disabled")
    }),
    $(".csr-form input, .csr-form textarea").click(function() {
        return a.html(teambitionI18n.applyNow + ' <img src="https://dn-site.oss.aliyuncs.com/v0.3.x/images/csr/heart@2x.png">')
    })
}),
$(function() {
    var a, b, c;
    return b = $(".deploy-form"),
    a = b.find(".confirm-handler"),
    c = !1,
    a.on("click",
    function(b) {
        var d;
        return b.preventDefault(),
        d = {
            type: "deployment",
            companyName: $("#company-name").val(),
            contactName: $("#contact-name").val(),
            contact: $("#contact-phone").val()
        },
        c === !1 ? $.post("/api/philanthropy", d,
        function() {
            return a.html(teambitionI18n.applySuccess),
            c = !0
        }).fail(function() {
            return a.html(teambitionI18n.pleaseRetry)
        }) : a.html(teambitionI18n.submitted).addClass("disabled")
    }),
    b.find(".form-input").click(function() {
        return a.html(teambitionI18n.confirm)
    })
}),
$(function() {
    var a, b, c, d, e, f;
    return d = $("#incubator-name-sh").html(),
    c = $("#incubator-name-bj").html(),
    e = $("#incubator-name-sz").html(),
    b = $("#incubator-name"),
    a = $(".incubator-form #submit-handler"),
    f = !1,
    b.html(d),
    $("#organization-city").change(function() {
        return b.empty(),
        b.html(0 === this.selectedIndex ? d: 1 === this.selectedIndex ? c: e)
    }),
    a.on("click",
    function(b) {
        var c;
        return b.preventDefault(),
        c = {
            type: "incubator",
            email: $("#teambition-account").val(),
            name: $("#organization-name").val(),
            city: $("#organization-city").val(),
            incubatorName: $("#incubator-name").val(),
            description: $("#organization-desc").val(),
            contactName: $("#organization-contact-name").val(),
            contact: $("#organization-contact-info").val()
        },
        f === !1 ? $.post("/api/philanthropy", c,
        function() {
            return a.html(teambitionI18n.applySuccess),
            f = !0
        }).fail(function() {
            return a.html(teambitionI18n.pleaseRetry)
        }) : a.html(teambitionI18n.submitted).addClass("disabled")
    }),
    $(".incubator-form .form-input").click(function() {
        return $("#submit-handler").html(teambitionI18n.applyNow)
    })
}),
$(function() {
    var a, b, c, d;
    return c = $(".research-form"),
    b = c.find(".input-group"),
    a = c.find(".confirm-handler"),
    d = !1,
    c.find(".subtype").on("click",
    function() {
        return $(".subtype").removeClass("active"),
        $(this).addClass("active")
    }),
    c.find("#share-experience").on("click",
    function() {
        return $("#content").attr("placeholder", "输入您要分享的内容")
    }),
    c.find("#need-help").on("click",
    function() {
        return $("#content").attr("placeholder", "输入您遇到的问题")
    }),
    a.on("click",
    function(b) {
        var c;
        return b.preventDefault(),
        c = {
            type: "research",
            subtype: $(".active").data("subtype"),
            industry: $("#industry").val(),
            companyName: $("#company-name").val(),
            name: $("#name").val(),
            jobTitle: $("#job-title").val(),
            contact: $("#contact").val(),
            useTime: $("#use-time").val(),
            users: $("#users").val(),
            content: $("#content").val()
        },
        d === !1 ? $.post("/api/philanthropy", c,
        function() {
            return a.html(teambitionI18n.applySuccess),
            d = !0
        }).fail(function() {
            return a.html(teambitionI18n.pleaseRetry)
        }) : a.html(teambitionI18n.submitted).addClass("disabled")
    }),
    c.find(".form-control").click(function() {
        return d === !1 ? a.html(teambitionI18n.confirm) : void 0
    }).on("keyup",
    function() {
        return $.trim(this.value).length ? $(this).parents().addClass("has-value") : $(this).parents().removeClass("has-value")
    })
}),
$(function() {
    var a;
    return a = {
        animation: !0,
        triggrt: "hover"
    },
    $(".devtooltip").tooltip(a),
    $("body").scrollspy({
        target: ".api-sidebar"
    }),
    $(".api-sidebar").affix({
        offset: {
            top: $(".site-header").outerHeight() + $(".jumbotron").outerHeight(!0)
        }
    }),
    $("body").on("click", ".back-to-top",
    function(a) {
        return a.preventDefault(),
        $("body").animate({
            scrollTop: 0
        },
        300)
    })
}),
$(function() {
    return $(".new").on("click", ".feature-title",
    function() {
        return $(this).parent(".feature").toggleClass("open")
    })
}),
$(function() {
    var a;
    return a = document.location.toString(),
    a.match("#") ? $(".partner-tab a[href=#" + a.split("#")[1] + "]").tab("show") : void 0
}),
$(function() {
    return $(".search-input").keyup(function() {
        var a;
        return a = $(this).val(),
        "" !== a ? $(".search-handler").addClass("active") : $(".search-handler").removeClass("active")
    }).focus(function() {
        return $(".search-icon").addClass("active")
    }).blur(function() {
        var a;
        return a = $(this).val(),
        "" === a ? $(".search-icon").removeClass("active") : void 0
    })
}),
$(function() {
    var a, b;
    return $(".info-member .next").on("click",
    function() {
        var a, b;
        return a = $(".act"),
        b = a.next(),
        b.length || (b = a.siblings("li:first")),
        a.fadeOut(600,
        function() {
            return $(".workerlist li").removeClass("act"),
            b.fadeIn(600).addClass("act")
        })
    }),
    $(".info-member .prev").on("click",
    function() {
        var a, b;
        return a = $(".act"),
        b = a.prev(),
        b.length || (b = a.siblings("li:last")),
        a.fadeOut(600,
        function() {
            return $(".workerlist li").removeClass("act"),
            b.fadeIn(600).addClass("act")
        })
    }),
    $(".info-team .about-me-icon").each(function(a, b) {
        return $(b).attr("href", "/info/member?index=" + a)
    }),
    a = function(a) {
        var b, c;
        return c = new RegExp("(^|&)" + a + "=([^&]*)(&|$)", "i"),
        b = window.location.search.substr(1).match(c),
        b ? unescape(b[2]) : void 0
    },
    b = a("index"),
    b >= 0 ? $(".workerlist li").removeClass("act").eq(b).addClass("act") : void 0
}),
$(function() {
    return $(".tour .slide img").lazyload({
        effect: "fadeIn",
        placeholder: "data:image/gif;base64,R0lGODlhAQABAIAAAAAAAP///yH5BAEAAAAALAAAAAABAAEAAAIBRAA7"
    })
}),
/*$(function() {
    var a, b, c, d;
    return b = $(".tbsite-article"),
    b.length ? (c = b.find(".topbanner"), a = b.find(".loadingbar"), navigator.userAgent.match(/(iPhone|iPod|Android|ios)/i) || $(window).scroll(function() {
        var a, b;
        return a = $(window).scrollTop(),
        b = 1 * (c.height() - a) / c.height(),
        c.css("opacity", b)
    }), d = new Image, d.src = c.data("bg"), d.onload = function() {
        return c.css("background-image", "url(" + d.src + ")"),
        a.fadeOut(500,
        function() {
            return this.remove()
        })
    }) : void 0
}),*/
$(function() {
    var a;
    return navigator.userAgent.match(/micromessenger/i) ? void 0 : (a = $("body.tbsite-article").length ? $(".tbsite-article .topbanner").height() : 300, $(".site-header").headroom({
        offset: a,
        tolerance: 5,
        classes: {
            initial: "animated",
            pinned: "slideDown",
            unpinned: "slideUp"
        }
    }))
}),
$(function() {
    var a;
    return a = $.ias({
        container: ".list-wrap .content",
        item: ".list-item",
        pagination: ".pagination",
        next: ".next a",
        delay: 1e3
    }),
    a.extension(new IASSpinnerExtension({
        html: '<div class="loading-indicator text-center"> <span class="loader-dot"></span> <span class="loader-dot"></span> <span class="loader-dot"></span> </div>'
    })),
    a.extension(new IASTriggerExtension({
        offset: 3
    }))
});