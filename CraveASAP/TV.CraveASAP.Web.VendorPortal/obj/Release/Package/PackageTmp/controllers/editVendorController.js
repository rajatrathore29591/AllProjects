'use strict';

app.controller('editVendorController', ['$scope', 'editVendorService', '$rootScope', '$cookies', '$cookieStore', '$location', 'translationService', function ($scope, editVendorService, $rootScope, $cookies, $cookieStore, $location, translationService) {
    $rootScope.isLogged = 1;
    $scope.outcome = {};
    $scope.outcome.checked = false;
    $scope.active = "EditVendor";
    $rootScope.SessionVendorId = $cookieStore.get('loggedId');
    $scope.loginVendorId = $cookieStore.get('loggedId');
    $rootScope.key = $cookieStore.get('oAuthkey');
    $scope.loader = true;

    if ($cookieStore.get('loggedName') === undefined) {
        $location.path('/');
    } else {

        $rootScope.UserName = $cookieStore.get('loggedName');
    }

    loadCategory();
  
    function init(lati,long) {
        
        var myLatlng = new google.maps.LatLng(lati, long);

        var mapProp = {
            center: myLatlng,
            zoom: 10,
            mapTypeId: google.maps.MapTypeId.ROADMAP
        };
        var map = new google.maps.Map(document.getElementById("venuemap"), mapProp);
        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map
        });
        plotmap(lati, long);
    }

    /*---------------------------------------------------------Grab My Location Start-------------------------------------------------------------------------*/

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
                    $('#lat').val(lati);
                    $('#lang').val(lonng);
                    plotmap(lati, lonng);
                }

            }
        });
    }

    function plotmap(lati, lonng) {
       
        var myLatlng = new google.maps.LatLng(lati, lonng);
        var mapOptions = {
            zoom: 10,
            center: myLatlng
        }
        var map = new google.maps.Map(document.getElementById('venuemap'), mapOptions);

        var marker = new google.maps.Marker({
            position: myLatlng,
            map: map

        });
    }
    /*---------------------------------------------------------Grab My Location End-------------------------------------------------------------------------*/

    function loadCategory() {

        editVendorService.getAllVendorCategory($scope, function (success, data, callback) {
            if (success) {
                $scope.category = data.data;
                loadVendorPage();
            }
            else
                console.log("failed", data);
        });
    }

    function loadVendorPage() {
            $scope.vendortitle = 'Edit Vendor';
            $scope.showTable = !$scope.showTable;
            $scope.showLoader = true;
            editVendorService.getVendorById($scope, $scope.loginVendorId, function (success, data) {
                if (success) {

                    $scope.vendor = data.data1;
                    //$rootScope.categoryId = ;
                    $cookieStore.put('categoryId', $scope.vendor.businessCategory);
                    $scope.vendor.phoneNo = parseInt($scope.vendor.phoneNo);
                    $scope.showLoader = false;
                    openingHourGet();
                    $scope.showTable = "Update";
                    $scope.editMode = 1;
                    init($scope.vendor.latitude, $scope.vendor.longitude);
                    $scope.loader = false;
                }
                else
                    console.log("failed", data);
            });
    }

    $scope.fileChanged = function (e) {
        //alert(e.target.files);
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

    //$scope.clearFile = function () {
    //    alert()
    //    this.val = null;
    //}

    $scope.manage = function () {

        $location.path('/Main/Manage');
    }

    $scope.clear = function () {
        $scope.imageCropStep = 1;
        delete $scope.imgSrc;
        delete $scope.result;
        delete $scope.resultBlob;
        $("#cropm").modal('hide');
    };
    function loaderheight() {
        var bodyheight = $('body').height();
        $('#loader').css('height', bodyheight);
    }

   

    $scope.update = function () {
        $scope.showLoader = true;
        $scope.Message = $scope.translation.YourProfileUpdatedsuccessfully;
        var allSrcs = Array();
        $("#vendorImages img").each(function () {
            allSrcs.push($(this).attr('src'));
            $scope.imgBase64 = $(this).attr('src');
            $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
        });
        openingHourAddUpdate();
        $scope.vendor.streetName = $('#location').val();
        
        if ($scope.imgBase64.length > 150) {
            $scope.img = $scope.imgBase64;
        }
        else {
            $scope.img = "";
        }
        $scope.vendor.latitude =  $('#lat').val();
        $scope.vendor.longitude = $('#lang').val();
        $cookieStore.put('VendorShort', $scope.vendor.shortDescription);
        var data = {
            vendorId: $scope.vendor.vendorId, loginVendorName: $scope.vendor.loginVendorName, companyName: $scope.vendor.companyName, businessCategory: $scope.vendor.businessCategory, shortDescription: $scope.vendor.shortDescription, fullDescription: $scope.vendor.fullDescription,
            email: $scope.vendor.email, phoneNo: $scope.vendor.phoneNo, contactPerson: $scope.vendor.contactPerson, contactPhoneNo: $scope.vendor.contactPhoneNo,
            contactEmail: $scope.vendor.contactEmail, streetName: $scope.vendor.streetName, postCode: $scope.vendor.postCode, buildingName: $scope.vendor.buildingName,
            floor: $scope.vendor.floor, area: $scope.vendor.area, city: $scope.vendor.city, longitude: $scope.vendor.longitude, latitude: $scope.vendor.latitude
            , logoImg: $scope.img
            , fromday: $scope.vendor.fromday, today: $scope.vendor.today, fromtime: $scope.vendor.fromtime, totime: $scope.vendor.totime

        };
        editVendorService.updateVendors($scope, $rootScope, data, function (success, data) {
            if (success) {
                $cookieStore.put('VendorLocal', data.Image);
                $("#static").modal('show');
                $scope.showLoader = false;
                loaderheight();
                $scope.redirect = function () {
                    location.reload();
                }
            
               
            }
            else {
                console.log("failed", data);
            }
        });
    }

    $scope.openinghours = [];
    $scope.addInput = function () {
        if ($scope.openinghours.length < 7) {
            $scope.openinghours.push({ startDay: '', startTime: '',  startType: '', endDay: '', endTime: '', endType: '' });
        }
    }

    $scope.removeInput = function (index) {
        $scope.openinghours.splice(index, 1);
    }

    function openingHourAddUpdate() {
        for (var i in $scope.openinghours) {
            if ($scope.outcome.checked == true) {
                $scope.vendor.fromday = "All Day";
                $scope.vendor.today = "All Day";
                $scope.vendor.fromtime = "24H";
                $scope.vendor.totime = "24H";
            }

            else {
                $scope.vendor.fromday = $scope.vendor.fromday + "," + $scope.openinghours[i].startDay;
                $scope.vendor.today = $scope.vendor.today + "," + $scope.openinghours[i].endDay;
                $scope.vendor.fromtime = $scope.vendor.fromtime + "," + $scope.openinghours[i].startTime + " " + $scope.openinghours[i].startType;
                $scope.vendor.totime = $scope.vendor.totime + "," + $scope.openinghours[i].endTime + " "+ $scope.openinghours[i].endType;
            }
        }
    }

    function openingHourGet() {

        var openinghours = [];
        if ($scope.vendor.OpeningHours != null)
        {
            for (var i in $scope.vendor.OpeningHours) {
            if ($scope.vendor.OpeningHours[i].fromday == "All Day") {
                $scope.outcome.checked = true;

            }
            else {
                var temp = { startDay: '', startTime: '', startType: '', endDay: '', endTime: '', endType: '' };
                temp.startDay = $scope.vendor.OpeningHours[i].fromday.trim();
                temp.endDay = $scope.vendor.OpeningHours[i].today.trim();
                
                var str = $scope.vendor.OpeningHours[i].fromtime.trim();
                
                var hour = str.substring(0, 2);
                var min = str.substring(3, 5);
                var type = str.substring(6, 8);
               
                temp.startTime = hour + ':' + min;
                temp.startType = type;
                var str1 = $scope.vendor.OpeningHours[i].totime.trim();
                var hour1 = str1.substring(0, 2);
                var min1 = str1.substring(3, 5);
                var type1 = str1.substring(6, 8);
                temp.endTime = hour1 +':'+ min1;
                temp.endType = type1;
                $scope.openinghours.push(temp);
            }

        }
      }
    }



    $('#location').blur(function () {
        locateMap();

    })

    function locateMap() {
        var geocoder = new google.maps.Geocoder();
        geocoder.geocode({
            address: $('input[name=address]').val(),
            region: 'no'
        },
            function (results, status) {

                if (status.toLowerCase() == 'ok') {
                    // Get center
                    var coords = new google.maps.LatLng(
                        results[0]['geometry']['location'].lat(),
                        results[0]['geometry']['location'].lng()
                    );

                    var lati = coords.lat();
                    var lonng = coords.lng();

                    var lati = coords.lat();
                    var long = coords.lng();
                    $("#lat").val(lati);
                    $("#lang").val(long);

                    var myLatlng = new google.maps.LatLng(lati, long);
                    var mapOptions = {
                        zoom: 10,
                        center: myLatlng
                    }
                    var map = new google.maps.Map(document.getElementById('venuemap'), mapOptions);

                    var marker = new google.maps.Marker({
                        position: myLatlng,
                        map: map

                    });

                }
            }
        );
    }

}])

