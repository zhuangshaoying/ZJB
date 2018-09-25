(function ($) {
    var memberLastId = 0;
    $.extend({
        EditGroup: function (option) {
            var groupName = option.GroupName.val();
            var description = option.Description.val();
            var showType = option.ShowType.val();
            var inviteType = option.InviteType.val();
            var length = this.ByteLength(groupName);
            if (length > 40 || length < 1) {
                alert("只能输入1-20个汉字或1-40个字母", 1.5, "error.gif");
                return;
            }
            $.ajax({
                type: 'post',
                url: '/Group/EditGroup',
                data: { Id: option.Id, GroupName: groupName, Description: description, ShowType: showType, InviteType: inviteType },
                success: function (result) {
                    alert(result.msg);
                }
            });
        },
        ByteLength: function (str) {
            var byteLen = 0, len = str.length;
            if (!str) return 0;
            for (var i = 0; i < len; i++)
                byteLen += str.charCodeAt(i) > 255 ? 2 : 1;
            return byteLen;
        },
        GetApproval: function (option) {
            var imageW50H50 = "?imageMogr2/strip|imageView2/1/w/50/h/50/q/85";
            var ran = Math.random() * 1000000;
            var approvalDiv = $(".tsq_approval_page .group-pending table tbody");
            jQuery.getJSON("/Group/MemberList", { GroupId: option.groupId,Status:option.status,LastId:0,PageSize:999, ran: ran }, function (msg) {
                var bulider = "";
                jQuery.each(msg, function (i, n) {
                    var pic = n.Portrait;
                    if (pic == "") pic = "/images/txDefault.png";
                    else {
                        pic = pic + imageW50H50;
                    }
                    bulider += '<tr class="pending">';
                    bulider += ' <td><a class="namecard" href="javascript:"><img src="' + pic + '" class="avatar"></a>';
                    bulider += ' <div class="name"><a class="namecard">' + n.UserName + '</a>&nbsp;</div>';
                    bulider += ' <div title="" class="position">&nbsp;</div></td>';
                    bulider += ' <td title="申请加入">申请加入</td>';
                    bulider += ' <td userId=' + n.UserId + ' userName="' + n.UserName + '">';
                    bulider += '<a  class="btn btn-blue em pass" onclick="$(this).Approval(' + option.groupId + ',1)"><em>通过</em></a>';
                    bulider += '<a class="btn btn-border reject" onclick="$(this).Approval(' + option.groupId + ',0)"><em>忽略</em></a></td>';
                    bulider += '</tr>';
                });
                bulider += '<tr class="last">';
                bulider += '<td colspan="3"><div class="paginator kcmp" style="display: block;">';
                bulider += '  <div></div>';
                bulider += '</div></td>';
                bulider += '</tr>';
                approvalDiv.html(bulider);
            });
        },
        GetMemberList: function (option) {
            var ran = Math.random() * 1000000;
            var imageW50H50 = "?imageMogr2/strip|imageView2/1/w/50/h/50/q/85";
            jQuery.get("/Group/MemberList", { GroupId: option.groupId, LastId: memberLastId, Status: 1, PageSize: option.pageSize, ran: ran },
             function (msg) {
             
                 //jQuery(".group-member-list-info groupCount").html(obj.totalCount);
                 var bulider = "";
                 var status = 0;
                 jQuery.each(msg, function (i, n) {
                     var pic = n.Portrait;
                     if (pic == "") pic = "/images/txDefault.png";
                     else {
                         pic = pic + imageW50H50;
                     }
                     bulider+='<div class="group-member"> <a class="namecard" href="javascript:"> <img src="'+ pic+'" class="avatar"> </a>';
                     bulider+='<div class="userinfo"> <a class="namecard" href="javascript:">'+ n.UserName+ '</a>';
                     if (n.MemberType > 0) {
                         bulider+='<span class="admin "><em></em><b>'+(n.MemberType == 1 ? "创建者" : "管理员")+ '</b></span>';
                     }
                     bulider+=' </div>';
                     bulider+='<div class="position">&nbsp;</div>';
                     //bulider.append('</div>');
                     status = n.MemberType > 0 ? 0 : 1;
                     if (n.MemberType == 0 || n.MemberType == 2) {
                         bulider+='<a class="ctrl" href="javascript:"></a>';
                         bulider+='<div userId="'+ n.UserId+ '" userName="'+ n.UserName+ '" status="'+ status+ '" class="dropmenu-ctrl" style="display: none;">'; // 
                         bulider+='<a class="ra-admin" href="javascript:">'+( status == 1 ? "升为管理员" : "取消管理员")+ '</a>';
                         n.MemberType == 0 ? bulider+='<a class="kickout" href="javascript:">请出该组</a></div>' : bulider+='</div>';
                     }
                     bulider+='</div>';
                 });
                 option.memberListDiv.append(bulider);
                 var state = "";
                 if (msg.length > 0) {
                     memberLastId = msg[length - 1].Id; // n.Id;
                 }
                 if (msg.legnth < option.pageSize) {
                     state = "1";
                 }
                 // me.BindMemberEvent();
                 // me.lybHandler.SetScrollState("1");
             });
        },
    });
    $.fn.extend({
            Approval: function (groupId, approvalState) {
                var dialog = jQuery.dialog;
                var td = this.parent();
                var userId = td.attr("userId");
                var userName = td.attr("userName");
                if (!userId) {
                    dialog.tips("请选择一个成员进行操作", 1.5, "error.gif");
                    return;
                }
                var opreate = "同意";
                if (approvalState == 0) opreate = "拒绝";
                dialog.confirm("你确定" + opreate + userName + "申请加入该群组",
                function () {
                    jQuery.post("/Group/CheckMember", { groupId: groupId, userId: userId, status: approvalState }, function (result) {
                        if (result.code == 1) {
                            dialog.tips(result.msg, 1.5, "success.gif");
                            td.parent().hide();
                        }
                        else {
                            dialog.tips(result.msg, 1.5, "error.gif");
                        }
                    });
                });
            }
    });
})(jQuery);