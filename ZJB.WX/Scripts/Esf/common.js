// JavaScript Document
var flagData  = true;
var cityId  = getQueryString("city_id") ; //获取URL 上面的参数
var houseId = getQueryString("house_id") ; //获取URL 上面的参数
var UDC = (function(){

        var mapRoomType = (function(){
            var arr = JSON.parse(localStorage.getItem('roomtype')) || [];
            var map = {};
            for(var i=0; i<arr.length; i++)
            {
                map[arr[i]['code']] = arr[i]['name'];
            }
            return map;
        })();

        function get_room_type($code)
        {
            if($code in mapRoomType)
                return mapRoomType[$code];
            else if($code == 0)
                return '';
            else
                return $code;// '[?:'+$code+']';
        }

        return {
            getRoomType : get_room_type
        };
})();
Date.prototype.format = function (fmt)
    {
        var o = {
            'M+': this.getMonth() + 1, //月份
            'd+': this.getDate(),      //日
            'h+': this.getHours(),     //小时
            'm+': this.getMinutes(),   //分
            's+': this.getSeconds(),   //秒
            'q+': Math.floor((this.getMonth() + 3) / 3), //季度
            'S': this.getMilliseconds() //毫秒
        };

        if (/(y+)/.test(fmt)) fmt = fmt.replace(RegExp.$1, (this.getFullYear() + '').substr(4 - RegExp.$1.length));
        for (var k in o)
            if (new RegExp('(' + k + ')').test(fmt)) fmt = fmt.replace(RegExp.$1, (RegExp.$1.length == 1) ? (o[k]) : (('00' + o[k]).substr(('' + o[k]).length)));
        return fmt;
    };

    Date.parseApi = function($strData){
        return new Date($strData.replace(/\-/g, '/'));
    };

    Date.check = function(str)
    {
        if(typeof(str) != 'string')
            return false;

        var arr = str.match(/^(\d+)\-(\d{1,2})\-(\d{1,2})/);
        if(arr == null)
            return false;

        arr[2]=arr[2]-1;
        var date = new Date(arr[1], arr[2],arr[3]);
        if(date.getFullYear() != arr[1])
            return false;
        if(date.getMonth() != arr[2])
            return false;
        if(date.getDate() != arr[3])
            return false;

        return true;
    };
