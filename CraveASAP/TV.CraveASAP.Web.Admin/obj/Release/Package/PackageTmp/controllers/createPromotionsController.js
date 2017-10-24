'use strict';
app.controller('createPromotionsController', ['$scope', '$stateParams', '$rootScope', '$location', 'manageActivePromotionSevice', '$cookies', '$cookieStore', 'initData', function ($scope, $stateParams, $rootScope, $location, manageActivePromotionSevice, $cookies, $cookieStore, initData) {
    $rootScope.isLogged = 1;
    $scope.promotionId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.createUpdate = "Save";
    $scope.path = $scope.config.api + "/Pictures/";
    $scope.ddlandbtn = false;
    $scope.showLoader = true;
    $scope.promotiontitle = "Create Promotion";
    $scope.showTable = false;
    $scope.randomCode = randomPromotionCode();
    loadPromotion();
    $scope.promotionImage = "Default.png";
    $scope.categorySelect = $rootScope.categoryId;
    $scope.promotion = {};

    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.CategoryClick = function ($http) {
        $scope.showLoader = true;
        $scope.categoryBussId = JSON.stringify($http.categoryId);
        $scope.categorySelect = $scope.category[$scope.categoryBussId - 1];

        manageActivePromotionSevice.getAllCategoryByBussID($scope, $scope.categoryBussId, function (success, data) {
            if (success) {
                $scope.subCategory = data.data[0].SubCategories;
                $scope.subCategorySelect = $scope.subCategory[0];
                $scope.optionalCategory = data.data[0].OptionalCategories;
                $scope.optionalCategorySelect = $scope.optionalCategory[0];
                $scope.showLoader = false;
            }

            else
                console.log("failed", data);
        });

    }


    $scope.CreatNew = function () {
        var expiryDate = $('#expiryDate').val();
        $scope.expiryDate = formatDate1(expiryDate);
        var createdDate = $('#createdDate').val();

        if ($scope.promotion.descriptionEnglish == undefined) {
            $scope.promotion.descriptionEnglish = '';
        }
        if ($scope.promotion.descriptionThai == undefined) {
            $scope.promotion.descriptionThai = '';
        }
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
            $scope.Message = "Start Date and End Date diffrence should be less than 7 Days";
            $('#static').modal('show');
        }
        var current = new Date();
        var curDiff = new Date(current - end);
        if (curDiff > 0) {

            $scope.Message = "End Date should be less than  current Date";
            $('#static').modal('show');
        }
        else {
            if ($scope.createUpdate == "Save") {

                if ($('#createdDate').val() != '') {
                    if ((expiryDate != '' && ($scope.promotion.quantity != null))) {

                        $scope.Message = "Can't Selected Both End Quantity And Expiry Date";
                        $('#static').modal('show');
                    }
                    else {
                        if (expiryDate == '' && ($scope.promotion.quantity == null)) {

                            $scope.Message = "Please Selected either Quantity Or Expiry Date";
                            $('#static').modal('show');

                        } else {
                            if ($scope.promotion.descriptionEnglish == ''  && $scope.promotion.descriptionThai == '' ) {

                                $scope.Message = "Please Fill the Description!";
                                $('#static').modal('show');
                            }
                            else {
                                
                                    CreateNewPromotion();
                            }
                        }
                    }
                }
                else {
                    $scope.Message = "Please select created Date";
                    $('#static').modal('show');
                }
            }

            else {
                if ($('#createdDate').val() != '') {
                    if ((expiryDate != '' && ($scope.promotion.quantity != null))) {

                        $scope.Message = "Can't Selected Both End Quantity And Expiry Date";
                        $('#static').modal('show');
                    }
                    else {
                        if (expiryDate == '' && ($scope.promotion.quantity == null)) {
                            $scope.Message = "Please Selected either Quantity Or Expiry Date";
                            $('#static').modal('show');

                        } else {
                            if ($scope.promotion.descriptionEnglish == ''  && $scope.promotion.descriptionThai == '' ) {

                                $scope.Message = "Please Fill the Description!";
                                $('#static').modal('show');
                            }
                            else {
                                    $scope.Update();
                            }

                        }
                    }
                }
                else {
                    $scope.Message = "Please select created Date";
                    $('#static').modal('show');
                }
            }


        }
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

    function CreateNewPromotion() {
        $scope.Message = "Promotion has been Successfully Created!";
        $scope.showLoader = true;
        var allSrcs = Array();
        $("#output img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
        });
        if ($scope.imgBase64 != null) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        var createdDate = $('#createdDate').val();
        $scope.createdDate = formatDate1(createdDate);
        //var expiryDate = $('#expiryDate').val();
        //$scope.expiryDate = formatDate1(expiryDate);

        if ($scope.optionalCategorySelect == null || $scope.optionalCategorySelect == 'undefined') { $scope.optionalCategorySelect = "0"; }
        if ($scope.subCategorySelect == null || $scope.subCategorySelect == 'undefined') { $scope.subCategorySelect = "0"; }
        var data = {
            code: $scope.randomCode, name: $scope.promotion.name, descriptionEnglish: $scope.promotion.descriptionEnglish, descriptionThai: $scope.promotion.descriptionThai,
            createdDate: $scope.createdDate, expiryDate: $scope.expiryDate, quantity: $scope.promotion.quantity, categoryId: $scope.categorySelect.categoryId, vendorId: $scope.vendor_name.vendorId,
            subCategoryId: $scope.subCategorySelect.subCategoryId, optCategoryId: $scope.optionalCategorySelect.optCategoryId, price: $scope.promotion.price, promotionImage: $scope.img
        };

        manageActivePromotionSevice.savePromotion($scope, data, function (success, data) {
            if (success) {
                $("#static").modal('show');
                $scope.redirect = function () {
                    $location.path('/Main/ManageActivePromotion');
                }

                $scope.showLoader = false;
                $("#static").modal('show');
            }
            else {
                console.log("failed", data);
            }
        });
    }

    function GetSubCategory() {
        manageActivePromotionSevice.getAllCategoryByBussID($scope, $scope.categoryBussId, function (success, data) {
            if (success) {
                $scope.subCategory = data.data;
                $scope.subCategorySelect = $scope.subCategory[0];
            }
            else
                console.log("failed", data);
        });
    }

    function loadPromotion() {
        if ($scope.promotionId == 0) {
            getAllVendor();
        }
        else  {
            $scope.createUpdate = "Update";
            manageActivePromotionSevice.getPromotionById($scope, $scope.promotionId, function (success, data) {
                if (success) {

                    $scope.promotion = data.data1;
                    $scope.vendor_name = $scope.promotion.vendorId;
                    getAllVendor();
                    $scope.promotiontitle = "Edit Promotion";
                    $scope.ddlandbtn = true;
                    $scope.categorySelect = $scope.promotion.categoryId;
                    $scope.subCategorySelect = $scope.promotion.subCategoryId;
                    $scope.optionalCategorySelect = $scope.promotion.optCategoryId;
                    $scope.expiryDate = formatDate($scope.promotion.expiryDate);
                    $scope.createdDate = formatDate($scope.promotion.createdDate);
                    alert($scope.promotion.expiryDate);
                    alert($scope.createdDate);
                    $scope.promotionImage = $scope.promotion.promotionImage;
                    $scope.randomCode = $scope.promotion.code;
                    //if ($scope.promotion.quantity == 0) {
                    //    $scope.promotion.quantity = 0;
                    //}
                }
               
            });
        }
    }

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

    $scope.Update = function () {
        $scope.Message = "Promotion has been Successfully Updated!";
        $scope.showLoader = true;

        var allSrcs = Array();
        $("#output img").each(function () {
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
        $scope.createdDate = formatDate1(createdDate);
    


        if ($scope.optionalCategorySelect == null || $scope.optionalCategorySelect == 'undefined') { $scope.optionalCategorySelect = "0"; }
        if ($scope.subCategorySelect == null || $scope.subCategorySelect == 'undefined') { $scope.subCategorySelect = "0"; }
        $scope.promotion.name = "Edit";
        var data = {
            promotionCodeId: $scope.promotionId, code: $scope.randomCode, name: $scope.promotion.name, descriptionEnglish: $scope.promotion.descriptionEnglish, descriptionThai: $scope.promotion.descriptionThai,
            createdDate: $scope.createdDate, expiryDate: $scope.expiryDate, quantity: $scope.promotion.quantity, categoryId: $scope.categorySelect.categoryId, vendorId: $scope.vendor_name.vendorId,
            subCategoryId: $scope.subCategorySelect.subCategoryId, optCategoryId: $scope.optionalCategorySelect.optCategoryId, price: $scope.promotion.price, promotionImage: $scope.img, isActive: true
        };
        manageActivePromotionSevice.updatePromotion($scope, data);
        $("#static").modal('show');
        $scope.redirect = function () {
            $location.path("/Main/ManageActivePromotion");
        }
        $scope.showLoader = false;
    }

    function loadCategory() {

        manageActivePromotionSevice.getAllCategory($scope, function (success, data) {
            if (success) {
                $scope.category = data.data;
                $scope.categorySelect = $scope.category[0];
                if ($scope.promotionId > 0) {
                    var id1 = _.findLastIndex($scope.category, { categoryId: $scope.promotion.categoryId });
                    $scope.categorySelect = $scope.category[id1];
                   
                }

            }
            else
                console.log("failed", data);
        });

    }

    function loadSubCategory() {

        manageActivePromotionSevice.getAllSubCategory($scope, function (success, data) {
            if (success) {
                $scope.subCategory = data.data;
                $scope.subCategorySelect = $scope.subCategory[0];
                if ($scope.promotionId > 0) {
                    var id1 = _.findLastIndex($scope.subCategory, { subCategoryId: $scope.promotion.subCategoryId });
                    $scope.subCategorySelect = $scope.subCategory[id1];
                }
            }
            else
                console.log("failed", data);
        });

    }

    function loadOptionalCategory() {

        manageActivePromotionSevice.getAllOptionalCategory($scope, function (success, data) {
            if (success) {
                $scope.optionalCategory = data.data;
                $scope.optionalCategorySelect = $scope.optionalCategory[0];
                if ($scope.promotionId > 0) {
                    var id1 = _.findLastIndex($scope.optionalCategory, { optCategoryId: $scope.promotion.optCategoryId });
                    $scope.optionalCategorySelect = $scope.optionalCategory[id1];
                }

            }
            else
                console.log("failed", data);
        });
    }
    function getAllVendor() {
        manageActivePromotionSevice.GetAllVendors($scope, function (success, data) {
            if (success) {
                $scope.GetAllVendors = data.data;
                loadCategory();
                loadSubCategory();
                loadOptionalCategory();
                $scope.isbusy = 0;
                $scope.showLoader = false;
                $scope.vendor_name = $scope.GetAllVendors[0];

                if ($scope.promotionId > 0) {

                    var id = _.findLastIndex(data.data, { vendorId: $scope.promotion.vendorId });
                    $scope.vendor_name = $scope.GetAllVendors[id];
                }
            }
            else
                console.log("failed", data);
        });

    }

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
        format: "mm-dd-yyyy hh:ii:ss",
        pick12HourFormat: false,
        showMeridian: true,
        autoclose: true,
        todayBtn: true,
    });

    //var someDate = new Date();
    //var numberOfDaysToAdd = 7;
    //someDate.setDate(someDate.getDate() + numberOfDaysToAdd);
    //var dd = someDate.getDate();
    //var mm = someDate.getMonth() + 1;
    //var y = someDate.getFullYear();

    //var someFormattedDate = mm + '-' + dd + '-' + y + ' ' + '00' + ':' + '00' + ':' + '00' + ' ' + '00';

    //$(".datepicker1").datetimepicker({
    //    format: "mm-dd-yyyy HH:ii:ss P",
    //    pick12HourFormat: false,
    //    showMeridian: true,
    //    autoclose: true,
    //    todayBtn: true,
    //    endDate: someFormattedDate
    //});

    $('.clear').click(function () {

        $('.startDate').val('');
        $scope.createdDate = null;
    });
    $('.clear1').click(function () {

        $('.endDate').val('');
        $scope.expiryDate = null;
    });

    $('.clearSub').click(function () {
        $scope.subCategorySelect = 0;
        $('.subCategoryClear').val('');
    });
    $('.clearOpt').click(function () {
        $scope.optionalCategorySelect = 0;
        $('.optCategoryClear').val('');
    });

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



