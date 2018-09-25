// 保存表单数据
var url_1 = "/OperateHouse";
// 提交数据获取title text
var url_2 = "/House/GetTitleView";
// 提交数据获取title json
var url_3 = "/House/GetTitle";
//上传图片的url
var upload_url = '/ImageUpload/Add';
//var upload_url = 'http://up.qiniu.com/';

// 提交数据获取desc text
var url_4 = "/House/GetDescView";
// 提交数据获取desc json
var url_5 = "/House/GetDesc";
// 保存草稿
var url_6 = "/House/SaveDraft";
//获取小区信息
var url_7 = "/House/GetCellsByInput";
//获取房源标签
var url_8 = "/house/getHouseLables";
//获取房源图片
var url_9 = "/house/getHouseLables";
var avgPrice1 = 0;
var imageW116H116Q85 = "?imageMogr2/strip|imageView2/1/w/116/h/116/q/85";
(function (config) {
    config['lock'] = true;
    config['fixed'] = true;
    config['okVal'] = '确定';
    config['cancelVal'] = '取消';
    // [more..]
})(art.dialog.defaults);

var dialog = null;
var regexp = /(^\s*)|(\s*$)/g;
var typeArray = ['IMG_M','IMG_I','IMG_O'];
$(function () {

    getWaterMarkSeting();
    /**图片上传Flash版本提示**/
    var fls = flashChecker();
    if(!fls.isHas){
        $("#picupload1").attr("checked",true);
        $(".picupload2-tip").hide();
        $(".picupload1-tip").show();
    }
    //默认上传方式
    var picupload1 = $("#picupload1").attr("checked");
    var picupload2 = $("#picupload2").attr("checked");
	
    if(picupload1 == "checked") {
        singleClick();
    } else if(picupload2 == "checked") {
        mutilDivClick();
    } else {
        compressClick();
    }
    /*Form表单是否被修改*/
    $("input").die().live("change",function(){
        $("#isChange").val("1");
    });
    $("select").die().live("change",function(){
        $("#isChange").val("1");
    });
	
    /*标签离开隐藏*/
    $("#saleHouse-eare-tip").click(function () { 
        $(".houseLabel-window").hide(); 
        $(".cellLabel-window").show();  
    });  
    $(".cellLabel-window").mouseleave(function () {  
        $(".cellLabel-window").hide();  
    }); 
    
    $("#saleHouse-house-tip").click(function () { 
        $(".cellLabel-window").hide(); 
        $(".houseLabel-window").show();  
    });  
    $(".houseLabel-window").mouseleave(function () {  
        $(".houseLabel-window").hide();  
    }); 
	
    var houseLabels = null;
    var yijuhuaFlag = false;
    $("input[name=houseLabel]").die().live("focus", function() {
        houseLableValue = $(this).val();
        yijuhuaValue = $("#saleHouse-tongcheng-tip").val();
        if (houseLableValue == yijuhuaValue) {
            yijuhuaFlag = true;
        } else {
            yijuhuaFlag = false;
        }
		
        if (houseLabels != null) {
            showHouseLabelWindow(houseLabels);
            return;
        }
        $.ajax({ 
            type: "post", 
            url: url_8,
            data: "postType=" + $("#postType").val(),
            dataType: "text",
            success: function (result) {
                houseLabels = result;
                showHouseLabelWindow(houseLabels);
            }
        });
    });
	
    $("#saleHouse-house-tip").die().live("blur", function() {
        if (yijuhuaFlag || $("#saleHouse-tongcheng-tip").val() == '') {
            $("#saleHouse-tongcheng-tip").val($("#saleHouse-house-tip").val());
            focusAndBlur("saleHouse-tongcheng-tip");
        }
    });
	
    $("input[name=cellLabel]").die().live("focus", function() {
        var cellLabels = ["唯一住房", "带户口", "得房率高", "学区房", "双学区", "配套成熟", "地铁房", "带车库", "带花园", "婚房",
		                  "交通便利", "景观房", "黄金楼层", "稀缺户型", "知名物业", "带阁楼", "带露台", "东边户", "西边户", "繁华商圈",
		                  "商住皆宜", "繁华地段", "挑高户型"];
        var result = '<h5>推荐标签：<a class="tip-close" href="javascript:tipWindowClose()">' + 
			'<span class="tip-img"></span></a></h5>' + 
			'<ul>';
        for (var key in cellLabels) {
            result += ('<li><a name="cell_label_link">' + cellLabels[key] + '</a></li>');
        }
        result += '</ul><div class="clear"><hr></hr></div>' + 
			'<div class="clear col-red tip-window-font">' + 
			'<span> *最多三个标签，标签之间用1个空格分隔，每个标签限2~6字！</span>' + 
			'</div>';
        $(".cellLabel-window").html(result);
        var left = $(".saleHouse-eare-tip").offset().left;
        var top = $(".saleHouse-eare-tip").offset().top;
        $(".houseLabel-window").hide();
        $(".cellLabel-window").show();
        $(".cellLabel-window").css({"left":left, "top":top + 18});
    });
	
    $("a[name=house_label_link]").die().live("click",function(){
        var houseLabelStr = $("input[name=houseLabel]").val().replace(regexp, "");
        var houseLabels = houseLabelStr.split(" ");
        if (houseLabels.length >= 3) {
            alert("房源标签不能超过3个！");
            return;
        }
        var exist = false;
        for (var index in houseLabels) {
            if (houseLabels[index].replace(regexp, "") == $(this).html()) {
                exist = true;
                break;
            }
        }
        if (! exist) {
            $("input[name=houseLabel]").val((houseLabelStr + " " + $(this).html()).replace(regexp, ""));
            if (yijuhuaFlag || $("#saleHouse-tongcheng-tip").val() == '') {
                $("#saleHouse-tongcheng-tip").val($("#saleHouse-house-tip").val());
                focusAndBlur("saleHouse-tongcheng-tip");
            }
        }
        focusAndBlur("saleHouse-house-tip");
    });
	 
    $("a[name=cell_label_link]").die().live("click",function(){
        var cellLabelStr = $("input[name=cellLabel]").val().replace(regexp, "");
        var cellLabels = cellLabelStr.split(" ");
        if (cellLabels.length >= 3) {
            alert("小区标签不能超过3个！");
            return;
        }
        var exist = false;
        for (var index in cellLabels) {
            if (cellLabels[index].replace(regexp, "") == $(this).html()) {
                exist = true;
                break;
            }
        }
        if (! exist) {
            $("input[name=cellLabel]").val((cellLabelStr + " " + $(this).html()).replace(regexp, ""));
        }
        focusAndBlur("saleHouse-eare-tip");
    });
	 
    var cells = null;
    var j = 0;
    var top = 0;
    var left = 0;
    var scrollNum = 0;
    //自动补全
    $(".keyWordBox").die().live("keyup",function(e){
        if((e.keyCode >= 37 && e.keyCode <= 40) || e.keyCode == 13){
            var objList = $(".keyList dd");
            $(".upordownSelect").css({'top':top,'left':left});
            if(e.keyCode == 38){
                if(j > 0){
                    $(".keyList").find("dd").eq(j).children("a").removeClass("upordownSelect");
                    j--;
                    $(".keyList").find("dd").eq(j).children("a").addClass("upordownSelect");
                    top -= 23;
                    if (j + 1 == scrollNum) {
                        scrollNum--;
                        $(".keyList").animate({scrollTop:23 * scrollNum},100);
                    }
                }else{
                    $(".keyList").find("dd").eq(0).children("a").addClass("upordownSelect");
                }
            }
            if(e.keyCode == 40){
                if(j < objList.length-1){
                    $(".keyList").find("dd").eq(j).children("a").removeClass("upordownSelect");
                    j++;
                    $(".keyList").find("dd").eq(j).children("a").addClass("upordownSelect"); 
                    top += 23;
                    if (j - scrollNum > 9) {
                        scrollNum++;
                        $(".keyList").animate({scrollTop:23 * scrollNum},100);
                    }
                }else{
                    $(".keyList").find("dd").eq(objList.length-1).children("a").addClass("upordownSelect"); 
                }
            }
            if(e.keyCode == 13){
                var cell = cells[$(".keyList").find(".upordownSelect").attr("index")];
                $("input[name=cell]").val(cell.cellName);
                $("#cellCode").val(cell.cellCode);
                $("select[name=distrct]").val(cell.district);
                $("select[name=distrct]").change();
                $("select[name=area]").val(cell.area);
                $("input[name=addr]").val(cell.address);
                if (cell.completionDate != 0) {
                    $("input[name=ddlUsedYear]").val(cell.completionDate);
                }
			    	
                if ($("#postType").val() == 0) {
                    avgPrice1 = (cell.avgPrice == null) ? 0 : cell.avgPrice;
                    var avgPrice = ((cell.avgPrice == null || cell.avgPrice == 0) ? "暂无" : cell.avgPrice);
                    $("#showPrice").html("【搜房均价：<font style='color:red'>" + avgPrice + "</font>&nbsp;元/平米】");
                    $("#showPrice").show();
                }
			    	
                focusAndBlur("cell");
                focusAndBlur("addr");
			    	
                //getModelImgs(cell.cellCode);
                getOutImgs(cell.cellCode);
                $("#div_IMG_O").html("");
            }
        }else{
            $("#cellCode").val("");
            var inputStr = $("input[name=cell]").val().replace(regexp, "");
            $(".keyList").find("a").removeClass("upordownSelect");
            if(inputStr != "") {
                $.ajax({
                    type: "post", 
                    url: url_7,
                    data: "buildingType=" + $("#buildingType").val() + "&inputStr=" + inputStr,
                    dataType: "json",
                    success: function (data) {
                        cells = data.cells;
                        $(".keyList").html("");
                        $(".keyList").show();
                        $.each(cells, function(i, n) {
                            $(".keyList").append("<dd><a index='" + i + "'>" + n.cellName + "</a></dd>");
                        });
                        $(".keyList").find("dd").eq(0).children("a").addClass("upordownSelect");
                        $(".keyList").animate({scrollTop:0},100);
                        j = 0;
                        top = 0;
                        scrollNum = 0;
                    }
                });
					
                $("body").one("click",function(){
                    $(".keyList").hide();
                    $("input[name=cell]").show();
                });
            }
        }
    });
	
    $(".keyList a").die().live("click", function(){
        var cell = cells[$(this).attr("index")];
        $("input[name=cell]").val(cell.cellName);
        $("#cellCode").val(cell.cellCode);
        $("select[name=distrct]").val(cell.district);
        $("select[name=distrct]").change();
        $("select[name=area]").val(cell.area);
        $("input[name=addr]").val(cell.address);
        if (cell.completionDate != 0) {
            $("input[name=ddlUsedYear]").val(cell.completionDate);
        }
    	
        if ($("#postType").val() == 0) {
            avgPrice1 = (cell.avgPrice == null) ? 0 : cell.avgPrice;
            var avgPrice = ((cell.avgPrice == null || cell.avgPrice == 0) ? "暂无" : cell.avgPrice);
            $("#showPrice").html("【搜房均价：<font style='color:red'>" + avgPrice + "</font>&nbsp;元/平米】");
            $("#showPrice").show();
        }
	    	
        focusAndBlur("cell");
        focusAndBlur("addr");
        if(vaildForm.check(false, "cell")) {
            changeTip("cell");
        }
    	
        //getModelImgs(cell.cellCode);
        getOutImgs(cell.cellCode);
        $("#div_IMG_O").html("");
    });
    
    function getModelImgs(cellCode) {
        $.ajax({
            url: url_9,
            type: "post",
            data: {
                "pageNow" : 1,
                "imageType" : "IMG_M",
                "cellCode" : cellCode
            },
            dataType: "text",
            success: function(result) {
                $("#container_IMG_M").html(result);
                if (result.indexOf("selectPic-content") > 0) {
                    $("#container_IMG_M-content").show();
                }else{
                    $("#container_IMG_M-content").hide();
                }
            }
        });
    }
    
    function getOutImgs(cellCode) {
        $.ajax({
            url: url_9,
            type: "post",
            data: {
                "pageNow" : 1,
                "imageType" : "IMG_O",
                "cellCode" : cellCode
            },
            dataType: "text",
            success: function(result) {
                $("#container_IMG_O").html(result);
                $("#container_IMG_O-content").hide();
            }
        });
    }
    
    if ($("#cellCode").val() != "0") {
        //getModelImgs($("#cellCode").val());
        getOutImgs($("#cellCode").val());
    }
    
    $(".containerImg img").die().live("click", function() {
        art.dialog({
            left: "10%",
            top: "3%",
            content: '<img style="max-width: 600px; max-height: 450px;" src="' + 
					$(this).attr("src").replace("_small", "") + '"/>'
        });
    });
	
    $(".importp-img").die().live("click", function() {
        art.dialog({
            left: "10%",
            top: "3%",
            content: '<img style="max-width: 600px; max-height: 450px;" src="' + 
					$(this).attr("src").replace("_small", "").replace(imageW116H116Q85,"") + '"/>'
        });
    });
    
    $("input[name=image_check]").die().live("click", function() {
        var parent = $(this).parents(".containerImg");
        var imgType = parent.attr("imgType");
		
        if ($(this).attr("checked") == "checked") {
            if(! checkImgNum(imgType, 1)) {
                $(this).attr("checked", false);
                return;
            }
            imgIndex = imgIndex + 1;
            createImgDiv($(this).attr("imgUrl"), imgType, '', imgIndex);
        } else {
            var smallImg = getSmallImg($(this).attr("imgUrl"));
            var tag = $("#div_" + imgType + " img[src='" + smallImg + "']");
            if (tag == null) return;
            tag.parent().find(".upload-delete").click();
        }
    });
    
    $("#house_desc_btn").die().live("click", function() {
        dialog = art.dialog.open( '/house/getHouseDescs?postType=' + $("#postType").val(), {
            title: '我的描述',
            width: 698,
            height: 450
        });
    });
    
    $("#agent_desc_btn").die().live("click", function() {
        dialog = art.dialog.open($("#basePath").val() + '/house/getAgentDescs?postType=' + $("#postType").val(), {
            title: '中介模板',
            width: 630,
            height: 450
        });
    });
    
    $("#save_desc_btn").die().live("click", function() {
        // var descContent = CKEDITOR.instances.houseDescribe.getData();
        var descContent = UE.getEditor('houseDescribe').getContent();
        var html = '<div style="font-size:14px;"><span>模板类型：</span><select style="font-size:14px;" id="desc_postType"><option value="0">通用</option><option value="1">出售</option><option value="3">出租</option></select></div>';
        html += '<div style="margin-top:5px;font-size:14px;"><span>模板名称：<input style="font-size:14px;width:420px;" id="desc_descName" value="" /></span></div>';
    	
        art.dialog({
            title: "新增模板",
            content: html,
            init: function() {
                $("#desc_postType").val($("#postType").val());
                $("#desc_descName").val($("#saleHouse-title").val());
            },
            ok: function() {
                var postType = $("#desc_postType").val();
                var descName = $("#desc_descName").val();
                var bValid = true;
                bValid = bValid && checkRangeWithHint(descName.length, 1, 35, "模板名称");
                //var contentLen = strlen(CKEDITOR.instances.houseDescribe.document.getBody().getText()); 
                var desc = trim(UE.getEditor('houseDescribe').getContent().replace(/<[^>]+>/g,""));
                desc = desc.replace(/&nbsp;/ig,"");
                desc = trimAll(desc,"g");
                var contentLen = desc.length;

                bValid = bValid && checkRangeWithHint(contentLen, 30, 3000, "房源描述内容");
                if(! bValid) return;
        		
                $.ajax({
                    url: "/house/addHouseDesc",
                    type: "post",
                    data: {
                        descName:  descName,
                        descContent: descContent,
                        postType: postType
                    },
                    success: function (data) {

                        if (data.hid < 1) {
                            art.dialog.alert(data.msg);
                            return;
                        } else {
        			   
                            art.dialog.alert("保存成功"); }
                    },
                    error: function(jqXHR) {
                        art.dialog.alert($.parseJSON(jqXHR.responseText).msg);
                    }
                });
            },
            cancel: true
        });
    });
    
    $(".my_img").die().live("click", function() {
        if ($("#cellCode").val() == 0) {
            alert("请选择一个我们提供的小区!");
            return;
        }
    	
        $("#temp_images").val('');// 将隐藏域的值清空
    	
        var imgType = $(this).attr("imageType");
        var cellCode = $("#cellCode").val();
        getMyImages(imgType, cellCode);
    });
    
    $(".my_area").die().live("click", function() {
        if ($("#cellCode").val() == 0) {
            alert("请选择一个我们提供的小区!");
            return;
        }
        var htmlResult = $("#container_IMG_O").html();
        if (htmlResult.indexOf("selectPic-content") > 0) {
            $("#container_IMG_O-content").slideDown()
        }else{
            alert("暂时没有小区图片,请立即上传。");
        }
    });
    
    $(".public_img").die().live("click", function() {
        var cellCode = $("#cellCode").val();
        if (cellCode == 0) {
            alert("请选择一个我们提供的小区!");
            return;
        }
        $("#temp_images").val('');// 将隐藏域的值清空
    	
        var imgType = $(this).attr("imageType");
        getPublicImages(imgType, cellCode);
    });
    
    $(".share_img").die().live("click", function() {
        var cellCode = $("#cellCode").val();
        if (cellCode == 0) {
            alert("请选择一个我们提供的小区!");
            return;
        }
    	
        $("#temp_images").val('');// 将隐藏域的值清空
    	
        var imgType = $(this).attr("imageType");
        getShareImages(imgType, cellCode);
    });
    
    $("#saleHouse-title").die().live("keyup", function() {
        var titleLength = 0;
        var regexp = /(^\s*)|(\s*$)/g;
        var gets = $(this).val().replace(regexp, "");
        for (var i = 0; i < gets.length; i++) {
            var c = gets.charAt(i);
            if (' ' == c) {
                titleLength += 0.5;
            } else {
                titleLength += 1;
            }
        }
        var num = 30 - titleLength;
        $("#titleNumsSpan").html("已输入" + titleLength + "字，还可输入" + num + "字");
    });
    
});

