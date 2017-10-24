'use strict';
app.service('howItWorkService', ['$http', function ($http) {
    this.getAllHowItWork = function ($scope, callback) {
      
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllBanners 2, Vendor App," + $scope.languageParam,
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };

}]);