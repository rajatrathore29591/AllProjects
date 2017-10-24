'use strict';
app.service('subscribeService', ['$http', function ($http) {

    this.getSubscribeUsers = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetData"
        }).success(function (data) {
            $scope.subscribeUsers = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };

    this.saveVendor = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateVendor",
            data: data
        }).success(function (data) {
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };


    this.deleteSubscribeUsers = function ($scope, callback) {
        return $http({
            method: "DELETE",
            url: $scope.config.api + "/DeleteSubscribedUsers",
            data: { id: 4 }
        }).success(function (data) {
            console.log("data", data);
        }).error(function (data) {
            console.log("data", data);
        });;
    };
}]);