/*Tab页面的切换*/
$(function(){
	
    /*TAB切换*/
    $("#saleHouse-basic-house").click(function(){
        houseDiv();
    });
	
    $("#saleHouse-basic-villa").click(function(){
        villaDiv();
    });
	
    $("#saleHouse-basic-shop").click(function(){
        shopDiv();
    });
	
    $("#saleHouse-basic-office").click(function(){
        officeDiv();
    });
	
    $("#saleHouse-basic-factory").click(function(){
        factoryDiv();
    });
	
});


function showHouseLabelWindow(houseLabels) {
    $(".houseLabel-window").html(houseLabels);
    var left = $(".saleHouse-house-tip").offset().left;
    var top = $(".saleHouse-house-tip").offset().top;
    $(".houseLabel-window").show();
    $(".houseLabel-window").css({"left":left, "top":top + 18});
}


function singleClick() {
    $.each(typeArray, function(i, imgType) {
        $("#singleDiv_" + imgType).show();
        $("#mutilDiv_" + imgType).hide();
        $("#compressDiv_" + imgType).hide();
    });
}

function mutilDivClick() {
    $.each(typeArray, function(i, imgType) {
        $("#singleDiv_" + imgType).hide();
        $("#mutilDiv_" + imgType).show();
        $("#compressDiv_" + imgType).hide();
    });
}

