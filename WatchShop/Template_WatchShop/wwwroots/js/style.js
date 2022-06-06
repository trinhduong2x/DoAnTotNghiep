$(window).scroll(function() {
    // var headerHeight = $(".header").outerHeight();
    // kiểm tra điều kiện > header thì mới addClass 
    if ($(window).scrollTop() > 400) {
        $('.header-main').addClass('fixed');
    } else {
        $('.header-main').removeClass('fixed');
    }
});


$(document).ready(function() {

    $(".image-item .image-item-inner").click(function() {
        if (!$(this).parents(".image-item").hasClass("active")) {
            $(this).parents(".image-item").addClass("active");

        }
    })
    $(".btn-open-menu").click(function() {
        $(this).toggleClass("opened");
        $(".menu-navigation").toggleClass("opened");
    });
    $(".btn-signup").click(function() {
        $('.login-wrap').addClass("opened");
    });
    $(".boxclose").click(function() {
        $('.login-wrap').removeClass("opened");
    });

    $(".menu-item-mobile .sub-menu-toggle").click(function() {
        $(this).parents(".menu-item-mobile:first").toggleClass("opened");
    });
    $("li .sub-menu-toggle2").click(function() {
        $(this).parents("li:first").toggleClass("opened");
    });


    $('.banner-main').owlCarousel({
        loop: true,
        center: true,
        items: 1,
        slideSpeed: 300,
        paginationSpeed: 400,
    })
    $('.room-list').owlCarousel({
        loop: true,
        margin: 20,
        // center: true,
        items: 5,
        nav: true,
        statePadding: 30,
        addClassActive: true,
        slideSpeed: 300,
        paginationSpeed: 400,
        responsive: {
            0: {
                items: 1
            },
            540: {
                items: 2
            },

            768: {
                items: 3
            },
            991: {
                item: 5
            }

        }

    })
    $('.testi-slide').owlCarousel({
        loop: true,
        center: true,
        items: 1,
        slideSpeed: 300,
        paginationSpeed: 400,
    })
    $('.lastest-new-slide').owlCarousel({
        loop: true,
        // center: true,
        items: 3,
        statePadding: 30,
        slideSpeed: 300,
        dots: false,
        paginationSpeed: 400,
        responsive: {
            0: {
                items: 1
            },
            769: {
                items: 2
            },

            991: {
                item: 3
            }

        }
    })
    $('.logo-list').owlCarousel({
        loop: true,
        items: 6,
        margin: 30,
        statePadding: 30,
        slideSpeed: 300,
        dots: false,
        paginationSpeed: 400,
        responsive: {
            0: {
                items: 2
            },
            540: {
                items: 3
            },

            768: {
                items: 4
            },
            991: {
                item: 5
            }

        }
    })

    $('.view-room-detail').owlCarousel({
        loop: true,
        // center: true,    
        margin: 20,
        items: 3,
        slideSpeed: 300,
        paginationSpeed: 400,
        responsive: {
            0: {
                items: 2
            },
            540: {
                items: 3
            }



        }

    })
    $('.single-room-list').owlCarousel({
        loop: true,
        // center: true,    
        margin: 20,
        items: 3,
        slideSpeed: 300,
        paginationSpeed: 400,
        responsive: {
            0: {
                items: 1
            },
            540: {
                items: 2
            },

            768: {
                items: 3
            }

        }
    })

    $('#date-contact').datepicker({
        dateFormat: "dd/mm/yy",
    });


    /* back to top */
    var btn = $('#backtotop');

    $(window).scroll(function() {
        if ($(window).scrollTop() > 300) {
            btn.addClass('show');
        } else {
            btn.removeClass('show');
        }
    });

    btn.on('click', function(e) {
        e.preventDefault();
        $('html, body').animate({
            scrollTop: 0
        }, '1000');
    });



    $(".register-link").click(function() {
        $(this).addClass("active");

        $(".register-form").addClass("active");
        $(".login-form").removeClass("active");
        $(".forgot-form").removeClass("active");
    });
    $(".login-link").click(function() {
        $(this).addClass("active");

        $(".login-form").addClass("active");
        $(".register-form").removeClass("active");
        $(".forgot-form").removeClass("active");
    });
    $(".forgot-link").click(function() {
        $(this).addClass("active");

        $(".forgot-form").addClass("active");
        $(".register-form").removeClass("active");
        $(".login-form").removeClass("active");
    });




});



