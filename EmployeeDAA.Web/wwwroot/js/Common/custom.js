$(document).ready(function () {
    $(".mobile__nav").click(function () {
        $(this).toggleClass('open');
        $(".mobile_menu_view").toggleClass("open");
        $(".header__nav--overlay").toggleClass("open");
    });

    $(".header__nav--overlay").click(function () {
        $(this).removeClass("open");
        $(".mobile__nav").removeClass("open");
        $(".mobile_menu_view").toggleClass("open");
    });
    $(".greedy-nav .hidden-links li").clone().prependTo(".mobile_menu_view");
    $(".greedy-nav .visible-links li").clone().prependTo(".mobile_menu_view");


    var $nav = $('.greedy-nav');
    var $btn = $('.greedy-nav button');
    var $vlinks = $('.greedy-nav .visible-links');
    var $hlinks = $('.greedy-nav .hidden-links');
    var breaks = [];

    function updateNav() {
        if ($(window).width() >= 991) {
            var availableSpace = $btn.hasClass('hidden') ? $nav.width() : $nav.width() - $btn.width();
            if ($vlinks.width() > availableSpace) {
                breaks.push($vlinks.width());
                $vlinks.children().last().prependTo($hlinks);
                if ($btn.hasClass('hidden')) {
                    $btn.removeClass('hidden');
                }
            } else {
                if (availableSpace > breaks[breaks.length - 1]) {
                    $hlinks.children().first().appendTo($vlinks);
                    breaks.pop();
                }
                if (breaks.length < 1) {
                    $btn.addClass('hidden');
                    $hlinks.addClass('hidden');
                }
            }
            if ($vlinks.width() > availableSpace) {
                updateNav();
            }
        }
    }
    $btn.on('click', function () {
        if ($hlinks.hasClass("hidden")) {
            $hlinks.removeClass("hidden");
        } else {
            $hlinks.addClass("hidden");
        }
    });

    updateNav();

    $(document).on('click', '.hidden-links li', function () {
        $(".visible-links  li:last-child").prependTo(".hidden-links");
        $(this).appendTo(".visible-links");
        $(".menulevel1").removeClass("openmenu");
    });

    $(document).on('click', '.submenu', function () {
        $(this).siblings('div').toggleClass("openmenu");
        $(this).parent().siblings('li').children("div").removeClass("openmenu");
    });

    $(document).on("click", ".visible-links li", function () {
        $(".hidden-links").addClass("hidden");
    });

    $(document).on("click", function (e) {
        var container1 = $(".submenu");
        if (!container1.is(e.target) && container1.has(e.target).length === 0) {
            if (!$(e.target).siblings().hasClass("menulevel1")) {
                container1.siblings().removeClass("openmenu");
            }
        }
    });

    $(document).mouseup(function (e) {
        var container = $(".hidden-links");
        if (!container.is(e.target) && container.has(e.target).length === 0) {
            if (!$(e.target).hasClass("menu_hamburger")) {
                container.addClass("hidden");
            }
        }
    });
    $('.header__dropdown > .btn').click(function () {
        $(".menulevel1").removeClass("openmenu");
    });
    $(window).on('resize', function (e) {
        updateNav();
    });
    (function ($) {
        $('.datepicker').datetimepicker({
            "allowInputToggle": true,
            "showClose": true,
            "showClear": true,
            "showTodayButton": true,
            "format": "DD/MM/YYYY",
        });
    })(jQuery);
    $('#timepicker1').timepicki();
    $('#timepicker2').timepicki();
    $(".tab-wrapper a.nav-link").each(function () {
        if (window.location.href.indexOf($(this).attr("href")) > -1 && !$(this).hasClass("back")) {
            $(this).addClass("active");
        }
    });
    $(".mob__drop--opener").click(function () {
        $(".tab-wrapper .nav").toggleClass("open");
    });

});
$(window).scroll(function () {
    var scroll = jQuery(window).scrollTop();

    if (scroll >= 10) {
        $('.middle__bar--no-tab').addClass('sticky__actions');
    }
    else {
        $('.middle__bar--no-tab').removeClass('sticky__actions');
    }

    if (scroll >= 10) {
        $('.middle__bar--has-tab').addClass('sticky__actions');
    }
    else {
        $('.middle__bar--has-tab').removeClass('sticky__actions');
    }

    if (scroll >= 10) {
        $('.tab-wrapper').addClass('sticky__tab--wrapper');
    }
    else {
        $('.tab-wrapper').removeClass('sticky__tab--wrapper');
    }
});
$(document).on("click", ".mob__btns--opener", function () {
    $('.mob__hidden--btns').toggleClass("open");
});
$(document).click(function (e) {
    e.stopPropagation();
    var container = $(".top__actions");
    if (container.has(e.target).length === 0) {
        $('.mob__hidden--btns').removeClass('open');
    }
})
$(document).ready(function () {
    if ($(".tab-wrapper .nav-link").hasClass('active')) {
        $(".tab-wrapper .nav-link.active").text()
        $(".mob__drop--opener").text($(".tab-wrapper .nav-link.active").text())
    }
    if (localStorage.getItem("Role") == "2")
        $(".sc-area").remove();
    else
        $(".sc-area").show();
});