
$(function () {
    //前往添加账号
    $(".button_add_account").die().live("click", function () {
        parent.$("#personManagerDT").click();
        parent.$("#webManager").click();
        href = parent.$("#webManager").attr("href");
        url = href.substring(href.indexOf("'") + 1, href.lastIndexOf("'"));
        parent.hrefLink(url);
    });
    $(".set_plan_state_function").die().live("click", function () {
        var siteId = $(this).attr("siteid");
        setRefreshPlan(siteId, $(this));
    });
    
    //右侧顶部tab
    $("#refresh_title_tab_navs_function li").die().live("click", function () {
        var navObjs = $("li.sale-sub-menu"); //title_tab_navs
        var index = navObjs.index($(this));
        changeTable(index);
    });
    changeTable($("#tabIndex").val());
});
function changeTable(index) {
    if (index == 0) {
        InitRefresh();
    }
    else if (index == 2) {
        InitRefreshSite();
        InitRefreshLog(1);
    }
    $("#tabIndex").val(index);
    $("#refresh_" + index).addClass("selected");
    $("#refresh_" + index).siblings().removeClass("selected");
    var bdObjs = $(".main_bd_function");
    bdObjs.hide();
    $(".main_bd_function:eq(" + index + ")").show();
}
//******************** 刷新设置
function NoConentTip()
{
    var initTxt = ' <div class="saleHouse-main"> <div style="font-family:微软雅黑;font-size:18px;padding:25px 0px 0px 20px;color:#222;line-height:24px">功能正在开发中，敬请期待……</div></div>';
    $(".main_bd_function").eq(0).html(initTxt);
}
function InitRefresh()
{
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Refresh/GetRefreshWeb',
        beforeSend: function () {
            if (parent) {
                parent.loadingShow();
            }
        },
        success: function (result) {
            if (result.length > 0) {
                var html = BuildRefreshHtml(result);
                $("#lay_card_panel_function").html(html);
            }
            else {
                NoConentTip();
            }
        },
        complete: function () {
            if (parent) {
                parent.loadingHide();
            }
        }
    });
}
function BuildRefreshHtml(data)
{
    var html = "";

    $.each(data, function (i, n) {
        var btnSettingName = n.State == 1 ? "关闭云刷新" : "开启云刷新";
        var labelTxt = n.State == 1 ? "开启" : "关闭";
        var setRefreshBtn = "";
        if (n.SiteUserName != "缺少账号"&&n.SiteID != 1)
        {
            setRefreshBtn = '<li><span class="set_plan_function" pid="' + n.SiteID + '">设置</span></li>';
        }
            var btnSetting = '';
            if (n.SiteUserName == "缺少账号") {
                btnSettingName = "立即添加";
                btnSetting = '<button class="button_add_account">' + btnSettingName + '</button>';
            }
            else {
                if (n.SiteID == 1) {
                    btnSetting = '<span class="set_plan_state_function" siteId="' + n.SiteID + '" state="' + n.State + '">' + btnSettingName + '</span>';
                }
            }
            var labelSeting = "";
            if (n.SiteUserName != "缺少账号")
            {
                labelSeting = '    <li class="currentState">当前状态:<label>' + labelTxt + '</label></li>';
            }
            html +=
            '<div class="lay_card_top">' +
            '   <img class="logo" src="' + n.Logo + '">' +
            '<div class="content blue_bg">' +
            '  <ul class="left_part" id="plan_select_function">' +
            '    <li class="account" title="' + n.SiteUserName + '">' + n.SiteUserName + '</li> ' +
            '' + labelSeting+
            '    <li>' + btnSetting + '</li>' +
			' ' + setRefreshBtn+
            '   </ul>' +
            ' </div>' +
            '</div>';
        });
 
    return html;
}
function setRefreshPlan(siteId,targetObj)
{
    var state=targetObj.attr("state") == "1"?"0":"1";
    var btnName = targetObj.attr("state") == "1" ? "开启云刷新" : "关闭云刷新";
    var labelTxt = targetObj.attr("state") == "1" ? "关闭" : "开启";
    if (siteId > 0) {
        $.ajax({
            type: 'post',
            url: '/Refresh/RefreshPlanState',
            data: { siteId: siteId },
            success: function (result) {
                art.dialog.tips(result.msg, 1.5);
                if (result.status > 0) {
                    targetObj.attr("state", state);
                    targetObj.parent("li").siblings("li.currentState").find("label").html(labelTxt);
                    targetObj.html(btnName);

                }
            }
        });
    }
    else {
        art.dialog.tips("请选择站点！", 1.5);
    }
}

