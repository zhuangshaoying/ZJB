

var imgW100H80 = "?imageView2/1/w/100/h/80/q/85";
window.onscroll = function () {
    onScrollEvent(GetHouseInfo);
};
$(function () {

    GetHouseInfo();
    //getHouseCommunityList();

});
function GetHouseInfo(newPage)
{
    var pageSize = 10;
    page = (newPage == undefined) ? 1 : newPage;
    var price,minprice=0,maxprice=0;
    price = $("#price").val();
    var str= new Array();  
    str = price.split(",");
    for (i = 0; i < str.length ; i++) {
        if(i==0)
            minprice = str[i];
        if(i==1)
            maxprice = str[i];
        }
    console.log(minprice);
    var room = $("#room").val();
    $.ajax({
        url: "/M/H/GetHouseList",
        type: "get",
        cache: false,
        data: {
            "TradeType": $("#postType").val(),
            "BuildType": $("#buildingType").val(),
            "MinPrice": minprice,
            "MaxPrice": maxprice,
            "HouseOrder": $("#sort").val(),
            "Room": room,
            "PageIndex": page,
            "PageSize": pageSize
        },
        dataType: "json",
        beforeSend: function (XMLHttpRequest) {
            canGet = false;
            $("#drag").show();
           // parent.loadingShow();
        },
        success: function (result) {
            var listContent = bulidHouseTableHtml(result);
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
        complete: function (XMLHttpRequest, textStatus) {
            $("#drag").hide();
           // parent.loadingHide();
        }
    });
}

function bulidHouseTableHtml(dataList)
{
    var html = "";
    if (dataList.data.length > 0) {
        $.each(dataList.data, function (a, item) {
            var imgStr = "";
            var smallImagePath = item.HouseImgPath == "" ? "/images/null.png" : item.HouseImgPath + imgW100H80;
            html += ' <div class="newhouse_item" onclick="window.location=\'/m/v/'+item.HouseID+'\'">' +
        '        <img src="' + smallImagePath + '" class="avater100">' +
        '        <div class="newhouse_dts">' +
        '            <p class="house_title">' + item.Title + '</p>' +
        '            <p class="myitems lh20 mybt">' + item.DistrctName + ' / ' + item.CommunityName + '</p>' +
        '            <p class="myitems lh20 mt2">' + item.CurFloor + '/' + item.MaxFloor + '，' + item.Room + '室' + item.Hall + '厅，' + item.BuildArea + '平，<span class="zongjia">' + item.Price + '' + item.PriceUnit + '</span></p>' +
        '            <p class="myitems lh20 mt2">' + item.PostTime + '</p>' +
        '        </div>' +
        '	</div>';
        });
    }
    return html;
}

///根据条件获取房源小区名称
function getHouseCommunityList() {
    var postType = $("#postType").val();
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/House/UserHouseCommunityList',
        data: { postType: postType },
        success: function (data) {
            var html = "<dl>";
            html += '<dd><a class="noarr" value="" href="javascript:void(0)">不限</a></dd>';
            $.each(data, function (a, i) {
                if (i.Name != "") {
                    html += '<dd><a class="noarr" value=' + i.Name + ' href="javascript:void(0)">' + i.Name + '</a></dd>';
                }
            });
            html += "</dl>";
            $("#priceChioce").html(html);
        }
    });
}



