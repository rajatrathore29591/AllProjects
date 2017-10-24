'use strict';

app.controller('addOptionalCategoryListController', ['$scope', '$location', '$rootScope', 'categoryService', '$cookies', '$cookieStore','initData', function ($scope, $location, $rootScope, categoryService,  $cookies, $cookieStore,initData) {

    $rootScope.active = 'AddOptCategoryList';
    $rootScope.active1 = "addoptcat";
    //$scope.currentOptCategoryId = ($routeParams.id > 0) ? $routeParams.id : 0;
    //$scope.isbusy = 1;
    $scope.showLoader = true;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    categoryService.getAllOptCategory($scope, function (success, data) {
        if (success) {

            $scope.optCategory = data.data;
            $scope.showLoader = false;

        }
        else {
        }
    });

    $scope.Delete = function (optCategoryId) {
        $scope.Message = "Subcategory has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
            categoryService.deleteOptCategory($scope, optCategoryId);
            var index = optCategoryId;
            for (var i in $scope.optCategory) {
                if ($scope.optCategory[i].optCategoryId == optCategoryId) {
                    $scope.optCategory.splice(i, 1);
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

