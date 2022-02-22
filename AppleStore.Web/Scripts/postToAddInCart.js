$('#Buy').click(function () {
    event.preventDefault();
    var count = $("#Count").val();
    var appleId = $("#AppleId").val();
    var userId = $("#UserId").val();
    var form = $('#__AjaxAntiForgeryForm');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    var url = "/Cart/Add";

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            model: {
                AppleId: appleId,
                UserId: userId,
                Count: count
            }
        },
        success: function (data) {
            document.location =
                "/Cart/";
        }
    });
})
