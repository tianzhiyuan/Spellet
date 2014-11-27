$(function () {
    $(".preNext").css("opacity", 0.1).hover(function () {
        $(this).stop(true, false).animate({ "opacity": "0.5" }, 400);
    }, function () {
        $(this).stop(true, false).animate({ "opacity": "0.1" }, 400);
    });
});

var curpage = 1;

function ajaxQuery(index) {
    var start = $("#txtStart").val();
    var end = $("#txtEnd").val();
    
    $.ajax({
        type: 'get',
        url: '/student/course/historybody',
        data: { start: start, end: end, index: index },
        success: function (responsetext) {
            $('body').unmask();
            $("#historybody").html(responsetext);
        }
    });
}