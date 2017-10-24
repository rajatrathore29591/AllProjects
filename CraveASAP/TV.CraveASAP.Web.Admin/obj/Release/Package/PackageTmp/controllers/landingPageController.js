'use strict';
app.controller('landingPageController', ['$scope', 'landingService', '$rootScope', '$cookies', '$cookieStore','initData', function ($scope, landingService, $rootScope, $cookies, $cookieStore,initData) {
    $rootScope.isLogged = 1;
    $rootScope.active = "ManageApplication";
    $rootScope.active1 = "Landing";
    $scope.showLoader = true;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    landingService.getAllLanding($scope, function (success, data) {
        if (success) {
            $scope.showLoader = false;
        }
        else {

        }

    });

    $scope.Update = function (landingPageId) {
        $scope.Message = "Changes Successfully Updated !";
        $scope.Message1 = "Sorry!!! Time is already defined in Category for selecting it first unselect and then select it !";
        $scope.showLoader = true;
        landingService.Update(landingPageId, $scope, function (success, data) {

            $scope.redirect = function () {
                location.reload();

            }
            if (data == "true") {

                $scope.showLoader = false;
                $("#static").modal('show');
            }
            else {
                $("#static1").modal('show');
                $scope.showLoader = false;
            }

        });
    }
}])

