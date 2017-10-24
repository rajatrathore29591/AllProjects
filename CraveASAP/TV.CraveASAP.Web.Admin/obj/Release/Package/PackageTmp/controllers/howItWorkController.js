'use strict';
app.controller('howItWorkController', ['$scope', '$rootScope', 'howItWorkService', 'flashBannerService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, howItWorkService, flashBannerService, $cookies, $cookieStore,initData) {
    //alert("Go");
    $rootScope.isLogged = 1;
    $scope.title = "Vendor App";
    $scope.checked = "checked";
    $scope.file = '';
    $rootScope.active = 'ContentManagement';
    $rootScope.active1 = "howItsWork";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.save = function () {
        $scope.showLoader = true;
        var allSrcs = Array();
        $scope.AllBanners = [];
        if ($scope.platform == "Vendor App".trim()) {
            $scope.video = "";
            $("#howItsWorkImagesVendor img").each(function () {
                allSrcs.push($(this).attr('src'));
                $scope.imgBase64 = $(this).attr('src');
                $scope.imgBase64 = $scope.imgBase64.replace('data:image/jpeg;base64,', "");
                $scope.AllBanners.push({ type: 2, imageURL: $scope.imgBase64, delay: "", reference1: $scope.video, platform: $scope.platform, language: $scope.language });
            });
            howItWorkService.save($scope);

        }
        else if ($scope.platform == "User App".trim()) {
            $scope.video = "";
            $scope.imgBase64 = "";
            $("#howItsWorkImagesUser img").each(function () {
                allSrcs.push($(this).attr('src'));
                $scope.imgBase64 = $(this).attr('src');
                $scope.imgBase64 = $scope.imgBase64.replace('data:image/jpeg;base64,', "");
                $scope.AllBanners.push({ type: 2, imageURL: $scope.imgBase64, delay: "", reference1: $scope.video, platform: $scope.platform, language: $scope.language });
            });
            howItWorkService.save($scope);
        }
        else {
            $scope.img = "video";
            howItWorkService.save($scope);
        }
    }

    /*---  Video Upload Start  --*/

    $scope.data = 'none';
    $scope.add = function () {
        var f = document.getElementById('file').files[0],
            r = new FileReader();
        r.onloadend = function (e) {
            $scope.data = e.target.result;
        }
        r.readAsBinaryString(f);
    }
    /*---  Video Upload End  --*/

    $scope.select = function () {
        //alert($scope.platform);
        if ($scope.platform == "Vendor App" || $scope.platform == "User App") {
            $scope.hide = false;
        }
        else {
            $scope.hide = true;;
        }

        if ($scope.language  == undefined) {

            $scope.language = "English";
        }
       
        if ($scope.language !== undefined) {
            $scope.showLoader = true;
           
            if ($scope.platform == "Vendor App") {
               
                $scope.title = "Vendor App";
                $scope.platform = "Vendor App";
            }
            else if ($scope.platform == "User App") {
                $scope.title = "User App";
                $scope.platform = "User App";
            }
            else {
                
                $scope.title = "Website";
                $scope.platform = "Website";
            }

            flashBannerService.GetAllBannerWorks($scope, function (success, data) {

                $scope.howitWork = data;
                $scope.showLoader = false;
            });
        }
    }

    $scope.radioChange = function () {
        if ($scope.platform !== undefined) {
            $scope.showLoader = true;
            if ($scope.platform == "Vendor App") {
                $scope.title = "Vendor App";
                $scope.platform = "Vendor App";
            }
            else if ($scope.platform == "User App") {
                $scope.title = "User App";
                $scope.platform = "User App";
            }
            else if ($scope.platform == "Website") {
                $scope.title = "Website";
                $scope.platform = "Website";
            }

            flashBannerService.GetAllBannerWorks($scope, function (success, data) {
                $scope.howitWork = data;
                $scope.showLoader = false;
                console.log("daatta", $scope.howitWork);
            });
        }

    }
    $scope.delete = function (imgId) {
        $scope.showLoader = true;
        $scope.Message = "Image has been successfully deleted";
        howItWorkService.DeleteImg(imgId, $scope, function (success, data) {
            if (success) {
                $("#static").modal('show');
                $scope.showLoader = false;

            }
            else {
                console.log("failed", data);
            }
        });
    }



}])