$(document).ready(function() {

    var sync1 = $("#sync1");
    var sync2 = $("#sync2");
    var slidesPerPage = 3; //globaly define number of elements per page
    var syncedSecondary = true;

    sync1.owlCarousel({
        items: 1,
        slideSpeed: 2000,
        nav: true,
        navText: ['<i class="far fa-angle-left"></i>', '<i class="far fa-angle-right"></i>'],
        autoplay: false,
        dots: false,
        loop: true,
        responsiveRefreshRate: 200,

    }).on('changed.owl.carousel', syncPosition);

    sync2
        .on('initialized.owl.carousel', function() {
            sync2.find(".owl-item").eq(0).addClass("current");
        })
        .owlCarousel({
            items: slidesPerPage,
            dots: false,
            nav: false,
            smartSpeed: 200,
            slideSpeed: 500,

            slideBy: slidesPerPage,
            responsiveRefreshRate: 100,
            responsive: {
                0: {
                    items: 1
                },
                540: {
                    items: 3
                }



            }

        }).on('changed.owl.carousel', syncPosition2);

    function syncPosition(el) {


        var count = el.item.count - 1;
        var current = Math.round(el.item.index - (el.item.count / 2) - .5);

        if (current < 0) {
            current = count;
        }
        if (current > count)  {
            current = 0;
        }

        //end block

        sync2
            .find(".owl-item")
            .removeClass("current")
            .eq(current)
            .addClass("current");
        var onscreen = sync2.find('.owl-item.active').length - 1;
        var start = sync2.find('.owl-item.active').first().index();
        var end = sync2.find('.owl-item.active').last().index();

        if (current > end) {
            sync2.data('owl.carousel').to(current, 100, true);
        }
        if (current < start) {
            sync2.data('owl.carousel').to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if (syncedSecondary) {
            var number = el.item.index;
            sync1.data('owl.carousel').to(number, 100, true);
        }
    }

    sync2.on("click", ".owl-item", function(e) {
        e.preventDefault();
        var number = $(this).index();
        sync1.data('owl.carousel').to(number, 300, true);
    });
});

/* service slide */
$(document).ready(function() {

    var service1 = $("#service1");
    var service2 = $("#service2");
    var slidesPerPage = 1; //globaly define number of elements per page
    var syncedSecondary = true;

    service1.owlCarousel({
        items: 1,
        slideSpeed: 2000,
        nav: true,
        navText: ['<i class="fas fa-long-arrow-left"></i>', '<i class="fas fa-long-arrow-alt-right"></i>'],
        autoplay: false,
        dots: false,
        loop: true,
        responsiveRefreshRate: 200,

    }).on('changed.owl.carousel', syncPosition);

    service2
        .on('initialized.owl.carousel', function() {
            service2.find(".owl-item").eq(0).addClass("current");
        })
        .owlCarousel({
            items: slidesPerPage,
            dots: false,
            nav: false,
            smartSpeed: 200,
            slideSpeed: 500,


            slideBy: slidesPerPage,
            responsiveRefreshRate: 100,
            // responsive: {
            //     0: {
            //         items: 1
            //     },
            //     540: {
            //         items: 3
            //     }



            // }

        }).on('changed.owl.carousel', syncPosition2);

    function syncPosition(el) {


        var count = el.item.count - 1;
        var current = Math.round(el.item.index - (el.item.count / 2) - .5);

        if (current < 0) {
            current = count;
        }
        if (current > count)  {
            current = 0;
        }

        //end block

        service2
            .find(".owl-item")
            .removeClass("current")
            .eq(current)
            .addClass("current");
        var onscreen = service2.find('.owl-item.active').length - 1;
        var start = service2.find('.owl-item.active').first().index();
        var end = service2.find('.owl-item.active').last().index();

        if (current > end) {
            service2.data('owl.carousel').to(current, 100, true);
        }
        if (current < start) {
            service2.data('owl.carousel').to(current - onscreen, 100, true);
        }
    }

    function syncPosition2(el) {
        if (syncedSecondary) {
            var number = el.item.index;
            service1.data('owl.carousel').to(number, 100, true);
        }
    }

    service2.on("click", ".owl-item", function(e) {
        e.preventDefault();
        var number = $(this).index();
        sync1.data('owl.carousel').to(number, 300, true);
    });
});