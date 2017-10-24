/* ------------------------------------------------------------------------------
*
*  # Template JS core
*
*  Core JS file with default functionality configuration
*
*  Version: 1.2
*  Latest update: Dec 11, 2015
*
* ---------------------------------------------------------------------------- */

// Allow CSS transitions when page is loaded
$(window).on('load', function () {
    $('body').removeClass('no-transitions');
});


$(function () {

    // Disable CSS transitions on page load
    $('body').addClass('no-transitions');



    // ========================================
    //
    // Content area height
    //
    // ========================================


    // Calculate min height
    function containerHeight() {
        var availableHeight = $(window).height() - $('.page-container').offset().top - $('.navbar-fixed-bottom').outerHeight();

        $('.page-container').attr('style', 'min-height:' + availableHeight + 'px');
    }

    // Initialize
    containerHeight();




    // ========================================
    //
    // Heading elements
    //
    // ========================================


    // Heading elements toggler
    // -------------------------

    // Add control button toggler to page and panel headers if have heading elements
    $('.panel-heading, .page-header-content, .panel-body, .panel-footer').has('> .heading-elements').append('<a class="heading-elements-toggle"><i class="icon-more"></i></a>');


    // Toggle visible state of heading elements
    $('.heading-elements-toggle').on('click', function () {
        $(this).parent().children('.heading-elements').toggleClass('visible');
    });



    // Breadcrumb elements toggler
    // -------------------------

    // Add control button toggler to breadcrumbs if has elements
    $('.breadcrumb-line').has('.breadcrumb-elements').append('<a class="breadcrumb-elements-toggle"><i class="icon-menu-open"></i></a>');


    // Toggle visible state of breadcrumb elements
    $('.breadcrumb-elements-toggle').on('click', function () {
        $(this).parent().children('.breadcrumb-elements').toggleClass('visible');
    });




    // ========================================
    //
    // Navbar
    //
    // ========================================


    // Navbar navigation
    // -------------------------

    // Prevent dropdown from closing on click
    $(document).on('click', '.dropdown-content', function (e) {
        e.stopPropagation();
    });

    // Disabled links
    $('.navbar-nav .disabled a').on('click', function (e) {
        e.preventDefault();
        e.stopPropagation();
    });

    // Show tabs inside dropdowns
    $('.dropdown-content a[data-toggle="tab"]').on('click', function (e) {
        $(this).tab('show');
    });




    // ========================================
    //
    // Element controls
    //
    // ========================================

    // Collapse elements
    // -------------------------

    //
    // Sidebar categories
    //

    // Hide if collapsed by default
    $('.category-collapsed').children('.category-content').hide();


    // Rotate icon if collapsed by default
    $('.category-collapsed').find('[data-action=collapse]').addClass('rotate-180');


    // Collapse on click
    $('.category-title [data-action=collapse]').click(function (e) {
        e.preventDefault();
        var $categoryCollapse = $(this).parent().parent().parent().nextAll();
        $(this).parents('.category-title').toggleClass('category-collapsed');
        $(this).toggleClass('rotate-180');

        containerHeight(); // adjust page height

        $categoryCollapse.slideToggle(150);
    });


    // ========================================
    //
    // Main navigation
    //
    // ========================================


    // Main navigation
    // -------------------------

    // Add 'active' class to parent list item in all levels
    $('.navigation').find('li.active').parents('li').addClass('active');

    // Hide all nested lists
    $('.navigation').find('li').not('.active, .category-title').has('ul').children('ul').addClass('hidden-ul');

    // Highlight children links
    $('.navigation').find('li').has('ul').children('a').addClass('has-ul');

    // Add active state to all dropdown parent levels
    $('.dropdown-menu:not(.dropdown-content), .dropdown-menu:not(.dropdown-content) .dropdown-submenu').has('li.active').addClass('active').parents('.navbar-nav .dropdown:not(.language-switch), .navbar-nav .dropup:not(.language-switch)').addClass('active');



    // Main navigation tooltips positioning
    // -------------------------

    // Left sidebar
    $('.navigation-main > .navigation-header > i').tooltip({
        placement: 'right',
        container: 'body'
    });



    // Collapsible functionality
    // -------------------------

    // Main navigation
    $('.navigation-main').find('li').has('ul').children('a').on('click', function (e) {
        e.preventDefault();

        // Collapsible
        $(this).parent('li').not('.disabled').not($('.sidebar-xs').not('.sidebar-xs-indicator').find('.navigation-main').children('li')).toggleClass('active').children('ul').slideToggle(250);

        // Accordion
        if ($('.navigation-main').hasClass('navigation-accordion')) {
            $(this).parent('li').not('.disabled').not($('.sidebar-xs').not('.sidebar-xs-indicator').find('.navigation-main').children('li')).siblings(':has(.has-ul)').removeClass('active').children('ul').slideUp(250);
        }
    });


    // Alternate navigation
    $('.navigation-alt').find('li').has('ul').children('a').on('click', function (e) {
        e.preventDefault();

        // Collapsible
        $(this).parent('li').not('.disabled').toggleClass('active').children('ul').slideToggle(200);

        // Accordion
        if ($('.navigation-alt').hasClass('navigation-accordion')) {
            $(this).parent('li').not('.disabled').siblings(':has(.has-ul)').removeClass('active').children('ul').slideUp(200);
        }
    });




    // ========================================
    //
    // Sidebars
    //
    // ========================================


    // Mini sidebar
    // -------------------------

    // Toggle mini sidebar
    $('.sidebar-main-toggle').on('click', function (e) {
        e.preventDefault();

        // Toggle min sidebar class
        $('body').toggleClass('sidebar-xs');
    });



    // Sidebar controls
    // -------------------------

    // Disable click in disabled navigation items
    $(document).on('click', '.navigation .disabled a', function (e) {
        e.preventDefault();
    });


    // Adjust page height on sidebar control button click
    $(document).on('click', '.sidebar-control', function (e) {
        containerHeight();
    });


    // Hide main sidebar in Dual Sidebar
    $(document).on('click', '.sidebar-main-hide', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-main-hidden');
    });


    // Toggle second sidebar in Dual Sidebar
    $(document).on('click', '.sidebar-secondary-hide', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-secondary-hidden');
    });


    // Hide all sidebars
    $(document).on('click', '.sidebar-all-hide', function (e) {
        e.preventDefault();

        $('body').toggleClass('sidebar-all-hidden');
    });



    //
    // Opposite sidebar
    //

    // Collapse main sidebar if opposite sidebar is visible
    $(document).on('click', '.sidebar-opposite-toggle', function (e) {
        e.preventDefault();

        // Opposite sidebar visibility
        $('body').toggleClass('sidebar-opposite-visible');

        // If visible
        if ($('body').hasClass('sidebar-opposite-visible')) {

            // Make main sidebar mini
            $('body').addClass('sidebar-xs');

            // Hide children lists
            $('.navigation-main').children('li').children('ul').css('display', '');
        }
        else {

            // Make main sidebar default
            $('body').removeClass('sidebar-xs');
        }
    });


    // Hide main sidebar if opposite sidebar is shown
    $(document).on('click', '.sidebar-opposite-main-hide', function (e) {
        e.preventDefault();

        // Opposite sidebar visibility
        $('body').toggleClass('sidebar-opposite-visible');

        // If visible
        if ($('body').hasClass('sidebar-opposite-visible')) {

            // Hide main sidebar
            $('body').addClass('sidebar-main-hidden');
        }
        else {

            // Show main sidebar
            $('body').removeClass('sidebar-main-hidden');
        }
    });


    // Hide secondary sidebar if opposite sidebar is shown
    $(document).on('click', '.sidebar-opposite-secondary-hide', function (e) {
        e.preventDefault();

        // Opposite sidebar visibility
        $('body').toggleClass('sidebar-opposite-visible');

        // If visible
        if ($('body').hasClass('sidebar-opposite-visible')) {

            // Hide secondary
            $('body').addClass('sidebar-secondary-hidden');

        }
        else {

            // Show secondary
            $('body').removeClass('sidebar-secondary-hidden');
        }
    });


    // Hide all sidebars if opposite sidebar is shown
    $(document).on('click', '.sidebar-opposite-hide', function (e) {
        e.preventDefault();

        // Toggle sidebars visibility
        $('body').toggleClass('sidebar-all-hidden');

        // If hidden
        if ($('body').hasClass('sidebar-all-hidden')) {

            // Show opposite
            $('body').addClass('sidebar-opposite-visible');

            // Hide children lists
            $('.navigation-main').children('li').children('ul').css('display', '');
        }
        else {

            // Hide opposite
            $('body').removeClass('sidebar-opposite-visible');
        }
    });


    // Keep the width of the main sidebar if opposite sidebar is visible
    $(document).on('click', '.sidebar-opposite-fix', function (e) {
        e.preventDefault();

        // Toggle opposite sidebar visibility
        $('body').toggleClass('sidebar-opposite-visible');
    });



    // Mobile sidebar controls
    // -------------------------

    // Toggle main sidebar
    $('.sidebar-mobile-main-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-mobile-main').removeClass('sidebar-mobile-secondary sidebar-mobile-opposite');
    });


    // Toggle secondary sidebar
    $('.sidebar-mobile-secondary-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-mobile-secondary').removeClass('sidebar-mobile-main sidebar-mobile-opposite');
    });


    // Toggle opposite sidebar
    $('.sidebar-mobile-opposite-toggle').on('click', function (e) {
        e.preventDefault();
        $('body').toggleClass('sidebar-mobile-opposite').removeClass('sidebar-mobile-main sidebar-mobile-secondary');
    });



    // Mobile sidebar setup
    // -------------------------

    $(window).on('resize', function () {
        setTimeout(function () {
            containerHeight();

            if ($(window).width() <= 768) {

                // Add mini sidebar indicator
                $('body').addClass('sidebar-xs-indicator');

                // Place right sidebar before content
                $('.sidebar-opposite').prependTo('.page-content');
            }
            else {

                // Remove mini sidebar indicator
                $('body').removeClass('sidebar-xs-indicator');

                // Revert back right sidebar
                $('.sidebar-opposite').insertAfter('.content-wrapper');

                // Remove all mobile sidebar classes
                $('body').removeClass('sidebar-mobile-main sidebar-mobile-secondary sidebar-mobile-opposite');
            }
        }, 100);
    }).resize();




    // ========================================
    //
    // Other code
    //
    // ========================================


    // Plugins
    // -------------------------

    // Popover
    $('[data-popup="popover"]').popover();


    // Tooltip
    $('[data-popup="tooltip"]').tooltip();


    // Checkboxes/radios (Uniform)
    // ------------------------------

    // if (jQuery(".styled, .multiselect-container input").size() > 0) {
    //     // Default initialization
    //     $(".styled, .multiselect-container input").uniform({
    //         radioClass: 'choice'
    //     });
    // }
    // File input
    // if (jQuery(".file-styled").size() > 0) {
    //     $(".file-styled").uniform({
    //         fileButtonClass: 'action btn bg-blue',
    //         fileButtonHtml: '<i class="icon-file-plus"></i> Choose File'
    //     });
    // }
    // Basic select
    // Override defaults
    if (jQuery(".selectpicker").size() > 0) {
        $.fn.selectpicker.defaults = {
            iconBase: '',
            tickIcon: 'icon-checkmark3'
        }
        $('.selectpicker').selectpicker();
    }

    //DatePicker 
    if (jQuery(".datepicker").size() > 0) {
        jQuery('.datepicker').datepicker({
            format: 'dd/mm/yyyy',
            autoclose: true,
            todayHighlight: true,
            startDate: '01/01/1900',
            clearBtn: true
        })
    }

    //Accordian
    function initAccordion() {
        var oAccordion = jQuery('.accordion_container');
        if (oAccordion.size() > 0) {
            var menu_ul = jQuery('.accordion_container > li > div.accordion_content'), menu_a = jQuery('.accordion_container > li > a.anchor'), default_open_slide = jQuery('.accordion_container > li > div.default_open_slide');
            menu_ul.hide();
            default_open_slide.show();
            menu_a.click(function (e) {
                e.preventDefault();
                if (!jQuery(this).hasClass('active')) {
                    menu_a.removeClass('active');
                    menu_ul.filter(':visible').slideUp('normal');
                    jQuery(this).addClass('active').next().stop(true, true).slideDown('normal');
                } else {
                    jQuery(this).removeClass('active');
                    jQuery(this).next().stop(true, true).slideUp('normal');
                }
            });
        }
    }
    initAccordion();
});
var SITEForms = {
    onlyInteger: function () {
        jQuery('.only-integer').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            //if (charCode > 31 && (charCode < 48 || charCode > 57) && charCode != 37 && charCode != 39 && charCode != 46) {
            if ((charCode < 48 || charCode > 57) && charCode != 8 && charCode != 46 && charCode != 9 && charCode != 37 && charCode != 39) {
                return false;
            }
            return true;
        });
    },
    onlyDigit: function () {
        jQuery('.only-digit').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if ((charCode < 48 || charCode > 57) && charCode != 8 && charCode != 9 && charCode != 46 && charCode != 37 && charCode != 39) {
                return false;
            }
            return true;
        });
    },
    onlyFloat: function () {
        jQuery('.only-float').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 59) && charCode != 37 && charCode != 39 && charCode != 46) {
                return false;
            } // prevent if not number/dot

            if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                return false;
            } // prevent if already dot
            return true;
        });
    },
    onlyAlphaDigit: function () {
        jQuery('.only-alpha-digit').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 37 || charCode == 43 || charCode == 60 || charCode == 62 || charCode == 63 || charCode == 91 || charCode == 91 || charCode == 125 || charCode == 123 || charCode == 92 || charCode == 64 || charCode == 126 || charCode == 33 || charCode == 35 || charCode == 36 || charCode == 94 || charCode == 38 || charCode == 42 || charCode == 47) {
                return false;
            } // prevent if not number/dot
            return true;
        });
    },

    onlyAlphaDigit_1: function () {
        jQuery('.only-alpha-digit_1').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 40 || charCode == 37 || charCode == 16 || charCode == 43 || charCode == 60 || charCode == 62 || charCode == 63 || charCode == 91 || charCode == 91 || charCode == 125 || charCode == 123 || charCode == 92 || charCode == 64 || charCode == 126 || charCode == 33 || charCode == 35 || charCode == 36 || charCode == 94 || charCode == 38 || charCode == 42 || charCode == 47) {
                return false;
            } // prevent if not number/dot
            return true;
        });
    },
    email: function () {
        jQuery('.email').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode == 43 || charCode == 60 || charCode == 61 || charCode == 124 || charCode == 58 || charCode == 34 || charCode == 39 || charCode == 59 || charCode == 62 || charCode == 40 || charCode == 41 || charCode == 37 || charCode == 63 || charCode == 91 || charCode == 91 || charCode == 125 || charCode == 123 || charCode == 92 || charCode == 126 || charCode == 33 || charCode == 35 || charCode == 36 || charCode == 94 || charCode == 38 || charCode == 42 || charCode == 47) {
                return false;
            } // prevent if not number/dot

            if (charCode == 46 && $(this).val().indexOf('.') != -1) {
                //return false;
            } // prevent if already dot
            return true;
        });
    },
    onlyDecimalDigit: function () {
        jQuery('.only-alpha-decimal-digit_1').keypress(function (event) {
            var charCode = (event.which) ? event.which : event.keyCode;
            if (charCode != 46 && charCode > 31

              && (charCode < 48 || charCode > 57))

                return false;

            return true;
        });
    }




}

