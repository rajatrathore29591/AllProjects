'use strict';
app.controller('flashBannerController', ['$scope', '$rootScope', 'flashBannerService', '$cookies', '$cookieStore', 'initData', '$location', function ($scope, $rootScope, flashBannerService, $cookies, $cookieStore, initData, $location) {
    $rootScope.active = "ManageApplication";
    $rootScope.active1 = "Banner";
    $rootScope.isLogged = 1;
    $scope.showLoader = true;

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
       
    $scope.save = function () {
        $scope.AllBanners = [];
        //$scope.showLoader = true;
        var allSrcs = Array();
        $("#bannerImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/jpeg;base64,', "");
            $scope.AllBanners.push({ type: 1, imageURL: $scope.imgBase64, delay: $scope.delay, reference: $scope.reference, platform: "flash", language: "English" });

        });
        flashBannerService.save($scope, function (success, data) {
            //$scope.landing = data;
            //$scope.showLoader = false;
            location.reload();
        });
        
    }
    imageDelete();

    function imageDelete() {
        $scope.Message = "Flash Banner has been successfully deleted";
        $('body').on('click', '.del', function () {
            $('#static').modal('show');
            $(this).parent().remove();
        })
    }


    $scope.delete = function (flashId) {
        
        $scope.showLoader = true;
       
        flashBannerService.Deleteflash(flashId, $scope, function (success, data) {
            if (success) {
                $scope.Message = "Flash Banner has been successfully deleted";
                $("#static").modal('show');
                $scope.redirect = function () {

                    $scope.showLoader = false;
                }

            }
            else {
                console.log("failed", data);
            }
        });
    }
    flashBannerService.GetAllBanners($scope, function (success, data) {
        if (data != "") {
           
            $scope.landing = data;
            $scope.delay = $scope.landing[0].delay;
            //console.log($scope.landing[0].delay);
            $scope.showLoader = false;
        }
        else {
            $scope.Message = "No Data";
            $('#static').modal('show');
            $scope.redirect = function () {
                $scope.showLoader = false;
            }
           
        }
    });
}])
