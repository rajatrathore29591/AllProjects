(function () {
    'use strict';

    /**
     * Booking Controller
     * Created by: (SIPL)
     */

    angular
        .module('borgcivil')
        .controller('WorkAllocationCtrl', WorkAllocationCtrl)
        .controller('ManageJobBookingCtrl', ManageJobBookingCtrl)
        .controller('ManageBookingFleetCtrl', ManageBookingFleetCtrl)
        .controller('ManageBookingSiteCtrl', ManageBookingSiteCtrl)
        .controller('BookingListCtrl', BookingListCtrl)
        .controller('DashboardBookingCtrl', DashboardBookingCtrl)
        .controller('ManageDocketCtrl', ManageDocketCtrl)
        .controller('DocketViewCtrl', DocketViewCtrl)

    //Inject required stuff as parameters to factories controller function
    WorkAllocationCtrl.$inject = ['$scope', '$state', '$stateParams', '$uibModal', 'WorkAllocationFactory', 'UtilsFactory', 'BCAFactory', '$filter'];

    /**
   * @name WorkAllocationCtrl
   * @param $scope
   * @param $uibModal
   * @param WorkAllocationFactory
   * @param UtilsFactory
   * @param BCAFactory
   * @param $filter
   */
    function WorkAllocationCtrl($scope, $state, $stateParams, $uibModal, WorkAllocationFactory, UtilsFactory, BCAFactory, $filter) {
        //Assign controller scope pointer to a variable
        var vm = this;
        // map create job booking property
        vm.attributes = {

            BookingFleet: {
                BookingFleetId: '',
                BookingId: '',
                FleetTypeId: '',
                FleetRegistrationId: '',
                DriverId: '',
                IsDayShift: '',
                Iswethire: '',
                AttachmentIds: '',
                NotesForDrive: '',
                IsfleetCustomerSite: false,
                Reason: '',
                StatusLookupId: '',
                BookingNumber: '',
                AllocationNotes: '',
                CreatedDate: '',
                EndDate: '',
                FleetBookingDateTime: new Date(),
                FleetBookingEndDate: '',
                CallingDateTime: '',
                IsActive: true,
                FleetName: ''
            },
        };
        vm.status = {
            BookingId: '',
            StatusValue: '',
            CancelNote: '',
            Rate: ''
        };
        vm.bookingId = '';
        vm.bookingFleetStatus = {
            BookingFleetId: '',
            StatusValue: ''
        }
        vm.bookingFleetHistory = [];
        vm.stateParamBookingId = '';
        vm.fleetType = '';
        vm.waListData = '';

        //Methods
        vm.getListing = getListing;
        vm.getStatusListing = getStatusListing;
        vm.getBookingInfo = getBookingInfo;
        vm.deleteBookingFleet = deleteBookingFleet;
        vm.bookingCustomerDetail = bookingCustomerDetail;
        //vm.manageFleetBooking = manageFleetBooking;
        vm.getAttachments = getAttachments;
        vm.getFleetypes = getFleetypes;
        vm.getBookingFleetDetail = getBookingFleetDetail;
        vm.fleetComplete = fleetComplete;
        vm.getBookingFleetHistory = getBookingFleetHistory;
        vm.getWorkAllocationByBookingId = getWorkAllocationByBookingId;
        vm.downloadExcelBookingList = downloadExcelBookingList;
        vm.downloadPrintBookingList = downloadPrintBookingList;
        vm.downloadPdfBookingList = downloadPdfBookingList;

        if (typeof $stateParams.BookingFleetId !== 'undefined' && typeof $stateParams.BookingId !== 'undefined') {
          
            // Calling when get StateParam of BookingFleetId
            vm.getBookingFleetHistory($stateParams.BookingFleetId, $stateParams.BookingId);

        } else if (typeof $stateParams.BookingId !== 'undefined') {
            // Calling when get StateParam of BookingId
            vm.getWorkAllocationByBookingId($stateParams.BookingId, 2);
            vm.stateParamBookingId = $stateParams.BookingId;
        } else {
            // calling method on load of page
            vm.getStatusListing('2');


        }

        /**
         * @name getFleetHistory
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getWorkAllocationByBookingId(bookingId, status) {
            vm.waListData = '';
            vm.bcaTableParams = '';
            //Call booking factory get all factories data
            WorkAllocationFactory
                .getWorkAllocationByBookingId(bookingId, status)
                .then(function () {
                    vm.waListData = WorkAllocationFactory.bookingDetail.DataObject;                   
                    vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.waListData.WorkAllocationList);
                });
        }

        /**
          * @name getBookingFleetHistory
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getBookingFleetHistory(fleetId, bookingId) {          
            BCAFactory
                .getBookingFleetHistory(fleetId, bookingId)
                 .then(function () {
                     vm.bookingFleetHistory = BCAFactory.bookingFleetHistoryDetail.DataObject;
                     vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.bookingFleetHistory.FleetHistory);
                     console.log(vm.bookingFleetHistory);
                 });
        }

        /**
         * @name getListing
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getListing() {
            //Call booking factory get all factories data
            vm.bookings = WorkAllocationFactory.getListing();
            vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.bookings);
        }

        /**
         * @name getPendingListing
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getStatusListing(status) {
            vm.waListData = '';
            vm.bcaTableParams = '';
            //Call booking factory get all factories data
            WorkAllocationFactory
                .getListingByStatus(status)
                .then(function () {
                    vm.waListData = WorkAllocationFactory.waStatus;                   
                    vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.waListData.data.DataList);
                });
        }

        /**
         * @name getFactoryInfo
         * @desc Get details of selected factory from factory.
         */
        function getBookingInfo() {
            //Call factories details factory get factory data
            vm.customerInfo = WorkAllocationFactory.getBookingInfo();
        }

        function addCustomerLabel() {
            //Call factories details factory get factory data
            $scope.modalInstance = $uibModal.open({
                animation: true,
                template: 'Hello wordl',
                size: 'lg',
                scope: $scope
            });
        }

        /**
         * click method of tabs
         */
        vm.tabClick = function (status) {
            vm.waListData = {};
            var statusValue = {
                "Pending": "2",
                "Allocated": "3",
                "Completed": "4"
            }
            if (typeof $stateParams.BookingId !== 'undefined') {
                // calling factory using funtion
                vm.getWorkAllocationByBookingId($stateParams.BookingId, statusValue[status]);
            }
            else {
                // calling factory using funtion           
                getStatusListing(statusValue[status]);
            }
        }

        /**
         * @name deleteBookingFleet booked fleet detailbookingCustomerDetail
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function deleteBookingFleet(bookingFleetId) {            
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call ManageJobBookingFactory factory get all factories data
                    BCAFactory
                        .deleteBookingFleet(bookingFleetId)
                        .then(function () {                           
                            $state.reload();
                        });
                }
            });
        }

        /**
        * @name bookingCustomerDetail
        * @desc  calling booking customer detail popup method and popup
        */
        function bookingCustomerDetail(data) {
            $scope.CustomerName = data.CustomerName;
            $scope.ABN = data.CustomerABN;
            $scope.EmailForInvoices = data.EmailForInvoices;
            $scope.ContactNumber = data.ContactNumber;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/BookingCustomerDetail.html',
                scope: $scope,
                controller: 'BookingListCtrl',
                controllerAs: 'blCtrl'
                //controller: ModalInstanceCtrl,
            });
        };

        /**
          * @name fleetAllocation
          * @desc Get details of selected fleet for allocation.
          */
        vm.fleetAllocation = function (booking, fleetType) {           
            if (fleetType == "new") {
                vm.fleetType = "New";
                vm.bookingId = $stateParams.BookingId;
                $scope.bookingFleetId = '';
                $scope.fleetActionFrom = "";
            } else {
                if (fleetType == "reallocation") {
                    vm.show = true;
                    vm.fromAllocation = true;
                }
                $scope.fleetActionFrom = "workAllocation";
                vm.bookingId = booking.BookingId;
                //vm.bookingFleetId = booking.BookingFleetId;
                $scope.bookingFleetId = booking.BookingFleetId;
                //vm.getBookingFleetDetail(vm.bookingFleetId);
            }

            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/JobBookingFleetDetail.html',
                size: 'lg',
                controller: ManageBookingFleetCtrl,
                controllerAs: 'mbfCtrl',
                scope: $scope
            });

        };

        /**
         * @name getBookingFleetDetail booked fleet detail
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getBookingFleetDetail(bookingFleetId) {
            //Call BCAFactory factory get all factories data
            BCAFactory
                .getBookingFleetDetail(bookingFleetId)
                .then(function () {
                    vm.bookingFleetDetail = BCAFactory.bookingFleetDetail.DataObject;
                    console.log(vm.bookingFleetDetail);
                    vm.attributes.BookingFleet.FleetTypeId = vm.bookingFleetDetail.FleetTypeId;
                    vm.attributes.BookingFleet.FleetRegistrationId = vm.bookingFleetDetail.FleetRegistrationId;
                    vm.attributes.BookingFleet.DriverId = vm.bookingFleetDetail.DriverId;
                    vm.attributes.BookingFleet.IsDayShift = vm.bookingFleetDetail.IsDayShift.toString();
                    vm.attributes.BookingFleet.Iswethire = vm.bookingFleetDetail.Iswethire.toString();
                    vm.attributes.BookingFleet.AttachmentIds = vm.bookingFleetDetail.AttachmentIds;
                    vm.attributes.BookingFleet.NotesForDrive = vm.bookingFleetDetail.NotesForDrive;
                    vm.attributes.BookingFleet.FleetBookingDateTime = new Date(vm.bookingFleetDetail.FleetBookingDateTime);
                    vm.attributes.BookingFleet.FleetBookingEndDate = new Date(vm.bookingFleetDetail.FleetBookingEndDate);
                    vm.attributes.BookingFleet.IsfleetCustomerSite = vm.bookingFleetDetail.IsfleetCustomerSite;
                    vm.attributes.BookingFleet.Reason = vm.bookingFleetDetail.Reason;                   

                });
        }

        /**
          * @name getFleetypes for dropdown
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getFleetypes() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getFleetTypesDDL()
                .then(function () {
                    vm.fleetTypeDropdownList = BCAFactory.fleetTypeList.DataList;
                });
        }

        // calling the method on page load
        vm.getFleetypes();

        /**
         * @name manageBooking for add and update
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function manageFleetBooking() {

            // checking selected checkbox
            var log = [];
            angular.forEach(vm.check, function (value, key) {
                if (value == true) {
                    this.push(key);
                }
            }, log);
            vm.attributes.BookingFleet.BookingId = vm.bookingId;
            vm.attributes.BookingFleet.AttachmentIds = log.toString();           
            // For Add new Fleet
            if ((typeof vm.bookingFleetId == 'undefined') || vm.bookingFleetId == '' || vm.bookingFleetId == '0') {
               
                BCAFactory
                       .addBookingFleet(vm.attributes)
                       .then(function () {
                           // Calling when get StateParam of BookingId

                           $state.reload();
                       });
            }
            else { // For Rellocation of the fleet
               
                vm.attributes.BookingFleet.BookingFleetId = vm.bookingFleetId;
                BCAFactory
                     .updateBookingFleet(vm.attributes)
                     .then(function () {
                         vm.tabClick("Allocated");
                         // $state.reload();
                     });
            }
        }

        /**
          * @name fleetComplete
          * @desc Change status allocation to complete
          */
        function fleetComplete(bookingFleetId) {
            vm.bookingFleetStatus.BookingFleetId = bookingFleetId;
            vm.bookingFleetStatus.StatusValue = 4;            
            BCAFactory
                .updateBookingFleetStatus(vm.bookingFleetStatus)
                .then(function () {
                    vm.tabClick("Allocated");
                    //$state.reload();
                });
        }

        /**
          * @name getAttachments for dropdown
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getAttachments() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getAttachmentsChk()
                .then(function () {
                    vm.attachementList = BCAFactory.attachementList.DataObject;
                });
        }

        // method call on Ctrl load
        vm.getAttachments();

        //Close allocation popup
        vm.cancel = function () {
            vm.bookingId = '';
            $scope.modalInstance.dismiss();
        };

        $scope.goBack = function () {
            window.history.back();
        };
        /* Call getListing method to show the factories data list */
        vm.getListing();

        /**
          * @name downloadExcelBookingList
          * @desc download pdf Booking list
          * @returns {*}
          */
        function downloadExcelBookingList(data) {
           
            var data = data;
            var innerContents = '';
            innerContents += '<table width="100%" border="1" cellpadding="0" cellspacing="0" align="center" style="border:1px solid #b5b5b5;width:100%;"><thead>';
            innerContents += '<tr><td><b>Booking Number</b></td>';
            innerContents += '<td><b>Fleet Booking Date &amp;Time</b></td>';
            innerContents += '<td><b>Fleet Number/Description</b></td>';
            innerContents += '<td><b>Customer Name</b></td>';
            innerContents += '<td><b>Driver Name</b></td>';
            innerContents += '<td><b>Site Details</b></td>';
            innerContents += '<td><b>Dockets</b></td>';
            innerContents += '<td><b>Start Date</b></td>';
            innerContents += '<td><b>End Date</b></td></tr></thead>';
            innerContents += '<tbody style="background-color:#fff;">';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    innerContents += '<tr>';
                    innerContents += '<td>' + data[i].BookingNumber + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].FleetBookingDateTime), 'dd-MM-yyyy h:mm') + '</td>';
                    innerContents += '<td>' + data[i].FleetDetail + '</td>';
                    innerContents += '<td>' + data[i].CustomerName + '</td>';
                    innerContents += '<td>' + data[i].DriverName + '</td>';
                    innerContents += '<td>' + data[i].SiteDetail + '</td>';
                    innerContents += '<td>' + data[i].Dockets + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].CallingDateTime), 'dd-MM-yyyy') + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].EndDate), 'dd-MM-yyyy') + '</td>';
                    innerContents += '</tr>';
                }
            } else {
                innerContents += '<tr><td colspan="9" class="text-center">Not records found</td></tr>';
            }
            innerContents += '</tbody></table>';
            if (window.navigator.msSaveBlob) { // IE 10+
                window.navigator.msSaveOrOpenBlob(new Blob([(innerContents)], { type: "text/richtext;charset=utf-8;" }), "WorkAllocation-List.xls")
            }
            else {
                var uri = 'data:application/vnd.ms-word,' + encodeURIComponent(innerContents);                
                var downloadLink = document.createElement("a");
                downloadLink.href = uri;
                downloadLink.download = "WorkAllocation-List.xls";
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            }
        }

        /**
         * @name downloadPrintBookingList
         * @desc print Booking list
         * @returns {*}
         */
        function downloadPrintBookingList(data) {
            var data = data;
            var innerContents = '';
            innerContents += '<table width="100%" border="1" cellpadding="0" cellspacing="0" align="center" style="border:1px solid #b5b5b5;width:100%;"><thead>';
            innerContents += '<tr><td><b>Booking Number</b></td>';
            innerContents += '<td><b>Fleet Booking Date &amp;Time</b></td>';
            innerContents += '<td><b>Fleet Number/Description</b></td>';
            innerContents += '<td><b>Customer Name</b></td>';
            innerContents += '<td><b>Driver Name</b></td>';
            innerContents += '<td><b>Site Details</b></td>';
            innerContents += '<td><b>Dockets</b></td>';
            innerContents += '<td><b>Start Date</b></td>';
            innerContents += '<td><b>End Date</b></td></tr></thead>';
            innerContents += '<tbody style="background-color:#fff;">';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    innerContents += '<tr>';
                    innerContents += '<td>' + data[i].BookingNumber + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].FleetBookingDateTime), 'dd-MM-yyyy h:mm') + '</td>';
                    innerContents += '<td>' + data[i].FleetDetail + '</td>';
                    innerContents += '<td>' + data[i].CustomerName + '</td>';
                    innerContents += '<td>' + data[i].DriverName + '</td>';
                    innerContents += '<td>' + data[i].SiteDetail + '</td>';
                    innerContents += '<td>' + data[i].Dockets + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].CallingDateTime), 'dd-MM-yyyy') + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].EndDate), 'dd-MM-yyyy') + '</td>';
                    innerContents += '</tr>';
                }
            } else {
                innerContents += '<tr><td colspan="9" class="text-center">Not records found</td></tr>';
            }
            innerContents += '</tbody></table>';
            var popupWindow = window.open('', '_blank', 'width=600,height=700,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWindow.document.open();
            popupWindow.document.write('<html><body onload="window.print()">' + innerContents + '</html>');
            popupWindow.document.close();
        }

        /**
          * @name savePDF
          * @desc click function for creating PDF
          * @returns {*}
          */
        vm.savePDF = function (printdata) {

            //PDF PRINTER
            var columns = [
                "Booking Number",
                "Fleet Booking Date &amp;Time",
                "Customer Name",
                "Site Details",
                "Start Date",
                "End Date"
            ];

            var rows = [];

            printdata.forEach(function (item) {
                rows.push([
                    item.BookingNumber,
                    $filter('date')(new Date(item.FleetBookingDateTime), 'dd-MM-yyyy h:mm'),
                    item.CustomerName,
                    item.SiteDetail,
                    $filter('date')(new Date(item.CallingDateTime), 'dd-MM-yyyy'),
                    $filter('date')(new Date(item.EndDate), 'dd-MM-yyyy')
                ])
            });

            var doc = new jsPDF('l', 'pt');

            function processPDF(doc) {
                var header = "Work Allocation List" + '\n';

                doc.setFontSize(14);
                doc.text(header, 40, 40);

                var altInfo = "";

                doc.setFontSize(10);
                doc.text(altInfo, 40, 60);

                var totalPagesExp = "{total_pages_count_string}";

                var pageContent = function (data) {

                    if (data.pageCount != 1) {
                        // HEADER
                        doc.setFontSize(8);
                        doc.setTextColor(40);
                        doc.setFontStyle('normal');

                        doc.text('full name' + ' (' + '1234' + ') ' +
                            "Payroll Date: " + "", data.settings.margin.left + 10, 22);
                    }
                    // FOOTER
                    var str = "Page " + data.pageCount;
                    // Total page number plugin only available in jspdf v1.0+
                    if (typeof doc.putTotalPages === 'function') {
                        str = str + " of " + totalPagesExp;
                    }
                    doc.setFontSize(10);
                    doc.text(str, data.settings.margin.left, doc.internal.pageSize.height - 10);
                };


                doc.autoTable(columns, rows, {
                    headerStyles: {
                        fillColor: [218, 236, 243],
                        textColor: [6, 6, 6]
                    },
                    startY: 70,
                    styles: {
                        overflow: 'linebreak',
                        columnWidth: 'wrap'
                    },
                    addPageContent: pageContent,
                    columnStyles: {
                        0: { columnWidth: 100 },
                        1: { columnWidth: 100 },
                        2: { columnWidth: 100 },
                        3: { columnWidth: 100 },
                        4: { columnWidth: 100 },
                        5: { columnWidth: 100 },
                        6: { columnWidth: 100 },
                        7: { columnWidth: 100 }

                    }
                    //theme: 'plain'
                });

                // Total page number plugin only available in jspdf v1.0+
                if (typeof doc.putTotalPages === 'function') {
                    doc.putTotalPages(totalPagesExp);
                }

                return doc;
            }

            doc = processPDF(doc);
            // doc.addPage();
            // doc = processPDF(doc);

            var filename = "WorkAllocationList";
            doc.save(filename.replace(/\s/g, '') + '.pdf');
        };

        /**
       * @name downloadPdfBookingList
       * @desc download pdf Booking list
       * @returns {*}
       */
        function downloadPdfBookingList() {
            var data = vm.waListData.data;
            var innerContents = '';
            innerContents += '<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" style="border:1px solid #b5b5b5;width:100%;"><thead>';
            innerContents += '<tr><td><b>Booking Number</b></td>';
            innerContents += '<td><b>Fleet Booking Date &amp;Time</b></td>';
            innerContents += '<td><b>Fleet Number/Description</b></td>';
            innerContents += '<td><b>Customer Name</b></td>';
            innerContents += '<td><b>Driver Name</b></td>';
            innerContents += '<td><b>Site Details</b></td>';
            innerContents += '<td><b>Dockets</b></td>';
            innerContents += '<td><b>Start Date</b></td>';
            innerContents += '<td><b>End Date</b></td></tr></thead>';
            innerContents += '<tbody style="background-color:#fff;">';
            if (data.DataList.length > 0) {
                for (var i = 0; i < data.DataList.length; i++) {
                    innerContents += '<tr>';
                    innerContents += '<td>' + data.DataList[i].BookingNumber + '</td>';
                    innerContents += '<td>' + data.DataList[i].FleetBookingDateTime + '</td>';
                    innerContents += '<td>' + data.DataList[i].FleetDetail + '</td>';
                    innerContents += '<td>' + data.DataList[i].CustomerName + '</td>';
                    innerContents += '<td>' + data.DataList[i].DriverName + '</td>';
                    innerContents += '<td>' + data.DataList[i].SiteDetail + '</td>';
                    innerContents += '<td>' + data.DataList[i].Dockets + '</td>';
                    innerContents += '<td>' + data.DataList[i].CallingDateTime + '</td>';
                    innerContents += '<td>' + data.DataList[i].EndDate + '</td>';
                    innerContents += '</tr>';
                }
            } else {
                innerContents += '<tr><td colspan="9" class="text-center">Not records found</td></tr>';
            }
            innerContents += '</tbody></table>';
            if (window.navigator.msSaveBlob) { // IE 10+
                window.navigator.msSaveOrOpenBlob(new Blob([(innerContents)], { type: "text/richtext;charset=utf-8;" }), "pdfFile.pdf")
            }
            else {
                var uri = 'data:application/pdf,' + encodeURIComponent(innerContents);
                var downloadLink = document.createElement("a");
                downloadLink.href = uri;
                downloadLink.download = "pdfFile.pdf";
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            }
        }
    }

    //Inject required stuff as parameters to factories controller function
    ManageJobBookingCtrl.$inject = ['$scope', 'UtilsFactory', 'BCAFactory', 'ManageJobBookingFactory', '$location', '$stateParams', '$uibModal', '$state', '$filter'];

    /**
   * @name ManageFactoryCtrl
   * @param $scope
   * @param UtilsFactory
   * @param BCAFactory
   * @param ManageJobBookingFactory
   * @param $location
   * @param $state
   * @param $filter
   */
    function ManageJobBookingCtrl($scope, UtilsFactory, BCAFactory, ManageJobBookingFactory, $location, $stateParams, $uibModal, $state, $filter) {

        var vm = this;
        vm.updateDetails = updateDetails;
        vm.getCustomers = getCustomers;
        vm.getSites = getSites;
        vm.getWorkTypes = getWorkTypes;
        vm.manageBooking = manageBooking;
        vm.getBookingByBookingId = getBookingByBookingId;
        vm.deleteBookingFleet = deleteBookingFleet;
        vm.reset = reset;

        // map create job booking property
        vm.attributes = {
            BookingId: '',
            CustomerId: '',
            SiteId: '',
            WorktypeId: '',
            StatusLookupId: '62E9A544-F295-4110-97F2-03A26E01ADE8',
            CallingDateTime: new Date(),
            FleetBookingDateTime: '',
            EndDate: '',
            BookingNumber: '',
            AllocationNotes: '',
            IsActive: 'true',
            BookingFleet: {
                BookingFleetId: '',
                BookingId: '',
                BookingNumber: '',
                RegistrationNumber: '',
                Driver: '',
                FleetBookingDateTime: new Date(),
                FleetBookingEndDate: '',
            },
        };
        vm.endDate = new Date();

        /**
        * @name getCustomerList for dropdown
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getCustomers() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getCustomersDDL()
                .then(function () {
                    vm.customerDropdownList = BCAFactory.customerList.DataList;
                });
        }

        // calling method on load
        vm.getCustomers();

        /**
       * @name getSites for dropdown
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function getSites() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getSitesByCustomerIdDDL(vm.attributes.CustomerId)
                .then(function () {
                    vm.siteDropdownList = BCAFactory.siteList.DataList;
                });
        }

        // method call on change of customer dropdown
        vm.customerChange = function () {
            //calling funtion of getSiteDropDownListing
            getSites();
        }

        /**
     * @name getWorkTypes for dropdown
     * @desc Retrieve factories listing from factory
     * @returns {*}
     */
        function getWorkTypes() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getWorkTypesDDL()
                .then(function () {
                    vm.workTypeDropdownList = BCAFactory.workTypeList.DataList;
                });
        }

        // calling the method on page load
        vm.getWorkTypes();

        /**
       * @name manageBooking for add and update
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function manageBooking() {
                        
            //var a = new Date(vm.attributes.CallingDateTime);
            //console.log(a)
            //console.log( $filter('date')(new Date(vm.attributes.CallingDateTime), 'yyyy-MM-dd'))

            ManageJobBookingFactory
                   .addBooking(vm.attributes)
                   .then(function () {
                       $location.path('booking/BookingList');                      
                   });
        }

        /**
     * @name getBookingByBookingId for edit
     * @desc Retrieve factories listing from factory
     * @returns {*}
     */
        function getBookingByBookingId(bookingId) {

            //Call BCAFactory factory get all factories data
            ManageJobBookingFactory
                .getBookingDetail(bookingId)
                .then(function () {
                    vm.bookingDetail = ManageJobBookingFactory.bookingDetail.DataObject;
                    vm.attributes.CallingDateTime = vm.bookingDetail.CallingDateTime;
                    vm.attributes.FleetBookingDateTime = new Date(vm.bookingDetail.FleetBookingDateTime);
                    vm.attributes.EndDate = new Date(vm.bookingDetail.EndDate);
                    vm.attributes.CustomerId = vm.bookingDetail.CustomerId;
                    vm.attributes.BookingNumber = vm.bookingDetail.BookingNumber;
                    vm.customerChange();
                    vm.attributes.SiteId = vm.bookingDetail.SiteId;
                    vm.attributes.WorktypeId = vm.bookingDetail.WorktypeId;
                    vm.attributes.AllocationNotes = vm.bookingDetail.AllocationNotes;
                    vm.attributes.BookingFleet = vm.bookingDetail.BookedFleetList;                   

                });
        }

        // showing and hiding fleet grid table on basis on ID
        if ($stateParams.BookingId == 0) {
            vm.showFleetTable = false;
        }
        else {
            vm.showFleetTable = true;
            vm.getBookingByBookingId($stateParams.BookingId);

        }

        vm.addFleet = function (bookingFleetId) {
            $scope.bookingFleetId = bookingFleetId;
            $scope.fleetActionFrom = "";
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/JobBookingFleetDetail.html',
                size: 'lg',
                controller: ManageBookingFleetCtrl,
                controllerAs: 'mbfCtrl',
                scope: $scope
            });
        };

        vm.cancel = function () {
            $scope.modalInstance.dismiss();
        };

        /**
       * @name deleteBookingFleet booked fleet detail
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function deleteBookingFleet(bookingFleetId) {
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call ManageJobBookingFactory factory get all factories data
                    ManageJobBookingFactory
                        .deleteBookingFleet(bookingFleetId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });
        }

        // Reset Function 
        var originalAttributes = angular.copy(vm.attributes);
        function reset() {
            vm.attributes = angular.copy(originalAttributes);
        };

        //Form validation messages
        vm.validationMsg = {
            required: 'Required',
            invalid: 'Invalid Input',
            email: 'Invalid Email'
        }

        function updateDetails(isValid) {
            if (isValid) {
                //$location.path('/WorkAllocation')
                alert('yes, its working!!!')
            }
        }

        //Customer Name Select Listing
        $scope.Customername = [
            "2010 Debtors",
            "Absolute Civil Australia Pty Ltd",
            "Active Earthworks",
            "Adduso Concrete and Pumping",
            "Adduso Holdings Pty Ltd"
        ];

        //Site Location Select Listing
        $scope.WorkTypename = [
            "Hourly",
            "Contract",
            "Tonnage"
        ];

        // set timeout for applying i-check class 
        setTimeout(function () {
            try {
                $(".i-checks").iCheck({
                    checkboxClass: 'icheckbox_square-green',
                    radioClass: 'iradio_square-green',
                    increaseArea: '80%' // optional
                });
            }
            catch (ex) { }
        }, 2000);

    }

    //Inject required stuff as parameters to factories controller function
    ManageBookingFleetCtrl.$inject = ['$scope', 'UtilsFactory', 'BCAFactory', 'ManageJobBookingFactory', '$location', '$stateParams', '$uibModal', '$state', '$filter'];

    /**
        * @name ManageBookingFleetCtrl
        * @param $scope
        * @param UtilsFactory
        * @param BCAFactory
        * @param ManageJobBookingFactory
        * @param $location
        * @param $stateParams
        * @param $uibModal
        * @param $state
        */
    function ManageBookingFleetCtrl($scope, UtilsFactory, BCAFactory, ManageJobBookingFactory, $location, $stateParams, $uibModal, $state, $filter) {

        var vm = this;

        // setting default date
        vm.minDate = new Date();

        vm.getFleetypes = getFleetypes;
        vm.getFleetRegistrations = getFleetRegistrations;
        vm.getDrivers = getDrivers;
        vm.getAttachments = getAttachments;
        vm.manageFleetBooking = manageFleetBooking;
        vm.getBookingFleetDetail = getBookingFleetDetail;
        vm.hideCancel = hideCancel;

        // map create job booking property
        vm.check = {};
        vm.radioValue = {};
        vm.isDay = '';
        vm.attributes = {

            BookingFleet: {
                BookingFleetId: '',
                BookingId: '',
                FleetTypeId: '',
                FleetRegistrationId: '',
                DriverId: '',
                IsDayShift: true,
                Iswethire: true,
                AttachmentIds: '',
                NotesForDrive: '',
                IsfleetCustomerSite: false,
                Reason: '',
                StatusLookupId: '',
                BookingNumber: '',
                AllocationNotes: '',
                CreatedDate: '',
                EndDate: '',
                FleetBookingDateTime: new Date(),
                FleetBookingEndDate: '',
                CallingDateTime: '',
                IsActive: true,
                FleetName: ''
            },
        };
        vm.bookingFleetId = $scope.bookingFleetId;
        vm.fleetActionFrom = $scope.fleetActionFrom;
        vm.bookingFleetDetail = '';       

        /**
      * @name getFleetypes for dropdown
      * @desc Retrieve factories listing from factory
      * @returns {*}
      */
        function getFleetypes() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getFleetTypesDDL()
                .then(function () {
                    vm.fleetTypeDropdownList = BCAFactory.fleetTypeList.DataList;
                });
        }
        // calling the method on page load
        vm.getFleetypes();

        /**
       * @name getFleetRegistrations for dropdown
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function getFleetRegistrations() {
            //Call BCAFactory factory get all factories data
            BCAFactory
                .getFleetRegistrationsByFleetTypeIdDDL(vm.attributes.BookingFleet.FleetTypeId)
                .then(function () {
                    vm.fleetRegistrationDropdownList = BCAFactory.fleetRegistrationList.DataList;                  
                    if (typeof vm.bookingFleetDetail.BookedFleetDetail[0] !== 'undefined' && vm.bookingFleetDetail.BookedFleetDetail[0] !== '') {
                        vm.fleetRegistrationDropdownList.push(vm.bookingFleetDetail.BookedFleetDetail[0]);
                    }                   
                });
        }

        // method call on change of fleet dropdown
        vm.fleetChange = function () {
            //calling funtion of getFleetRegistrationDropDownListing
            getFleetRegistrations();
        }

        /**
      * @name getDrivers for dropdown
      * @desc Retrieve factories listing from factory
      * @returns {*}
      */
        function getDrivers() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getDriversByFleetRegistrationIdDDL(vm.attributes.BookingFleet.FleetRegistrationId)
                .then(function () {
                    vm.driverDropdownList = BCAFactory.driverList.DataList;
                });
        }

        // method call on change of fleet registration dropdown
        vm.fleetRegistrationChange = function () {
            //calling funtion of getDrivers
            getDrivers();
        }

        /**
        * @name getAttachments for check list
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getAttachments() {

            //Call BCAFactory factory get all factories data
            BCAFactory
                .getAttachmentsChk()
                .then(function () {
                    vm.attachementList = BCAFactory.attachementList.DataObject;
                });
        }

        // method call on Ctrl load
        vm.getAttachments();

        /**
       * @name manageBooking for add and update
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function manageFleetBooking() {

            // checking selected checkbox
            var log = [];
            angular.forEach(vm.check, function (value, key) {
                if (key !== '' && value === true) {                    
                    this.push(key);
                }
            }, log);

            if (typeof $stateParams.BookingId !== 'undefined') {               
                vm.attributes.BookingFleet.BookingId = $stateParams.BookingId;
            }
            vm.attributes.BookingFleet.AttachmentIds = log.toString();

            if (typeof vm.bookingFleetId == 'undefined' || vm.bookingFleetId == '0' || vm.bookingFleetId == '') {
                ManageJobBookingFactory
                       .addBookingFleet(vm.attributes)
                       .then(function () {
                           $state.reload();
                       });
            }
            else {
                vm.attributes.BookingFleet.BookingFleetId = vm.bookingFleetId;               
                ManageJobBookingFactory
                     .updateBookingFleet(vm.attributes)
                     .then(function () {
                         $state.reload();
                     });
            }
        }

        /**
       * @name getBookingFleetDetail booked fleet detail
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function getBookingFleetDetail() {
            //vm.bookingFleetId = bookingFleetId;
            if (vm.bookingFleetId != undefined) {
                if (vm.bookingFleetId == 0)
                    vm.show = false;
                else
                    vm.show = true;
            }

            //Call BCAFactory factory get all factories data
            ManageJobBookingFactory
                .getBookingFleetDetail(vm.bookingFleetId)
                .then(function () {
                    vm.bookingFleetDetail = ManageJobBookingFactory.bookingFleetDetail.DataObject;                    
                    vm.attributes.BookingFleet.BookingId = vm.bookingFleetDetail.BookingId;
                    vm.attributes.BookingFleet.FleetTypeId = vm.bookingFleetDetail.FleetTypeId;
                    vm.attributes.BookingFleet.FleetRegistrationId = vm.bookingFleetDetail.FleetRegistrationId;
                    vm.attributes.BookingFleet.DriverId = vm.bookingFleetDetail.DriverId;
                    vm.attributes.BookingFleet.IsDayShift = vm.bookingFleetDetail.IsDayShift.toString();
                    vm.attributes.BookingFleet.Iswethire = vm.bookingFleetDetail.Iswethire.toString();
                    vm.attributes.BookingFleet.AttachmentIds = vm.bookingFleetDetail.AttachmentIds;
                    vm.attributes.BookingFleet.NotesForDrive = vm.bookingFleetDetail.NotesForDrive;
                    vm.attributes.BookingFleet.FleetBookingDateTime = vm.bookingFleetDetail.FleetBookingDateTime;
                    vm.attributes.BookingFleet.FleetBookingEndDate = new Date(vm.bookingFleetDetail.FleetBookingEndDate);
                    vm.attributes.BookingFleet.IsfleetCustomerSite = vm.bookingFleetDetail.IsfleetCustomerSite;
                    vm.attributes.BookingFleet.Reason = vm.bookingFleetDetail.Reason;
                    isChecked(vm.bookingFleetDetail.AttachmentIds);
                    vm.fleetChange();
                    vm.fleetRegistrationChange();
                    console.log("vm.bookingFleetDetail", vm.bookingFleetDetail)
                    // calling method to check the selected checkbox


                });
        }

        vm.getBookingFleetDetail(vm.bookingFleetId);

        // calling on page load
        //vm.bookingFleetInit = function (bookingFleetId) {
        //    vm.bookingFleetId = bookingFleetId;
        //    if (bookingFleetId != undefined) {
        //        vm.getBookingFleetDetail(bookingFleetId);
        //        if (vm.bookingFleetId == 0)
        //            vm.show = false;
        //        else
        //            vm.show = true;
        //    }
        //}
        function hideCancel() {
            $scope.modalInstance.dismiss();
        }

        // checking checkboxes by Id while updating
        function isChecked(attachmentIds) {
            vm.check = {};
            var array = attachmentIds.split(',');            
            if (array.length > 0) {
                angular.forEach(array, function (items) {
                    vm.check[items] = true;                   
                });
            }
        }

        // change event of FleetBookingDateTime
        vm.setMinEndDate = function () {

            var splitedDate = vm.attributes.BookingFleet.FleetBookingDateTime;
            var newSplited = splitedDate.split("-");
            var a = new Date(newSplited[1] + "/" + newSplited[0] + "/" + newSplited[2]);
            vm.minEndDate = a.toUTCString();
        }
    }

    //Inject required stuff as parameters to factories controller function
    ManageBookingSiteCtrl.$inject = ['$scope', 'UtilsFactory', 'BCAFactory', 'ManageBookingSiteFactory', '$location', '$stateParams', '$state', '$uibModal'];

    /**
    * @name ManageBookingSiteCtrl
    * @param $scope
    * @param UtilsFactory
    * @param BCAFactory
    * @param ManageJobBookingFactory
    * @param $location
    * @param $stateParams
    * @param $state
    * @param $uibModal    
     */
    function ManageBookingSiteCtrl($scope, UtilsFactory, BCAFactory, ManageBookingSiteFactory, $location, $stateParams, $state, $uibModal) {

        var vm = this;
        vm.getSiteDetail = getSiteDetail;
        vm.manageBookingSite = manageBookingSite;
        vm.getContactDetail = getContactDetail;
        vm.bookingContactDetailModal = bookingContactDetailModal;
        // vm.manageContactPerson = manageContactPerson;
        vm.saveContact = saveContact;

        // map create booking site property
        vm.attributes = {
            BookingId: '',
            CustomerName: '',
            SiteName: '',
            SiteDetail: '',
            SupervisorId: '',
            GateId: '',
            SupervisorName: '',
            SupervisorEmail: '',
            SupervisorMobileNumber: '',
            GateContactPersonId: '',
            SupervisorId: '',
            SiteNote: '',
            IsActive: true
        };
        vm.SiteNote = '';

        // map Create/Update contact site property
        vm.contact = {
            GateContactPersonId: '',
            ContactPerson: '',
            MobileNumber: '',
            Email: '',
            GateId: '',
            IsDefault: '',
            IsActive: true
        };

        // map create booking site(gate dropdown) property
        vm.gate = {
            GateNumber: '',
            RegistrationDescription: '',
            ContactPerson: '',
            Email: '',
            MobileNumber: ''
        };

        // map create booking site(fleet dropdown) property
        vm.fleet = {
            FleetRegistrationId: '',
            RegistrationDescription: '',
        };

        // array's for locally managing adding supervisor and gate
        vm.addSupervisor = [];
        vm.superVisorId = [];
        vm.addGate = [];
        vm.gateId = [];
        vm.fleetRegistrationId = [];
        vm.gateContactPersonId = [];
        vm.contactModalGetList = [];

        /**
      * @name getSiteDetail 
      * @desc Retrieve factories listing from factory
      * @returns {*}
      */
        function getSiteDetail(bookingId) {

            //Call ManageBookingSiteFactory factory get all factories data
            ManageBookingSiteFactory
                .getBookingSiteDetail(bookingId)
                .then(function () {
                    vm.siteDetail = ManageBookingSiteFactory.siteDetail.DataObject;
                    vm.addSupervisor = vm.siteDetail.AllocatedSupervisor;
                    vm.superVisor = vm.siteDetail.SupervisorList;
                    vm.fleetList = vm.siteDetail.FleetList;
                    vm.gateList = vm.siteDetail.GateList;                                      
                    vm.SiteName = vm.siteDetail.SiteName;
                    vm.CustomerName = vm.siteDetail.CustomerName;
                    vm.SiteDetail = vm.siteDetail.SiteDetail;
                    vm.addGate = vm.siteDetail.AllocatedGateContactList;
                    vm.SiteNote = vm.siteDetail.SiteNote;                   
                    for (var i = 0; i < vm.siteDetail.AllocatedGateContactList.length; ++i) {
                        vm.gateId[i] = vm.siteDetail.AllocatedGateContactList[i].GateId;
                        vm.fleetRegistrationId[i] = vm.siteDetail.AllocatedGateContactList[i].FleetRegistrationId;
                        vm.gateContactPersonId[i] = vm.siteDetail.AllocatedGateContactList[i].GateContactPersonId;

                    }
                    for (var i = 0; i < vm.siteDetail.AllocatedSupervisor.length; ++i) {
                        vm.superVisorId[i] = vm.siteDetail.AllocatedSupervisor[i].SupervisorId;
                    }
                   
                });

        }

        // calling the method on page load
        vm.getSiteDetail($stateParams.BookingId);

        /**
          * @name getContactDetail
          * @desc Retrieve contact list from the factory
          * @return {*}
          */
        function getContactDetail(gateId) {
            if (!vm.gate) {
                vm.gate = {};
            }
            if (gateId) {
                vm.gate.contact = {};
                ManageBookingSiteFactory
               .getContactPersons(gateId)
               .then(function () {
                   vm.contactList = ManageBookingSiteFactory.contactDetail.DataList;
                   vm.gate.contact = vm.contactList.filter(function (c) { return c.IsDefault === true })[0];

               });
            } else {
                vm.contactList = {};
            }
        }

        /**
          * @name bookingContactDetailModal
          * @desc calling popup
          */
        function bookingContactDetailModal(contactId, getList) {
            vm.contact.GateContactPersonId = '';
            vm.contact.ContactPerson = '';
            vm.contact.MobileNumber = '';
            vm.contact.Email = '';
            vm.contact.IsDefault = '';
            vm.contact.IsActive = '';
            vm.contactModalGetList = {};

            if (contactId) {
                var filter = $(vm.contactList)
                           .filter(function (index, element) {
                               return element.GateContactPersonId == contactId;
                           });
                vm.contact.GateContactPersonId = filter[0].GateContactPersonId;
                vm.contact.ContactPerson = filter[0].ContactPerson;
                vm.contact.MobileNumber = parseInt(filter[0].MobileNumber);
                vm.contact.Email = filter[0].Email;
                vm.contact.IsDefault = filter[0].IsDefault;
                vm.contact.IsActive = filter[0].IsActive;
                vm.contact.gate = vm.gate;
            }
            vm.contactModalGetList = getList;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/bookingContactDetail.html',
                scope: $scope
                //controller: ModalInstanceCtrl,
            });
        };

        // hide modal
        vm.hideContact = function () {
            $scope.modalInstance.dismiss();
        }

        // hide cancel popup
        vm.hideCancel = function () {
            $scope.modalInstance.dismiss();
        }

        /**
        * @name saveContact
        * @desc Add/Update contact detail
        */
        function saveContact() {
          
            // store data for api call 
            var contactAttribute = {
                GateId: vm.contact.gate.GateId,
                ContactPerson: vm.contact.ContactPerson,
                MobileNumber: vm.contact.MobileNumber,
                Email: vm.contact.Email,
                IsDefault: vm.contact.IsDefault,
                IsActive: vm.contact.IsActive
            }

            vm.contactModalGetList = {};
            if (vm.contact.GateContactPersonId) {
                contactAttribute.GateContactPersonId = vm.contact.GateContactPersonId;
                ManageBookingSiteFactory
                 .updateGateContactPerson(contactAttribute)
                 .then(function () {                     
                 });
            } else {
                ManageBookingSiteFactory
                .addGateContactPerson(contactAttribute)
                .then(function () {                   
                });
            }
            vm.getContactDetail(vm.contact.gate.GateId);
            vm.hideContact();
        }

        /**
         * @name manageBookingSite for add and update
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function manageBookingSite() {
            vm.attributes.SupervisorId = vm.superVisorId.toString();
            vm.attributes.BookingId = $stateParams.BookingId;
            vm.attributes.GateId = vm.gateId.toString();
            vm.attributes.FleetRegistrationId = vm.fleetRegistrationId.toString();
            vm.attributes.GateContactPersonId = vm.gateContactPersonId.toString();
            vm.attributes.SiteNote = vm.SiteNote;
            console.log(vm.attributes);           
            ManageBookingSiteFactory
                   .addBookingSiteDetail(vm.attributes)
                   .then(function () {
                       $state.reload();
                   });
        }

        // start supervisor list code add and remove code
        vm.addRow = function () {
            var bl = false;
            if (vm.attributes !== null) {                
                $.grep(vm.addSupervisor, function (n, i) {
                    if (n["SupervisorName"] == vm.attributes.SupervisorName)
                        bl = true;
                });
           
                if ( vm.attributes.SupervisorName !== "" && typeof vm.attributes.SupervisorName !== "undefined" ) {
                    // Apply check for existance
                    if (!bl) {
                        // adding supervisorId in array
                        vm.superVisorId.push(vm.attributes.SupervisorId);
                        vm.addSupervisor.push({
                            'Options': vm.attributes.Options,
                            'SupervisorId': vm.attributes.SupervisorId,
                            'SupervisorName': vm.attributes.SupervisorName,
                            'SupervisorEmail': vm.attributes.SupervisorEmail,
                            'SupervisorMobileNumber': vm.attributes.SupervisorMobileNumber
                        });
                    }
                }
            }
        }

        vm.removeRow = function (index) {
            //var index = -1;
            //var comArr = eval(vm.addSupervisor);
            //for (var i = 0; i < comArr.length; i++) {
            //    if (comArr[i].name === name) {
            //        index = i;
            //        break;
            //    }
            //}
            //if (index === -1) {
            //    alert("Something gone wrong");
            //}
            vm.addSupervisor.splice(index, 1);
            vm.superVisorId.splice(index, 1);
        };
        // end supervisor list code add and remove code

        // start gate list code add and remove code
        vm.addGateRow = function () {
            var bl = false;
            if (vm.fleet.RegistrationDescription && vm.gate.contact) {
                $.grep(vm.addGate, function (n, i) {
                    if ((n["GateNumber"] == vm.gate.GateNumber) && (n["FleetRegistrationId"] == vm.fleet.FleetRegistrationId) || (n["ContactPerson"] == vm.gate.contact.ContactPerson))
                        bl = true;
                });
                if (vm.gate.GateNumber != "") {
                    // Apply check for existance
                    if (!bl) {
                        // adding GateId in array                       
                        vm.gateId.push(vm.gate.GateId);
                        vm.fleetRegistrationId.push(vm.fleet.FleetRegistrationId);
                        vm.gateContactPersonId.push(vm.gate.contact.GateContactPersonId);
                        vm.addGate.push({
                            'GateNumber': vm.gate.GateNumber,
                            'Registration': vm.fleet.RegistrationDescription,
                            'FleetRegistrationId': vm.fleet.FleetRegistrationId,
                            'ContactPerson': vm.gate.contact.ContactPerson,
                            'Email': vm.gate.contact.Email,
                            'MobileNumber': vm.gate.contact.MobileNumber
                        });
                    }
                }
            }
        }

        vm.removeGateRow = function (index) {           
            vm.addGate.splice(index, 1);
            vm.gateId.splice(index, 1);
            vm.fleetRegistrationId.splice(index, 1);
            vm.gateContactPersonId.splice(index, 1);
        }
        // end gate list code add and remove code
    }

    //Inject required stuff as parameters to factories controller function
    BookingListCtrl.$inject = ['$scope', '$uibModal', 'BookingListFactory', 'UtilsFactory', '$filter', '$state'];

    /**
    * @name BookingListCtrl
    * @param $scope
    * @param $uibModal 
    * @param BookingListFactory 
    * @param UtilsFactory
    * @param $filter
    * @param $state
    */
    function BookingListCtrl($scope, $uibModal, BookingListFactory, UtilsFactory, $filter, $state) {
        //Assign controller scope pointer to a variable
        var vm = this;

        // map create job booking property
        vm.attributes = {
            BookingId: '',
            FromDate: '',
            ToDate: '',
            CustomerId: '',
            CustomerName: '',
            SiteId: '',
            WorktypeId: '',
            StatusLookupId: '62E9A544-F295-4110-97F2-03A26E01ADE8',
            CallingDateTime: '',
            FleetBookingDateTime: '',
            EndDate: '',
            BookingNumber: '',
            AllocationNotes: '',
            ABN: '',
            EmailForInvoices: '',
            ContactNumber: '',
            BookingFleet: {
                FleetTypeId: '',
                FleetRegistrationId: '',
                DriverId: '',
            },
            BookingFleet1 : [],
        }
        vm.status = {
            BookingId: '',
            StatusValue: '',
            CancelNote: '',
            Rate: ''
        };

        //Methods
        vm.getBookingList = getBookingList;
        vm.getListing = getListing;
        vm.getBookingInfo = getBookingInfo;
        vm.bookingCustomerDetail = bookingCustomerDetail;
        vm.bookFleetDetail = bookFleetDetail;
        vm.deleteBooking = deleteBooking;
        vm.updateBookingStatus = updateBookingStatus;
        vm.closeModel = closeModel;
        vm.downloadPrintBookingList = downloadPrintBookingList;
        vm.downloadExcelBookingList = downloadExcelBookingList;

        /**
       * @name getAllBooking for booking list
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function getBookingList() {

            // declaring variable
            var fromDate;
            var toDate;

            // checking condition of from and to
            if (vm.attributes.FromDate == "" && vm.attributes.ToDate == "") {
                fromDate = "0";
                toDate = "0";
                //vm.attributes.ToDate = $filter('date')(new Date(), 'yyyy-MM-dd');
            }
            else {
                var tempFromDate = vm.attributes.FromDate;
                var tempToDate = vm.attributes.ToDate;
                fromDate = $filter('date')(tempFromDate, 'yyyy-MM-dd');
                toDate = $filter('date')(tempToDate, 'yyyy-MM-dd');
            }


            //Call BookingListFactory factory get all factories data
            BookingListFactory
                .getAllBookingByDateRange(fromDate, toDate)
                .then(function () {
                    vm.bookingList = BookingListFactory.bookingList.DataList;
                    vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.bookingList);
                });
        }
        // calling method on load
        vm.getBookingList();

        /**
         * @name getListing
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getListing() {
            //Call booking factory get all factories data
            vm.bookings = BookingsListDetailsFactory.getListing();
            vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.bookings);
        }

        /**
         * @name getFactoryInfo
         * @desc Get details of selected factory from factory.
         */
        function getBookingInfo() {
            //Call factories details factory get factory data
            vm.customerInfo = BookingsListDetailsFactory.getBookingInfo();
        }

        /**
         * @name bookingCustomerDetail
         * @desc callin popup
         */
        function bookingCustomerDetail(data) {
            $scope.CustomerName = data.CustomerName;
            $scope.ABN = data.CustomerABN;
            $scope.EmailForInvoices = data.EmailForInvoices;
            $scope.ContactNumber = data.ContactNumber;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/BookingCustomerDetail.html',
                scope: $scope,
                controller: 'BookingListCtrl',
                controllerAs: 'blCtrl'
                //controller: ModalInstanceCtrl,
            });
        };

        // hide modal
        vm.hideCustomer = function () {
            $scope.modalInstance.dismiss();
        }

        //CancelBookingModal
        vm.cancelStatusPopup = function (bookingId) {
            vm.bookingIdForCancel = bookingId;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/CancelBooking.html',
                //controller: ModalInstanceCtrl,
                scope: $scope
            });
        }

        // hide cancel popup
        vm.hideCancel = function () {
            $scope.modalInstance.dismiss();
        }

        /**
       * @name customerDetail
       * @desc calling booking customer detail popup method
       */
        vm.customerDetail = function (data) {
            vm.attributes.CustomerName = data.CustomerName;
            vm.attributes.ABN = data.CustomerABN;
            vm.attributes.EmailForInvoices = data.EmailForInvoices;
            vm.attributes.ContactNumber = data.ContactNumber;
            vm.bookingCustomerDetail();           
        }

        /**
        * @name bookFleetDetail
        * @desc callin popup
        */
        function bookFleetDetail() {
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/booking/partial/AllocatedFleetList.html',
                size: 'lg',
                scope: $scope
                //controller: ModalInstanceCtrl,
            });
        };

        // hide AllocatedFleet popup
        vm.hideFleet = function () {
            $scope.modalInstance.dismiss();
        }

        /**
        * @name fleetDetail
        * @desc calling booking fleet detail popup method
        */
        vm.fleetDetail = function (data) {
            vm.fleetList = data;
            vm.bcaFleetList = UtilsFactory.bcaTableOptions(vm.fleetList);           
            vm.bookFleetDetail();
        }

        /**
        * @name deleteBooking by bookingId
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function deleteBooking(bookingId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call BookingListFactory factory get all factories data
                    BookingListFactory
                    .deleteBooking(bookingId)
                    .then(function () {
                        $state.reload();
                    });
                }
            });
        }

        /**
       * @name updateBookingStatus by bookingId
       * @desc Retrieve factories listing from factory
       * @returns {*}
       */
        function updateBookingStatus(bookingId, statusValue, note, rate) {           
            if (bookingId == undefined) {
                vm.status.BookingId = vm.bookingIdForCancel;              
            } else {
                vm.status.BookingId = bookingId;
            }
            vm.status.StatusValue = statusValue;
            //Call BCAFactory factory get all factories data
            BookingListFactory
                .updateBookingStatus(vm.status)
                .then(function () {
                    $state.reload();
                });
        }

        /**
         * @name closeModal
         * @desc Close pop up
         */
        function closeModel() {
            $scope.modalInstance.dismiss();
        }

        /**
         * @name downloadExcelBookingList
         * @desc download pdf Booking list
         * @returns {*}
         */
        function downloadExcelBookingList(data) {
            var data = data;
            var innerContents = '';
            innerContents += '<table width="100%" border="1" cellpadding="0" cellspacing="0" align="center" style="border:1px solid #b5b5b5;width:100%;"><thead>';
            innerContents += '<tr><td><b>Booking Number</b></td>';
            innerContents += '<td><b>Customer Name</b></td>';
            innerContents += '<td><b>Site Details</b></td>';
            innerContents += '<td><b>Work Type</b></td>';
            innerContents += '<td><b>Status</b></td>';
            innerContents += '<td><b>Booked Fleet</b></td>';
            innerContents += '<td><b>Start Date</b></td>';
            innerContents += '<td><b>End Date</b></td></tr></thead>';
            innerContents += '<tbody style="background-color:#fff;">';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    innerContents += '<tr>';
                    innerContents += '<td>' + data[i].BookingNumber + '</td>';
                    innerContents += '<td>' + data[i].CustomerName + '</td>';
                    innerContents += '<td>' + data[i].SiteDetail + '</td>';
                    innerContents += '<td>' + data[i].WorkType + '</td>';
                    innerContents += '<td>' + data[i].StatusTitle + '</td>';
                    innerContents += '<td>' + data[i].FleetCount + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].FleetBookingDateTime), 'dd-MM-yyyy h:mm') + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].EndDate), 'dd-MM-yyyy') + '</td>';
                    innerContents += '</tr>';
                }
            } else {
                innerContents += '<tr><td colspan="8" class="text-center">Not records found</td></tr>';
            }
            innerContents += '</tbody></table>';

            if (window.navigator.msSaveBlob) { // IE 10+
                window.navigator.msSaveOrOpenBlob(new Blob([(innerContents)], { type: "text/richtext;charset=utf-8;" }), "Booking-List.xls")
            }
            else {
                var uri = 'data:application/vnd.ms-word,' + encodeURIComponent(innerContents);
                var downloadLink = document.createElement("a");
                downloadLink.href = uri;
                downloadLink.download = "Booking-List.xls";
                document.body.appendChild(downloadLink);
                downloadLink.click();
                document.body.removeChild(downloadLink);
            }
        }

        /**
         * @name downloadPrintBookingList
         * @desc print Booking list
         * @returns {*}
         */
        function downloadPrintBookingList(data) {           
            var data = data;
            var innerContents = '';
            innerContents += '<table width="100%" border="1" cellpadding="0" cellspacing="0" align="center" style="border:1px solid #b5b5b5;width:100%;"><thead>';
            innerContents += '<tr><td><b>Booking Number</b></td>';
            innerContents += '<td><b>Customer Name</b></td>';
            innerContents += '<td><b>Site Details</b></td>';
            innerContents += '<td><b>Work Type</b></td>';
            innerContents += '<td><b>Status</b></td>';
            innerContents += '<td><b>Booked Fleet</b></td>';
            innerContents += '<td><b>Start Date</b></td>';
            innerContents += '<td><b>End Date</b></td></tr></thead>';
            innerContents += '<tbody style="background-color:#fff;">';
            if (data.length > 0) {
                for (var i = 0; i < data.length; i++) {
                    innerContents += '<tr>';
                    innerContents += '<td>' + data[i].BookingNumber + '</td>';
                    innerContents += '<td>' + data[i].CustomerName + '</td>';
                    innerContents += '<td>' + data[i].SiteDetail + '</td>';
                    innerContents += '<td>' + data[i].WorkType + '</td>';
                    innerContents += '<td>' + data[i].StatusTitle + '</td>';
                    innerContents += '<td>' + data[i].FleetCount + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].FleetBookingDateTime), 'dd-MM-yyyy h:mm') + '</td>';
                    innerContents += '<td>' + $filter('date')(new Date(data[i].EndDate), 'dd-MM-yyyy') + '</td>';
                    innerContents += '</tr>';
                }
            } else {
                innerContents += '<tr><td colspan="8" class="text-center">Not records found</td></tr>';
            }
            innerContents += '</tbody></table>';
            var popupWindow = window.open('', '_blank', 'width=600,height=700,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
            popupWindow.document.open();
            popupWindow.document.write('<html><body onload="window.print()">' + innerContents + '</html>');
            popupWindow.document.close();
        }

        /**
           * @name savePDF
           * @desc click function for creating PDF
           * @returns {*}
           */
        vm.savePDF = function (printdata) {

            //PDF PRINTER
            var columns = [
                "Booking Number",
                "Customer Name",
                "Site Details",
                "Work Type",
                "Status",
                "Booked Fleet",
                "Start Date",
                "End Date"
            ];

            var rows = [];

            printdata.forEach(function (item) {
                rows.push([
                    item.BookingNumber,
                    item.CustomerName,
                    item.SiteDetail,
                    item.WorkType,
                    item.StatusTitle,
                    item.FleetCount,
                    item.FleetBookingDateTime,
                    $filter('date')(new Date(item.EndDate), 'dd-MM-yyyy')
                ])
            });

            var doc = new jsPDF('l', 'pt');

            function processPDF(doc) {
                var header = "BookingList" + '\n';

                doc.setFontSize(14);
                doc.text(header, 40, 40);

                var altInfo = "";
                //altInfo += "Payroll Date: " + moment.utc(printdata[0].payroll_date).format('MM/DD/YYYY') + "\n";
                //altInfo += "\nTotal Hours: " + printdata[printdata.length - 1].TotalHours + "\n";
                //altInfo += "Total Payment: $" + numberWithCommas(printdata[printdata.length - 1].TotalPayment + "\n");
                //altInfo += "Total Units: " + totalUnits + "\n";

                doc.setFontSize(10);
                doc.text(altInfo, 40, 60);

                var totalPagesExp = "{total_pages_count_string}";

                var pageContent = function (data) {

                    if (data.pageCount != 1) {
                        // HEADER
                        doc.setFontSize(8);
                        doc.setTextColor(40);
                        doc.setFontStyle('normal');

                        doc.text('full name' + ' (' + '1234' + ') ' +
                            "Payroll Date: " + "", data.settings.margin.left + 10, 22);
                    }
                    // FOOTER
                    var str = "Page " + data.pageCount;
                    // Total page number plugin only available in jspdf v1.0+
                    if (typeof doc.putTotalPages === 'function') {
                        str = str + " of " + totalPagesExp;
                    }
                    doc.setFontSize(10);
                    doc.text(str, data.settings.margin.left, doc.internal.pageSize.height - 10);
                };


                doc.autoTable(columns, rows, {
                    headerStyles: {
                        fillColor: [218, 236, 243],
                        textColor: [6, 6, 6]
                    },
                    startY: 70,
                    styles: {
                        overflow: 'linebreak',
                        columnWidth: 'wrap'
                    },
                    addPageContent: pageContent,
                    columnStyles: {
                        0: { columnWidth: 100 },
                        1: { columnWidth: 100 },
                        2: { columnWidth: 100 },
                        3: { columnWidth: 100 },
                        4: { columnWidth: 100 },
                        5: { columnWidth: 100 },
                        6: { columnWidth: 100 },
                        7: { columnWidth: 100 }

                    }
                    //theme: 'plain'
                });

                // Total page number plugin only available in jspdf v1.0+
                if (typeof doc.putTotalPages === 'function') {
                    doc.putTotalPages(totalPagesExp);
                }

                return doc;
            }

            doc = processPDF(doc);
            // doc.addPage();
            // doc = processPDF(doc);

            var filename = "BookingList";
            doc.save(filename.replace(/\s/g, '') + '.pdf');
        };

        /**
         * @name sendEmail
         * @desc click function for sending Email
         * @returns {*}
         */
        vm.sendEmail = function (data) {

            vm.sendEmailResponse = BookingListFactory.sendAttachment(data);
            console.log(vm.sendEmailResponse);
            console.log(JSON.stringify(data));
        }

    }

    //Inject required stuff as parameters to factories controller function
    DashboardBookingCtrl.$inject = ['$scope'];

    /**
    * @name DashboardBookingCtrl
    * @param $scope
    */
    function DashboardBookingCtrl($scope) {

        var vm = this;
        $scope.customerslist = [{
            name: 'SEYMOUR WHYTE CONSTRUCTION PVT LTD',
            place: 'NARELLAN, NSW',
        },
            {
                name: 'BMD CONSTRUCTIONS PTY LTD',
                place: 'WYNNUM, QLD',

            },
            {
                name: 'RIZZANI LEIGHTON JOINT VENTURE',
                place: 'SILVERWATER, NSW',
            },
            {
                name: 'SIMONDS HOMES (NSW) PTY LTD',
                place: 'BAULKHAM HILLS, NSW',
            },
            {
                name: 'MIRVAC HOMES NSW PTY LTD',
                place: 'SYDNEY, NSW',
            },
            {
                name: 'WISDOM LANDSCAPES PTY LTD (HOMES)',
                place: 'NARELLAN, NSW',
            },
            {
                name: 'THE CPB CONTRACTORS DRAGADOS SAMSUNG JOINT VENTURE',
                place: 'MASCOT, NSW',
            },
            {
                name: 'STELLA EARTHWORKS CIVIL PTY LTD',
                place: 'KEMPS CREEK, NSW',
            },
            {
                name: 'EATHER GROUP PTY LTD',
                place: 'LONDONDERRY, NSW',
            },
            {
                name: 'HORSLEY HEAVY HAULAGE PTY LTD',
                place: 'HORSLEY PARK, NSW',
            }
        ];
        //Dashboard Booking FleetList 
        $scope.fleetlist = [{
            fleetheading: 'Bogie/ 8 & 10 Wheeler',
            availability: 'Available: 5',
            total: 'Total: 9',
            imagePath: 'Content/images/vehicle-bogie.png'
        },

            {
                fleetheading: 'Primemover/ Beaver Tail/ Float',
                availability: 'Available: 6',
                total: 'Total: 9',
                imagePath: 'Content/images/Primemover.jpg'

            },
            {
                fleetheading: 'Service Vehicles',
                availability: 'Available: 4',
                total: 'Total: 3',
                imagePath: 'Content/images/service-vehicles.png'
            },
            {
                fleetheading: 'Trailers',
                availability: 'Available: 5',
                total: 'Total: 9',
                imagePath: 'Content/images/vehicle-trailers.png'
            },

            {
                fleetheading: 'Truck & Dog',
                availability: 'Available: 10',
                total: 'Total: 8',
                imagePath: 'Content/images/truck-dog-img.png'
            },
            {
                fleetheading: 'Utes, Cars, Mini Tippers',
                availability: 'Available: 4',
                total: 'Total: 5',
                imagePath: 'Content/images/mini-tippers.png'
            },
            {
                fleetheading: 'Water Cart/ Sweeper/ Skip Bin/ Plant',
                availability: 'Available: 5',
                total: 'Total: 9',
                imagePath: 'Content/images/vehicle-water-cart.png'
            }

        ]

    }

    //Inject required stuff as parameters to factories controller function
    ManageDocketCtrl.$inject = ['$scope', '$rootScope', 'UtilsFactory', 'BCAFactory', 'ManageDocketFactory', '$location', '$stateParams', '$state', '$uibModal', 'Upload', '$timeout', '$filter', 'CONST', 'bootstrap3ElementModifier'];

    /**
    * @name ManageDocketCtrl
    * @param $scope
    * @param $rootScope
    * @param UtilsFactory
    * @param BCAFactory
    * @param ManageDocketFactory
    * @param $location
    * @param $stateParams  
    * @param $state
    * @param $uibModal
    * @param Upload
    * @param $timeout
    * @param $filter
    * @param CONST
    * @param bootstrap3ElementModifier
    */
    function ManageDocketCtrl($scope, $rootScope, UtilsFactory, BCAFactory, ManageDocketFactory, $location, $stateParams, $state, $uibModal, Upload, $timeout, $filter, CONST, bootstrap3ElementModifier) {

        var vm = this;
        vm.bookedFleetRegistration = [];
        vm.fleetRegistrationNumber = {
            "FleetRegistrationId": '',
            "Registration": '',
            "BookingFleetId": ''
        },
        vm.bookingFleetDetail = '';
        vm.attachementList = '';
        vm.check = {};
        vm.ImageBase64 = '';
        vm.Image = CONST.CONFIG.DEFAULT_IMG;
        vm.BookingSiteSupervisors = {
            SupervisorId: '',
            MobileNumber: ''
        };
        vm.fromState = '';
        vm.checkList = {};
        vm.invalidTime = false;
        vm.checkTotalKMs = false;
        vm.docketCheckList = '';
        vm.attributes = {
            BookingFleetId: '',
            SiteId: '',
            FleetRegistrationId: '',
            DocketNo: '',
            StartTime: '',
            EndTime: '',
            StartKMs: '',
            FinishKMsA: '',
            LunchBreak1From: '',
            LunchBreak1End: '',
            LunchBreak2From: '',
            LunchBreak2End: '',
            AttachmentIds: '',
            DocketCheckListId: '',
            IsActive: true,
            ImageBase64: '',
            SupervisorId: '',
            DocketDate: new Date(),
            LoadDocketDataModel: [{
                DocketId: '',
                LoadingSite: '',
                Weight: 0,
                LoadTime: '',
                TipOffSite: '',
                TipOffTime: '',
                Material: '',
                IsActive: true,
            }]
        };

        //Methods
        vm.registrationNumber = registrationNumber;
        vm.getBookingFleetDetail = getBookingFleetDetail;
        vm.getAttachments = getAttachments;
        vm.saveDocket = saveDocket;
        vm.checkTotalHour = checkTotalHour;
        vm.getAllDocketCheckboxList = getAllDocketCheckboxList;
        vm.totalKm = totalKm;
        vm.checkTime = checkTime;
        vm.isCheckedList = isCheckedList;
        vm.clearImage = clearImage;
        vm.loadImageFileAsURL = loadImageFileAsURL;

        //Check States for manage Docket
        $rootScope.$on('$stateChangeStart', function (event, toState, toParams, fromState, fromParams) {
            vm.fromState = fromState;
            vm.toState = toState;
            vm.fromParams = fromParams;
        });

        /**
           * @name registrationNumber
           * @desc retrive booked registration numbers
           * @returns {*}
           */
        function registrationNumber() {
            //if (typeof $stateParams.BookingFleetId !== 'undefined' && $stateParams.BookingFleetId !== '') {
            //    vm.fleetRegistrationNumber.BookingFleetId = $stateParams.BookingFleetId;
            //    getBookingFleetDetail($stateParams.BookingFleetId);
            //}
            ManageDocketFactory
            .getBookedFleetRegistration()
            .then(function () {
                vm.bookedFleetRegistration = ManageDocketFactory.docketDetail.DataList;
            });


        }
        vm.registrationNumber();

        // retrive Docket Detail for update Docket
        if (typeof $stateParams.DocketId !== 'undefined' && $stateParams.DocketId !== '') {
            ManageDocketFactory
               .getDocketDetail($stateParams.DocketId)
               .then(function () {
                   vm.attributes = ManageDocketFactory.docketDetail.DataObject;
                   if (vm.attributes.LoadDocket.length > 0) {
                       vm.attributes.LoadDocketDataModel = vm.attributes.LoadDocket;
                   } else {
                       vm.attributes.LoadDocketDataModel = [{
                           "DocketId": "",
                           "LoadingSite": "",
                           "Weight": 0,
                           "LoadTime": "",
                           "TipOffSite": "",
                           "TipOffTime": "",
                           "Material": "",
                           "IsActive": "true",
                       }];
                   }
                   vm.StartTime = vm.attributes.StartTime;
                   vm.EndTime = vm.attributes.EndTime;
                   vm.attributes.DocketDate = ManageDocketFactory.docketDetail.DataObject.DocketDate;                  
                   if (vm.attributes.FinishKMsA - vm.attributes.StartKMs > 0)
                       vm.attributes.totalKMs = vm.attributes.FinishKMsA - vm.attributes.StartKMs;
                   else {
                       vm.attributes.totalKMs = 0;
                   }
                   //vm.attributes.DocketDate = new Date(vm.attributes.DocketDate);
                   vm.Image = CONST.CONFIG.IMG_URL + vm.attributes.Image;
                   $("#fileName").val(vm.attributes.Image);
                   $("#previewId").attr('src', CONST.CONFIG.IMG_URL + vm.attributes.Image);
                   vm.getBookingFleetDetail(vm.attributes.BookingFleetId);
                   vm.isCheckedList(vm.attributes.DocketCheckListId);
                   ManageDocketFactory
                   .getBookedFleetRegistration()
                   .then(function () {
                       vm.bookedFleetRegistration = ManageDocketFactory.docketDetail.DataList;                      
                       vm.fleetRegistrationNumber = $filter('filter')(vm.bookedFleetRegistration, { FleetRegistrationId: vm.attributes.FleetRegistrationId })[0];

                       vm.BookingSiteSupervisors.SupervisorId = vm.attributes.SupervisorId;
                       vm.BookingSiteSupervisors.MobileNumber = vm.attributes.MobileNumber;
                       //cdctrl.fleetRegistrationNumber

                   });
               });
        }

        /**
           * @name addNewLoading
           * @desc Add new row
           * @returns {*}
           */
        vm.addNewLoading = function () {           
            var i = vm.attributes.LoadDocketDataModel.length - 1;

            if ((vm.attributes.LoadDocketDataModel[i].LoadingSite !== '' && typeof vm.attributes.LoadDocketDataModel[i].LoadingSite !== 'undefined')
                && (vm.attributes.LoadDocketDataModel[i].Weight !== 0 && typeof vm.attributes.LoadDocketDataModel[i].Weight !== 'undefined')
                && (vm.attributes.LoadDocketDataModel[i].LoadTime !== '' && typeof vm.attributes.LoadDocketDataModel[i].LoadTime !== 'undefined')
                && (vm.attributes.LoadDocketDataModel[i].TipOffSite !== '' && typeof vm.attributes.LoadDocketDataModel[i].TipOffSite !== 'undefined')
                && (vm.attributes.LoadDocketDataModel[i].TipOffTime !== '' && typeof vm.attributes.LoadDocketDataModel[i].TipOffTime !== 'undefined')
                && (vm.attributes.LoadDocketDataModel[i].Material !== '' && typeof vm.attributes.LoadDocketDataModel[i].Material !== 'undefined')) {
                vm.attributes.LoadDocketDataModel.push({
                    'LoadingSite': '',
                    'Weight': 0,
                    'LoadTime': '',
                    'TipOffSite': '',
                    'TipOffTime': '',
                    'Material': '',
                    'IsActive': true
                });
            }
        };

        /**
           * @name checkTotalHour
           * @desc Calculate Total Hours 
           * @returns {*}
           */
        function checkTotalHour(StartTime, EndTime) {
            var start_actual_time = Date.parse('01/01/2016 ' + StartTime);
            var end_actual_time = Date.parse('01/01/2016 ' + EndTime);
            var diff = end_actual_time - start_actual_time;
            var diffSeconds = diff / 1000;
            var HH = Math.floor(diffSeconds / 3600);
            var MM = Math.floor(diffSeconds % 3600) / 60;
            if ((HH <= 0 && MM <= 0) || (isNaN(HH) && isNaN(MM))) {
                vm.attributes.totalHour = '';
                vm.invalidTime = true;
            } else {
                vm.invalidTime = false;
                var formatted = ((HH < 10) ? ("0" + HH) : HH) + ":" + ((MM < 10) ? ("0" + MM) : MM)
                vm.attributes.totalHour = formatted;
            }
        }

        /**
           * @name isChecked
           * @desc Check checkboxes by Id while updating
           * @returns {*}
           */
        function isChecked(attachmentIds) {
            vm.check = {};
            var array = attachmentIds.split(',');
            if (array.length > 0) {
                angular.forEach(array, function (items) {
                    vm.check[items] = true;
                });
            }
        }

        /**
          * @name isCheckedList
          * @desc check checkboxes by Id while updating
          * @returns {*}
          */
        function isCheckedList(checkListIds) {
            vm.checkList = {};
            var array = checkListIds.split(',');
            if (array.length > 0) {
                angular.forEach(array, function (items) {
                    vm.checkList[items] = true;
                });
            }
        }

        /**
          * @name getBookingFleetDetail
          * @desc retrive booking fleet detail
          * @returns {*}
          */
        function getBookingFleetDetail(bookingFleetId) {
            BCAFactory
              .getBookingFleetDetail(bookingFleetId)
              .then(function () {
                  vm.bookingFleetDetail = BCAFactory.bookingFleetDetail.DataObject;                  
                  isChecked(vm.bookingFleetDetail.AttachmentIds);
              });
        }

        /**
         * @name getAttachments for check list
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getAttachments() {
            //Call BCAFactory factory get all factories data
            BCAFactory
                .getAttachmentsChk()
                .then(function () {
                    vm.attachementList = BCAFactory.attachementList.DataObject;
                });
        }

        /**
         * @name getAllDocketCheckboxList for check list
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getAllDocketCheckboxList() {
            ManageDocketFactory
             .getAllDocketCheckboxList()
            .then(function () {
                vm.docketCheckList = ManageDocketFactory.docketCheckList.DataList;
            });
        }

        // method call on Ctrl load
        vm.getAttachments();

        // method call on Ctrl load
        vm.getAllDocketCheckboxList();

        /**
          * @name openStartTimeCalendar for check list
          * @desc Open for StartTime DateTimePicker       
          */
        vm.openStartTimeCalendar = function (picker) {
            $('#StartTimePicker').focus();
        };

        /**
          * @name openEndTimeCalendar for check list
          * @desc Open for EndTime DateTimePicker       
          */
        vm.openEndTimeCalendar = function (picker) {
            $('#EndTimePicker').focus();
        };

        /**
        * @name openBreak1FromCalendar for check list
        * @desc Open for Break1From DateTimePicker       
        */
        vm.openBreak1FromCalendar = function (picker) {
            $('#Break1FromPicker').focus();
        };

        /**
          * @name openBreak1ToCalendar for check list
          * @desc Open for Break1To DateTimePicker       
          */
        vm.openBreak1ToCalendar = function (picker) {
            $('#Break1ToPicker').focus();
        };

        /**
          * @name openBreak2FromCalendar for check list
          * @desc Open for Break2From DateTimePicker       
          */
        vm.openBreak2FromCalendar = function (picker) {
            $('#Break2FromPicker').focus();
        };

        /**
        * @name openBreak2ToCalendar for check list
        * @desc Open for forBreak2To DateTimePicker       
        */
        vm.openBreak2ToCalendar = function (picker) {
            $('#Break2ToPicker').focus();
        };

        /**
          * @name openLoadTimePickerCalendar for check list
          * @desc Open for LoadTime DateTimePicker       
          */
        vm.openLoadTimePickerCalendar = function () {
            //alert($(this).find('input[type=text]').focus());
            alert($(this).siblings(":text").attr("name"));
            //$(this).prev().attr().focus();
        };

        /**
         * @name checkTime for check list
         * @desc Check valid totalTime       
         */
        function checkTime() {
            if (vm.EndTime && vm.StartTime) {
                if (Date.parse('01/01/2016 ' + vm.EndTime) > Date.parse('01/01/2016 ' + vm.StartTime)) {
                    return true;
                } else {
                    return false;
                }
            } else {
                return false;
            }
        }

        /**
        * @name totalKm for check list
        * @desc Calculate totalTime       
        */
        function totalKm() {
            if ((vm.attributes.FinishKMsA - vm.attributes.StartKMs) <= 0) {
                vm.attributes.totalKMs = '';
                vm.checkTotalKMs = true;
            } else {
                vm.attributes.totalKMs = (vm.attributes.FinishKMsA - vm.attributes.StartKMs);
                vm.checkTotalKMs = false;
            }
            return vm.checkTotalKMs;
        }

        /**
         * @name saveDocket for add and update
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function saveDocket() {          

            // checking selected checkbox
            console.log(vm.checkTime(), vm.totalKm());
            if (vm.checkTime() === true && vm.totalKm() === false) {                
                vm.invalidTime = true;
                vm.checkTotalKMs = true;
                var log = [];
                var checkListlog = [];
                angular.forEach(vm.check, function (value, key) {
                    if (value == true) {
                        this.push(key);
                    }
                }, log);
                angular.forEach(vm.checkList, function (value, key) {
                    if (value == true) {
                        this.push(key);
                    }
                }, checkListlog);
                var i = vm.attributes.LoadDocketDataModel.length - 1;
                if (vm.attributes.LoadDocketDataModel[i].LoadingSite === '' || vm.attributes.LoadDocketDataModel[i].Weight === 0 || vm.attributes.LoadDocketDataModel[i].LoadTime === '' || vm.attributes.LoadDocketDataModel[i].TipOffSite === '' || vm.attributes.LoadDocketDataModel[i].TipOffTime === '' || vm.attributes.LoadDocketDataModel[i].Material === '') {
                    vm.attributes.LoadDocketDataModel.splice(i, 1);
                }

                vm.attributes.BookingFleetId = vm.fleetRegistrationNumber.BookingFleetId;
                vm.attributes.SupervisorId = vm.BookingSiteSupervisors.SupervisorId;
                vm.attributes.DocketDate = vm.attributes.DocketDate;
                vm.attributes.FleetRegistrationId = vm.fleetRegistrationNumber.FleetRegistrationId;
                vm.attributes.AttachmentIds = log.toString();
                vm.attributes.DocketCheckListId = checkListlog.toString();
                vm.attributes.StartTime = $filter('date')(Date.parse('01/01/2016 ' + vm.StartTime), 'HH:mm:ss');
                vm.attributes.EndTime = $filter('date')(Date.parse('01/01/2016 ' + vm.EndTime), 'HH:mm:ss');
                vm.attributes.LunchBreak1From = $filter('date')(Date.parse('01/01/2016 ' + vm.attributes.LunchBreak1From), 'HH:mm:ss');
                vm.attributes.LunchBreak1End = $filter('date')(Date.parse('01/01/2016 ' + vm.attributes.LunchBreak1End), 'HH:mm:ss');
                vm.attributes.LunchBreak2From = $filter('date')(Date.parse('01/01/2016 ' + vm.attributes.LunchBreak2From), 'HH:mm:ss');
                vm.attributes.LunchBreak2End = $filter('date')(Date.parse('01/01/2016 ' + vm.attributes.LunchBreak2End), 'HH:mm:ss');

                if (angular.element('#imageBase64')[0].value != '') {
                    vm.ImageBase64 = angular.element('#imageBase64')[0].value;
                    vm.attributes.ImageBase64 = vm.ImageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
                } else {
                    vm.attributes.ImageBase64 = '';
                }

                if ($("#fileName").val() === '') {
                    vm.attributes.Image = '';
                }
               
                if (typeof $stateParams.DocketId !== 'undefined' && $stateParams.DocketId !== '') {
                    ManageDocketFactory
                     .updateDocket(vm.attributes)
                      .then(function () {
                          $location.path('booking/DocketList/' + vm.attributes.BookingFleetId);
                      });
                } else {
                    console.log('vm.attributes', vm.attributes);
                    ManageDocketFactory
                     .addDocket(vm.attributes)
                      .then(function () {
                          $location.path('booking/DocketList/' + vm.attributes.BookingFleetId);
                      });
                }

            } else {
                $scope.validationError = true;
                vm.invalidTime = true;
                return false;
            }
        }

        /**
         * @name loadImageFileAsURL
         * @desc Convert Image to base64
          @returns {}
         */
        function loadImageFileAsURL() {
            var filesSelected = document.getElementById("inputFileToLoad").files;
            
            if (filesSelected.length > 0) {
                var fileToLoad = filesSelected[0];
                var fileReader = new FileReader();
                fileReader.onload = function (fileLoadedEvent) {
                    var textAreaFileContents = document.getElementById
                    (
                        "textAreaFileContents"
                    );
                    $("#previewId").attr('src', fileLoadedEvent.target.result);
                    $("#imageBase64").val(fileLoadedEvent.target.result);
                    $("#fileName").val(filesSelected[0].name);
                    $('#clearImageId').show();
                };

                fileReader.readAsDataURL(fileToLoad);
            }
        }

        /**
         * @name clearImage
         * @desc For clear image and set as default image
          @returns {}
         */
        function clearImage() {
            $("#imageBase64").val('');
            $("#fileName").val('');
            $("#previewId").attr('src', CONST.CONFIG.DEFAULT_IMG);
            $('#clearImageId').hide();
        }
    }

    //Inject required stuff as parameters to factories controller function
    DocketViewCtrl.$inject = ['$scope', '$rootScope', 'UtilsFactory', 'BCAFactory', 'ManageDocketFactory', '$location', '$stateParams', '$state', '$uibModal', 'Upload', '$timeout', '$filter', 'CONST'];

    /**
     * @name DocketViewCtrl
     * @param $scope
     * @param $rootScope
     * @param UtilsFactory
     * @param BCAFactory
     * @param ManageDocketFactory
     * @param $location
     * @param $stateParams  
     * @param $state
     * @param $uibModal
     * @param Upload
     * @param $timeout
     * @param $filter   
     */
    function DocketViewCtrl($scope, $rootScope, UtilsFactory, BCAFactory, ManageDocketFactory, $location, $stateParams, $state, $uibModal, Upload, $timeout, $filter, CONST) {
        var vm = this;
        vm.bookingFleetId = '';
        vm.docketList = [];
        vm.checkList = [];
        vm.viewAttributes = {
            "DocketId": "",
            "BookingFleetId": "",
            "SiteId": "",
            "FleetRegistrationId": "",
            "DocketNo": "",
            "StartTime": "",
            "EndTime": "",
            "StartKMs": "",
            "FinishKMsA": "",
            "LunchBreak1From": "",
            "LunchBreak1End": "",
            "LunchBreak2From": "",
            "LunchBreak2End": "",
            "AttachmentIds": "",
            "DocketCheckListId": "",
            "IsActive": true,
            "ImageBase64": "",
            "SupervisorId": "",
            "LoadDocketDataModel": [{
                "DocketId": "",
                "LoadingSite": "",
                "Weight": 0,
                "LoadTime": "",
                "TipOffSite": "",
                "TipOffTime": "",
                "Material": "",
                "IsActive": "true",
            }]
        };
        vm.docketId = ''

        //Method
        vm.isCheckedList = isCheckedList;
        vm.getAllDocketCheckboxList = getAllDocketCheckboxList;
        vm.getDocketDetail = getDocketDetail;
        vm.downloadPrintDocket = downloadPrintDocket;

        /**
           * @name isCheckedList for check list
           * @desc for check checkboxes by Id while updating
           * @returns {*}
           */
        function isCheckedList(checkListIds) {
            vm.checkList = {};
            var array = checkListIds.split(',');
            if (array.length > 0) {
                angular.forEach(array, function (items) {
                    vm.checkList[items] = true;
                });
            }
        }

        /**
            * @name getAllDocketCheckboxList for check list
            * @desc Retrieve factories listing from factory
            * @returns {*}
            */
        function getAllDocketCheckboxList() {
            ManageDocketFactory
             .getAllDocketCheckboxList()
            .then(function () {
                vm.docketCheckList = ManageDocketFactory.docketCheckList.DataList;                
            });
        }

        //Call on load 
        vm.getAllDocketCheckboxList();

        //Check BookingFleetId param for Docketlist
        if (typeof $stateParams.BookingFleetId !== 'undefined' && $stateParams.BookingFleetId !== '') {
            vm.bookingFleetId = $stateParams.BookingFleetId;
            ManageDocketFactory
            .GetAllDocketByBookingFleetId(vm.bookingFleetId)
            .then(function () {
                vm.docketList = ManageDocketFactory.docketList.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.docketList);               
            });
        }

        //Check BookingFleetId param for DocketView
        if (typeof $stateParams.DocketId !== 'undefined' && $stateParams.DocketId !== '') {
            vm.docketId = $stateParams.DocketId;
            vm.getDocketDetail(vm.docketId);
        }

        /**
           * @name getDocketDetail
           * @desc Retrieve factories listing from factory
           * @returns {*}
           */
        function getDocketDetail(docketId) {
            ManageDocketFactory
           .getDocketDetail(docketId)
           .then(function () {
               vm.viewAttributes = ManageDocketFactory.docketDetail.DataObject;              
               $("#image").attr('href', CONST.CONFIG.IMG_URL + vm.viewAttributes.Image);
               if (Date.parse('01/01/2016 ' + vm.viewAttributes.EndTime) > Date.parse('01/01/2016 ' + vm.viewAttributes.StartTime)) {
                   var endDate = Date.parse('01/01/2016 ' + vm.viewAttributes.EndTime);
                   var startDate = Date.parse('01/01/2016 ' + vm.viewAttributes.StartTime);
                   var diff = endDate - startDate;
                   var diffSeconds = diff / 1000;
                   var HH = Math.floor(diffSeconds / 3600);
                   var MM = Math.floor(diffSeconds % 3600) / 60;
                   vm.TotalHour = ((HH < 10) ? ("0" + HH) : HH) + ":" + ((MM < 10) ? ("0" + MM) : MM);
                   vm.viewAttributes.TotalHour = vm.TotalHour;

               } else {
                   vm.viewAttributes.TotalHour = vm.TotalHour = '00:00:00';
                   return false;
               }
               vm.isCheckedList(vm.viewAttributes.DocketCheckListId);

           });
        }

        /**
           * @name downloadPrintDocket
           * @desc print Docket
           * @returns {*}
           */
        function downloadPrintDocket(docketId) {
            vm.getAllDocketCheckboxList();
            ManageDocketFactory
          .getDocketDetail(docketId)
          .then(function () {
              vm.viewAttributes = ManageDocketFactory.docketDetail.DataObject;              
              if (Date.parse('01/01/2016 ' + vm.viewAttributes.EndTime) > Date.parse('01/01/2016 ' + vm.viewAttributes.StartTime)) {
                  var endDate = Date.parse('01/01/2016 ' + vm.viewAttributes.EndTime);
                  var startDate = Date.parse('01/01/2016 ' + vm.viewAttributes.StartTime);
                  var diff = endDate - startDate;
                  var diffSeconds = diff / 1000;
                  var HH = Math.floor(diffSeconds / 3600);
                  var MM = Math.floor(diffSeconds % 3600) / 60;
                  vm.TotalHour = ((HH < 10) ? ("0" + HH) : HH) + ":" + ((MM < 10) ? ("0" + MM) : MM);
                  vm.viewAttributes.TotalHour = vm.TotalHour;

              } else {
                  vm.viewAttributes.TotalHour = vm.TotalHour = '00:00:00';
                  return false;
              }
              // vm.isCheckedList(vm.viewAttributes.DocketCheckListId);
              var array = vm.viewAttributes.DocketCheckListId.split(',');
              var isDayShift = (vm.viewAttributes.IsDayShift === true) ? "Day" : "Night";
              var iswethire = (vm.viewAttributes.Iswethire === true) ? "Yes" : "No";
              var innerContents = '';
              innerContents += '<h3 style="padding-top: 10px;paddding-bottom:20px;font-weight: 200; color: #6a6c6f; text-align:right;font-size:24px;font-family:Arial, Helvetica, sans-serif;">Docket Number:' + (vm.viewAttributes.DocketNo) ? vm.viewAttributes.DocketNo : '________' + '</h3><br>';
              innerContents += '<table width="100%" border="0" cellpadding="0" cellspacing="0" align="center" style="width:100%;">';
              innerContents += '<tbody>';
              innerContents += '<tr>';
              innerContents += '<td width="50%">';
              innerContents += '<table class="table table-bordered" cellpadding="0" cellspacing="0" style="border:1px solid #ddd;width:100%; font-family:Arial, Helvetica, sans-serif; font-size:13px; text-align:left;">';
              innerContents += '<tbody>';
              innerContents += '<tr><th width="180" style="border:1px solid #ddd; padding:8px; text-align-left;">Customer Name</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.CustomerName + '</td>';
              innerContents += '<th width="180" style="border:1px solid #ddd; padding:8px;">Site Name</th>';
              innerContents += '<td style="border:1px solid #ddd;padding:8px;">' + vm.viewAttributes.SiteName + '</td>';
              innerContents += '</tr><tr>';
              innerContents += '<th width="180" style="border:1px solid #ddd; padding:8px;">Contact Person Name</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.SupervisorName + '</td>';
              innerContents += '<th width="180" style="border:1px solid #ddd; padding:8px;">Contract Number</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.BookingNumber + '</td>';
              innerContents += '</tr><tr><th style="border:1px solid #ddd; padding:8px;">Driver\'s Name</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.DriverName + '</td>'
              innerContents += '<th width="180" style="border:1px solid #ddd; padding:8px;">Shift</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + isDayShift + '</td>';
              innerContents += '</tr><tr><th style="border:1px solid #ddd; padding:8px;">Date &amp; Day</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.DocketDate + '</td>';
              innerContents += '<th width="180" style="border:1px solid #ddd; padding:8px;">Wet Hire</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + iswethire + '</td></tr>';
              innerContents += '</tbody></table></td>';
              //Second Table
              innerContents += '<table class="table table-bordered text-center" width="100%" cellpadding="0" cellspacing="0" style="table-layout: fixed;border:1px solid #ddd; font-family:Arial, Helvetica, sans-serif; font-size:13px; margin-top:20px; text-align:center">';
              innerContents += '<tbody><tr><td style="border:1px solid #ddd; padding:8px;"><label>Start</label></td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;"><label>End</label></td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;"><label>Total Hours</label></td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;"><label>Brog Civil Truck No.</label></td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;"><label>Truck Rego</label></td></tr>';
              innerContents += '<tr><td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.StartTime + '</td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.EndTime + '</td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.TotalHour + '</td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;"> </td>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.Registration + '</td>';
              innerContents += '</tr></tbody></table>';

              //Lunch Table 
              innerContents += '<table class="table m-t10 m-b0" style="font-family:Arial, Helvetica, sans-serif; font-size:13px;">';
              innerContents += '<tbody><tr>';
              innerContents += '<td nowrap="" style="padding:8px;"><h4 style="font-size:18px; padding-top:10px; padding-bottom:10px; width:130px;">Lunch</h4></td>';
              innerContents += '<td style="padding:8px;"><p style="margin-bottom:10px;"><label style="font-size:12px;color:#666666; font-weight:400;">Break 1</label>';
              innerContents += '<label style="font-size:12px;color:#666666; font-weight:400;"> From:' + ((vm.viewAttributes.LunchBreak1From != '00:00:00' && vm.viewAttributes.LunchBreak1From) ? vm.viewAttributes.LunchBreak1From : '______________') + '</label>';
              innerContents += '<label style="font-size:12px;color:#666666; font-weight:400;"> To:' + ((vm.viewAttributes.LunchBreak1End != '00:00:00' && vm.viewAttributes.LunchBreak1End) ? vm.viewAttributes.LunchBreak1End : '______________') + '</label>';
              innerContents += '</p><p style="margin-bottom:0px;"><label style="font-size:12px;color:#666666; font-weight:400;">Break 2</label>';
              innerContents += '<label style="font-size:12px;color:#666666; font-weight:400;"> From:' + ((vm.viewAttributes.LunchBreak2From != '00:00:00' && vm.viewAttributes.LunchBreak2From) ? vm.viewAttributes.LunchBreak2From : '______________') + '</label>';
              innerContents += '<label style="font-size:12px;color:#666666; font-weight:400;"> To:' + ((vm.viewAttributes.LunchBreak2End != '00:00:00' && vm.viewAttributes.LunchBreak2End) ? vm.viewAttributes.LunchBreak2End : '______________') + '</label>';
              innerContents += '</p></td>';
              innerContents += '</tr></tbody></table>';

              //Loading Table
              innerContents += '<table class="table table-bordered" cellpadding="0" cellspacing="0" style="border:1px solid #ddd; font-family:Arial, Helvetica, sans-serif; font-size:13px; margin-top:0px; text-align:left;">';
              innerContents += '<tbody><tr><th nowrap="" width="1%" style="border:1px solid #ddd; padding:8px;">Load</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Loading Site</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Weight(tonne)</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Load Time</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Tip Off Site</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Tip Off Time</th>';
              innerContents += '<th style="border:1px solid #ddd; padding:8px;">Material</th>';
              innerContents += '</tr>';
              console.log(vm.viewAttributes.LoadDocket[0]);
              if (vm.viewAttributes.LoadDocket.length > 0) {

                  for (var i = 0; i < vm.viewAttributes.LoadDocket.length; i++) {
                      innerContents += '<tr>';
                      innerContents += '<td align="center" style="border:1px solid #ddd; padding:8px;">' + i + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].LoadingSite + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].Weight + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].LoadTime + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].TipOffSite + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].TipOffTime + '</td>';
                      innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.LoadDocket[i].Material + '</td>';
                      innerContents += '</tr>';

                  }
              } else {
                  innerContents += '<tr>';
                  innerContents += '<td align="center" colspan="7" style="padding:8px;">No Records Found</td>';
                  innerContents += '</tr>';
              }
              innerContents += '</tbody></table>';

              //Check list Table
              innerContents += '<table width="100%">';
              innerContents += '<tbody><tr>';
              innerContents += '<td width="75%" style="vertical-align: top">';

              if (vm.docketCheckList.length > 0) {
                  innerContents += '<ul class="list-left-box" style="font-family:Arial, Helvetica, sans-serif; font-size:13px; margin-top:20px; padding-left:0;">';
                  for (var i = 0; i < vm.docketCheckList.length; i++) {
                      innerContents += '<li style="display: inline-block;vertical-align: top;margin-right: 5px;width: 23%; padding-left:0; padding-bottom:7px;">';
                      if (array.indexOf(vm.docketCheckList[i].DocketCheckListId) != -1) {
                          innerContents += '<img src="Content/images/form-checkbox.png" style="vertical-align:middle; margin-right:5px;"/>';
                      }
                      else {
                          innerContents += '<img src="Content/images/form-checkbox-notselect.png" style="vertical-align:middle; margin-right:5px;"/>';
                      }
                      innerContents += vm.docketCheckList[i].Title;
                      innerContents += '</li>';
                  }
                  innerContents += '</ul>'
              }

              innerContents += '</td>';
              innerContents += '<td width="25%" align="right" style="vertical-align: top">';
              // KMs Table
              innerContents += '<table class="table table-bordered" cellpadding="0" cellspacing="0" width="100%" style="border:1px solid #ddd; font-family:Arial, Helvetica, sans-serif; font-size:13px;  text-align:left;">';
              innerContents += '<tbody><tr><th width="110" style="border:1px solid #ddd; padding:8px;">Start KMs</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.StartKMs + '</td>';
              innerContents += '</tr><tr><th style="border:1px solid #ddd; padding:8px;">Finish KMs</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + vm.viewAttributes.FinishKMsA + '</td>';
              innerContents += '</tr><tr><th style="border:1px solid #ddd; padding:8px;">Total KMs</th>';
              innerContents += '<td style="border:1px solid #ddd; padding:8px;">' + (((vm.viewAttributes.FinishKMsA - vm.viewAttributes.StartKMs) > 0) ? (vm.viewAttributes.FinishKMsA - vm.viewAttributes.StartKMs) : 0) + '</td>';
              innerContents += '</tr></tbody></table>';
              innerContents += '</td></tr></tbody></table>';
              innerContents += '<p style="font-size:13px;font-family:Arial, Helvetica, sans-serif;margin-top:20px;margin-bottom:10px;">By signing this Daliy Drivers docket I confirm that I am free from the effects of drugs/alcohol and fatigue';
              innerContents += 'and I am fit for work. I understand that I am to report to my supervisor if not fit for duty.';
              innerContents += 'I understand that if I make a false or misleading declaration it may be considered to be an offence and I may be subject to Brog Civil disciplinary action.<br>';
              innerContents += 'Driver\'s Signature_____________________________<br></p>';

              // Signature Table
              innerContents += '<table width="100%"><tbody><tr><td>Customer Name_________________</td>';
              innerContents += '<td align="right">Supervisor Signature_________________</td></tr></tbody></table>';

              var popupWindow = window.open('', '_blank', 'width=800,height=900,scrollbars=yes,menubar=no,toolbar=no,location=no,status=no,titlebar=no');
              popupWindow.document.open();
              popupWindow.document.write('<html><body onload="window.print()">' + innerContents + '</html>');
              popupWindow.document.close();
          });
            // var data = vm.waListData.data;

        }
    }
})();