function getQueryString(name) {
	var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)", "i");
	var r = window.location.search.substr(1).match(reg);
	if (r != null) return unescape(r[2]); return null;
}
//2.0新加总价单价切换
function price_switch(){
	$(".maximum div:nth-child(1)").on("click",function(){
		localStorage.setItem('tbs_totalAndsinSwitch', 0);
		$(this).addClass("price-active");
		$(".maximum div:nth-child(2)").removeClass("price-active");
		$("#hlprice-container-0").show();
		$("#hlprice-container-1").hide()
	});
	$(".maximum div:nth-child(2)").on("click",function(){
		localStorage.setItem('tbs_totalAndsinSwitch', 1);
		$(this).addClass("price-active");
		$(".maximum div:nth-child(1)").removeClass("price-active");
		$("#hlprice-container-0").hide();
		$("#hlprice-container-1").show()
	});
}
var ChartInfo = {};
function parse_trade_chart_data($data) {
        if ('calcdate' in $data == false || $data['calcdate'].length < 6)
            return null;
        if ('1' in $data == true && $data['1'].length < 6)
            return null;
        if ('2' in $data == true && $data['2'].length < 6)
            return null;

        var i;
        var idx;
        var info = {
            x : []
            ,y : []
            ,pt : {
                first  : []
                ,second : []
            }
            ,price : null
            ,m3 : {
                base : null
                ,refer : null
            }
            ,m6 : {
                base : null
                ,refer : null
            }
        };

        var year = 0;
        var dt;
        idx = -1;
        for (i = 0; i < 6; i++) {
            dt = Date.parseApi($data['calcdate'][i]);
            if (dt.getFullYear() != year) {
                year = dt.getFullYear();
                info.x[++idx] = (dt.getMonth() + 1) + '月<br />' + year;
            }
            else {
                info.x[++idx] = (dt.getMonth() + 1) + '月';
            }
        }

        var min = 0x7FFFFFFF;
        var max = 0;
        var minStep  = 100;
        var minRange = minStep * 3;
        var price;
        if('1' in $data == true)
        {
            for (i = 0; i < 6; i++)
            {
                price = parseFloat($data[1][i]);
                if(isNaN(price) == true || price < 0)
                    continue;

                min = Math.min(min, price);
                max = Math.max(max, price);
            }
        }
        if('2' in $data == true)
        {
            for (i = 0; i < 6; i++)
            {
                price = parseFloat($data[2][i]);
                if(isNaN(price) == true || price < 0)
                    continue;

                min = Math.min(min, price);
                max = Math.max(max, price);
            }
        }
        var minY  = 0;
        var maxY  = 0;
        var stepY = minStep;

        if(max - min < minRange)
        {
            minY = Math.max(0, min - (minRange-max+min)/2);
            minY = Math.floor(minY / 100) * 100;
        }
        else
        {
            minY  = Math.floor(min / 100) * 100;
            stepY = (max - minY)/3;
            stepY = Math.ceil(stepY / 100) * 100 + minStep;
        }
        idx = -1;
        for(i=0; i<4; i++)
        {
            info.y[++idx] = minY + stepY * i;
            maxY = minY + stepY * i;
        }

        var areaHeight = 165;
        var baseY      = 180;
        var arrPtRight = [580,468,356,244,132,20];
        if('1' in $data == true)
        {
            for(i=0; i<6; i++)
            {
                price = parseFloat($data[1][i]);
                if(isNaN(price) == true || price <= 0)
                    info.pt.first[i] = null;
                else
                {
                    info.pt.first[i] = {
                        right : arrPtRight[i]
                        ,y    : baseY - Math.floor(areaHeight*(price-min)/(maxY-minY))
                    };
                    if(courtHand == 1)
                    {
                        info.price = price;

                        if(i == 0)
                            info.m6.base = price;
                        else
                            info.m6.refer = price;

                        if(i == 3)
                            info.m3.base = price;
                        else if(i > 3)
                            info.m3.refer = price;
                    }
                }
            }
        }
        if('2' in $data == true)
        {
            for(i=0; i<6; i++)
            {
                price = parseFloat($data[2][i]);
                if(isNaN(price) == true || price <= 0)
                    info.pt.second[i] = null;
                else
                {
                    info.pt.second[i] = {
                        right : arrPtRight[i]
                        ,y    : baseY - Math.floor(areaHeight*(price-min)/(maxY-minY))
                    };
                    if(courtHand == 2)
                    {
                        info.price = price;

                        if(i == 0)
                            info.m6.base = price;
                        else
                            info.m6.refer = price;

                        if(i == 3)
                            info.m3.base = price;
                        else if(i > 3)
                            info.m3.refer = price;
                    }
                }
            }
        }

        return info;
}
function load_trade_chart()
    {
        if(flagData == false)
        {
            block_hide('.block-trade-chart');
            return;
        }

        var dtNow = new Date();
        var handle = $.ajax({
            method : 'post'
            //,url   : apiBase + '/house/getHouseAPrice'
            , url: 'http://ZJB.com/xzht/2016/getHouseAPriceRoomtypeMix.html'
            ,data  : {
                city_id   : cityId
                ,house_id : 25804
                ,calcdate : dtNow.format('yyyy-MM-dd')
            }
            ,dataType : 'json'
        });
        handle.done(function($data, $status, $jqXHR){
            if($data == 0)
            {
                block_hide('.block-trade-chart');
                return;
            }
            if(Array.isArray($data) == false)
            {
                load_fail('.block-trade-chart', $jqXHR, 'error', 'invalid data');
                return;
            }
            if($data.length <= 0)
            {
                load_fail('.block-trade-chart', $jqXHR, 'error', 'no data');
                return;
            }

            load_ok('.block-trade-chart');

            //$data[0][1] = [1001,3040,5500,5420,1000,2044];
            //$data[0][2] = [101,300,1000,4620,2000,3044];
            //console.log($data);

            var domFrame = $('.block-trade-chart .trade-chart-tab');
            domFrame.html('');
            var arr, label;
            for(var i=0; i<$data.length; i++)
            {
                arr = $data[i];
                label = arr['roomtype']+'房';
                if(arr['roomtype'] >= 5) {
                    label += '及以上';
                }
                if (arr['roomtype'] == 0) {
                    label = "全部";
                }

                var newDomNode = $('<div class="tab" data-key="'+arr['roomtype']+'">'+label+'</div>');
                if (arr['roomtype'] == 0) {
                    newDomNode.prependTo(domFrame);
                } else {
                    newDomNode.appendTo(domFrame);
                }

                ChartInfo[arr['roomtype']] = parse_trade_chart_data(arr);
            }

            domFrame.click(function($evt){
                var key = $($evt.target).attr('data-key');
                console.log("qqqqqqqqqqqqqqq");
                console.log(key);
                trade_chart_select_tab(key);

            });

            var dom = $('.block-trade-chart .trade-chart-tab .tab:first-child')[0];
            trade_chart_select_tab($(dom).attr('data-key'));
        });
        handle.fail(function($jqXHR, $status, $error){
            load_fail('.block-trade-chart', $jqXHR, $status, $error);
        });
}
function load_ok($blockSelector)//将正在加载点状样式去掉
{
	$($blockSelector).removeClass('frame-block-loading');
}
function trade_chart_select_tab($key)
    {
        if($key==1){
            if(window.courtHand == 1){
                window.eventForZG(174, {'houseId': window.houseId, 'houseName': window.houseName});
            }else if(window.courtHand == 2){
                window.eventForZG(131, {'houseId': window.houseId, 'houseName': window.houseName});
            }
        }else if($key==2){
            if(window.courtHand == 1){
                window.eventForZG(175, {'houseId': window.houseId, 'houseName': window.houseName});
            }else if(window.courtHand == 2){
                window.eventForZG(132, {'houseId': window.houseId, 'houseName': window.houseName});
            }
        }else if($key==3){
            if(window.courtHand == 1){
                window.eventForZG(176, {'houseId': window.houseId, 'houseName': window.houseName});
            }else if(window.courtHand == 2){
                window.eventForZG(133, {'houseId': window.houseId, 'houseName': window.houseName});
            }
        }else if($key==4){
            if(window.courtHand == 1){
                window.eventForZG(177, {'houseId': window.houseId, 'houseName': window.houseName});
            }else if(window.courtHand == 2){
                window.eventForZG(134, {'houseId': window.houseId, 'houseName': window.houseName});
            }
        }else if($key==5){
            if(window.courtHand == 1){
                window.eventForZG(178, {'houseId': window.houseId, 'houseName': window.houseName});
            }else if(window.courtHand == 2){
                window.eventForZG(135, {'houseId': window.houseId, 'houseName': window.houseName});
            }
        }else{
            if(window.courtHand == 1){
                if(is_first_trade){
                    is_first_trade = false;
                }else {
                    window.eventForZG(173, {'houseId': window.houseId, 'houseName': window.houseName});
                }
            }else if(window.courtHand == 2){
                if(is_first_trade){
                    is_first_trade = false;
                }else {
                    window.eventForZG(130, {'houseId': window.houseId, 'houseName': window.houseName});
                }
            }
        }
        $('.block-trade-chart .trade-chart-tab .tab').removeClass('tab-active');
        $('.block-trade-chart .trade-chart-tab .tab[data-key='+$key+']').addClass('tab-active');

        var i = 0;
        var info = ChartInfo[$key];
        for(i=0; i<info.x.length; i++)
        {
            $('.trade-chart-area .x-'+i).html(info.x[i]);
        }
        for(i=0; i<info.y.length; i++)
        {
            $('.trade-chart-area .y-'+i).html(info.y[i]);
        }
        // Dot
        var y;
        var right;
        var jqDotDom;
        $('.block-trade-chart .trade-chart-area .dot').hide();
        for(i=0; i<info.pt.first.length; i++)
        {
            jqDotDom = $('.trade-chart-area .dot-first-'+i);
            if(info.pt.first[i] == null)
            {
                jqDotDom.hide();
                jqDotDom.css({top:'0px'});
            }
            else
            {
                y     = info.pt.first[i].y;
                right = info.pt.first[i].right;

                jqDotDom.show();
                jqDotDom.css({top:y+'px',right:right+'px'});
            }
        }
        for(i=0; i<info.pt.second.length; i++)
        {
            jqDotDom = $('.trade-chart-area .dot-second-'+i);
            if(info.pt.second[i] == null)
            {
                jqDotDom.hide();
                jqDotDom.css({top:'0px'});
            }
            else
            {
                y     = info.pt.second[i].y;
                right = info.pt.second[i].right;

                jqDotDom.show();
                jqDotDom.css({top:y+'px',right:right+'px'});
            }
        }
        // Line
        var lineBegin = null;
        var lineEnd   = null;
        var jqLineDom;
        var tmpWidth;
        var tmpHeight;
        var lineWidth;
        var lineDegree;

        $('.block-trade-chart .frame-line .line').hide();
        for(i=0; i<info.pt.first.length; i++)
        {
            if(info.pt.first[i] == null)
                continue;

            if(lineBegin == null)
            {
                lineBegin = info.pt.first[i];
                continue;
            }

            lineEnd = info.pt.first[i];

            jqLineDom  = get_line_jqDom('first', i);
            tmpWidth  = lineBegin.right-lineEnd.right;
            tmpHeight = lineEnd.y-lineBegin.y;
            lineWidth  = Math.floor(Math.sqrt(tmpWidth*tmpWidth+tmpHeight*tmpHeight));
            lineDegree = 180 * Math.atan2(tmpHeight, tmpWidth)/Math.PI;
            jqLineDom.css({
                width      : lineWidth+'px'
                ,top       : (lineBegin.y+8)+'px'
                ,right     : (lineBegin.right - lineWidth)+'px'
                ,opacity   : 1
                ,transform : 'rotate('+lineDegree+'deg)'
            });
            jqLineDom.show();
            lineBegin = lineEnd;
        }
        lineBegin = null;
        lineEnd   = null;
        for(i=0; i<info.pt.second.length; i++)
        {
            if(info.pt.second[i] == null)
                continue;

            if(lineBegin == null)
            {
                lineBegin = info.pt.second[i];
                continue;
            }

            lineEnd = info.pt.second[i];

            jqLineDom  = get_line_jqDom('second', i);
            tmpWidth  = lineBegin.right-lineEnd.right;
            tmpHeight = lineEnd.y-lineBegin.y;
            lineWidth  = Math.floor(Math.sqrt(tmpWidth*tmpWidth+tmpHeight*tmpHeight));
            lineDegree = 180 * Math.atan2(tmpHeight, tmpWidth)/Math.PI;
            jqLineDom.css({
                width      : lineWidth+'px'
                ,top       : (lineBegin.y+8)+'px'
                ,right     : (lineBegin.right - lineWidth)+'px'
                ,opacity   : 1
                ,transform : 'rotate('+lineDegree+'deg)'
            });
            jqLineDom.show();
            lineBegin = lineEnd;
        }
        // Info
        var jqDom = $('.chart-info-table .info-price .info');
        if(info.price == null)
        {
            jqDom.attr('class', 'info info-none');
            jqDom.text('没有数据');
        }
        else
        {
            // alert("1761不知道原因操作"+info.price);
            jqDom.text(info.price+'元/平');
        }
        jqDom = $('.chart-info-table .info-m3 .info');
        if(info.m3.base == null || info.m3.refer == null)
        {
            jqDom.attr('class', 'info info-none');
            jqDom.text('没有数据');
        }
        else
        {
            if(info.m3.refer >= info.m3.base)
            {
                jqDom.attr('class', 'info info-rise');
                var v = Math.ceil((info.m3.refer/info.m3.base-1)*100);
                jqDom.text(v+'%')
            }
            else
            {
                jqDom.attr('class', 'info info-fail');
                var v = Math.ceil((info.m3.refer/info.m3.base-1)*100);
                jqDom.text(v+'%')
            }
        }
        jqDom = $('.chart-info-table .info-m6 .info');
        if(info.m6.base == null || info.m6.refer == null)
        {
            jqDom.attr('class', 'info info-none');
            jqDom.text('没有数据');
        }
        else
        {
            if(info.m6.refer >= info.m6.base)
            {
                jqDom.attr('class', 'info info-rise');
                var v = Math.ceil((info.m6.refer/info.m6.base-1)*100);
                jqDom.text(v+'%')
            }
            else
            {
                jqDom.attr('class', 'info info-fail');
                var v = Math.ceil((info.m6.refer/info.m6.base-1)*100);
                jqDom.text(v+'%')
            }
        }
}
function load_fail($blockSelector, $jqXHR, $status, $error)
    {
        if('apiFail' in window == true && apiFail == 'hide')
        {
            console.log('fail - hide ['+$blockSelector+']');
            block_hide($blockSelector);
            return;
        }

        var domFrame = $($blockSelector);
        domFrame.removeClass('frame-block-loading');
        domFrame.addClass('frame-block-fail');
        domFrame.attr('data-fail', '[error:'+$error+'][api return:'+$jqXHR.responseText+']')
    }

    function block_hide($blockSelector)
    {
        var domFrame = $($blockSelector);

        //Console.log("domFramedomFramedomFramedomFrame"+domFrame);
        if(domFrame.hasClass("blockFlag")){

            return;
        }
        domFrame.prev('.frame-block-title').hide();
        domFrame.hide();
    }
