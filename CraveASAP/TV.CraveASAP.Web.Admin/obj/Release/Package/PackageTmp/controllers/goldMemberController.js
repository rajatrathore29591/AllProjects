'use strict';
app.controller('goldMemberController', ['$scope', '$rootScope', '$location', '$stateParams', 'goldMemberService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, $location, $stateParams, goldMemberService, $cookies, $cookieStore,initData) {
    //$rootScope.active = "ManageApplication";
    $scope.Text = "Save";
    $rootScope.isLogged = 1;
    $scope.goldtitle = "Create Gold Member";
    $scope.hideImg = true;
    $scope.randomCode = randomPromotionCode();
    $rootScope.active = "PointReward";
    $rootScope.active1 = "Gold";
    $scope.image = "Default.png";
    $scope.rewardID = ($stateParams.id > 0) ? $stateParams.id : 0;
    loadRewardPage();
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.save = function () {
        if ($scope.goldReward.nextAvailability != "0" && $scope.goldReward.point != "0") {
            if ($scope.Text == "Save") {
                if ($('#endDate').val() != '') {
                    $scope.saveGold();
                } else {
                    $scope.Message = "Please select end date";
                    $('#static').modal('show');
                }
            }
            else {
                if ($('#endDate').val() != '') {
                    $scope.update();
                } else {
                    $scope.Message = "Please select end date";
                    $('#static').modal('show');
                }
            }
        }
        else {
            $scope.Message = "Point and Recurring should be greater than zero!";
            $('#static').modal('show');
        }
          
    }

    $scope.saveGold = function () {
        $scope.isbusy = true;
        var allSrcs = Array();
        $scope.Message = "Reward has been Successfully Created!";
        $("#rewardImages img").each(function () {
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

        var endDate = $('#endDate').val();
        $scope.endDate = formatDate1(endDate);

        var data = {
            rewardName: $scope.goldReward.rewardName, description: $scope.goldReward.description, image: $scope.img, code: $scope.randomCode, expiryHours: $scope.goldReward.expiryHours,
            isSpecial: $scope.goldReward.isSpecial, nextAvailability: $scope.goldReward.nextAvailability, language: "English", type: "Gold", endDate: $scope.endDate, point: $scope.goldReward.point

        };

        goldMemberService.saveGold($scope, data, function (success, data) {
            $scope.redirect = function () {
                $location.path("/Main/GoldRewardList");
            }
            if (success) {
                $scope.isbusy = false;
                $("#static").modal('show');

            }
            else {
                console.log("failed", data);
            }
        });
    }

    $scope.update = function () {
        $scope.isbusy = true;
        $scope.Message = "Reward has been Successfully Updated!";
        var allSrcs = Array();
        $("#rewardImages img").each(function () {
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

        var endDate = $('#endDate').val();
        $scope.endDate = formatDate1(endDate);
        //alert($scope.goldReward.point);
        var data = {
            rewardId: $scope.goldReward.rewardId, rewardName: $scope.goldReward.rewardName, description: $scope.goldReward.description, image: $scope.img, code: $scope.randomCode, expiryHours: $scope.goldReward.expiryHours,
            isSpecial: $scope.goldReward.isSpecial, nextAvailability: $scope.goldReward.nextAvailability, language: "English", type: "Gold", endDate: $scope.endDate, point: $scope.goldReward.point

        };
        goldMemberService.updateGold($scope, data, function (success, data) {
            $scope.redirect = function () {
                $location.path("/Main/GoldRewardList");
            }
            if (success) {
                $scope.isbusy = false;
                $("#static").modal('show');
            }
            else {
                console.log("failed", data);
            }
        });
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

    function loadRewardPage() {

        if ($scope.rewardID == 0) {

        }
        else {

            $scope.goldtitle = 'Edit Gold Member';
            goldMemberService.getRewardById($scope, $scope.rewardID, function (success, data) {
                if (success) {
                    console.log("Edit Data", data);

                    $scope.goldReward = data;
                    $scope.NewDate = formatDate($scope.goldReward.endDate);
                    $scope.Text = "Update";
                    $scope.randomCode = $scope.goldReward.code;
                    $scope.image = $scope.goldReward.image;
                    $scope.editMode = 1;

                }
                else
                    console.log("failed", data);
            });
        }
    }

    $scope.remove = function () {

        $scope.hideImg = false;
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

    $(".datepicker").datetimepicker({
        format: "mm-dd-yyyy hh:ii:ss",
        pick12HourFormat: false,
        showMeridian: true,
        autoclose: true,
        todayBtn: true
    });


    $('.clear1').click(function () {

        $('#endDate').val('');
        $scope.endDate = null;
    });

    //$(".datepicker").datepicker({
    //    autoclose: true,
    //    todayHighlight: true,
    //    format: 'yyyy/mm/dd',
    //    todayBtn: true
    //});

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
