$(function () {
    $(".preNext").css("opacity", 0.1).hover(function () {
        $(this).stop(true, false).animate({ "opacity": "0.5" }, 400);
    }, function () {
        $(this).stop(true, false).animate({ "opacity": "0.1" }, 400);
    });
});

var oIndex = 0;
var total = 0;
function init(t) {
    total = t;
}
function move(index) {
    $(".cloud-area").hide().eq(index).show();
}
function next() {
    oIndex++;
    if (oIndex >= total) {
        oIndex = 0;
    }
    move(oIndex);
}