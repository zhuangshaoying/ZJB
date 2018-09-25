/*iframe高度自适应*/
$(function(){
	var height = document.documentElement.clientHeight;
	$(".saleHouse").css("min-height",height-2);

	$("#personHouse" + $("#postType").val()).siblings().removeClass('selected');
	$("#personHouse" +  $("#postType").val()).addClass('selected');
	
	if ($("#postType").val() == 1 || $("#postType").val() == 2) {
		$("#priceUnit").html("万元");
	} else {
		$("#priceUnit").html("元");
	}
	getSiteSource();
	getDistrictList();
	ajaxSub(1);
	
	
});
/*tab切换*/
function switchTab(postType) {
	$("#postType").val(postType);
	$("#personHouse" +postType).siblings().removeClass('selected');
	$("#personHouse" +postType).addClass('selected');
	if (postType == 1 || postType == 2) {
		$("#priceUnit").html("万元");
	} else {
		$("#priceUnit").html("元");
	}
	ajaxSub();
}

function startTimer() {
	$('body').stopTime();//停止定时器
	
	var time = $("#rTimer").val();
	if (time != '') {
		$('body').everyTime(time + 's', refresh);//启动定时器
	}
}

var firstHouseId0;
var firstHouseId1;
var rTimer = false;
function refresh() {
	firstHouseId0 = $("#firstPersonHouse").attr("houseId");
	rTimer = true;
	ajaxSub(1);
}

function enterSumbit() {
	var event = arguments.callee.caller.arguments[0] || window.event;//消除浏览器差异
	if (event.keyCode == 13){
		ajaxSub();
	}
}

