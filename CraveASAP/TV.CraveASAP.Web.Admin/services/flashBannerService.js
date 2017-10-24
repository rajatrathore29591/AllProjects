'use strict';
app.service('flashBannerService', ['$http', function ($http) {
   
    this.save = function ($scope, callback) {
        
        return $http({
            
            method: "POST",
            url: $scope.config.api + "/CreateBanner",
           // data: { type: 1, imageURL: $scope.imgBase64, delay: $scope.delay, reference: $scope.reference,platfrom:"flash",language:"English" }
            data: $scope.AllBanners
        }).success(function (data) {
            
            callback(true,data);
            //console.log("How IT Work" + JSON.stringify(data));
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };
    this.Deleteflash = function (flashId, $scope, callback) {
       // alert("IDtHello :" + flashId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/Deleteflash",
            data: { bannerId: flashId }
        }).success(function (result) {
            //console.log("data====", result);
            callback(true, result);
            // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false, result);
            // alert("Record Not Successfully Server Error !");
        });;
    };
    this.GetAllBanners = function ($scope,callback) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllBanners 1,flash,English"
        }).success(function (data) {
            //console.log(data);
            callback(true, data);
            //console.log("BannerDATA" + JSON.stringify(data));

        }).error(function (data) {
            console.log(data);
            callback(true, data);
        });;
    };

    this.GetAllBannerWorks = function ($scope, callback) {
        alert($scope.language)
        alert($scope.platform)
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllBanners 2" + ","+ $scope.platform + "," + $scope.language,
        }).success(function (data) {
            //$scope.howitWork = data;
            callback(true,data);
           
        }).error(function (data) {
            console.log(data);
            callback(false, data);
        });;
    };

    this.GetAllDealApp = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllDealApp"
        }).success(function (data) {
            $scope.landing = data;
            

        }).error(function (data) {
            console.log(data);
        });;
    };
    
}]);