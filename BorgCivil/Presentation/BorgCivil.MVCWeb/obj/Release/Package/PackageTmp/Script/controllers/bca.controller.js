(function() {
    'use strict';

    /**
     * Tabs Controller
     * Created by: Rahul Yadav (SIPL)
     * Created On: 28-03-2017
     */

    angular
        .module('borgcivil')
        //.controller('JobBookingCtrl', JobBookingCtrl);
        .controller('TabCtrl', TabCtrl)
        .controller('StartDatepickerCtrl', StartDatepickerCtrl)
        .controller('EndDatepickerCtrl', EndDatepickerCtrl)
        .controller('ModalInstanceCtrl', ModalInstanceCtrl)
        .controller('DateTimePickerCtrl', DateTimePickerCtrl);

    //Inject required stuff as parameters to factories controller function
    //LoginCtrl.$inject = ['$scope', 'UtilsFactory', '$location'];
    TabCtrl.$inject = ['$scope', '$window'];


    /**
     * Tab function
     */
    function TabCtrl($scope, $window){
       $scope.model = {
        name: 'Tabs'
      };
    }

    //StartDatepickerCtrl
    function StartDatepickerCtrl($scope) {
        $scope.today = function () {
            $scope.dt = new Date();
        };
        $scope.today();

        $scope.clear = function () {
            $scope.dt = null;
        };

        // Disable weekend selection
        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };
        $scope.toggleMin();

        //$scope.open = function ($event) {
        //    $event.preventDefault();
        //    $event.stopPropagation();

        //    $scope.opened = true;
        //};

        $scope.startDate = function () {
            $scope.popup1.opened = true;
        };

        $scope.endDate = function () {
            $scope.popup2.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.formats = ['dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];

        $scope.popup1 = {
            opened: false
        };

        $scope.popup2 = {
            opened: false
        };
    }

    //EndDatepickerCtrl
    function EndDatepickerCtrl($scope) {
        $scope.today = function () {
            $scope.dt = new Date();
        };
        $scope.today();

        $scope.clear = function () {
            $scope.dt = null;
        };

        // Disable weekend selection
        $scope.disabled = function (date, mode) {
            return (mode === 'day' && (date.getDay() === 0 || date.getDay() === 6));
        };

        $scope.toggleMin = function () {
            $scope.minDate = $scope.minDate ? null : new Date();
        };
        $scope.toggleMin();

        $scope.open = function ($event) {
            $event.preventDefault();
            $event.stopPropagation();

            $scope.opened = true;
        };

        $scope.dateOptions = {
            formatYear: 'yy',
            startingDay: 1
        };

        $scope.formats = ['dd-MM-yyyy', 'yyyy/MM/dd', 'dd.MM.yyyy', 'shortDate'];
        $scope.format = $scope.formats[0];
    }

    //DateTimePickerCtrl
    function DateTimePickerCtrl($scope) {
        // openCalendar Function 
        $scope.openCalendar = function () {
            $('#dateTime').focus();
        };
    }

    

    ModalInstanceCtrl.$inject = ['$scope', '$uibModalInstance'];
    // ModalInstanceCtrl for close popup
    function ModalInstanceCtrl($scope, $uibModalInstance) {

        //Assign controller scope pointer to a variable
        var vm = this;
        vm.ok = function () {
            $uibModalInstance.close();
        };

        vm.cancel = function () {
            $uibModalInstance.dismiss('cancel');
        };
    };
    

})();

