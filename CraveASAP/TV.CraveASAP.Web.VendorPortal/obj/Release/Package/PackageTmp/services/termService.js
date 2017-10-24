'use strict';
app.service('termService', ['$http', function ($http) {

    this.getTermContent = function ($scope, callback) {
        console.log();
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent Terms and Condition,Vendor App," + $scope.tempLanguage
        }).success(function (data) {
            $scope.term = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

   
}]);