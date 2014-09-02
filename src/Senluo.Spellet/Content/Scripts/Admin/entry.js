$(function() {
    $('#addTran').on('click', function () {

        var content = [
            '<label>释义：</label>',
            '<div class="input-control password"><input type="password" name="OldPwd"></div>',
            '<button class="button primary" type="button">添&nbsp;加</button>&nbsp;',
            '<button class="button" type="button" onclick="$.Dialog.close()">取&nbsp;消</button> '
        ].join("");

        $.Dialog({
            shadow: true,
            overlay: true,
            draggable: true,
            flat:true,
            icon: '<span class="icon-cog"></span>',
            title: '添加释义',
            width: 500,
            height: 200,
            padding: 10,
            content: content,
            onShow: function () {
                var window = arguments[0];
                window.find('.primary').on('click', function() {
                    
                });
                
            }

        });
    })
})