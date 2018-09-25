/*

 @Name: Fangsi WebIM 1.0.0
 @Author：锋哥@Fangsi.net
 @Date: 2015-01-01
 @Blog: http://www.fangsi.net
 
 */

; !function (win, undefined) {
    var config = {
        msgurl: 'Message',
        chatlogurl: '聊天记录url前缀',
        aniTime: 200,
        right: -232,
        api: {
            friend: 'friend.json', //好友列表接口
            group: 'group.json', //群组列表接口 
            chatlog: 'chatlog.json', //聊天记录接口
            groups: 'groups.json', //群组成员接口
            sendurl: '' //发送消息接口
        },
        user: { //当前用户信息
            name: username,
            dept: chatid,
            face: portrait
        },

        //自动回复内置文案，也可动态读取数据库配置
        autoReplay: [
            '您好，我现在有事不在，一会再和您联系。',
            '你没发错吧？',
            '洗澡中，请勿打扰，偷窥请购票，个体四十，团体八折，订票电话：一般人我不告诉他！',
            '你好，我是主人的美女秘书，有什么事就跟我说吧，等他回来我会转告他的。',
            '我正在拉磨，没法招呼您，因为我们家毛驴去动物保护协会把我告了，说我剥夺它休产假的权利。',
            '<（@￣︶￣@）>',
            '你要和我说话？你真的要和我说话？你确定自己想说吗？你一定非说不可吗？那你说吧，这是自动回复。',
            '主人正在开机自检，键盘鼠标看好机会出去凉快去了，我是他的电冰箱，我打字比较慢，你慢慢说，别急……',
            '(*^__^*) 嘻嘻，是胖子吗？'
        ],
        chating: {},
        hosts: (function () {
            var dk = location.href.match(/\:\d+/);
            dk = dk ? dk[0] : '';
            return 'http://www.zhujia001.com/';//'http://' + document.domain + dk + '/'
        })(),
        json: function (url, data, callback, error) {
            return $.ajax({
                type: 'POST',
                url: url,
                data: data,
                dataType: 'json',
                success: callback,
                error: error
            });
        },
        stopMP: function (e) {
            e ? e.stopPropagation() : e.cancelBubble = true;
        }
    },
    dom = [$(window), $(document), $('html'), $('body')],
    ChatCore = {},
    systemHub = $.connection.chatHub,
    onlinenum = 0;//在线人数

    //主界面tab
    ChatCore.tabs = function (index) {
        var node = ChatCore.node;
        node.tabs.eq(index).addClass('ChatCore_tabnow').siblings().removeClass('ChatCore_tabnow');
        node.list.eq(index).show().siblings('.ChatCore_list').hide();
        if (node.list.eq(index).find('li').length === 0) {
            ChatCore.getDates(index);
        }
    };

    //节点
    ChatCore.renode = function () {
        var node = ChatCore.node = {
            tabs: $('#ChatCore_tabs>span'),
            list: $('.ChatCore_list'),
            online: $('.ChatCore_online'),
            setonline: $('.ChatCore_setonline'),
            onlinetex: $('#ChatCore_onlinetex'),
            ChatCoreon: $('#ChatCore_on'),
            ChatCoreFooter: $('#ChatCore_bottom'),
            ChatCoreHide: $('#ChatCore_hide'),
            ChatCoreSearch: $('#ChatCore_searchkey'),
            searchMian: $('#ChatCore_searchmain'),
            closeSearch: $('#ChatCore_closesearch'),
            ChatCoreMin: $('#ChatCore_min')
        };
    };

    //主界面缩放
    ChatCore.expend = function () {
        var node = ChatCore.node;
        if (ChatCore.ChatCoreNode.attr('state') !== '1') {
            ChatCore.ChatCoreNode.stop().animate({ right: config.right }, config.aniTime, function () {
                node.ChatCoreon.addClass('ChatCore_off');
                try {
                    localStorage.ChatCoreState = 1;
                } catch (e) { }
                ChatCore.ChatCoreNode.attr({ state: 1 });
                node.ChatCoreFooter.addClass('ChatCore_expend').stop().animate({ marginLeft: config.right }, config.aniTime / 2);
                node.ChatCoreHide.addClass('ChatCore_show');
            });
        } else {
            ChatCore.ChatCoreNode.stop().animate({ right: 1 }, config.aniTime, function () {
                node.ChatCoreon.removeClass('ChatCore_off');
                try {
                    localStorage.ChatCoreState = 2;
                } catch (e) { }
                ChatCore.ChatCoreNode.removeAttr('state');
                node.ChatCoreFooter.removeClass('ChatCore_expend');
                node.ChatCoreHide.removeClass('ChatCore_show');
            });
            node.ChatCoreFooter.stop().animate({ marginLeft: 0 }, config.aniTime);
        }
    };

    //初始化窗口格局
    ChatCore.FangsiInit = function () {
        var node = ChatCore.node;

        //主界面
        try {
            /*
            if(!localStorage.ChatCoreState){       
                config.aniTime = 0;
                localStorage.ChatCoreState = 1;
            }
            */
            if (localStorage.ChatCoreState === '1') {
                ChatCore.ChatCoreNode.attr({ state: 1 }).css({ right: config.right });
                node.ChatCoreon.addClass('ChatCore_off');
                node.ChatCoreFooter.addClass('ChatCore_expend').css({ marginLeft: config.right });
                node.ChatCoreHide.addClass('ChatCore_show');
            }
        } catch (e) {
            //layer.msg(e.message, 5, -1);
        }
    };

    //聊天窗口
    ChatCore.popchat = function (param) {
        var node = ChatCore.node, log = {};

        log.success = function (layero) {

            ChatCore.chatbox = layero.find('#ChatCore_chatbox');
            log.chatlist = ChatCore.chatbox.find('.ChatCore_chatmore>ul');

            log.chatlist.html('<li data-id="' + param.id + '" type="' + param.type + '"  id="ChatCore_user' + param.type + param.id + '"><span>' + param.name + '</span><em>×</em></li>')
            ChatCore.tabchat(param, ChatCore.chatbox);

            ChatCore.transmit();
        };
        var reval = "";
        $.ajax({
            type: 'get',
            dataType: 'json',
            url: 'GetUserChatList',
            data: { pageSize: 20, pageIndex: 1, cid: parseInt(groupid) },
            success: function (data) {
                var irow = 0;
                if (data.data != undefined && data.data.length > 0) {
                    $.each(data.data, function (i, item) {
                        irow++;
                        reval += '      <li class="speech-time"> <span>' + item.AddTime + '</span> </li>'
                        if (item.UserID == parseInt(userid)) {
                            reval += '      <li class="right-say"> <a href="javascript:;" class="userImg" style="background-image:url(' + item.Portrait + ')"></a>'
                        }
                        else {
                            reval += '      <li class="left-say"> <a href="javascript:;" class="userImg" style="background-image:url(' + item.Portrait + ')"></a>'
                        }
                        reval += '        <div class="content">'
                        reval += '          <div class="speech" > ' + item.Content + ' </div>'
                        reval += '        </div>'
                        reval += '      </li>'
                    });
                }
                $('#ChatCore_area' + param.type + param.id).append(reval);
                $(".chat").scrollTop($(".chat")[0].scrollHeight);
            }
        });
        log.html = '<header>'
                + '  <div class="title red-title">'
                + '    <!--a class="back" href="javascript:history.go(-1);"></a--> '
                + '    ' + toname + ' <br>'
                + '    <span class="sub">' + deptname + '</span> <a class="home" href="list.html"></a> </div>'
                + '</header>'
                + '<section id="ChatCore_chatbox">'
                + '  <div class="chat">'
                + '    <div class="empty2"></div>'
                + '    <ul id="ChatCore_area' + param.type + param.id + '">'
                + reval
                + '    </ul>'
                + '    <div class="empty3"></div>'
                + '    <a name="end"></a> </div>'
                + '</section>'
                + '<footer>'
                + '  <div class="publish">'
                + '    <textarea id="ChatCore_write"></textarea>'
                + '    <div class="send ChatCore_send">'
                + '      <button class="ChatCore_sendbtn" id="ChatCore_sendbtn">发送</button>'
                + '    </div>'
                + '  </div>'
                + '</footer>'
        if (config.chatings < 1) {
            $("#xuboxPageHtml").html(log.html);
            log.success($("#xuboxPageHtml"));
        }

        //点击群员切换聊天窗
        //log.chatgroup.on('click', 'ul>li', function () {
        //    ChatCore.popchatbox($(this));
        //});
    };

    //定位到某个聊天队列
    ChatCore.tabchat = function (param) {
        var node = ChatCore.node, log = {}, keys = param.type + param.id;
        ChatCore.nowchat = param;

        ChatCore.chatbox.find('#ChatCore_user' + keys).addClass('ChatCore_chatnow').siblings().removeClass('ChatCore_chatnow');
        ChatCore.chatbox.find('#ChatCore_area' + keys).addClass('ChatCore_chatthis').siblings().removeClass('ChatCore_chatthis');
        ChatCore.chatbox.find('#ChatCore_group' + keys).addClass('ChatCore_groupthis').siblings().removeClass('ChatCore_groupthis');

        ChatCore.chatbox.find('.ChatCore_face>img').attr('src', param.face);
        ChatCore.chatbox.find('.ChatCore_face, .ChatCore_names').attr('href', param.href);
        ChatCore.chatbox.find('.ChatCore_names').text(param.name);

        ChatCore.chatbox.find('.ChatCore_seechatlog').attr('href', config.chatlogurl + param.id);

        log.groups = ChatCore.chatbox.find('.ChatCore_groups');
        if (param.type === 'group') {
            log.groups.show();
        } else {
            log.groups.hide();
        }

        $('#ChatCore_write').focus();

    };

    //弹出聊天窗
    ChatCore.popchatbox = function (othis) {
        
        var node = ChatCore.node, userId = othis.attr('id'), dataId = othis.attr('data-id'), param = {
            id: dataId, //用户ID
            userid: userId,
            type: othis.attr('type'),
            name: othis.attr('data-name'),  //用户名
            face: othis.attr('data-portrait'),  //用户头像
            href: 'http://www.zhujia001.com/'//config.hosts + 'user/' + dataId //用户主页
        }, key = param.type + dataId;
        if (!config.chating[key]) {
            ChatCore.popchat(param);
            config.chatings++;
        } else {
            ChatCore.tabchat(param);
        }
        config.chating[key] = param;
        ChatCore.nowchat = param;
        var chatbox = $('#ChatCore_chatbox');
        if (chatbox[0]) {
            node.ChatCoreMin.hide();
            chatbox.parents('.xubox_layer').show();
        }
    };
    ChatCore.getchatuser = function (othis) {

        var node = ChatCore.node, userId = othis.attr('id'), dataId = othis.attr('data-id'), param = {
            id: dataId, //用户ID
            userid: userId,
            type: othis.attr('type'),
            name: othis.attr('data-name'),  //用户名
            face: othis.attr('data-portrait'),  //用户头像
            href: 'http://www.zhujia001.com/'//config.hosts + 'user/' + dataId //用户主页
        }, key = param.type + dataId;
        config.chating[key] = param;
        ChatCore.nowchat = param;
        var chatbox = $('#ChatCore_chatbox');
        if (chatbox[0]) {
            node.ChatCoreMin.hide();
            chatbox.parents('.xubox_layer').show();
        }
    }

    //请求群员
    ChatCore.getGroups = function (param) {
        var keys = param.type + param.id, str = '',
        groupss = ChatCore.chatbox.find('#ChatCore_group' + keys);
        groupss.addClass('loading');
        config.json(config.api.groups, {}, function (datas) {
            if (datas.status === 1) {
                var ii = 0, lens = datas.data.length;
                if (lens > 0) {
                    for (; ii < lens; ii++) {
                        str += '<li data-id="' + datas.data[ii].id + '" type="one"><img src="' + datas.data[ii].face + '" class="ChatCore_oneface"><span class="ChatCore_onename">' + datas.data[ii].name + '</span></li>';
                    }
                } else {
                    str = '<li class="ChatCore_errors">没有群员</li>';
                }

            } else {
                str = '<li class="ChatCore_errors">' + datas.msg + '</li>';
            }
            groupss.removeClass('loading');
            groupss.html(str);
        }, function () {
            groupss.removeClass('loading');
            groupss.html('<li class="ChatCore_errors">请求异常</li>');
        });
    };

    //消息传输
    ChatCore.transmit = function () {
        var node = ChatCore.node, log = {};
        node.sendbtn = $('#ChatCore_sendbtn');
        node.imwrite = $('#ChatCore_write');

        //发送
        log.send = function () {
            var data = {
                content: node.imwrite.val(),
                id: ChatCore.nowchat.id,
                sign_key: '', //密匙
                _: +new Date
            };

            if (data.content.replace(/\s/g, '') === '') {
                layer.tips('说点啥呗！', '#ChatCore_write', 2);
                node.imwrite.focus();
            } else {
                //此处皆为模拟
                var keys = ChatCore.nowchat.type;//+ ChatCore.nowchat.id

                //聊天模版
                log.html = function (param, type) {
                    return '<li class="speech-time"> <span>' + param.time + '</span> </li>'
                    + '<li class="right-say">'
                    + '  <a href="javascript:;" class="userImg" style="background-image:url(' + param.face + ')"></a>'
                    + '  <div class="content">'
                    + '    <div class="speech" > ' + param.content + ' </div>'
                    + '  </div>'
                    + '</li>'
                    //return '<li class="' + (type === 'me' ? 'ChatCore_chateme' : '') + '">'
                    //    + '<div class="ChatCore_chatuser">'
                    //        + function () {
                    //            if (type === 'me') {
                    //                return '<span class="ChatCore_chattime">' + param.time + '</span>'
                    //                       + '<span class="ChatCore_chatname">' + param.name + '</span>'
                    //                       + '<img src="' + param.face + '" >';
                    //            } else {
                    //                return '<img src="' + param.face + '" >'
                    //                       + '<span class="ChatCore_chatname">' + param.name + '</span>'
                    //                       + '<span class="ChatCore_chattime">' + param.time + '</span>';
                    //            }
                    //        }()
                    //    + '</div>'
                    //    + '<div class="ChatCore_chatsay">' + param.content + '<em class="ChatCore_zero"></em></div>'
                    //+ '</li>';
                };

                log.imarea = ChatCore.chatbox.find('#ChatCore_area' + keys);
                //$("#asklistBlock").append(innerHtml);
                log.imarea.append(log.html({
                    time: new Date().toLocaleString(),
                    name: config.user.name,
                    dept: config.user.dept,
                    face: config.user.face,
                    content: data.content
                }, 'me'));
                node.imwrite.val('').focus();
                //log.imarea.scrollTop(log.imarea[0].scrollHeight);

                $(".chat").scrollTop($(".chat")[0].scrollHeight);
                //setTimeout(function () {
                //    log.imarea.append(log.html({
                //        time: '2014-04-26 0:38',
                //        name: ChatCore.nowchat.name,
                //        face: ChatCore.nowchat.face,
                //        content: config.autoReplay[(Math.random() * config.autoReplay.length) | 0]
                //    }));
                //    log.imarea.scrollTop(log.imarea[0].scrollHeight);
                //}, 500);

                /*
                that.json(config.api.sendurl, data, function(datas){
                
                });
                */

                systemHub.server.sendPrivateMessage(ChatCore.nowchat.id, data.content, parseInt(ChatCore.nowchat.userid), chatid, deptname, groupid);
            }

        };
        node.sendbtn.on('click', log.send);

        node.imwrite.keyup(function (e) {
            if (e.keyCode === 13) {
                log.send();
            }
        });
    };

    //事件
    ChatCore.event = function () {
        var node = ChatCore.node;

        //主界面tab
        node.tabs.eq(0).addClass('ChatCore_tabnow');
        node.tabs.on('click', function () {
            var othis = $(this), index = othis.index();
            ChatCore.tabs(index);
        });

        //列表展收
        node.list.on('click', 'h5', function () {
            var othis = $(this), chat = othis.siblings('.ChatCore_chatlist'), parentss = othis.parent();
            if (parentss.hasClass('ChatCore_liston')) {
                chat.hide();
                parentss.removeClass('ChatCore_liston');
            } else {
                chat.show();
                parentss.addClass('ChatCore_liston');
            }
        });

        //设置在线隐身
        node.online.on('click', function (e) {
            config.stopMP(e);
            node.setonline.show();
        });
        node.setonline.find('span').on('click', function (e) {
            var index = $(this).index();
            config.stopMP(e);
            if (index === 0) {
                node.onlinetex.html('在线');
                node.online.removeClass('ChatCore_offline');
            } else if (index === 1) {
                node.onlinetex.html('隐身');
                node.online.addClass('ChatCore_offline');
            }
            node.setonline.hide();
        });

        node.ChatCoreon.on('click', ChatCore.expend);
        node.ChatCoreHide.on('click', ChatCore.expend);

        //搜索
        node.ChatCoreSearch.keyup(function () {
            var val = $(this).val().replace(/\s/g, '');
            if (val !== '') {
                node.searchMian.show();
                node.closeSearch.show();
                //此处的搜索ajax参考ChatCore.getDates
                node.list.eq(3).html('<li class="ChatCore_errormsg">没有符合条件的结果</li>');
            } else {
                node.searchMian.hide();
                node.closeSearch.hide();
            }
        });
        node.closeSearch.on('click', function () {
            $(this).hide();
            node.searchMian.hide();
            node.ChatCoreSearch.val('').focus();
        });

        //弹出聊天窗
        config.chatings = 0;
        node.list.on('click', '.ChatCore_childnode', function () {
            var othis = $(this);
            ChatCore.popchatbox(othis);
        });

        //点击最小化栏
        node.ChatCoreMin.on('click', function () {
            $(this).hide();
            $('#ChatCore_chatbox').parents('.xubox_layer').show();
        });

        //document事件
        dom[1].on('click', function () {
            node.setonline.hide();
            $('#ChatCore_sendtype').hide();
        });

        //连接IM服务器成功
        systemHub.client.onConnected = function (id, userName, allUsers) {
            var node = ChatCore.node, myf = node.list.eq(0), str = '', i = 0;
            myf.addClass('loading');
            onlinenum = allUsers.length;
            var othis = $(".ChatCoreChild");
            if (onlinenum > 0) {
                str += '<li class="ChatCore_parentnode  ChatCore_liston">'
                     + '<h5><i></i><span class="ChatCore_parentname">在线家人</span><em class="ChatCore_nums">（' + onlinenum + '）</em></h5>'
                     + '<ul id="ChatCore_friend_list" class="ChatCore_chatlist">';
                for (; i < onlinenum; i++) {
                    str += '<li id="userid-' + allUsers[i].UserID + '" data-id="' + allUsers[i].ConnectionId + '" class="ChatCore_childnode" type="one"><img src="' + allUsers[i].Portrait + '"  class="ChatCore_oneface"><span  class="ChatCore_onename">' + allUsers[i].NickName + '</span><em class="ChatCore_time">' + allUsers[i].LoginTime + '</em></li>';
                    if (parseInt(othis.attr('id')) == allUsers[i].UserID && groupid == allUsers[i].CID) {
                        othis.attr('data-id', allUsers[i].ConnectionId)
                        othis.attr('data-name', allUsers[i].NickName);//用户名
                        othis.attr('data-portrait', allUsers[i].Portrait);  //用户头像
                        //var othis = $(".ChatCoreChild");
                        ChatCore.getchatuser(othis);
                    }
                }
                str += '</ul></li>';
                myf.html(str);

            } else {
                myf.html('<li class="ChatCore_errormsg">没有任何数据</li>');
            }
            myf.removeClass('loading');
        };
        //连接IM服务器成功
        systemHub.client.ontoConnected = function (connectionId, userName, portrait) {
            //var node = ChatCore.node, myf = node.list.eq(0), str = '', i = 0;
            //myf.addClass('loading');
            //onlinenum = allUsers.length;
            var othis = $(".ChatCoreChild");
            //if (allUsers != null && allUsers != undefined) {
            othis.attr('data-id', connectionId);
            othis.attr('data-name', userName);//用户名
            othis.attr('data-portrait', portrait);  //用户头像
            //var othis = $(".ChatCoreChild");
            ChatCore.getchatuser(othis);
            //} 
            //myf.removeClass('loading');
        };



        //新用户上线
        systemHub.client.onNewUserConnected = function (id, userID, userName, deptName, portrait, loginTime) {
            onlinenum = onlinenum + 1;
            $(".ChatCore_nums").html("（" + onlinenum + "）");
            var myf = $('#ChatCore_friend_list'), str = '';
            str += '<li id="userid-' + userID + '" data-id="' + id + '" class="ChatCore_childnode" type="one"><img src="' + portrait + '"  class="ChatCore_oneface"><span  class="ChatCore_onename">' + userName + '</span><em class="ChatCore_time">' + loginTime + '</em></li>';
            var othis = $(".ChatCoreChild");
            if (parseInt(othis.attr('id')) == parseInt(userID) && parseInt(chatid) == parseInt(deptName)) {
                othis.attr('data-id', id)
                othis.attr('data-name', userName);//用户名
                othis.attr('data-portrait', portrait);  //用户头像
                //var othis = $(".ChatCoreChild");
                ChatCore.getchatuser(othis);
            }
            myf.append(str);
        };

        //用户离线
        systemHub.client.onUserDisconnected = function (id, userName) {
            onlinenum = onlinenum - 1;
            $(".ChatCore_nums").html("（" + onlinenum + "）");
            $("#ChatCore_friend_list li[data-id=" + id + "]").remove();
        };

        //发送消息时，对方已不在线
        systemHub.client.absentSubscriber = function () {
            //layer.msg("对方已不在线,已发消息通知", 2, -1);
            //$.gritter.add({
            //    title: "系统提醒",
            //    text: "对方已不在线，请采用其它方式沟通！",
            //    class_name: "gritter-info gritter-center"
            //});
        };

        //接收消息
        systemHub.client.receivePrivateMessage = function (fromUserId, userName, message) {

            var node = ChatCore.node, log = {}, othis = $(".ChatCoreChild");//othis = $("#ChatCore_friend_list li[data-id=" + fromUserId + "]");
            //聊天模版
            log.html = function (param, type) {
                return '<li class="speech-time"> <span>' + param.time + '</span> </li>'
                   + '<li class="left-say">'
                   + '  <a href="javascript:;" class="userImg" style="background-image:url(' + param.face + ')"></a>'
                   + '  <div class="content">'
                   + '    <div class="speech" > ' + param.content + ' </div>'
                   + '  </div>'
                   + '</li>'
                //return '<li class="' + (type === 'me' ? 'ChatCore_chateme' : '') + '">'
                //    + '<div class="ChatCore_chatuser">'
                //        + function () {
                //            if (type === 'me') {
                //                return '<span class="ChatCore_chattime">' + param.time + '</span>'
                //                       + '<span class="ChatCore_chatname">' + param.name + '</span>'
                //                       + '<img src="' + param.face + '" >';
                //            } else {
                //                return '<img src="' + param.face + '" >'
                //                       + '<span class="ChatCore_chatname">' + param.name + '</span>'
                //                       + '<span class="ChatCore_chattime">' + param.time + '</span>';
                //            }
                //        }()
                //    + '</div>'
                //    + '<div class="ChatCore_chatsay">' + param.content + '<em class="ChatCore_zero"></em></div>'
                //+ '</li>';
            };
            //ChatCore.getchatuser(othis);

            var keys = ChatCore.nowchat.type;//+ ChatCore.nowchat.id;
            log.imarea = ChatCore.chatbox.find('#ChatCore_area' + keys);

            log.imarea.append(log.html({
                time: new Date().toLocaleString(),
                name: othis.attr('data-name'),//用户名
                face: othis.attr('data-portrait'),  //用户头像
                //name: othis.find('.ChatCore_onename').text(),
                //face: othis.find('.ChatCore_oneface').attr('src'),
                content: message,
                type: "you"
            }));
            //log.imarea.scrollTop(log.imarea[0].scrollHeight);
            $(".chat").scrollTop($(".chat")[0].scrollHeight);
        };
    };

    //请求列表数据
    ChatCore.getDates = function (index) {
        var api = [config.api.friend, config.api.group, config.api.chatlog],
            node = ChatCore.node, myf = node.list.eq(index);
        myf.addClass('loading');
        config.json(api[index], {}, function (datas) {
            if (datas.status === 1) {
                var i = 0, myflen = datas.data.length, str = '', item;
                if (myflen > 1) {
                    if (index !== 2) {
                        for (; i < myflen; i++) {
                            str += '<li data-id="' + datas.data[i].id + '" class="ChatCore_parentnode">'
                                + '<h5><i></i><span class="ChatCore_parentname">' + datas.data[i].name + '</span><em class="ChatCore_nums">（' + datas.data[i].nums + '）</em></h5>'
                                + '<ul class="ChatCore_chatlist">';
                            item = datas.data[i].item;
                            for (var j = 0; j < item.length; j++) {
                                str += '<li data-id="' + item[j].id + '" class="ChatCore_childnode" type="' + (index === 0 ? 'one' : 'group') + '"><img src="' + item[j].face + '" class="ChatCore_oneface"><span class="ChatCore_onename">' + item[j].name + '</span></li>';
                            }
                            str += '</ul></li>';
                        }
                    } else {
                        str += '<li class="ChatCore_liston">'
                            + '<ul class="ChatCore_chatlist">';
                        for (; i < myflen; i++) {
                            str += '<li data-id="' + datas.data[i].id + '" class="ChatCore_childnode" type="one"><img src="' + datas.data[i].face + '"  class="ChatCore_oneface"><span  class="ChatCore_onename">' + datas.data[i].name + '</span><em class="ChatCore_time">' + datas.data[i].time + '</em></li>';
                        }
                        str += '</ul></li>';
                    }
                    myf.html(str);
                } else {
                    myf.html('<li class="ChatCore_errormsg">没有任何数据</li>');
                }
                myf.removeClass('loading');
            } else {
                myf.html('<li class="ChatCore_errormsg">' + datas.msg + '</li>');
            }
        }, function () {
            myf.html('<li class="ChatCore_errormsg">请求失败</li>');
            myf.removeClass('loading');
        });
    };

    //渲染骨架
    ChatCore.view = (function () {
        //var ChatCoreNode = ChatCore.ChatCoreNode = $('');
        var ChatCoreNode = ChatCore.ChatCoreNode = $('<div id="ChatCoremm" style=" display: none;" class="ChatCore_main">'
                + '<div class="ChatCore_top" id="ChatCore_top">'
                + '  <div class="ChatCore_search"><i></i><input id="ChatCore_searchkey" /><span id="ChatCore_closesearch">×</span></div>'
                + '  <div class="ChatCore_tabs" id="ChatCore_tabs"><span class="ChatCore_tabfriend" title="好友"><i></i></span><span class="ChatCore_tabgroup" title="群组" style="display:none;"><i></i></span><span class="ChatCore_latechat"  title="最近聊天" style="display:none;"><i></i></span></div>'
                + '  <ul class="ChatCore_list" style="display:block"></ul>'
                + '  <ul class="ChatCore_list"></ul>'
                + '  <ul class="ChatCore_list"></ul>'
                + '  <ul class="ChatCore_list ChatCore_searchmain" id="ChatCore_searchmain"></ul>'
                + '</div>'
                + '<ul class="ChatCore_bottom" id="ChatCore_bottom">'
                + '<li class="ChatCore_online" id="ChatCore_online">'
                    + '<i class="ChatCore_nowstate"></i><span id="ChatCore_onlinetex">在线</span>'
                    + '<div class="ChatCore_setonline">'
                        + '<span><i></i>在线</span>'
                        + '<span class="ChatCore_setoffline"><i></i>隐身</span>'
                    + '</div>'
                + '</li>'
                + '<li class="ChatCore_mymsg" id="ChatCore_mymsg" title="聊天室"><i></i><a href="' + config.msgurl + '" target="rightFrame"></a></li>'
                + '<li class="ChatCore_seter" id="ChatCore_seter" title="设置">'
                    + '<i></i>'
                    + '<div class="">'

                    + '</div>'
                + '</li>'
                + '<li class="ChatCore_hide" id="ChatCore_hide"><i></i></li>'
                + '<li id="ChatCore_on" class="ChatCore_icon ChatCore_on"></li>'
                + '<div class="ChatCore_min" id="ChatCore_min"></div>'
            + '</ul>'
        + '</div>');
        dom[3].append(ChatCoreNode);
        ChatCore.renode();
        ChatCore.event();

        $.connection.hub.start().done(function () {
            systemHub.server.connect(userid, username, portrait, groupid);//chatid
            var othis = $(".ChatCoreChild");
            ChatCore.popchatbox(othis);
        });

        //ChatCore.getDates(0);


        ChatCore.FangsiInit();
    }());

    //$("#ChatCore_sendbtn").click(function () {
    //    ChatCore.transmit();
    //});
}(window);


