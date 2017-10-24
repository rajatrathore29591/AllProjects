'use strict';
app.service('aboutService', ['$http', function ($http) {
    this.getAboutContent = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent About Us,Vendor App," + $scope.tempLanguage
        }).success(function (data) {
            $scope.about = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };
}]);