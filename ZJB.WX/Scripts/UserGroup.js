jQuery(document).ready(function () {
    var group = new CircleGroupHander();
    jQuery(window).bind("scroll", function () {
        var totalheight = parseFloat(jQuery(window).height()) + parseFloat(jQuery(window).scrollTop()) + 80;
        if (jQuery(document).height() <= totalheight) {
            var ynLoad = group.GetScrollState();
            //加载数据
            if (ynLoad == 0) { //0表示可以加载 1表示不加载
                group.SetScrollState(1);
                switch (parseInt(group.queryType)) {
                    case 1:
                        group.GetList();
                        break;
                    case 2:
                        group.LetterSearch();
                        break;
                }
            }
        }
    });
});
function CircleGroupHander() {
    this.Url = "/Group/GroupList";
    this.pageSize = 15;
    this.lastId = 0;
    this.groupDiv = jQuery(".tsq_allgroup_div");
    this.tbody = this.groupDiv.find(".grp-list-table tbody");
    this.searchInput = this.groupDiv.find(".group-search-key");
    this.searchBtn = this.groupDiv.find(".group-search-btn");
    this.timerSearch;
    this.zmListLi = this.groupDiv.find(".filter-letters li a");
    this.curZmEm; //选中字母查询
    this.myGroupDiv = jQuery(".tsq_mygroup_div");
    this.myTbody = this.myGroupDiv.find(".grp-list-table tbody");
    this.tircks = this.myGroupDiv.find(".group-tab-sub-trick span");
    this.scrollState = jQuery("#hdState");
    this.myJoinStatus = 1;
    this.queryType = 1;
    this.createGroupBtn = jQuery("a.create-group-btn");
    this.goGroupHomeBtn = jQuery(".tsq_addgroup_div .to-group-home");
    this.curUserId = jQuery("#hdUserId").val();
    this.defaultQzPic = "/Images/qz_pic.jpg";
    this.selGroupEm;
    this.handOverUserInput = jQuery(".hand-over-user");
    this.overBtn = jQuery(".hand-over-dialog .btn-big");
    this.searchMemberDiv = jQuery(".tsq_member_search");
    this.timerUserSearch;
    this.Init();
}

