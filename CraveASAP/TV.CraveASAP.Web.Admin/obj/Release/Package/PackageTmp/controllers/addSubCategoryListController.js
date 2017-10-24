'use strict';

app.controller('addSubCategoryListController', ['$scope', '$location', '$rootScope', 'categoryService',  '$cookies', '$cookieStore','initData', function ($scope, $location, $rootScope, categoryService,  $cookies, $cookieStore,initData) {

    $rootScope.active = 'AddSubCategoryList';
    //$scope.currentSubCategoryId = ($routeParams.id > 0) ? $routeParams.id : 0;
    //$scope.isbusy = 1;
    $rootScope.active1 = "addsubcat";
    $scope.showLoader = true;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    categoryService.getAllSubCategory($scope, function (success, data) {
        if (success) {
            $scope.subCategory = data.data;
            $scope.showLoader = false;

        }
        else {
        }
    });

    $scope.Delete = function (subCategoryId) {
        $scope.Message = "Subcategory has been successfully deleted";
        $scope.Message1 = "Are you sure you want to delete!";
        $scope.sure = function () {
            categoryService.deleteSubCategory($scope, subCategoryId);
            var index = subCategoryId;
            for (var i in $scope.subCategory) {
                if ($scope.subCategory[i].subCategoryId == subCategoryId) {
                    $scope.subCategory.splice(i, 1);
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

