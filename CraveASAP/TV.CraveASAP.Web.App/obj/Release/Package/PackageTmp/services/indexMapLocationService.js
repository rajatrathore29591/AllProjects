'use strict'; 
app.service('indexMapLocationService', ['$http', function ($http) {
    alert("hi");
    this.GetMapLocation = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetMapLocation"
        }).success(function (data) {
            $scope.landingLocationMap = data;
            console.log("GetMapLocation" + JSON.stringify(data));

        }).error(function (data) {
            console.log(data);
        });;
    };
}]);