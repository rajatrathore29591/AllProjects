'use strict';

app.controller('loginController', ['$scope', '$rootScope', '$location', 'loginService', '$cookies', '$cookieStore', 'initData', function ($scope, $rootScope, $location, loginService, $cookies, $cookieStore, initData) {
    $rootScope.isLogged = 0;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/Login');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedUserName');
        $location.path('/Main/Dashboard');
    }
    $(':input').keypress(function (e) {
        var code = e.keyCode || e.which;
        if (code == 13) {

            $scope.login();
        }

    });

    $scope.login = function () {
        $scope.showLoader = true
        loginService.loginAuth($scope, function (success, data) {
            if (success) {
                $scope.showLoader = false;
                $cookieStore.remove('loggedUserName');

                if (data.statusCode == 1) {
                   
                    $rootScope.isLogged = 1;
                    $cookieStore.put('loggedUserId', data.data[0].loginId);
                    $cookieStore.put('isLogged', 1);
                    $cookieStore.put('loggedUserName', data.data[0].loginName);

                    $cookies.userName = data.data[0].loginName;

                    $location.path('/Main/Dashboard');
                } else {
                    $cookieStore.remove('loggedUserId');
                    $cookieStore.remove('isLogged');
                    $cookieStore.put('loggedUserName', 1);
                    $scope.loginError = "Username or Password not matched.";
                    alert("Username or Password not matched.");
                }
            }
            else {
                $scope.loginError = "Username or Password not matched.";
                console.log("failed", data);
            }
        });

    }



}])
