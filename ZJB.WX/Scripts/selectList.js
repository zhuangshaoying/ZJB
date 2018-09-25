/**查询区**/
	$(function(){
		
		$(".manage-list-content").die().live("mouseleave",function(){							
			$(".manage-list-content").hide();
		});
		
	})
	function selectOption(on){
		var obj = $("."+on).siblings("li").find(".manage-list-content");
		$("body").find(".manage-list-content").not(obj).hide();
		if(obj.css("display") == "block"){
	        obj.hide();
	    }else{
	    	obj.show();
	    }
	}
	
	
	$(".manage-area-content li").die().live("click",function(){	
		var content = $(this).html();
		$("#area").val($(this).attr("areaId"));
		$(".manage-area-tip .select-font").html(content);
		$(".manage-list-content").hide();
		initSq($(this).attr("areaId"));
	});
	
	$(".manage-sq-content li").die().live("click",function(){	
		var content = $(this).html();
		$("#sq").val($(this).attr("sqId"));
		$("#sq").attr("content",content);
		$(".manage-sq-tip .select-font").html(content);
		$(".manage-list-content").hide();
	});
	
	$(".manage-rent-content li").die().live("click",function(){	
		var content = $(this).html();
		$("#rent").val($(this).attr("postType"));
		$(".manage-rent-tip .select-font").html(content);
		$(".manage-list-content").hide();
	});