'use strict';
app.controller('goldMemberListController', ['$scope', '$rootScope', 'goldMemberService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, goldMemberService, $cookies, $cookieStore,initData) {
    //$rootScope.active = "ManageApplication";
    $rootScope.isLogged = 1;
    $scope.isbusy = 1;
    $rootScope.active = "PointReward";
    $rootScope.active1 = "Gold";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    goldMemberService.getRewardByType($scope, function (success, data) {
        if (success) {
            $scope.isbusy = 0;
            $scope.goldReward = data;
        }
        else {
        }
    });

    $scope.Delete = function (rewardId) {
        $scope.Message = "Reward has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
            goldMemberService.DeletReward($scope, rewardId);
            var index = rewardId;
            for (var i in $scope.goldReward) {
                if ($scope.goldReward[i].rewardId == rewardId) {
                    $scope.goldReward.splice(i, 1);
                }

            }
            $("#static").modal('show');
        }


        $("#delete").modal('show');

    }
}])
