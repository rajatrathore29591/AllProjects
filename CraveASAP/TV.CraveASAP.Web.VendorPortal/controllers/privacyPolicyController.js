'use strict';

app.controller('privacyPolicyController', ['$scope', 'privacyService', '$rootScope', '$sce', '$cookies', '$cookieStore', '$location', 'translationService', function ($scope, privacyService, $rootScope, $sce, $cookies, $cookieStore, $location, translationService) {
    $scope.data = {};
    $rootScope.isLogged = 1;
    $rootScope.active = "ContentManagement";
    $scope.isbusy = 1;
    $scope.showLoader = 1;
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

    privacyService.getPrivacyContent($scope, function (success, data) {
        if (success) {
            $scope.privacy = data.data;
            $scope.privacyContent = $scope.privacy[0].pageContent;
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

