

$(function () {
    function action(table, record, td) {
        var html = [
            '<a class="button primary" href="/admin/student/modify?id='+record.ID + '">修改</a>',
            '&nbsp;&nbsp;',
            '<a class="button link">删除</a>'
        ].join("");
        td.html(html);
        td.find('.primary').on('click',function(e) {
            
        })
        td.find('.link').on('click',function(e) {
            $.delete('/admin/student', record.ID, function() { table.reload(); });
        })
    }
    var colModel = [
        { field: 'StudentID', caption: '学号', width: 120, sortable: false, cls: 'text-center', hcls: "text-center" },
        { field: 'Name', caption: '姓名', width: 200, sortable: false, cls: 'text-left', hcls: "text-left" },
        { field: 'EnglishName', caption: 'English Name', width: 150, sortable: false, cls: 'text-left', hcls:"text-left" },
        { field: 'Action', caption: '操作', width: '80', sortable: false, cls: 'text-left', hcls: "text-center", renderer: action}
        
    ];
    
    
    
    $('#StudentTable').datatable({
        colModel: colModel,
        url: '/admin/student/list',
        cls:'table striped bordered hovered'
    });

});