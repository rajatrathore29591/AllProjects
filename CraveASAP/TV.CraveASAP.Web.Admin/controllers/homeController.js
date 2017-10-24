'use strict';

app.controller('homeController', ['$scope', 'userService', '$cookies', '$cookieStore', function ($scope, userService,$cookies, $cookieStore) {
    $scope.message = "Now viewing home!";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedUserName');
        $location.path('/Dashboard');
    }
    userService.getUsers($scope);

    $scope.CreatNew = function () {
        userService.saveUsers($scope);
        userService.getUsers($scope);
    }
}])

