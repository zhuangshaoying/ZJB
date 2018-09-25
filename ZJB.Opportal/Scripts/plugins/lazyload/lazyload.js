
function checkbrowse() {
	var ua = navigator.userAgent.toLowerCase(); 
	var is = (ua.match(/\b(chrome|opera|safari|msie|firefox)\b/) || ['', 'mozilla'])[1]; 
	var r = '(?:' + is + '|version)[\\/: ]([\\d.]+)'; 
	var v = (ua.match(new RegExp(r)) || [])[1]; 
	jQuery.browser.is = is; 
	jQuery.browser.ver = v; 
	return { 
	'is': jQuery.browser.is, 
	'ver': jQuery.browser.ver 
	};
	} 

var public = checkbrowse();
var showeffect = "";
if ((public['is'] == 'msie' && public['ver'] < 8.0)) {
		showeffect = "show";
	} else {
		showeffect = "fadeIn";
	} 

$(function () {
    
	$("img").lazyload({ 
		placeholder: "/images/grey.gif",
		effect: showeffect,
		failurelimit: 10
		});
});
