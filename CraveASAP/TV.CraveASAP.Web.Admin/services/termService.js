'use strict';
app.service('termService', ['$http', function ($http) {

    this.getTermContent = function ($scope, callback) {
        console.log();
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent Terms and Condition," + $scope.content.contentFor + "," + $scope.radio,
            headers: {
                "oAuthKey": "Admin"
            },
            //data: { page: "About Us", content: $scope.contentFor }
        }).success(function (data) {

            $scope.term = data;
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getAllHTMLContentById = function ($scope, data, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllHTMLContentById " + $scope.contentId,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.content = data;
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.getAllContentByType = function ($scope, data) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContentByType Terms and Condition",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.term = data.data;
            $scope.showLoader = false;
            //callback(true, data);
        }).error(function (data) {
            //callback(false, data);
        });;
        
        $("#taTextElement").addClass("col-md-12");
    };

    this.updatePrivacyContent = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/UpdateHTMLContent ",
            headers: {
                "oAuthKey": "Admin"
            },
            //url: "http://craveservices.techvalens.net/CraveServices.svc/UpdateHTMLContent",
            data: data
        }).success(function (data) {
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

    this.saveTermContent = function ($scope, data, callback) {
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateHTMLContent",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
            console.log("data", data);
        }).error(function (data) {
            callback(false, data);
            console.log("data", data);
        });;
    };

}]);