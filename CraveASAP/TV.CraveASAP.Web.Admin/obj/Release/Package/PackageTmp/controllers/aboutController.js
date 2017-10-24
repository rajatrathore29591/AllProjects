'use strict';

app.controller('aboutController', ['$scope', '$rootScope', '$cookies', '$cookieStore', 'aboutService', '$sce', 'initData', function ($scope, $rootScope, $cookies, $cookieStore, aboutService, $sce, initData) {

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
       // initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.checked = "checked";
    $rootScope.isLogged = 1;
    $rootScope.active = "ContentManagement";
    $rootScope.active1 = "About";
    $scope.data = {};
    $scope.showLoader = true;
    $scope.contentFor = "All";
    aboutService.getAllContentByType($scope, function (success, data) {
        if (success) {
            alert("hi");
            $scope.about = data.data;
        }
        else
            alert('Some Error Occured');
    });

    $scope.deliberatelyTrustDangerousSnippet = function (item) {
        return $sce.trustAsHtml(item);
    };

    $scope.saveAbout = function () {
        if ($scope.content.language == "English".trim()) {

            $scope.radio = "English";
        }
        else {
            $scope.radio = "Thai";
        }
        var data = {
            pageName: "About Us", language: $scope.radio, pageContent: $scope.content.pageContent,
            contentFor: $scope.content.contentFor

        };
        aboutService.saveAboutContent($scope, data);

        location.reload();
    }

    $scope.radioChange = function () {
        $scope.showLoader = true;
        if ($scope.content.contentFor != null) {
            if ($scope.content.language == "English".trim()) {

                $scope.radio = "English";
            }
            else {

                $scope.radio = "Thai";
            }
            aboutService.getAboutContent($scope, function (success, data) {
                if (success) {
                    $scope.showLoader = false;
                    $scope.about = data.data;
                    $scope.aboutContent = $scope.about[0].pageContent;
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
        aboutService.getAboutContent($scope, function (success, data) {
            if (success) {
                $scope.showLoader = false;
                $scope.about = data.data;
                $scope.aboutContent = $scope.about[0].pageContent;

            }
            else
                alert('Some Error Occured');
        });

    }

}])

