'use strict';
app.service('aboutService', ['$http', function ($http) {
    this.getAboutContent = function ($scope, callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContent About Us," + $scope.content.contentFor + "," + $scope.radio,
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.about = data;
            
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
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
        }).error(function (data) {
            callback(false, data);
        });;
        
        $("#taTextElement").addClass("col-md-12");  
    };

    this.getAllContentByType = function ($scope, data) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllContentByType About Us",
            headers: {
                "oAuthKey": "Admin"
            },
        }).success(function (data) {
            $scope.about = data.data;
            $scope.showLoader = false;
            //callback(true, data);
        }).error(function (data) {
           //callback(false, data);
        });
        
        $("#taTextElement").addClass("col-md-12");
    };

    this.updateAboutContent = function ($scope, data, callback) {
        return $http({
            method: "POST",
            //url: "http://localhost:3338/CraveServices.svc/UpdateHTMLContent",
            url: $scope.config.api + "/UpdateHTMLContent ",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };

    this.saveAboutContent = function ($scope, data, callback) {
        return $http({
            method: "POST",
            //url: "http://localhost:3338/CraveServices.svc/CreateHTMLContent",
            url: "http://craveservices.techvalens.net/CraveServices.svc/CreateHTMLContent",
            headers: {
                "oAuthKey": "Admin"
            },
            data: data
        }).success(function (data) {
            callback(true, data);
        }).error(function (data) {
            callback(false, data);
        });;
    };

}]);