$(".nav-icon-wrapper").click(function setActive(e) {
    console.log(e.currentTarget);
    $('.nav-link').removeClass("active");

    var pathname = window.location.pathname;
    if (pathname.includes("Apple")) {
        console.log(pathname);
        $('#link-apple').addClass('active');
    }
    else if (pathname.includes("Contacts")) {
        $('#link-contacts').addClass('active');
    }
    else if (pathname.includes("Cart")) {
        $('#link-cart').addClass('active');
        $('#link-cart').addClass('active');
    }
});