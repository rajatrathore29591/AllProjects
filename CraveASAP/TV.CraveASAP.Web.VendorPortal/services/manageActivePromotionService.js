'use strict';
app.service('manageActivePromotionService', ['$http', function ($http) {
  
    this.getAllCategory = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllCategory",
        }).success(function (data) {
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
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.getAllCategoryByBussID = function ($scope, CategoryId, callback) {
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

    this.getPromotionByVendorId = function ($scope,$rootScope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetPromotionByVendorId" + $rootScope.SessionVendorId,
            headers: {
                "oAuthKey": $rootScope.key
            },
        }).success(function (data) {
            $scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
    
    this.getPromotionById = function ($scope, currentPromotionId, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllPromotionsById" + $scope.currentPromotionId,
        }).success(function (data) {
            $scope.promotion = data;
            callback(true,data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.GetAllVendors = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetVendorsList",
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

    this.getActivePromotionByVendorId = function ($scope,$rootScope, vendorID, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetActivePromotionByVendorId" + $scope.vendorID,
            headers: {
                "oAuthKey": $rootScope.key
            },
        }).success(function (data) {
            $scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.updatePromotion = function ($scope, $rootScope, data, callback) {
       
        return $http({
            
            method: "POST",
            url: $scope.config.api + "/UpdateVendorPromotion",
            headers: {
                "oAuthKey": $rootScope.key
            },
            data: data
        }).success(function (data) {
            $scope.showLoader = false;
            console.log(data);
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
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.pinPromotion = function ($scope, data,callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/PinVendorPromotion",
            data: data
        }).success(function (data) {
            console.log(data);
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.savePromotion = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateVendorPromotions",
            data: data
        }).success(function (data) {

            callback(true, data);
            console.log(data);
        }).error(function (data) {
            callback(false, data);
            console.log(data);
        });;
    };

    this.DeletePromotion = function ($scope,$rootScope, promotionCodeId, callback) {
       // alert("IDt :" + promotionCodeId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/DeletePromotion",
            headers: {
                "oAuthKey": $rootScope.key
            },
            data: { promotionCodeId: promotionCodeId, vendorId: $rootScope.SessionVendorId }
        }).success(function (result) {
            callback(true, result);
            // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false, result);
            // alert("Record Not Successfully Server Error !");
        });;
    };

    this.deActivatePromotion = function ($scope, $rootScope, promotionCodeId, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/DeActivatePromotion" + promotionCodeId + "," + $rootScope.SessionVendorId,
            headers: {
                "oAuthKey": $rootScope.key
            },
          
        }).success(function (result) {
            callback(true, result);
            // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false, result);
            // alert("Record Not Successfully Server Error !");
        });;
    };
    

}]);
