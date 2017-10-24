
app.controller('indexControllerDeal', ['$scope', 'indexServiceDeal', 'initLoader', function ($scope, indexService, initLoader) {
    {
        //$scope.limit = 16;
        
        indexService.GetAllDealWebApp($scope);
        $scope.path = $scope.config.api + "/Pictures/";
    }

}])
app.service('indexServiceDeal', ['$http', function ($http) {
   
    this.GetAllDealWebApp = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllDealWebApp"
        }).success(function (data) {
            $scope.landingDeal = data;

        }).error(function (data) {
            console.log(data);
        });;
    };
}]);