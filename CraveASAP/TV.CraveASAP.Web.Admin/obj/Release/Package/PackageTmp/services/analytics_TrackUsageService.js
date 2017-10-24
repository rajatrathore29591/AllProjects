'use strict';
app.service('analytics_TrackUsageService', ['$http', function ($http) {
   
    this.GetTrackUsage = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetTrackUsage",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

 
}]);
