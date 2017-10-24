'use strict';

app.controller('vendorListController', ['$scope', '$location', '$rootScope', 'vendorService', '$cookies', '$cookieStore','initData', function ($scope, $location, $rootScope, vendorService, $cookies, $cookieStore,initData) {

    $rootScope.active = 'VendorList';
   // $scope.currentVendorId = ($stateParams.id > 0) ? $stateParams.id : 0;
    //$scope.isbusy = 1;
    $scope.showLoader = true;
    loadCategory();
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
       // initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    vendorService.GetVendors($scope, function (success, data) {
        if (success) {
            $scope.isbusy = 0;
            $scope.vendor = data.data;
            $scope.showLoader = false;

        }
        else {
        }
    });

    function loadCategory() {

        vendorService.getAllVendorCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.category = data.data;

            }
            else
                console.log("failed", data);
        });
    }

    $scope.getCategoryNameById = function (id) {

        var data = _.findWhere($scope.category, { categoryId: id });
        return data.categoryName;
    }

    $scope.Delete = function (vendorId) {
        $scope.Message = "Vendor has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
            vendorService.deleteVendor($scope, vendorId);
            var index = vendorId;
            for (var i in $scope.vendor) {
                if ($scope.vendor[i].vendorId == vendorId) {
                    $scope.vendor.splice(i, 1);
                }
            }
            $("#static").modal('show');
        }

        $("#delete").modal('show');
    }

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;   //set the sortKey to the param passed
        $scope.reverse = !$scope.reverse; //if true make it false and vice versa
    }

}])

