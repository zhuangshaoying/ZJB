define(function(require, exports, module){
	exports.mobileUtilMethod = function(){
		/*
		* MobileWeb 通用功能助手，包含常用的 UA 判断、页面适配、search 参数转 键值对。
		* 该 JS 应在 head 中尽可能早的引入，减少重绘。
		*
		* fixScreen 方法根据两种情况适配，该方法自动执行。
		*      1. 定宽： 对应 meta 标签写法 -- <meta name="viewport" content="target-densitydpi=device-dpi,width=750">
		*          该方法会提取 width 值，主动添加 scale 相关属性值。
		*          注意： 如果 meta 标签中指定了 initial-scale， 该方法将不做处理（即不执行）。
		*      2. REM: 不用写 meta 标签，该方法根据 dpr 自动生成，并在 html 标签中加上 data-dpr 和 font-size 两个属性值。
		*          该方法约束：IOS 系统最大 dpr = 3，其它系统 dpr = 1，页面每 dpr 最大宽度（即页面宽度/dpr） = 750，REM 换算比值为 16。
		*          对应 css 开发，任何弹性尺寸均使用 rem 单位，rem 默认宽度为 视觉稿宽度 / 16;
		*              scss 中 $ppr(pixel per rem) 变量写法 -- $ppr: 750px/16/1rem;
		*                      元素尺寸写法 -- html { font-size: $ppr*1rem; } body { width: 750px/$ppr; }。
		*/
		window.mobileUtil = (function(win, doc) {
			var UA = navigator.userAgent,
				isAndroid = /android|adr/gi.test(UA),
				isIos = /iphone|ipod|ipad/gi.test(UA) && !isAndroid, // 据说某些国产机的UA会同时包含 android iphone 字符
				isMobile = isAndroid || isIos;  // 粗略的判断

			return {
				isAndroid: isAndroid,
				isIos: isIos,
				isMobile: isMobile,

		        isNewsApp: /NewsApp\/[\d\.]+/gi.test(UA),
				isWeixin: /MicroMessenger/gi.test(UA),
				isQQ: /QQ\/\d/gi.test(UA),
				isYixin: /YiXin/gi.test(UA),
				isWeibo: /Weibo/gi.test(UA),
				isTXWeibo: /T(?:X|encent)MicroBlog/gi.test(UA),

				tapEvent: isMobile ? 'tap' : 'click',

				/*
				 * 缩放页面
				 */
				fixScreen: function() {
		            var metaEl = doc.querySelector('meta[name="viewport"]'),
		                metaCtt = metaEl ? metaEl.content : '',
		                matchScale = metaCtt.match(/initial\-scale=([\d\.]+)/),
					    matchWidth = metaCtt.match(/width=([^,\s]+)/);

		            if ( !metaEl ) { // REM
		                var docEl = doc.documentElement,
		                    // maxwidth = docEl.dataset.mw || 750, // 每 dpr 最大页面宽度
		                    maxwidth = docEl.dataset.mw || 640, // 每 dpr 最大页面宽度(jd是640,taobao是540)
		                    dpr = isIos ? Math.min(win.devicePixelRatio, 3) : 1,
		                    scale = 1 / dpr,
		                    tid;

		                docEl.removeAttribute('data-mw');
		                docEl.dataset.dpr = dpr;
		                metaEl = doc.createElement('meta');
		                metaEl.name = 'viewport';
		                metaEl.content = fillScale(scale);
		                docEl.firstElementChild.appendChild(metaEl);

		                var refreshRem = function() {
		                    var width = docEl.getBoundingClientRect().width;
		                    if (width / dpr > maxwidth) {
		                        width = maxwidth * dpr;
		                    }
		                    // var rem = width / 16;
		                    var rem = width / 10;
		                    docEl.style.fontSize = rem + 'px';
		                    docEl.style.width = "100%";
		                    docEl.style.height = "100%";
		                };

		                win.addEventListener('resize', function() {
		                    clearTimeout(tid);
		                    tid = setTimeout(refreshRem, 300);
		                }, false);
		                win.addEventListener('pageshow', function(e) {
		                    if (e.persisted) {
		                        clearTimeout(tid);
		                        tid = setTimeout(refreshRem, 300);
		                    }
		                }, false);

		                refreshRem();
		            } else if ( isMobile && !matchScale && ( matchWidth && matchWidth[1] != 'device-width' ) ) { // 定宽
		                var	width = parseInt(matchWidth[1]),
		                    iw = win.innerWidth || width,
		                    ow = win.outerWidth || iw,
		                    sw = win.screen.width || iw,
		                    saw = win.screen.availWidth || iw,
		                    ih = win.innerHeight || width,
		                    oh = win.outerHeight || ih,
		                    ish = win.screen.height || ih,
		                    sah = win.screen.availHeight || ih,
		                    w = Math.min(iw,ow,sw,saw,ih,oh,ish,sah),
		                    scale = w / width;

		                if ( scale < 1 ) {
		                    metaEl.content = metaCtt + ',' + fillScale(scale);
		                }
		            }

		            function fillScale(scale) {
		                return 'initial-scale=' + scale + ',maximum-scale=' + scale + ',minimum-scale=' + scale + ',user-scalable=no,width=device-width';
		            }
				},

				/*
				 * 转href参数成键值对
				 * @param href {string} 指定的href，默认为当前页href
				 * @returns {object} 键值对
				 */
				getSearch: function(href) {
					href = href || win.location.search;
					var data = {},reg = new RegExp( "([^?=&]+)(=([^&]*))?", "g" );
					href && href.replace(reg,function( $0, $1, $2, $3 ){
						data[ $1 ] = $3;
					});
					return data;
				}
			};
		})(window, document);

		// 默认直接适配页面
		mobileUtil.fixScreen();
	}

	//loading
	exports.loading = function(){
		var interval = setInterval(function(){
			if (document.readyState === "interactive" || document.readyState === "complete"){
				$(".main-content .item").css({opacity: 1});
				$("#loading").css({display: "none"});
				clearInterval(interval);
			}
		},80);
	}

	//表单验证
	exports.dyValidate = function(url, tourl, erurl){
		require("layer");
		require("validate");
		$("form.Jvalidation").each(function(){
		    var $form = $(this);
		    $form.validate({
				submitHandler:function(form){
					require("submitform");
					var index = layer.open({type: 2});
                    var $submitBtn = $form.find(".button"),
					    txt = $submitBtn.text();
					$submitBtn.attr("disabled", "disabled").text("提交中...");
					$(form).ajaxSubmit({
						url: url,
						success: function(data){
							layer.close(index);
                         	if(data.status == "success"){
                         		if(typeof($("#tourl").val()) != "undefined"){
                         			tourl = $("#tourl").val();
                         		}
                         		if(typeof(data.data) != "undefined" && typeof(data.data.tourl) != "undefined" && data.data.tourl != ""){
 									tourl = data.data.tourl;
 								}
 								if(data.data.type == "filing"){
 									$("#pop").addClass("popIn");
									$("#mask").fadeIn();
									$submitBtn.removeAttr("disabled").text(txt);
									return false;
 								}
                         		layer.open({
                         			content: data.remark || "提交成功",
                         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
                         			time: 2,
                         			end: function(){
	 									if(tourl){
	 										parent.window.location.href = tourl;
	 									} else {
	 										parent.window.location.reload();
	 									}
	 		                    	}
                         		})
                         	} else {
                         		layer.open({
                         			content: data.remark || "提交失败",
                         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
                         			time: 2,
                         			end: function(){
	 									if(erurl) parent.window.location.href = erurl;
                         				$submitBtn.removeAttr("disabled").text(txt);
	 		                    	}
                         		})
                         	}
						}
					});
					return false;
				}
			});
		});
	}


	/*
    * 手机校验码倒计时发送
    * @param 第一个参数为触发元素，第二个参数是一个对象
    * obj中有两个默认的接口(图形验证码接口、手机短信校验码接口)
    * 如不需要验证图形验证码就可获取短信校验码则 imgVericode: false
    */
    exports.countDown = function(target, options){
        var defaults = {
            vericodeApi: "wechat/member/regsend",  //验证验证码接口
            sendApi: "wechat/member/regsend",  //验证手机短信校验码发送接口
            time: 59,
            imgVericode: true  //是否需要验证验证码
        };
        var opts = $.extend({}, defaults, options);
        var time = opts.time;

        $("#" + target).click(function() {
			var $this = $(this), $phone = $("#phone"), $vericode = $("#veriCode"), $phonecode = $("#phoneCode"),
                phone = $phone.val(),
				phonecode = $phonecode.val(),
                vericode = $vericode.val();
			if(phone == ""){
				layer.open({
         			content: "请先填写手机号码",
         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
         			time: 2
         		})
				return false;
			} else {
				if(/^1[3|4|5|7|8][0-9]\d{8}$/.test(phone) == false){
					layer.open({
	         			content: "请填写正确的手机号码格式",
	         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
	         			time: 2
	         		})
					$phone.focus();
					return false;
				}
				if(vericode == ""){
					layer.open({
	         			content: "请先填写图形验证码",
	         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
	         			time: 2
	         		})
					return false;
				}
			}
            $this.attr("disabled", "disabled");
            function _success(){
                $this.val("发送中...");
                $.ajax({
                    type: "post",
                    url: "/",
                    data: {
                        phone: phone,
						valid_code: vericode,
						getapi: opts.sendApi
                    },
                    dataType: "json",
                    success: function(data){
                        if(data.status == "success"){
                            layer.open({
			         			content: "发送成功",
			         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
			         			time: 1.5,
			         			end: function(){
			         				$this.val("重新获取(" + time + ")");
	                                var timer = setInterval(function(){
	                                    time--;
	                                    if(time < 0){
	                                        $this.removeAttr("disabled").val("重新获取");
	                                        clearInterval(timer);
	                                        time = opts.time;
	                                    } else {
	                                        $this.val("重新获取(" + time + ")");
	                                    }
	                                }, 1000);
			         			}
			         		})
                        } else {
                            layer.open({
			         			content: data.remark || "发送失败",
			         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
			         			time: 1.5,
			         			end: function(){
			         				if(data.remark != "send_too_frequent"){
	                                    $this.removeAttr("disabled").val("获取验证码");
	                                } else {
	                                    time = 300;
	                                    $this.val("重新获取(" + time + ")");
	                                    var timer = setInterval(function(){
	                                        time--;
	                                        if(time < 0){
	                                            $this.removeAttr("disabled").val("重新获取");
	                                            clearInterval(timer);
	                                        } else {
	                                            $this.val("重新获取(" + time + ")");
	                                        }
	                                    }, 1000);
	                                }
			         			}
			         		})
                        }
                    }
                })
            }
            if(opts.imgVericode){
                $.ajax({
                    type: "post",
                    url: "/",
                    data: {
                        phone: phone,
						getapi: opts.vericodeApi,
                        vericode: vericode
                    },
                    dataType: "json",
                    success: function(data){
                        if(data.status == "success"){
                            _success();
                        } else {
                        	layer.open({
                        		content: data.remark,
                        		style: "background: rgba(0, 0, 0, 0.5); color: #fff",
                        		time: 1.5
                        	})
							//刷新验证码
							$("#veriCode").click();
                            $this.removeAttr("disabled");
                        }
                    }
                });
            } else {
                _success();
            }
        })
    }
})