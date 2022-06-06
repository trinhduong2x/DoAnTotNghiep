$(document).ready(function () {
  
    $("#owl-1").owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: false,
        items: 3, //10 items above 1000px browser width
        dots: false,
        nav: false,
        //navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    });
    $("#owl-2").owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: false,
        items: 3, //10 items above 1000px browser width
        dots: false,
        nav: false,
        //navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    }); 
    $("#tour-detail").owlCarousel({
        loop: false,
        margin: 20,
        responsiveClass: true,
        autoplay: false,
        items: 3, //10 items above 1000px browser width
        dots: false,
        nav: false,
        //navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 2
            },
            1000: {
                items: 3
            }
        }
    }); 
});
