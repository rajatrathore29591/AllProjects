'use strict';
app.service('manageActivePromotionSevice', ['$http', function ($http) {
    this.getAllPromotion = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetPromotion",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true,data);
        }).error(function (data) {
            callback(false,data);
            console.log(data);
        });;
    }; 

    this.getAllCategory = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllCategory",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true,data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.getAllSubCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllSubCategory",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.getAllOptionalCategory = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllOptionalCategory",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
    this.GetAllVendors = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVendors",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.getAllCategoryByBussID = function ($scope, CategoryId, callback) {
        //alert(JSON.stringify($scope.categoryBussId));
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllCategoryByCategoryId" + $scope.categoryBussId,
        }).success(function (data) {
            $scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
    this.getPromotionById = function ($scope, promotionId, callback) {
        //alert("dataaaaa"+ promotionId);
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllPromotionsById" + $scope.promotionId,
        }).success(function (data) {
            $scope.promotion = data;
            callback(true,data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.savePromotion = function ($scope ,data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateVendorPromotions",
            data: data
        }).success(function (data) {
            
            callback(true , data);
            console.log(data);
        }).error(function (data) {
            callback(false , data);
            console.log(data);
        });;
    };

    this.updatePromotion = function ($scope, data) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateVendorPromotion",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            console.log(data);
        }).error(function (data) {
            console.log(data);
        });;
    };

    this.DeletePromotion = function ($scope,promotionCodeId,callback) {
        //alert("IDt :" + promotionCodeId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/DeletePromotion",
            headers: {
                "oAuthKey": "Admin"
            },
            data: { promotionCodeId: promotionCodeId ,vendorId:1}
        }).success(function (result) {
            
            callback(true,result);
           // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false,result);
           // alert("Record Not Successfully Server Error !");
        });;
    };

}]);
