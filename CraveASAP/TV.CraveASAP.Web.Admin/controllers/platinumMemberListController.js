'use strict';
app.controller('platinumMemberListController', ['$scope', '$rootScope', 'platinumMemberService', '$cookies', '$cookieStore', '$location','initData', function ($scope, $rootScope, platinumMemberService, $cookies, $cookieStore, $location,initData) {
    $scope.isbusy = 1; $scope.PremiumReward;
    $rootScope.active = "PointReward";
    $rootScope.active1 = "Premium";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
       // initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    platinumMemberService.getRewardByType($scope, function (success, data) {
        if (success) {


            $scope.PremiumReward = data;
            $scope.isbusy = 0;
        }
        else {
            //fnShowMessage(true, data);
        }
    });

    $scope.Delete = function (rewardId) {
        $scope.Message = "Reward has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
            platinumMemberService.DeletReward($scope, rewardId);
            var index = rewardId;
            for (var i in $scope.PremiumReward) {
                if ($scope.PremiumReward[i].rewardId == rewardId) {
                    $scope.PremiumReward.splice(i, 1);
                }
            }
            $("#static").modal('show');
           //$location.path("/PlatinumRewardList");
        }


        $("#delete").modal('show');
    }

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

}])
