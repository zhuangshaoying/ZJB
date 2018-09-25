var Attention = {
    City: function (s) {
        switch (s) {
            case "1":
                return "厦门";
            case "2":
                return "漳州";
            case "3":
                return "泉州";
            case "4":
                return "福州";
            case "5":
                return "龙岩";
            case "6":
                return "莆田";
            case "7":
                return "三明";
            case "8":
                return "南平";
            case "9":
                return "宁德";
            default:
                return " ";
        }
    },
    RegionName: function (regionCode) {
        switch (regionCode) {
            case "3505240000":
                return "安溪县";
            case "3501040000":
                return "仓山区";
            case "3501820000":
                return "长乐市";
            case "3506250000":
                return "长泰县";
            case "3508210000":
                return "长汀县";
            case "3503020000":
                return "城厢区";
            case "3504250000":
                return "大田县";
            case "3505260000":
                return "德化县";
            case "3506260000":
                return "东山县";
            case "3505030000":
                return "丰泽区";
            case "3509810000":
                return "福安市";
            case "3509820000":
                return "福鼎市";
            case "3501810000":
                return "福清市";
            case "3509220000":
                return "古田县";
            case "3501020000":
                return "鼓楼区";
            case "3507230000":
                return "光泽县";
            case "3502050000":
                return "海沧区";
            case "3503030000":
                return "涵江区";
            case "3502060000":
                return "湖里区";
            case "3506290000":
                return "华安县";
            case "3505210000":
                return "惠安县";
            case "3502110000":
                return "集美区";
            case "3504300000":
                return "建宁县";
            case "3507830000":
                return "建瓯市";
            case "3507840000":
                return "建阳市";
            case "3504280000":
                return "将乐县";
            case "3509020000":
                return "蕉城区";
            case "3501110000":
                return "晋安区";
            case "3505820000":
                return "晋江市";
            case "3505020000":
                return "鲤城区";
            case "3503040000":
                return "荔城区";
            case "3508250000":
                return "连城县";
            case "3501220000":
                return "连江县";
            case "3506810000":
                return "龙海市";
            case "3506030000":
                return "龙文区";
            case "3501230000":
                return "罗源县";
            case "3505040000":
                return "洛江区";
            case "3501050000":
                return "马尾区";
            case "3504020000":
                return "梅列区";
            case "3501210000":
                return "闽候县";
            case "3501240000":
                return "闽清县";
            case "3504210000":
                return "明溪县";
            case "3505830000":
                return "南安市";
            case "3506270000":
                return "南靖县";
            case "3504240000":
                return "宁化县";
            case "3506280000":
                return "平和县";
            case "3501280000":
                return "平潭县";
            case "3509230000":
                return "屏南县";
            case "3507220000":
                return "浦城县";
            case "3504230000":
                return "清流县";
            case "3505010000":
                return "清濛区";
            case "3505050000":
                return "泉港区";
            case "3504030000":
                return "三元区";
            case "3504270000":
                return "沙县	";
            case "3508230000":
                return "上杭县";
            case "3507810000":
                return "邵武市";
            case "3505810000":
                return "石狮市";
            case "3509240000":
                return "寿宁县";
            case "3507210000":
                return "顺昌县";
            case "3502030000":
                return "思明区";
            case "3507240000":
                return "松溪县";
            case "3501030000":
                return "台江区";
            case "3504290000":
                return "泰宁县";
            case "3502120000":
                return "同安区";
            case "3509260000":
                return "拓荣县";
            case "3508240000":
                return "武平县";
            case "3509210000":
                return "霞浦县";
            case "3503220000":
                return "仙游县";
            case "3506020000":
                return "芗城区";
            case "3502130000":
                return "翔安区";
            case "3508020000":
                return "新罗区";
            case "3503050000":
                return "秀屿区";
            case "3507020000":
                return "延平区";
            case "3504810000":
                return "永安市";
            case "3505250000":
                return "永春县";
            case "3508220000":
                return "永定县";
            case "3501250000":
                return "永泰县";
            case "3504260000":
                return "尤溪县";
            case "3506220000":
                return "云霄县";
            case "3508810000":
                return "漳平市";
            case "3506230000":
                return "漳浦县";
            case "3506240000":
                return "诏安县";
            case "3507250000":
                return "政和县";
            case "3509250000":
                return "周宁县";
            case "3506844000":
                return "漳州台商投资区";
            case "3506830000":
                return "招商局漳州开发区";
            case "3508000002":
                return "漳平工业园区	";
            case "3507820000":
                return "武夷山市";
            case "3505000002":
                return "台商投资区";
            case "3508000001":
                return "龙岩开发区";
            case "3506000001":
                return "龙池开发区";
            case "3505000001":
                return " 经济技术开发区";
            case "3509300000":
                return "东侨开发区";
            default:
                return "";
        }
    },
    GetLouPan: function (sid) {
        var lp = [];

        for (var i = 0; i < loupanHitsRank.length; i++) {
            if (loupanHitsRank[i].s == sid) {
                lp.push(loupanHitsRank[i]);
            }
        }
        return lp;
    },
    RegionTitle: function (loupan) {
        var region = [];
        for (var i = 0; i < loupan.length; i++) {
            !RegExp(loupan[i].r, "g").test(region.join(",")) && (region.push(loupan[i].r));
        }
        return region;
    },
    RegionTiteName: function (arr) {
        var rn = [];
        for (var i = 0; i < arr.length; i++) {
            rn.push(this.RegionName(arr[i]));
        }
        return rn;
    },
    HtmlPriceBoxTitle: function (arr) {
        var addclass = '';
        var html = '';
        for (var i = 0; i < arr.length; i++) {
            //  html += ' <li><a href="javascript:;">' + arr[i] + '</a> </li>';
            if (i == 0) {
                addclass = "on";
            } else if (i == arr.length - 1) {
                addclass = "end";
            } else {
                addclass = "";
            }
            html += '<div class="' + addclass + ' " >' + arr[i] + '</div>';
        }
        return html;
    },
    HtmlListContent: function (all, arr) {
        var html = "";
        var regionArr = [];
        // for (var i = 0; i < all.length; i++) {

        //    html += '<div class="lp-area" id="r' + all[i] + '" >';

        regionArr = [];
        for (var j = 0; j < arr.length; j++) {
            //  if (arr[j].r == all[i]) {
            regionArr.push(arr[j]);
            //   }
        }
        _temp.prototype = tablesort;
        var sort1 = new _temp(regionArr, "price", "down"); //建立对象
        sort1.init(regionArr, "ref", "down"); //初始化参数更改
        sort1.sot();
        for (var j = 0; j < regionArr.length; j++) {
            if (regionArr[j].rank > 100) {
                break;
            }
            var change = regionArr[j].p, prstyle = '', bhimg = '', imgalt = '';
            if (change > 0) {
                prstyle = "red";
                bhimg = "images/p2.png";
                imgalt = "楼盘售价与上月比上涨";
            } else if (change < 0) {
                prstyle = "gray";
                bhimg = "images/p3.png";
                imgalt = "楼盘售价与上月比下跌";
            } else {
                prstyle = "gray";
                bhimg = "images/p1.png";
                imgalt = "楼盘售价与上月比无变动";
            }
            var price = regionArr[j].ref == "0" ? "待定" : regionArr[j].ref + "元";
            var rank = regionArr[j].rank;

            html += ' <li><a href="http://m.xmhouse.com/house/house_detail.aspx?id=' + regionArr[j].id + '"><span class="lp_name">' + regionArr[j].n + '</span><span class="lp_price ' + prstyle + '"><span class="spanprice">' + price + '</span><img src="' + bhimg + '" alt="" title="' + imgalt + '"  /></span></a></li>';
            //   html += '<li><a target="_blank" href="/loupan' + regionArr[j].id + '"><span class="lp_name">';
            //if (rank > 3) {
            //    html += '  <i class="rank" title="该楼盘在' + this.RegionName(all[i]) + '排名: 第' + rank + '名">' + rank + '</i>';
            //} else {
            //    html += '  <i  title="该楼盘在' + this.RegionName(all[i]) + '排名: 第' + rank + '名">' + rank + '</i>';
            //}
            //html += '<em>' + regionArr[j].n + '</em></span><span class="price ' + prstyle + '">' + price + '</span><img src="' + bhimg + '" alt="" title="' + imgalt + '" width="16" height="17" /></a></li>';
            //var jj = j + 1;
            //var bc = Math.ceil(regionArr.length / 4);
            //if (jj % bc == 0 && jj != 100) {
            //    html += '</ul><ul>';
            //}
        }
        //     html += '</div>';
        // }
        return html;
    }
};

