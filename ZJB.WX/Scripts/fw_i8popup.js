//define(function (require, exports) {
    //var boxLayer = $('<div class="i8att-shadow"></div><div class="i8att-container"><div class="i8att-box"></div><div class="i8att-closed"></div><div class="i8att-overLay"></div><div class="i8att-controlPanel"><a class="ctlpanel-rotate-left"></a><a class="ctlpanel-rotate-right"></a><a class="ctlpanel-zoomout"></a><a class="ctlpanel-zoomin"></a></div><a class="vieworiginalfile" target="_blank">查看源图</a></div>');
    function popupDialog(config) {
        this.boxLayer = $('<div class="i8att-shadow"></div><div class="i8att-container"><div class="i8att-box"></div><div class="i8att-closed"></div><div class="i8att-overLay"></div><div class="i8att-controlPanel"><a class="ctlpanel-rotate-left"></a><a class="ctlpanel-rotate-right"></a><a class="ctlpanel-zoomout"></a><a class="ctlpanel-zoomin"></a></div><a class="vieworiginalfile" target="_blank">查看原图</a></div>');
        this.containerID = config.containerID || "";
        this.content = config.content || "";
        this.position = "center";
        this.width = config.width || 680;
        this.height = config.height || 510;
        this.opened = config.opened || function () { };
        this.closed = config.closed || function () { };
        this.overlay = config.overlay || false;
        this.imgctrl = config.imgctrl || false;
    }
    popupDialog.prototype = {
        initBox: function () {
            var _this = this;
            this.boxLayer.find(".i8att-closed").click(function () {
                _this.close();
            });
            this.boxLayer.find(".i8att-box").append(this.content);
            $("body").append(this.boxLayer);
            this.boxLayer.show();
            if (this.imgctrl) {
                this.boxLayer.find(".i8att-controlPanel").show();
            }
            if (this.overlay) {
                this.boxLayer.find(".i8att-overLay").show();
            }
            var boxleft = ($(window).width() - this.width) / 2;
            var boxtop = ($(window).height() - this.height) / 2;
            $(this.boxLayer[1]).css({ 'width': this.width, 'height': this.height, 'top': boxtop, 'left': boxleft });
        },
        close: function () {
            var _this = this;
            this.boxLayer.toggle(100, function () { $(this).remove(); _this.closed(); });
            $("body").removeClass("bigPic_over");
        },
        container: function () {
            return this.boxLayer.find(".i8att-box");
        },
        boxOverlay: function () {
            return this.boxLayer.find(".i8att-overLay");
        },
        ctrlPanel: function () {
            return this.boxLayer.find(".i8att-controlPanel");
        }
    };
//    return popupDialog;
//})