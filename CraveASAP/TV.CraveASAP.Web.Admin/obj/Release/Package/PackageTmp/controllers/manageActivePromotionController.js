'use strict';
app.controller('manageActivePromotionController', ['$scope', '$location',  'manageActivePromotionSevice', '$rootScope', '$cookies', '$cookieStore','initData', function ($scope, $location, manageActivePromotionSevice, $rootScope,$cookies, $cookieStore,initData) {
    $rootScope.isLogged = 1;
    $rootScope.active = "ManageApplication";
    $rootScope.active1 = "Promotion";
    //$scope.promotionId = ($routeParams.id > 0) ? $routeParams.id : 0;
   
    $scope.showLoader = true;

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    manageActivePromotionSevice.getAllPromotion($scope, function (success, data) {
        if (success) {
            $scope.getAllPromotion = data.data;
            $scope.isbusy = 0;
            $scope.showLoader = false;
        }
        else {
            console.log("failed", data);
        }
    });

    $scope.DeleteOld = function (promotionCodeId) {
        $scope.Message = "Promotion has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
        manageActivePromotionSevice.DeletePromotion($scope, promotionCodeId, function (success, data) {
            if (success) {
                    var index = promotionCodeId;
                   
                    for (var i in $scope.getAllPromotion) {
                        if ($scope.getAllPromotion[i].promotionCodeId == promotionCodeId) {
                            $scope.getAllPromotion.splice(i, 1);
                        }

                    }
                }
                else {
                    console.log("failed", data);
                }
            });
            $("#static").modal('show');
        }
       
     
        $("#delete").modal('show');
    }

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

}])
