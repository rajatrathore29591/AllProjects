'use strict';
app.service('analytics_MultipleUsageService', ['$http', function ($http) {

    this.GetTrackUsage = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetMultipleUsage",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };


}]);
