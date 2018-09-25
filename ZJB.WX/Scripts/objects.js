/**
 * 对象
 */

Date.prototype.format=function(fmt) {        
    var o = {        
	    "M+" : this.getMonth()+1, //月份        
	    "d+" : this.getDate(), //日        
	    "h+" : this.getHours()%12 == 0 ? 12 : this.getHours()%12, //小时        
	    "H+" : this.getHours(), //小时        
	    "m+" : this.getMinutes(), //分        
	    "s+" : this.getSeconds(), //秒        
	    "q+" : Math.floor((this.getMonth()+3)/3), //季度        
	    "S" : this.getMilliseconds() //毫秒        
    };        
    var week = {        
	    "0" : "\u65e5",        
	    "1" : "\u4e00",        
	    "2" : "\u4e8c",        
	    "3" : "\u4e09",        
	    "4" : "\u56db",        
	    "5" : "\u4e94",        
	    "6" : "\u516d"       
    };        
    if(/(y+)/.test(fmt)){        
        fmt=fmt.replace(RegExp.$1, (this.getFullYear()+"").substr(4 - RegExp.$1.length));    
    }        
    if(/(E+)/.test(fmt)){        
        fmt=fmt.replace(RegExp.$1, ((RegExp.$1.length>1) ? (RegExp.$1.length>2 ?  
        		"\u661f\u671f" : "\u5468") : "")+week[this.getDay()+""]);        
    }        
    for(var k in o){        
        if(new RegExp("("+ k +")").test(fmt)){        
            fmt = fmt.replace(RegExp.$1, (RegExp.$1.length==1) ? (o[k]) : (("00"+ o[k]).substr((""+ o[k]).length)));        
        }        
    }        
    return fmt;        
};

function Map(){
	this.container = new Object();
}

Map.prototype.put = function(key, value){
	this.container[key] = value;
};

Map.prototype.get = function(key){
	return this.container[key];
};

Map.prototype.keySet = function() {
	var keyset = new Array();
	var count = 0;
	for (var key in this.container) {
	// 跳过object的extend函数
	if (key == 'extend') {
		continue;
	}
	keyset[count] = key;
		count++;
	}
	return keyset;
};

Map.prototype.size = function() {
	var count = 0;
	for (var key in this.container) {
	// 跳过object的extend函数
		if (key == 'extend'){
			continue;
		}
		count++;
	}
	return count;
};

Map.prototype.remove = function(key) {
	delete this.container[key];
};

Map.prototype.toString = function(){
	var str = "";
	for (var i = 0, keys = this.keySet(), len = keys.length; i < len; i++) {
		str = str + keys[i] + "=" + this.container[keys[i]] + ";\n";
	}
	return str;
};

var city_map = new Map();
city_map.put(592, "厦门");
city_map.put(591, "福州");
city_map.put(571, "杭州");
city_map.put(791, "南昌");
