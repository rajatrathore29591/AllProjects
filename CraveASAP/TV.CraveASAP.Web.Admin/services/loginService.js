'use strict';
app.service('loginService', ['$http', function ($http) {
    this.loginAuth = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/SuperAdminLogin" + $scope.loginName + "," + $scope.password,
        }).success(function (data) {
            $scope.about = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };



  
}]);