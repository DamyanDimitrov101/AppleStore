function SetCount(id, pairs, newPrice) {
    
    var count = $('#Count').val(pairs);

    $('.details-total-input').attr("data-total", newPrice);
    $('.details-total-input').attr("value", newPrice);


    $("#Buy").removeClass("btn-outline-primary");
    $("#Buy").addClass("btn-primary");
    $(".btn-minus").removeAttr("disabled");
};
