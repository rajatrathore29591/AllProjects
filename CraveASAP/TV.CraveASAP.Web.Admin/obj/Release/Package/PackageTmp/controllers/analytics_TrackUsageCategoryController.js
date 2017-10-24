'use strict';
app.controller('analytics_TrackUsageCategoryController', ['$scope', '$rootScope', 'analytics_TrackUsageService', '$cookies', '$cookieStore', 'initData', function ($scope, $rootScope, analytics_TrackUsageService, $cookies, $cookieStore, initData) {
    $rootScope.active = 'Analytics'; $scope.date = new Date();
    $rootScope.active1 = 'TrackUsageCategory';
    $scope.LineChart = false;
    $scope.Map1 = true;
    $scope.GlobleVariable = 0;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
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

        var lineChartData = {
            labels: ['Morning', 'Evening', 'Midday', 'Lunch'],
            datasets: [

                {
                    label: "My Second dataset",
                    fillColor: "rgba(151,187,205,0.2)",
                    strokeColor: "rgba(29, 138, 128, 1)",
                    pointColor: "rgba(151,187,205,1)",
                    pointStrokeColor: "#fff",
                    pointHighlightFill: "#fff",
                    pointHighlightStroke: "rgba(151,187,205,1)",
                    data: [$scope.Morning, $scope.Evening, $scope.Midday, $scope.Lunch]
                }
            ]

        }

        setData(lineChartData);
       

    }
    $scope.ShowMapPlot = function ()
    {
        $scope.VendorNameShow = "";
        if ($scope.GlobleVariable == 1) {
            window.myLine.removeData();
            window.myLine.removeData();
            window.myLine.removeData();
            $scope.GlobleVariable = 0;
        } else {

            window.myLine.removeData();
            window.myLine.removeData();
            window.myLine.removeData();
            window.myLine.removeData();
            
        }
        window.myLine.addData([$scope.Morning], "Morning");
        window.myLine.addData([$scope.Evening], "Evening");
        window.myLine.addData([$scope.Midday], "Midday");
        window.myLine.addData([$scope.Lunch], "Lunch");
      
       
    }
    $scope.BarChartSeparate = function (Lable) {
        $scope.VendorNameShow = Lable
        $scope.labelData = _.pluck($scope.sp_data, Lable);

      
        if ($scope.GlobleVariable == 0) {
            window.myLine.removeData(); window.myLine.removeData(); window.myLine.removeData(); window.myLine.removeData();
            $scope.GlobleVariable = 1;
        } else { window.myLine.removeData(); window.myLine.removeData(); window.myLine.removeData(); }
      
      
        window.myLine.addData([$scope.labelData[0]], "Restaurant");
        window.myLine.addData([$scope.labelData[1]], "Cafe");
        window.myLine.addData([$scope.labelData[2]], "Dessert");

    }
    function setData(lineChartData) {
     
        var ctx = document.getElementById("canvas").getContext("2d");
        ctx.clearRect(0, 0, canvas.width, canvas.height);
        window.myLine = new Chart(ctx).Line(lineChartData, {
        responsive: true
        });
    }

    function setData2(lineChartData) {
     
        var ctxP = document.getElementById("canvasZ").getContext("2d");
        ctxP.clearRect(0, 0, canvasZ.width, canvasZ.height);
        window.myLine = new Chart(ctxP).Line(lineChartData, {
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
