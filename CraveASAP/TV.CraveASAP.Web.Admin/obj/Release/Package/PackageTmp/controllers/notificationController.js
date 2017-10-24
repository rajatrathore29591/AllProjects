'use strict';
app.controller('notificationController', ['$scope', '$rootScope', '$location', 'notificationService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, $location,  notificationService, $cookies, $cookieStore,initData) {
    $rootScope.active = 'Notification';
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.contacts = [{ type: "AppType", value: "User" }, { type: "AppType", value: "Vendor" }, { type: "AppType", value: "Both" }];
    $scope.contactTypes = { "AppType": { default: "User" } };


    $scope.SendManualNoti = function () {
        $scope.showLoader = true;
        
        $scope.Type = _.pluck($scope.contactTypes, 'default');

        var data = {
            AppType: String($scope.Type), Alert: $scope.alert, MessageEnglish: $scope.noti
        };
        alert(JSON.stringify(data));
        notificationService.SendManualNotification($scope, data, function (success, data) {
           
            if (success) {
                $scope.Message = "Notification Send Successfully To " + String($scope.Type);
                $('#static').modal('show');
                $scope.showLoader = false;
                //$location.path("/Main/Manual_Notification");

                
            }
            else {
                console.log("failed", data);
            }
        });
    }

}])
