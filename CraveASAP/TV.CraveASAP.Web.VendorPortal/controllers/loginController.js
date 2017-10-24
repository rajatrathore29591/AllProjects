'use strict';
app.controller('loginController', ['$scope', '$rootScope', 'loginService', '$cookies', '$cookieStore', '$location', '$localStorage', 'translationService', function ($scope, $rootScope, loginService, $cookies, $cookieStore, $location, $localStorage, translationService) {
    $scope.message = "Now viewing home!";

    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
        $location.path('/Main/Manage');
    }

    $(':input').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {

            $scope.login();
        }

    });
    if ($cookieStore.get('Language') == undefined) {
        $cookieStore.put('Language', 'english');
    }

    $scope.tempEngLang = function () {
        $cookieStore.put('Language', 'english');
        $rootScope.lang = $cookieStore.get('Language');
        translationService.getTranslation($scope, $rootScope, function (data) {
            $rootScope.translation = data;
        });
    }

    $scope.tempThaiLang = function () {
        $cookieStore.put('Language', 'thai');
        $rootScope.lang = $cookieStore.get('Language');
        translationService.getTranslation($scope, $rootScope, function (data) {
            $rootScope.translation = data;
        });
    }

    $rootScope.lang = $cookieStore.get('Language');
    translationService.getTranslation($scope, $rootScope, function (data) {
        
        $rootScope.translation = data;
    });

    $scope.forgetPassword = function () {

        $('#forgetpwd').modal('show');
    }

    $scope.forget = function () {
        var data = {
            email: $scope.email
        };
        $scope.Loader = true;
        loginService.sendForgetPassword($scope, data, function (success, data) {
            if (data.responseCode == 1) {
                $scope.Loader = false;
                $rootScope.Message = $scope.translation.Passwordhasbeensenttoyouremailaddress;
            }
            else {
                $scope.Loader = false;
                $rootScope.Message = $scope.translation.Emailaddressisincorrect;
                console.log("failed", data);
            }
        });
    }

    $scope.cancel = function () {

        $rootScope.Message = null;
        $scope.email = null;
        $scope.Loader = false;
    }

    $rootScope.SessionVendorId = $cookieStore.get('loggedId');
    $scope.login = function () {
        $scope.showLoader = true;
        loginService.loginAuth($scope, function (success, data) {
            if (success) {
                $cookieStore.remove('loggedUserName');

                if (data.statusCode == 1) {
                    $rootScope.isLogged = 0;
                    $location.path('/Main/Manage');
                    $cookieStore.put('loggedId', data.data[0].vendorId);
                    $cookieStore.put('loggedName', data.data[0].companyName);
                    $cookieStore.put('oAuthkey', data.oAuthkey);
                    $cookieStore.put('categoryId', data.data[0].businessCategory);
                    $rootScope.categoryId = $cookieStore.get('categoryId');
                    $rootScope.key = $cookieStore.get('oAuthkey');
                    $cookies.userName = data.data[0].companyName;
                    $cookieStore.put('VendorLocal', data.data[0].logoImg);
                    $cookieStore.put('VendorShort', data.data[0].shortDescription);
                    $cookieStore.put('VendorLat', data.data[0].latitude);
                    $cookieStore.put('VendorLong', data.data[0].longitude);

                    $rootScope.lang = $cookieStore.get('Language');

                    $scope.showLoader = false;

                } else {
                    $cookieStore.remove('loggedId');
                    $cookieStore.remove('isLogged');
                    $cookieStore.put('loggedName', 1);
                    $scope.loginError = "username or password is invalid.";
                    alert("username or password is invalid.");
                    $scope.showLoader = 0;
                }
            }
            else {
                $scope.loginError = "username or password is invalid.";
                console.log("failed", data);
            }
        });

    }


}])