/*获取区域列表*/
function getDistrictList()
{
    var cityId = $("#city").val();
    var districtId = $("#district").val();
    if (cityId > 0)
    {
        $.ajax({
            type: 'get',
            dataType: 'json',
            url: '/User/GetRegionList',
            data: { cityId: cityId, districtId: districtId },
            success: function (result) {
                if (districtId > 0) { 
                    var html = '<a class="col-f60 font-bold" href="javascript:" onclick="changeArea(this, \'\')">全部</a>';
                    $.each(result.data, function (i, item) {
                        html += '<a href="javascript:" onclick="changeArea(this, ' + item.RegionID + ')">' + item.Name + '</a>';
                    });
                    $("#areaLi").html(html);
                    $("#areaLi").show();
                }
                else {
                    var html = '<a class="col-f60 font-bold" href="javascript:" onclick="changeDistrict(this, \'\')">全部</a>';
                    $.each(result.data, function (i, item) {
                        html += '<a href="javascript:" onclick="changeDistrict(this, ' + item.RegionID + ')">' + item.Name + '</a>';
                    });
                    $("#AllDistrict").html(html);
                }
            }
        });
    }
}
	var isCanSubmit = true;
	function ajaxSub(i) {
   		if(checkPrice()) {
   			i == undefined ? $("#pageNow").val(1) : $("#pageNow").val(i);
   			var price1 = $("#price1").val();
   			var price2 = $("#price2").val();
   			$.ajax({
   				type : 'get',
   				url: "/House/GetHouseCollectionList",
	            data: {
	            	"postType" : $("#postType").val(),
	            	"buildingType": $("#houseType").val(),
	            	"minPrice": price1,
	            	"maxPrice": price2,
	            	"districtId": $("#district").val(),
	            	"regionId": $("#area").val(),
	            	"title" : $("#place").val(),
	            	"roomType": $("#roomType").val(),
	            	"page": $("#pageNow").val(),
	            	"webName": $("#personHouse-Website").val(),
	            	"sort": $("#personHouse-order").val(),
	                "siteId":0,
	                "cityId": $("#city").val()
	            },
	            dataType: "json",
                beforeSend: function(XMLHttpRequest) {
					isCanSubmit = false;
					parent.loadingShow();
   				},
                success: function (result) {
                    var html = BuildHouseCollectListHtml(result);
                    $("#showTd").html(html);
   					/**房源提醒**/
   					if(rTimer){
   						firstHouseId1 = $("#firstPersonHouse").attr("houseId");
   						if(firstHouseId1 != firstHouseId0){
   							top.startModifyTip();
   							top.$(".newHouse").show();
   							rTimer = false;
   						}
   					}
                },
                error: function(jqXHR) {
                },
                complete:function(XMLHttpRequest, textStatus) {
					isCanSubmit = true;
					parent.loadingHide();
   				}
			});
		}
	}	
	function BuildHouseCollectListHtml(result)
	{
	    var totalSize = result.totalSize;
	    var data = result.data;
	    var html = "";
	    var buildTypeMap = ["", "住宅", "别墅", "商铺", "写字楼", "厂房"]
	    if (data.length > 0) {
	        $.each(data, function (i, item) {
	            var firstTr = "";
	            if (i == 0) {
	                firstTr = '<tr houseid="' + item.HouseID + '" id="firstPersonHouse">';
	            } else {
	                firstTr = '<tr>';
	            }
	         
	            html += firstTr +
                        '		<td width="60"><input type="checkbox" name="buildCheck" class="saleManager-state-checkbox" value="' + item.Id + '">【' + buildTypeMap[item.BuildingType] + '】</td>' +
                        '		<td width="470">' +
                        '			<dl class="h_info">' +
                        '				<dt style="text-align:left" class="personHouse-title">' +
                        '  						 			<a  style="width:360px"  target="_blank"  title="' + item.Title + '" href="'+item.Url+'">' +
                        '					' + item.Title +
                        '					</a>' +
                        '				</dt>' +
                        '				<dd class="h_houseCollect clear">' +
                        '					<div class="float-l">' +
                       '                      ' + (item.DistrctName == null || item.DistrctName == "null" ? '' : ' <span class="personHouse-districtNa">' + item.DistrctName + '</span>') +
                        '                      ' + (item.RegionName == null || item.RegionName == "null" ? '' : '<span class="personHouse-areaNa">' + item.RegionName + '</span>') +
                        '                      ' + (item.CommunityName == null || item.CommunityName == "null" ? '' : '<span class="personHouse-cell">' + item.CommunityName + '</span>') +
                        '                      ' + (item.Address == null || item.Address == "null" ? '' : '<span title="" class="personHouse-addr">' + item.Address + '</span>') +
                        '                   </div>' +
                        '				</dd>' +
                        '				<dd class="h_property clear">' +
                        '					<p class="float-l">' +
                        '						' +
                        '							<span>' + item.Room + '房' + item.Hall + '厅' + item.Toilet + '卫</span>' +
                        '						' +
                        '						<span>' + item.BuildArea + '㎡</span>' +
                        '						' +
                        '							<span>' + item.CurFloor + '/' + item.MaxFloor + '层</span><span class="font-bold col-f60" style="margin-left:12px">' + (item.Price > 0 ? item.Price : "") + '' + item.PriceUnit + '</span>' +
                        '						' +
                        '					</p>' +
                        '					<div class="float-r">' +
                        '						' +
                        '							<span class="personHouse-pic">' + (item.PicNum > 0 ? item.PicNum : '无') + '图</span>' +
                        '							' +
                        '						' +
                        '					</div>' +
                        '				</dd>' +
                        '			</dl>' +
                        '		</td>' +
                        '		<td width="80">' +
                        '			<dl>' +
                        '				<dd>' + (item.Publisher == null ? "业主姓名" : item.Publisher) + '</dd>' +
                        '				<dd>' +
                        '				<span class="font-bold col-f60">' + (item.Tel == null ? "业主号码" : item.Tel) + '</span>' +
                        '				</dd>' +
                        '			</dl>' +
                        '		</td>' +
                        '		<td width="80">' +
                        '			<dl>' +
                        '				<dd>' + (item.Source == null ? "来源网站" : item.Source) + '</dd>' +
                        '				<dd><span class="col-999"> ' +
                        '							' + item.AddDate +
                        '					 </span></dd>' +
                        '			</dl>' +
                        '		</td>' +
                        '	</tr>';
	        });

	        var pageSize = parseInt($("#pageSize").val());
	        var pageCount = parseInt(totalSize / pageSize) + ((totalSize % pageSize) == 0 ? 0 : 1);
	        $("#saleManager-fanye").paginate({
	            count: pageCount,
	            start: $("#pageNow").val(),
	            display: 10,
	            border: false,
	            text_color: '#50b63f',
	            text_hover_color: '#fff',
	            background_color: '#fff',
	            background_hover_color: '#50b63f',
	            images: false,
	            mouse: 'click',
	            onChange: function () {
	                $("#pageNow").val($(".jPag-current").html());
	                ajaxSub($(".jPag-current").html());
	            }
	        });
	    }
	    else {
	        $("#saleManager-fanye").html("");
	    }
	    return html;
	}

	function getAreaList(district) {
		$.ajax({
   				type : 'get',
	            url:  "getAreaList.html",
	            data: {
	            	"district" : district
	            },
	            dataType: "json",
                beforeSend: function(XMLHttpRequest) {
                	parent.loadingShow();
   				},
                success: function(result) {
   					var html = '<a class="col-f60 font-bold" href="javascript:" onclick="changeArea(this, \'\')">全部</a>';
   					
   					for (var i = 0; i < result.length; i++) {
   						html += '<a href="javascript:" onclick="changeArea(this, ' + result[i].postSiteId + ')">' + result[i].siteName + '</a>';
   					}
   					
   					$("#areaLi").html(html);
   					$("#areaLi").show();
                },
                error: function(jqXHR) {
                },
                complete:function(XMLHttpRequest, textStatus) {
                	parent.loadingHide();
   				}
			});
	}
	function getSiteSource()
	{
	    $.ajax({
	        type: 'get',
	        url: "/House/GetHouseCollectSite",
	        data: {
	            "cityId": $("#city").val()
	        },
	        dataType: "json",
	        success: function (result) {
	            var html = "";
	            html = '<option value="">全部网站</option>';
	            if (result.data.length > 0)
	            {
	                $.each(result.data, function (i, item) {
	                    html += '<option value="' + item.Source + '">' + item.Source + '</option>';
	                });

	            }
	            $("#personHouse-Website").html(html);
	        }
	    });
	}
