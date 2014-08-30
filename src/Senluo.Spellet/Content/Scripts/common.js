(function ($) {
    
    var template = '<div class="msg_tips" style="display: none;"> <div class="inner shadow"></div></div>';
    var message = function (content, timeout) {
        if (!content) return;
        var msgDiv = $(template).appendTo('body');
        msgDiv.show();
        $('div', msgDiv).html(content);
        msgDiv.removeClass().addClass('msg_tips success');
        if (!timeout) timeout = 3000;
        setTimeout(function () { msgDiv.fadeOut('normal', function () { msgDiv.remove(); }) }, timeout);
    };
    var error = function (content, timeout) {
        if (!content) return;
        var msgDiv = $(template).appendTo('body');
        msgDiv.show();
        $('div', msgDiv).html(content);
        msgDiv.removeClass().addClass('msg_tips error');
        if (!timeout) timeout = 4000;
        setTimeout(function () { msgDiv.fadeOut('normal', function () { msgDiv.remove(); }) }, timeout);
    };
    
    
    //获取一个表单的数据
    var getValues = function () {
        var me = this;
        var targetForm = $(me);
        if (!targetForm || !targetForm.length) return {};
        var formArray = targetForm.serializeArray();
        var retObj = {}, index;
        for (index = 0; index < formArray.length; index++) {
            var obj = formArray[index];
            if (!obj.name) continue;
            retObj[obj.name] = obj.value;
        }
        $(me).find('input[type=checkbox]:not(:checked)').each(function (i, tar) {
            if (tar.hasAttribute('uncheckedValue') && tar.name) {
                retObj[tar.name] = $(tar).attr('uncheckedValue');
            }
        });
        return retObj;
    };
    var mask = function (msg, maskDivClass) {
        this.unmask();
        // 参数
        var op = {
            opacity: 0.8,
            z: 10000,
            bgcolor: '#ccc'
        };
        var original = $(document.body);
        var position = { top: 0, left: 0 };
        if (this[0] && this[0] !== window.document) {
            original = this;
            position = original.position();
        }
        // 创建一个 Mask 层，追加到对象中
        var maskDiv = $('<div class="maskdivgen">&nbsp;</div>');
        maskDiv.appendTo(original);
        var maskWidth = original.outerWidth();
        if (!maskWidth) {
            maskWidth = original.width();
        }
        var maskHeight = original.outerHeight();
        if (!maskHeight) {
            maskHeight = original.height();
        }
        maskDiv.css({
            position: 'absolute',
            top: position.top,
            left: position.left,
            'z-index': op.z,
            width: maskWidth,
            height: maskHeight,
            'background-color': op.bgcolor,
            opacity: 0
        });
        if (maskDivClass) {
            maskDiv.addClass(maskDivClass);
        }
        if (msg) {
            var msgDiv = $('<div style="position:absolute;border:#6593cf 1px solid; padding:2px;background:#ccca"><div style="line-height:24px;border:#a3bad9 1px solid;background:white;padding:2px 10px 2px 10px">' + msg + '</div></div>');
            msgDiv.appendTo(maskDiv);
            var widthspace = (maskDiv.width() - msgDiv.width());
            var heightspace = (maskDiv.height() - msgDiv.height());
            msgDiv.css({
                cursor: 'wait',
                top: (heightspace / 2 - 2),
                left: (widthspace / 2 - 2)
            });
        }
        maskDiv.show('fast', function () {
            // 淡入淡出效果
            $(this).fadeTo('normal', op.opacity);
        })
        //what ever happens, mask will disappear when timeout
        setTimeout(function () { maskDiv.unmask(); }, 5000);
        return maskDiv;
    }
    var unmask = function () {
        var original = $(document.body);
        if (this[0] && this[0] !== window.document) {
            original = $(this[0]);
        }
        original.find("> div.maskdivgen").remove();
    }

    $.extend({
        //弹出一个层显示信息
        msg: message,
        //弹出一个层显示错误
        error: error
    });
    
    $.fn.extend({
        //将当前DOM模态
        mask: mask,
        //取消模态
        unmask: unmask,
        //获取一个表单的json数据
        getValues: getValues
    });

    $.delete = function(url, id, cb) {
        if (!url || !id) return;
        if (window.confirm('确定删除吗？')) {
            $.ajax({
                type: 'delete',
                data: { ID: id },
                url: url,
                contentType:'application/json',
                success:function(result) {
                    if (result.success) {
                        if (cb) {
                            cb();
                        }
                        $.msg('删除成功');
                    } else {
                        $.error(result.msg);
                    }
                }
            })
        }
    }
    $.update = function(form, cb) {
        var json = getValues();
        $.ajax({
            url: form.attr('action'),
            data: json,
            type: 'PUT',
            contentType: 'application/json',
            success:function(result) {
                if (result.success) {
                    if (cb && $.isFunction(cb)) {
                        cb();
                    }
                    $.msg('保存成功');
                } else {
                    $.error(result.msg);
                }
            }
        })
    }
    $.create = function (form, cb) {
        var json = getValues();
        $.ajax({
            url: form.attr('action'),
            data: json,
            type: 'post',
            contentType: 'application/json',
            success: function (result) {
                if (result.success) {
                    if (cb && $.isFunction(cb)) {
                        cb();
                    }
                    $.msg('保存成功');
                } else {
                    $.error(result.msg);
                }
            }
        })
    }
})(jQuery)

METRO_AUTO_REINIT = true;