//http://www.asquare.net/javascript/tests/KeyCode.html

$(window).on('load', function () {
    if (jQuery('.i-checks').size() > 0) {
        function tslChecks() {
            jQuery('.i-checks').iCheck({
                checkboxClass: 'icheckbox_square-green',
                radioClass: 'iradio_square-green',
                increaseArea: '10%' // optional
            });
        }
        tslChecks();
    }

    if (jQuery('.only-float').size() > 0) {
        SITEForms.onlyFloat();
    }

    if (jQuery('.only-integer').size() > 0) {
        SITEForms.onlyInteger();
    }

    if (jQuery('.only-digit').size() > 0) {
        SITEForms.onlyDigit();
    }

    if (jQuery('.tsl-phone').size() > 0) {
        $(".tsl-phone").mask("9999999999");
    }

    if (jQuery('.tsl-zip-code').size() > 0) {
        $(".tsl-zip-code").mask("999999");
    }

    if (jQuery('.only-alpha-digit').size() > 0) {
        SITEForms.onlyAlphaDigit();
    }

    if (jQuery('.only-alpha-digit_1').size() > 0) {
        SITEForms.onlyAlphaDigit_1();
    }

    if (jQuery('.email').size() > 0) {
        SITEForms.email();
    }
    if (jQuery('.only-alpha-digit_1').size() > 0) {
        SITEForms.onlyAlphaDigit_1();
    }

    if (jQuery('.only-alpha-decimal-digit_1').size() > 0) {
        SITEForms.onlyDecimalDigit();
    }



    if (jQuery('.file-styled').size() > 0) {
        $(document).on('change', '.file-styled:file', function () {
            var input = $(this),
                numFiles = input.get(0).files ? input.get(0).files.length : 1,
                label = input.val().replace(/\\/g, '/').replace(/.*\//, '');
            input.trigger('fileselect', [numFiles, label]);

            var fileName = numFiles > 1 ? numFiles + ' files selected' : label;

            input.parents('.input-group').find(':text').val(fileName);

            console.log(fileName)
        });
    }


});

//Custom Notifications Box
function initNotifications(N_type, N_alert, N_alert_title) {
    var HTML = '<div class="tsl-notif ' + N_type + '"><a class="tsl-notif-close">×</a><div class="noti-header">' + N_alert_title + '</div><div class="jGrowl-message">' + N_alert + '</div></div>'
    jQuery('#tsl_notifications').html(HTML).fadeIn(100);
    setTimeout(function () { $("#tsl_notifications").hide(); }, 2000);
}

//Custom Conformation Box yes or no
function initConformation(CYF, CNF, CText, C_Title, CparentId) {
    var HTML = '<div class="tsl-Conf"><div class="conf-header">' + C_Title + '</div><div class="conf-message">' + CText + '</div><div class="conf-action clearfix"><a onclick="' + CNF + '" class="Conf-close pull-left btn btn-default">NO</a><a onclick="' + CYF + '" class="Conf-close pull-right btn btn-primary">Yes</a></div></div>'
    jQuery(CparentId).addClass('Conf-open');
    jQuery(CparentId).append(HTML).fadeIn(500);

    jQuery('.Conf-close').on("click", function () {
        jQuery(this).closest(".tsl-Conf").fadeOut(0);
        jQuery(CparentId).removeClass('Conf-open');
    });
}




$(document).on("click", function (event) {
    $(event.target).closest(".tsl-notif").fadeOut(500);
    //$(event.target).closest(".Conf-close").fadeOut(500);
    $('.selectpicker.parsley-error').closest(".btn-default").fadeOut(500);

});


/**
 * Multiselect
 */
/* ------------------------------------------------------------------------------
*
*  # Bootstrap multiselect
*
*  Specific JS code additions for form_multiselect.html page
*
*  Version: 1.1
*  Latest update: Oct 20, 2015
*
* ---------------------------------------------------------------------------- */

$(function () {


    // Basic examples
    // ------------------------------

    // Basic initialization
    $('.multiselect').multiselect({
        onChange: function () {
            $.uniform.update();
        }
    });


    // Limit options number
    $('.multiselect-number').multiselect({
        numberDisplayed: 1
    });


    // Custom empty text
    $('.multiselect-nonselected-text').multiselect({
        nonSelectedText: 'Please choose'
    });


    // Select All option
    $('.multiselect-select-all').multiselect({
        includeSelectAllOption: true,
        onSelectAll: function () {
            $.uniform.update();
        }
    });


    // Enable filtering
    $('.multiselect-filtering').multiselect({
        enableFiltering: true,
        templates: {
            filter: '<li class="multiselect-item multiselect-filter"><i class="icon-search4"></i> <input class="form-control" type="text"></li>'
        },
        onChange: function () {
            $.uniform.update();
        }
    });


    // Select All and Filtering features
    $('.multiselect-select-all-filtering').multiselect({
        includeSelectAllOption: true,
        enableFiltering: true,
        templates: {
            filter: '<li class="multiselect-item multiselect-filter"><i class="icon-search4"></i> <input class="form-control" type="text"></li>'
        },
        onSelectAll: function () {
            $.uniform.update();
        }
    });


    // Linked button style
    $('.multiselect-link').multiselect({
        buttonClass: 'btn btn-link'
    });


    // Custom button color
    $('.multiselect-custom-color').multiselect({
        buttonClass: 'btn bg-teal-400'
    });


    // Clickable optgroups
    $('.multiselect-clickable-groups').multiselect({
        enableClickableOptGroups: true,
        onChange: function () {
            $.uniform.update();
        }
    });


    // Disable if empty
    $('.multiselect-disable-empty').multiselect({
        disableIfEmpty: true
    });


    // Menu background color
    $('.multiselect-menu-bg-color').multiselect({
        templates: {
            ul: '<ul class="multiselect-container bg-teal-400 dropdown-menu"></ul>'
        }
    });


    // Combined colors
    $('.multiselect-combine-all').multiselect({
        buttonClass: 'btn bg-slate',
        templates: {
            ul: '<ul class="multiselect-container bg-slate dropdown-menu"></ul>'
        }
    });


    // Full featured example
    $('.multiselect-full-featured').multiselect({
        includeSelectAllOption: true,
        enableFiltering: true,
        templates: {
            filter: '<li class="multiselect-item multiselect-filter"><i class="icon-search4"></i> <input class="form-control" type="text"></li>'
        },
        onSelectAll: function () {
            $.uniform.update();
        }
    });


    // With max height
    $('.multiselect-max-height').multiselect({
        maxHeight: 200
    });


    // Prevent deselect
    $('.multiselect-prevent-deselect').multiselect({
        onChange: function (option, checked) {
            if (checked === false) {
                $('.multiselect-prevent-deselect').multiselect('select', option.val());
                $.uniform.update();
            }
        }
    });


    // Remove active option class
    $('.multiselect-no-active-class').multiselect({
        selectedClass: null
    });



    // Contextual alternatives
    // ------------------------------

    // Primary
    $('.multiselect-primary').multiselect({
        buttonClass: 'btn btn-primary'
    });

    // Danger
    $('.multiselect-danger').multiselect({
        buttonClass: 'btn btn-danger'
    });

    // Success
    $('.multiselect-success').multiselect({
        buttonClass: 'btn btn-success'
    });

    // Warning
    $('.multiselect-warning').multiselect({
        buttonClass: 'btn btn-warning'
    });

    // Info
    $('.multiselect-info').multiselect({
        buttonClass: 'btn btn-info'
    });



    // Height sizing
    // ------------------------------

    // Large
    $('.multiselect-lg').multiselect({
        buttonClass: 'btn btn-default btn-lg'
    });

    // Small
    $('.multiselect-sm').multiselect({
        buttonClass: 'btn btn-default btn-sm'
    });

    // Mini
    $('.multiselect-xs').multiselect({
        buttonClass: 'btn btn-default btn-xs'
    });



    // Width sizing
    // ------------------------------

    // Full width
    $('.multiselect-full').multiselect({
        buttonWidth: '100%'
    });

    // Percentage width
    $('.multiselect-custom-percents').multiselect({
        buttonWidth: '80%'
    });

    // Auto width
    $('.multiselect-auto').multiselect({
        buttonWidth: 'auto'
    });



    // Events
    // ------------------------------

    // onChange
    $('.multiselect-onchange-notice').multiselect({
        buttonClass: 'btn btn-info',
        onChange: function (element, checked) {
            $.uniform.update();
            new PNotify({
                text: '<code>onChange</code> callback fired.',
                addclass: 'bg-teal alert-styled-left'
            });
        }
    });


    // onChange desktop
    $('.multiselect-onchange-desktop').multiselect({
        buttonClass: 'btn btn-info',
        onChange: function (element, checked) {
            $.uniform.update();
            PNotify.desktop.permission();
            (new PNotify({
                title: 'Desktop Notice',
                text: 'onChange callback desktop notification.',
                desktop: {
                    desktop: true,
                    addclass: 'bg-blue',
                    icon: 'assets/images/pnotify/info.png'
                }
            })).get().click(function (e) {
                if ($('.ui-pnotify-closer, .ui-pnotify-sticker, .ui-pnotify-closer *, .ui-pnotify-sticker *').is(e.target)) return;
                alert('Hey! You clicked the desktop notification!');
            });
        }
    });


    // onShow
    $('.multiselect-show-event').multiselect({
        buttonClass: 'btn btn-info',
        onDropdownShow: function () {
            new PNotify({
                text: '<code>onDropdownShow</code> event fired.',
                addclass: 'bg-teal alert-styled-left'
            });
        }
    });


    // onHide
    $('.multiselect-hide-event').multiselect({
        buttonClass: 'btn btn-info',
        onDropdownHide: function () {
            new PNotify({
                text: '<code>onDropdownHide</code> event fired.',
                addclass: 'bg-teal alert-styled-left'
            });
        }
    });



    // Methods
    // ------------------------------

    //
    // Create and destroy
    //

    // Initialize
    $('.multiselect-method-destroy').multiselect();

    // Destroy
    $('.multiselect-destroy-button').on('click', function () {
        $('.multiselect-method-destroy').multiselect('destroy');
    });

    // Create
    $('.multiselect-create-button').on('click', function () {
        $('.multiselect-method-destroy').multiselect({
            onInitialized: function (select, container) {
                $(".styled, .multiselect-container input").uniform({ radioClass: 'choice' });
            }
        });
    });


    //
    // Refresh
    //

    // Initialize
    $('.multiselect-method-refresh').multiselect();

    // Select option
    $('.multiselect-select-button').on('click', function () {

        $('option[value="tomatoes"]', $('.multiselect-method-refresh')).attr('selected', 'selected');
        $('option[value="tomatoes"]', $('.multiselect-method-refresh')).prop('selected', true);

        $('option[value="mushrooms"]', $('.multiselect-method-refresh')).prop('selected', true);
        $('option[value="mushrooms"]', $('.multiselect-method-refresh')).attr('selected', 'selected');

        $('option[value="onions"]', $('.multiselect-method-refresh')).prop('selected', true);
        $('option[value="onions"]', $('.multiselect-method-refresh')).attr('selected', 'selected');

        alert('Selected Tomatoes, Mushrooms and Onions.');
    });

    // Deselect
    $('.multiselect-deselect-button').on('click', function () {
        $('option', $('.multiselect-method-refresh')).each(function (element) {
            $(this).removeAttr('selected').prop('selected', false);
        });
    });

    // Refresh
    $('.multiselect-refresh-button').on('click', function () {
        $('.multiselect-method-refresh').multiselect('refresh');
        $.uniform.update();
    });


    //
    // Rebuild
    //

    // Initialize
    $('.multiselect-method-rebuild').multiselect();

    // Add option
    $('.multiselect-add-button').on('click', function () {
        $('.multiselect-method-rebuild').append('<option value="add1">Addition 1</option><option value="add2">Addition 2</option><option value="add3">Addition 3</option>');
    });

    // Remove option
    $('.multiselect-delete-button').on('click', function () {
        $('option[value="add1"]', $('.multiselect-method-rebuild')).remove();
        $('option[value="add2"]', $('.multiselect-method-rebuild')).remove();
        $('option[value="add3"]', $('.multiselect-method-rebuild')).remove();
    });

    // Rebuild menu
    $('.multiselect-rebuild-button').on('click', function () {
        $('.multiselect-method-rebuild').multiselect('rebuild');
        $(".multiselect-container input").uniform({ radioClass: 'choice' });
    });


    //
    // Select
    //

    // Initialize
    $('.multiselect-method-select').multiselect();

    // Select first option
    $('.multiselect-select-cheese-button').on('click', function () {
        $('.multiselect-method-select').multiselect('select', 'cheese'),
        $.uniform.update();
    });

    // Select second option
    $('.multiselect-select-onions-button').on('click', function () {
        $('.multiselect-method-select').multiselect('select', 'onions'),
        $.uniform.update();
    });


    //
    // Deselect
    //

    // Initialize
    $('.multiselect-method-deselect').multiselect();

    // Deselect first option
    $('.multiselect-deselect-cheese-button').on('click', function () {
        $('.multiselect-method-deselect').multiselect('deselect', 'cheese'),
        $.uniform.update();
    });

    // Deselect second option
    $('.multiselect-deselect-onions-button').on('click', function () {
        $('.multiselect-method-deselect').multiselect('deselect', 'onions'),
        $.uniform.update();
    });


    //
    // Disable
    //

    // Initialize
    $('.multiselect-method-disable').multiselect();

    // Enable
    $('.multiselect-enable1-button').on('click', function () {
        $('.multiselect-method-disable').multiselect('enable');
    });

    // Disable
    $('.multiselect-disable1-button').on('click', function () {
        $('.multiselect-method-disable').multiselect('disable');
    });


    //
    // Enable
    //

    // Initialize
    $('.multiselect-method-enable').multiselect({
        buttonContainer: '<div class="btn-group dropup" />',
    });

    // Enable
    $('.multiselect-enable2-button').on('click', function () {
        $('.multiselect-method-enable').multiselect('enable');
    });

    // Disable
    $('.multiselect-disable2-button').on('click', function () {
        $('.multiselect-method-enable').multiselect('disable');
    });




    // Advanced examples
    // ------------------------------

    // Simulate selections
    $('.multiselect-simulate-selections').multiselect({
        onChange: function (option, checked) {
            var values = [];
            $('.multiselect-simulate-selections option').each(function () {
                if ($(this).val() !== option.val()) {
                    values.push($(this).val());
                }
            });

            $('.multiselect-simulate-selections').multiselect('deselect', values), $.uniform.update();
        }
    });


    // Close dropdown automaticallywhen options are selected
    $('.multiselect-close-dropdown').multiselect({
        onChange: function (option, checked) {
            var selected = 0;
            $('option', $('.multiselect-close-dropdown')).each(function () {
                if ($(this).prop('selected')) {
                    selected++;
                }
            });

            if (selected >= 3) {
                $('.multiselect-close-dropdown').siblings('div').children('ul').dropdown('toggle');
            }
        }
    });


    // Templates
    $('.multiselect-templates').multiselect({
        buttonContainer: '<div class="btn-group dropup" />',
        templates: {
            divider: '<div class="divider" data-role="divider"></div>'
        }
    });


    //
    // Display values
    //

    // Initialize
    $('.multiselect-display-values').multiselect();

    // Select options
    $('.multiselect-display-values-select').on('click', function () {
        $('.multiselect-display-values').multiselect('select', 'cheese');
        $('.multiselect-display-values').multiselect('select', 'tomatoes');
        $.uniform.update();
    });

    // Deselect options
    $('.multiselect-display-values-deselect').on('click', function () {
        $('.multiselect-display-values').multiselect('deselect', 'cheese');
        $('.multiselect-display-values').multiselect('deselect', 'tomatoes');
        $.uniform.update();
    });

    // Display values
    $('.multiselect-show-values').on('click', function () {
        $('.values-area').text('Selected: ' + $('.multiselect-display-values').val().join(', ')).addClass('alert alert-info');
    });


    //
    // Toggle selection
    //

    // Select all/Deselect all
    function multiselect_selected($el) {
        var ret = true;
        $('option', $el).each(function (element) {
            if (!!!$(this).prop('selected')) {
                ret = false;
            }
        });
        return ret;
    }
    function multiselect_selectAll($el) {
        $('option', $el).each(function (element) {
            $el.multiselect('select', $(this).val());
        });
    }
    function multiselect_deselectAll($el) {
        $('option', $el).each(function (element) {
            $el.multiselect('deselect', $(this).val());
        });
    }
    function multiselect_toggle($el, $btn) {
        if (multiselect_selected($el)) {
            multiselect_deselectAll($el);
            $btn.text("Select All");
        }
        else {
            multiselect_selectAll($el);
            $btn.text("Deselect All");
        }
    }

    // Initialize
    $('.multiselect-toggle-selection').multiselect();

    // Toggle selection on button click
    $(".multiselect-toggle-selection-button").click(function (e) {
        e.preventDefault();
        multiselect_toggle($(".multiselect-toggle-selection"), $(this));
        $.uniform.update();
    });


    //
    // Order options
    //

    var orderCount = 0;

    // Initialize
    $('.multiselect-order-options').multiselect({
        buttonText: function (options) {
            if (options.length == 0) {
                return 'None selected';
            }
            else if (options.length > 3) {
                return options.length + ' selected';
            }
            else {
                var selected = [];
                options.each(function () {
                    selected.push([$(this).text(), $(this).data('order')]);
                });

                selected.sort(function (a, b) {
                    return a[1] - b[1];
                });

                var text = '';
                for (var i = 0; i < selected.length; i++) {
                    text += selected[i][0] + ', ';
                }

                return text.substr(0, text.length - 2);
            }
        },

        onChange: function (option, checked) {
            if (checked) {
                orderCount++;
                $(option).data('order', orderCount);
            }
            else {
                $(option).data('order', '');
            }
        }
    });

    // Order selected options on button click
    $('.multiselect-order-options-button').on('click', function () {
        var selected = [];
        $('.multiselect-order-options option:selected').each(function () {
            selected.push([$(this).val(), $(this).data('order')]);
        });

        selected.sort(function (a, b) {
            return a[1] - b[1];
        });

        var text = '';
        for (var i = 0; i < selected.length; i++) {
            text += selected[i][0] + ', ';
        }
        text = text.substring(0, text.length - 2);

        alert(text);
    });


    //
    // Reset selections
    //

    // Initialize
    $('.multiselect-reset').multiselect();

    // Reset using reset button
    $('#multiselect-reset-form').on('reset', function () {
        $('.multiselect-reset option:selected').each(function () {
            $(this).prop('selected', false);
        })

        $('.multiselect-reset').multiselect('refresh');
        $.uniform.update();
    });



    // Related plugins
    // ------------------------------

    // Styled checkboxes and radios
    $(".styled, .multiselect-container input").uniform({ radioClass: 'choice' });

});

/**
 * DataTable
 */
/* ------------------------------------------------------------------------------
*
*  # Datatable sorting
*
*  Specific JS code additions for datatable_sorting.html page
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */

$(function () {


    // Table setup
    // ------------------------------

    // Setting datatable defaults
    $.extend($.fn.dataTable.defaults, {
        autoWidth: false,
        columnDefs: [{
            orderable: false,
            width: '100px',
            targets: [5]
        }],
        dom: '<"datatable-header"fl><"datatable-scroll"t><"datatable-footer"ip>',
        language: {
            search: '<span>Filter:</span> _INPUT_',
            lengthMenu: '<span>Show:</span> _MENU_',
            paginate: { 'first': 'First', 'last': 'Last', 'next': '&rarr;', 'previous': '&larr;' }
        },
        drawCallback: function () {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').addClass('dropup');
        },
        preDrawCallback: function () {
            $(this).find('tbody tr').slice(-3).find('.dropdown, .btn-group').removeClass('dropup');
        }
    });


    // Default ordering example
    $('.datatable-sorting').DataTable({
        order: [3, "desc"]
    });


    // Multi column ordering
    $('.datatable-multi-sorting').DataTable({
        columnDefs: [{
            targets: [0],
            width: '100px',
            orderData: [0, 1]
        }, {
            targets: [1],
            orderData: [1, 0]
        }, {
            targets: [4],
            orderData: [4, 0]
        }, {
            orderable: false,
            width: '100px',
            targets: [5]
        }]
    });

    $(document).ready(function () {
        var t = $('#companyUser_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0, 6]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1,2,3,4,5]
                    }
                },
                'pageLength'
            ],
            
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    //$('#tbCustomer').DataTable({
    //    "order": [[3, "desc"]],
    //    "fnRowCallback": function (nRow, aData, iDisplayIndex) {
    //        $("td:first", nRow).html(iDisplayIndex + 1);
    //        return nRow;
    //    },
    //    columnDefs: [{
    //        orderable: false,
    //        width: '1%',
    //        targets: [0]
    //    }, {
    //        orderable: false,
    //        width: '1%',
    //        targets: [0]
    //    }]
    //});


    $(document).ready(function () {
        var t = $('#tbCustomer').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                'pageLength'
            ],
            
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#tbCustomerChange').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1,2,3,4,5,6,7]
                    }
                },
                'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#inventory_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
               {
                   extend: 'excelHtml5',
                   exportOptions: {
                       columns: [1,2,3,4,5]
                   }
               },
                'pageLength'
            ],
            
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#inventory_admin_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1,2,3,4,5]
                    }
                },
                  'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#investment_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {                
                    extend: 'excelHtml5',
                    text: 'Excel',
                    exportOptions: {
                        columns: [1,2,3,4,5]
                    }
                },
                'pageLength'
            ],
           
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#withdrawal_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0, 8]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1,2,3,4,5,6,7,8]
                    }
                },
                'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#custsupport_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0, 5]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                {
                    extend: 'excelHtml5',
                    exportOptions: {
                        columns: [1,2,3,4]
                    }
                },
                'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });


    $(document).ready(function () {
        var t = $('#investmentslists_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
              [10, 25, 50, -1],
              ['10', '25', '50', 'Show all']
            ],
            buttons: [
              'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#notification_list_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
              'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });

    $(document).ready(function () {
        var t = $('#saleslists_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
              'pageLength'
            ],
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });


    $(document).ready(function () {
        var t = $('#cust_ticketraise_data_table').DataTable({
            "columnDefs": [{
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": 0
            }, {
                "searchable": false,
                "width": '1%',
                "orderable": false,
                "targets": [0, 4]
            }],
            dom: 'Bfrtip',
            lengthMenu: [
            [10, 25, 50, -1],
            ['10', '25', '50', 'Show all']
            ],
            buttons: [
                'excelHtml5',
                'pageLength'
            ],
           
        });

        t.on('order.dt search.dt', function () {
            t.column(0, { search: 'applied', order: 'applied' }).nodes().each(function (cell, i) {
                cell.innerHTML = i + 1;
            });
        }).draw();
    });


    // Complex headers with sorting
    $('.datatable-complex-header').DataTable({
        columnDefs: []
    });


    // Sequence control
    $('.datatable-sequence-control').dataTable({
        "aoColumns": [
            null,
            null,
            { "orderSequence": ["asc"] },
            { "orderSequence": ["desc", "asc", "asc"] },
            { "orderSequence": ["desc"] },
            null
        ]
    });



    // External table additions
    // ------------------------------

    // Add placeholder to the datatable filter option
    //$('.dataTables_filter input[type=search]').attr('placeholder', 'Type to filter...');


    // Enable Select2 select for the length option
    /*use in future to select multiple options $('.dataTables_length select').select2({
        minimumResultsForSearch: Infinity,
        width: 'auto'
    });*/

});

/*load Datatable at report page*/
function loadReportTable() {
    $('#reportsData').DataTable({
        columnDefs: [{
            orderable: false,
            targets: [0]
        }, {
            orderable: false,
            targets: [0]
        }],
        dom: 'Bfrtip',
        lengthMenu: [
        [10, 25, 50, -1],
        ['10', '25', '50', 'Show all']
        ],
        buttons: [
            {
                extend: 'excelHtml5',
            },
            'pageLength',

        ]
    });
}

/* ------------------------------------------------------------------------------
*
*  # WYSIHTML5 editor
*
*  Specific JS code additions for editor_wysihtml5.html page
*
*  Version: 1.0
*  Latest update: Aug 1, 2015
*
* ---------------------------------------------------------------------------- */

$(function () {

    // Default initialization
    $('.wysihtml5-default').wysihtml5({
        parserRules: wysihtml5ParserRules,
        "link": false, // Button to insert a link. Default true
        "image": false, // Button to insert an image. Default true,
        "color": true, // Button to change color of font
        "stylesheets": ['../Content/css/core.css']
    });


    // Simple toolbar
    $('.wysihtml5-min').wysihtml5({
        parserRules: wysihtml5ParserRules,
        "font-styles": true, // Font styling, e.g. h1, h2, etc. Default true
        "emphasis": true, // Italics, bold, etc. Default true
        "lists": true, // (Un)ordered lists, e.g. Bullets, Numbers. Default true
        "html": false, // Button which allows you to edit the generated HTML. Default false
        "link": false, // Button to insert a link. Default true
        "image": false, // Button to insert an image. Default true,
        "action": false, // Undo / Redo buttons,
        "color": true // Button to change color of font
    });


    // Editor events
    $('.wysihtml5-init').on('click', function () {
        $(this).off('click').addClass('disabled');
        $('.wysihtml5-events').wysihtml5({
            parserRules: wysihtml5ParserRules,
            events: {
                load: function () {
                    $.jGrowl('Editor has been loaded.', { theme: 'bg-slate-700', header: 'WYSIHTML5 loaded' });
                },
                change_view: function () {
                    $.jGrowl('Editor view mode has been changed.', { theme: 'bg-slate-700', header: 'View mode' });
                }
            }
        });
    });




    // Style form components
    $('.styled').uniform();

});
