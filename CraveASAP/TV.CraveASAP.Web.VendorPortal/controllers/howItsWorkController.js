'use strict';

app.controller('howItsWorkController', ['$scope', '$rootScope', 'howItWorkService', '$cookies', '$cookieStore', '$sce', '$location', '$timeout', 'translationService', function ($scope, $rootScope, howItWorkService, $cookies, $cookieStore, $sce, $location, $timeout, translationService) {
   
    $scope.english = function () {
        $cookieStore.put('Language', 'english');
    }

    $scope.thai = function () {
        $cookieStore.put('Language', 'thai');
    }
    $scope.lang = $cookieStore.get('Language');

    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }
    if ($scope.lang == "english") {
        $scope.languageParam = "English";
    }
    else {
        $scope.languageParam = "Thai";
    }

    howItWorkService.getAllHowItWork($scope, function (success, data) {
        if (success) {
            $scope.howItWorkImage = data;
            console.log($scope.howItWorkImage)
            $scope.showLoader = 0;
        }
        else
            console.log("failed", data);
    });

    translationService.getTranslation($scope, $rootScope, function (success, data) {
        if (success) {

        }
        else
            alert('Some Error Occured');
    });

    $scope.$on('$viewContentLoaded', function (event) {
        $timeout(function () {
            $("#owl").owlCarousel({
                navigation: true, // Show next and prev buttons
                slideSpeed: 300,
                paginationSpeed: 400,
                singleItem: true,
                autoHeight: true

            });
        }, 1000);
    });
}])




