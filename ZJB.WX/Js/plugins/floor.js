define(function(require, exports, module){

	//带看日期选择
	exports.selectDate = function(){
		require("mobiscroll");
		var currYear = (new Date()).getFullYear();
		var opt = {
			date: {
				preset: "date"
			},
			datetime: {
				preset : 'datetime'
			},			
			time: {
				preset: "time"
			},
			default: {
				theme: "android-ics light", //皮肤样式
		        display: "modal", //显示方式
		        mode: "scroller", //日期选择模式
				dateFormat: "yyyy-mm-dd",
				lang: "zh",
				// showNow: true,
				showNow: false,
				nowText: "今天",
		        startYear: currYear - 10, //开始年份
		        endYear: currYear + 10 //结束年份
			}
		};

	  	$("#appDate").mobiscroll($.extend(opt["date"], opt["default"]));
	  	var optDateTime = $.extend(opt['datetime'], opt['default']);
	  	var optTime = $.extend(opt["time"], opt["default"]);
	  	$("#appDateTime").mobiscroll(optDateTime).datetime(optDateTime);
	    $("#appTime").mobiscroll(optTime).time(optTime);

		$(".select-close").on("click", function(){
			var $this = $(this),
				$par = $this.closest(".select-date-box");
			$(".select-date-box").hide().removeClass("select-date-transform");
		});
	}

    //选择楼盘
    exports.selectArea = function(options){
		$("#chosearea").on("click", function(){
			$("#area").show();
			$("#yes").on("click", function(){
				var val = $('input:radio[name="property_id"]:checked').attr('property_name');
				if(val){
					$("#area").hide();
					$("#chosearea").text(val);
				}
			});
			$("#no").on("click", function(){
				$("#area").hide();
			});
		});
	}

	//报备、带看客户
	exports.filingClient = function(options){
		$(".item-check").on("click", function(){
			var $checked = $(".item-check:checked"),
				length = $checked.length;
			$("#number").text(length);
		});
		var defaults = {
            filing: true  //报备客户
        };
        var opts = $.extend({}, defaults, options);

		$(".submit-btn").on("click", function(){
			var $this = $(this),
				$ipt = $this.siblings(".data-field"),
				number = $("#number").text(),
				propertyid = $ipt.data("propertyid"),
				agentid = $ipt.data("agentid"),
				appDate = $("#appDate").val(),
				appTime = $("#appTime").val(),
				ids = [];
			$(".item-check:checked").each(function(){
				var $this = $(this), $par = $this.closest(".item"),
					id = $par.data("clientid");
				ids.push(id);
			})
			function submitAjax(str, datas){
				//if(number == 0){
					//layer.open({
						//content: "请先选择" + str + "客户",
						//style: "background: rgba(0, 0, 0, 0.5); color: #fff",
						//time: 2
					//});
					//return false;
				//} else {
					$.ajax({
						url: "/",
						type: "post",
						data: datas,
						dataType: "json",
						success: function(data){
							if(data.status == "success"){
								$("#pop").addClass("popIn");
								$("#mask").fadeIn();
							} else {
								layer.open({
									content: data.remark,
									style: "background: rgba(0, 0, 0, 0.5); color: #fff",
									time: 2
								});
							}
						}
					})
				//}
			}
			if(opts.filing){
				submitAjax("报备", {
					getapi: "wechat/customer/add_customer",
					client_id: ids,
					property_id: propertyid,
					agent_id: agentid
				});
			} else {
				submitAjax("带看", {
					getapi: "wechat/show/edit",
					client_id: ids,
					agent_id: agentid,
					show_time: appDate + " " + appTime
				});
			}
		})
	}

	//发起带看
	exports.openView = function(){
		$(".view-btn").on("click", function(){
			var $this = $(this),
				$par = $this.closest(".item"),
				id = $par.data("itemid");
			$(".select-date-box").show().addClass("select-date-transform");
			$("#itemId").val(id);
			return false;
		})

		$(".submit-btn").on("click", function(){
			var $this = $(this),
				ids = $("#itemId").val(),
				appDate = $("#appDate").val(),
				appTime = $("#appTime").val();
			$.ajax({
				url: "/",
				type: "post",
				data: {
					getapi: "wechat/show/applyshow",
					id: ids,
					show_time: appDate + " " + appTime
				},
				dataType: "json",
				success: function(data){
					if(data.status == "success"){
						$("#pop").addClass("popIn");
						$("#mask").fadeIn();
					} else {
						layer.open({
							content: data.remark,
							style: "background: rgba(0, 0, 0, 0.5); color: #fff",
							time: 2
						});
					}
				},
				error: function(data){
					layer.open({
						content: data.remark,
						style: "background: rgba(0, 0, 0, 0.5); color: #fff",
						time: 2
					});
				}
			})
		})
	}

	//进度展开伸缩
	exports.toggleProgress = function(){
		$(".toggle-btn").on("click", function(){
			var $this = $(this),
				$sib = $this.siblings(".pro-group");
			if($this.hasClass("up")){
				$this.removeClass("up").addClass("down");
				$sib.height("auto");
			} else {
				$this.removeClass("down").addClass("up");
				$sib.height("0.66rem");
			}
		})

		$(".pro-group").each(function(){
			var $this = $(this),
				length = $this.find("p").length;
			if(length == 1){
				$this.siblings(".toggle-btn").hide();
			} else {
				$this.siblings(".toggle-btn").show();
			}
		})
	}

	//楼盘详情图片放大
	exports.imagesZoom = function(){
		require("scale");
		ImagesZoom.init({
			"elem": ".type-list"
		});
	}

	//添加新客户
	exports.roleChange = function(){
		$(".client-role").on("click", function(){
			var $this = $(this),
				txt = $this.data("clientrole");
			if(!$this.hasClass("on")){
				$this.addClass("on").siblings(".client-role").removeClass("on");
				$this.siblings("input[type='hidden']").val(txt);
			}
		})
	}

	//楼盘列表搜索
	exports.houseList = function(){
		//搜索
		$(".submit-btn").on("click", function(){
			var $this = $(this), $ipt = $this.siblings(".ipt"),
				content = $ipt.val();
			window.location.href = "/wechat/property_list&name=" + content;
		});

		//筛选收缩
		$(".filter-btn").on("click", function(){
			var $this = $(this), $sib = $this.siblings(".filter-drop");
			$sib.slideDown();
			if(!$this.hasClass("active")){
				$this.addClass("active");
				$("#mask").fadeIn("fast");
				$sib.slideDown();
			} else {
				$this.removeClass("active");
				$sib.slideUp();
				$("#mask").fadeOut("fast");
			}
		});

		//筛选按钮
		$(".filter-drop .col").each(function(){
			var $this = $(this), $dd = $this.find("dd");
			$dd.on("click", function(){
				$(this).addClass("on").siblings("dd").removeClass("on");
			})
		})

		//点击遮罩层
		$("#mask").on("click", function(){
			$(this).hide();
			$("#filterList").find(".filter-drop").hide().end().find(".filter-btn").removeClass("active");
		});

		//取消筛选
		$(".cancel-btn").on("click", function(){
			$("#mask").hide();
			$("#filterList").find(".filter-drop").hide().end().find(".filter-btn").removeClass("active");
		})

		//筛选
		$(".ok-btn").on("click", function(){
			var $area = $("#areas").find(".on"),
				$type = $("#types").find(".on"),
				areas = $area.data("area"),
				types = $type.data("type");
			window.location.href = "/wechat/property_list&areas=" + areas + "&type=" + types;
		});
	}

	//楼盘意向
	exports.intention = function(){
		//一次最多选择2个报备客户
//		$(".item-check").on("click", function(){
//			var $this = $(this),
//				$checked = $(".item-check:checked"),
//				length = $checked.length;
//			if(length > 2){
//				layer.open({
//					content: "一次最多只能选择2个报备客户",
//					style: "background: rgba(0, 0, 0, 0.5); color: #fff",
//					time: 2
//				});
//				return false;
//			}
//		})

		//保存报备客户
		$(".save-btn").on("click", function(){
			var $this = $(this),
				$sib = $this.siblings(".data-field"),
				txt = $this.text(),
				names = $sib.data("realname"),
				phones = $sib.data("phone"),
				$checked = $(".item-check:checked"),
				length = $checked.length,
				ids = [];
			$checked.each(function(i, el){
				var $el = $(el), $par = $el.closest(".item"),
					property_id = $par.data("id");
				ids.push(property_id);
			})
			if(length == 0){
				layer.open({
					content: "请先选择报备客户",
					style: "background: rgba(0, 0, 0, 0.5); color: #fff",
					time: 2
				});
				return false;
			} else {
				$this.text("保存中...");
				$.ajax({
					url: "/",
					type: "post",
					data: {
						property_id: ids,
						real_name: names,
						phone: phones,
						getapi: "wechat/client/addcustomer"
					},
					dataType: "json",
					success: function(data){
						if(data.status == "success"){
							$("#pop").addClass("popIn");
							$("#mask").fadeIn();
							$this.text("保存成功");
						} else {
							layer.open({
								content: data.remark,
								style: "background: rgba(0, 0, 0, 0.5); color: #fff",
								time: 2
							});
							$this.text(txt);
						}
					}
				})
			}
		})
	}

	//佣金奖励
	exports.rebatePop = function(){
		$("#rebateBtn").on("click", function(){
			$("#pop").addClass("popIn");
			$("#mask").show();
		})

		$(".close-btn").on("click", function(){
			$(this).closest(".pop-success").removeClass("popIn");
			$("#mask").hide();
		})
	}
})