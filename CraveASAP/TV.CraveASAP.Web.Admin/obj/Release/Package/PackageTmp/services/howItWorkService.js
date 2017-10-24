'use strict';
app.service('howItWorkService', ['$http', function ($http) {

    this.save = function ($scope) {
        //alert(data);
        return $http({
            method: "POST",
            url: $scope.config.api + "/CreateBanner",
            //data: { type: 2, imageURL: $scope.img, delay: "", reference1: $scope.video, platform: $scope.platform, language: $scope.language }
            data: $scope.AllBanners
        }).success(function (data) {
            $scope.showLoader = false;
           
            //location.reload();
        }).error(function (data) {
            console.log(data);
        });;
    };

    this.GetAllBanners = function ($scope) {
        return $http({
            method: "GET",
            url: $scope.config.api + "/GetAllBanners 2" + $scope.language + "," + $scope.platform,
        }).success(function (data) {
            $scope.howitWork = data;
            
        }).error(function (data) {
            console.log(data);
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

    this.DeleteImg = function (imgId, $scope, callback) {
        // alert("IDtHello :" + flashId);
        return $http({
            method: "POST",
            url: $scope.config.api + "/Deleteflash",
            data: { bannerId: imgId }
        }).success(function (result) {
           
            callback(true, result);
            // alert("Record Deleted Successfully");
        }).error(function (result) {
            callback(false, result);
            // alert("Record Not Successfully Server Error !");
        });;
    };

}]);