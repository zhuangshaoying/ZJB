
$(function(){
	$("a[name=thumbLink]").die().live("click", function() {
		$.ajax({
			url: "../import/getDescContent.html",
			type: "get",
			data: {
				id: $(this).attr("aid")
			},
			success: function(result) {
				parent.setDescContent(result);
			}
		});
    });
	
	$(".agent-group-head").die().live("click", function() {
		var id = $(this).attr("id");
		$(".agent-group-head").hide();
		$("." + id).show();
		$(".agent-group-btn").show();
		var imgs = $("." + id).find("img");
		$.each(imgs, function(i, n) {
			var img = imgs.eq(i);
			if (img.attr("src") == null) {
				img.attr("src", img.attr("i"));
			}
		});
	});
	
	$(".agent-group-btn").die().live("click", function() {
		$(".agent-group-list").hide();
		$(".agent-group-btn").hide();
		$(".agent-group-head").show();
	});
});
