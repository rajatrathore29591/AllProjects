'use strict';
app.controller('Analytic_UserPerDayUseController', ['$scope', '$rootScope', 'Analytic_UserPerDayUseService','initData','$cookies', '$cookieStore', function ($scope, $rootScope, Analytic_UserPerDayUseService,initData,$cookies, $cookieStore) {

    $rootScope.active = 'Analytics'; $scope.date = new Date();
    $rootScope.active1 = "UserPerDayUse";
    $scope.LineChart = false;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
    $scope.Map1 = true; $scope.Map2 = true;
    $scope.ShowMap = function (scope) {
        $scope.fromdate = $('#fromDate').val(); $scope.toDate = $('#toDate').val(); $scope.vendorId = scope.vendorId;
        $scope.LineChart = true;
        $scope.compare();
        var randomScalingFactor = function () { return Math.round(Math.random() * 100) };
        var lineChartData = {
            labels: $scope.weeks,
            datasets: [
                {
                    label: "My Second dataset",
                    fillColor: "rgba(151,187,205,0.2)",
                    strokeColor: "rgba(151,187,205,1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(151,187,205,1)",
                    data: [1, 4, 3, 0, 1, 2]
                }
            ]
        }
        onload();
        function onload() {

            var ctx = document.getElementById("canvas").getContext("2d");
            window.myLine = new Chart(ctx).Line(lineChartData, {
                responsive: true
            });
        }
    }

    $scope.UserClick = function ($http) {
        $scope.MasterData = [];
        $scope.MasterData[0] = _.findWhere($scope.sp_data, { Users: $http.Users });
        var data = $scope.MasterData;
        LineChart(data)
    }

    Analytic_UserPerDayUseService.GetPerDayUses($scope, function (success, data) {
        if (success) {
            $scope.sp_data = JSON.parse(data.data1);
            $scope.User_data = JSON.parse(data.data1);
            $scope.Master = JSON.parse(data.data1);
            $scope.Masterkey = JSON.parse(data.data1);
        }
        else
            console.log("failed", data);

        $scope.UserList = _.pluck($scope.GetAllVendors, "Users");
        $scope.Hader = _.keys($scope.Masterkey[0]);
        $scope.user_name = $scope.User_data[0];
        $scope.sp_data = JSON.parse(data.data1);
        $scope.MasterData = JSON.parse(data.data1);
        $scope.MasterData = [];
        $scope.MasterData[0] = _.findWhere($scope.sp_data, { Users: 'Users' });
        $scope.GetValue = _.pluck($scope.sp_data, 'Users');
        var data = $scope.MasterData;
        $scope.Rest_Cafe = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Rest_Cafe); }, 0);
        $scope.Cafe_Dess = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Cafe_Dess); }, 0);
        $scope.Rest_Dess = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Rest_Dess); }, 0);
        $scope.Rest_Rest = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Rest_Rest); }, 0);
        $scope.Cafe_Cafe = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Cafe_Cafe); }, 0);
        $scope.Dess_Dess = _.reduce($scope.User_data, function (memo, num) { return memo + parseInt(num.Dess_Dess); }, 0);

        LineChart(data);
    });


    function LineChart(data) {
        var randomScalingFactor = function () { return Math.round(Math.random() * 100) };
        var lable = angular.copy($scope.Hader);
        var valuesGet = angular.copy($scope.MasterData);
        var values = [0,$scope.Rest_Cafe, $scope.Cafe_Dess, $scope.Rest_Dess, $scope.Rest_Rest, $scope.Cafe_Cafe, $scope.Dess_Dess];
        values.splice(0, 1);
        lable.splice(0, 1);
        var lineChartData = {
            labels: $scope.Hader,
            datasets: [

                {
                    label: "My Second dataset",
                    fillColor: "rgba(151,187,205,0.2)",
                    strokeColor: "rgba(151,187,205,1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(151,187,205,1)",
                    data: values
                }
            ]
        }

        var ctx = document.getElementById("canvas").getContext("2d");
        window.myLine = new Chart(ctx).Line(lineChartData, {
            responsive: true
        });
    }

    $(".datepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        todayBtn: true
    });
}])
