'use strict';
app.service('userService', ['$http', function ($http) {
    this.getUsers = function ($scope) {
        return $http({
            method: "GET",
            url: "http://localhost:5000/api/user"
        }).success(function (data) {
            $scope.users = data;
            console.log(data);
        }).error(function (data) {
            console.log(data);
        });;
    };

    this.saveUsers = function ($scope) {
        return $http({
            method: "POST",
            url: "http://localhost:5000/api/user",
            data: { FirstName: 'Test', LastName: 'Test', Email: 'abc', Password: '123' }
        }).success(function (data) {
            console.log(data);
        }).error(function (data) {
            console.log(data);
        });;
    };
}]);