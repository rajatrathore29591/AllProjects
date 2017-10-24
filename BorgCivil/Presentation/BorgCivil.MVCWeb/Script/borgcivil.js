/**
 * BorgCivil
 * version 1.0
 *
 */
$(document).ready(function() {

    //$('#datetimepicker1').datetimepicker();
 



});

 $('ul.nav li.dropdown').hover(function() {
        alert('hi');
      $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeIn(500);
    }, function() {
      $(this).find('.dropdown-menu').stop(true, true).delay(200).fadeOut(500);
    });


$(window).bind("load", function() {

    // Remove splash screen after load
    $('.splash').css('display', 'none');

    // Default select checkbox
    $('.inline-form-element input').iCheck({
        checkboxClass: 'icheckbox_square-green',
        radioClass: 'iradio_square-green',
        increaseArea: '80%',
    });
});

$(window).bind("resize click", function() {

    // Add special class to minimalize page elements when screen is less than 768px
    //setBodySmall();

    // Waint until metsiMenu, collapse and other effect finish and set wrapper height
    /*setTimeout(function() {
        fixWrapperHeight();
    }, 300);*/
});

function fixWrapperHeight() {

    // Get and set current height
    var headerH = 62;
    var navigationH = $("#navigation").height();
    var contentH = $(".content").height();

    // Set new height when contnet height is less then navigation
    if (contentH < navigationH) {
        $("#wrapper").css("min-height", navigationH + 'px');
    }

    // Set new height when contnet height is less then navigation and navigation is less then window
    if (contentH < navigationH && navigationH < $(window).height()) {
        $("#wrapper").css("min-height", $(window).height() - headerH + 'px');
    }

    // Set new height when contnet is higher then navigation but less then window
    if (contentH > navigationH && contentH < $(window).height()) {
        $("#wrapper").css("min-height", $(window).height() - headerH + 'px');
    }
}


function setBodySmall() {
    if ($(this).width() < 769) {
        $('body').addClass('page-small');
    } else {
        $('body').removeClass('page-small');
        $('body').removeClass('show-sidebar');
    }
}