var tablesort = {
    init: function (arry, parm, sortby) {
        this.obj = arry
        this.parm = parm
        this.b = sortby
    },

    sot: function () {
        var $this = this
        var down = function (x, y) {
            return (eval("x." + $this.parm) > eval("y." + $this.parm)) ? -1 : 1
        } //通过eval对json对象的键值传参
        var up = function (x, y) {
            return (eval("x." + $this.parm) < eval("y." + $this.parm)) ? -1 : 1
        }
        if (this.b == "down") {
            this.obj.sort(down)
        }
        else {
            this.obj.sort(up)
        }

    } //遍历添加dom元素，添加dom
}

function _temp() {
    this.init.apply(this, arguments)
}

$(function () {
    var cityName = Attention.City(siteid);
    var siteloupan = Attention.GetLouPan(siteid);
    var regionAll = Attention.RegionTitle(siteloupan);
    var cityRegion = Attention.RegionTiteName(regionAll);
    $("#tags").html(Attention.HtmlPriceBoxTitle(cityRegion));
    $("#cell").html(Attention.HtmlListContent(regionAll, siteloupan));
    qh();
});


var qh = function () {
    var oHead = document.getElementsByTagName('HEAD').item(0);
    var oScript = document.createElement("script");
    oScript.type = "text/javascript";
    oScript.src = "/Js/v4/tags.js";
    oHead.appendChild(oScript);
}
//区域筛选
var region;
var regionName;
var price;


