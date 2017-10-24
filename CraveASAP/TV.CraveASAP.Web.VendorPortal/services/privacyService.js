'use strict';
app.service('privacyService', ['$http', function ($http) {
    this.getPrivacyContent = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent Privacy and Policy,Vendor App," + $scope.tempLanguage
        }).success(function (data) {
            $scope.privacy = data;
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    
}]);