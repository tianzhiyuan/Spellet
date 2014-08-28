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
    $('input[type=button]').on('click',function() {
        var obj = $('form').getValues();
        if (!obj.username) {
            $('.text').login_hint('请输入用户名');
            return;
        }
        if (!obj.password) {
            $('.password').login_hint('请输入密码 ');
            return;
        }
        $.ajax({
            url: '/admin/login',
            type: 'post',
            success:function(result) {
                if (!result.success) {
                    $('form').login_hint(result.msg);
                } else {
                    window.location.href = '/admin/home';
                }
            }
        })
    })
})