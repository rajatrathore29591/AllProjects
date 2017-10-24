'use strict';

app.controller('translationController', ['$scope', '$rootScope', 'translationService', '$cookies', '$cookieStore', '$sce', '$location', function ($scope, $rootScope, translationService, $cookies, $cookieStore, $sce, $location) {
    $rootScope.isLogged = 1;
    $rootScope.active = "ContentManagement";
    $scope.data = {};
    $scope.isbusy = 1;
    $scope.showLoader = 1;

    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }
   
    $scope.english = function () {
        $cookieStore.put('Language', 'english');
        $scope.lang = $cookieStore.get('Language');
        translationService.getTranslation($scope, $rootScope, function (data) {
            $rootScope.translation = data;
          
        });
    }

    $scope.thai = function () {
        $cookieStore.put('Language', 'thai');
        $scope.lang = $cookieStore.get('Language');
        translationService.getTranslation($scope, $rootScope, function (data) {
            $rootScope.translation = data;
            
        });
    }

}])




