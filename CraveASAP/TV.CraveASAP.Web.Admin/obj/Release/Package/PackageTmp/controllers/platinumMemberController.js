'use strict';
app.controller('platinumMemberController', ['$scope', '$stateParams', '$location', '$rootScope', 'platinumMemberService', '$cookies', '$cookieStore', 'dateFilter','initData', function ($scope, $stateParams, $location, $rootScope, platinumMemberService, $cookie, $cookieStore, dateFilter,initData) {
    $scope.showTable = true;
    $scope.path = $scope.config.api + "/Pictures/"; $scope.hideImg = false;
    $rootScope.isLogged = 1;
    $scope.image = "Default.png";
    $scope.Text = "Save";
    $scope.rewardID = ($stateParams.id > 0) ? $stateParams.id : 0;
    $scope.randomCode = randomPromotionCode();
    $rootScope.active = "PointReward";
    $rootScope.active1 = "Premium";
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    function addUserId() {

        for (var i in $scope.user) {
            $scope.temp = $scope.temp + "," + $scope.user[i].userId;

        }
    }

    $scope.Save = function () {
        if ($scope.user != undefined) {
            $scope.isbusy = true;
            $scope.Message = "Reward has been Successfully Created!";
            var allSrcs = Array();
            $("#rewardImages img").each(function () {
                allSrcs.push($(this).attr('src'));
                $scope.imgBase64 = $(this).attr('src');
                $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
                
            });
            var dateGet = $('#endate').val();
            $scope.NewDate = formatDate1(dateGet);
            addUserId();
            var data = {
                image: $scope.imgBase64, code: $scope.randomCode, expiryHours: $scope.PremReward.expiryHours, endDate: $scope.NewDate,
                isSpecial: $scope.PremReward.isSpecial, nextAvailability: $scope.PremReward.nextAvailability, point: $scope.PremReward.point, language: "English", type: "Prem",
                message: $scope.notification, temp: $scope.temp, link: $scope.PremReward.link
            };

            platinumMemberService.savePremium($scope, data, function (success, data) {
                $scope.redirect = function () {
                    $location.path("/Main/PlatinumRewardList");
                }
                if (success) {
                    $("#static").modal('show');
                    $scope.isbusy = false;
                }
                else {
                    console.log("failed", data);
                }
            });
        }
        else {
            $scope.Message = "please click on select!";
            $scope.redirect = function () {
                $scope.isbusy = false;
            }
            $("#static").modal('show');
            
        }
    }

    $scope.random = function () {
        $scope.isbusy = true;
        if ($scope.PremReward1.count == undefined) {
            $scope.PremReward1.count = 0;
        }
        if ($scope.num != undefined) {
            if ($scope.num <= $scope.PremReward1.count) {

                platinumMemberService.getRandomUser($scope, function (success, data) {
                    if (success) {
                        
                        $scope.user = data;

                        $scope.isbusy = false;
                    }
                    else {
                        console.log("failed", data);
                    }
                });

            }
            else {
                $scope.Message = "users should be less than total no users!";
                $("#static").modal('show');
                $scope.redirect = function () {
                    $scope.isbusy = false;
                }

            }
        }
        else {
            $scope.Message = "Please select total num of platinum users!";
            $("#static").modal('show');
            $scope.isbusy = false;
        }
    }

    platinumMemberService.getUserCount($scope, function (success, data) {
        if (success) {
            $scope.PremReward1 = data;
        }
        else
            console.log("failed", data);
    });

    loadRewardPage();

    function loadRewardPage() {

        if ($scope.rewardID == 0) {

        }
        else {
            $scope.Text = "Relaunch";
            $scope.isbusy = true;
            $scope.Premiumtitle = 'Edit Premium Reward';
            platinumMemberService.getRewardById($scope, $scope.rewardID, function (success, data) {
                if (success) {
                    
                    $scope.PremReward = data;
                    $scope.NewDate = formatDate($scope.PremReward.endDate);
                    $scope.image = $scope.PremReward.image;
                    //$scope.hideImg = true;
                    $scope.randomCode = $scope.PremReward.code;
                    $scope.editMode = 1;
                    $scope.isbusy = false;
                    //$scope.showTable = false;
                }
                else
                    console.log("failed", data);
            });
        }
    } 

    $scope.submit = function () {
        if ($scope.PremReward.point != "0" && $scope.PremReward.nextAvailability != "0") {
            if ($scope.Text == "Save".trim()) {
                if ($('#endate').val() != '') {
                    $scope.Save();
                }
                else {
                    $scope.Message = "Please select end date";
                    $('#static').modal('show');
                }
            }
            else {
                if ($('#endate').val() != '') {
                    $scope.Update();
                }
                else {
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

    $scope.Update = function () {
        if ($scope.user != undefined) {
            
            $scope.Message = "Reward Successfully Updated!";
            $scope.isbusy = true;
            var allSrcs = Array();
            $("#rewardImages img").each(function () {
                allSrcs.push($(this).attr('src'));
                $scope.imgBase64 = $(this).attr('src');
                $scope.imgBase64 = $scope.imgBase64.replace('data:image/png;base64,', "");
               
            });
            if ($scope.imgBase64.length > 150) {
                $scope.image = $scope.imgBase64;
            }
            else {
                $scope.image = "";
            }
            var dateGet = $('#endate').val();
            $scope.NewDate = formatDate1(dateGet);
            addUserId();
            var data = {
                rewardId: $scope.PremReward.rewardId,
                image: $scope.image, code: $scope.randomCode, expiryHours: $scope.PremReward.expiryHours, endDate: $scope.NewDate,
                isSpecial: $scope.PremReward.isSpecial, nextAvailability: $scope.PremReward.nextAvailability, point: $scope.PremReward.point, language: "English", type: "Prem",
                message: $scope.notification, temp: $scope.temp, link: $scope.PremReward.link

            };


            platinumMemberService.updatePremium($scope, data, function (success, data) {

                $scope.redirect = function () {
                    $location.path("/Main/PlatinumRewardList");
                }

                if (success) {
                    $("#static").modal('show');
                    $scope.isbusy = false;
                }
                else {
                    console.log("failed", data);
                }

            });
        }
        else {
            $scope.Message = "please click on select!";
            $("#static").modal('show');
        }

    }

    $scope.HideImgReward = function () {
        $scope.hideImg = false;
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
        todayBtn: true
    });

   
    $('.clear1').click(function () {

        $('#endate').val('');
        $scope.NewDate = null;
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
