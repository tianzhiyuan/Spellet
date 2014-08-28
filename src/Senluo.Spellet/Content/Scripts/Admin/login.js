$(function() {
    var hint = function (content) {
        var me = this;
        var height = me.offset().top - 10;
        $('.notice-wrapper').css({
            display: "block",
            top:height
        });
        $('.notice').html('<i class="icon-warning on-left-more fg-orange"></i>' + content);
    }
    $.fn.extend({
        login_hint: hint
    });
    var login = function() {
        var obj = $('form').getValues();
        if (!obj.username) {
            $('.text').login_hint('请输入用户名');
            $('input[type=text]').focus();
            return;
        }
        if (!obj.password) {
            $('.password').login_hint('请输入密码 ');
            $('input[type=password]').focus();
            return;
        }
        $.ajax({
            url: '/admin/login',
            type: 'post',
            data:obj,
            success:function(result) {
                if (!result.success) {
                    $('form').login_hint(result.msg);
                } else {
                    if (url) {
                        window.location.href = url;
                    } else {
                        window.location.href = '/admin/home';
                    }
                    
                }
            }
        })
    }
    $('input[type=button]').on('click', login);
    $('input[type=password]').bind('keypress', function(e) {
        var key = window.event ? e.keyCode : e.which;
        if (key == "13") {
            login();
        }
    });
    $('input[type=text]').bind('keypress', function(e) {
        var key = window.event ? e.keyCode : e.which;
        if (key == "13") {
            login();
        }
    })
})