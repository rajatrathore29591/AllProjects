'use strict';
app.controller('predictiveNotificationController', ['$scope', '$rootScope', '$location',  'notificationService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, $location, notificationService, $cookies, $cookieStore,initData) {
    $rootScope.active = 'Notification';
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.contacts = [{ type: "AppType", value: "User" }, { type: "AppType", value: "Vendor" }, { type: "AppType", value: "Both" }, ];
    $scope.contactTypes = { "AppType": { default: "User" } };
    $scope.FailureNoti = 20;
    $scope.SuccessNoti = 15;
    $scope.SendPredNoti = function () {



        var data = { reSendTime: $scope.reSendTime };

        notificationService.SendPredictiveNotification($scope, data, function (success, data) {
            if (success) {
                alert("Notification Send Successfully");
            }
            else {
                console.log("failed", data);
            }
        });
    }

    notificationService.GetPredictiveNotification($scope, function (success, data) {
        if (success) {
            $scope.isbusy = 0;
            $scope.Noti = data.data1;
            $scope.showLoader = false;

        }
        else {
        }
    });


}]);
