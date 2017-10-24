'use strict';
app.service('editVendorService', ['$http', function ($http) {
  
    this.getVendorById = function ($scope, loginVendorId, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetVendorById " + $scope.loginVendorId,
        }).success(function (data) {
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
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.updateVendors = function ($scope, $rootScope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateVendor",
            headers: {
                "oAuthKey": $rootScope.key
            },
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.changePassword = function ($scope, $rootScope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/ChangeVendorPassword",
            headers: {
                "oAuthKey": $rootScope.key
            },
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };
   
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