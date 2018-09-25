jQuery(".msg-attach-pics .att-file-pic,.cattach-img-preview .att-file-pic").live("click", function () {
	PicViewHandler.picviewInit(this);
});
jQuery(".i8att-shadow").live("click", function () {
	jQuery(".i8att-shadow").remove();
	jQuery(".i8att-container").remove();
	$("body").removeClass("bigPic_over");
})