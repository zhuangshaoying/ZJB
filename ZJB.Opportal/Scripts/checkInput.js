/**
 * 输入验证
 */
function checkRegexp(value, regexp) {
	if (!( regexp.test(value))) {
		return false;
	} else {
		return true;
	}
}

function checkRegexpWithHint(obj, regexp, hint) {
	if (!(regexp.test(obj.val()))) {
		art.dialog.alert(hint, function () {obj.focus();});
		return false;
	} else {
		return true;
	}
}

function checkRange(value, min, max) {
	value = parseInt(value);
	if (isNaN(value) || value > max || value < min) {
		return false;
	} else {
		return true;
	}
}

function checkRangeWithHint(value, min, max, name) {
	value = parseInt(value);
	if (isNaN(value) || value > max || value < min) {
		art.dialog.alert(name + "必须在" + min + " 到 " + max + "之间");
		return false;
	} else {
		return true;
	}
}

function compareValue(value1, value2, hint) {
	if (value1 < value2) {
		art.dialog.alert(hint);
		return false;
	} else {
		return true;
	}
}

function compareValueNotEqual(value1, value2, hint) {
	if (value1 <= value2) {
		art.dialog.alert(hint);
		return false;
	} else {
		return true;
	}
}

function compareObject(obj1, obj2, name1, name2) {
	if (parseInt(obj1.val()) < parseInt(obj2.val())) {
		art.dialog.alert(name1 + "不能小于" + name2);
		return false;
	} else {
		return true;
	}
}