function OnclickRegion(r, rname) {
    region = r;
    regionName = rname;
    Load();
}
function OnclickPrice(p) {
    price = p;
    Load();
}
function Load() {
    var price1 = 0;
    var price2 = 10000000;

    if (price == 6) {
        $("#selprice").html("7000以下");
        price2 = 7000;
    }
    if (price == 78) {
        $("#selprice").html("7000~9000");
        price1 = 7000;
        price2 = 9000;
    }
    if (price == 90) {
        $("#selprice").html("9000~12000");
        price1 = 9000;
        price2 = 12000;
    }
    if (price == 11) {
        $("#selprice").html("12000~15000");
        price1 = 12000;
        price2 = 15000;
    }
    if (price == 12) {
        $("#selprice").html("15000~20000");
        price1 = 15000;
        price2 = 20000;
    }
    if (price == 13) {
        $("#selprice").html("20000以上");
        price1 = 20000;
        price2 = 10000000;
    }

    if (region == 0) {
        if (price == 0) {
            $("#cell").find("div").show();
        } else {
            var liarr = $("#cell").find("div").find("li");
            for (var i = 0; i < liarr.length; i++) {
                var t = parseInt($(liarr[i]).find(".spanprice").html());
                if (t >= price1 && t < price2) {
                    $(liarr[i]).show();
                } else {
                    $(liarr[i]).hide();
                }

            }
        }
    } else {
        $("#selareaname").html(regionName);
        $("#cell").find("div").hide();

        $(("#r" + region)).show();
        var liarr = $(("#r" + region)).find("li");
        for (var i = 0; i < liarr.length; i++) {
            var t = parseInt($(liarr[i]).find(".spanprice").html());
            if (t >= price1 && t < price2) {
                $(liarr[i]).show();
            } else {
                $(liarr[i]).hide();
            }

        }

    } hidemenu();
}