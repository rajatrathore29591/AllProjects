'use strict';

app.controller('termConditionController', ['$scope', '$rootScope', 'termService', '$sce', '$cookies', '$cookieStore', '$location', 'translationService', function ($scope, $rootScope, termService, $sce, $cookies, $cookieStore, $location, translationService) {
    $rootScope.isLogged = 1;
    $scope.data = {};
    $scope.isbusy = 1;
    $scope.showLoader = 1;
    $rootScope.active = 'ContentManagement';
    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }
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

    termService.getTermContent($scope, function (success, data) {
        if (success) {

            $scope.term = data.data;
            $scope.termContent = $scope.term[0].pageContent;
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
