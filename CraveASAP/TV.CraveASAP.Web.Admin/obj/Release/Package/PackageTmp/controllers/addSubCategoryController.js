'use strict';

app.controller('addSubCategoryController', ['$scope', '$location', 'categoryService', '$stateParams', '$cookies', '$cookieStore', '$rootScope', '$parse','initData', function ($scope, $location, categoryService, $stateParams, $cookies, $cookieStore, $rootScope, $parse,initData) {

    $scope.checked = false;
    $scope.showTable = "Save";
    $scope.showLoader = true;
    loadCategory();

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.currentCategoryId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.data = {};
    $scope.vendortitle = "Create Vendor";
    $scope.editMode = 0;
    $scope.save = function () {

        if ($scope.showTable == "Save") {

            $scope.saveCategory();
        }
        else {

            $scope.update();
        }

    }

    function loadCategoryPage() {
        if ($scope.currentCategoryId == 0) {

        }
        else {
            $scope.vendortitle = 'Edit Vendor';
            $scope.showTable = !$scope.showTable;
            $scope.showLoader = true;
            categoryService.getSubcategoryById($scope, $scope.currentCategoryId, function (success, data) {
                if (success) {

                    $scope.subCategory = data.data1;
                    $scope.showLoader = false;
                    $scope.showTable = "Update";

                }

            });
        }
    }

    function loadCategory() {

        categoryService.getAllVendorCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.category = data.data;
                loadCategoryPage();
                $scope.showLoader = false;
            }
            else
                console.log("failed", data);
        });
    }

    $scope.saveCategory = function () {
        $scope.Message = "Please select category";
        if ($scope.subCategory.categoryId) {
            $scope.Message = "Optional category has been Successfully Created!";
            $scope.showLoader = true;

            var data = {
                subCategoryId: $scope.subCategory.subCategoryId, subCategoryName: $scope.subCategory.subCategoryName, categoryId: $scope.subCategory.categoryId
            };
            categoryService.saveSubCategory($scope, data, function (success, data) {
                if (success) {
                    $scope.redirect = function () {
                        $location.path("/Main/AddSubCategoryList");
                    }

                    $("#static").modal('show');
                    $scope.showLoader = false;

                }
                else {

                }
            });
        }
        $("#static").modal('show');
    }

    $scope.update = function () {
        $scope.Message = "Please select category";
        if ($scope.subCategory.categoryId) {
            $scope.Message = "Optional Category has been Successfully Updated!";
            $scope.showLoader = true;
            var data = {
                subCategoryId: $scope.subCategory.subCategoryId, subCategoryName: $scope.subCategory.subCategoryName, categoryId: $scope.subCategory.categoryId
            };

            categoryService.updateSubCategory($scope, data, function (success, data) {
                if (success) {
                    $scope.redirect = function () {
                        $location.path("/Main/AddSubCategoryList");
                    }

                    loadCategory();
                    $("#static").modal('show');
                    $scope.showLoader = false;

                }
                else {

                }
            });
        }
        $("#static").modal('show');
    }
}])

