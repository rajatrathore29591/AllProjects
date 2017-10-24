'use strict';
app.service('analytics_ActivePromotionService', ['$http', function ($http) {
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
    this.GetAllVendors = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllVendors",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.GetAllChart = function ($scope, callback) {

        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllChart",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.GetAllChartByid = function ($scope, callback) {
        //alert("Go" + $scope.vendorId)
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllChartByVendorID" + $scope.vendorId + "," + $scope.fromdate + "," + $scope.toDate,
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.GetAllChartByTime = function ($scope, callback) {
        console.log("/CraveServices.svc/GetAllChartByTime'" + $scope.vendorId + "','" + $scope.categoryId + "','" + $scope.time + "','" + $scope.fromdate + "'," + $scope.toDate);
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllChartByTime " + $scope.vendorId + "," + $scope.categoryId + "," + $scope.time + "," + $scope.fromdate + "," + $scope.toDate,
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
}]);
