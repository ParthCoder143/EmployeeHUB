var lefttextMenuState = 0;
var lefttextMenuWidth;
var lefttextMenuHeight;
var lefttextMenuPosition;
var lefttextMenuPositionX;
var lefttextMenuPositionY;

var windowWidth;
var windowHeight;

var lefttextMenu = document.querySelector("#lefttext__menu");

function getPosition(e) {
    var posx = 0;
    var posy = 0;

    if (!e) var e = window.event;

    if (e.pageX || e.pageY) {
        posx = e.pageX;
        posy = e.pageY;
    } else if (e.clientX || e.clientY) {
        posx = e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft;
        posy = e.clientY + document.body.scrollTop + document.documentElement.scrollTop;
    }

    return {
        x: posx,
        y: posy
    }
}


function positionlefttextMenu(e) {
    clickCoords = getPosition(e);
    clickCoordsX = clickCoords.x;
    clickCoordsY = clickCoords.y;

    lefttextMenuWidth = lefttextMenu.offsetWidth + 40;
    lefttextMenuHeight = lefttextMenu.offsetHeight + 4;

    windowWidth = window.innerWidth;
    //windowHeight = window.innerHeight;
    windowHeight = document.innerHeight;

    if ((windowWidth - clickCoordsX) < lefttextMenuWidth) {
        lefttextMenu.style.left = windowWidth - lefttextMenuWidth + "px";
    } else {
        lefttextMenu.style.left = clickCoordsX + "px";
    }

    if ((windowHeight - clickCoordsY) < lefttextMenuHeight) {
        lefttextMenu.style.top = windowHeight - lefttextMenuHeight + "px";
    } else {
        lefttextMenu.style.top = clickCoordsY + "px";
    }
}


/* Outside click Active class removed from Column */
// $(document).mousedown(function(event) {
//     $('.lefttext__menu').removeClass('lefttext__menu--active');
// });

/* click To Open */
$(document).ready(function () {
    $(document).on("click", ".left__context", function (e) {
        $("#lefttext__menu").addClass('lefttext__menu--active');
        $("#lefttext__menu").find(".context-menu__link").attr("data-id", $(this).val());
        positionlefttextMenu(e);
        if (typeof ContextMenuAction === 'function') {            
            ContextMenuAction(this);            
        }
    });
});

/* Outside click Active class removed from Column */
$(document).on('click', function (e) {
    if (!$(e.target).parent().hasClass('left__context')) {

        if ($(e.target).closest('nav').length > 0) {
            if (!$(e.target).closest('nav').hasClass('lefttext__menu')) {
                $('.lefttext__menu').removeClass('lefttext__menu--active');
                $('.lefttext__menu').removeAttr('style');
            }
        }
        else {
            $('.lefttext__menu').removeClass('lefttext__menu--active');
            $('.lefttext__menu').removeAttr('style');
        }
    }

});

/* On Esc click Active class removed from Column */
$(document).on('keydown', function (e) {
    $('.lefttext__menu').removeClass('lefttext__menu--active');
    $('.lefttext__menu').removeAttr('style');
});

/* On window resize Active class removed from Column */
$(window).resize(function () {
    $('.lefttext__menu').removeClass('lefttext__menu--active');
    $('.lefttext__menu').removeAttr('style');
});

/* On Window Scroll Active Class Remove*/
$(window).scroll(function () {
    var scroll = jQuery(window).scrollTop();

    if (scroll >= 10) {
        $('.lefttext__menu').removeClass('lefttext__menu--active');
        $('.lefttext__menu').removeAttr('style');
    }

});