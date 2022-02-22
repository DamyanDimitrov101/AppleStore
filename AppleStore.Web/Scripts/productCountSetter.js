var num;
var total;
var price = parseFloat($('.details-price-input').attr("data-price"));


$('.btn-minus').click(function () {
    num = parseInt($('.number-product:text').val());
    total = parseFloat($('.details-total-input').attr("data-total"));
    var sum = total - price;

    if (num > 1) {
        $('input:text').val(num - 1);
        $('.details-total-input').attr("data-total", sum);
        $('.details-total-input').attr("value", sum);
        $('.details-total-input').html(sum.toString());
    }
    if (num == 2) {
        $('.details-total-input').attr("data-total", price);
        $('.details-total-input').attr("value", price);
        $('.btn-minus').prop('disabled', true);
    }
});

$('.button-count:last-child').click(function () {
    num = parseInt($('input:text').val());
    total = parseFloat($('.details-total-input').attr("data-total"));
    var sum = total + price;
    
    if (num < 100) {
        $('input:text').val(num + 1);
        $('.details-total-input').attr("data-total", sum);
        $('.details-total-input').attr("value",sum);
        $('.details-total-input').html(sum.toString());
    }
    if (num > 0) {
        $("#Buy").removeClass("btn-outline-primary");
        $("#Buy").addClass("btn-primary");
        $(".btn-minus").removeAttr("disabled");
    }
});