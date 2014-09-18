(function (w, $) {
    w.UAS = {
        isBB: function () {
            var uagent = navigator.userAgent.toLowerCase();
            if (uagent.search('blackberry') > -1) {
                return true;
            }
            return false;
        },
        isTouchDevice: function () {
            var uagent = navigator.userAgent.toLowerCase();
            if ((uagent.search('iphone') > -1) || (uagent.search('ipod') > -1) || (uagent.search('android') > -1) || (uagent.search('ipad') > -1)) {
                return true;
            }
            return false;
        },
        isIE6: function () {
            if (navigator.userAgent.search(/MSIE 6.0/i) != -1) {
                return true;
            }
            return false;
        },
        isIE7: function () {
            var isIE7 = $.browser.msie && ($.browser.version == "7.0");
            return isIE7;
        },
        isIE: function () {
            var isIE = (navigator.userAgent.indexOf('MSIE') >= 0) && (navigator.userAgent.indexOf('Opera') < 0);
            return isIE;
        },
        isFF: function () {
            var isFF = (navigator.userAgent.indexOf('Firefox') >= 0);
            return isFF;
        },
        isOpera: function () {
            var isOpera = (navigator.userAgent.indexOf('Opera') >= 0);
            return isOpera;
        },
        isChrome: function () {
            var isChrome = ((navigator.userAgent.toString().toLowerCase().indexOf('chrome')) >= 0);
            return isChrome;
        }
    };

    w.CommonReg = {
        numberReg: /\d+$/,
        mobileReg: /^(((13[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}))+\d{8})$/,
        spaceReg: /^\s+$/
    };

    var elOverlay = $("<div id='overlay'></div>");
    elOverlay.width($(document).width()).height($(document).height());

    function showTimeoutDialog() {
        var _w = 250, _h = 50;
        elOverlay.empty().appendTo('body');
        var elDialog = $("<div id='overlay-dialog'>Oops... Sorry, you have lost connection to server. Please try <a href='#'>reload</a> this page.</div>").appendTo('body');
        var resize = function () {
            elOverlay.width($(document).width()).height($(document).height());
            elDialog.css({
                top: ($(w).height() - _h) / 2,
                left: ($(w).width() - _w) / 2,
                height: _h,
                width: _w
            });
        };
        $(w).resize(resize);
        resize();
        $('a', elDialog).click(function () {
            location.reload();
            return false;
        });
    };

    w.$rpc = function (path, data, success, failure, always) {
        if (path.charAt(0) == '/') path = path.substring(1);
        var it = setTimeout(function () {
            showTimeoutDialog();
            req.abort();
        }, 30000);
        console.log(baseUrl);
        var req = $.ajax({
            url: baseUrl + $.cookie('store') + '/' + path,
            data: data,
            type: "POST",
            dataType: "json",
            success: function (data) {
                clearTimeout(it);
                if (data.ack == "ok") {
                    if ($.isFunction(success)) success(data);
                } else {
                    console.log("Request error, message: " + data);
                    if ($.isFunction(failure)) failure(data.msg);
                }
                if ($.isFunction(always)) always();
            },
            error: function (request, status, ex) {
                clearTimeout(it);
                console.error("Request " + path + " failed with status: " + status + ", exception: " + ex);
            }
        });
    };

    w.trimSlash = function (orgUrl) {
        if (orgUrl.charAt(0) == '/')
            return orgUrl.substring(1);
        else
            return orgUrl;
    };

    //read the cookie: fromUser & store, and global plus a validate on it
    function handerGlobalHref() {
        var trims = "dy:", allhref = $('a'), c_store = $.cookie('store') ? $.cookie('store') : '' , allform = $('form');
        var hrefPlus = '/Mall/' + c_store + '/?';
        $(allhref).each(function () {
            if ($(this).attr('href').substring(0, 3).toLowerCase() == trims) {
                var redir = $(this).attr('href').substring(3);
                $(this).attr('href', hrefPlus + "redirect=" + redir);
            }
        });
        var formPlus = '/Mall/' + c_store + '/';
        $(allform).each(function() {
            if ($(this).attr('action').substring(0, 3).toLowerCase() == trims) {
                var realAction = $(this).attr('action').substring(3);
                $(this).attr('action', formPlus + trimSlash(realAction));
            }
        });
    };

    handerGlobalHref();
    //全局处理img的宽高,标准是295x295  大的就缩小至该像素，小的就自动放大
    //w.handerGlobalImg = function (elements, options) {
       // var me = this;
        //this.rule_height = options.rule_height || 295;
        //this.rule_width = options.rule_width || 295;

       // var viewport = {
          //  width: $(window).width(),
           // height: $(window).height()
        //};

       // $(elements).each(function () {
         //   var elparent = $(this).parents('li.hp');
        //   if (viewport.width >= 660) {
         //       $(this).width(me.rule_height).height(me.rule_width);
         //       elparent.css({ paddingTop: 10 });
          //  } else {
              //  var eldivWidth = elparent.width();
              //  var calWidth = eldivWidth * 9 / 10;
              //  var calHeight = calWidth;
              //  elparent.css({ paddingTop: eldivWidth/20});
              //  $(this).width(calWidth).height(calHeight);
          //  }
      //  });
   // };
   // handerGlobalImg($('img.lazyload'),{});
    //global event register and handler
    
    //取消横屏
   $(window).bind('resize', function(e) {
       e.preventDefault();
       e.stopPropagation();
   });

    w.Controller = function (options) {
        var events = this.events = options.events || {};
        var handlers = this.handlers = options.handlers || {};
        //var elements = this.elements = options.elements || {};
        var baseCtx = this.baseContext = options.baseContext || document;

        for (var ekey in events) {
            (function () {
                var ctxevent = ekey.trim(), handler = events[ekey];
                var eventtype = ctxevent.substring(ctxevent.lastIndexOf(' ') + 1);
                var ctx = ctxevent.substring(0, ctxevent.lastIndexOf(' '));
                $(ctx, baseCtx).bind(eventtype, function () {
                    handlers[handler].apply($(this), arguments);
                });
            })();
        }
    };
    //数值input输入数值限制,看是否需要加global alert，全局控制详情页，购物车内商品数量的控件
    w.formatNumberInput = function (elements, minL, maxL) {
        $(elements).each(function () {
            var ctx = $(this), orgval = ctx.val();
            //$(this).blur();
            ctx.bind('blur', function (e) {
                var curval = ctx.val();
                //console.log(curval.trim());
                if (CommonReg.numberReg.test(curval.trim())) {
                    if (parseInt(curval) > maxL) {
                        ctx.val(orgval);
                        new CustomizingUI.Popup({
                            text: "购买数量不能大于最大库存量!"
                        });
                        e.stopImmediatePropagation();
                    } else if (parseInt(curval) < minL) {
                        ctx.val(orgval);
                        new CustomizingUI.Popup({
                            text: "购买数量不能小于1!"
                        });
                        e.stopImmediatePropagation();
                    }
                } else {
                    ctx.val(orgval);
                    new CustomizingUI.Popup({
                        text: "请填入正确的商品数量值!"
                    });
                    e.stopImmediatePropagation();
                }
            });
        });
    };
    //其他输入类型, 用Reg来限制
    w.formatOtherInput = function () {

    };

    w.hideShareButton = function () {
        if (typeof w.WeixinJSBridge != 'undefined') {
            document.addEventListener('WeixinJSBridgeReady', function onBridgeReady() {
                w.WeixinJSBridge.call('hideOptionMenu');
            });
        }
    };

    /*****************************自定义手机控件系列****************************/
    w.CustomizingUI = {};
    //w.CustomizingUI.Overlay = $("<div id='ui-customizing-overlay'></div>");
    //w.CustomizingUI.Overlay.width(document.width).height(document.height);
    //console.log($(document).width() + " " + document.height);

    //Popup的前端控件
    /*text为显示pop的内容, 
      disappearCb为Pop消失的回调函数
      modifyWidth,modifyHeight为调整Pop的位置高度与宽度
      timeoutSec为自动消失的时间，如果striggleType是自动消失
      striggleType为Pop消失的类型，可以是timeoutTriggle和clickTriggle
      */
    w.CustomizingUI.Popup = function (options) {
        this.themeStyle = options.themeStyle || 'ui-customizing-shadow ui-customizing-popup ui-body-c';
        this.text = options.text || '';
        this.shadow = typeof options.shadow == "undefined" ? true : options.shadow;
        this.ctx = $('<div></div>');
        this.modifyWidth = options.modifyWidth || 250;
        this.modifyHeight = options.modifyHeight || 300;
        this.timeoutSec = options.timeoutSec || 2000;
        this.disappearCb = options.disappearCb || function() {};
        var striggleType = typeof options.striggleType == 'undefined' ? 'timeoutTriggle' : options.striggleType;

        this.shadow = $("<div id='ui-customizing-overlay'></div>").css({
            'background': 'transparent'
        }).width($(document).width()).height($(document).height()).empty();

        this.display();
        if (striggleType) {
            (this[striggleType])();
        }
    };

    w.CustomizingUI.Popup.prototype = {
        display: function () {
            var me = this;
            var pop = this.ctx.addClass(this.themeStyle);
            pop.css({
                top: ($(w).height() - this.modifyHeight) / 2,
                left: ($(w).width() - this.modifyWidth) / 2
            });
            $('<p></p>').text(this.text).appendTo(pop);
            me.shadow.appendTo($('body'));
            pop.appendTo($('body'));

            pop.bind('disappear', function () {
                pop.fadeOut();
                me.shadow.remove();
            });
            return this;
        },
        remove: function () {
            this.ctx.remove();
        },
        //triggle the disappear event when n's over
        timeoutTriggle: function () {
            var me = this;
            setTimeout(function () {
                me.ctx.trigger('disappear');
                me.shadow.remove();
                (me.disappearCb)();
            }, me.timeoutSec);
        },
        //triggle the disappear event when when body click
        clickTriggle: function () {
            var me = this;
            me.shadow.click(function () {
                me.ctx.trigger('disappear');
                me.shadow.remove();
                (me.disappearCb)();
            });
        }
    };

    /*Dialog是前端的对话框
    Buttons看代码的配置
    htmlContent 是Dialog里面的内容
    modifyWidth和modifyHeight是对相应对话框位置的调整
    title是对话框的主题
    */
    w.CustomizingUI.Dialog = function (options) {
        var me = this;
        me.themeStyle = options.themeStyle || 'ui-customizing-shadow ui-customizing-dialog ui-body-c';
        me.buttons = typeof options.buttons == "undefined" ? [{
            text: "确定",
            handler: function () {
                this.remove();
            }
        }, {
            text: "取消",
            handler: function () {
                this.remove();
            }
        }] : options.buttons;  // the default button
        var ctx = me.ctx = $('<div></div>').addClass(me.themeStyle);

        me.modifyWidth = options.modifyWidth || 250;
        me.modifyHeight = options.modifyHeight || 300;

        me.title = options.title || 'dialog';
        var htmlContent = me.htmlContent = typeof options.htmlContent == "undefined" ? '' : options.htmlContent;
        var header = me.header = $('<div class="dialog-header"></div>').appendTo(ctx);
        //me.closeIcon = $('<span class="close">X</span>').appendTo(header);//-------
        var content = me.content = $('<div class="dialog-content"></div>').appendTo(ctx);

        //$(htmlContent).appendTo(content);

        me.shadow = $("<div id='ui-customizing-overlay'></div>").css({
            'background': '#666'
        }).width($(document).width()).height($(document).height()).empty();
        me.display().closeClickTriggle();
    };

    w.CustomizingUI.Dialog.prototype = {
        display: function () {
            var me = this;
            me.header.append($('<h1></h1>').html(me.title));
            var btns = me.buttons;
            $(me.htmlContent).appendTo(me.content);
            $(btns).each(function (index) {
                var eachBtn = this;
                var tempBtn = $('<a href="javascript:;"><span class="ui-button">' + this.text + '</span></a>').appendTo(me.content);
                if (index == 0) {
                    $('span', tempBtn).addClass('active');
                }
                tempBtn.click(function () {
                    eachBtn.handler.apply(me, arguments);
                });
            });

            var dialog = this.ctx.css({
                top: ($(w).height() - this.modifyHeight) / 2,
                left: ($(w).width() - this.modifyWidth) / 2
            });

            me.shadow.appendTo($('body'));
            dialog.appendTo($('body'));
            dialog.bind('disappear', function () {
                me.remove();
            });
            return this;
        },
        remove: function () {
            var me = this;
            me.ctx.fadeOut(function () {
                me.shadow.fadeOut();
            });
        },
        closeClickTriggle: function () {
            var me = this;
            //me.closeIcon.click(function () {
            //    me.ctx.trigger('disappear');
            //});
        }
    };

    //地区控件,3级地址
    w.CustomizingUI.Loading = function(options) {
        var me = this;
        
    };

    w.CustomizingUI.Loading.prototype = {
        display: function() {
            
        },
        remove: function() {
            
        }
    };
    //详情页点击商品图片展示大图，占满手机屏幕大图的控件
    w.ImageOverlay = function(ele, options) {
        var me = this;
        console.log(me);
        me.element = ele;
        me.imgwidth = options.imgwidth || 400;
        me.imgheight = options.imgheight || 400;
        me.shadow = $("<div id='ui-no-opacity-overlay'></div>").css({
            'background': '#666',
            'text-align': 'center'
        }).width($(document).width()).height($(document).height()).empty();
        //me.closeIcon = $('<span class="close">X</span>');------------------ kael
        //me.closeIcon.bind('click', me.removeImg);
        me.imageCtx = $('<div class="image-overlay"></div>').width($(window).width()).height($(window).height());
        $('img', ele).each(function () {
            $(this).click(function () {
                var orgsrc = $(this).attr('src');
                me.shadow.appendTo($('body'));
                $(me.createImg(orgsrc.replace('295_295', '400_400'))).insertAfter(me.shadow);
            });
        });
    };

    w.ImageOverlay.prototype = {
        //create same scaling size img element and return
        createImg: function(src) {
            var devicewidth = $(window).width(), deviceheight = $(window).height();
            var ctx = this.imageCtx;
            //first 
            //var closeIcon = this.closeIcon;--------------kael
            var innerctx = $('<div style="position:relative;"></div>').appendTo(ctx);
            
            //closeIcon.bind('click', this.removeImg)---------kael
            //closeIcon.appendTo(innerctx);------------------kael
            ctx.bind('click', this.removeImg);//-----------------kael
            var elimg = $('<img />').attr('src', src);
            var picwidth = 0;
            if (deviceheight > devicewidth) {
                picwidth = devicewidth * 9 / 10;
            } else {
                picwidth = deviceheight * 9 / 10;
            }
            elimg.width(picwidth).height(picwidth);
            elimg.css('margin-top', (devicewidth < deviceheight ? devicewidth/20: deviceheight/20) + 'px');
            elimg.appendTo(innerctx);
            return ctx;
        },
        removeImg: function () {
            console.log('close click');
            $('#ui-no-opacity-overlay').remove();
            $('.image-overlay').empty().remove();
        }
    };
    /****************************UI End ****************************************/
    //Area Widget，an area ID div & .province  select makes it work
    //地址管理页面的控件，代码比较乱
    w.AreaWidget = function (options) {
        var me = this;
        me.element = options.element || $('#areawidget');
        me.areaInfoUrl = options.areaInfoUrl || 'BaseAjax/JsonAreaInfo';

        $('.province', me.element).change(function () {
            if($(this).val() != '0')
            me.jsonAreaInfo($(this).val(), me.operateSelect, 'city');
            
        });

        $('.city', me.element).change(function () {
            if ($(this).val() != '0')
            me.jsonAreaInfo($(this).val(), me.operateSelect, 'region');
        });
    };

    w.AreaWidget.prototype = {
        operateSelect: function (items, signclass) {
            var me = this, signctx = me.element.find($('.' + signclass)).empty();
            switch (signclass) {
                case 'city':
                    signctx.append($('<option></option>').val(0).text('请选择市'));
                    break;
                case 'region':
                    signctx.append($('<option></option>').val(0).text('请选择区'));
                    break;
                default:
                    break;
            }
            $(items).each(function () {
                var opitem = $('<option></option>');
                opitem.val(this.ID);
                opitem.text(this.Name);
                opitem.appendTo(signctx);
            });
            if (signclass == 'city') {
                $('.region', me.element).hide();
            }
            signctx.show();
        },
        jsonAreaInfo: function (areaID, cb, signclass) {
            var me = this;
            if (!areaID) {
                new CustomizingUI.Popup({
                    text: "请点选省市区"
                });
                return;
            }
            $rpc(this.areaInfoUrl, {
                areaID: areaID
            }, function (data) {
                console.log(data);
                if (typeof cb == "function")
                   cb.call(me, data.Items, signclass);
            }, function (msg) {
                new CustomizingUI.Popup({
                    text: "服务器异常或网络连接异常，请重试."
                });
            });
        }
    };
    

})(window, jQuery);