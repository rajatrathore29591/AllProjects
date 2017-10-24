'use strict';
app.controller('createPromotionController', ['$scope', '$rootScope', 'manageActivePromotionService', '$filter', '$stateParams', '$cookies', '$cookieStore', '$location', '$localStorage', 'initData', 'translationService', function ($scope, $rootScope, manageActivePromotionService, $filter, $stateParams, $cookies, $cookieStore, $location, $localStorage, initData, translationService) {
    $rootScope.categoryId = $cookieStore.get('categoryId');
    $rootScope.key = $cookieStore.get('oAuthkey');
    $scope.currentPromotionId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.currentStatus = ($stateParams.status == 'Relaunch') ? $stateParams.status : 'Edit';
    $rootScope.isLogged = 1;
    $scope.selectedTab = 1;
    $scope.image = "Default.png";
    $scope.vendor_Id = $cookieStore.get('loggedId');
    $rootScope.btnLabel = "Confirm";
    $scope.showLoader = true;
    $scope.promotion = {};
    $rootScope.translation = {};
    LoadAllCategory();

    /*Refresh on Thank U Screen Start*/
    var url = window.location.href;
    var thanku = "/Main/ThankYou";
    var tempUrl = "http://eddee.cloudapp.net/eddeevendor" + "/#" + thanku;
    //var tempUrl = "http://localhost:25215" + "/#" + thanku;
    if ($rootScope.confirm == undefined)
        if (tempUrl == url) {
            $location.path('/Main/CreatePromotion/0/Edit');
        }

    /*Refresh on Thank U Screen End*/
   
    $scope.lang = $cookieStore.get('Language');
   
    translationService.getTranslation($scope, $rootScope, function ( data) {
        $rootScope.translation = data;
    });
   
    $scope.confirmInit = function () {
        $scope.promotion = null;
        //$scope.confirm = {};
        var tempId = $cookieStore.get('promoId');
        var url1 = window.location.href;
        var beforeThanku = "/Main/Confirm";
        var tempUrl1 = $scope.config.api + "/#" + beforeThanku;
        if (tempId > 0) {
            $rootScope.createUpdate = "Update";
            if ($scope.confirm != undefined) {

                $rootScope.rndCode = $scope.confirm.code;
            }
        }
        if ($scope.currentPromotionId == 0 && tempId == undefined) {
            $rootScope.createUpdate = "Save";
        }


        if ($rootScope.confirm != undefined) {
            if (tempId == undefined) {

                $rootScope.createUpdate = "Save";
                $rootScope.rndCode = $scope.confirm.code;
                $scope.randomCode = $scope.confirm.code;
            }

            $cookieStore.put('confirmCatId', $scope.confirm.categoryId);
            $cookieStore.put('confirmSubId', $scope.confirm.subCategoryId);
            $cookieStore.put('confirmOptId', $scope.confirm.optCategoryId);
            $rootScope.vendorLat = $cookieStore.get('VendorLat');
            $rootScope.vendorLong = $cookieStore.get('VendorLong');
            plotMap();
            $scope.promotion = $rootScope.confirm;

            $rootScope.vendorImg = $cookieStore.get('VendorLocal');
            $rootScope.vendorShort = $cookieStore.get('VendorShort');

            $scope.createdDate = convertTo24Hour($rootScope.confirm.createdDate);
            $scope.expiryDate = convertTo24Hour($rootScope.confirm.expiryDate);
            if ($rootScope.confirm.id == 0) {
                $scope.createUpdate = "Save";
            }
            if ($rootScope.createUpdate == "Save".trim()) {
                if ($scope.createdDate != '' && $scope.expiryDate != '') {
                    if (ComputedDate() >= 1) {
                        $rootScope.DateQuantity = "End in " + ComputedDate() + " Days";
                    }
                    else {
                        $rootScope.DateQuantity = "End in " + ComputedDate();
                    }
                }
                else {
                    if ($scope.confirm.quantity == 1) {
                        $rootScope.DateQuantity = "Only " + $scope.confirm.quantity + " Left";
                    }
                    else {
                        $rootScope.DateQuantity = "Only " + $scope.confirm.quantity + " Left";
                    }
                }
            }

            else {

                if ($scope.createdDate != '' && $scope.expiryDate != '') {
                    if (ComputedDate() >= 1) {
                        $rootScope.DateQuantity = "End in " + ComputedDate() + " Days";
                    }
                    else {
                        $rootScope.DateQuantity = "End in " + ComputedDate();
                    }
                }
                else {

                    if ($scope.confirm.quantity == 1) {
                        $rootScope.DateQuantity = "Only " + $scope.confirm.quantity + " Left";
                    }
                    else {
                        $rootScope.DateQuantity = "Only " + $scope.confirm.quantity + " Left";
                    }
                }
                //alert($rootScope.createUpdate)
            }

            ComputedDate();
            $scope.promoImage = $rootScope.confirm.imggggg;
            $scope.imgBase64 = $scope.promoImage.replace('data:image/png;base64,', "");
            if ($scope.imgBase64.length > 150) {
                $scope.img = $scope.imgBase64;
            }
            else {
                $scope.img = "";
            }
            $localStorage.$reset();
        }
        else {
            if (tempId > 0) {
                $location.path("/Main/CreatePromotion/" + tempId + "/Edit");
                //$location.path('/Main/CreatePromotion/0/Edit');
                //$localStorage.$reset();
            }
            else {
                $location.path('/Main/CreatePromotion/0/Edit');
                //$localStorage.$reset();
            }
        }
    }

    function ComputedDate() {

        var myString = $scope.expiryDate;
        myString = myString.replace(/-/g, '/');
        //var myString1 = new Date(Date.now());
        //myString1 = myString1.replace(/-/g, '/');
        var date1 = new Date(myString);
        var date2 = new Date();
        //alert(date2.getTime() + date1.getTime());
        //var timeDiff = Math.abs(date2.getTime() - date1.getTime());
        //var timeDiffH = Math.abs(date1.getTime() - date2.getTime());
        //var diffHrs = Math.floor((timeDiff % 86400000) / 3600000)
        //var diffDays = Math.ceil(timeDiff / (1000 * 3600 * 24));
        //alert(diffDays + "H" + timeDiffH);

        var diffMs = (date1 - date2); // milliseconds between now & Christmas
        var diff = (diffMs / 86400000)
        if (diff >= 1) {
            var diffDays = parseInt(diffMs / 86400000) + 1; // days
        }
        var diffHrs = Math.round((diffMs % 86400000) / 3600000); // hours
        var diffMins = Math.round(((diffMs % 86400000) % 3600000) / 60000); // minutes
        if (diffDays >= 1) {
            return parseInt(diffDays);
        }
        else {

            return (diffHrs + "h:" + diffMins + "m");
        }
    }
    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');

    }

    function plotMap() {

        var lati = $rootScope.vendorLat;
        var long = $rootScope.vendorLong;
        var myLatlng = new google.maps.LatLng(lati, long);

        var mapProp = {
            center: myLatlng,
            zoom: 12,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("confirmmap"), mapProp);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map

        });

        google.maps.event.addDomListener(window, 'load');
    }

    function LoadAllCategory() {
        //var url = window.location.href;
        //var thanku = "/Main/ThankYou";
        //var confirmTemp = "/Main/Confirm";
        //var tempUrl = "http://vendor.eddee.it" + "/#" + thanku;
        //var tempUrl1 = "http://vendor.eddee.it" + "/#" + confirmTemp;

        $scope.categorySelect = $cookieStore.get('categoryId');
        //  if (url != tempUrl || url != tempUrl1) {
        
        loadCategory();
        //}

        if ($localStorage.message != undefined) {
            $scope.promotion = $localStorage.message;
            if ($scope.promotion.imggggg != undefined) {
                $scope.result = $scope.promotion.imggggg;
            }
            $scope.createdDate = $scope.promotion.createdDate;
            if ($scope.promotion.expiryDate != undefined) {
                $scope.expiryDate = $scope.promotion.expiryDate;
            }
        }
        $scope.isbusy = 0;
        $scope.showLoader = false;
        //$scope.vendor_name = $scope.GetAllVendors[0];
        //if ($scope.promotionId > 0) {

        //    var id = _.findLastIndex(data.data, { vendorId: $scope.promotion.vendorId });
        //    $scope.vendor_name = $scope.GetAllVendors[id];
        //}
    }

    //manageActivePromotionService.GetAllVendors($scope, function (success, data) {
    //    if (success) {
    //        $scope.GetAllVendors = data.data;
    //        var url = window.location.href;
    //        var create = "/Main/CreatePromotion/0/Edit";
    //        var thanku = "/Main/ThankYou";
    //        var confirmTemp = "/Main/Confirm";
    //        var tempUrl = "http://vendor.eddee.it" + "/#" + thanku;
    //        //var tempUrl1 = "http://cravevendor.techvalens.net" + "/#" + create;
    //        //var tempUrl1 = "http://localhost:25215" + "/#" + create;
    //        if (url == tempUrl && url == tempUrl) {

    //        }
    //        else {
    //            loadCategory();
    //            loadSubCategory();
    //            loadOptionalCategory();
    //        }
    //            if ($localStorage.message != undefined) {
    //                $scope.promotion = $localStorage.message;
    //                if ($scope.promotion.imggggg != undefined) {
    //                    $scope.result = $scope.promotion.imggggg;
    //                }
    //                $scope.createdDate = $scope.promotion.createdDate;
    //                if ($scope.promotion.expiryDate != undefined) {
    //                    $scope.expiryDate = $scope.promotion.expiryDate;
    //                }
    //            }


    //        $scope.categorySelect = $cookieStore.get('categoryId');
    //        if ($scope.currentPromotionId > 0) {
    //            loadPromotion();
    //        }
    //        else {
    //            bindSubOpt();
    //        }
    //        $scope.isbusy = 0;
    //        $scope.showLoader = false;
    //        $scope.vendor_name = $scope.GetAllVendors[0];
    //        if ($scope.promotionId > 0) {

    //            var id = _.findLastIndex(data.data, { vendorId: $scope.promotion.vendorId });
    //            $scope.vendor_name = $scope.GetAllVendors[id];
    //        }
    //    }
    //    else
    //        console.log("failed", data);

    //});


    if ($scope.currentPromotionId == 0) {
        $rootScope.createUpdate = "Save";
    }
   
    function bindSubOpt() {
        //$scope.showLoader = true;
       
        $scope.categoryBussId = $scope.categorySelect;
        manageActivePromotionService.getAllCategoryByBussID($scope, $scope.categoryBussId, function (success, data) {
            if (success) {
                $scope.subCategory = data.data[0].SubCategories;
                $scope.optionalCategory = data.data[0].OptionalCategories;
                //$scope.showLoader = false;
                
            }
            else
                console.log("failed", data);
        });
    }

    $scope.CategoryClick = function ($http) {
        $scope.showLoader = true;
        $scope.categorySelect = $http;
        $scope.categoryBussId = $http;
        manageActivePromotionService.getAllCategoryByBussID($scope, $scope.categoryBussId, function (success, data) {
            if (success) {
                $scope.subCategory = data.data[0].SubCategories;
                $scope.optionalCategory = data.data[0].OptionalCategories;
                $scope.showLoader = false;
            }
            else
                console.log("failed", data);
        });
    }

    $scope.SubCategoryClick = function ($http) {
        $scope.subCategorySelect = $http;
    }

    $scope.OptCategoryClick = function ($http) {
        $scope.optionalCategorySelect = $http;
    }

    $scope.fileChanged = function (e) {
        $scope.clear();
        var files = e.target.files;

        var fileReader = new FileReader();
        fileReader.readAsDataURL(files[0]);

        fileReader.onload = function (e) {
            $scope.imgSrc = this.result;
            $scope.$apply();
        };

        $("#cropm").modal('show');
    }

    $scope.clear = function () {
        $scope.imageCropStep = 1;
        delete $scope.imgSrc;
        delete $scope.result;
        delete $scope.resultBlob;
        $("#cropm").modal('hide');
    };

    function loadPromotion() {
       
        $scope.showLoader = true;
        if ($scope.currentStatus == "Relaunch".trim()) {
            $rootScope.createUpdate = "Update";
        }
        else {

            $rootScope.createUpdate = "Update";
        }
        $scope.show = false;
         
        manageActivePromotionService.getPromotionById($scope, $scope.currentPromotionId, function (success, data) {

            if (success) {
                $scope.promotion = data.data1;
                $scope.showLoader = false;
                $cookieStore.put('promoId', $scope.promotion.promotionCodeId);
                $scope.categorySelect = $scope.promotion.categoryId;
                //$cookieStore.get('categoryId')
               
                $scope.subCategorySelect = $scope.promotion.subCategoryId;
                $scope.optionalCategorySelect = $scope.promotion.optCategoryId;
                bindSubOpt();
                $scope.image = $scope.promotion.promotionImage;
                $scope.randomCode = $scope.promotion.code;
                if ($scope.promotion.quantity == 0) {
                    $scope.promotion.quantity = null;
                }
                if ($scope.currentStatus == "Relaunch".trim()) {
                    $scope.expiryDate = null;
                    $scope.createdDate = $filter('date')(new Date(), "MM-dd-yyyy hh:mm:ss a");
                }
                else {

                    $scope.expiryDate = $scope.promotion.expiryDate;
                    $scope.createdDate = ($scope.promotion.createdDate);
                }
            }
            else
                console.log("failed", data);
        });
    }

    function CreateUpdateFormData() {

        /* calculating date diffrence start*/
        var myEnd = $('#expiryDate').val();
        myEnd = myEnd.replace(/-/g, '/');
        var myStart = $('#createdDate').val();
        myStart = myStart.replace(/-/g, '/');

        var start = new Date(myStart);
        var end = new Date(myEnd);
        var diff = new Date(end - start);
        var days = diff / 1000 / 60 / 60 / 24;

        if ($scope.promotion.descriptionEnglish == undefined) {
            $scope.promotion.descriptionEnglish = '';
        }
        if ($scope.promotion.descriptionThai == undefined) {
            $scope.promotion.descriptionThai = '';
        }
        /* calculating date diffrence end*/
        if (days > 7) {
            // $scope.Message = "Start Date and End Date diffrence should be less than 7 Days";
            $scope.Message = $rootScope.translation.startDateandEndDatediffrenceshouldbelessthan7Days;
            $('#static').modal('show');
        }

        var current = new Date();
        var curDiff = new Date(current - end);
        if (curDiff > 0) {

            //$scope.Message = "End Date should be less than current Date";
            $scope.Message = $rootScope.translation.EndDateshouldbelessthancurrentDate;
            $('#static').modal('show');
        }

        else {
            if ($scope.promotion.descriptionEnglish == '' && $scope.promotion.descriptionThai == '') {

                $scope.Message = $rootScope.translation.PleaseFilltheDescription;
                $('#static').modal('show');
            }
                //else if ($scope.promotion.descriptionEnglish != '' && $scope.promotion.descriptionThai != '') {
                //    $scope.Message = "Please Fill any one of the Description!";
                //    $('#static').modal('show');
                //}
            else {
                if (($('#expiryDate').val() != '' && ($scope.promotion.quantity != undefined))) {
                    //$scope.Message = "Can't Selected Both End Quantity And Expiry Date";
                    $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                    $('#static').modal('show');
                    //alert("Can't Selected Both End Quantity And Expiry Date");
                }
                else {

                    if ($('#expiryDate').val() == '' && ($scope.promotion.quantity == undefined)) {
                        // $scope.Message = "Can't Selected Both End Quantity And Expiry Date";
                        $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                        $('#static').modal('show');
                        //alert("Can't Selected Both End Quantity And Expiry Date");
                    }
                    else {
                        var allSrcs = Array();
                        $("#promotionImages img").each(function () {
                            allSrcs.push($(this).attr('src'));
                            $scope.imgBase64 = $(this).attr('src');
                            $scope.promoimg = $(this).attr('src');
                            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
                        });
                        if ($scope.imgBase64.length > 150) {
                            $scope.img = $scope.imgBase64;
                        }
                        else {
                            $scope.img = "";
                        }
                        if ($scope.currentPromotionId == 0) {
                            $scope.randomCode = randomPromotionCode();
                            $cookieStore.remove('promoId');
                        }
                        var createdDate = $('#createdDate').val();
                        $scope.createdDate = createdDate;
                        var expiryDate = $('#expiryDate').val();
                        $scope.expiryDate = formatDate1(expiryDate);

                        if ($scope.optionalCategorySelect == null || $scope.optionalCategorySelect == 'undefined') { $scope.optionalCategorySelect = "0"; }
                        if ($scope.subCategorySelect == null || $scope.subCategorySelect == 'undefined') { $scope.subCategorySelect = "0"; }
                        var dataLocal = {
                            promotionCodeId: $scope.promotion.promotionCodeId, code: $scope.randomCode, name: $scope.promotion.name, descriptionEnglish: $scope.promotion.descriptionEnglish, descriptionThai: $scope.promotion.descriptionThai,
                            createdDate: $scope.createdDate, expiryDate: $scope.expiryDate, quantity: $scope.promotion.quantity, categoryId: $scope.categorySelect, vendorId: $scope.vendor_Id,
                            subCategoryId: $scope.subCategorySelect, optCategoryId: $scope.optionalCategorySelect, price: $scope.promotion.price, promotionImage: $scope.img, imggggg: $scope.promoimg, tempSub: $scope.tempSubCategorySelect, tempOpt: $scope.tempOptionalCategorySelect, id: $scope.currentPromotionId
                        };
                        $cookieStore.put('ConfirmLocal', dataLocal);
                        $localStorage.message = $cookieStore.get('ConfirmLocal');;
                        $location.path('/Main/Confirm');
                        $rootScope.confirm = $localStorage.message;
                    }
                }
            }
        }
    }

    $scope.CreatNew = function () {

        if ($('#createdDate').val() != '' && $('#expiryDate').val() != '') {
            var myEnd = $('#expiryDate').val();
            myEnd = myEnd.replace(/-/g, '/');
            var myStart = $('#createdDate').val();
            myStart = myStart.replace(/-/g, '/');

            var start = new Date(myStart);
            var end = new Date(myEnd);
            var diff = new Date(end - start);
            var days = diff / 1000 / 60 / 60 / 24;
            //alert(days);
            var currenttimediff = new Date(end - new Date());
            var time = currenttimediff / 1000 / 60 / 60 / 24;
            if (time < 0) {
                $scope.Message = $rootScope.translation.Pleaseverifyenddate;
                $('#static').modal('show');
            }
            else {
                if (days > 7) {
                    $scope.Message = $rootScope.translation.startDateandEndDatediffrenceshouldbelessthan7Days;
                    $('#static').modal('show');
                    //alert();
                }
                else {
                    CreateUpdateFormData();
                }
            }
        }
        else {
            CreateUpdateFormData();
        }
    }

    $scope.Confirm = function () {

        var expiryDate = $('#expiryDate').val();
        if ($rootScope.createUpdate == "Save") {

            if (($scope.expiryDate != '' && ($scope.promotion.quantity != undefined))) {
                $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                $('#static').modal('show');
            }
            else {

                if ($scope.expiryDate == '' && ($scope.promotion.quantity == undefined)) {
                    $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                    $('#static').modal('show');

                }
                else {
                    CreateNewPromotion();
                }
            }
        }
        else {

            if (($scope.expiryDate != '' && ($scope.promotion.quantity != null))) {

                $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                $('#static').modal('show');
            }
            else {
                if ($scope.expiryDate == '' && ($scope.promotion.quantity == null)) {
                    $scope.Message = $rootScope.translation.CantSelectedBothEndQuantityAndExpiryDate;
                    $('#static').modal('show');

                } else {
                    $scope.update();
                }
            }
        }
    }

    function CreateNewPromotion() {

        $scope.Message = "promotion successfully created!";
        $scope.showLoader = true;
        var allSrcs = Array();
        $("#promotionImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
        });
        if ($scope.imgBase64.length > 150) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        var createdDate = $('#createdDate').val();
        $scope.categorySelect = $cookieStore.get('confirmCatId');
        $scope.optionalCategorySelect = $cookieStore.get('confirmOptId');
        $scope.subCategorySelect = $cookieStore.get('confirmSubId');
        if ($cookieStore.get('confirmOptId') == 0) {
            $scope.optionalCategorySelect = 0;
        }
        else {
            $scope.optionalCategorySelect = $cookieStore.get('confirmOptId');
        }
        if ($cookieStore.get('confirmSubId') == 0) {
            $scope.subCategorySelect = 0;
        }
        else {
            $scope.subCategorySelect = $cookieStore.get('confirmSubId');
        }
        var data = {
            code: $scope.randomCode, name: $scope.promotion.name, descriptionEnglish: $scope.promotion.descriptionEnglish, descriptionThai: $scope.promotion.descriptionThai,
            createdDate: $scope.createdDate, expiryDate: $scope.expiryDate, quantity: $scope.promotion.quantity, categoryId: $scope.categorySelect, vendorId: $scope.vendor_Id,
            subCategoryId: $scope.subCategorySelect, optCategoryId: $scope.optionalCategorySelect, price: $scope.promotion.price, promotionImage: $scope.img
        };
        manageActivePromotionService.savePromotion($scope, data, function (success, data) {
            if (success) {
                $scope.showLoader = false;
                $location.path('/Main/ThankYou');
                $localStorage.message = null;


            }
            else {
                console.log("failed", data);
            }
        });
    }

    $scope.update = function () {
        if ($scope.currentStatus == "Relaunch".trim()) {
            $scope.promotion.name = "Relaunch";
        }
        else {
            $scope.promotion.name = "Edit";
        }
        $scope.showLoader = true;
        $scope.Message = "promotion successfully updated!";
        var allSrcs = Array();
        $("#promotionImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
        });
        if ($scope.imgBase64.length > 150) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        //var createdDate = $('#createdDate').val();
        //$scope.createdDate = convertTo24Hour(createdDate);
        $scope.categorySelect = $cookieStore.get('confirmCatId');
        if ($cookieStore.get('confirmOptId') == 0) {
            $scope.optionalCategorySelect = 0;
        }
        else {
            $scope.optionalCategorySelect = $cookieStore.get('confirmOptId');
        }
        if ($cookieStore.get('confirmSubId') == 0) {
            $scope.subCategorySelect = 0;
        }
        else {
            $scope.subCategorySelect = $cookieStore.get('confirmSubId');
        }
       
        var data = {
            promotionCodeId: $scope.promotion.promotionCodeId, code: $scope.randomCode, name: $scope.promotion.name, descriptionEnglish: $scope.promotion.descriptionEnglish, descriptionThai: $scope.promotion.descriptionThai,
            createdDate: $scope.createdDate, expiryDate: $scope.expiryDate, quantity: $scope.promotion.quantity, categoryId: $scope.categorySelect, vendorId: $scope.vendor_Id,
            subCategoryId: $scope.subCategorySelect, optCategoryId: $scope.optionalCategorySelect, price: $scope.promotion.price, promotionImage: $scope.img, isActive: true
        };
        manageActivePromotionService.updatePromotion($scope, $rootScope, data, function (success, data) {

            $location.path('/Main/ThankYou');
            $cookieStore.remove("promoId");
            $localStorage.message = null;

        });
    }

    function loadCategory() {
        manageActivePromotionService.getAllCategory($scope, function (success, data) {
            if (success) {
                $scope.category = data.data;
               // bindSubOpt()
                //loadSubCategory();
            }
            else
                console.log("failed", data);
        });
    }

    function loadSubCategory() {
        manageActivePromotionService.getAllSubCategory($scope, function (success, data) {
            if (success) {
                $scope.subCategory = data.data;
                //loadOptionalCategory();
            }
            else
                console.log("failed", data);
        });
    }

    function loadOptionalCategory() {
        manageActivePromotionService.getAllOptionalCategory($scope, function (success, data) {
            if (success) {
                $scope.optionalCategory = data.data;
                //bindSubOpt();
            }
            else
                console.log("failed", data);
        });
    }
    if ($scope.currentPromotionId > 0) {
        loadPromotion();
    }
    else {
        bindSubOpt();
    }

    //$scope.changeCategory = function ($http) {
    //    $scope.showLoader = 1;
    //    $scope.categoryBussId = JSON.stringify($http.categoryId);
    //    manageActivePromotionService.getAllCategoryByBussID($scope, $scope.categoryBussId, function (success, data) {
    //        if (success) {
    //            $scope.subCategory = data.data[0].SubCategories;
    //            $scope.subCategory = $scope.subCategory[0];
    //            $scope.optionalCategory = data.data[0].OptionalCategories;
    //            $scope.optionalCategory = $scope.optionalCategory[0];
    //            $scope.showLoader = 0;
    //        }
    //        else
    //            console.log("failed", data);
    //    });

    //}

    function formatDate(value) {
        if (value) {
            Number.prototype.padLeft = function (base, chr) {
                var len = (String(base || 10).length - String(this).length) + 1;
                return len > 0 ? new Array(len).join(chr || '0') + this : this;
            }
            var d = new Date(value),
            dformat = [(d.getMonth() + 1).padLeft(),
                         d.getDate().padLeft(),
                         d.getFullYear()].join('/') +
                      ' ' +
                      [d.getHours().padLeft(),
                        d.getMinutes().padLeft(),
                        d.getSeconds().padLeft()].join(':');
            return dformat;
        }
    }

    $(".datepicker").datetimepicker({
        format: "mm-dd-yyyy HH:ii:ss P",
        pick12HourFormat: false,
        showMeridian: true,
        autoclose: true,
        todayBtn: true
    });

    $('.clear').click(function () {

        $('.startDate').val('');
    });

    $('.clear1').click(function () {

        //  $scope.expiryDate = null;

        $('.endDate').val('');
    });

    $('.clearSub').click(function () {
        $scope.subCategorySelect = undefined;
        $('.subCategoryClear').val('');
    });

    $('.clearOpt').click(function () {
        $scope.optionalCategorySelect = undefined;
        $('.optCategoryClear').val('');
    });

    $scope.back = function () {

        HandleBackFunctionality();
        $scope.promotion.descriptionEnglish = $rootScope.confirm.descriptionEnglish;
        $scope.promotion.descriptionThai = $rootScope.confirm.descriptionThai;
        $scope.createdDate = $rootScope.confirm.createdDate;
        $scope.expiryDate = $rootScope.confirm.expiryDate;
        $scope.promotion.quantity = $rootScope.confirm.quantity;
        $scope.categorySelect = $rootScope.confirm.categorySelect;

        $scope.subCategorySelect = $rootScope.confirm.subCategorySelect;
        $scope.optionalCategorySelect = $rootScope.confirm.optionalCategorySelect;
        $scope.promotion.price = $rootScope.confirm.price;
        $scope.promoimg = $rootScope.confirm.promoimg;

    }

    /*Handling back funtionality of browser start*/

    function HandleBackFunctionality() {

        window.history.back();


    }
    /*Handling back funtionality of browser end*/

    function formatDate1(date) {
        var d = new Date(date);
        var hh = d.getHours();
        var m = d.getMinutes();
        var s = d.getSeconds();
        var dd = "AM";
        var h = hh;
        if (h >= 12) {
            h = hh - 12;
            dd = "PM";
        }
        if (h == 0) {
            h = 12;
        }
        m = m < 10 ? "0" + m : m;

        s = s < 10 ? "0" + s : s;

        /* if you want 2 digit hours: */
        h = h < 10 ? "0" + h : h;

        var pattern = new RegExp("0?" + hh + ":" + m + ":" + s);
        return date.replace(pattern, h + ":" + m + ":" + s + " " + dd)
    }

    function convertTo24Hour(time) {
        var hours = parseInt(time.substr(0, 2));
        if (time.indexOf('am') != -1 && hours == 12) {
            time = time.replace('12', '0');
        }
        if (time.indexOf('pm') != -1 && hours < 12) {
            time = time.replace(hours, (hours + 12));
        }
        return time.replace(/(am|pm)/, '');
    }

    /*-- Genrate random string start--*/
    function randomPromotionCode() {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        var pass = "";
        for (var x = 0; x < 9; x++) {
            var i = Math.floor(Math.random() * chars.length);
            pass += chars.charAt(i);
        }
        return pass;
    }
    /*---Genrate random string end--*/
}])

