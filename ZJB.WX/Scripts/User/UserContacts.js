var contacts;
var lettr;
jQuery(document).ready(function () {
   var  contacts = new UserContactsHandler();
   contacts.GetList();
   jQuery(window).bind("scroll", function () {
       var totalheight = parseFloat(jQuery(window).height()) + parseFloat(jQuery(window).scrollTop()) + 80;
       if (jQuery(document).height() <= totalheight) {
           var ynLoad = contacts.GetScrollState();
           //加载数据
           if (ynLoad == 0) { //0表示可以加载 1表示不加载
               contacts.SetScrollState(1);
               switch (parseInt(contacts.queryType)) {
                   case 1:
                       contacts.GetList();
                       break;
                   case 2:
                       contacts.Search();
                       break;
                   case 3:
                       contacts.LettrSearch();
                       break;
               }
           }
       }
   });
});
//function OnclickPage(pageIndex) {
//    contacts.pageIndex = pageIndex;
//    switch (parseInt(contacts.queryType)) {
//        case 1:
//            contacts.GetList();
//            break;
//        case 2:
//            contacts.Search();
//            break;
//        case 3:
//            contacts.LettrSearch();
//            break;
//    }
//}
function UserContactsHandler() {
    this.defaultIcon = "/images/txDefault.png";
    this.lastTime =0;
    this.lastId = 0;
    this.ptcyDiv = jQuery(".tsq_txl_ptcy");
    this.ptcyDivList = this.ptcyDiv.find(".ab-info-list tbody");
    this.tabs = jQuery(".mb-home-tab ul li");
    this.glyDiv = jQuery(".tsq_txl_gly");
    this.glyDivList = this.glyDiv.find(".manager-items tbody");
    this.url = "/User/GetUserContacts";
    this.searchInput = jQuery(".ab-search-input");
    this.searchBtn = jQuery(".ab-search-submit");
    this.zmListLi = jQuery(".ab-filter-letters li a");
    this.pageIndex = 1;
    this.pageSize = 20;
    this.queryType = 1;
    this.scrollState = jQuery("#hdState");
    //  this.cyPage = new showPages('cyPage', this.pageSize);
    this.Init();
}

