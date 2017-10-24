'use strict';
app.service('Analytic_UserPerDayUseService', ['$http', function ($http) {
   
    this.GetPerDayUses = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetUserPerDayUse",
        }).success(function (data) {
            //$scope.promotion = data;
            callback(true, data);
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };


}]);
