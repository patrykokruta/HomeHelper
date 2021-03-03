$(document).ready(function () {
    $(".owl-carousel").owlCarousel({
        items: 2,
        autoplay: true,
        loop: true,
        margin: 30
    });
});

$(document).ready(function () {

    $("#sidebarCollapse").on("click", function () {
        $("#sidebar").toggleClass("dashboard-active");

    });

});
