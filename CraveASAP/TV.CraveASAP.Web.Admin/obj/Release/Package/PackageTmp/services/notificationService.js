'use strict';
app.service('notificationService', ['$http', function ($http) {
    this.SendManualNotification = function ($scope, data, callback) {
      return $http({
            method: "POST",
            url: $scope.config.api + "/NotificationManual",
            data: data
        }).success(function (data) {
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.GetPredictiveNotification = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetPredictiveNotication",
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };
  
    this.SendPredictiveNotification = function ($scope, data, callback) {
        alert(JSON.stringify(data));
        return $http({
            method: "POST",
            url: $scope.config.api + "/PredictiveNotication",
            data: data
        }).success(function (data) {
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

}]);