function compressClick() {
    $.each(typeArray, function(i, imgType) {
        $("#singleDiv_" + imgType).hide();
        $("#mutilDiv_" + imgType).hide();
        $("#compressDiv_" + imgType).show();
    });
}

function houseDiv() {
    $("#saleHouse-basic-house").addClass("selected");
    $("#saleHouse-basic-house").siblings(".sale-sub-menu").removeClass("selected");
    $(".saleHouse-basic-house").show();
    $(".saleHouse-basic-villa").hide();
    $(".saleHouse-basic-shop").hide();
    $(".saleHouse-basic-office").hide();	
    $(".saleHouse-basic-factory").hide();
    $("#tip_houseType").html('<a class="tip_houseType" srcollType="houseType" href="javascript:importScroll(\'houseType\')">住宅信息</a>');
    changeBuildingType(1);
}
function villaDiv() {
    $("#saleHouse-basic-villa").addClass("selected");
    $("#saleHouse-basic-villa").siblings(".sale-sub-menu").removeClass("selected");
    $(".saleHouse-basic-house").hide();
    $(".saleHouse-basic-villa").show();
    $(".saleHouse-basic-shop").hide();
    $(".saleHouse-basic-office").hide();	
    $(".saleHouse-basic-factory").hide();
    $("#tip_houseType").html('<a class="tip_villaType" srcollType="villaType" href="javascript:importScroll(\'villaType\')">别墅信息</a>');
    changeBuildingType(2);
}
function shopDiv() {
    $("#saleHouse-basic-shop").addClass("selected");
    $("#saleHouse-basic-shop").siblings(".sale-sub-menu").removeClass("selected");
    $(".saleHouse-basic-house").hide();
    $(".saleHouse-basic-villa").hide();
    $(".saleHouse-basic-shop").show();
    $(".saleHouse-basic-office").hide();
    $(".saleHouse-basic-factory").hide();
    $("#tip_houseType").html('<a class="tip_shopType" srcollType="shopType" href="javascript:importScroll(\'shopType\')">商铺信息</a>');
    changeBuildingType(3);
}
function officeDiv() {
    $("#saleHouse-basic-office").addClass("selected");
    $("#saleHouse-basic-office").siblings(".sale-sub-menu").removeClass("selected");
    $(".saleHouse-basic-house").hide();
    $(".saleHouse-basic-villa").hide();
    $(".saleHouse-basic-shop").hide();
    $(".saleHouse-basic-office").show();
    $(".saleHouse-basic-factory").hide();
    $("#tip_houseType").html('<a class="tip_officeType" srcollType="officeType" href="javascript:importScroll(\'officeType\')">写字楼信息</a>');
    changeBuildingType(4);
}
function factoryDiv() {
    $("#saleHouse-basic-factory").addClass("selected");
    $("#saleHouse-basic-factory").siblings(".sale-sub-menu").removeClass("selected");
    $(".saleHouse-basic-house").hide();
    $(".saleHouse-basic-villa").hide();
    $(".saleHouse-basic-shop").hide();
    $(".saleHouse-basic-office").hide();
    $(".saleHouse-basic-factory").show();
    $("#tip_houseType").html('<a class="tip_factoryType" srcollType="factoryType" href="javascript:importScroll(\'factoryType\')">厂房信息</a>');
    changeBuildingType(5);
}

