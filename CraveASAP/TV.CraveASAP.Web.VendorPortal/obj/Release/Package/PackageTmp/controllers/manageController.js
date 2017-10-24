'use strict';

app.controller('manageController', ['$scope', '$rootScope', 'manageActivePromotionService', '$cookies', '$cookieStore', '$location', 'translationService', function ($scope, $rootScope, manageActivePromotionService, $cookies, $cookieStore, $location, translationService) {
    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }
    $rootScope.key = $cookieStore.get('oAuthkey');
    $rootScope.isLogged = 1;
    $scope.selectedTab = 3;
    $scope.showLoader = true;
    $rootScope.SessionVendorId = $cookieStore.get('loggedId');
   
    getPromotion();
    
    $scope.english = function () {
        $cookieStore.put('Language', 'english');
       
    }

    $rootScope.thai = function () {
        $cookieStore.put('Language', 'thai');
        
    }

    $rootScope.lang = $cookieStore.get('Language');
   
    translationService.getTranslation($scope, $rootScope, function (data) {
       
        $rootScope.translation = data;
    });

    function getPromotion()
    {
        manageActivePromotionService.getPromotionByVendorId($scope, $rootScope, function (success, data) {
            if (data.statusCode == "1") {
                $scope.showLoader = false;
                $scope.promotion = data.data;
                $cookieStore.put('serverTime', $scope.promotion.serverTime);
                
            }
            else
            $scope.showLoader = false;
        });
    }
   
    $scope.pinnedIt = function (promotionCodeId) {
        var data = { promotionCodeId: promotionCodeId };

        manageActivePromotionService.pinPromotion($scope, data, function (success, data) {

            if (success) {
                location.reload();
            }
            else {
                console.log("failed",data);
            }

        });
    }

    $scope.DeleteOld = function (promotionCodeId) {
        $scope.Message = $scope.translation.Yourpromotionhasbeenremovedsuccessfully;
        $scope.Message1 = $scope.translation.Areyousurewanttoremovethispromotion;
        $scope.sure = function () {
            manageActivePromotionService.DeletePromotion($scope, $rootScope, promotionCodeId, function (success, data) {
                if (success) {
                    var index = promotionCodeId;

                    for (var i in $scope.promotion) {
                        if ($scope.promotion[i].promotionCodeId == promotionCodeId) {
                            $scope.promotion.splice(i, 1);
                        }
                    }
                }
                else {
                    console.log("failed", data);
                }
            });
            $("#static").modal('show');
        }
        $("#delete").modal('show');
    }

    $scope.DeactivatePromotion = function (promotionCodeId) {
        $scope.Message = $scope.translation.Yourpromotionhasbeencancelledsuccessfully;
        $scope.Message1 = $scope.translation.Areyousurewanttocancelthispromotion;
        $scope.sure = function () {
            manageActivePromotionService.deActivatePromotion($scope, $rootScope, promotionCodeId, function (success, data) {
                if (success) {
                    var index = promotionCodeId;
                }
                else {
                    console.log("failed", data);
                }
            });
            $("#static").modal('show');
            $scope.redirect = function () {
                location.reload();
            }
        }
        $("#delete").modal('show');
    }

    $scope.pin = function () {


    }

}]);

