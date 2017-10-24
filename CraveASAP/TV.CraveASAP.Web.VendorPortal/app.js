var app = angular.module('myVendorApp', ['ui.router', 'ngCookies', 'ngSanitize', 'ImageCropper', 'ngStorage', 'ngResource'])
.config(['$stateProvider', '$urlRouterProvider', function ($stateProvider, $urlRouterProvider) {
    $stateProvider
          .state('/Login', {
              url: '/Login',
              templateUrl: 'views/Login.html',
              controller: 'loginController'
          })

        .state('Main', {
            url: '/Main',
            templateUrl: 'views/Main.html',
            abstract: true
        })

       .state('Main.CreatePromotion', {
           url: '/CreatePromotion/:id/:status',
           templateUrl: 'views/CreatePromotion.html',
           controller: 'createPromotionController'
       })

         .state('Main.Track', {
             url: '/Track',
             templateUrl: 'views/Track.html',
             controller: 'trackController'
         })

        .state('Main.Manage', {
            url: '/Manage',
            templateUrl: 'views/Manage.html',
            controller: 'manageController'
        })

        .state('Main.EditVendor', {
            url: '/EditVendor',
            templateUrl: 'views/EditVendor.html',
            controller: 'editVendorController'
        })

        .state('Main.subscribe', {
            url: '/subscribe',
            templateUrl: 'views/subscribe.html',
            controller: 'subscribeController'
        })

         .state('Main.AboutUs', {
             url: '/AboutUs',
             templateUrl: 'views/AboutUs.html',
             controller: 'aboutController'
         })

         .state('Main.TermCondition', {
             url: '/TermCondition',
             templateUrl: 'views/TermCondition.html',
             controller: 'termConditionController'
         })

         .state('Main.PrivacyPolicy', {
             url: '/PrivacyPolicy',
             templateUrl: 'views/PrivacyPolicy.html',
             controller: 'privacyPolicyController'
         })

        .state('Main.Confirm', {
            url: '/Confirm',
            templateUrl: 'views/Confirm.html',
            controller: 'createPromotionController'
        })

       .state('Main.ThankYou', {
           url: '/ThankYou',
           templateUrl: 'views/ThankYou.html',
           controller: 'createPromotionController'
       })

      .state('Main.HowItsWork', {
          url: '/HowItsWork',
          templateUrl: 'views/HowItsWork.html',
          controller: 'howItsWorkController'
      })

    $urlRouterProvider.otherwise('/Login');


}]).factory('initData', function () {
    return {
        initPromotion: function () {
        }
    }

}).controller('mainController', function ($scope, $rootScope, $cookies, $cookieStore, $location, editVendorService) {

    $rootScope.isLogged = 1;
    $scope.message = "Main Content";
    $scope.config = {
        //api: 'http://localhost:3338'
        api: 'http://104.215.253.235/eddee'
        //api: 'http://eddee.cloudapp.net/eddee'
    };

    $rootScope.changePassword = function () {
        $('#changepwd').modal('show');
    }

    if ($cookieStore.get('loggedUserName') != undefined) {
        $('#topMenu').css({ 'display': 'block' });
    }

    $rootScope.logout = function () {
        $('#topMenu').css({ 'display': 'none' });
        $cookieStore.remove('loggedUserName'); 
        $cookieStore.remove('loggedName');
        $location.path('/');
        $(':input').val('');
    }

    $rootScope.change = function () {
        $rootScope.isbusy = true;
      
        if ($scope.newPassword == $scope.reTypePassword) {
            //$scope.vendorId = $cookieStore.get('loggedUserId');
            $scope.vendorId = $cookieStore.get('loggedId');
            var data = {
                vendorId: $scope.vendorId, password: $scope.newPassword, oldPassword: $scope.password
            };

            editVendorService.changePassword($scope, $rootScope, data, function (success, data) {
                if (data.responseCode == 1) {
                    $rootScope.Message = "password successfully changed!";
                    $rootScope.isbusy = false;
                    $(':input').val('');
                }
                else {
                    $rootScope.isbusy = false;
                    $rootScope.Message = "confirmpassword and newpassword not matched!";
                   // $(':input').val('');
                }
            });
        }
        else {
            $rootScope.isbusy = false;
            $rootScope.Message = "newpassword and retype-password not matched!";
           // $(':input').val('');
        }
    }

    $rootScope.cancel = function () {

        $(':input').val('');
        $rootScope.Message = null;
    }


});;