function changeBuildingType(i) {
    $("#buildingType").val(i);
    if ($("#postType").val() == 3) {
        if (i == 1 || i == 2) {
            $("#priceTypeSpan").show();
            $("#priceTypeSpan3").hide();
            changePrice(200);
        } else {
            $("#priceTypeSpan").hide();
            $("#priceTypeSpan3").show();
            changePrice(1);
        }
    } else {
        $("#priceTypeSpan2").show();
        changePrice(3);
    }
}

function changePrice(val) {
    if ($("#price").val() == '') {
        $("#price").val(val);
    } else {
        $("#price")[0].focus();
        var val = $("#price").val();
        $("#price").val(0);
        $("#price")[0].blur();
        $("#price")[0].focus();
        $("#price").val(val);
        $("#price")[0].blur();
    }
}

//重定向到房源管理
function redirectManager() {
    postType = $('#postType').val();
    if (postType == 0) {
        id = "houseManagerSell";
    } else {
        id = "houseManagerRent";
    }
    top.$("#" + id).click();
    href = top.$("#" + id).attr("href");
    url = href.substring(href.indexOf("'") + 1, href.lastIndexOf("'"));
    var isDraft = ($("#draftType").val() > 0) ? 1 : 0;
    url += "&buildingType=" + $("#buildingType").val()
			+ "&isDraft=" + isDraft;
    top.$("#"+id).parents(".menu").addClass("menu_open");
    top.$("#"+id).parents(".menu").siblings().removeClass("menu_open");
    top.hrefLink(url);
}

function getMyImages(imgType, cellCode) {
    if (typeof(cellCode) == "undefined") {
        cellCode = $("#cellCode").val();
    }
    art.dialog({id: 'dlg_my_images'}).close();
    var url = $("#basePath").val() + '/import/getMyImages.do?imageType=' + imgType + "&cellCode=" + cellCode;
    dialog = art.dialog.open(encodeURI(url), {
        id: "dlg_my_images", 
        title: '我的图库',
        width: 798,
        height: 400,
        ok: function () {
            var urls = $("#temp_images").val().split(",");
            for (var i = 0; i < urls.length; i++) {
                if (urls[i] != '') {
                    imgIndex = imgIndex + 1;
                    createImgDiv(urls[i], imgType, '',imgIndex);
                }
            }
        },
        cancel: true
    });
}

function getPublicImages(imgType, cellCode) {
    if (typeof(cellCode) == "undefined") {
        cellCode = $("#cellCode").val();
    }
    art.dialog({id: 'dlg_public_images'}).close();
    var url = $("#basePath").val() + '/import/getPublicImages.do?imageType=' + imgType + "&cellCode=" + cellCode;
    dialog = art.dialog.open(encodeURI(url), {
        id: "dlg_public_images", 
        title: '公共图库',
        width: 798,
        height: 400,
        ok: function () {
            var urls = $("#temp_images").val().split(",");
            for (var i = 0; i < urls.length; i++) {
                if (urls[i] != '') {
                    imgIndex = imgIndex + 1;
                    createImgDiv(urls[i], imgType, '',imgIndex);
                }
            }
        },
        cancel: true
    });
}

//图片跨域上传
try {
	
} catch (e) {}

function getShareImages(imgType, cellCode) {
    if (typeof(cellCode) == "undefined") {
        cellCode = $("#cellCode").val();
    }
    art.dialog({id: 'dlg_share_images'}).close();
    var url = $("#basePath").val() + '/import/getShareImages.do?imageType=' + imgType + "&cellCode=" + cellCode;
    dialog = art.dialog.open(encodeURI(url), {
        id: "dlg_share_images", 
        title: '共享图库',
        width: 798,
        height: 450,
        ok: function () {
            var urls = $("#temp_images").val().split(",");
            for (var i = 0; i < urls.length; i++) {
                if (urls[i] != '') {
                    imgIndex = imgIndex + 1;
                    createImgDiv(urls[i], imgType, '',imgIndex);
                }
            }
        },
        cancel: true
    });
}

function setDescContent(descContent) {
    //CKEDITOR.instances.houseDescribe.setData(descContent);
    UE.getEditor('houseDescribe').setContent(descContent);
    dialog.close();
}

function setTempImages(tempImages) {
    $("#temp_images").val(tempImages);
}

function setPersonConfig(watermark,pos,img) {
    $("#personConfig-watermark").val(watermark);
    $("#personConfig-pos").val(pos);
    $("#personConfig-img").val(img);
}

/**
 * 验证正整数
 * @param evt
 * @returns {Boolean}
 */
function checkNumber(evt){
    var k = window.event ? evt.keyCode : evt.which;
    if (k<=57 && k>=48){  
        return true;  
    } else if (k == 8) {
        return true;
    } else {  
        return false;
    }
}

/**
 * 验证整数
 * @param evt
 * @returns {Boolean}
 */
function checkInteger(evt){
    var k = window.event ? evt.keyCode : evt.which;
    if (k<=57 && k>=48){  
        return true;  
    } else if (k == 8) {
        return true;
    } else if (k == 45) {
        return true;
    } else {  
        return false;
    }
}


/**
 * 验证小数
 * @param evt
 * @returns {Boolean}
 */
function checkFloat(evt, thisObj){
    var k = window.event ? evt.keyCode : evt.which;
    if (k<=57 && k>=48){
        return true;  
    } else if (k == 8) {
        return true;
    } else if (k == 46) {
        if ($(thisObj).val() == '' || $(thisObj).val().indexOf(".") > 0) {
            return false;
        }
        return true;
    } else {  
        return false;
    }
}

var isIE = /msie/i.test(navigator.userAgent) && !window.opera;
function getFileSize(target) {
    var fileSize = 0;
    if (isIE && !target.files) {
        //    	var filePath = target.value;
        //    	var fileSystem = new ActiveXObject("Scripting.FileSystemObject");
        //    	var file = fileSystem.GetFile (filePath);
        //    	fileSize = file.Size;
        return true;
    } else {
        fileSize = target.files[0].size;
    }
    var size = fileSize / 1024;

    if (size > (500)) {
        alert("附件不能大于500kb");
        return false;
    }
    return true;
}

