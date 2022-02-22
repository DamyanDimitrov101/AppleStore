function GetDetails (id, pairs, newPrice, isAdmin) {
    if (isAdmin) {
        document.location =
            "/Admin/Details/" + id;
    } else {
        document.location =
            "/Apple/Details/" + id;
    }
    
    $('#Count').val(pairs);

    $('.details-total-input').attr("data-total", newPrice);
    $('.details-total-input').attr("value", newPrice);
};