//******************* 刷新日志
var pi = 1, ps = 20;
var buildTypeMap = ["不限", "住宅", "别墅", "商铺", "写字楼", "厂房"]//房屋类型 0不限 1 住宅 2 别墅 3 商铺 4写字楼 5厂房
function InitRefreshLog(currentPage)
{
    pi = currentPage > 0 ? currentPage : 1;
    var siteId = $("#selectSiteId").val();
    var siteUserName = $("#txtSiteUserName").val();
    var viewData = $("#selectViewData").val();
    var refreshMode = $("#selectRefrehMode").val();
    var planNo = $("#selectPlanNo").val();
    var time = $("#txtTime").val();
    var listParame = {
        pi: pi,
        ps: ps,
        SiteId: siteId,
        SiteUserName: siteUserName,
        ViewData: viewData,
        RefreshMode: refreshMode,
        PlanNo: planNo,
        Time: time
    };
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Refresh/RefreshLogList',
        data: listParame,
        beforeSend: function () {
            if (parent) {
                parent.loadingShow();
            }
        },
        success: function (result) {
            var html = BuildRefreshLogHtml(result);
            $("#refreshLogList").html(html);
        },
        complete: function () {
            if (parent) {
                parent.loadingHide();
            }
        }
    });
}
function BuildRefreshLogHtml(result)
{
    var html = "";
    var data = result.data;
    var totalSize = result.totalSize;

    $.each(data, function (i, item) {
        var houseRefreshCount = item.RefreshMode == 2 ? 1 : item.RefreshNum;
        var houseRefreshDetailText = "";
        if (item.RefreshMode == 2) {
            houseRefreshDetailText = "一键刷新不显示";
        }
        else {
            if (item.Status == 1)
                {
            var houselist = eval(item.Houses);
            
            var houseIds = "";
            var houseinfos = "";
            if (houselist != undefined && item.Houses != "") {
                for (var i = 0; i < houselist.length; i++) {
                    var itemInfo = houselist[i];
                    houseinfos += "<a target='_blank' href='" + itemInfo.hr + "'>" + itemInfo.id + ":" + itemInfo.cm + itemInfo.fx + itemInfo.ps + "</a><br/>";
                    if (i < houselist.length - 1) {
                        houseIds += itemInfo.id + ",";
                    }
                    else {
                        houseIds += itemInfo.id;
                    }
                }
                houseRefreshDetailText = '<a  onclick="seeAbout(\'' + escape(houseinfos) + '\',1)" loginname="' + item.SiteUserName + '" webbasicid="' + item.SiteId + '" housetype="0" class="link_view_houses" title="' + houseIds + '">' + houseIds + '</a>';
            }
            }
        }
        html+=   '<tr>' +
                 '<td>'+item.Id+'</td>' +
                 '<td>' + item.SiteName + '</td>' +
                 '<td>' + item.SiteUserName + '</td>' +
                 '<td>' + buildTypeMap[item.BuildType] + '</td>' +
                 '<td>' + houseRefreshCount + '</td>' +
                 '<td><span title="' + item.Msg + '"> '+item.Msg + '</span></td>' +
                 '<td>' + houseRefreshDetailText + '</td>' +
                 //'<td>计划' + item.PlanNo + '</td>' +
                 '<td>'+item.DateTime+'</td>' +
                 '</tr>';
    });
    if (totalSize > 0) {
        var pageSize =ps;
        var pagecount = parseInt(totalSize / pageSize) + ((totalSize % pageSize) == 0 ? 0 : 1);
        $("#saleManager-fanye").paginate({
            count: pagecount,
            start: pi,
            display: 10,
            border: false,
            text_color: '#50b63f',
            text_hover_color: '#fff',
            background_color: '#fff',
            background_hover_color: '#50b63f',
            images: false,
            mouse: 'click',
            onChange: function (currentPage) {
                InitRefreshLog(currentPage);
            }
        });
    }
    else {
        $("#saleManager-fanye").html("");
    }
    return html;
}
/*查看房源*/
function seeAbout(content, flag) {
    content = unescape(content);
    content = content.replace("[", "");
    content = content.replace("]", "");
    var str;
    if (flag == 1) {
        str = content.split(", 【");
    } else {
        str = content.split(", ");
    }

    var html = '';
    for (var i = 0; i < str.length; i++) {
        var title = "<span>";
        if (flag == 1) {
            if (i > 0) {
                title = "【" + str[i];
            } else {
                title = str[i];
            }
        } else {
            title = "【" + str[i] + "】";
        }
        html += title + "</span><br>";
    }

    art.dialog({
        title: '查看',
        content: html,
        width: 480,
        height: 60,
        ok: true
    });
}