var subFlag = true;
var imgIndex = 0;
// 单张上传
function uploadImg(type, i) {
    if ($("#" + type + i).val() == '') {
        alert("请先选择上传文件！");
        return;
    }
    target = document.getElementById(type + i);
    if (!getFileSize(target)) {
        return ;
    }
	
    if (checkImgNum(type, 1)) {
        createNewUpload(type, i);
		
        imgIndex = imgIndex + 1;
        createIngImg(type, imgIndex);
		
        wmUrl = $("#hideWaterMarkPic").val();
        wmPos = $("#hideWaterMarkPosition").val();
		
        $.ajaxFileUpload ({
            fileElementId: type + i,  //文件选择框的id属性
            url: upload_url,
            secureuri: false,
            data: {
                city: $("#city").val(),
                imageType: type,
                wmUrl: wmUrl,
                wmPos: wmPos,
                imgNo : imgIndex
            },
            dataType: 'json'
        });
    }
}

// 批量上传
$(function(){
    $.each(typeArray, function(i, imgType){
        $("#uploadify_" + imgType).uploadify({
            'button_image_url':  '/images/grey.gif',
            'swf': '/Scripts/plugins/uploadify/uploadify.swf',
            'uploader' : upload_url,//后台处理的请求
            'folder' : 'uploads',//您想将文件保存到的路径
            'queueID' : 'fileQueue_' + imgType,//与下面的id对应
            'queueSizeLimit' : 10,
            'removeTimeout' : 1,
            'preventCaching' : false,
            'fileTypeDesc' : '图片文件',
            'fileTypeExts' : '*.jpg;*.jpeg;*.gif;*.png;*.bmp',
            'fileSizeLimit' : '500KB',
            'method' : 'post',
            'formData' : {
                'city' : $("#city").val(),
                'imageType' : imgType,
                'wmUrl': $("#hideWaterMarkPic").val(),
                'wmPos': $("#hideWaterMarkPosition").val()
            },
            'auto' : true,
            'multi' : true,
            'uploadLimit' : 10,
            'buttonText' : '上传图片',
            //'itemTemplate' : '<div></div>',
            'onFallback' : function() {
                $(".picupload2-tip").hide();
                $("#picupload1").attr("checked","checked");
                $("#picupload2").attr("disable",true);
                $(".picupload1-tip").show();
                alert("您的浏览器不支持多图上传");
                singleClick();
            },
            'onUploadStart' : function(file) {
                if (!checkImgNum(imgType, 1)) {// 超过张数时，清空剩余队列
                    //	        		$("#uploadify_" + imgType).uploadify('stop');
                    $("#uploadify_" + imgType).uploadify('cancel', '*');
                } else {
                    createIngImg(imgType, file.id, file.id);
                }
            },
            'onUploadProgress' : function(file, bytesUploaded, bytesTotal, totalBytesUploaded, totalBytesTotal) {
                $('#progress').val(file.name + ": " + bytesUploaded + "/" + bytesTotal);
            },
            'onUploadSuccess' : function(file, data, response) {
                var str = "parent.callback(";
                var index = data.indexOf(str) + str.length;
                var strs = data.substring(index, data.indexOf(");")).split(",");
                var imgUrl = strs[1].substring(2, strs[1].length - 1);
	        	
                callback(imgType, imgUrl, file.id);
            }
        });
    });   
});

//批量压缩上传
var uploader_IMG_M;
var uploader_IMG_I;
var uploader_IMG_O;
$(function(){
    $.each(typeArray, function(i, imgType){
        var $list = $('#fileList_' + imgType);
		
        var limitNum;
        if (i == 0) {
            limitNum = 5;
        } else {
            limitNum = 10;
        }
		
        // 初始化Web Uploader
        var uploader = new plupload.Uploader({
            runtimes : 'html5,flash,silverlight,html4',
            browse_button: 'filePicker_' + imgType,
            container: document.getElementById('compressDiv_' + imgType),
            url: upload_url,
            flash_swf_url: '/Scripts/plugins/plupload-2.1.2/js/Moxie.swf',
            silverlight_xap_url: '/Scripts/plugins/plupload-2.1.2/js/Moxie.xap',
            filters : {
                max_file_size : '10mb',
                mime_types: [
					{title : "Image files", extensions : "gif,jpg,jpeg,bmp,png"}
                ]
            },
            multipart_params: {
                city: $("#city").val(),
                imageType: imgType,
                wmUrl: $("#hideWaterMarkPic").val(),
                wmPos: $("#hideWaterMarkPosition").val()
                //token: $("#qiniuToken").val()
				
            },
            resize: {
                width: 800,
                height: 800,
                crop: false,
                quality: 90,
                preserve_headers: true
            },
			
            init: {
                PostInit: function() {
                    //					document.getElementById('filelist').innerHTML = '';
                 
                },
                BeforeUpload:function(){
                    this.setOption("multipart_params", {
                        city: $("#city").val(),
                        imageType: imgType,
                        wmUrl: $("#hideWaterMarkPic").val(),
                        wmPos: $("#hideWaterMarkPosition").val()
                    });
                },
                FilesAdded: function(up, files) {
                    var uploadObject;
                    if (imgType == 'IMG_M') {
                        uploadObject = uploader_IMG_M;
                    } else if (imgType == 'IMG_I') {
                        uploadObject = uploader_IMG_I;
                    } else {
                        uploadObject = uploader_IMG_O;
                    }
                    $.each(files, function(i, file) {
                        fileId = file.id;
						
                        if (!checkImgNum(imgType, 1)) {// 超过张数时，清空剩余队列
                            uploadObject.removeFile(file);
                        } else {
                            createIngImg(imgType, fileId, fileId);
				        	
                            var $li = $('<div id="' + fileId + '" class="file-item thumbnail"></div>');
                            $list.append($li);
                            createProgress(file);//生成进度条
                        }
                    });
                    uploadObject.start();
                },

                UploadProgress: function(up, file) {
                    var $li = $('#' + file.id);
                    if ($li != undefined && $li != null) {
                        var $percent = $li.find('.progress');
						
                        p = file.percent;
                        $percent.find('.data').html(" - " + p + "%");
                        $percent.find('.uploadify-progress-bar').css("width", p + "%");
                    }
                },

                Error: function(up, err) {
                    alert(err.code + ": " + err.message);
                },
                FileUploaded: function(uploader,file,responseObject) {
                    $('#' + file.id).addClass('upload-state-done');
                    var data = responseObject.response;
                    var str = "parent.callback(";
                    var index = data.indexOf(str) + str.length;
                    var strs = data.substring(index, data.indexOf(");")).split(",");
                    var imgUrl = strs[1].substring(2, strs[1].length - 1);
		           	
                    callback(imgType, imgUrl, file.id);
                },
                UploadComplete: function(up,files) {
                    $.each(files, function(i, file) {
                        var $li = $('#' + file.id);
                        if ($li != undefined && $li != null) {
                            $li.find('.progress').remove();
                        }
                    });
                }
            }
        });
		
        function createProgress(file) {
            var $li = $('#'+file.id ),
	        $percent = $li.find('.progress');
	
            var fileSize = Math.round(file.size / 1024);
            var o = "KB";
            if (fileSize > 1024) {
                fileSize = Math.round(fileSize / 1024);
                o = "MB";
            }
            var l = fileSize.toString().split(".");
            fileSize = l[0];
            if (l.length > 1) {
                fileSize += "." + l[1].substr(0, 2);
            }
            fileSize += o;
            var fileName = file.name;
            if (fileName.length > 25) {
                fileName = fileName.substr(0, 25) + "...";
            }
		    
            // 避免重复创建
            if (!$percent.length ) {
                $percent = $('<div class="uploadify-queue-item progress"><span class="fileName">' + fileName + ' (' + fileSize + ')</span><span class="data"></span><div class="uploadify-progress"><div class="uploadify-progress-bar"><!--Progress Bar--></div></div></div>')
		                .appendTo( $li );
            }
        }
		
        uploader.init();
		
        if (imgType == 'IMG_M') {
            uploader_IMG_M = uploader;
        } else if (imgType == 'IMG_I') {
            uploader_IMG_I = uploader;
        } else {
            uploader_IMG_O = uploader;
        }
        uploader = null;
    });
});