function checkPrice() {
	var price1 = $("#price1").val();
	var price2 = $("#price2").val();
	if ((price1 != '' && price2 == '') || (price1 == '' && price2 != '')) {
		alert("请输入完整的价格区间");
		return false;
	}
	if((price1 != '' && isNaN(price1)) || (price2 != '' && isNaN(price2))) {
		alert("请输入数字");
		return false;
	}
	
	if ((price1 != '' && price2 != '') && (parseInt(price2) < parseInt(price1))) {
		alert("请输入正确的价格区间");
		return false;
	}
	
	return true;
}
	
function changeDistrict(objThis, district) {
	$("#district").val(district);
	$("#area").val('');
	$(objThis).parent().find('.col-f60').removeClass();
	$(objThis).addClass('col-f60 font-bold');
	
	if (district == '') {
		$("#areaLi").hide();
		ajaxSub();
	} else {
	    getDistrictList();
		changeArea();
	}
}

function changeArea(objThis, area) {
	$("#area").val(area);
	$(objThis).parent().find('.col-f60').removeClass();
	$(objThis).addClass('col-f60 font-bold');
	ajaxSub();
}

function changeHouseType(objThis, houseType) {
	$("#houseType").val(houseType);
	$(objThis).parent().find('.col-f60').removeClass();
	$(objThis).addClass('col-f60 font-bold');
	ajaxSub();
}

function changeRoomType(objThis, roomType) {
	$("#roomType").val(roomType);
	$(objThis).parent().find('.col-f60').removeClass();
	$(objThis).addClass('col-f60 font-bold');
	ajaxSub();
}


/*选择房源表格全选*/
$("#saleManager-all").click(function () {

    if ($("#saleManager-all").attr("checked")) {
        $(".saleManager-state-checkbox").each(function () {
            $(this).attr("checked", true);
        });
    } else {
        $(".saleManager-state-checkbox").each(function () {
            $(this).attr("checked", false);
        });
    }
});

function CancleHouseCollect()
{
    var chk_value = [];
    /*计算选择条数*/
    var sum = 0;
    var appBuildingIds = "";
    $(".saleManager-state-checkbox").each(function () {
        if ($(this).attr("checked")) {
            sum++;
        }
    });

    if (sum == 0) {
        alert("请先选择房源");
        return;
    }
    if (!confirm("确定删除?")) return;
    
        //var houseList = [];
        $('input[name="buildCheck"]:checked').each(function () {
            chk_value.push({
                name: 'cId',
                value: $(this).val()
            });
        });

        $.ajax({
            url: "/House/DeleteHouseCollect",
            data: chk_value,
            type: "post",
            cache: false,
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                parent.loadingShow();

            },
            success: function (result) {
                location.reload();
            },
            error: function (jqXHR) {
                alert($.parseJSON(jqXHR.responseText).msg);
            },
            complete: function (XMLHttpRequest, textStatus) {
                parent.loadingHide();
            }
        });
 
}