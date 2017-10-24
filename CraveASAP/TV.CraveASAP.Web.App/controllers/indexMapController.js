app.controller('indexMapController', ['$scope', 'indexMapService', 'initLoader', function ($scope, indexMapService, initLoader) {
    {
        indexMapService.GetMapLocation($scope);
        indexMapService.GetConfiguration($scope);
    }
}])

app.service('indexMapService', ['$http', 'initLoader', function ($http, initLoader) {

    this.GetMapLocation = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetMapLocation"
        }).success(function (data) {
            $scope.MapLocation = data;
        }).error(function (data) {
            console.log(data);
        });;
    };

    this.GetConfiguration = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetConfiguration"
        }).success(function (data) {
            $scope.ConfigData = data;
          initLoader.facebook($scope.ConfigData[6].value);
          //initLoader.instagrams($scope.ConfigData[10].value);

        }).error(function (data) {
            console.log(data);
        });;
    };
}]);