function createNewUpload(type, i) {
    $("#" + type + i).hide();
    $("#upload_" + type + i).hide();
    var obj = $("#upload_" + type + i);
	
    if (i == '') {
        i = 1;
    }
    i = parseInt(i);
    i++;
    var html = '<input id="' + type + i + '" type="file" name="file">'
				+ '<input id="upload_' + type + i + '" class="btn-green" type="button"'
				+ 'value="上传" onclick="javascript:uploadImg(\'' + type + '\',\'' + i + '\')">';
    obj.after(html);
}

//图片数量验证
function checkImgNum(type, num) {
    if (type == 'IMG_M') {
        if (($("#div_" + type).children().length + num) > 5) {
            alert("该类型最多能上传5张");
            return false;
        }
    } else {
        if (($("#div_" + type).children().length + num) > 10) {
            alert("该类型最多能上传10张");
            return false;
        }
    }
    return true;
}

//创建加载中图片
function createIngImg(type, imgIndex, fileId) {
    var imgUrl = '/images/uploadIng.gif';
    createImgDiv(imgUrl, type, '', imgIndex, fileId);
    $("#imgUrl_" + type + "_" + imgIndex).val('ing');
}

// 上传完成后返回
function callback(imgType, imgUrl, imgIndex) {
    if (imgUrl.indexOf("http://") < 0) {
        alert(imgUrl);
        $("#img_" + imgType + "_" + imgIndex).attr("src", '/images/uploadFail.jpg');
        $("#imgUrl_" + imgType + "_" + imgIndex).val('fail');
    } else {
        $("#imgUrl_" + imgType + "_" + imgIndex).val(imgUrl);
        $("#img_" + imgType + "_" + imgIndex).attr("src", getSmallImg(imgUrl));
    }
}

// 获取缩略图
function getSmallImg(imgUrl) {
    if (imgUrl.indexOf("uploadIng.gif") > -1
			|| imgUrl.indexOf("uploadFail.jpg") > -1) {
        return imgUrl;
    }
	
    smallImg = imgUrl +imageW116H116Q85;
    return smallImg;
    //   return imgUrl;
}

// 生成上传图片信息
function createImgDiv(imgUrl, imgType, imgDescribe, imgIndex, fileId) {
    var i = $("#div_" + imgType).children().length + 1;
    smallImg = getSmallImg(imgUrl);
	
    var html = '<div id="import_' + imgType + "_" + imgIndex + '" class="import-pic" style="float : left;margin : 0 8px; display:inline">'
				+ '<img id="img_' + imgType + "_" + imgIndex + '" class="importp-img" src="' + smallImg + '"/>'
				+ '<div class="moveList"><a title="向左移动" href="javascript:" onclick="moveLeft(this, \'' + imgType + '\')" class="upload-left"></a>'
				+ '<a title="向右移动" href="javascript:" onclick="moveRight(this, \'' + imgType + '\')" class="upload-right"></a>'
				+ '<a title="设为封面" id="cover_' + imgType + "_" + imgIndex + '" href="javascript:setCover(\''+ imgType + '\',\'' + imgIndex + '\')" class="upload-index"></a>'
				+ '<a title="删除图片" href="javascript:" onclick="deleteImg(this, \'' + imgType + '\',\'' + imgIndex + '\',\'' + fileId + '\')" class="upload-delete"></a></div>'
				+ '<input id="desc_' + imgType + "_" + imgIndex + '" class="import-img-font" style="margin:0 0 8px 0" type="text" value="' + imgDescribe + '" name="imgDescribe" placeholder="请填写图片说明" maxlength="12"></input>'
				+ '<input type="hidden" name="imageType" value="' + imgType + '"/>'
				+ '<input type="hidden" id="imgUrl_' + imgType + "_" + imgIndex + '" name="imgUrl" value="' + imgUrl + '"/>'
				+ '<div id="fileQueue_IMG_M"></div>';
    if(imgType == "IMG_I"){
        html += '<ul class="import-img-tip" style="display:none">'
              + '<li>卧室</li>'
              + '<li>客厅</li>'
              + '<li>阳台</li>'
              + '<li>卫生间</li>'
              + '<li>厨房</li>'
              + '<li>储物间</li>'
              + '<li>书房</li>'
              + '<li>餐厅</li>'
              +'</ul>'; 
    }	
    html += '</div>';
    $("#div_" + imgType).append(html);
    $(".import-img-font").placeholder();
    importImgTip();
}

// 图片左移
function moveLeft(obj, type) {
    var curDiv = $(obj).parents(".import-pic");
    var preDiv = $(obj).parents(".import-pic").prev();
    if (preDiv.length > 0) {
        $("#isChange").val(1);
        var imgDesc = $(curDiv).find(".import-img-font").val();
        var imgDescPre = $(preDiv).find(".import-img-font").val();
		
        var html = preDiv.html();
        preDiv.html(curDiv.html());
        curDiv.html(html);
		
        $(curDiv).find(".import-img-font").val(imgDescPre);
        $(preDiv).find(".import-img-font").val(imgDesc);
		
        importImgTip();
        //h5Sort(type);
    }
}

//图片右移
function moveRight(obj, type) {
    var curDiv = $(obj).parents(".import-pic");
    var nextDiv = $(obj).parents(".import-pic").next();
    if (nextDiv.length > 0) {
        $("#isChange").val(1);
        var imgDesc = $(curDiv).find(".import-img-font").val();
        var imgDescNext = $(nextDiv).find(".import-img-font").val();
		
        var html = nextDiv.html();
        nextDiv.html(curDiv.html());
        curDiv.html(html);
		
        $(curDiv).find(".import-img-font").val(imgDescNext);
        $(nextDiv).find(".import-img-font").val(imgDesc);
		
        importImgTip();
        //h5Sort(type);
    }
}

// 第几张图重新排序
function h5Sort(type) {
    var i = 0;
    $("#div_" + type).children().each(function(){
        i = i + 1;
        $(this).find("h5").html("第" + i + "张图");  
    });
}

function setCover(type, i) {
    var coverIndex = $("#coverIndex").val();
    if (coverIndex != "") {
        $("#cover_" + coverIndex).show();
    }
    $("#coverIndex").val(type + "_" + i);
    $("#cover_" + type + "_" + i).hide();
	
    var imgUrl = $("#imgUrl_" + type + "_" + i).val();
    if (imgUrl == 'ing' || imgUrl == 'fail') {
        alert('上传中或上传失败的图片不可作为封面');
        return;
    }
    smallImg = getSmallImg(imgUrl);
    $("#coverImg").attr("src", smallImg);
    $("#coverUrl").val(imgUrl);
    $("#coverType").val(type);
    $("#coverImg").show();
}

