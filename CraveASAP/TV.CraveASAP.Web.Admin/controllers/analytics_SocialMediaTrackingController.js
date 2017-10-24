'use strict';
app.controller('analytics_SocialMediaTrackingController', ['$scope', '$rootScope', 'analytics_SocialMediaTrackingService', '$cookies', '$cookieStore','initData', function ($scope, $rootScope, analytics_SocialMediaTrackingService,$cookies, $cookieStore,initData) {
    $rootScope.active = 'Analytics'; $scope.date = new Date();
    $rootScope.active1 = 'SocialMediaTracking';
    $scope.LineChart = false;

    $scope.Map1 = true; $scope.Map2 = true;
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

    $scope.UserClick = function ($http) {
        $scope.MasterData = [];
        $scope.MasterData[0] = _.findWhere($scope.sp_data, { Users: $http.Users });
        var data = $scope.MasterData;
        LineChart(data)
}

    analytics_SocialMediaTrackingService.GetSocialMedia($scope, function (success, data) {
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
        $scope.GetValue = _.findWhere($scope.sp_data, { Users: 'Users' });
        var data = $scope.MasterData;
        LineChart(data);
    });


    function LineChart(data) {
        var randomScalingFactor = function () { return Math.round(Math.random() * 100) };
        var lable = angular.copy($scope.Hader);
        var valuesGet = angular.copy($scope.MasterData);
        var values = _.values($scope.GetValue);
       
        values.splice(0, 1);
        lable.splice(0, 1);
        var lineChartData = {
            labels: lable,
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
