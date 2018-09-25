
function FaceHandler(param) {
    this.curTargetObj = param.targetObj;//arguments[0];
    this.curInput = param.inputObj; // arguments[1];
    this.callBack = param.callBack;//回调函数
    this.curTypeId = 19;
    this.curFaceWord = "";
    this.faceDiv; //=jQuery(".mb-smiley-layer");
    this.Init();
}
FaceHandler.prototype = {
    Init: function () {
        this.CreaeteFaceDiv();
        var me = this;
        this.faceDiv = jQuery(".mb-smiley-layer");
        var faceListDiv = me.faceDiv.find(".layer-main .layer-bd");
        faceListDiv.find("ul li").die("click").live("click", function () {
            var objLi = jQuery(this);
            var fw = objLi.attr("fw");
            if (fw == undefined || fw == "") return;
            me.curFaceWord = '[' + fw + ']';
            me.SetFace();
        });
        this.ShowFace();
    },
    CreaeteFaceDiv: function () {
        var body = jQuery("body");
        if (body.find(".mb-smiley-layer").length > 0) return;
        var bulider = new StringBulider();
        bulider.append('<!--表情开始-->');
        bulider.append('<div style="z-index: 5050; left: 362px; top: 303.767px; display: none;" class="layer layer-shim br5 mb-smiley-layer">');
        bulider.append(' <div style="" class="layer-main">');
        bulider.append(' <span class="layer-arrow p1"></span>');
        bulider.append('<div class="layer-bd">');
        bulider.append('<ul>');
        bulider.append(' </ul>');
        bulider.append('</div>');
        bulider.append('  </div>');
        bulider.append('</div>');
        bulider.append('<!--表情结束-->');
        body.append(bulider.toString());
    },
    GetFaceByTypeId: function () {
        var me = this;
        var faceListDiv = me.faceDiv.find(".layer-main .layer-bd");
        if (faceListDiv.find("ul li").length > 0) return;
        var ran = Math.random() * 100000;
        var strHtml = "  <ul>";
        var url = "/Social/GetFaceList";
        jQuery.getJSON(url, { typeId: me.curTypeId, ran: ran },
                function (data) {
                    var htmlstr = "<ul>";
                    for (var i = 0; i < data.length; i++) {
                        var onjJsonItem = data[i];
                        strHtml += "<li fw=\"" + onjJsonItem.Faceword + "\">";
                        strHtml += "<a> <img src=\"" + onjJsonItem.Faceurl + "\"></a></li>";
                    }
                    strHtml += "</ul>";
                    faceListDiv.html(strHtml);
                   
                }); //
    },
    ShowFace: function () {
        var me = this;
        var offset = this.curTargetObj.offset();
        if (this.faceDiv.is(":visible")) {
            this.faceDiv.hide();
            return;
        }
        else {
            this.GetFaceByTypeId();
        }
        this.faceDiv.css({ "top": (offset.top + me.curTargetObj.height()) + "px", "left": (offset.left - 30) + "px" }).show();
    },
    SetFace: function () {
        var me = this;
        if (document.selection) {
            me.curInput.focus();
            sel = document.selection.createRange();
            sel.text = me.curFaceWord;
            sel.select();

        }
            //MOZILLA/NETSCAPE support
        else if (me.curInput[0].selectionStart || me.curInput[0].selectionStart == '0') {
            var startPos = me.curInput[0].selectionStart;
            var endPos = me.curInput[0].selectionEnd;
            myValue = me.curFaceWord;
            var inputVal = me.curInput.val();
            me.curInput.val(inputVal.substring(0, startPos) + myValue + inputVal.substring(endPos, inputVal.length));
            me.curInput[0].selectionStart = startPos + myValue.length;
            me.curInput[0].selectionEnd = startPos + myValue.length;
        } else {
            me.curInput[0].value += myValue;
        }
        me.curInput.focus();
        me.faceDiv.hide();
        me.curFaceWord = "";
        if (me.callBack) {
            me.callBack();
        }
    }
}