CircleGroupHander.prototype = {
    Init: function () {
        var me = this;
        this.searchInput.keyup(function () {
            me.queryType = 1;
            clearTimeout(me.timerSearch);
            me.InitValue();
            me.timerSearch = setTimeout(function () {
                me.GetList();
            }, 500);
        });
        this.searchBtn.click(function () {
            me.queryType = 1;
            me.InitValue();
            me.GetList();
        });
        this.zmListLi.click(function () {
            me.queryType = 2;
            me.InitValue();
            me.curZmEm = jQuery(this);
            me.LetterSearch();
        });
        this.tircks.live("click", function () {
            var obj = jQuery(this);
            var type = obj.attr("data-type");
            obj.addClass("curr").siblings().removeClass("curr");
            if (type == "jr") {//加入
                me.myJoinStatus = 1;
            }
            else if (type == "sp") {//审核中
                me.myJoinStatus = 2;
            }
            me.InitMyValue();
            me.GetMyList();
        });
        this.groupDiv.find(".tsq_group_tab ul li").click(function () {
            me.ShowTab(this);
        });
        this.myGroupDiv.find(".tsq_group_tab ul li").click(function () {
            me.ShowTab(this);
        });
        this.createGroupBtn.click(function () {
            jQuery(".bd-content").hide();
            jQuery(".tsq_addgroup_div").show();
        });
        this.groupSaveBtn = jQuery(".tsq_group_save");
        this.groupSaveBtn.click(function () {
            me.Save();
        });
        this.goGroupHomeBtn.click(function () {
            me.GoToIndex();
        });
        this.overBtn.click(function () {
            me.Transfer(this);
        });
        this.handOverUserInput.keyup(function () {
            clearTimeout(me.timerUserSearch);
            me.timerUserSearch = setTimeout(function () {
                me.Search();
                var v = jQuery.trim(me.handOverUserInput.val());
                if (v == "") me.handOverUserInput.attr("userId", 0);
            }, 500)
        });
        jQuery(".btn-close").on("click", function () {
            me.Close();
        });
        jQuery(".joined").live("click", function () {
            me.selGroupEm = jQuery(this);
            me.Quit();
        }); //
        jQuery(".manage").live("click", function () {
            me.selGroupEm = jQuery(this);
            var groupId = me.selGroupEm.attr("groupId");
            me.Manage(groupId);
        });
        //        me.myTbody.find(".tsq_circlegroup_dissolve").off("click").on("click", function () {
        //            me.selGroupEm = jQuery(this);
        //            var groupId = me.selGroupEm.attr("groupId");
        //            me.Dissolve(groupId);
        //        });
        jQuery(".tsq_transfer").live("click", function () {
            me.selGroupEm = jQuery(this);
            jQuery(".tsq_circle_mz").html(me.selGroupEm.attr("groupname"));
            me.ShowOver();
        });
        jQuery(".tsq_link_group").live("click", function () {
            me.LinkToGroup(this);
        });

        jQuery(".apply").live("click", function () {
            me.selGroupEm = jQuery(this);
            me.Apply();
        });
        this.GetList();
    },
    ShowTab: function () {
        var me = this;
        var obj = jQuery(arguments[0]);
        // jQuery(".bd-content").hide();
        var type = obj.attr("type");
        if (type == "my") {
            me.myJoinStatus = 1;
            me.InitMyValue();
            me.tircks.eq(0).click();
            //  me.GetMyList();
        }
        else {
            me.queryType = 1;
            me.InitValue();
            me.GetList();
        }
    },
    GoToIndex: function () {
        jQuery(".bd-content").hide();
        jQuery(".tsq_allgroup_div").show();
    },
    InitValue: function () {
        this.lastId = 0;
        this.myGroupDiv.hide();
        this.groupDiv.show();
        this.tbody.find(".gadmin").remove();
        this.SetScrollState(1);
    },
    InitMyValue: function () {
        //  var temp = arguments[0] == undefined ? 1 : arguments[0];
        //  this.myJoinStatus = temp;
        this.groupDiv.hide();
        this.myGroupDiv.show();
        this.myTbody.find(".gadmin").remove();
        this.SetScrollState(1);
    },
    GetScrollState: function () {
        return this.scrollState.val();
    },
    SetScrollState: function (v) {
        this.scrollState.val(v);
    },
    GetList: function () {
        var ran = Math.random() * 10000000;
        var me = this;
        var keyword = this.searchInput.val();
        jQuery.getJSON(this.Url, { SearchType: 0, LastId: this.lastId, KeyWord: keyword, PageSize: this.pageSize }, function (msg) {
            if (!msg) return;
            me.Formate(msg);
        });
    },
    LetterSearch: function () {
        var curObj = this.curZmEm;
        var me = this;
        this.zmListLi.parent().removeClass("curr");
        curObj.parent().addClass("curr");
        var letter = jQuery.trim(curObj.attr("zm"));
        if (letter == "all") {
            me.InitValue();
            this.GetList();
            return;
        }
        var ran = Math.random() * 10000000;
        var me = this;
        jQuery.getJSON(this.Url, { SearchType: 1, LastId: this.lastId, KeyWord: letter, PageSize: this.pageSize }, function (msg) {
            me.Formate(msg);
        })
    },
    GetMyList: function () {
        var ran = Math.random() * 10000000;
        var me = this;
        //var keyword = this.searchInput.val();
        jQuery.getJSON(this.Url, { SearchType: 2, status: this.myJoinStatus, pageSize: this.pageSize }, function (msg) {
            me.MyFormate(msg);
        });
    },
    Formate: function (msg) {
        var me = this;
        var length = msg.length;
        if (length > 0) {
            me.lastId = msg[length - 1].CircleId;
        }
        var bulider = new StringBulider();
        jQuery.each(msg, function (i, n) {
            bulider.append(me.FormateRecord(n).toString());
        });
        me.tbody.append(bulider.toString());
        var state = 0;
        if (length < me.pageSize) {
            state = 1;
        }
        me.SetScrollState(state);
    },
    FormateRecord: function (n) {
        var bulider = new StringBulider();
        var href = "/Content/Group/CircleGroupMain.aspx?circleId=" + n.Id;
        bulider.append('<tr class="grp-item gadmin" groupId="', n.Id, '">');
        var pic = TsqFuncTool.NullStr(n.Icon) == "" ? this.defaultQzPic : n.Icon;
        bulider.append('  <td class="gavatar"><a groupId="', n.Id, '" MemberStatus="', n.MemberStatus, '"  class="gimg tsq_link_group"><img src="', pic, '"></a></td>');
        bulider.append('  <td class="ginfo"><p class="gname"><a title="', n.GroupName, '" groupId="', n.Id, '" MemberStatus="', n.MemberStatus, '" class="ellipsis tsq_link_group">', n.GroupName, '</a></p>');
        var temp = n.Description;
        if (temp == null || temp == undefined || temp == "") temp = "欢迎来到" + n.GroupName + "小组";
        bulider.append('    <p class="gdesc">简介：', temp, ' </p></td>');
        bulider.append('  <td class="members"><a href="javascript:">' + n.MemberCount + '</a></td>');
        if (n.MemberType==1||n.MemberType==2) {
            bulider.append('  <td class="gstate "><a href="/Group/Detail?id=', n.Id, '" class="btn btn-blue manage" groupId="', n.Id, '"><em>管理群组</em></a></td>');
        }
        else if (n.MemberStatus == 0) {
            bulider.append('  <td class="gstate "><a class="btn btn-green apply" groupId="', n.Id, '" groupName="', n.GroupName, '" href="javascript:" ><i class="icon icon-add"></i><em>申请加入</em></a></td>');
        }
        else if (n.MemberStatus == 1 && n.MemberType != 1) {
            bulider.append('  <td class="gstate "><a href="javascript:" groupId="', n.Id, '" groupName="', n.GroupName, '" data-gstate="joined" class="btn btn-gray joined"><i class="icon icon-tick"></i><em>退出</em></a></td>');
        }
        else if (n.MemberStatus == 2) {
            bulider.append('  <td class="gstate "><a class="btn btn-yellow approval" href="javascript:"><i class="icon icon-time"></i><em>审批中</em></a></td>');
        }
        bulider.append(' </tr>');
        return bulider.toString();
    },
    MyFormate: function (msg) {
        var me = this;
        var bulider = new StringBulider();
        var href = "/Content/Group/CircleGroupMain.aspx?circleId=";
        jQuery.each(msg, function (i, n) {
            href = href + n.CircleId;
            bulider.append('<tr class="grp-item gadmin" groupId="', n.Id, '">'); //href="', href, '"
            var pic = TsqFuncTool.NullStr(n.Icon) == "" ? me.defaultQzPic : n.Icon;
            bulider.append('  <td class="gavatar"><a  groupId="', n.Id, '" MemberStatus="', n.MemberStatus, '" class="gimg tsq_link_group"><img src="', pic, '"></a></td>');
            bulider.append('  <td class="ginfo"><p class="gname"><a title="', n.GroupName, '" groupId="', n.Id, '" MemberStatus="', n.MemberStatus, '" class="ellipsis tsq_link_group" >', n.GroupName, '</a></p>');
            var temp = n.Description;
            if (temp == null || temp == undefined || temp == "") temp = "欢迎来到" + n.GroupName + "小组";
            bulider.append('    <p class="gdesc">简介：', temp, ' </p></td>');

            bulider.append('  <td class="members"><a href="javascript:">' + n.MemberCount + '</a></td>');
            if (n.MemberType==1||n.MemberType==2) {
                bulider.append('  <td class="gstate "><a href="/Group/Detail?id=', n.Id, '" class="btn btn-blue manage" groupId="', n.Id, '" ><em>管理群组</em></a></td>');
              //  if (n.UserId == me.curUserId) {
                  //  bulider.append('  <td class="gstate "><a href="javascript:" class="btn btn-blue  tsq_transfer" groupId="', n.CircleId, '" groupName="', n.CircleName, '" ><em>转让</em></a></td>');
              //  }
            }
            else if (n.MemberStatus == 1 && n.MemberType != 1) {
                bulider.append('  <td class="gstate "><a href="javascript:" groupId="', n.Id, '" groupName="', n.GroupName, '" data-gstate="joined" class="btn btn-gray joined"><i class="icon icon-tick"></i><em>退出</em></a></td>');
            }
            else if (n.MemberStatus == 2) {
                bulider.append('  <td class="gstate "><a href="javascript:" class="btn btn-blue manage"><em>审核中</em></a></td>');
            }
            bulider.append(' </tr>');
        });
        me.myTbody.append(bulider.toString());
    },
    LinkToGroup: function () {
        this.selGroupEm = jQuery(arguments[0]);
        var groupId = this.selGroupEm.attr("groupId");
        var isJoin = this.selGroupEm.attr("isJoin");
        if (isJoin == 1) {
            var href = "/Content/Group/CircleGroupMain.aspx?circleId=" + groupId;
            window.location.href = href;
        }
        else {
            jQuery.dialog.tips("未加入该群组", 1.5, "error.gif");
        }
    },
    Apply: function () {
        var me = this;
        var dialog = jQuery.dialog;
        if (!this.selGroupEm) {
            dialog.tips("请选择一个群组进行操作！", 1.5, "error.gif");
            return;
        }
        var groupName = this.selGroupEm.attr("groupName");
        var groupId = this.selGroupEm.attr("groupId");
        dialog.confirm("你确定加入“" + groupName + "”小组", function () {
            jQuery.post("/Group/Apply", { groupId: groupId }, function (result) {
                if (result.code>0) {
                    dialog.tips(result.msg, 1.5, "success.gif");
                    me.selGroupEm.removeClass("btn-green apply").addClass("btn-yellow approval").html("<i class=\"icon icon-time\"></i><em>审核中</em>");
                }
                else {
                    dialog.tips(result.msg, 1.5, "error.gif");
                }
            });
        });
    },
    Quit: function () {
        var me = this;
        var dialog = jQuery.dialog;
        if (!this.selGroupEm) {
            dialog.tips("请选择一个群组进行操作！", 1.5, "error.gif");
            return;
        }
        var groupName = this.selGroupEm.attr("groupName");
        var groupId = this.selGroupEm.attr("groupId");
        dialog.confirm("你确定退出“" + groupName + "”小组", function () {
            jQuery.post(me.Url, { type: "quit", groupId: groupId }, function (msg) {
                if (msg == -100) {
                    window.location.href = "/login.aspx";
                }
                var result = eval('(' + msg + ')');
                if (result.success) {
                    dialog.tips(result.msg, 1.5, "success.gif");
                    me.selGroupEm.removeClass("btn-gray joined").addClass("btn-green apply").html("<i class=\"icon icon-add\"></i><em>申请加入</em>");
                    if (me.myGroupDiv.is(":visible")) {
                        me.selGroupEm.parent().parent().remove();
                    }
                }
                else {
                    dialog.tips(result.msg, 1.5, "error.gif");
                }
            });
        });
    },
    Dissolve: function () {
        var me = this;
        var dialog = jQuery.dialog;
        if (!this.selGroupEm) {
            dialog.tips("请选择一个群组进行操作！", 1.5, "error.gif");
            return;
        }
        var groupName = this.selGroupEm.attr("groupName");
        var groupId = this.selGroupEm.attr("groupId");
        dialog.confirm("你确定解散 “" + groupName + "”小组", function () {
            jQuery.post(me.Url, { type: "dissolve", groupId: groupId }, function (msg) {
                if (msg == -100) {
                    window.location.href = "/login.aspx";
                }
                var result = eval('(' + msg + ')');
                if (result.success) {
                    dialog.tips(result.msg, 1.5, "success.gif");
                    if (me.myGroupDiv.is(":visible")) {
                        me.selGroupEm.parent().parent().remove();
                    }
                }
                else {
                    dialog.tips(result.msg, 1.5, "error.gif");
                }
            });
        });
    },
    Search: function () {
        var ran = Math.random() * 1000000;
        var me = this;
        var keyword = this.handOverUserInput.val();
        var offset = this.handOverUserInput.offset();
        var groupId = this.selGroupEm.attr("groupId");
        this.searchMemberDiv.css({ "top": (offset.top + me.handOverUserInput.height() + 10) + "px", "left": (offset.left) + "px" }).show();
        if (jQuery.trim(keyword) == "") {
            me.searchMemberDiv.hide().find("ul").html("");
            return;
        }
        keyword = keyword.substring(0, 50);
        jQuery.get("/AjaxAshx/CircleGroup/CircleGroupHandler.ashx", {
            type: "member",
            lastTime: 0,
            lastId: 0,
            pageSize: 20,
            pageIndex: 1,
            groupId: groupId,
            keyword: keyword,
            ran: ran
        }, function (msg) {
            var obj = eval('(' + msg + ')');
            var bulider = new StringBulider();
            jQuery.each(obj.data, function (i, n) {
                bulider.append('<li class="ac_even" userName="', n.NickName, '" userId="', n.UserId, '" Icon="', n.Icon, '" Phone="', n.Phone, '" Type="', n.Type, '" >');
                bulider.append('<div align="left">', n.NickName, '&lt;', n.Phone, '&gt;</div>');
                bulider.append('</li>');
            });
            var str = bulider.toString();
            if (obj.data.length > 0) {
                me.searchMemberDiv.show().find("ul").html(str);
            }
            else {
                me.searchMemberDiv.hide();
            }
            me.searchMemberDiv.find("ul li").off("click").on("click", function () {
                var obj = jQuery(this);
                me.handOverUserInput.val(obj.attr("username")).attr("userId", obj.attr("userid"));
                me.searchMemberDiv.hide();
            });
        });
    },
    Transfer: function () {
        var obj = jQuery(obj);
        jQuery(".layer-shim-overlay").hide();
        this.HideOverlay();
        var me = this;
        var userName = me.handOverUserInput.val();
        var userId = me.handOverUserInput.attr("userId");
        var dialog = jQuery.dialog;
        if (!userId || userId < 1) {
            dialog.tips("请选择一个要转让的成员", 1.5, "error.gif");
            return;
        }
        if (!this.selGroupEm) {
            dialog.tips("请选择一个群组进行操作！", 1.5, "error.gif");
            return;
        }
        var groupName = this.selGroupEm.attr("groupName");
        var groupId = this.selGroupEm.attr("groupId");
        dialog.confirm("你确定转让 “" + groupName + "”小组", function () {
            jQuery.post(me.Url, { type: "transfer", groupId: groupId, userId: userId }, function (msg) {
                if (msg == -100) {
                    window.location.href = "/login.aspx";
                }
                var result = eval('(' + msg + ')');
                if (result.success) {
                    dialog.tips(result.msg, 1.5, "success.gif");
                    me.selGroupEm.removeClass("btn-green tsq_transfer").addClass("btn-gray joined").html("<i class=\"icon icon-tick\"></i><em>退出</em>").parent().siblings(".gstate ").remove();
                }
                else {
                    dialog.tips(result.msg, 1.5, "error.gif");
                }
            });
        });
    },
    ShowOverlay: function () {
        var obj = jQuery(document);
        jQuery(".overlay").css({ "width": obj.width() + "px", "height": obj.height() + "px" }).show();
    },
    HideOverlay: function () {
        jQuery(".overlay").hide();
    },
    Close: function () {
        this.HideOverlay();
        jQuery(".layer-shim-overlay").hide();
        this.searchMemberDiv.hide();
    },
    ShowOver: function () {
        this.ShowOverlay();
        jQuery(".hand-over-dialog").show();
    },
    Manage: function (gid) {
        window.location.href = "/Content/Group/CircleGroupInfo.aspx?circleId=" + gid;
    },
    //创建群组
    Save: function () {
        var name = jQuery("#name").val();
        var length = TsqFuncTool.ByteLength(name);
        var dialog = jQuery.dialog;
        if (length > 40 || length < 1) {
           dialog.tips("只能输入1-20个汉字或1-40个字母", 1.5, "error.gif");
            return;
        }
        var isSecret = jQuery("input:radio[name=\"joinType\"][checked]").val();
        var me = this;
        dialog.confirm("确定创建群组“" + name + "”", function () {
            jQuery.post("/Group/GroupAdd", { GroupName: name, ShowType: isSecret }, function (result) {
                if (result.code>0) {
                    dialog.tips(result.msg, 1.5, "success.gif");
                    JQuery(".to-group-home").click();
                }
                else {
                    dialog.tips(result.msg, 1.5, "error.gif");
                }
            });
        });
    }
}
