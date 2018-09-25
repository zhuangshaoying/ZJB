
var vaildForm;
var autoArea = true;
$(function(){
	vaildForm = $("#bForm").Validform({
		tiptype:4,
		showAllError:true,
		ignoreHidden:true,
		postonce:true,
		datatype:{
			"zh1-6":/^[\u4E00-\u9FA5\uf900-\ufa2d]{1,6}$/,
			"cell":function(gets,obj,curform,regxp){
				if ($("#cellCode").val() == '' || $("#cellCode").val() == 0) {
					return "请选择我们提供的一个小区";
				}
	        	checkAvgPrive();
			},
			"addr":function(gets,obj,curform,regxp){
				if (gets.length < 2 || gets.length > 30) {
					return "地址必须为2~30个字";
				}
				var addrTip = "";
				 $.each(webNameArray, function(i, value){
					 if (gets.length > addrLengthArray[i]) {
						 addrTip += "<li>【" + value + "】:地址字数最多" + addrLengthArray[i] + "个字，超出部分将被删除<span>";
					 }
				 });
				 if (addrTip != "") {
					 $("#addrTipDiv").find("ul").html(addrTip);
					 $("#addrTipDiv").show();
				 }
			},
			"houseArea":function(gets,obj,curform,regxp){
			    if (gets == '') {
			        return "请填写信息！";
			    }
			    var id = "houseArea";
			    if ($("#buildingType").val() == 4) {
			        if (gets <= 2 || gets >= 50000) {
			            return "建筑面积必须大于2小于50000";
			        }
			    } else if ($("#buildingType").val() == 5) {
			        if (gets <= 2 || gets >= 1000000) {
			            return "建筑面积必须大于2小于1000000";
			        }
			    } else {
			        if (gets <= 2 || gets >= 10000) {
			            return "建筑面积必须大于2小于10000";
			        }
			    }

			    var flag = checkDecimals(id);
			    if (flag != true) {
			        return flag;
			    }

			    if (autoArea && ($("#buildingId").val() == null || $("#buildingId").val() == '')) {
			        $("#areaUsed").val(parseInt(gets) - 1);
			    }

			    if ($("#buildingType").val() != 2) {
			        if (parseFloat(gets) <= parseFloat($("#areaUsed").val())) {
			            changeRightTip("areaUsed", "建筑面积须大于使用面积");
			            return "建筑面积须大于使用面积";
			        }
			        checkAreaUsed();
			    } else {
			        changeTip("houseArea");
			    }
			    checkAvgPrive();
			},
			"areaUsed":function(gets,obj,curform,regxp){
			    var id = "areaUsed";
			    if (gets == '') {
			        return "请填写信息！";
			    }
			    if ($("#buildingType").val() == 4) {
			        if (gets <= 0 || gets >= 50000) {
			            return "使用面积必须大于0小于50000";
			        }
			    } else if ($("#buildingType").val() == 5) {
			        if (gets <= 0 || gets >= 1000000) {
			            return "使用面积必须大于0小于1000000";
			        }
			    } else {
			        if (gets <= 0 || gets >= 10000) {
			            return "使用面积必须大于0小于10000";
			        }
			    }

			    if (parseFloat(gets) <= 0) {
			        return "使用面积必须大于0";
			    }
			    var flag = checkDecimals(id);
			    if (flag != true) {
			        return flag;
			    }

			    if ($("#buildingType").val() != 2) {
			        if (parseFloat($("#houseArea").val()) <= parseFloat(gets)) {
			            changeRightTip("areaUsed", "建筑面积须大于使用面积");
			            return "建筑面积须大于使用面积";
			        }
			        checkHouseArea();
			    } else {
			        changeTip("areaUsed");
			    }
			    autoArea = false;
			},
			"price":function(gets,obj,curform,regxp){
				 if (gets == '') {
					return "请填写信息！";
				 }
				 var price = parseInt(gets);
				 
				 var flag;
				 if ($("#postType").val() == 1) {
					 flag = checkDecimals('price');
					 if (flag != true) {
						 return flag;
					 }
					 
					 if (price < 3 || price > 999999) {
						 return "出售价格必须在3~999999之间";
					 }
				 } else {
					 if ($("#buildingType").val() == 1 || $("#buildingType").val() == 2) {
						 flag = isInteger("price");
						 if (flag != true) {
							 return flag;
						 }
						 if (price < 200 || price > 300000) {
							 return "出租价格必须在200~300000之间";
						 }
					 } else {
						 flag = checkDecimals('price');
						 if (flag != true) {
							 return flag;
						 }
						 if ($("input[name='priceType']:checked").val() == '元/平米·天') {
							 if (price <= 0 || price >= 30) {
								 return "出租价格必须大于0小于30";
							 }
						 } else {
							 if (price < 1 || price > 300000) {
								 return "出租价格必须在1~300000之间";
							 }
						 }
					 }
				 }
				 checkAvgPrive();
//				 countLowPay();
			},
			"saleHouse-basic-year":function(gets,obj,curform,regxp){
				if (gets == '') {
					return "请填写信息！";
				}
				
				var flag = isInteger("saleHouse-basic-year");
				if (flag != true) {
					 return flag;
				}
				
				var date = new Date();
				if (parseInt(gets) > date.getFullYear()) {
					return "建筑年限不可大于" + date.getFullYear();
				}
				if (parseInt(gets) < 1981) {
					return "建筑年限不可小于1981";
				}
			},
			"floor":function(gets,obj,curform,regxp){
				if (gets == "") {
					return "请输入楼层";
				} else {
					var flag = isInteger("saleHouse-basic-flooron");
					var flag2 = isInteger("saleHouse-basic-floorall");
					if (flag != true) {
						 return flag;
					}
					if (flag2 != true) {
						 return flag2;
					}
					if (gets < -3 || gets > 99) {
						return "楼层的范围在-3~99之间";
					}
					
					if (parseInt($("#saleHouse-basic-floorall").val()) < parseInt($("#saleHouse-basic-flooron").val())) {
						return "总楼层须大于等于当前楼层";
					}
					$("#saleHouse-basic-floorall").removeClass("Validform_error");
					$("#saleHouse-basic-flooron").removeClass("Validform_error");
				}
			},
			"internalNum":function(gets,obj,curform,regxp){
				 if (gets.replace(/([^\x00-\xff])/g,'**').length > 16) {
					 return "内部编号不可超过16个字符(中文占2个字符)";
				 } 
			},
			"saleHouse-eare-tip":function(gets,obj,curform,regxp){
				 var cellLabelStr = gets.replace(regexp, "");
				 
				 if (cellLabelStr != '') {
					 var testStr = trimAll(cellLabelStr,"g");
					 testStr = removerNum(testStr);
					 if (testStr != '') {
						 var re = /^[\u4e00-\u9fa5a-z]+$/gi;
						 if (!re.test(testStr)) {
							 return "只能输入字母、数字和汉字";
						 }
					 }
					 
					 var labels = cellLabelStr.split(" ");
					 if (labels.length > 3) {
						 return "小区标签不能超过3个！";
					 }
					 
					 for (var i = 0; i < labels.length; i++) {
						 if (labels[i].length > 6 || labels[i].length < 2) {
							 return "每个标签限2~6字！";
						 } 
					 }
				 } else {
					 return "请填写信息！";
				 }
			},
			"saleHouse-house-tip":function(gets,obj,curform,regxp){
				 var houseLabelStr = gets.replace(regexp, "");
				 
				 if (houseLabelStr == '') {
					 return "请填写信息！";
				 }
				 
				 var testStr = trimAll(houseLabelStr,"g");
				 testStr = removerNum(testStr);
				 if (testStr != '') {
					 var re = /^[\u4e00-\u9fa5a-z]+$/gi;
					 if (!re.test(testStr)) {
						 return "只能输入字母、数字和汉字";
					 }
				 }
				 
				 var labels = houseLabelStr.split(" ");
				 if (labels.length > 3) {
					 return "房源标签不能超过3个！";
				 }
				 
				 for (var i = 0; i < labels.length; i++) {
					 if (labels[i].length > 6 || labels[i].length < 2) {
						 return "每个标签限2~6字！";
					 } 
				 }
			},
			"saleHouse-tongcheng-tip":function(gets,obj,curform,regxp){
				 var length = gets.length;
				 if (length < 6 || length > 35 ) {
					 return "一句话广告必须为6~35个字";
				 }
				 var testStr = getNum(gets);
				 var mobile = /^1[3|5|8]\d{9}$/;
				 var phone = /^0\d{2,3}-?\d{7,8}$/;
				 
				 if(mobile.test(testStr) || phone.test(testStr)) {
					 return "一句话广告不允许填写联系方式";
				 }
			},
			"saleHouse-title":function(gets,obj,curform,regxp) {
				$("#titleTipDiv").hide();
				 if (!autoFlag) {
					 return true;
				 }
				 var titleLength = 0;
				 var regexp = /(^\s*)|(\s*$)/g;
				 gets = gets.replace(regexp, "");
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
			    	
				 if (titleLength < 6 ||  titleLength > 30) {
					 return "房源标题必须为6~30个字";
				 }
				 var testStr = getNum(gets);
				 var mobile = /^1[3|5|8]\d{9}$/;
				 var phone = /^0\d{2,3}-?\d{7,8}$/;
				 
				 if(mobile.test(testStr) || phone.test(testStr)) {
					 return "标题不允许填写联系方式";
				 }
				 var titleTip = "";
				 $.each(webNameArray, function(i, value){
					 if (titleLength > titleLengthArray[i]) {
						 titleTip += "<li>【" + value + "】:标题字数最多" + titleLengthArray[i] + "个字，超出部分将被删除<span>";
					 }
				 });
				 if (titleTip != "") {
					 $("#titleTipDiv").find("ul").html(titleTip);
					 $("#titleTipDiv").show();
				 }
			},
			"fee":function(gets,obj,curform,regxp){
				 id = $(obj).attr("id");
				 buildingType = $("#buildingType").val();
				 if ((buildingType == 3 && id == "fee2") || 
						 (buildingType == 4 && id == "fee3")) {
					 flag = checkDecimals($(obj).attr("id"));
					 if (flag != true) {
						 return flag;
					 }
					 
					 price = parseInt(gets);
					 if (price < 0 || price > 999999) {
						 return "价格必须在0~999999之间";
					 }
				 }
			}
		},
		beforeSubmit:function(curform){
			if (!autoFlag) {
				return true;
			}
			
			if ($("#cellCode").val() == 0 || $("#cellCode").val() == '0') {
				alert("小区编号错误，请重新选择");
			}
			
			var desc = UE.getEditor('houseDescribe').getContent();
			$("#houseDescribe").val(desc);
			desc = trim(desc.replace(/<[^>]+>/g,""));
			desc = desc.replace(/&nbsp;/ig,"");
			desc = trimAll(desc,"g");
			descLength = desc.length;
			if (desc == "" || descLength < 30 || descLength > 3000) {
				alert("房源描述的字数为30-3000个字！");
				$(document).scrollTop($("#cke_houseDescribe").offset().top);
				return false;
			}
		
			if(!checkImgs()) {
				return false;
			}
			
			$("#currentTimeMillis").val(new Date().getTime());
			$("#bForm").attr("action", $("#basePath").val() + url_1);
			
			top.loadingShow();
			return true;
		},
		ajaxPost:false
	});
	
	vaildForm.addRule([
		{
			ele:"#cell",
            datatype:"cell"
        },
        {
			ele:"#addr",
            datatype:"addr"
        },
        {
			ele:"#houseArea",
            datatype:"houseArea"
        },
        {
			ele:"#areaUsed",
            datatype:"areaUsed"
        },
        {
			ele:"#price",
            datatype:"price"
        },
        {
			ele:"#saleHouse-basic-year",
            datatype:"saleHouse-basic-year"
        },
        {
			ele:"#saleHouse-basic-flooron",
            datatype:"floor"
        },
        {
			ele:"#saleHouse-basic-floorall",
            datatype:"floor"
        },
        {
			ele:"#saleHouse-eare-tip",
            datatype:"saleHouse-eare-tip"
        },
        {
			ele:"#saleHouse-house-tip",
            datatype:"saleHouse-house-tip"
        },
        {
			ele:"#saleHouse-tongcheng-tip",
            datatype:"saleHouse-tongcheng-tip"
        },
        {
			ele:"#saleHouse-title",
            datatype:"saleHouse-title"
        },
        {
			ele:"#internalNum",
            datatype:"internalNum"
        }
	]);
});