function deleteImg(obj, imgType, i, fileId) {
    var url = $("#imgUrl_" + imgType + "_" + i).val();
    var coverUrl = $("#coverUrl").val();
    if (url == coverUrl) {
        alert("已设为封面，不可删除！");
        return;
    }
	
    $(obj).parents(".import-pic").remove();
    $("#isChange").val("1");
    //h5Sort(imgType);
    if (fileId.indexOf("WU_FILE") > -1) {
        if (imgType == 'IMG_M') {
            uploader_IMG_M.stop(fileId);
        } else if (imgType == 'IMG_I') {
            uploader_IMG_I.stop(fileId);
        } else {
            uploader_IMG_O.stop(fileId);
        }
        $('#' + fileId).find('.progress').remove();
    } else if (fileId != "undefined") {
        $("#uploadify_" + imgType).uploadify('cancel', fileId);
    }
}

/** 修改物业栏位编辑状态*/
function changeFee2(i) {
    if (i == 0) {
        $('#fee2').attr('disabled', true);
    } else {
        $('#fee2').attr('disabled', false);
    }
}

/** 修改物业栏位编辑状态*/
function changeFee3(i) {
    if (i == 0) {
        $('#fee3').attr('disabled', true);
    } else {
        $('#fee3').attr('disabled', false);
    }
}

// 另存为提交
function otherSub() {
    $("#houseid").val(0);
    $("#isChange").val(1);
    $("#bForm").submit();
}

function trim(str){   
    str = str.replace(/^(\s|\u00A0)+/,'');   
    for(var i=str.length-1; i>=0; i--){   
        if(/\S/.test(str.charAt(i))){   
            str = str.substring(0, i+1);   
            break;   
        }   
    }   
    return str;
}  

var autoFlag = true;
var autoDialog;
var artTitle;
var height;
var viewUrl;
function getAutoView(i) {
    if (autoFlag) {
        autoFlag = false;
        if (vaildForm.check()) {
            if (i == 1) {
                artTitle = '帮我写标题';
                height = 380;
                viewUrl =  url_2;
            } else if (i == 2) {
                artTitle = '帮我写描述';
                height = 450;
                viewUrl =  url_4;
            }
            $.ajax({
                url : viewUrl,
                type : "post",
                data : $("#bForm").serialize(),
                dataType : "text",
                beforeSend: function(XMLHttpRequest) {
                    top.loadingShow();
                    return true;
                },
                success: function(data) {
                    autoDialog = art.dialog({
                        title: artTitle,
                        content : data,
                        width: 600,
                        height: height
                    });
                },
                error: function(jqXHR) {
                    alert($.parseJSON(jqXHR.responseText).msg);
                },
                complete:function(XMLHttpRequest, data) {
                    top.loadingHide();
                    autoFlag = true;
                }
            });
        } else {
            autoDialog = art.dialog({
                title: "提示",
                content : "为了生成更为详细的信息，请先填写必填项",
                ok : true
            });
            autoFlag = true;
        }
    }
}

var draftFlag = true;

