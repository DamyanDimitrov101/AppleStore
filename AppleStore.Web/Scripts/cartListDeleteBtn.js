function DeletePurchase() {
    event.preventDefault();
    var form = $('#__AjaxAntiForgeryForm');
    var purchasedAppleId = form.attr('value');
    var token = $('input[name="__RequestVerificationToken"]', form).val();

    var url = "/Cart/Delete";

    $.ajax({
        url: url,
        type: 'POST',
        data: {
            __RequestVerificationToken: token,
            purchasedAppleId: purchasedAppleId
        },
        success: function (data) {
            document.location =
                "/Cart/";
        }
    });
}