function checkImgs() {
	var imgFlag = true;
	var urlList = ["null"];
	var tip = '';
	var imgIndex = 0;
	$('input[name="imgUrl"]').each(function() {
		imgIndex++;
		var imgUrl = $(this).val();
		if (imgUrl == 'ing' || imgUrl == 'fail' || imgUrl.indexOf("uploadIng") > -1) {
			imgFlag = false;
			tip = "还存在上传中或上传失败的图片,请确定";
		}
		$.each(urlList, function(i, url) {
			if (imgIndex > 1) {
				if (url == imgUrl && url != 'ing' && url != 'fail' && imgUrl.indexOf("uploadIng") == -1) {
					imgFlag = false;
					tip = "存在相同的图片，无法保存";
				} else {
					urlList.push(imgUrl);
				}
			}
		});
	});
	if (!imgFlag) {
		alert(tip);
		return false;
	}
	
	if ($("#coverUrl").val() == '') {
		var div = null;
		if ($("#div_IMG_I ").children().eq(0).length > 0) {
			if(!confirm("您没有设定封面图，默认以第一张室内图为封面，确定提交？")) {
				return false;
			}
			div = $("#div_IMG_I ").children().eq(0);
		} else if ($("#div_IMG_O ").children().eq(0).length > 0) {
			if(!confirm("您没有设定封面图，默认以第一张小区图为封面，确定提交？")) {
				return false;
			}
			div = $("#div_IMG_O ").children().eq(0);
		} else if ($("#div_IMG_M ").children().eq(0).length > 0) {
			if(!confirm("您没有设定封面图，默认以第一张房型图为封面，确定提交？")) {
				return false;
			}
			div = $("#div_IMG_M ").children().eq(0);
		}
		if (div != undefined && div.length > 0) {
			$("#coverDiv").html(div.html());
			$("#coverDiv h5").remove();
			$("#coverDiv .import-img-font").remove();
			$("#coverDiv").hide();
		} else {
			if(!confirm("您没有上传图片，确定提交？")) {
				return false;
			}
		}
	}
	return true;
}

