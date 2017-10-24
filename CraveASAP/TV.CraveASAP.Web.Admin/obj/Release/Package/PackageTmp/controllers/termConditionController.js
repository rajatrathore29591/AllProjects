'use strict';

app.controller('termConditionController', ['$scope', '$rootScope', 'termService', '$sce', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, termService, $sce, $cookies, $cookieStore,initData) {
    $rootScope.isLogged = 1;
    $scope.checked = "checked";
    $scope.data = {};
    $scope.showLoader = true;
    $rootScope.active = 'ContentManagement';
    $rootScope.active1 = "Term";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.saveTerm = function () {
        if ($scope.english == "English".trim()) {
            $scope.radio = "English";
        }
        else {
            $scope.radio = "Thai";
        }
        var data = {
            pageName: "Terms and Condition", language: $scope.radio, pageContent: $scope.data.aboutContent,
            contentFor: $scope.contentFor

        };
        termService.saveTermContent($scope, data);
        location.reload();
    }

    termService.getAllContentByType($scope, function (success, data) {
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
            termService.getTermContent($scope, function (success, data) {
                if (success) {
                    $scope.term = data.data;
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
        termService.getTermContent($scope, function (success, data) {
            if (success) {
                $scope.term = data.data;
                $scope.showLoader = false;
            }
            else
                alert('Some Error Occured');
        });
    }
}])