UserContactsHandler.prototype = {
    Init: function () {
        var me = this;
        this.searchBtn.click(function () {
            me.ptcyDivList.html("");
            me.SetScrollState(1);
            me.InitParams(2);
            me.Search();
        });
        this.zmListLi.click(function () {
            me.ptcyDivList.html("");
            me.InitParams(3);
            me.SetScrollState(1);
            me.LettrSearch(this);
        });
        this.tabs.click(function () {
            var curObj = jQuery(this);
            var dataType = curObj.attr("data-type");
            jQuery(".bd-content").hide();
            if (dataType == "gl") {
                me.InitParams(4);
                me.glyDiv.show();
                me.glyDivList.html("");
                me.GetManagers();
            }
            else if (dataType == "pt") {
                me.ptcyDiv.show();
                me.ptcyDivList.html("");
                me.InitParams(1);
                me.GetList();
            }
        });
        this.searchInput.keyup(function () {
            clearTimeout(me.timerMemberSearch);
            me.timerMemberSearch = setTimeout(function () { me.ptcyDivList.html(""); me.SetScrollState(1); me.InitParams(2); me.Search(); }, 500)
        });
    },
    GetScrollState: function () {
        return this.scrollState.val();
    },
    SetScrollState: function (v) {
        this.scrollState.val(v);
    },
    InitParams: function (type) {
        this.lastTime = 0;
        this.lastId = 0;
        this.pageIndex = 1;
        this.queryType = type;
    },
    LettrSearch: function (obj) {
        var curObj = jQuery(obj);
        var me = this;
        this.zmListLi.removeClass("curr");
        if (typeof (obj) != "undefined")
            lettr = curObj;
        
        lettr.addClass("curr");
   
        var letter = jQuery.trim(lettr.attr("zm"));
        if (letter == "all") {
            me.InitParams(1);
            this.GetList();
            return;
        }
        var ran = Math.random() * 1000000;
     
        jQuery.get(this.url, {
            type: "zmlist",
            lastTime: this.lastTime,
            lastId: this.lastId,
            pageSize: this.pageSize,
            pageIndex: this.pageIndex,
            letter: letter,
            ran: ran
        }, function (msg) {
            var obj = eval(msg);
            var str = me.FormateContacts(obj.data);
            me.tabs.find(".tsq_ptcy_nums span").html("成员(" + obj.totalCount + ")");
            me.ptcyDivList.append(str);
            var state = 0;
            if (msg.length < me.pageSize) {
                state = 1;
            }
            me.SetScrollState(state);
            me.pageIndex++;
        });
    },
    Search: function () {
        var ran = Math.random() * 1000000;
        var me = this;
        var keyword = this.searchInput.val();
        //if (jQuery.trim(keyword) == "") return;
        keyword = keyword.substring(0, 50);
        jQuery.get(this.url, {
            type: "search",
            lastTime: this.lastTime,
            lastId: this.lastId,
            pageSize: this.pageSize,
            pageIndex: this.pageIndex,
            keyword: this.searchInput.val(),
            ran: ran
        }, function (msg) {
            var obj = eval( msg );
            var str = me.FormateContacts(obj.data);
            me.tabs.find(".tsq_ptcy_nums span").html("成员(" + obj.totalCount + ")");
            me.ptcyDivList.append(str);
            var state = 0;
            if (msg.length < me.pageSize) {
                state = 1;
            }
            me.SetScrollState(state);
            me.pageIndex++;
        });
    },
    GetList: function () {
        var ran = Math.random() * 1000000;
        var me = this;
     
        jQuery.get(this.url, {
            type: "list",
            lastTime: 0,
            lastId: 0,
            pageSize: this.pageSize,
            pageIndex: this.pageIndex,
            ran: ran
        }, function (msg) {
         
            var obj = eval( msg);
            var str = me.FormateContacts(obj.data);
            me.tabs.find(".tsq_ptcy_nums span").html("成员(" + obj.totalCount + ")");
            me.ptcyDivList.append(str);
            var state = 0;
            if (msg.length < me.pageSize) {
                state = 1;
            }
            me.SetScrollState(state);
            me.pageIndex++;
            //me.cyPage.createHtml(me.pageIndex, obj.totalCount);
        });
    },
    FormateContacts: function (dataArr) {
        var bulider = new StringBulider();
        var me = this;
        jQuery.each(dataArr, function (i, n) {

            var pic = TsqFuncTool.NullStr(n.Portrait) == "" ? me.defaultIcon : n.Portrait + "?imageMogr2/strip|imageView2/1/w/50/h/50/q/85";
            bulider.append('<tr class="addressbook-ctrl  kcmp">');
            bulider.append('<td class="ab-userinfo-box"><a class="ab-info-avatar namecard" href="javascript:Global.LinkUserMain(', n.UserID, ')"> <img src="', pic, '" class="avatar"> </a>');
            bulider.append('    <div class="ab-userinfo ellipsis">');
            bulider.append('    <p class="ab-name ellipsis ab-name-alias"><a class="" href="javascript:Global.LinkUserMain(', n.UserID, ')">', n.EnrolnName, '</a></p>');
            bulider.append('    </div>');
            //bulider.append('    <div class="ad-list-operate"><a class="btn light ab-fwd"><em>转发名片</em></a></div>');
            bulider.append('</td>');
            bulider.append('<td><p title="', n.StoreName, '" class="ab-position ellipsis ">', n.StoreName, '</p></td>');
            bulider.append('<td><p title="&nbsp;/&nbsp;" class="ab-position ellipsis ">', n.Tel, '</p></td>');
            // bulider.append('<td><div class="ab-tel ellipsis"> </div></td>');
         
            bulider.append('<td><div class="ab-email ellipsis">');
            bulider.append('    <p title="', n.Email, '" class="ellipsis"><a class="" href="javascript:">', n.Email, '</a></p>');
            bulider.append('<td><div class="ab-mobile ellipsis"> </div>', n.Points, '</td>');
            bulider.append('    </div>');
            //            if (TsqFuncTool.NullStr(n.Email) != "") {mailto:', n.Email, '
//                bulider.append('    <div class="ad-list-operate"><a href="mailto:', n.Email, '" class="btn light"><em>发邮件</em></a></div>');
//            }
            bulider.append('</td>')
            bulider.append(' </tr>');
        });
        return bulider.toString();
    },
    GetManagers: function () {
        var me = this;
        var ran = Math.random() * 100000;
        jQuery.getJSON(this.url, { type: "managers", ran: ran }, function (msg) {
            var bulider = new StringBulider();
            jQuery.each(msg, function (i, n) {
                var pic = TsqFuncTool.NullStr(n.Portrait) == "" ? me.defaultIcon : n.Portrait + "?imageMogr2/strip|imageView2/1/w/50/h/50/q/85";
                bulider.append('<tr>');
                bulider.append('<td class="manager-detail"><a class="namecard" href="javascript:Global.LinkUserMain(', n.UserID, ')"> <img src="', pic, '" class="avatar"> </a>');
                bulider.append('<div title="', n.Name, '" class="manager-name ellipsis"> <a class="namecard" href="javascript:">', n.Name, '</a> </div>');
                bulider.append('<div title="', n.Dept, '&nbsp;&nbsp;', n.Post, '" class="manager-position ellipsis"> <span>', n.Dept, '</span><span class="pos">', n.Post, '</span> </div></td>');
                bulider.append('<td class="manager-contact"><a href="javascript:">', n.Email, '</a>&nbsp;&nbsp;<span>', n.Phone, '</span></td>');
                bulider.append('<td class="manager-ctrl"></td>');
                bulider.append('</tr>');
            });
            me.glyDivList.html(bulider.toString());
        });
    }
}