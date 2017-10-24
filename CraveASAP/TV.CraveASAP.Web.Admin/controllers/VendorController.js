'use strict';

app.controller('vendorController', ['$scope', '$location', 'vendorService', '$stateParams', '$cookies', '$cookieStore', '$rootScope', '$parse', 'initData', 'vendorData', function ($scope, $location, vendorService, $stateParams, $cookies, $cookieStore, $rootScope, $parse, initData, vendorData) {
    vendorData.vendorFunction();
    $scope.checked = false;
    $scope.showTable = "Save";
    $scope.logoImg = "Default.png";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    MapLoc();


   /*---------------------------------------------------------Grab My Location Start-------------------------------------------------------------------------*/

    $scope.currentLocation = function () {

        navigator.geolocation.getCurrentPosition(locationHandler);

        function locationHandler(position) {
            var currentLat = position.coords.latitude;
            var currentLng = position.coords.longitude;
            GetAddress(currentLat, currentLng);
        }
    }

     
    function GetAddress(currentLat, currentLng) {

        var lat = parseFloat(currentLat);
        var lng = parseFloat(currentLng);

            var latlng = new google.maps.LatLng(lat, lng);
            var geocoder = geocoder = new google.maps.Geocoder();
            geocoder.geocode({ 'latLng': latlng }, function (results, status) {

                if (status == google.maps.GeocoderStatus.OK) {
                    if (results[1]) {

                        $('.location').val(results[1].formatted_address);
                        var lati = lat;
                        var lonng = lng;
                        $('.lat').val(lati);
                        $('.long').val(lonng);
                        plotmap(lati, lonng);
                    }

                }

            });

    }

    /*---------------------------------------------------------Grab My Location End-------------------------------------------------------------------------*/

    function MapLoc() {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({
            address: $('input[name=address]').val(),
            region: 'no'
        },


            function (results, status) {

                if (status.toLowerCase() == 'ok') {
                    var coords = new google.maps.LatLng(
                        results[0]['geometry']['location'].lat(),
                        results[0]['geometry']['location'].lng()
                    );

                    var lati = coords.lat();
                    var lonng = coords.lng();
                    $('iframe[name=map_iframe]').contents().find('#lat').val(lati);
                    $('iframe[name=map_iframe]').contents().find('#long').val(lonng);

                    $('#lat').val(coords.lat());
                    $('#long').val(coords.lng());

                    var myLatlng = new google.maps.LatLng(lati, lonng);
                    var mapOptions = {
                        zoom: 5,
                        center: myLatlng
                    }
                    var map = new google.maps.Map(document.getElementById('gmap'), mapOptions);

                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map

                    });

                }
            }
        );

    }

    $scope.currentVendorId = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.data = {};


    $scope.vendortitle = "Create Vendor";
    $scope.editMode = 0;

    loadCategory();
    vendorService.GetVendors($scope, function (success, data) {
        if (success) {
            $scope.vendorName = data.data;
        }
        else {
        }
    });
    $scope.save = function () {
        if ($scope.vendor.businessCategory != undefined) {
            if ($scope.showTable == "Save") {

                $scope.saveVendor();
            }
            else {

                $scope.update();
            }
        }
        else {
            $scope.Message = "Please select business category!";
            $("#static").modal('show');
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

    $scope.saveVendor = function () {
        $scope.Message = "Vendor has been Successfully Created!";
        $scope.showLoader = true;
        var allSrcs = Array();
        $("#vendorImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");

        });
        openingHourAddUpdate();

        $scope.vendor.streetName = $('#location').val();
        $scope.longitude = $('#long').val();
        $scope.latitude = $('#lat').val();
        if ($scope.imgBase64.length > 150) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        var data = {
            loginVendorName: $scope.vendor.loginVendorName, companyName: $scope.vendor.companyName, businessCategory: $scope.vendor.businessCategory, shortDescription: $scope.vendor.shortDescription, fullDescription: $scope.vendor.fullDescription,
            email: $scope.vendor.email, phoneNo: $scope.vendor.phoneNo, contactPerson: $scope.vendor.contactPerson, contactPhoneNo: $scope.vendor.contactPhoneNo,
            contactEmail: $scope.vendor.contactEmail, streetName: $scope.vendor.streetName, postCode: $scope.vendor.postCode, buildingName: $scope.vendor.buildingName,
            floor: $scope.vendor.floor, area: $scope.vendor.area, city: $scope.vendor.city, longitude: $scope.longitude, latitude: $scope.latitude, password: $scope.vendor.password
            , logoImg: $scope.img
            , fromday: $scope.vendor.fromday, today: $scope.vendor.today, fromtime: $scope.vendor.fromtime, totime: $scope.vendor.totime


        };
        if ($scope.vendor.fromday != null) {
        vendorService.saveVendors($scope, data, function (success, data) {
            if (data.responseCode == 1) {
                $scope.redirect = function () {
                    $location.path("/Main/VendorList");
                }

                $("#static").modal('show');
                $scope.showLoader = false;

            }
            else {
                $scope.Message = "Email or loginVendorId already exist!!";
                $("#static").modal('show');
                $scope.showLoader = false;
            }
        });
        }
        else {
            $scope.Message = "Please select opening hours!";
            $("#static").modal('show');
            $scope.showLoader = false;
        }
    }
    
    $scope.update = function () {
        $scope.Message = "Vendor has been Successfully Updated!";
        $scope.showLoader = true;
        var allSrcs = Array();
        $("#vendorImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");

        });
        openingHourAddUpdate();
        $scope.vendor.streetName = $('#location').val();
        $scope.longitude = $('#long').val();
        $scope.latitude = $('#lat').val();

        if ($scope.imgBase64.length > 150) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        $scope.vendor.city = $('#city').val();
        var data = {
            vendorId: $scope.vendor.vendorId, loginVendorName: $scope.vendor.loginVendorName, companyName: $scope.vendor.companyName, businessCategory: $scope.vendor.businessCategory, shortDescription: $scope.vendor.shortDescription, fullDescription: $scope.vendor.fullDescription,
            email: $scope.vendor.email, phoneNo: $scope.vendor.phoneNo, contactPerson: $scope.vendor.contactPerson, contactPhoneNo: $scope.vendor.contactPhoneNo,
            contactEmail: $scope.vendor.contactEmail, streetName: $scope.vendor.streetName, postCode: $scope.vendor.postCode, buildingName: $scope.vendor.buildingName,
            floor: $scope.vendor.floor, area: $scope.vendor.area, city: $scope.vendor.city, longitude: $scope.longitude, latitude: $scope.latitude, password: $scope.vendor.password
            , logoImg: $scope.img
            , fromday: $scope.vendor.fromday, today: $scope.vendor.today, fromtime: $scope.vendor.fromtime, totime: $scope.vendor.totime

        };
        if ($scope.vendor.fromday != null) {
            vendorService.updateVendors($scope, data, function (success, data) {

                if (data.statusCode == 1) {
                    $scope.redirect = function () {
                        $location.path("/Main/VendorList");
                    }

                    $scope.Message = "Vendor has been Successfully Updated!";
                    loadCategory();
                    $("#static").modal('show');
                    $scope.showLoader = false;

                }
                else {
                    $scope.Message = "Email or loginVendorId already exist!!";
                    $("#static").modal('show');
                    $scope.showLoader = false;
                }
            });
        }
        else {
            $scope.Message = "Please select opening hours!";
            $("#static").modal('show');
            $scope.showLoader = false;
        }
    }

    $scope.saveBranch = function () {
        $scope.showLoader = true;
        if ($scope.headOffice == "HeadOffice".trim()) {
            $scope.radio = "Head Office";
        }
        else {
            $scope.radio = "Branch";
        }
        var data = {
            businessName: $scope.businessName, taxId: $scope.taxId, location: $scope.radio, fullAddress: $scope.fullAddress, deliveryAddress: $scope.deliveryAddress,
            phoneNo: $scope.phoneNo, contactPerson: $scope.contactPerson, additionalInfo: $scope.additionalInfo, defaultBranch: $scope.defaultBranch, email: $scope.email,
            vendorId: $scope.vendorId, branchCode: $scope.branchCode
        };
        vendorService.saveVendorBranch($scope, data, function (success, data) {
            $scope.Message = "Vendor branch successfully created";
            $('#static').modal('show');
            $scope.showLoader = false;
            $scope.redirect = function () {
                $location.path("/Main/VendorList");
            }
        });
    }

    function loadVendorPage() {
        if ($scope.currentVendorId == 0) {
            var newVendor = angular.copy(vendorService.defaultvendor);
            newVendor['vendor_branch'] = [];
            $scope.vendor = newVendor;


        }
        else {
            $scope.vendortitle = 'Edit Vendor';
            $scope.showTable = !$scope.showTable;
            $scope.showLoader = true;
            vendorService.getVendorById($scope, $scope.currentVendorId, function (success, data) {
                if (success) {

                    $scope.vendor = data.data1;
                    var lati = $scope.vendor.latitude;
                    var lonng = $scope.vendor.longitude;
                    plotmap(lati, lonng);
                    openingHourGet();
                    $scope.latitude = $scope.vendor.latitude;
                    $scope.longitude = $scope.vendor.longitude;
                    $scope.vendor.phoneNo = parseInt($scope.vendor.phoneNo);
                    $scope.vendor.contactPhoneNo = parseInt($scope.vendor.contactPhoneNo);
                    $scope.vendor.postCode = parseInt($scope.vendor.postCode);
                    $scope.logoImg = $scope.vendor.logoImg;
                   
                    
                    $scope.showLoader = false;
                  
                    $scope.showTable = "Update";
                    $scope.editMode = 1;
                    $('iframe[name=map_iframe]').contents().find('#lat').val($scope.vendor.latitude);
                    $('iframe[name=map_iframe]').contents().find('#long').val($scope.vendor.longitude);

                }
                else
                    console.log("failed", data);
            });
        }
    }

    function loadCategory() {

        vendorService.getAllVendorCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.category = data.data;
                loadVendorPage();
            }
            else
                console.log("failed", data);
        });
    }
    $scope.Print = function (PrintDiv) {
        vendorService.Print(PrintDiv)
    }
    $scope.openinghours = [];
    $scope.addInput = function () {
        if ($scope.openinghours.length < 7) {
            $scope.openinghours.push({ startDay: '', startTime: '', startMinute: '', startType: '', endDay: '', endTime: '', endMinute: '', endType: '' });
        }
    }

    $scope.removeInput = function (index) {
        $scope.openinghours.splice(index, 1);
    }

    function openingHourAddUpdate() {
       
        if ($scope.checked == true) {
            $scope.vendor.fromday = "All Day";
            $scope.vendor.today = "All Day";
            $scope.vendor.fromtime = "24H";
            $scope.vendor.totime = "24H";
        }
        else {
            for (var i in $scope.openinghours) {
                $scope.vendor.fromday = $scope.vendor.fromday + "," + $scope.openinghours[i].startDay;
                $scope.vendor.today = $scope.vendor.today + "," + $scope.openinghours[i].endDay;
                $scope.vendor.fromtime = $scope.vendor.fromtime + "," + $scope.openinghours[i].startTime + ':' + $scope.openinghours[i].startMinute +" "+ $scope.openinghours[i].startType;
                $scope.vendor.totime = $scope.vendor.totime + "," + $scope.openinghours[i].endTime + ':' + $scope.openinghours[i].endMinute +" "+ $scope.openinghours[i].endType;
            }
        }
    }

    function openingHourGet() {

        var openinghours = [];
        for (var i in $scope.vendor.OpeningHours) {
            if ($scope.vendor.OpeningHours[i].fromday == "All Day") {
                $scope.checked = true;
            }
            else {
                var temp = { startDay: '', startTime: '', startMinute: '', startType: '', endDay: '', endTime: '', endMinute: '', endType: '' };
                temp.startDay = $scope.vendor.OpeningHours[i].fromday.trim();
                temp.endDay= $scope.vendor.OpeningHours[i].today.trim();

                var str = $scope.vendor.OpeningHours[i].fromtime.trim();
                var hour = str.substring(0, 2);
                var min = str.substring(3, 5);
                var type = str.substring(6, 8);
                temp.startTime = hour;
                temp.startMinute = min;
                temp.startType = type;

                var str1 = $scope.vendor.OpeningHours[i].totime.trim();
                var hour1 = str1.substring(0, 2);
                var min1 = str1.substring(3, 5);
                var type1 = str1.substring(6, 8);
                temp.endTime = hour1;
                temp.endMinute = min1;
                temp.endType = type1;

                $scope.openinghours.push(temp);
            }

        }
    }

    function plotmap(lati, lonng) {

        var myLatlng = new google.maps.LatLng(lati, lonng);
        var mapOptions = {
            zoom: 5,
            center: myLatlng
        }
        var map = new google.maps.Map(document.getElementById('gmap'), mapOptions);

        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map

        });
    }


}])

