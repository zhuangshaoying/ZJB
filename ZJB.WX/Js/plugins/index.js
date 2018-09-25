define(function(require, exports, module){
	exports.filterList = function(){
		var $id = $("#filterList");
		$id.find(".select-txt").on("click", function(){
			var $this = $(this),
				$par = $this.closest(".item"),
				$child = $par.find(".filter-drop"),
				left = $par.position().left;
			if(!$par.hasClass("active")){
				$("#mask").fadeIn("fast");
				$par.addClass("active").siblings().removeClass("active");
				$id.find(".filter-drop").slideUp();
				$child.css("left", -left).slideDown();
			} else {
				$par.removeClass("active");
				$id.find(".filter-drop").slideUp();
				$("#mask").fadeOut("fast");
			}
		})

		$("#mask").on("click", function(){
			$(this).hide();
			$id.find(".filter-drop").hide().end().find(".item").removeClass("active");
		})
	}
})