/**
* @Description: 字符串拼接
* @CreateTime: 2014-1-18 下午2:42:35
* @author: chenzw 
* @version V1.0
*/
function StringBulider() {
    this.data = new Array();
}

/**
* 拼接字符串,可以连续拼接
* @return {}
*/
StringBulider.prototype.append = function () {
    var length = arguments.length;
    if (length == 0) return this;
    for (var i = 0; i < length; i++) {
        this.data.push(arguments[i]);
    }
    return this;
}

/**
* 转成字符串输出
* @return {}
*/
StringBulider.prototype.toString = function () {
    if (arguments.length > 0) {
        return this.data.join(arguments[0]);
    }
    else {
        return this.data.join('');
    }
}

/**
* 判断字符串数组是否为空
* @return {}
*/
StringBulider.prototype.isEmpty = function () {
    return this.data.length <= 0;
}

/**
* 清空字符串数组
*/
StringBulider.prototype.clear = function () {
    this.data = [];
    this.data.length = 0;
}