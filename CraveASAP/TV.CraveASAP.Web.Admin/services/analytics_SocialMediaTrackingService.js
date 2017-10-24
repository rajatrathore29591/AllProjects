'use strict';
app.service('analytics_SocialMediaTrackingService', ['$http', function ($http) {
    this.GetSocialMedia = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetSocialMedia",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
  
}]);
