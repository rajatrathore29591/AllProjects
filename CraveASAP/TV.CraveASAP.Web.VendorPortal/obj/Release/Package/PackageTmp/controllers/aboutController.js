'use strict';

app.controller('aboutController', ['$scope', '$rootScope', 'aboutService', '$cookies', '$cookieStore', '$sce', '$location', 'translationService', function ($scope, $rootScope, aboutService, $cookies, $cookieStore, $sce, $location, translationService) {
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

    //$cookieStore.put('Language', 'english');
    $scope.english = function () {
        $cookieStore.put('Language', 'english');

    }

    $scope.thai = function () {
        $cookieStore.put('Language', 'thai');
    }
    $scope.lang = $cookieStore.get('Language');
    if ($scope.lang == "english".trim()) {
        $scope.tempLanguage = "English";
    }
    else {
        $scope.tempLanguage = "Thai";
    }
    translationService.getTranslation($scope, $rootScope, function (success, data) {
        if (success) {

        }
        else
            alert('Some Error Occured');
    });

    aboutService.getAboutContent($scope, function (success, data) {
        if (success) {
            $scope.about = data.data;
            $scope.aboutContent = $scope.about[0].pageContent;
            $scope.showLoader = 0;
            $scope.isbusy = 0;
        }
        else
            alert('Some Error Occured');
    });

    $scope.deliberatelyTrustDangerousSnippet = function (item) {
        return $sce.trustAsHtml(item);
    };
}])




