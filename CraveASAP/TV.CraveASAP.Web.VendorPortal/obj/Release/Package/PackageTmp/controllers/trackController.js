'use strict';

app.controller('trackController', ['$scope', '$rootScope', 'manageActivePromotionService', '$cookies', '$cookieStore', '$location', 'translationService', function ($scope, $rootScope, manageActivePromotionService, $cookies, $cookieStore, $location, translationService) {
    
    $rootScope.isLogged = 1;
    $scope.selectedTab = 2;
    $rootScope.key = $cookieStore.get('oAuthkey');
    $rootScope.SessionVendorId = $cookieStore.get('loggedId');
    $scope.vendorID = $cookieStore.get('loggedId');
    //alert($scope.vendorID);
    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }

    $scope.english = function () {
        $cookieStore.put('Language', 'english');

    }

    $scope.thai = function () {
        $cookieStore.put('Language', 'thai');
    }
    $scope.lang = $cookieStore.get('Language');
    translationService.getTranslation($scope, $rootScope, function (success, data) {
        if (success) {

        }
        else
            alert('Some Error Occured');
    });

    $scope.showLoader = true;
   
    $scope.count = 0;
    $scope.nextHide = true;
    $scope.next = function () {
        //$scope.nextHide = true;
        if (promotionID.length > 1) {
            $scope.preHide = true;
        }
        if (promotionID.length - 2 > $scope.count) {
            $scope.nextHide = true;
        }
        else {
            $scope.nextHide = false;
        }
        if (promotionID.length - 1 > $scope.count) {
           
            $scope.count += 1;
            $scope.promotionNextPre = promotionID[$scope.count];
            //alert($scope.promotionNextPre);
        }
        else {
        }
        
    }

    $scope.pre = function () {
       
        if ($scope.count > 1) {
            $scope.preHide = true;
        }
        else {
            $scope.preHide = false;
        }
        if ($scope.count > 0) {
            $scope.nextHide = true;
            $scope.count -= 1;
            $scope.promotionNextPre = promotionID[$scope.count];
        }
        else {
        }
    }

    var promotionID = [];
    manageActivePromotionService.getActivePromotionByVendorId($scope, $rootScope, $scope.vendorID, function (success, data) {
        if (data.statusCode == 1) {
            $scope.promotion = data.data;

            for (var i in $scope.promotion) {
                var temp = { promotionCodeId: '' };
                temp.promotionCodeId = $scope.promotion[i];
                promotionID.push(temp);
                
                $scope.promotionNextPre = promotionID[$scope.count];
            }
            $scope.showLoader = false;
        }
        else {
           
            $scope.showLoader = false;
        }
    });
   
}])

