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
	parent.$("#houseCollect"+postType).click();
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
   				url: "/House/GetHouseCollectList",
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
	    var buildTypeMap = ["", "住宅", "别墅", "商铺", "写字楼", "厂房"];
	    if (data.length > 0) {
	        $.each(data, function (i, item) {
	            var firstTr = "";
	            if (i == 0) {
	                firstTr = '<tr houseid="' + item.HouseID + '" id="firstPersonHouse">';
	            } else {
	                firstTr = '<tr>';
	            }
	            var houseType = '<span title="新房源" class="personHouse-new">新</span>';//新房源或者刷新房源
	            if (item.houseType == 0) {
	                houseType = '		 				  	<span title="新房源" class="personHouse-refresh">新</span>';
	            }
	            else if (item.houseType == 1) {
	                houseType = '		 				  	<span title="刷新房源" class=" personHouse-new">刷新</span>';
	            }
	            
	            var isJjr = '';// 是否中介
	            if (item.isJjr == 1) {
	                isJjr = '<span title="中介冒充个人" class="personHouse-new">中介</span>';
	            }

	            var isReadClass = "NoRead";
	            var ReadSpan = '';
	            if (item.isRead > 0)
	            {
	                ReadSpan = '<span class="personHouse-read">已阅</span>';
	                isReadClass = "HaveRead";
	            }
	            html += firstTr +
                        '		<td width="60">【' + buildTypeMap[item.BuildingType] + '】</td>' +
                        '		<td width="470">' +
                        '			<dl class="h_info">' +
                        '				<dt style="text-align:left" class="personHouse-title">' +
                        '		 				  	' + houseType + ReadSpan + isJjr +
                        '  						 			<a  style="width:360px" class="' + isReadClass + '"  onclick="AddViewLog(this,\'' + escape(JSON.stringify(item)) + '\')" title="' + item.Title + '" href="javascript:void(0);">' +
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
                        '							<span>' + item.CurFloor + '/' + item.MaxFloor + '层</span><span class="font-bold col-f60" style="margin-left:12px">' + (item.Price>0?item.Price:"") + '' + item.PriceUnit + '</span>' +
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
                        '		<td id="viewCount_' + item.Id + '" width="60">' +
                        +  item.viewCount + 
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
	function AddViewLog(obj,data)
	{
	    data = unescape(data);
	    data = JSON.parse(data);
	    if (!$(obj).hasClass("HaveRead"))
	    { 
	        var viewCount = parseInt($("#viewCount_" + data.Id).html());

	    $.ajax({
	        type: 'post',
	        data: { id: data.Id },
	        url: '/House/AddHouseCollectRead',
	        success: function (result)
	        {
	            viewCount++;
	            var ReadSpan = '<span class="personHouse-read">已阅</span>';
	            $(obj).siblings(".personHouse-read").remove();
	            $(obj).before(ReadSpan);
	            $("#viewCount_" + data.Id).html(viewCount);
	            $(obj).removeClass("NoRead").addClass("HaveRead");
	        }
	    });
	    }
	    openBlank('/house/Browser', data);
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

function openBlank(action, data) {
    data.TradeType = $("#postType").val();
    data.CityID = $("#city").val();
    data.ReleaseTime = new Date(parseInt(data.ReleaseTime.substr(6))).formatDateTime();
    data.UpdateTime = new Date(parseInt(data.UpdateTime.substr(6))).formatDateTime();
         var form = $("<form/>").attr('action',action).attr('method','post');
                 form.attr('target','_blank');
        var input = '';
         $.each(data, function(i,n){
             input += '<input type="hidden" name="' + i + '" value="' + n + '" />';
            });
         form.append(input).appendTo("body").css('display','none').submit();
}
Date.prototype.formatDate = function () {
    var s = this.getFullYear() + '-',
    d = this.getMonth() + 1;
    s += (d < 10 ? '0' + d : d) + '-';
    d = this.getDate();
    s += (d < 10 ? '0' + d : d);
    return s;
};
Date.prototype.formatDateTime = function () {
    var s = this.formatDate(), d = this.getHours();
    s += ' ' + (d < 10 ? '0' + d : d) + ':';
    d = this.getMinutes();
    s += (d < 10 ? '0' + d : d) + ':';
    d = this.getSeconds();
    s += (d < 10 ? '0' + d : d);
    return s;
};
