

$(function () {
    function action(table, record, td) {
        var html = [
            '<a class="button primary" href="/admin/student/modify?id=' + record.ID + '">修改</a>',
            '&nbsp;&nbsp;'
        ];
        if (record.Enabled) {
            html.push('<a class="button link" data-action-type="off" >禁用</a>')
        } else {
            html.push('<a class="button link" data-action-type="on" >启用</a>')
        }
        td.html(html);
        td.find('.primary').on('click',function(e) {
            
        })
        td.find('.link').on('click', function (e) {
            var type = $(this).attr('data-action-type');
            var enabled = false;
            if (type == "on") {
                enabled = true;
            }
            $.ajax({
                url: '/admin/student',
                type: 'put',
                data: { ID: record.ID, Enabled: enabled },
                success:function(res) {
                    if (res.success) {
                        table.reload();
                        $.msg('保存成功');
                    } else {
                        $.error(res.msg);
                    }
                }
            })
        })
    }
    var colModel = [
        { field: 'StudentID', caption: '学号', width: 120, sortable: false, cls: 'text-center', hcls: "text-center" },
        { field: 'Name', caption: '姓名', width: 200, sortable: false, cls: 'text-left', hcls: "text-left" },
        { field: 'EnglishName', caption: 'English Name', width: 150, sortable: false, cls: 'text-left', hcls: "text-left" },
        { field: 'Enabled', caption: '状态', type: 'booleancolumn', width: 100, sortable: false, cls: 'text-center', hcls: 'text-center', trueText: '已启用', falseText: '已禁用' },
        { field: 'Action', caption: '操作',  width: '80', sortable: false, cls: 'text-left', hcls: "text-center", renderer: action}
        
    ];
    
    
    
    $('#StudentTable').datatable({
        colModel: colModel,
        url: '/admin/student/list',
        cls:'table striped bordered hovered'
    });

});