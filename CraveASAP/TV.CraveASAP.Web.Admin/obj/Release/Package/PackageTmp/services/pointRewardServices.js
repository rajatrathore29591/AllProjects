'use strict';
app.service('pointRewardServices', ['$http', function ($http) {

    this.getPointConfiguration = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllPointConfiguration",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true ,data);
        }).error(function (data) {
            callback(false ,data);
            console.log("data error", data);
        });;
    };


    this.Update = function ($scope, callback) {



        //alert("Services Call" +$scope.pointsEarned[$scope.i]  + $scope.limit[$scope.i] + $scope.j);
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdatePointsRewards",
            headers: {
                "oAuthKey": "Admin"
            },
            data: { limit: $scope.limit[$scope.i], pointsEarned: $scope.pointsEarned[$scope.i], ptConfigurationId: $scope.j }
        }).success(function (data) {
           
            $scope.Status = data;

           
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;


    };

}]);