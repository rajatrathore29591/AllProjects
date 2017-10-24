'use strict';
app.controller('analytics_MultipleUsageController', ['$scope', '$rootScope', 'analytics_MultipleUsageService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, analytics_MultipleUsageService,$cookies, $cookieStore,initData) {
    $rootScope.active = 'Analytics'; $scope.date = new Date();
    $rootScope.active1 = 'MultipleUsage';
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

    analytics_MultipleUsageService.GetTrackUsage($scope, function (success, data) {
        if (success) {
            $scope.sp_data = JSON.parse(data.data1);
            $scope.Master = JSON.parse(data.data1);
            $scope.Masterkey = JSON.parse(data.data1);



        }
        else
            console.log("failed", data);
        $scope.Hader = _.keys($scope.Masterkey[0]);
        LineChart($scope.Masterkey);
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


    function LineChart(MapData) {

        
        
        $scope.Lab = []; $scope.Val = [];

        $scope.Lab = _.pluck(MapData, 'Promotion');
        $scope.Val = _.pluck(MapData, 'Users');
        var lineChartData = {
            labels: $scope.Val,
            datasets: [

                {
                    label: "My Second dataset",
                    fillColor: "rgba(151,187,205,0.2)",
                    strokeColor: "rgba(29,138,128,1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(151,187,205,1)",
                    data: $scope.Lab
                }
            ]

        }

        var ctx = document.getElementById("canvas").getContext("2d");
        window.myLine = new Chart(ctx).Line(lineChartData, {
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