function saveDraft(draftType) {
    if (draftFlag) {
        if(!checkImgs()) {
            return false;
        }
        $("#draftType").val(draftType);
        //var desc = CKEDITOR.instances.houseDescribe.getData();
        var desc = UE.getEditor('houseDescribe').getContent();
        $("#houseDescribe").val(desc);
        $.ajax({
            url: $("#basePath").val() + url_6,
            data : $("#bForm").serialize(),
            type: "post",
            cache : false,
            dataType: "json",
            beforeSend: function(XMLHttpRequest) {
                draftFlag = false;
                top.loadingShow();
            },
            success: function(data) {
                $("#isChange").val(0);
                redirectManager(1);
            },
            error: function(jqXHR) {
                alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete:function(XMLHttpRequest, data) {
                top.loadingHide();
                draftFlag = true;
            }
        });
    }
}

/*全选 */
function selectAll(name){
    $("input[name="+name+"]").each(function(){
        $(this).attr("checked",true);
    });
}
/*取消全选*/

function clearAll(name){
    $("input[name="+name+"]").each(function(){
        $(this).attr("checked",false);
    });
}
 
/*加1*/
function addone(on,all,onOrAll){
    var number = ($("#"+on).val() == '') ? 0 : parseInt($("#"+on).val());
    number += 1;
    if(number == 0){
        $("#"+on).val(1); 
    }else{
        $("#"+on).val(number);
    }
    focusAndBlur(on);
}
 
/*减1*/
function minusone(on,all,onOrAll){
    var number = ($("#"+on).val() == '') ? 0 : parseInt($("#"+on).val());
    number -= 1;
    if(number == 0){
        $("#"+on).val(1); 
    }else{
        $("#"+on).val(number);
    }
    focusAndBlur(on);
}
 
// 计算最低首付
function countLowPay() {
    var price = $("#price").val();
	 
    price = Math.ceil(price * 0.3);
	 
    $("#lowPay").val(price);
}
 
/*提示内容*/
function tipContent(id, content){
    content = "<span class='col-f60 tipContent'>" + content + "</span>";
    $("#"+id).parent().after(content);
}
 
function removeTip(id){
    $("#"+id).parent().next(".tipContent").remove();
}
 
 
function tip(id){ 
    var val = $("#"+id).val();
    var spanContent = $("#"+id).parent().next(".tipContent").html();
    removeTip(id);
    if(val == ""){
        tipContent(id, "*此为必填项");
        return false;
    }else{
        return checkImprot(id);
    }
};
 
/*楼层提示*/
function changefloor(on,all){
    var onVal = parseInt($("#"+on).val());
    var allVal = parseInt($("#"+all).val());
    var left = $("#"+on).offset().left;
    var top =  $("#"+on).offset().top;
    if(onVal>allVal){
        $(".hdtop").show(); 
        $(".hdtop").css({"left":left+10,"top":top-36});
    }else{
        $(".hdtop").hide(); 
    }
}

function tipWindowClose(){
    $(".tip-window").hide();
}
 
function titleWindowClose(id){
    $("#" + id).hide();
}
 
function focusAndBlur(id) {
    $("#" + id).focus();
    $("#" + id).blur();
}
 
function changeTip(id) {
    $("#" + id).removeClass("Validform_error");
    $("#" + id).parent().find(".Validform_checktip").html("通过信息验证！");
    $("#" + id).parent().find(".Validform_checktip").removeClass("Validform_wrong");
    $("#" + id).parent().find(".Validform_checktip").addClass("Validform_right");
}
 
function changeRightTip(id, tip) {
    $("#" + id).addClass("Validform_error");
    $("#" + id).parent().find(".Validform_checktip").html(tip);
    $("#" + id).parent().find(".Validform_checktip").removeClass("Validform_right");
    $("#" + id).parent().find(".Validform_checktip").addClass("Validform_wrong");
}
 
var waterFlag = true;
function getWatermark(){
    if (waterFlag) {
        $.ajax({
            url: '../ajax/personConfig/getWatermarksTip.do',
            type : "post",
            cache : false,
            dataType: "text",
            beforeSend: function(XMLHttpRequest) {
                waterFlag = false;
                top.loadingShow();
            },
            success: function(result) {
                art.dialog({
                    title: '公司水印',
                    content : result,
                    ok: function () {
                        saveWatermark();
                    },
                    padding:0,
                    width: 600,
                    height: 400,
                    cancel: true
                });
            },
            error: function(jqXHR) {
                alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete:function(XMLHttpRequest, data) {
                top.loadingHide();
                waterFlag = true;
            }
        });
    }
}
 
function saveWatermark() {
    $.ajax({
        type: 'post',
        url: '/PersonManage/SaveWaterMark',
        data: { position: $("#hideWaterMarkPosition").val() },
        success: function (result) {

        }
    });
}

function getHelp(){
    dialog = art.dialog.open('/House/GetHelp', {
        title: '上传图片帮助',
        width: 698,
        height: 500
    });
};

/*室内图下拉*/
function importImgTip(){
    $(".import-img-tip li").click(function() {
        var importImgContent = $(this).html();
        $(this).parents(".import-pic").find(".import-img-font").val(importImgContent);
        $(this).parents(".import-pic").find(".import-img-tip").hide();
    });
	
    $(".import-img-font").click(function() {
        $(this).parents(".import-pic").find(".import-img-tip").show();
    });
	
    $(".import-img-font").keyup(function() {
        $(this).parents(".import-pic").find(".import-img-tip").hide();
    });
	
    $(".import-img-tip").mouseleave(function () {  
        $(this).hide();
    });
	
    $(".importp-img").mouseleave(function () {  
        $(this).parents(".import-pic").find(".import-img-tip").hide();
    });
}

function kcTip() {
    alert("您的库存不足，请删除旧房源后再进行保存");
}

/**页面快捷滚动**/
var ifmObj = "";
function importScroll(id){
    $(".tip_"+id).parent("li").addClass("selected");
    $(".tip_"+id).parent("li").siblings(".sale-sub-menu").removeClass("selected");
    ifmObj = scrollBrowser();
    if(id == 'basic'){
        $(ifmObj).animate({scrollTop:0},500);
    }else{
        $(ifmObj).animate({scrollTop:$("#" + id).offset().top-40},500);
    }
}


$(window).bind("scroll",function() {//定义滚动条位置改变时触发的事件。
    importScrollView();
});


/**显示快捷滚动页面**/
function importScrollView(){
    ifmObj = scrollBrowser();
    var top = ifmObj.scrollTop;
    var srcollType = $("#tip_houseType").find("a").attr("srcollType");
    if( top >= 100 && top < $("#" + srcollType).offset().top-40 ) {
        $(".import-keyway").show(); 
        $(".import-keyway").find(".sale-sub-menu").removeClass("selected");
        $("#tip_basic").addClass("selected");
    }else if( top >= $("#" + srcollType).offset().top-40 && top < $("#house").offset().top-40 ){
        $(".import-keyway").find(".sale-sub-menu").removeClass("selected");
        $(".tip_" + srcollType).parent(".sale-sub-menu").addClass("selected");
    }else if( top >= $("#house").offset().top-40 && top < $("#pic").offset().top-40 ){
        $(".import-keyway").find(".sale-sub-menu").removeClass("selected");
        $("#tip_house").addClass("selected");
    }else if( top >= $("#pic").offset().top-40 ){
        $(".import-keyway").find(".sale-sub-menu").removeClass("selected");
        $("#tip_pic").addClass("selected");
    }else {
        $(".import-keyway").hide(); 
    }
}

/**
 * 右侧滚动条兼容性
 */
function scrollBrowser(){
    var ua = navigator.userAgent.toLowerCase();
    if(ua.indexOf("chrome") > 0){
        ifmObj = document.getElementsByTagName("body")[0];
    }else{
        ifmObj = document.documentElement;
    }
    return ifmObj;
}


/**检查flash版本**/
function flashChecker() {
    var hasFlash = false;　　　　 //是否安装了flash
    var flashVersion = 0;　　 //flash版本 
    if(document.all) {
        var swf = new ActiveXObject('ShockwaveFlash.ShockwaveFlash'); 
        if(swf) {
            hasFlash = true; 
            var VSwf = swf.GetVariable("$version"); 
            flashVersion = parseInt(VSwf.split(" ")[1].split(",")[0]); 
        }
    }else {
        if(navigator.plugins && navigator.plugins.length > 0) { 
            var swf = navigator.plugins["Shockwave Flash"]; 
            if(swf) {
                hasFlash = true; 
                var words = swf.description.split(" "); 
                for(var i = 0; i < words.length; ++i) {
                    if(isNaN(parseInt(words[i]))) continue; 
                    flashVersion = parseInt(words[i]); 
                }
            } 
        }
    }
    return {
        isHas: hasFlash, 
        version: flashVersion 
    }; 
}



///获取我的水印设置
function getWaterMarkSeting() {
    $.ajax({
        type: 'get',
        dataType: 'json',
        async:false,
        url: '/PersonManage/GetWaterMarkSeting',
        success: function (result) {
            $("#hideWaterMarkPic").val(result.Watermark);
            $("#hideWaterMarkPosition").val(result.WaterMarkPosition);
            var html = "";
            var imageLogo = "";
            var imageDisplay = "";
            if (result != null && result.Watermark != null && result.Watermark != "") {
                imageLogo = '<img src="' + result.Watermark + '" width="240" height="60">';
            }
            else {
                imageLogo = '<span class="col-fd6e05">暂无公司水印图,请联系客服添加。联系QQ：2605757603</span>';
            }
            if (result.WaterMarkPosition <= 0) {
                imageDisplay = "display:none;";
            }
            html += '<div class="personConfig-uploadType clear">' +
'							<div class="personConfig-watermark-right float-l">' +
'                               <input ' + (result.WaterMarkPosition == 0 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_0" type="radio"  value="0"><label for="watermark_position_0">不加水印</label>' +
'								<input ' + (result.WaterMarkPosition == 1 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_1" type="radio"  value="1"><label for="watermark_position_1">左上</label>' +
'								<input ' + (result.WaterMarkPosition == 2 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_2" type="radio"  value="2"><label for="watermark_position_2">左下</label>' +
'								<input ' + (result.WaterMarkPosition == 3 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_3" type="radio"  value="3"><label for="watermark_position_3">居中</label>' +
'								<input ' + (result.WaterMarkPosition == 4 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_4" type="radio"  value="4"><label for="watermark_position_4">右上</label>' +
'								<input ' + (result.WaterMarkPosition == 5 ? "checked='checked'" : "") + 'name="watermark_position" id="watermark_position_5" type="radio"  value="5"><label for="watermark_position_5">右下</label>' +
'							</div>' +
'						</div>' +
'						<div id="personConfig-img" class="clear" style="' + imageDisplay + '">' +
'							' +
'								<ul class="personConfig-watermark-list">' +
'									<li class="personConfig-watermark-pic">' + imageLogo + '</li>' +
'								</ul>' +
'							' +
'						</div>' +
'				';
            $("#sale-watermark-content").html(html);
        }
    });
}
$("input[name='watermark_position']").live("change", function () {
    $("#hideWaterMarkPosition").val($(this).val());
    if ($(this).val() == "0") {
        $("#personConfig-img").hide();
    }
    else {
        $("#personConfig-img").show();
    }
    saveWatermark();
});