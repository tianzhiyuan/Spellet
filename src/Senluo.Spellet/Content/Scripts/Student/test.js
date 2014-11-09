$(function () {
    $('.cm_bg').css({ 'height': '700px', 'width': $('body').width() + 'px' });

    $(".cm_ht a").bind("click", function () {
        $("#examPannel").html();
        $(".cm_bg").hide();
        $(".cm_dg").hide();
    });

    $(".cm_label_s").bind("click", function () {
        showAnswer();
    });

    $(".cm_sumbit_s").bind("click", function () {
        submitAnswer();
    });
})

function GetExam(type, tid) {
    $.ajax({
        type: 'get',
        url: '/student/test/exam',
        data: { type: type, tid: tid },
        success: function (responsetext) {
            $("#examPannel").html(responsetext);
        }
    });
    $(".cm_bg").show();
    $(".cm_dg").show();
}

function selfHistory(type) {
    var num = $(".number").val();
    if (!num) {
        alert("请输入题目数量");
        return false;
    }
    if (isNaN(num)) {
        alert("输入合法的数字");
        return false;
    }
    GetExam(type, Number(num));
}

function selfCourse(cid) {
    GetExam(2,cid);
}

var state = 1;
function showAnswer() {
    if (state == 1) {
        $(".answer").show();
        state = 2;
    } else {
        $(".answer").hide();
        state = 1;
    }
}

function submitAnswer() {
    var data = "";
    $(".sheet").each(function () {
        var id = $(this).attr("id").split("_")[1];
        var word = $(this).attr("data-head") + $(this).val();
        data += id + "_" + word + ";";
    })
    data += $(".cm_sumbit_s").attr("data-qid");
    $.ajax({
        type: 'post',
        url: '/student/test/answer',
        data: { data: data },
        success: function (response) {
            if (response.success) {
                $("#examPannel").html();
                $(".cm_bg").hide();
                $(".cm_dg").hide();
                alert(response.msg);
            } else {
                alert(response.msg);
            }
        }
    });
}