function InitRefreshSite()
{
    //GetEnableRefreshSite
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Refresh/GetEnableRefreshSite',
        success: function (data) {
            if (data.length > 0)
            {
                var html = "";
                html += ' <option value="0">不限</option>';
                $.each(data, function (i, item) {
                    html += ' <option value="' + item.SiteID + '">' + item.SiteName + '</option>';
                });
                $("#selectSiteId").html(html);
            }
        }
    });
} 

//刷新设置弹窗	
$("#plan_select_function .set_plan_function").die().live("click",function(){
	var webBasicId = $(this).attr("pid");
	if(webBasicId == 0){
		art.dialog({
			lock: true,
			content: '本网站支持自动刷新，房产盒子为你解决设置的烦恼！根据用户等级，采取最优的时间策略，自动一键刷新房源。',
			width:300,
			height:100
		});
		return;
	}
	refreshSetPopUp(webBasicId);
});

function refreshSetPopUp(webBasicId) {
	setDialog = art.dialog.load('/Refresh/Refresh_SetView?SiteId='+webBasicId, {
		title: '刷新设置',
		width:900,
		height:460,
		padding:0
	});
}

function getRefreshSetPlanList(SiteId)
{
    //1 出售 2 求购 3 出租 4求租 0 出售出租全部
    var tradeTypeMap = ["不限", "出售", "求购", "出租", "求租"];
    $.ajax({
        type: 'get',
        dataType: 'json',
        url: '/Refresh/GetRefreshSetList',
        data: { SiteId: SiteId },
        success: function (result) {
            var html = "";
            $.each(result.data, function (a, b) {
                var tradetype = tradeTypeMap[b.TradeType];
                if (tradetype == null) {
                    tradetype = "不限";
                }
                html += '<tr>' +
                        '            	<td><input class="saleManager-state-checkbox" type="checkbox" name="buildCheck" value="' + b.PlanId + '" id="bid' + b.PlanId + '" style="margin:0px"></td>' +
                        '                <td>' + (b.BeginHour < 10 ? '0' + b.BeginHour : b.BeginHour) + ':' + (b.BeginMinute < 10 ? '0' + b.BeginMinute : b.BeginMinute) + '</td>' +
                        '                <td>' + (b.EndHour < 10 ? '0' + b.EndHour : b.EndHour) + ':' + (b.EndMinute < 10 ? '0' + b.EndMinute : b.EndMinute) + '</td>' +
                        '                <td>' + tradetype + '</td>' +
                        '                <td>' + b.IntervalTime + '</td>' +
                        '                <td>' + b.CountPerTime + '</td>' +
                        '           </tr>';
            });
            $("#houseTable").html(html);
        }
    });
}
function AddRefreshSetPlan(SiteId)
{
    parame = { SiteId: SiteId, TradeType: $("#refresh_mode").val(), BeginHour: $("#start_house").val(), BeginMinute: $("#start_minute").val(), EndHour: $("#end_house").val(), EndMinute: $("#end_minute").val(), IntervalTime: $("#refresh_jiange").val(), CountPerTime: $("#refresh_num").val() };
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Refresh/RefreshSet',
        data: parame,
        success: function (result) {
            if (result.status > 0) {
                getRefreshSetPlanList(SiteId);
            }
            else {
                alert(result.msg);
            }
        }
    });
}

function RefreshPlanDelete(SiteId)
{
    var cb_list = $("input[name=buildCheck]");
    var plandIds = [];
    $.each(cb_list, function (a,b) {
        if ($(b).attr("checked") == "checked")
        {
            plandIds.push($(b).val());
        }
    });
    if (plandIds.length <= 0)
    {
        alert("请选择要删除的刷新计划");
        return false;
    }
    $.ajax({
        type: 'post',
        dataType: 'json',
        url: '/Refresh/RefreshSetDelete',
        data: { planIds: plandIds.toString() },
        success: function (result) {
            if (result.status > 0) {
                getRefreshSetPlanList(SiteId);
            }
            
        }
    });
}
function RefreshCheckAll()
{
    var cb_list = $("input[name=buildCheck]");
    var cbType = $("#saleManager-all").attr("checked") == "checked" ? true : false;
    $.each(cb_list, function (a, b) {
        $(b).attr("checked", cbType);
    });
}