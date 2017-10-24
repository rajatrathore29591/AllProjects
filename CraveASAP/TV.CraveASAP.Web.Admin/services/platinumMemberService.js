'use strict';
app.service('platinumMemberService', ['$http', function ($http) {

    this.getRewardByType = function ($scope, callback) {

        return $http({
            method: "GET",
            url: $scope.config.api + "/GetALLRewardByType Prem",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getRewards = function ($scope, callback) {

        return $http({
            method: "GET",
            url: $scope.config.api + "/GetRewards",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.savePremium = function ($scope, data, callback) {

        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateReward",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getRewardById = function ($scope, rewardID, callback) {
      return $http({
            method: "GET",
            url: $scope.config.api + "/GetRewardById " + $scope.rewardID,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            //$scope.VendorDetail = data;
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getUserCount = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetUserCount " ,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            //$scope.VendorDetail = data;
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.getRandomUser = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetRandomUsers " + $scope.num,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            //$scope.VendorDetail = data;
            callback(true, data);
        }).error(function (data) {
            console.log("data", data);
            callback(false, data);
        });;
    };

    this.DeletReward = function ($scope, rewardId, callback) {
      
        return $http({
            method: "GET",
            url: $scope.config.api + "/DeleteReward" + rewardId,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (result) {
            callback(true, result);
            // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false, result);
            // alert("Record Not Successfully Server Error !");
        });;
    };

    this.updatePremium = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateReward",
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

}]);