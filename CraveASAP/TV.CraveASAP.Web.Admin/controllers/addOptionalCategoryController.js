'use strict';

app.controller('addOptionalCategoryController', ['$scope', '$location', 'categoryService', '$stateParams', '$cookies', '$cookieStore', '$rootScope', '$parse', 'initData', function ($scope, $location, categoryService, $stateParams, $cookies, $cookieStore, $rootScope, $parse, initData) {

    $scope.checked = false;
    $scope.showTable = "Save";

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
       // initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.currentCategoryId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.data = {};
    $scope.vendortitle = "Create Vendor";
    $scope.editMode = 0;
    $scope.showLoader = true;
    loadCategory();
    loadSubCategory();
    
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
            categoryService.getOptcategoryById($scope, $scope.currentCategoryId, function (success, data) {
                if (success) {

                    $scope.optCategory = data.data1;
                    $scope.categorySelect = $scope.optCategory.categoryId;
                    $scope.subCategorySelect = $scope.optCategory.subCategoryId;
                    $scope.showTable = "Update";
                    $scope.showLoader = false;
                }
            });
        }
    }
    $scope.change = function () {
        //alert($scope.categorySelect);
        loadSubCategoryById();
    }
    function loadCategory() {

        categoryService.getAllVendorCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.category = data.data;
                $scope.showLoader = false;
            }
            else
                console.log("failed", data);
        });
    } 

    function loadSubCategory() {

        categoryService.getAllSubCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.subCategory = data.data;
                loadCategoryPage();
                $scope.showLoader = false;
            }
            else
                console.log("failed", data);
        });
    }

    function loadSubCategoryById() {
        $scope.showLoader = true;
        categoryService.getAllCategoryByCategoryId($scope, function (success, data, callback) {
            if (success) {
                $scope.subCategory = data.data[0].SubCategories;
                $scope.subCategorySelect = $scope.subCategory[0].subCategoryId;
                $scope.showLoader = false;
            }
            else
                console.log("failed", data);
        });
    }
     

    $scope.saveCategory = function () {
        $scope.Message = "Please select category";
        if ($scope.categorySelect) {
            $scope.Message = "Optional category has been Successfully Created!";
            $scope.showLoader = true;

            var data = {
                optCategoryId: $scope.optCategory.optCategoryId, optCategoryName: $scope.optCategory.optCategoryName, categoryId: $scope.categorySelect,subCategoryId:$scope.subCategorySelect

            };
            categoryService.saveOptCategory($scope, data, function (success, data) {
                if (success) {
                    $scope.redirect = function () {
                        $location.path("/Main/AddOptionalCategoryList");
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

    $scope.cancel = function () {
        $location.path("/AddOptionalCategoryList");
    }

    $scope.update = function () {
        $scope.Message = "Please select category";
        if ($scope.categorySelect) {
            $scope.Message = "Optional Category has been Successfully Updated!";
            $scope.showLoader = true;
            var data = {

                optCategoryId: $scope.optCategory.optCategoryId, optCategoryName: $scope.optCategory.optCategoryName, categoryId: $scope.categorySelect, subCategoryId: $scope.subCategorySelect
            };

            categoryService.updateOptCategory($scope, data, function (success, data) {
                if (success) {
                    $scope.redirect = function () {
                        $location.path("/Main/AddOptionalCategoryList");
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

