'use strict';
app.controller('analytics_TrackUsageController', ['$scope', '$rootScope', 'analytics_TrackUsageService', '$cookies', '$cookieStore', '$location','initData', function ($scope, $rootScope, analytics_TrackUsageService, $cookies, $cookieStore, $location,initData) {
    $rootScope.active = 'Analytics'; $scope.date = new Date();
    $rootScope.active1 = 'TrackUsage';
    $scope.LineChart = false;

    $scope.Map1 = true; $scope.Map2 = true;
    // BarChart();
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }
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
                    data: [1, 2, 3, 5, 10, 6, 7, 8, 9]
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

    $scope.VendorClick = function ($http) {
        $scope.categoryBussId = JSON.stringify($http.businessCategory);
       
        $scope.categorySelect = $scope.category[$scope.categoryBussId - 1];
    }

    analytics_TrackUsageService.GetTrackUsage($scope, function (success, data) {
        if (success) {
            $scope.sp_data = JSON.parse(data.data1);
            $scope.Master = JSON.parse(data.data1);
            $scope.Masterkey = JSON.parse(data.data1);
        }
        else
            console.log("failed", data);
        $scope.Hader = _.keys($scope.Masterkey[0]);
        $scope.category = _.keys($scope.Masterkey[0]);
        $scope.category.pop();
        $scope.categorySelect = $scope.category[0];
        $scope.GetAllVendors = $scope.Master;
        $scope.vendor_name = $scope.Master[0];
        $scope.GetAllVendors.pop();
        $scope.sp_data = JSON.parse(data.data1);
        $scope.labels = _.pluck($scope.sp_data, "Name");
        $scope.labelsMorning = _.pluck($scope.sp_data, "Morning"); $scope.labelsEvening = _.pluck($scope.sp_data, "Evening");
        $scope.labelsMidday = _.pluck($scope.sp_data, "Midday"); $scope.labelsLunch = _.pluck($scope.sp_data, "Lunch");
        $scope.Morning = _.reduce($scope.Masterkey, function (memo, num) { return memo + parseInt(num.Morning); }, 0);
        $scope.Evening = _.reduce($scope.Masterkey, function (memo, num) { return memo + parseInt(num.Evening); }, 0);
        $scope.Midday = _.reduce($scope.Masterkey, function (memo, num) { return memo + parseInt(num.Midday); }, 0);
        $scope.Lunch = _.reduce($scope.Masterkey, function (memo, num) { return memo + parseInt(num.Lunch); }, 0);
        BarChart();
    });

    $scope.compare = function () {
        var oneDay = 24 * 60 * 60 * 1000; // hours*minutes*seconds*milliseconds
        var firstDate = new Date($scope.fromdate); var secondDate = new Date($scope.toDate);
        var diffDays = Math.round(Math.abs((firstDate.getTime() - secondDate.getTime()) / (oneDay)));
        var weeks = Math.floor(diffDays / 7); $scope.weeks = [];
        for (var i = 1; i <= weeks; i++) {
            if (i === 1) { $scope.weeks[0] = 'W1'; } else { $scope.weeks[i - 1] = 'W' + i; }
        }

        $scope.Months = Math.floor(diffDays / 30);
        $scope.years = Math.floor(diffDays / 365);
    }


    function BarChart() {

        var barChartData = {
            labels: $scope.labels,
            datasets: [

                {
                    fillColor: "rgba(166,38,80,1)",
                    strokeColor: "rgba(166,38,80,1)",
                    highlightFill: "rgba(166,38,80,1)",
                    highlightStroke: "rgba(166,38,80,1)",
                    data: $scope.labelsMorning
                }
                , {
                    fillColor: "rgba(29,138,128,1)",
                    strokeColor: "rgba(29,138,128,1)",
                    highlightFill: "rgba(29,138,128,1)",
                    highlightStroke: "rgba(29,138,128,1)",
                    data: $scope.labelsEvening
                },
             {
                 fillColor: "rgba(167, 72, 17,1)",
                 strokeColor: "rgba(167, 72, 17,1)",
                 highlightFill: "rgba(167, 72, 17,1)",
                 highlightStroke: "rgba(167, 72, 17,1)",
                 data: $scope.labelsMidday
             },
            {
                fillColor: "rgba(38, 42, 166,1)",
                strokeColor: "rgba(38, 42, 166,1)",
                highlightFill: "rgba(38, 42, 166,1)",
                highlightStroke: "rgba(38, 42, 166,1)",
                data: $scope.labelsLunch
            }

            ]

        }

        var ctx = document.getElementById("canvas").getContext("2d");
        window.myBar = new Chart(ctx).Bar(barChartData, {
            responsive: true
        });

    }
    $scope.ShowDoughnutChart = function () {
        DoughnutChart();
        $scope.Map2 = true;
    }
    function DoughnutChart() {

        var doughnutData = [];
        for (var i = 0; i < $scope.vendorName.length; i++) {
            alert();
            var datas = {
                value: $scope.cat1[i],
                color: getRandomColor(),
                highlight: getRandomColor(),
                label: $scope.vendorName[i]
            };
            doughnutData.push(datas);

        }


        
        var ctx = document.getElementById("chart-area").getContext("2d");
        window.myDoughnut = new Chart(ctx).Doughnut(doughnutData, { responsive: true });

    }
    function getRandomColor() {
        var letters = '0123456789ABCDEF'.split('');
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }


    $(".datepicker").datepicker({
        autoclose: true,
        todayHighlight: true,
        format: 'dd/mm/yyyy',
        todayBtn: true
    });

}])
