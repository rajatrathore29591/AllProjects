'use strict';
app.service('loginService', ['$http', function ($http) {
    this.loginAuth = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/VendorLogin" + $scope.loginName + "," + $scope.password + "," + 'xk0OeXBLQzjk' + "," + 'VendorPortal'
        }).success(function (data) {
            $scope.about = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };

    this.sendForgetPassword = function ($scope,data,callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/ForgotVendorPassword",
            data:data
        }).success(function (data) {
            $scope.about = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };


}]);