function checkDecimals(id) {
	 var regu = /^[0-9]+\.{0,1}[0-9]{0,2}$/;
	 var desc = "最多只能输入2位小数";
	 str = $("#" + id).val();
	 if (str == '' || regu.test(str)) {
		 return true;
	 }
	 return desc;
}

function isInteger(id){
	 var regu = /^[-]{0,1}[0-9]{1,}$/;
	 str = $("#" + id).val();
	 if (str == '' || regu.test(str)) {
		 return true;
	 }
	 return "必须为整数";
}

function checkAreaUsed() {
	gets = $("#areaUsed").val();
	if (gets == '' || gets <= 0 || gets >= 10000) {
		return;
	}
	 
	var flag = checkDecimals("areaUsed");
	if (flag == true) {
		if (parseFloat($("#houseArea").val()) > parseFloat($("#areaUsed").val())) {
			changeTip("areaUsed");
		}
	}
}

function checkHouseArea() {
	 gets = $("#houseArea").val();
	 if (gets == '' || gets <= 2 || gets >= 10000) {
		return;
	 }
	 
	 var flag = checkDecimals("houseArea");
	 if (flag == true) {
		if (parseFloat($("#houseArea").val()) > parseFloat($("#areaUsed").val())) {
			changeTip("houseArea");
		}
	 }
}

