var app = angular.module('myApp', ['ngRoute'])
.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
      .when('/index', {
          templateUrl: 'views/index.html',
          controller: 'indexController'
      })
      .otherwise({
          redirectTo: '/index'
      });
}])
    .factory('initLoader', function () {
        return {
            testLader: function (GetData) {
                var temp = [];
                var data = GetData;
                var slide_intervalset = data.delay;

                jQuery(function ($) {

                    var i = 3000;
                    $.supersized({

                        // Functionality
                        slideshow: 1,			// Slideshow on/off
                        autoplay: 1,			// Slideshow starts playing automatically
                        start_slide: 1,			// Start slide (0 is random)
                        stop_loop: 0,			// Pauses slideshow on last slide
                        random: 0,			// Randomize slide order (Ignores start slide)
                        slide_interval: slide_intervalset,// Length between transitions
                        transition: 6, 			// 0-None, 1-Fade, 2-Slide Top, 3-Slide Right, 4-Slide Bottom, 5-Slide Left, 6-Carousel Right, 7-Carousel Left
                        transition_speed: 1000,		// Speed of transition
                        new_window: 1,			// Image links open in new window/tab
                        pause_hover: 0,			// Pause slideshow on hover
                        keyboard_nav: 1,			// Keyboard navigation on/off
                        performance: 1,			// 0-Normal, 1-Hybrid speed/quality, 2-Optimizes image quality, 3-Optimizes transition speed // (Only works for Firefox/IE, not Webkit)
                        image_protect: 1,			// Disables image dragging and right click with Javascript

                        // Size & Position						   
                        min_width: 0,			// Min width allowed (in pixels)
                        min_height: 0,			// Min height allowed (in pixels)
                        vertical_center: 1,			// Vertically center background
                        horizontal_center: 1,			// Horizontally center background
                        fit_always: 0,			// Image will never exceed browser width or height (Ignores min. dimensions)
                        fit_portrait: 1,			// Portrait images will not exceed browser height
                        fit_landscape: 0,			// Landscape images will not exceed browser width

                        // Components							
                        slide_links: 'blank',	// Individual links for each slide (Options: false, 'num', 'name', 'blank')
                        thumb_links: 1,			// Individual thumb links for each slide
                        thumbnail_navigation: 0,			// Thumbnail navigation
                        //slides: [			// Slideshow Images
                        //                           { image: 'img/bg/555174-1920x1200.jpg', title: 'Image Credit: Maria Kazvan', thumb: 'img/bg/555174-1920x1200.jpg', url: '' },
                        //                           { image: 'img/bg/555174-1920x1200.jpg', title: 'Image Credit: Maria Kazvan', thumb: 'img/bg/555174-1920x1200.jpg', url: '' },
                        //                           { image: 'img/bg/555174-1920x1200.jpg', title: 'Image Credit: Maria Kazvan', thumb: 'img/bg/555174-1920x1200.jpg', url: '' },

                        //],

                        slides: GetData,
                        // Theme Options			   
                        progress_bar: 1,			// Timer for each slide							
                        mouse_scrub: 0
                    });
                });
            },

            facebook: function (fb) {
                (function (d, s, id) {
                    var js, fjs = d.getElementsByTagName(s)[0];
                    if (d.getElementById(id)) return;
                    js = d.createElement(s); js.id = id;
                    js.src = "//connect.facebook.net/en_US/sdk.js#xfbml=1&version=v2.4&appId=" + fb;
                    fjs.parentNode.insertBefore(js, fjs);
                }(document, 'script', 'facebook-jssdk'));
            },

            //instagrams: function (ig) {
            //    function GetScripts(ig) {
            //        scripts = [];
            //        host = 'http://tsatchi.com';
            //        js = document.createElement('script');
            //        js.src = host + "/apps/instagram-widget-builder/b/" + ig;
            //        scripts[0] = js;
            //        for (i = 0; i < scripts.length; i++) { document.getElementsByTagName("HEAD")[0].appendChild(scripts[i]); }
            //    }
            //    GetScripts(ig);
            //},
        }

    }).controller('mainController', function ($scope) {
        $scope.message = "Main Content";
        $scope.config = {
            //api: 'http://localhost:3338'
            api: 'http://104.215.253.235'
            //api: 'http://eddee.cloudapp.net/eddee'

        };
    });

