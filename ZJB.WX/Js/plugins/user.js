define(function(require, exports, module){

	//弹窗关闭
	exports._closePop = function(){
		$("#mask, .pop-close").on("click", function(){
			$("#mask").fadeOut("fast");
			$("#pop").addClass("popOut").removeClass("popIn");
			if($("#pop").has(".popOut")) {
				var t = setTimeout(function(){
					$("#pop").removeClass("popOut");
				}, 500);
			} else {
				clearInterval(t);
			}
		});
	}

	//触发弹窗
	exports.popMethod = function(target){
		$(target).on("click", function(){
			$("#mask").fadeIn("fast");
			$("#pop").addClass("popIn");
		});
		exports._closePop();
	}

	//我的佣金
	exports.swiperSlide = function(){
		require("swiper");
		var tabsSwiper = new Swiper(".swiper-container",{
			speed:500,
			reInit: function(){
				var active = $(".swiper-container .tabs-page").eq(tabsSwiper.activeIndex);
				$(".swiper-container").height(active.height()); //初始化，设置页面高度。
			},
			onCreateLoop: function(){
				var active = $(".swiper-container .tabs-page").eq(tabsSwiper.activeIndex);
				$(".swiper-container").height(active.height()); //初始化，设置页面高度。
			},
			onSlideChangeStart: function(){
				$(".tabs-group a").removeClass("active").eq(tabsSwiper.activeIndex).addClass("active");  //tabs状态调整
				var active = $(".swiper-container .tabs-page").eq(tabsSwiper.activeIndex);
				$(".swiper-container").height(active.height()); //页面切换时，重新设置高度
			}
		});
		$(".tabs-group a").on("touchstart mousedown",function(e){
			e.preventDefault();
			var $this = $(this),
				index = $this.index();
			$this.addClass("active").siblings().removeClass("active");
			tabsSwiper.slideTo(index);
		}).on("click", function(e){
			e.preventDefault();
		})
	}

	//一键提现
	exports.withdraw = function(){
		$(".withdraw").on("click", function(){
			$.ajax({
				url: "/",
				type: "post",
				data: {
					getapi: "wechat/expendlog/add"
				},
				success: function(data){
					if(data.status == "success"){
						layer.open({
		         			content: "提现成功！到账需要24小时，请留意您绑定的银行卡。",
		         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
		         			time: 2
		         		});
					} else {
						layer.open({
		         			content: data.remark,
		         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
		         			time: 2
		         		});
					}
				}
			})
		})
	}

	//退出登录
	exports.logout = function(){
        require("layer");
        $(".logout").on('click', function() {
            $.post('/', {
                getapi: 'wechat/member/logout'
            }, function(data) {
                if (data.status == "success") {
                    window.location.href = '/wechat/login';
                } else {
                    layer.open({
	         			content: "退出登录失败",
	         			style: "background: rgba(0, 0, 0, 0.5); color: #fff",
	         			time: 2
	         		});
                }
            })
        })
    }

    //注册类型选择
    exports.typeSelect = function(){
    	$("#userType li").on("click", function(){
    		var $this = $(this),
    			$ipt = $("#userType").find("input[name='type']"),
    			member = $this.data("member");
    		if(!$this.hasClass("on")){
    			$this.addClass("on").siblings().removeClass("on");
    			$ipt.val(member);
    		}
    	})
    }
})