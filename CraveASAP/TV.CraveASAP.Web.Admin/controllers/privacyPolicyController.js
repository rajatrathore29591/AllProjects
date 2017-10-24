'use strict';

app.controller('privacyPolicyController', ['$scope', 'privacyService', '$rootScope', '$sce', '$cookies', '$cookieStore','initData', function ($scope, privacyService, $rootScope, $sce, $cookies, $cookieStore,initData) {
    $scope.data = {};
    $rootScope.isLogged = 1;
    $scope.checked = "checked";
    $rootScope.active = "ContentManagement";
    $rootScope.active1 = "Privacy";
    $scope.showLoader = true;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.savePrivacy = function () {
        if ($scope.english == "English".trim()) {
            $scope.radio = "English";
        }
        else {
            $scope.radio = "Thai";
        }
        var data = {
            pageName: "Privacy and Policy", language: $scope.radio, pageContent: $scope.data.aboutContent,
            contentFor: $scope.contentFor
        };
        privacyService.savePrivacyContent($scope, data);
        location.reload();
    }

    privacyService.getAllContentByType($scope, function (success, data) {
        if (success) {
            alert("hi");
            $scope.privacy = data.data;
            
        }
        else
            alert('Some Error Occured');
    });

    $scope.deliberatelyTrustDangerousSnippet = function (item) {
        return $sce.trustAsHtml(item);
    };

    $scope.radioChange = function () {
        $scope.showLoader = true;
        if ($scope.content.contentFor != null) {
            if ($scope.content.language == "English".trim()) {

                $scope.radio = "English";
            }
            else {

                $scope.radio = "Thai";
            }
            privacyService.getPrivacyContent($scope, function (success, data) {
                if (success) {
                    $scope.privacy = data.data;
                    $scope.showLoader = false;
                }
                else
                    alert('Some Error Occured');
            });
        }
        else { $scope.showLoader = false; }
    }

    $scope.select = function () {
        $scope.showLoader = true;
        if ($scope.content.language == "English".trim()) {
            $scope.radio = "English";
        }
        else {
            $scope.radio = "Thai";
        }
        privacyService.getPrivacyContent($scope, function (success, data) {
            if (success) {
                $scope.privacy = data.data;
                $scope.showLoader = false;
            }
            else
                alert('Some Error Occured');
        });
    }

}])