function trimAll(str,is_global) {
    var result;
    result = str.replace(/(^\s*)|(\s*$)/g,"");
    if(is_global.toLowerCase()=="g") {
        result = result.replace(/\s/g,"");
    }
    return result;
}

function removerNum(num) {
	var str = '';
	for(var i = 0; i < num.length; i++) {
		var j = num.substring(i, i + 1);
		if (!/^[0-9]*$/.test(j)) {
			str += j;
		}
	}
	return str;
}

function getNum(str) {
	var num = '';
	for(var i = 0; i < str.length; i++) {
		var j = str.substring(i, i + 1);
		if (/^[0-9]*$/.test(j)) {
			num += j;
		}
	}
	return num;
}

function checkAvgPrive() {
	var html = "";
	if (avgPrice1 > 0) {
		var totalPrice = $("#price").val();
		var houseArea = $("#houseArea").val();
		
		var avgPrice2 = totalPrice * 10000 / houseArea;
		var p = parseFloat(avgPrice2) - parseFloat(avgPrice1);
		
		p = p.toFixed(2);
		
		if (p > 0) {
			html = "多于搜房小区均价" + p + "元/平米";
		} else if (p < 0) {
			p = p + "";
			html = "少于搜房小区均价" + p.substring(1) + "元/平米";
		}
	}
	$("#avgPriceTip").html(html);
}