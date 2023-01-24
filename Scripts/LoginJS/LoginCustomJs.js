    (function ($) {
        "use strict";

    // on ready function
    jQuery(document).ready(function ($) {
                var $this = $(window);

    // on load function
                $this.on("load", function () {
        $("#status").fadeOut();
    $("#preloader").delay(350).fadeOut("slow");
});


// on scroll function
                $this.on("scroll", function () {
                    // fixed menu on scroll
                    var window_top = $(window).scrollTop() + 1;
    var widowWidth = $(window).width();
    var hbw_height = $('.header_bottom_wrapper').innerHeight();
                    if ((window_top > 2200) && (widowWidth > 768)) {
        $('.header_top_wrapper').css('marginTop', hbw_height);
    $('.header_bottom_wrapper').addClass('fixed_menu');
                    } else {
        $('.header_top_wrapper').css('marginTop', '0px');
    $('.header_bottom_wrapper').removeClass('fixed_menu');
}

});

// on resize function
                $this.on("resize", function () {

    });

    //on bind scroll
                $this.bind("scroll", function () {

    });

                $.stellar({
        horizontalScrolling: false
});

//woocomerce single page number input
                $('.tab-wrapper > ul').each(function () {
                    var $active, $content, $links = $(this).find('a');

    $active = $($links.filter('[href="' + location.hash + '"]')[0] || $links[0]);
    $active.addClass('active');

    $content = $($active[0].hash);

                    $links.not($active).each(function () {
        $(this.hash).hide();
    });

                    $(this).on('click', 'a', function (e) {
        $active.removeClass('active');
    $content.hide();

    $active = $(this);
    $content = $(this.hash);

    $active.addClass('active');
    $content.show();

    e.preventDefault();
});
});



//woocomerce single page number input
                $('.minus').on("click", function () {
                    var $input = $(this).parent().find('input');
    var count = parseInt($input.val(), 10) - 1;
                    count = count < 1 ? 1 : count;
    $input.val(count);
    $input.change();
    return false;
});
                $('.plus').on("click", function () {
                    var $input = $(this).parent().find('input');
    $input.val(parseInt($input.val(), 10) + 1);
    $input.change();
    return false;
});

//woocommerce product single thumbnail slider
                $("#thumbnail_slider.owl-carousel").owlCarousel({
        items: 3,
    dots: false,
    nav: true,
    autoplay: false,
    margin: 20,
    navText: ['', '']
});


                $(".slider2 .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: true,
    nav: false,
    autoplay: false,
    margin: 0,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

                $(".post_image_slider .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: false,
    nav: true,
    margin: 0,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

// sidbar testimonial
                $(".testimonial_wrapper .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: false,
    nav: false,
    animateIn: 'fadeInDown',
    animateOut: 'fadeOutDown',
    autoHeight: true,
    touchDrag: false,
    mouseDrag: false,
    margin: 0,
    autoplay: true,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

//woocommerce filter by price slider
$('.slider').slider();


// blog youtube video play on click
                $("#playvideo").on("click", function () {
        $("#youtube_video")[0].src += "&autoplay=1";
    $(this).unbind("click");
    $(".video_overlay").css("display", "none");
});
                $("#playvideo2").on("click", function () {
        $("#youtube_video2")[0].src += "&autoplay=1";
    $(this).unbind("click");
    $(".video_overlay").css("display", "none");
});
                $("#playvideo3").on("click", function () {
        $("#youtube_video3")[0].src += "&autoplay=1";
    $(this).unbind("click");
    $(".video_overlay").css("display", "none");
});


// index 1 layerslider
                jQuery("#layerslider").layerSlider({
        responsive: true,
    pagination: true,
    responsiveUnder: 1150,
    layersContainer: 1150,
    skin: 'noskin',
    hoverPrevNext: true,
    autoPlayVideos: false,
    skinsPath: 'js/plugin/layerslider/skins/',







});



// Submenu Check Visible Space
                $(".summit_nav_wrapper ul li").hover(function () {
                    if ($(window).width() < 700) {
                        return ;
}
var subMenu = $(this).find("ul.sub-menu");

                    if (!subMenu.get(0)) {
                        return ;
}
var screenWidth = $(window).width(),
    subMenuOffset = subMenu.offset(),
    subMenuWidth = subMenu.width(),
    subMenuParentWidth = subMenu.parents("ul.sub-menu").width(),
    subMenuPosRight = subMenu.offset().left + subMenu.width();

                    if (subMenuPosRight > screenWidth) {
        subMenu.css("margin-left", "-" + (subMenuParentWidth + subMenuWidth + 10) + "px");
    } else {
        subMenu.css("margin-left", 0);
    }
});

                $(".summit_nav_wrapper ul li").hover(function () {
                    if ($(window).width() < 700) {
                        return ;
}
var subMenu = $(this).find(".mega-menu-wrapper");

                    if (!subMenu.get(0)) {
                        return ;
}
var screenWidth = $(window).width(),
    subMenuOffset = subMenu.offset(),
    subMenuWidth = subMenu.width(),
    subMenuParentWidth = subMenu.parents(".mega-menu-wrapper").width(),
    subMenuPosRight = subMenu.offset().left + subMenu.width();

                    if (subMenuPosRight > screenWidth) {
        subMenu.css("margin-left", "-" + (subMenuParentWidth + subMenuWidth / 1.2) + "px");
    } else {
        subMenu.css("margin-left", 0);
    }
});

                $(".header_style4 .summit_nav_wrapper ul li").hover(function () {
                    if ($(window).width() < 700) {
                        return ;
}
var subMenu = $(".header_style4 .summit_nav_wrapper ul").find(".mega-menu-wrapper");

var subMenu123 = $(".header_style4 .summit_nav_wrapper ul").find("li.mega-menu");

                    if (!subMenu.get(0)) {
                        return ;
}

var screenWidth = $(window).width();
var subMenuPosRight = subMenu123.offset().left;

                    if (subMenuPosRight > 250) {
        subMenu.css("margin-left", "-" + (subMenuPosRight / 2) + "px");
    } else {
        subMenu.css("margin-left", 0);
    }
});

                $(".header_style6 .summit_nav_wrapper ul li").hover(function () {
                    if ($(window).width() < 700) {
                        return ;
}
var subMenu = $(".header_style6 .summit_nav_wrapper ul").find(".mega-menu-wrapper");
var subMenu123 = $(".header_style6 .summit_nav_wrapper ul").find("li.mega-menu");

                    if (!subMenu.get(0)) {
                        return ;
}
var screenWidth = $(window).width();
var subMenuPosRight = subMenu123.offset().left;

                    if (subMenuPosRight > 250) {
        subMenu.css("margin-left", "-" + (subMenuPosRight / 1.5) + "px");
    } else {
        subMenu.css("margin-left", 0);
    }
});

                $(".header_style7 .summit_nav_wrapper ul li").hover(function () {
                    if ($(window).width() < 700) {
                        return ;
}
var subMenu = $(".header_style7 .summit_nav_wrapper ul").find(".mega-menu-wrapper");
var subMenu123 = $(".header_style7 .summit_nav_wrapper ul").find("li.mega-menu");

                    if (!subMenu.get(0)) {
                        return ;
}
var screenWidth = $(window).width();
var subMenuPosRight = subMenu123.offset().left;

                    if (subMenuPosRight > 250) {
        subMenu.css("margin-left", "-" + (subMenuPosRight / 1.5) + "px");
    } else {
        subMenu.css("margin-left", 0);
    }
});


//main wrapper height
var main_wrapper_h = $("body").innerHeight() - 400;
$("#wrapper").css('height', main_wrapper_h);


// portfolio_1col slider
                $(".portfolio_1col_slider .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: false,
    nav: true,
    margin: 0,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

                $(".port_img_slider .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: false,
    autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    nav: true,

    margin: 0,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

//related project slider
var project_slider = $("#project_slider");
                project_slider.owlCarousel({
        autoPlay: true,
    responsiveClass: true,
    pagination: true,
    items: 5,
    dots: false,
                    responsive: {
        0: {
        items: 1
},
                        600: {
        items: 2
},
                        1000: {
        items: 5
}
}
});
// Go to the next item
                $('.project_slider_Next').on("click", function () {
        project_slider.trigger('next.owl.carousel');
    });
    // Go to the previous item
                $('.project_slider_Prev').on("click", function () {
        project_slider.trigger('prev.owl.carousel', [300]);
    });

    //portfolio single wide slider
                $(".portfolio_wide_slider .owl-carousel").owlCarousel({
        loop: true,
    items: 1,
    dots: false,
    nav: true,
    margin: 0,
                    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

//partner slider
                $("#summit_partner").owlCarousel({
        autoplay: true,
    autoplayTimeout: 3000,
    autoplayHoverPause: true,
    loop: true,
    responsiveClass: true,
    pagination: false,
    items: 7,
    dots: false,
    margin: 0,
                    navText: ['<svg version="1.1" id="Capa_1" xmlns="http://www.w3.org/2000/svg" xmlns: xlink="http://www.w3.org/1999/xlink" x="0px" y="0px" width="53px" height="53px" viewBox="0 0 94.669 94.67" style="enable-background:new 0 0 94.669 94.67;" xml:space="preserve"><g><path d="M27.981,47.335l41.398,45.997l-1.485,1.338L25.291,47.335L67.894,0l1.485,1.338L27.981,47.335z"></path></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g><g></g></svg>', ' < svg version = "1.1" id = "Capa_1" xmlns = "http://www.w3.org/2000/svg" xmlns: xlink = "http://www.w3.org/1999/xlink" x = "0px" y = "0px" width = "53px" height = "53px" viewBox = "0 0 94.671 94.671" style = "enable-background:new 0 0 94.671 94.671;" xml: space = "preserve" > <g><path d="M69.38,47.335L26.777,94.671l-1.486-1.338l41.396-45.998L25.291,1.337L26.776,0L69.38,47.335z"></path></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g> <g></g></svg > '],
responsive: {
    0: {
        items: 1,
            nav: true
    },
    480: {
        items: 3,
            nav: true
    },
    600: {
        items: 5,
            nav: true
    },
    1000: {
        items: 7,
            nav: true
    }
}
                });


//fancybox
$(".fancybox").fancybox({
    padding: 0,

    openEffect: 'elastic',
    openSpeed: 150,

    closeEffect: 'elastic',
    closeSpeed: 150,

    closeClick: true
});

/* ---------------------------------------------------------------------- */
/*	Detect Touch Device													  */
/* ---------------------------------------------------------------------- */

(function () {

    if (Modernizr.touch) {
        $('body').addClass('touch-device');
    }
}());

/* end Detect Touch Device */

//full width slider
$("#fullwidth").owlCarousel({
    loop: true,
    items: 1,
    dots: false,
    nav: true,
    margin: 0,
    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>']
});

//circle Skill
/* ---------------------------------------------------- */
/*	Pie Chart										*/
/* ---------------------------------------------------- */


//$('.percentage-light').easyPieChart({
//    barColor: function (percent) {
//        percent /= 100;
//        return "#99a4bd";
//    },
//    trackColor: 'rgba(255, 255, 255,0)',
//    scaleColor: false,
//    lineCap: 'butt',
//    rotate: 180,
//    lineWidth: 6,
//    animate: 6000,
//    onStep: function (value) {
//        this.$el.find('span').text(~~value);
//    }
//});

/* end Pie Chart */




//team
$("#team").owlCarousel({
    autoPlay: false,
    responsiveClass: true,
    pagination: true,
    items: 4,
    dots: false,
    margin: 20,
    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 2,
            nav: true
        },
        1000: {
            items: 4,
            nav: true
        }
    }
});

//team 2
$("#team2").owlCarousel({
    autoPlay: false,
    responsiveClass: true,
    pagination: true,
    items: 3,
    dots: false,
    margin: 20,
    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 2,
            nav: true
        },
        1000: {
            items: 3,
            nav: true
        }
    }
});

$('#slider-one').movingBoxes({
    startPanel: 3,
    wrap: true,
    buildNav: false,
    panelWidth: 0.12,
    navFormatter: function () {
        return "&#9679;";
    }
});


$('#about_atc3').show();
$('.about_the_compny_icon li').on("click", function () {
    var itm = $(this).attr("id");
    $('#about_' + itm).show().addClass('animated fadeInDown');

    var cur_id = 'about_' + itm;
    $('.about_the_compny').each(function () {
        if ($(this).attr("id") != cur_id) {
            $(this).hide();
        }
    });
});


//for counter
//$('.appear-count').appear(function () {
//    $('.count').countTo();
//});

// tooltip
$('[data-toggle="tooltip"]').tooltip();

//accordion single-collapse
var active1 = $('#accordion .panel-collapse.in').prev().addClass('active');
active1.find('a').prepend('<i class="fa fa-minus"></i>');
$('#accordion .panel-heading').not(active1).find('a').prepend('<i class="fa fa-plus"></i>');
$('#accordion').on('show.bs.collapse', function (e) {
    $('#accordion .panel-heading.active').removeClass('active').find('.fa').toggleClass('fa-plus').toggleClass('fa-minus');
    $(e.target).prev().addClass('active').find('.fa').toggleClass('fa-plus').toggleClass('fa-minus');
});


//accordion style 2 single-collapse
var active2 = $('#accordion2 .panel-collapse.in').prev().addClass('active');
active2.find('a').prepend('<i class="fa fa-minus"></i>');
$('#accordion2 .panel-heading').not(active2).find('a').prepend('<i class="fa fa-plus"></i>');
$('#accordion2').on('show.bs.collapse', function (e) {
    $('#accordion2 .panel-heading.active').removeClass('active').find('.fa').toggleClass('fa-plus').toggleClass('fa-minus');
    $(e.target).prev().addClass('active').find('.fa').toggleClass('fa-plus').toggleClass('fa-minus');
});


//related project slider
$("#blog_post_slider").owlCarousel({
    autoplay: true,
    autoplayTimeout: 5000,
    autoplayHoverPause: true,
    loop: true,
    responsiveClass: true,
    pagination: true,
    items: 2,
    dots: false,
    nav: true,

    navText: ['<i class="fa fa-angle-left"></i>', '<i class="fa fa-angle-right"></i>'],
    responsive: {
        0: {
            items: 1,
            nav: true
        },
        600: {
            items: 2,
            nav: true
        },
        1000: {
            items: 4,
            nav: true
        }
    }
});
//$('div').hide();
//shortcode related project slider
var owl = $("#shortcode_project_slider");
owl.owlCarousel({
    autoPlay: true,
    loop: true,
    responsiveClass: true,
    pagination: true,
    items: 7,
    margin: 40,
    dots: false,
    responsive: {
        0: {
            items: 1
        },
        600: {
            items: 2
        },
        1000: {
            items: 2
        }
    }
});
// Go to the next item
$('.sc_project_slider_Next').on("click", function () {
    owl.trigger('next.owl.carousel');
});
// Go to the previous item
$('.sc_project_slider_Prev').on("click", function () {
    owl.trigger('prev.owl.carousel', [300]);
});


/**************************** Portfolio start *****************************/
$(window).load(function () {
    //var $container = $('.portfolioPageContainer');
    //$container.isotope({
    //    filter: '*',
    //    animationOptions: {
    //        duration: 750,
    //        easing: 'linear',
    //        isFitWidth: true,
    //        queue: false
    //    }
    //});

    //$('.portfolioFilter a').on("click", function () {
    //    $('.portfolioFilter .current').removeClass('current');
    //    $(this).addClass('current');

    //    var selector = $(this).attr('data-filter');
    //    $container.isotope({
    //        filter: selector,
    //        animationOptions: {
    //            duration: 750,
    //            easing: 'linear',
    //            queue: false
    //        }
    //    });
    //    return false;
    //});
});


                //$(window).load(function () {
                //    var $container = $('.portfolioPageContainer');
                //    $container.isotope({
                //        filter: '*',
                //        animationOptions: {
                //            duration: 750,
                //            easing: 'linear',
                //            isFitWidth: true,
                //            queue: false
                //        }
                //    });
                //    $('.portfolioFilter a').on("click", function () {
                //        $('.portfolioFilter .current').removeClass('current');
                //        $(this).addClass('current');

                //        var selector = $(this).attr('data-filter');
                //        $container.isotope({
                //            filter: selector,
                //            animationOptions: {
                //                duration: 750,
                //                easing: 'linear',
                //                queue: false
                //            }
                //        });
                //        return false;
                //    });
                //});

                //$(window).load(function () {
                //    var $container = $('.isotope');
                //    $container.isotope({
                //        layoutMode: 'packery',
                //        itemSelector: '.item',
                //        packery: {
                //            gutter: 0,
                //            isFitWidth: true

                //        }
                //    });
                //    $('.portfolioFilter a').on("click", function () {
                //        $('.portfolioFilter .current').removeClass('current');
                //        $(this).addClass('current');

                //        var selector = $(this).attr('data-filter');
                //        $container.isotope({
                //            filter: selector,
                //            animationOptions: {
                //                duration: 750,
                //                easing: 'linear',
                //                queue: false
                //            }
                //        });
                //        return false;
                //    });
                //});
                /**************************** Portfolio end *****************************/


            });

        }) ();

