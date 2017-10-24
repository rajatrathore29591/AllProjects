var app = angular.module('myAdminApp', ['ngCookies', 'textAngular', 'naif.base64', 'angularUtils.directives.dirPagination', 'ngSanitize', 'ImageCropper', 'ui.router'])
.run(function ($rootScope) {
    // $rootScope.isLogged = 0;
})
.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $stateProvider
       .state('Login', {
           url: '/Login',
           templateUrl: 'views/Login.html',
           controller: 'loginController',
       })

       .state('Main', {
           url: '/Main',
           templateUrl: 'views/Main.html',
           controller: 'masterController',
           abstract: true,
       })

       .state('Main.Dashboard', {
           url: '/Dashboard',
           templateUrl: 'views/Dashboard.html',
           controller: 'dashBoardController'
       })

       .state('Main.VendorList', {
           url: '/VendorList',
           templateUrl: 'views/VendorList.html',
           controller: 'vendorListController'
       })

       .state('Main.Vendor', {
           url: '/Vendor/:id',
           templateUrl: 'views/Vendor.html',
           controller: 'vendorController'
       })

       .state('Main.About', {
           url: '/About',
           templateUrl: 'views/About.html',
           controller: 'aboutController'
       })

       .state('Main.UpdateAbout', {
           url: '/UpdateAbout/:id',
           templateUrl: 'views/UpdateAbout.html',
           controller: 'updateAboutController'
       })

       .state('Main.PrivacyPolicy', {
           url: '/PrivacyPolicy',
           templateUrl: 'views/PrivacyPolicy.html',
           controller: 'privacyPolicyController'
       })

       .state('Main.UpdatePrivacyPolicy', {
           url: '/UpdatePrivacyPolicy/:id',
           templateUrl: 'views/UpdatePrivacyPolicy.html',
           controller: 'updatePrivacyPolicyController'
       })

       .state('Main.TermCondition', {
           url: '/TermCondition',
           templateUrl: 'views/TermCondition.html',
           controller: 'termConditionController'
       })

       .state('Main.UpdateTermCondition', {
           url: '/UpdateTermCondition/:id',
           templateUrl: 'views/UpdateTermCondition.html',
           controller: 'updateTermConditionController'
       })


       .state('Main.ManageActivePromotion', {
           url: '/ManageActivePromotion',
           templateUrl: 'views/ManageActivePromotion.html',
           controller: 'manageActivePromotionController'
       })

       .state('Main.CreatePromotions', {
           url: '/CreatePromotions/:id',
           templateUrl: 'views/CreatePromotions.html',
           controller: 'createPromotionsController'
       })

       .state('Main.PointReward', {
           url: '/PointReward',
           templateUrl: 'views/PointReward.html',
           controller: 'pointRewardController'
       })

       .state('Main.LandingPage', {
           url: '/LandingPage',
           templateUrl: 'views/LandingPage.html',
           controller: 'landingPageController'
       })

       .state('Main.HowItWork', {
           url: '/HowItWork',
           templateUrl: 'views/HowItWork.html',
           controller: 'howItWorkController'
       })

       .state('Main.FlashBanner', {
           url: '/FlashBanner',
           templateUrl: 'views/FlashBanner.html',
           controller: 'flashBannerController'
       })

       .state('Main.Test', {
           url: '/Test',
           templateUrl: 'views/Test.html',
           controller: 'testController'
       })

       .state('Main.PlatinumReward', {
           url: '/PlatinumReward/:id',
           templateUrl: 'views/PlatinumReward.html',
           controller: 'platinumMemberController'
       })

       .state('Main.PlatinumRewardList', {
           url: '/PlatinumRewardList',
           templateUrl: 'views/PlatinumRewardList.html',
           controller: 'platinumMemberListController'
       })

       .state('Main.GoldReward', {
           url: '/GoldReward/:id',
           templateUrl: 'views/GoldReward.html',
           controller: 'goldMemberController'
       })

       .state('Main.GoldRewardList', {
           url: '/GoldRewardList',
           templateUrl: 'views/GoldRewardList.html',
           controller: 'goldMemberListController'
       })

       .state('Main.Analytics_ActivePromotion', {
           url: '/Analytics_ActivePromotion',
           templateUrl: 'views/Analytics_ActivePromotion.html',
           controller: 'analytics_ActivePromotionController'
       })

      .state('Main.Analytics_TrackUsage', {
          url: '/Analytics_TrackUsage',
          templateUrl: 'views/Analytics_TrackUsage.html',
          controller: 'analytics_TrackUsageController'
      })


      .state('Main.Analytics_TrackUsageCategory', {
          url: '/Analytics_TrackUsageCategory',
          templateUrl: 'views/Analytics_TrackUsageCategory.html',
          controller: 'analytics_TrackUsageCategoryController'
      })

      .state('Main.Analytics_MultipleUsage', {
          url: '/Analytics_MultipleUsage',
          templateUrl: 'views/Analytics_MultipleUsage.html',
          controller: 'analytics_MultipleUsageController'
      })


      .state('Main.Analytics_SocialMediaTracking', {
          url: '/Analytics_SocialMediaTracking',
          templateUrl: 'views/Analytics_SocialMediaTracking.html',
          controller: 'analytics_SocialMediaTrackingController'
      })

     .state('Main.Analytic_UserPerDayUse', {
         url: '/Analytic_UserPerDayUse',
         templateUrl: 'views/Analytic_UserPerDayUse.html',
         controller: 'Analytic_UserPerDayUseController'
     })

     .state('Main.AddSubCategory', {
         url: '/AddSubCategory/:id',
         templateUrl: 'views/AddSubCategory.html',
         controller: 'addSubCategoryController'
     })

     .state('Main.AddSubCategoryList', {
         url: '/AddSubCategoryList',
         templateUrl: 'views/AddSubCategoryList.html',
         controller: 'addSubCategoryListController'
     })

     .state('Main.AddOptionalCategory', {
         url: '/AddOptionalCategory/:id',
         templateUrl: 'views/AddOptionalCategory.html',
         controller: 'addOptionalCategoryController'
     })

     .state('Main.AddOptionalCategoryList', {
         url: '/AddOptionalCategoryList',
         templateUrl: 'views/AddOptionalCategoryList.html',
         controller: 'addOptionalCategoryListController'
     })

     .state('Main.Predictive_Notification', {
         url: '/Predictive_Notification',
         templateUrl: 'views/Predictive_Notification.html',
         controller: 'predictiveNotificationController'
     })

     .state('Main.Manual_Notification', {
         url: '/Manual_Notification',
         templateUrl: 'views/Manual_Notification.html',
         controller: 'notificationController'
     })

    $urlRouterProvider.otherwise('/Login');

}]).factory('initData', function () {
    return {
        initFunction: function () {

            setTimeout(function () {
                Metronic.init(); // init metronic core componets
                Layout.init(); // init layout
                QuickSidebar.init(); // init quick sidebar
                Demo.init(); // init demo features
                Tasks.initDashboardWidget();
            }, 1000)
        }
    }

    ////////////////////////////////////////////////////////////Vendor Page lat long script start/////////////////////////////////////////////////////////////

}).factory('vendorData', function () {

    return {

        vendorFunction: function () {

            function initialize() {

                var l = 13.736717;

                var ln = 100.523186;
                //var address = document.getElementById('location').value;
                //alert(address);
                var myLatlng = new google.maps.LatLng(l, ln);
                var mapOptions = {
                    zoom: 8,
                    center: myLatlng
                }
                //console.log(document.getElementById('gmap'));
                var map = new google.maps.Map(document.getElementById('gmap'), mapOptions);
                //console.log(document.getElementById('gmap'));
                //console.log("map", map);
                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map,
                    // title: address,
                    draggable: true
                });

                google.maps.event.addListener(marker, 'd ragend', function (a) {

                    var pos = marker.getPosition();
                    var geocoder = new google.maps.Geocoder();
                    geocoder.geocode({
                        latLng: pos
                    }, function (responses) {
                        if (responses && responses.length > 0) {
                            //updateMarkerAddress(responses[0].formatted_address);
                            var location = document.getElementById("location");
                            location.value = responses[0].formatted_address;

                        } else {
                            updateMarkerAddress('Cannot determine address at this location.');
                        }
                    });

                    // var div = document.createElement('div');
                    // div.innerHTML = a.latLng.lat().toFixed(4) + ', ' + a.latLng.lng().toFixed(4);


                    var lat = document.getElementById("lat");
                    lat.value = a.latLng.lat().toFixed(4);

                    var lng = document.getElementById("long");
                    lng.value = a.latLng.lng().toFixed(4);

                    document.getElementsByTagName('body')[0].appendChild(div);
                });
            }
            initialize();
            google.maps.event.addDomListener(window, 'load', initialize);
            var input = document.getElementById('location');

            //alert(input);
            var options = {
                componentRestrictions: { country: 'th' }
            };
            var autocomplete = new google.maps.places.Autocomplete(input, options);

            autocomplete.addListener('place_changed', fillInAddress);

            function fillInAddress() {
                // Get the place details from the autocomplete object.
                var place = autocomplete.getPlace();
                var address = $('input[name=address]').val();
                $('.location').val(address);
                var lati = place.geometry.location.lat();
                var lonng = place.geometry.location.lng();
                $('.lat').val(lati);
                $('.long').val(lonng);
                plotmap(lati, lonng);

            }

            function plotmap(lati, lonng) {
                
                var myLatlng = new google.maps.LatLng(lati, lonng);
                var mapOptions = {
                    zoom: 10,
                    center: myLatlng
                }
                var map = new google.maps.Map(document.getElementById('gmap'), mapOptions);

                var marker = new google.maps.Marker({
                    position: myLatlng,
                    map: map

                });
            }
            $('#croop').click(function () {

                $('#cropm').modal('hide');


            })
        }

    }

    ////////////////////////////////////////////////////////////Vendor Page lat long script end/////////////////////////////////////////////////////////////

}).controller('mainController', function ($scope, $rootScope, $cookies, $cookieStore, $location, vendorService, initData) {
    //$rootScope.isLogged = 0;
    //initData.initFunction();

    $scope.message = "Main Content";
    $scope.config = {
        //api: 'http://localhost:3338'
        //api: 'http://cravetemp.techvalens.net'
        //api:'http://craveservices.techvalens.net'
        api: 'http://eddee.cloudapp.net/eddee'
    };

    $rootScope.logout = function () {
        $cookieStore.remove('loggedUserName');
        $rootScope.UserName = $cookieStore.get('loggedUserName');
        $rootScope.isLogged = 1;
        $location.path('/Login');
    }

    $rootScope.changePassword = function () {
        $('#changepwd').modal('show');
    }

    $rootScope.change = function () {
        $rootScope.isbusy = true;

        if ($scope.newPassword == $scope.reTypePassword) {

            $scope.loginVendorId = $cookieStore.get('loggedUserId');
            var data = {
                loginId: $scope.loginVendorId, password: $scope.newPassword, oldPassword: $scope.password

            };
            vendorService.changePassword($scope, data, function (success, data) {
                if (data.responseCode == 1) {
                    $rootScope.Message = "password successfully changed!";
                    $rootScope.isbusy = false;
                }
                else {
                    $rootScope.Message = "oldpassword and newpassword not matched!";
                }
            });
        }
        else {
            $rootScope.Message = "newpassword and retype-password not matched!";
        }
    }

    
})

