//var imgW100H80 = "?imageView2/1/w/100/h/320/q/340";
var imgW100H80 = "";
var firstHouseId0;
var firstHouseId1;
var rTimer = false;
window.onscroll = function () {
    onScrollEvent(GetHouseCollectInfo);
};
$(function () {

    GetHouseCollectInfo();
    getSiteSource();
    getDistrictList("厦门","");
    //getHouseCommunityList();
    
});
function GetHouseCollectInfo(newPage)
{
    if (checkPrice()) {
        page = (newPage == undefined) ? 1 : newPage;
        var price1 = $("#pricemin").val();
        var price2 = $("#pricemax").val();
        var cell;
        cell = $("#cell").val();
        cell = (cell == '小区') ? "" : cell;
        $.ajax({
            type: 'get',
            url: "/House/GetHouseCollectList",
            data: {
                "postType": $("#postType").val(),
                "buildingType": $("#houseType").val(),
                "minPrice": price1,
                "maxPrice": price2,
                "districtId": $("#district").val(),
                "regionId": $("#area").val(),
                "title": $("#txtlpSearch").val(),
                "roomType": $("#roomType").val(),
                "page": page,
                "webName": $("#personHouse-Website").val(),
                "sort": $("#personHouse-order").val(),
                "siteId": 0,
                "cityId": $("#city").val()
            },
            dataType: "json",
            beforeSend: function (XMLHttpRequest) {
                canGet = false;
                $("#drag").show();
            },
            success: function (result) {
                //var html = BuildHouseCollectListHtml(result);
                //$("#showTd").html(html);
                ///**房源提醒**/
                if (rTimer) {
                    firstHouseId1 = $("#firstPersonHouse").attr("houseId");
                    if (firstHouseId1 != firstHouseId0) {
                        //top.startModifyTip();
                        //top.$(".newHouse").show();
                        rTimer = false;
                    }
                }
                var listContent = BuildHouseCollectListHtml(result);
                if (page == 1) {
                    $("#houseTable").html(listContent);
                }
                else {
                    $("#houseTable").append(listContent);
                }
                if (listContent == "") {
                    canGet = false;
                    $("#dataEnd").show();
                }
                else {
                    canGet = true;
                }
            },
            error: function (jqXHR) {
            },
            complete: function (XMLHttpRequest, textStatus) {
                $("#drag").hide();
            }
        });
    }

}
function checkPrice() {
    var price1 = $("#pricemin").val();
    var price2 = $("#pricemax").val();
    if ((price1 != '' && price2 == '') || (price1 == '' && price2 != '')) {
        alert("请输入完整的价格区间");
        return false;
    }
    if ((price1 != '' && isNaN(price1)) || (price2 != '' && isNaN(price2))) {
        alert("请输入数字");
        return false;
    }

    if ((price1 != '' && price2 != '') && (parseInt(price2) < parseInt(price1))) {
        alert("请输入正确的价格区间");
        return false;
    }

    return true;
}
function getSiteSource() {
    $.ajax({
        type: 'get',
        url: "/House/GetHouseCollectSite",
        data: {
            "cityId": $("#city").val()
        },
        dataType: "json",
        success: function (result) {
            var html = "";
            html = '<div class="tit">网站</div>';
            html += '<dl>';
            html += '<dd class="" value="">不限</dd>';
            if (result.data.length > 0) {
                $.each(result.data, function (i, item) {
                    html += ' <dd value="' + item.Source + '">' + item.Source + '</dd>';
                });
            }
            html += '</dl>';
            $("#orderbyTw").html(html);
        }
    });
}
/*获取区域列表*/
function getDistrictList(cityName,districtName) {
    var cityId = $("#city").val();
    var districtId = $("#district").val();
    if (cityId > 0) {
        if (districtId > 0 && $("#districts_" + districtId).length>0) {
            $("#districts_" + districtId).show();
        }
        else {
            $.ajax({
                type: 'get',
                dataType: 'json',
                url: '/User/GetRegionList',
                data: { cityId: cityId, districtId: districtId },
                success: function (result) {
                    if (districtId > 0) {
                        var html = '';
                        html += '<section id="districts_' + districtId + '"  class="districts">';
                        html += '     <dl>';
                        html += '<dd class="active" name="areas"  str="areas_0" onclick="changeArea(\'\',\'' + districtName + '\')"><a>全' + districtName + '</a></dd>';
                        $.each(result.data, function (i, item) {
                            html += '       <dd name="areas" str="areas_' + item.RegionID + '" onclick="changeArea(' + item.RegionID + ',\'' + item.Name + '\')"><a>' + item.Name + '</a></dd>';
                        });
                        html += '     </dl>';
                        html += '   </section>';
                        $("#wapxfsy_D02_01").append(html);
                    }
                    else {
                        var html = '<dl>';
                        html += ' <dd class="active" str="districts_0" name="districts" onclick="changeDistrict(\'\',\'' + cityName + '\')" ><a>全' + cityName + '</a></dd>';
                        $.each(result.data, function (i, item) {
                            html += '<dd name="districts" onclick="changeDistrict(' + item.RegionID + ',\'' + item.Name + '\')" str="districts_' + item.RegionID + '"><a>' + item.Name + '</a></dd>';
                        });
                        html += '</dl>';
                        $("#districtChioce").html(html);
                    }
                }
            });
        }
    }
}
function changeDistrict(distirctId,name)
{
    $("#district").val(distirctId);
    $("#sift").find("span[for='whereChioce']").html(name);
    $(".districts").hide();
    $("dd[name='districts']").removeClass("active");
    if (distirctId == "") {
        $("#area").val("");
        $("dd[str='districts_0']").addClass("active");
        clearAllChoice();
        GetHouseCollectInfo();
    }
    else {
        $("dd[str='districts_" + distirctId + "']").addClass("active");
        getDistrictList("厦门", name);
    }

}
function changeArea(areaId, name)
{
    $("#area").val(areaId);
    $("#sift").find("span[for='whereChioce']").html(name);
    clearAllChoice();
    GetHouseCollectInfo();
    $("dd[name='areas']").removeClass("active");
    if (areaId == "") {
        $("dd[str='areas_0']").addClass("active");
    }
    else {
        $("dd[str='areas_" + areaId + "']").addClass("active");
    }
}
function clearAllChoice()
{
    hideAllOut();
    hideActive();
    hideChioce();
}
function BuildHouseCollectListHtml(result) {
   
    var data = result.data;
    var html = "";
    var buildTypeMap = ["", "住宅", "别墅", "商铺", "写字楼", "厂房"]
    if (data.length > 0) {
        $.each(data, function (i, item) {
            var firstTr = "";
            if (i == 0) {
                firstTr = '<div class="house_item" houseid="' + item.Id + '" id="firstPersonHouse">';
            } else {
                firstTr = '<div class="house_item">';
            }
            var houseType = '<span title="新房源" class="personHouse-new">新</span>';//新房源或者刷新房源
            if (item.houseType == 0) {
                houseType = '		 				  	<span title="新房源"  class="new">新</span>';
            }
            else if (item.houseType == 1) {
                houseType = '		 				  	<span title="刷新房源" class="new">刷新</span>';
            }
	            
            var isJjr = '';// 是否中介
            if (item.isJjr == 1) {
                isJjr = '<span title="中介冒充个人" class="new personHouse-new">中介</span>';
            }

            var isReadClass = "NoRead";
            var ReadSpan = '';
            //if (item.isRead > 0)
            //{
            //    ReadSpan = '<span class="personHouse-read">已阅</span>';
            //    isReadClass = "HaveRead";
            //}
            html += firstTr +
                    '<h1><a href="' + item.Url + '" target="_blank">' + item.Title + '</a></h1>' +
                    '<p>' + (item.DistrctName == null || item.DistrctName == "null" ? '' : item.DistrctName) +
                    (item.RegionName == null || item.RegionName == "null" ? '' : '  ' +item.RegionName) +
                    (item.CommunityName == null || item.CommunityName == "null" ? '' : '  ' +item.CommunityName) +
                    (item.Address == null || item.Address == "null" ? '' :  ' '+item.Address) +
                    houseType+ReadSpan+isJjr+
                    '<span class="pic_num">' + (item.PicNum > 0 ? item.PicNum : '无') + '图</span></p>' +
                    '<p>' + buildTypeMap[item.BuildingType] + 
                    '，' + item.Room + '房' + item.Hall + '厅' + item.Toilet + '卫，' + item.BuildArea + '平米，' + item.CurFloor + '/' + item.MaxFloor + '层，<span class="price">' + (item.Price > 0 ? item.Price : "") + '' + item.PriceUnit + '</span></p>' +
                    '<p>' + (item.Source == null ? "来源网站" : item.Source) +
                    ' (' + item.AddDate + ')' +
                    '<span class="yezhu">' + (item.Publisher == null ? "业主姓名" : item.Publisher) +
                    '</span><a href="tel:'+ (item.Tel == null ? "业主号码" : item.Tel)+'" class="yztel">'+ (item.Tel == null ? "业主号码" : item.Tel)+'</a></p>' +
                    '</div>';
        });

    }
   
    return html;
}

function startTimer() {
    $('body').stopTime();//停止定时器

    var time = $("#rTimer").val();
    if (time != '') {
        $('body').everyTime(time + 's', refreshCollect);//启动定时器
    }
}
function refreshCollect() {
    firstHouseId0 = $("#firstPersonHouse").attr("houseId");
    rTimer = true;
    GetHouseCollectInfo(1);
}