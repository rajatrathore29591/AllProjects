'use strict';
app.service('landingService', ['$http', function ($http) {
    this.getAllLanding = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetLandingDetails",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            callback(true, data);
            $scope.landing = data.data;
        }).error(function (data) {
            callback(false, data);
        });;
    };

    this.Update = function (landingPageId, $scope, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateLandingDetails",
            headers: {
                "oAuthKey": "Admin"
            },
            data: { landingPageId: landingPageId }
        }).success(function (data) {
            $scope.Status = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    }
}]);