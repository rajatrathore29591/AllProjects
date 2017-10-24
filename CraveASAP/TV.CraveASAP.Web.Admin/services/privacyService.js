'use strict';
app.service('privacyService', ['$http', function ($http) {
    //alert("Privacy");
    this.getPrivacyContent = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent Privacy and Policy," + $scope.content.contentFor + "," + $scope.radio,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.privacy = data;
            callback(true, data);
            
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
            url: $scope.config.api + "/GetAllContentByType Privacy and Policy",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.privacy = data.data;
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
           // url: "http://localhost:3338/CraveServices.svc/UpdateHTMLContent",
            url: $scope.config.api + "/UpdateHTMLContent ",
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

    this.savePrivacyContent = function ($scope, data, callback) {
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