function load_surrounding_map(){
        console.log("pppppppppppppp");
        var map = new BMap.Map("surrounding-map");

        // 百度地图API功能
        // map.centerAndZoom(new BMap.Point(116.404, 39.915), 11);  // 初始化地图,设置中心点坐标和地图级别
        var _city_id = localStorage.getItem('tbs_cityId');
        var _city;
        if(_city_id == 605){
            _city = "上海";
        }else if(_city_id == 604){
            _city = "北京";
        }else if(_city_id == 607){
            _city = "深圳";
        }else if(_city_id == 626){
            _city = "杭州";
        } else{
            _city = "厦门";
        }
        console.log(_city);
        map.centerAndZoom(_city, 15);
        map.disableDragging();
        map.disablePinchToZoom();
        var local = new BMap.LocalSearch(map, {
            renderOptions:{map: map, autoViewport: true, selectFirstResult: true},
            pageCapacity: 1
        });
        local.disableFirstResultSelection();
        //local.search(window.houseName.replace("(新房)","") );
        local.setSearchCompleteCallback(function(xx){
            if( !local.getResults().getPoi(0)){
                hide_Map();
                return;
            }
            var _site = "地址: " + local.getResults().getPoi(0).address;
            if(typeof _site == "string" && _site.length > 20){
                _site = _site.substr(0, 20) + "...";
            }
            $("#surrounding-title").html(_site);
        })
    }