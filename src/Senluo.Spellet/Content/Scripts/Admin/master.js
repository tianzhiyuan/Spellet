
$(function () {
    function submitChangePwd(evt) {
        var target = evt.target;
        var form = $(target).parents('form');
        if (!form.valid()) {
            return;
        }
        $.ajax({
            url: '/ChangePassword',
            type: 'post',
            data: form.getValues(),
            success: function(result) {
                if (result.success) {
                    $.msg('修改成功');
                } else {
                    $.error(result.msg);
                }
            }
        });
    }
    $("#ModifyPwd").on('click', function () {
        var content = [
            '<form id="changepwd">',
            '<label>当前密码</label>',
            '<div class="input-control password"><input type="password" name="OldPwd"></div>',
            '<label>新密码</label>',
            '<div class="input-control password"><input type="password" name="NewPwd"><button class="btn-reveal" ></button></div>',
            '<div class="form-actions">',
            '<button class="button primary" type="button">确认修改</button>&nbsp;' ,
            '<button class="button" type="button" onclick="$.Dialog.close()">取&nbsp;消</button> ' ,
            '</div>' ,
            '</form>'
        ].join("");
        $.Dialog({
            shadow: true,
            overlay: false,
            draggable: true,
            icon: '<span class="icon-cog"></span>',
            title: '修改登录密码',
            width: 500,
            height:280,
            padding: 10,
            content: content,
            onShow: function () {
                var window = arguments[0];
                window.find('.primary').on('click', submitChangePwd);
                var form = window.find('form');
                form.validate({
                    rules: {
                        OldPwd: {
                            required: true
                        },
                        NewPwd: {
                            required: true,
                            minlength:6
                        }
                    },
                    errorClass: 'text-warning'
                });
            }

        });
    });
})