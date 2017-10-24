'use strict';
app.service('categoryService', ['$http', function ($http) {

    this.getAllSubCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllSubCategory",
        }).success(function (data) {
            callback(true, data);
            //console.log("data", data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getSubcategoryById = function ($scope, currentCategoryId, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetSubCategoryById " + $scope.currentCategoryId,
        }).success(function (data) {
            $scope.VendorDetail = data;
            //console.log("VendorDetail date====", JSON.stringify(data));
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getAllVendorCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVendorCategory",
        }).success(function (data) {
            // console.log("data", data);
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.saveSubCategory = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/AddSubCategory",
            data: data
        }).success(function (data) {
            callback(true, data);
            //console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.updateSubCategory = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateSubCategory",
            data: data
        }).success(function (data) {
            callback(true, data);
            // console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.deleteSubCategory = function ($scope, subCategoryId, callback) {
        //alert("id=="+vendorId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/DeleteSubCategory",
            data: { subCategoryId: subCategoryId }
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getAllOptCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllOptionalCategory",
        }).success(function (data) {
            callback(true, data);
            //console.log("data", data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getOptcategoryById = function ($scope, currentCategoryId, callback) {
        //$scope.vendorId = currentVendorId;
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetOptCategoryById " + $scope.currentCategoryId,
        }).success(function (data) {
            $scope.VendorDetail = data;
            //console.log("VendorDetail date====", JSON.stringify(data));
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.saveOptCategory = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/AddOptionalCategory",
            data: data
        }).success(function (data) {
            callback(true, data);
            // console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.updateOptCategory = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateOptCategory",
            data: data
        }).success(function (data) {
            callback(true, data);
            // console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getAllCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllCategory",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.deleteOptCategory = function ($scope, optCategoryId, callback) {
        //alert("id=="+vendorId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/DeleteOptCategory",
            data: { optCategoryId: optCategoryId }
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getAllCategoryByCategoryId = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllCategoryByCategoryId" + $scope.categorySelect,
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
   

    this.Print = function (PrintDiv) {
        var element = document.getElementById("PrintDiv");
        window.document.write(element.innerHTML);
        window.print();
    }

    this.defaultvendor = {
        case_id: 0,
        registration_no: '',
        filed_on: '',//"\/Date(1434652200000+0530)\/",
        main_section_id: 0,
        ps_code: 0,
        filed_by: 0,
        filed_court: 0,
        next_hearing_date: null,
        interim_bond_status: 0,
        interim_bond_date: null,
        interim_bond_description: '',
        final_bond_status: 0,
        final_bond_date: null,
        final_bond_description: '',
        closure_status: 1,
        close_date: null,
        closure_remarks: ''
    };

}]);