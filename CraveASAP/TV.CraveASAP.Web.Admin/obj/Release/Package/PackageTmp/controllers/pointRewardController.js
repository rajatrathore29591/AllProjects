'use strict';
app.controller('pointRewardController', ['$scope', 'pointRewardServices', '$rootScope', '$cookies', '$cookieStore','initData', function ($scope, pointRewardServices, $rootScope, $cookies, $cookieStore,initData) {
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $rootScope.active = "PointReward";
    //console.log($rootScope.active);
    $rootScope.active1 = "Point";
    $rootScope.isLogged = 1;
    $scope.showLoader = true;
    pointRewardServices.getPointConfiguration($scope, function (success, data) {
        $scope.showLoader = false;
        $scope.point = data;
    });


    $scope.Update = function () {
        $scope.limit = [];

        $scope.limit[0] = $scope.point[0].limit;
        $scope.limit[1] = $scope.point[1].limit;
        $scope.limit[2] = $scope.point[2].limit;
        $scope.limit[3] = $scope.point[3].limit;
        $scope.limit[4] = $scope.point[4].limit;

        $scope.pointsEarned = [];

        $scope.pointsEarned[0] = $scope.point[0].pointsEarned;
        $scope.pointsEarned[1] = $scope.point[1].pointsEarned;
        $scope.pointsEarned[2] = $scope.point[2].pointsEarned;
        $scope.pointsEarned[3] = $scope.point[3].pointsEarned;
        $scope.pointsEarned[4] = $scope.point[4].pointsEarned;


        for (var i = 0; i < 5; i++) {
            $scope.i = i; $scope.j = i + 1;
            pointRewardServices.Update($scope, function (success, data) {
                //  alert($scope);
            });


        }
        location.reload();
    }



}])



