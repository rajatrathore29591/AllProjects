'use strict';
app.controller('analytics_ActivePromotionController', ['$scope', '$rootScope', 'analytics_ActivePromotionService', '$cookies', '$cookieStore', 'initData', '$location', function ($scope, $rootScope, analytics_ActivePromotionService, $cookies, $cookieStore, initData, $location) {

    $scope.date = new Date();
    $scope.contacts = [{ type: "AppType", value: "Weekly" }, { type: "AppType", value: "Monthly" }, { type: "AppType", value: "Yearly" }, ];
    $scope.contactTypes = { "AppType": { default: "Weekly" } };
    OnloadView();
    $scope.Chart = true;
    $scope.showLoader = true;
    $rootScope.active = 'Analytics';
    $rootScope.active1 = 'ActivePromotion';
    $scope.Map1 = true; $scope.Map2 = true;
    if ($cookieStore.get('loggedUserName') === undefined) {
        $location.path('/');
    } else {
        //initData.initFunction();
        $rootScope.UserName = $cookieStore.get('loggedUserName');
    }

    $scope.change = function () { alert(); };
    $scope.ShowMapDate = function () { alert(); }
    $scope.ShowMap = function (scope, Id) {
   
        $scope.categoryId = Id;
        $scope.Type = _.pluck($scope.contactTypes, 'default');
        $scope.fromdate = String($('#fromDate').val()); $scope.toDate = String($('#toDate').val()); $scope.vendorId = scope.vendorId; $scope.time = (String($scope.Type)).charAt(0);
        if ($scope.fromdate == $scope.toDate) { alert("From date and To date should be diffrent") } else {
            $scope.VendorNameShow = 'Vendor : ' + scope.Vendor;
            analytics_ActivePromotionService.GetAllChartByTime($scope, function (success, data) {
                if (success) {
                    $scope.DataCategoryWise = JSON.parse(data.data1);
                    GetVandor($scope.DataCategoryWise);
                    $scope.labelGet = _.pluck($scope.DataCategoryWise, 'Wise');
                    $scope.valueGet = _.pluck($scope.DataCategoryWise, 'Total');
                    $scope.LineChartShow();
                }
                else
                    console.log("failed", data);
            });
            $scope.LineChart = true;
        }

    }

    $scope.ShowMapTotal = function (scope) {

        $scope.catArraryTotal = [];

        $scope.catArraryTotal[0] = { 'y': Number(scope['Restaurant']), 'label': 'Restaurant' };
        $scope.catArraryTotal[1] = { 'y': Number(scope['Cafe']), 'label': 'Cafe' };
        $scope.catArraryTotal[2] = { 'y': Number(scope['Dessert']), 'label': 'Dessert' };


        var chart = new CanvasJS.Chart("chartContainer",
        {
            title: { text: "Total" }, data: [{
                type: "doughnut",
                dataPoints: $scope.catArraryTotal
            }]
        });
        chart.render();

    }

    $scope.ShowMapPlot = function (scope) {
        OnloadView();
        $scope.LineChart = false;
        $scope.Map2 = true;
        $scope.VendorNameShow = null;
        GetAllMapData($scope.sp_data)
    }

    $scope.LineChartShow = function () {
        var Dates = $scope.fromdate + " To " + $scope.toDate;

        $scope.TimeArrary = [];
        var i = 0;
        _.each($scope.valueGet, function (data) {
            $scope.TimeArrary[i] = { 'y': Number(data), 'label': $scope.labelGet[i] }; i++;
        });

        var chart = new CanvasJS.Chart("chartContainer",
   { title: { text: Dates }, data: [{ type: "line", dataPoints: $scope.TimeArrary }] });
        chart.render();

    }

    $scope.LineChartShowTotal = function (scope) {

        var names = "Vendor Name: " + scope['Vendor'];
        var Maindata = _.initial(_.values(_.omit(scope, 'Vendor', 'vendorId', 'Total')))
        $scope.List = []; $scope.categoryList = ['Restaurant', 'Cafe', 'Dessert'];
        var i = 0;
        _.each(Maindata, function (data) {
            $scope.List[i] = { 'y': Number(data), 'label': $scope.categoryList[i] }; i++;
        });


        var chart = new CanvasJS.Chart("chartContainer",
        { title: { text: names }, data: [{ type: "line", dataPoints: $scope.List }] });
        chart.render();

    }
    $scope.VendorClick = function ($http) {
        $scope.vid = $http.vendorId;
        $scope.categoryBussId = JSON.stringify($http.businessCategory);
        $scope.categorySelect = $scope.category[0];
        $scope.sp_data = $scope.HoldData;
        $scope.master_single = _.where($scope.sp_data, { vendorId: String($scope.vid) });
        $scope.sp_data = _.where($scope.sp_data, { vendorId: String($scope.vid) });
    }

    $scope.CategoryClick = function ($http) {
        $scope.sp_data = $scope.master_single;
        $scope.vid = $http;
        var answer = _.map($scope.sp_data, 'Vendor');
        answer = _.map($scope.sp_data, $http);
        var newArr = _.map($scope.sp_data, function (o) { return _.pick(o, 'Vendor', $http); });
        $scope.sp_data = null;
        $scope.sp_data = newArr;
    }

    function OnloadView() {
        analytics_ActivePromotionService.GetAllChartByid($scope, function (success, data) {
            if (success) {
                $scope.sp_dataByID = JSON.parse(data.data1);
                GetVandor($scope.sp_dataByID);
            }
            else
                console.log("failed", data);
        });
        $scope.fromdate = String($('#fromDate').val()); $scope.toDate = String($('#toDate').val());
        analytics_ActivePromotionService.GetAllChart($scope, function (success, data) {
            if (success) {
                $scope.sp_data = JSON.parse(data.data1);
                $scope.Master = JSON.parse(data.data1);
                $scope.Masterkey = JSON.parse(data.data1);
            }
            else
                console.log("failed", data);
            if ($scope.Masterkey != null) {
                $scope.Hader = _.keys($scope.Masterkey[0]);
                $scope.Hader.splice(0, 1);
                $scope.category = _.keys($scope.Masterkey[0]);
                $scope.category.splice(0, 2);
                $scope.category.pop();
                $scope.categorySelect = $scope.category[0];

                $scope.GetAllVendors = $scope.Master;
                $scope.vendor_name = $scope.Master[0];
                $scope.GetAllVendors.pop();
                $scope.sp_data = JSON.parse(data.data1);
                $scope.HoldData = JSON.parse(data.data1);
                var data = _.pluck($scope.GetAllVendors, "Vendor");
                GetAllMapData($scope.sp_data);
            }
        });
    }

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

    function GetAllMapData(sp_dataByID) {
        //console.log(JSON.stringify(sp_dataByID));
        $scope.cats = []; $scope.vendorName = []; $scope.cat1Arrary = []; $scope.cat2Arrary = []; $scope.cat3Arrary = [];
        $scope.vendorName; $scope.cat1; $scope.cat2; $scope.cat3;
        $scope.vendorName = _.initial(_.pluck(sp_dataByID, 'Vendor'));
        $scope.vendorNameShort = _.initial(_.pluck(sp_dataByID, 'Vendor'));
        $scope.cat1 = _.initial(_.pluck(sp_dataByID, 'Restaurant'));
        $scope.cat2 = _.initial(_.pluck(sp_dataByID, 'Cafe'));
        $scope.cat3 = _.initial(_.pluck(sp_dataByID, 'Dessert'));

        var i = 0;
        _.each($scope.vendorName, function (data) {
            $scope.vendorName[i] = { 'y': 0, 'label': data }; i++;
        }); i = 0;
        _.each($scope.cat1, function (data) {
            $scope.cat1Arrary[i] = { 'y': Number(data), 'label': 'Restaurant' }; i++;
        }); i = 0;
        _.each($scope.cat2, function (data) {
            $scope.cat2Arrary[i] = { 'y': Number(data), 'label': 'Cafe' }; i++;
        }); i = 0;
        _.each($scope.cat3, function (data) {
            $scope.cat3Arrary[i] = { 'y': Number(data), 'label': 'Dessert' }; i++;
        }); i = 0;

        BarChart(sp_dataByID);
    }
    function GetVandor(sp_dataByID) {
        $scope.vendorName = []; $scope.cat1 = []; $scope.cat2 = []; $scope.cat3 = [];
        if (sp_dataByID !== null) {
            for (var i = 0; i < sp_dataByID.length; i++) {
                $scope.vendorName[i] = sp_dataByID[i].companyName;

            }


            $scope.vendorName = _.uniq($scope.vendorName);

            var i = 0;
            _.each($scope.vendorName, function (data) {

                var filteredGoal = _.where(sp_dataByID, { companyName: data });

                $scope.cat1[i] = "Y:" + _.size(_.where(filteredGoal, { categoryName: 'Restaurant' }));
                $scope.cat2[i] = _.size(_.where(filteredGoal, { categoryName: 'Cafe' }));
                $scope.cat3[i] = _.size(_.where(filteredGoal, { categoryName: 'Dessert' }));
                i++;
            });
        }
    }


    $scope.showLoader = false;
    function BarChart(sp_dataByID) {


        var chart = new CanvasJS.Chart("chartContainer",
        {
            title: {
                text: "Active Promotion "

            },
            data: [{
                type: "stackedColumn",
                dataPoints: $scope.cat1Arrary
            }
           , {
               type: "stackedColumn",
               dataPoints: $scope.cat2Arrary
           }
          , {
              type: "stackedColumn",
              dataPoints: $scope.cat3Arrary
          }
           , {
               type: "stackedColumn",
               dataPoints: $scope.vendorName
           }]
        });

        chart.render();
    }
    $scope.ShowDoughnutChart = function (html) {
        DoughnutChart(html);
    }

    function DoughnutChart(CategoryName) {
        $scope.Lab = []; $scope.Val = []; $scope.catArrary = [];
        var i = 0;
        _.each($scope.vendorNameShort, function (data) {
            var filteredGoal = _.where($scope.sp_dataByID, { companyName: data });
            $scope.Lab[i] = _.pluck(_.where(filteredGoal, { categoryName: CategoryName }), 'companyName');
            $scope.Val[i] = _.size(_.where(filteredGoal, { categoryName: CategoryName }), 'companyName');

            i++;
        });
        i = 0;
        _.each($scope.Lab, function (data) {
            $scope.catArrary[i] = { 'y': Number(_.size(data)), 'label': String(_.uniq(data)) };
            i++;
        });

        var CategoryType = _.pluck($scope.GetAllVendors, CategoryName);

        var chart = new CanvasJS.Chart("chartContainer",
        {
            title: {
                text: CategoryName
            },
            data: [
            {
                type: "doughnut",
                dataPoints: $scope.catArrary
            }
            ]
        });

        chart.render();

    }
    function getRandomColor() {
        var letters = '0123456789ABCDEF'.split('');
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }


    $(".datepicker").datepicker(
        {
            autoclose: true,
            todayHighlight: true,
            format: 'yyyy-mm-dd',
            todayBtn: true

        });

}])
