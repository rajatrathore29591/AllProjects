﻿'use strict';

app.controller('updateAboutController', ['$scope', '$location', '$stateParams', 'aboutService', '$cookies', '$cookieStore', '$rootScope','initData', function ($scope, $location, $stateParams, aboutService, $cookies, $cookieStore, $rootScope,intiData) {

    $scope.contentId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.data = {};
    loadContent();
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.updateAbout = function () {
        if ($scope.content.language == "English".trim()) {

            $scope.radio = "English";
        }
        else {

            $scope.radio = "Thai";
        }
        var data = {
            Id: $scope.content.Id, pageName: "About Us", language: $scope.radio, pageContent: $scope.content.pageContent,
            contentFor: $scope.content.contentFor

        };
        aboutService.updateAboutContent($scope, data, function (success, data, callback) {
            if (success) {
                $location.path("/Main/About");
            }
            else
                console.log("failed", data);
        });
    }

    $scope.cancel = function () {
        $location.path("/Main/About");
    }

    function loadContent() {

        aboutService.getAllHTMLContentById($scope, $scope.contentId, function (success, data) {
            if (success) {
                $scope.content = data.data1;
                
            }
            else
                console.log("failed", data);
        });
        
        $("#taTextElement").addClass("col-md-12");

    }

}])