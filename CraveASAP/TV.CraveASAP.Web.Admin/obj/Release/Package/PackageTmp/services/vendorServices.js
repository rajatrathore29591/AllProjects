'use strict';
app.service('vendorService', ['$http', function ($http) {

    this.GetVendors = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVendors",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true,data);
            //console.log("data", data);
        }).error(function (data) {
            console.log("data", data);
            callback(false,data);
        });;
    };

    this.getAllVendorCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVendorCategory",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
           // console.log("data", data);
            callback(true,data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getVendorById = function ($scope, currentVendorId, callback) {
        $scope.vendorId = currentVendorId;
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetVendorById " + $scope.currentVendorId,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.VendorDetail = data;
            //console.log("VendorDetail date====", JSON.stringify(data));
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.saveVendors = function ($scope, data,callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateVendor",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
            //console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.updateVendors = function ($scope, data, callback) {
        var key = $scope.oAuthkey;
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateVendor",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
           // console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.saveVendorBranch = function ($scope, data,callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateVendorBranch",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
           // console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.deleteVendor = function ($scope,vendorId) {
        //alert("id=="+vendorId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/DeleteVendor",
            headers: {
                "oAuthKey": "Admin"
            },
            data: { vendorId: vendorId }
        }).success(function (data) {
            //callback(true, data);
        }).error(function (data) {
            //callback(false, data);
            console.log("data", data);
        });;
    };

    this.Print = function (PrintDiv) {
        var element = document.getElementById("PrintDiv");
        window.document.write(element.innerHTML);
        window.print();
    }

    this.changePassword = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/ChangeAdminPassword",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
            //console.log("data", data);
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