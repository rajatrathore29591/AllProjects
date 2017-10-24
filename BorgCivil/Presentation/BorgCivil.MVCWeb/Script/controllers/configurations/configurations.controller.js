(function () {
    'use strict';

    /**
     * Configuration Controller
     * Created by: (SIPL)
     */

    angular
        .module('borgcivil')
        .controller('FleetRegistrationListCtrl', FleetRegistrationListCtrl)
        .controller('ManageFleetRegistrationCtrl', ManageFleetRegistrationCtrl)
        .controller('UserListCtrl', UserListCtrl)
        .controller('ManageUserCtrl', ManageUserCtrl)
        .controller('DriverListCtrl', DriverListCtrl)
        .controller('ManageDriverCtrl', ManageDriverCtrl)
        .controller('FleetTypeListCtrl', FleetTypeListCtrl)
        .controller('ManageFleetTypeCtrl', ManageFleetTypeCtrl)
        .controller('WorkTypeCtrl', WorkTypeCtrl)
        .controller('CheckListCtrl', CheckListCtrl)
        .controller('CustomerListCtrl', CustomerListCtrl)
        .controller('ManageCustomerCtrl', ManageCustomerCtrl)
        .controller('SiteListCtrl', SiteListCtrl)
        .controller('ManageSiteCtrl', ManageSiteCtrl)
        .controller('SupervisorCtrl', SupervisorCtrl)
        .controller('GateListCtrl', GateListCtrl)
        .controller('ManageGateCtrl', ManageGateCtrl)

    //Inject required stuff as parameters to factories controller function
    FleetRegistrationListCtrl.$inject = ['$scope', '$state', '$uibModal', 'ManageFleetRegistrationFactory', 'UtilsFactory', 'CONST'];

    /**
     * @name FleetRegistrationListCtrl
     * @param $scope
     * @param $state
     * @param $uibModal
     * @param ManageFleetRegistrationFactory
     * @param UtilsFactory
     */
    function FleetRegistrationListCtrl($scope, $state, $uibModal, ManageFleetRegistrationFactory, UtilsFactory, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.fleetRegList = '';
        vm.fleetAttribute = '';
        //vm.images = '';
        vm.filePath = CONST.CONFIG.IMG_URL;

        //Methods
        vm.getFleetRegList = getFleetRegList;
        vm.viewFleetRegistration = viewFleetRegistration;
        vm.deleteFleetRegistration = deleteFleetRegistration;
        vm.cancel = cancel;

        /**
         * @name getFleetRegList
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getFleetRegList() {
            //Call Fleet Registration factory get all factories data
            ManageFleetRegistrationFactory
                .getAllFleetRegistration()
            .then(function () {
                vm.fleetRegList = ManageFleetRegistrationFactory.fleetRegLists.DataList;
                console.log(vm.fleetRegList);
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.fleetRegList);
            });
            //vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.bookings);
            
        }

        /* Call getFleetRegList method to show the factories data list on page load */
        vm.getFleetRegList();

        /**
        * @name viewFleetRegistration
        * @desc View Fleet Registration on popup modal        
        */
        function viewFleetRegistration(fleetRegData) {
            vm.fleetAttribute = fleetRegData;
            vm.fleetAttribute.images = fleetRegData.Image.split(',');
            console.log(vm.fleetAttribute.attachments);
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/FleetRegistrationDetail.html',
                scope: $scope
            });
        }

        /**
          * @name cancel
          * @desc To Dismiss Modal.
          */
        function cancel() {
            $scope.modalInstance.dismiss();
        }

        function deleteFleetRegistration(fleetRegistrationId) {
            ManageFleetRegistrationFactory
            .deleteFleetRegistration(fleetRegistrationId)
            .then(function () {
                $state.reload();
            });
        }
    }

    //Inject required stuff as parameters to factories controller function
    ManageFleetRegistrationCtrl.$inject = ['$scope', '$stateParams', '$state', 'ManageFleetRegistrationFactory', 'BCAFactory', 'Upload', '$timeout', '$filter', 'CONST'];

    /**
     * @name ManageFleetRegistrationCtrl
     * @param $scope
     * @param ManageFleetRegistrationFactory
     * @param BCAFactory
     * @param Upload
     * @param $timeout
     * @param $filter
     */
    function ManageFleetRegistrationCtrl($scope, $stateParams, $state, ManageFleetRegistrationFactory, BCAFactory, Upload, $timeout, $filter, CONST) {

        //Assign controller scope pointer to a variable
        var vm = this;
        vm.fleetRegDetail = '';
        vm.fleetRegistrationId = '';
        vm.fleetTypeList = '';
        vm.fleetType = '';
        vm.Images = '';
        vm.imagePath = CONST.CONFIG.IMG_URL;
        vm.yearRegx = /^(19[5-9]\d|20[0-4]\d|2050)$/;
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.attachementList = '';

        // map create Fleet Registration property
        vm.attributes = {
            FleetRegistrationId: '',
            FleetTypeId: '',
            SubcontractorId:'',
            Make: '',
            Model: '',
            Capacity: '',
            Registration: '',
            BorgCivilPlantNumber: '',
            VINNumber: '',
            EngineNumber: '',
            InsuranceDate: new Date(),
            CurrentMeterReading: '',
            LastServiceMeterReading: '',
            ServiceInterval: '',
            HVISType: '',
            UnavailableFromDate: new Date(),
            UnavailableToDate: new Date(),
            UnavailableNote: '',
            AttachmentId: '',
            IsActive: true,
            CreatedBy: '',
            EditedBy: '',
            files: [],
        };
      
        //Methods
        vm.getFleetType = getFleetType;
        vm.reset = reset;
        vm.updateDetails = updateDetails;
        vm.closeForm = closeForm;
        vm.getAttachments = getAttachments;
        vm.getSubcontractor = getSubcontractor;

        /**
          * @name getFleetType
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getFleetType() {
            BCAFactory
            .getFleetTypesDDL()
            .then(function () {
                vm.fleetTypeList = BCAFactory.fleetTypeList.DataList;
                //console.log(vm.fleetTypeList);
            });
        }

        /**
          * @name getAttachments for dropdown
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getAttachments() {
            //console.log("window.location.hostname", window.location.hostname);
            //Call BCAFactory factory get all factories data
            BCAFactory
                .getAttachmentsChk()
                .then(function () {
                    vm.attachmentList = BCAFactory.attachementList.DataObject.Attachments;
                    //console.log(vm.attachmentList);
                });
        }

        /**
          * @name getSubcontractor for dropdown
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getSubcontractor() {
            //console.log("window.location.hostname", window.location.hostname);
            //Call BCAFactory factory get all factories data
            BCAFactory
                .getSubcontractor()
                .then(function () {
                    vm.subcontractorList = BCAFactory.subContractors.DataObject;
                    console.log("vm.subcontractorList", vm.subcontractorList);
                });
        }

        //Fetch attachments on page load
        vm.getAttachments();
        vm.getSubcontractor();

        //Get Fleet Registration Data by ID for update
        if (typeof $stateParams.FleetRegistrationId !== 'undefined' && $stateParams.FleetRegistrationId !== '') {
            vm.fleetRegistrationId = $stateParams.FleetRegistrationId;
            ManageFleetRegistrationFactory
            .getFleetRegistrationDetail(vm.fleetRegistrationId)
            .then(function () {
                vm.attributes = ManageFleetRegistrationFactory.fleetRegInfo.DataObject;
                vm.attributes.Year = parseInt(vm.attributes.Year);
                vm.attributes.CurrentMeterReading = parseInt(vm.attributes.CurrentMeterReading);
                vm.attributes.LastServiceMeterReading = parseInt(vm.attributes.LastServiceMeterReading);
                vm.attributes.InsuranceDate = new Date(vm.attributes.InsuranceDate);
                vm.attributes.UnavailableFromDate = new Date(vm.attributes.UnavailableFromDate);
                vm.attributes.UnavailableToDate = new Date(vm.attributes.UnavailableToDate);
                console.log(vm.attributes);
                vm.files= vm.attributes.Image.split(',');
                BCAFactory
               .getFleetTypesDDL()
               .then(function () {
                   vm.fleetTypeList = BCAFactory.fleetTypeList.DataList;
                   //console.log(vm.fleetTypeList);
                   vm.fleetType = $filter('filter')(vm.fleetTypeList, { FleetTypeId: vm.attributes.FleetTypeId })[0];
               });

            })
        } else {
            //List of Fleet Types Call on page load
            vm.getFleetType();
        }

        /**
          * @name updateDetails
          * @desc Save records of Fleet Registration
          * @returns {*}
          */
        function updateDetails(add_fleetregistration_form) {
           
            vm.attributes.files = vm.Image;           
            vm.attributes.FleetTypeId = vm.fleetType.FleetTypeId;
            console.log(vm.attributes);
            if (typeof $stateParams.FleetRegistrationId !== 'undefined' && $stateParams.FleetRegistrationId !== '') {
                vm.attributes.FleetRegistrationId = vm.fleetRegistrationId;
                ManageFleetRegistrationFactory
                .updateFleetRegistration(vm.attributes)
                .then(function () {
                    $state.go('configuration.FleetRegistrationList');
                });
            } else {
                ManageFleetRegistrationFactory
                .addFleetRegistration(vm.attributes)
                .then(function () {
                    $state.go('configuration.FleetRegistrationList');
                });
            }
        }

        // Reset Function 
        var originalAttributes = angular.copy(vm.attributes);

        /**
       * @name reset
       * @desc Reset elements value
       * @returns {*}
       */
        function reset() {
            vm.attributes = angular.copy(originalAttributes);
        };

        /**
        * @name loadImageFileAsURL
        * @descDisplay Image name 
        * @returns {*}
        */
        function loadImageFileAsURL() {
            vm.attributes.Image = '';
            vm.files = '';
            vm.filesSelected = document.getElementById("inputFileToLoad").files;
            //console.log(vm.filesSelected);
        }

        function closeForm() {
            $state.go('configuration.FleetRegistrationList');
        }
    };

    //Inject required stuff as parameters to factories controller function
    UserListCtrl.$inject = ['$scope', '$state', 'UtilsFactory', 'UserFactory', '$stateParams'];

    /**
        * @name UserListCtrl
        * @param $scope
        * @param $state
        * @param UtilsFactory
        * @param UserFactory
        * @param $stateParams
       */
    function UserListCtrl($scope, $state, UtilsFactory, UserFactory, $stateParams) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.userId = '';
        vm.countries = {};
        vm.states = {};
        vm.userList = {};

        //Methods
        vm.getAllCountries = getAllCountries;
        vm.getStates = getStates;
        vm.getAllUsers = getAllUsers;
        vm.deleteUser = deleteUser;

        /**
        * @name getAllUsers
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getAllUsers() {
            UserFactory
             .getAllEmployee()
             .then(function () {
                 vm.userList = UserFactory.users.DataList;
                 vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.userList);
                 console.log(vm.userList);
             });
        }
        vm.getAllUsers();
        /**
         * @name getAllCountries
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getAllCountries() {
            UserFactory
            .getAllCountry()
            .then(function () {
                vm.countries = UserFactory.countries;
            });

        }
        vm.getAllCountries();

        /**
         * @name getAllStates
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getStates(countryId) {
            UserFactory
             .getAllState(countryId)
             .then(function () {
                 vm.states = UserFactory.states;
             });
        }

        /**
        * @name deleteUser
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function deleteUser(userId) {
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    UserFactory
                        .deleteEmployee(userId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });
        }

    }

    //Inject required stuff as parameters to factories controller function
    ManageUserCtrl.$inject = ['$scope', '$state', 'UserFactory', '$stateParams', 'CONST'];

    /**
       * @name ManageUserCtrl
       * @param $scope
       * @param $state
       * @param UserFactory
       * @param $stateParams
       * @param CONST
       */
    function ManageUserCtrl($scope, $state, UserFactory, $stateParams, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.userId = '';
        vm.countries = {};
        vm.country = '';
        vm.state = '';
        vm.states = {};
        vm.role = '';
        //Job Title Select Listing
        vm.roles = {};
        vm.Image = CONST.CONFIG.DEFAULT_IMG;
        vm.attributes = {
            RoleId: '',
            FirstName: '',
            SurName: '',
            Email: '',
            ContactNumber: '',
            Address1: '',
            Address2: '',
            CountryId: '',
            StateId: '',
            City: '',
            ZipCod: '',
            ImageBase64: '',
            EmploymentCategoryId: '',
            EmploymentStatusId: '',
            IsActive: true
        };
        vm.passwordAttributes = {

        };

        //Methods
        vm.getAllCountries = getAllCountries;
        vm.getStates = getStates;
        vm.updateDetails = updateDetails;
        vm.getAllRoles = getAllRoles;
        vm.clearImage = clearImage;
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.changePassword = changePassword;

        //For Edit User
        if (typeof $stateParams.UserId !== 'undefined' && $stateParams.UserId !== '') {
            vm.userId = $stateParams.UserId;
            UserFactory
            .getEmployeeDetail(vm.userId)
            .then(function () {
                vm.country = { Value: '' };
                vm.state = { Value: '' };
                vm.attributes = UserFactory.userInfo.DataObject;
                //console.log(vm.attributes);
                vm.country.Value = vm.attributes.CountryId;
                vm.state.Value = vm.attributes.StateId;
                vm.attributes.ContactNumber = parseInt(vm.attributes.ContactNumber);
                vm.attributes.ZipCode = parseInt(vm.attributes.ZipCode);
                vm.role = vm.attributes.RoleId
                vm.Image = CONST.CONFIG.IMG_URL + vm.attributes.Image;              
                $("#fileName").val(vm.attributes.Image);
                $("#previewId").attr('src', CONST.CONFIG.IMG_URL + vm.attributes.Image);
                vm.getStates(vm.attributes.CountryId)
            });
        }

        /**
         * @name getAllCountries
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getAllCountries() {
            UserFactory
            .getAllCountry()
            .then(function () {
                vm.countries = UserFactory.countries;
            });
        }

        //Call on page load
        vm.getAllCountries();

        /**
        * @name getAllRoles
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getAllRoles() {
            UserFactory
            .getAllRoles()
            .then(function () {
                vm.roles = UserFactory.roles.DataList;
            });
        }

        //Call on page load
        vm.getAllRoles();

        /**
         * @name getAllStates
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getStates(countryId) {
            UserFactory
             .getAllState(countryId)
             .then(function () {
                 vm.states = UserFactory.states;
             });
        }

        /**
          * @name loadImageFileAsURL
          * @desc Convert Image to base64
          * @returns {*}
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
                    $("#fileName").html(filesSelected[0].name);
                    $('#clearImageId').show();
                };

                fileReader.readAsDataURL(fileToLoad);
            }
        }

        /**
         * @name clearImage
         * @desc For clear image and set as default image
         * @returns {*}
         */
        function clearImage() {
            $("#imageBase64").val('');
            $("#fileName").html('');
            $("#previewId").attr('src', CONST.CONFIG.DEFAULT_IMG);
            $('#clearImageId').hide();
        }

        /**
        * @name updateDetails
        * @desc Update User detail
        * @returns {*}
        */
        function updateDetails(formValid) {            
            if (angular.element('#imageBase64')[0].value != '') {
                vm.ImageBase64 = angular.element('#imageBase64')[0].value;
                vm.attributes.ImageBase64 = vm.ImageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
            } else {
                vm.attributes.ImageBase64 = '';
            }
            vm.attributes.CountryId = vm.country.Value;
            vm.attributes.StateId = vm.state.Value;
            vm.attributes.RoleId = vm.role;
            if (typeof vm.userId !== 'undefined' && vm.userId !== '') {
                //console.log("update", vm.attributes);
                UserFactory
                 .updateEmployee(vm.attributes)
                 .then(function () {
                     $state.go("configuration.UserList");
                 });

            } else {
                //console.log("add", vm.attributes);
                UserFactory
                 .addEmployee(vm.attributes)
                 .then(function () {
                     $state.go("configuration.UserList");
                 });
            }
        }

        /**
       * @name changePassword
       * @desc Update User password
       * @returns {*}
       */
        function changePassword(form) {
            console.log(vm.passwordAttributes);
            vm.passwordAttributes.EmployeeId = 'e9da2bf0-7376-4770-922e-0a9ae26b2f35';
            UserFactory
            .changePassword(vm.passwordAttributes)
            .then(function () {
                console.log("password Updated");
                $state.go('booking.BookingDashboard');
            });
        }

    }

    //Inject required stuff as parameters to factories controller function
    DriverListCtrl.$inject = ['$scope', '$state', 'UtilsFactory', 'DriverFactory', '$stateParams', 'CONST'];

    /**
        * @name DriverListCtrl
        * @param $scope
        * @param $state
        * @param UtilsFactory
        * @param DriverFactory     
        * @param $stateParams
       */
    function DriverListCtrl($scope, $state, UtilsFactory, DriverFactory, $stateParams, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.driverId = '';
        vm.countries = {};
        vm.states = {};
        vm.driverList = {};
        vm.imagePath = CONST.CONFIG.IMG_URL;
        vm.defaultImg = CONST.CONFIG.BASE_URL + CONST.CONFIG.DEFAULT_IMG;

        //Methods
        vm.getAllDriver = getAllDriver;
        //vm.getStates = getStates;
        //vm.getAllUsers = getAllUsers;
        vm.deleteDriver = deleteDriver;

        /**
        * @name getAllDriver
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getAllDriver() {
            DriverFactory
             .getAllDriver()
             .then(function () {
                 vm.driverList = DriverFactory.drivers.DataList;
                 vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.driverList);
                 console.log(vm.driverList);
             });
        }

        //Call on page load
        vm.getAllDriver();       

        /**
        * @name deleteDriver
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function deleteDriver(driverId) {
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    DriverFactory
                        .deleteDriver(driverId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });
        }

    }

    //Inject required stuff as parameters to factories controller function
    ManageDriverCtrl.$inject = ['$scope', '$state', 'DriverFactory', 'BCAFactory', 'UserFactory', '$stateParams', 'CONST', '$filter'];

    /**
       * @name ManageDriverCtrl
       * @param $scope
       * @param $state
       * @param DriverFactory
       * @param BCAFactory
       * @param UserFactory
       * @param $stateParams
       * @param CONST
       */
    function ManageDriverCtrl($scope, $state, DriverFactory, BCAFactory, UserFactory, $stateParams, CONST, $filter) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.attachments = '';
        vm.DriverId = '';
        vm.countries = {};
        vm.country = '';
        vm.state = '';
        vm.states = {};      
        vm.employeeCategoriesList = {};
        vm.statusLookupList = {};
        vm.getLicenseClassList = {};
        vm.employmentCategory = '';
        vm.employmentStatus = '';
        vm.licenseClass = '';       
        vm.ProfilePic = CONST.CONFIG.BASE_URL + CONST.CONFIG.DEFAULT_IMG;
        //vm.Attachments = '';
        vm.licenseImage = '';
        vm.attributes = {
            Attachments:'',
            AddressLine1:'',
            AddressLine2:'',
            Awards:'',
            BaseRate: '',
            ImageBase64:'',
            CardNumber:'',
            CountryId: '',
            CreatedDate: '',
            DriverId: '',
            Email: '',
            EmploymentCategoryId: '',
            ExpiryDate: new Date(),
            StatusLookupId:'',
            FirstName: '',
            FleetRegistrationId: '',           
            LastName: '',
            LicenseClassId: '',
            LicenseNumber: '',
            LeaveFromDate: new Date(),
            LeaveToDate: new Date(),
            LeaveNote: '',
            MobileNumber: '',
            PostCode: '',
            Shift: '',
            StateId: '',
            TypeFromDate: new Date(),
            TypeToDate: new Date(),
            TypeNote: '',
            DriverWhiteCard: [],
            DriverWhiteCardCount: '',
            DriverInductionCard: [],
            DriverInductionCardCount: '',
            DriverVocCard: [],
            DriverVocCardCount: '',
            AnonymousField: [],
            AnonymousFieldCount: '',
            IsActive: true
        };

        //white Cards Grid List Content with PUsh
        vm.whiteCards = [];
        vm.whiteCardsRow = {
            issueDate: new Date(),
            cardNumber: '',
            note: ''
        };

        //Induction Cards Grid List Content with PUsh
        vm.inductionCards = [];
        vm.inductionRow = {
            siteCost:'',
            issueDate: new Date(),
            cardNumber: '',
            expiryDate: new Date(),
            note: ''
        };

        //VOC Cards Grid List Content with PUsh
        vm.vocCards = [];
        vm.vocRow = {
            issueDate: new Date(),
            cardNumber: '',
            rtoNumber: '',
            note: ''
        }
        
        //VOC Cards Grid List Content with PUsh
        vm.anonymousFields = [];
        vm.anonymousFieldRow = {
            issueDate: new Date(),
            expiryDate: new Date(),
            title: '',
            otherOne: '',
            otherTwo: '',
            note: ''
        }

        vm.tabs = [{ active: true }, { active: false }, { active: false }, { active: false }];

        vm.tabError = {
            personInformation: false,
            sensitiveInformation: false
        }

        //Methods
        vm.getAllCountries = getAllCountries;
        vm.getStates = getStates;
        vm.updateDetails = updateDetails;      
        vm.clearImage = clearImage;
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.loadAttachmentFile = loadAttachmentFile;
        vm.getEmploymentCategory = getEmploymentCategory;
        vm.getLicenseClass = getLicenseClass;
        vm.getStatusLookup = getStatusLookup;
        vm.goDriverList = goDriverList;
      
        /**
           * @name goNextTab
           * @desc Tab Navigation          
           */
        vm.goNextTab = function (index) {
            
            for(var i=0; i<=vm.tabs.length; i++){
                if (i === index + 1) {
                    vm.tabs[index + 1].active = true;
                }
                else {
                    vm.tabs[index].active = false;
                }
            }                     
        }

        /**
          * @name goPreviousTab
          * @desc Tab Navigation         
          */
        vm.goPreviousTab = function (index) {
            for (var i = 0; i <= vm.tabs.length; i++) {
                if (i === index - 1) {
                    vm.tabs[index - 1].active = true;
                }
                else {
                    vm.tabs[index].active = false;
                }
            }
            vm.tabs[index-1].active = true;
        }

        /**
        * @name getEmploymentCategory
        * @desc Retrieve factories listing from factory
        * @returns {*}
        */
        function getEmploymentCategory() {
            BCAFactory
            .getEmploymentCategory()
            .then(function () {
                vm.employeeCategoriesList = BCAFactory.employmentCategoryList.DataList;                
            });
        }      

        /**
          * @name getStatusLookup
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getStatusLookup() {
            BCAFactory
            .getStatusLookup()
            .then(function () {
                vm.statusLookupList = BCAFactory.statusLookupList.DataList;               
            });
        }

        /**
          * @name getLicenseClass
          * @desc Retrieve factories listing from factory
          * @returns {*}
          */
        function getLicenseClass() {
            BCAFactory
            .getLicenseClass()
            .then(function () {
                vm.getLicenseClassList = BCAFactory.getLicenseClassList.DataList;               
            });
        }

        /**
         * @name getAllCountries
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getAllCountries() {
            UserFactory
            .getAllCountry()
            .then(function () {
                vm.countries = UserFactory.countries;
            });
        }

        //Call on page load
        vm.getEmploymentCategory();
        vm.getStatusLookup();
        vm.getLicenseClass();
        vm.getAllCountries();

        //For Edit User
        if (typeof $stateParams.DriverId !== 'undefined' && $stateParams.DriverId !== '') {           

            vm.DriverId = $stateParams.DriverId;
            DriverFactory
            .getDriverDetail(vm.DriverId)
            .then(function () {

                //initialise variables
                vm.country = { Value: '' };
                vm.state = { Value: '' };
                vm.employmentCategory = { EmploymentCategoryId: '' };
                vm.employmentStatus = { StatusId: '' };
                vm.licenseClass = { LicenseClassId: '' };

                vm.attributes = DriverFactory.driverInfo.DataObject;
                console.log(vm.attributes);
                               
                vm.country.Value = vm.attributes.CountryId;
                vm.state.Value = vm.attributes.StateId;

                //getting date type
                vm.attributes.ExpiryDate = new Date(vm.attributes.ExpiryDate);
                vm.attributes.LeaveFromDate = new Date(vm.attributes.LeaveFromDate);
                vm.attributes.LeaveToDate = new Date(vm.attributes.LeaveToDate);
                vm.attributes.TypeFromDate = new Date(vm.attributes.TypeFromDate);
                vm.attributes.TypeToDate = new Date(vm.attributes.TypeToDate);

                //Parsing the values
                vm.attributes.MobileNumber = parseInt(vm.attributes.MobileNumber);
                vm.attributes.ZipCode = parseInt(vm.attributes.ZipCode);
                vm.attributes.BaseRate = parseFloat(vm.attributes.BaseRate); 
                vm.attributes.CardNumber = parseInt(vm.attributes.CardNumber);
                vm.attributes.LicenseNumber = parseInt(vm.attributes.LicenseNumber);
                vm.attributes.AnnualLeaveBalance = parseInt(vm.attributes.AnnualLeaveBalance);
                vm.attributes.SickLeaveBalance = parseInt(vm.attributes.SickLeaveBalance);

                //
                vm.employmentCategory = $filter('filter')(vm.employeeCategoriesList, { EmploymentCategoryId: vm.attributes.EmploymentCategoryId })[0];
                vm.employmentStatus = $filter('filter')(vm.statusLookupList, { StatusId: vm.attributes.StatusLookupId })[0];
                vm.licenseClass = $filter('filter')(vm.getLicenseClassList, { LicenseClassId: vm.attributes.LicenseClassId })[0];
                
                //Array
                vm.whiteCards = vm.attributes.DriverWhiteCard;
                vm.inductionCards = vm.attributes.DriverInductionCard;
                vm.vocCards = vm.attributes.DriverVocCard;
                vm.anonymousFields = vm.attributes.AnonymousField;               

                //Image
                vm.licenseImage = vm.attributes.LicenseImage.split(',');                
                if (vm.attributes.ProfilePic !== '') {
                    vm.ProfilePic = CONST.CONFIG.IMG_URL + vm.attributes.ProfilePic;
                    $("#fileName").val(vm.attributes.ProfilePic);
                }               
                $("#previewId").attr('src', vm.ProfilePic);
                vm.getStates(vm.attributes.CountryId);
            });
        } else {
            $("#previewId").attr('src', vm.ProfilePic);
        }
        
        /**
         * @name getAllStates
         * @desc Retrieve factories listing from factory
         * @returns {*}
         */
        function getStates(countryId) {
            UserFactory
             .getAllState(countryId)
             .then(function () {
                 vm.states = UserFactory.states;
             });
        }

        /**
         * @name loadImageFileAsURL
         * @desc Convert Image to base64
         * @returns {*}
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
                    $("#fileName").html(filesSelected[0].name);
                    $('#clearImageId').show();
                };

                fileReader.readAsDataURL(fileToLoad);
            }
        }

        /**
         * @name clearImage
         * @desc For clear image and set as default image
         * @returns {*}
         */
        function clearImage() {
            $("#imageBase64").val('');
            $("#fileName").html('');
            $("#previewId").attr('src', CONST.CONFIG.DEFAULT_IMG);
            $('#clearImageId').hide();
        }

        /**
          * @name loadAttachmentFile
          * @descDisplay multiple Images of Attachement 
          * @returns {*}
          */
        function loadAttachmentFile() {
            vm.licenseImage = '';
            vm.licenseImageSelected = document.getElementById("inputAttachmentFileToLoad").files;
            console.log(vm.licenseImageSelected);
        }
        
        /**
          * @name addWhiteCardRow          
          * @desc Temporary add WhiteCard Data into array               
          */
        vm.addWhiteCardRow = function () {
            console.log(vm.whiteCardsRow);
            if ((vm.whiteCardsRow.issueDate !== '' && typeof vm.whiteCardsRow.issueDate !== 'undefined')
                && (vm.whiteCardsRow.cardNumber !== '' && typeof vm.whiteCardsRow.cardNumber !== 'undefined')) {
                vm.whiteCards.push({
                    'IssueDate': vm.whiteCardsRow.issueDate,
                    'CardNumber': vm.whiteCardsRow.cardNumber,
                    'Notes': vm.whiteCardsRow.note,
                    'IsActive': true
                });
                vm.whiteCardsRow = {                  
                    issueDate: new Date(),
                    cardNumber: '',                   
                    note: ''
                };
                console.log(vm.whiteCards);
            }
        };

        /**
          * @name removeWhiteCardRow          
          * @desc Temporary remove WhiteCard Data into from array               
          */
        vm.removeWhiteCardRow = function (index) {
            vm.whiteCards.splice(index, 1);
        }

        /**
           * @name addInductionRow          
           * @desc Temporary add Induction Data into array               
           */
        vm.addInductionRow = function () {
            //console.log(vm.inductionRow.issueDate, vm.inductionRow.expiryDate);
            if ((vm.inductionRow.siteCost !== '' && typeof vm.inductionRow.siteCost !== 'undefined')
                && (vm.inductionRow.issueDate !== '' && typeof vm.inductionRow.issueDate !== 'undefined')
                && (vm.inductionRow.cardNumber !== '' && typeof vm.inductionRow.cardNumber !== 'undefined')
                && (vm.inductionRow.expiryDate !== '' && typeof vm.inductionRow.expiryDate !== 'undefined')) {
                vm.inductionCards.push({
                    'SiteCost': vm.inductionRow.siteCost,
                    'IssueDate': vm.inductionRow.issueDate,
                    'CardNumber': vm.inductionRow.cardNumber,
                    'ExpiryDate': vm.inductionRow.expiryDate,
                    'Notes': vm.inductionRow.note,
                    'IsActive': true
                });
                vm.inductionRow = {
                    siteCost: '',
                    issueDate: new Date(),
                    cardNumber: '',
                    expiryDate: new Date(),
                    note: ''
                };
            }          
        };

        /**
          * @name removeInductionRow          
          * @desc Temporary remove Induction Data into from array               
          */
        vm.removeInductionRow = function (index) {
            vm.inductionCards.splice(index, 1);
        }

        /**
           * @name vocAddRow          
           * @desc Temporary add VOC Data into array               
           */
        vm.vocAddRow = function () {
            if ((vm.vocRow.issueDate !== '' && typeof vm.vocRow.issueDate !== 'undefined')
                && (vm.vocRow.cardNumber !== '' && typeof vm.vocRow.cardNumber !== 'undefined')
                && (vm.vocRow.rtoNumber !== '' && typeof vm.vocRow.rtoNumber !== 'undefined')) {
                
                vm.vocCards.push({
                    'IssueDate': vm.vocRow.issueDate,
                    'CardNumber': vm.vocRow.cardNumber,
                    'RTONumber': vm.vocRow.rtoNumber,
                    'Notes': vm.vocRow.note,
                    'IsActive': true
                });
                vm.vocRow = {
                    issueDate: new Date(),
                    cardNumber: '',
                    rtoNumber: '',
                    note: ''
                }
                console.log(vm.vocCards);
            }
        };

        /**
          * @name removeVocRow          
          * @desc Temporary remove VOC Data into from array               
          */
        vm.removeVocRow = function (index) {
            vm.vocCards.splice(index, 1);
        }

        /**
          * @name anonymousFieldAddRow          
          * @desc Temporary add nonymousField Data into array               
          */
        vm.anonymousFieldAddRow = function () {
            console.log(vm.anonymousFieldRow);
            if ((vm.anonymousFieldRow.issueDate !== '' && typeof vm.anonymousFieldRow.issueDate !== 'undefined')
                && (vm.anonymousFieldRow.expiryDate !== '' && typeof vm.anonymousFieldRow.expiryDate !== 'undefined')
                && (vm.anonymousFieldRow.title !== '' && typeof vm.anonymousFieldRow.title !== 'undefined')
                && (vm.anonymousFieldRow.otherOne !== '' && typeof vm.anonymousFieldRow.otherOne !== 'undefined')
                && (vm.anonymousFieldRow.otherTwo !== '' && typeof vm.anonymousFieldRow.otherTwo !== 'undefined')) {
                vm.anonymousFields.push({
                    'IssueDate': vm.anonymousFieldRow.issueDate,
                    'ExpiryDate': vm.anonymousFieldRow.expiryDate,
                    'Title': vm.anonymousFieldRow.title,
                    'OtherOne': vm.anonymousFieldRow.otherOne,
                    'OtherTwo': vm.anonymousFieldRow.otherTwo,
                    'Notes': vm.anonymousFieldRow.note,
                    'IsActive': true
                });

                vm.anonymousFieldRow = {
                    issueDate: new Date(),
                    expiryDate: new Date(),
                    title: '',
                    otherOne: '',
                    otherTwo: '',
                    note: ''
                }
            }           
        };

        /**
         * @name removeAnonymousFieldRow          
         * @desc Temporary remove anonymousField Data into from array               
         */
        vm.removeAnonymousFieldRow = function (index) {
            vm.anonymousFields.splice(index, 1);
        }        

        /**
         * @name checkFormValidation
         * @desc check validation and show message according to tab
         * @returns {*}
         */
        vm.checkFormValidation = function (formValid) {
            console.log(formValid);
            if (formValid.$invalid) {
                vm.tabError = [];
                if (formValid.firstName.$invalid
                    || formValid.lastName.$invalid
                    || formValid.email.$invalid
                    || formValid.mobileNumber.$invalid
                    || formValid.country.$invalid
                    || formValid.state.$invalid
                    || formValid.employmentCategory.$invalid
                    || formValid.employmentStatus.$invalid
                    || formValid.licenseNumber.$invalid
                    || formValid.licenseClass.$invalid
                    || formValid.expiryDate.$invalid
                    || formValid.annualLeaveBalance.$invalid
                    || formValid.sickLeaveBalance.$invalid) {
                    vm.tabError.personInformation = true;
                } else {
                    vm.tabError.personInformation = false;
                }
                if (formValid.baseRate.$invalid || formValid.shift.$invalid) {
                    vm.tabError.sensitiveInformation = true;
                } else {
                    vm.tabError.sensitiveInformation = false;
                }
                console.log(vm.tabError);
            }
        }

        /**
          * @name updateDetails
          * @desc Add/Update User detail
          * @returns {*}
          */
        function updateDetails(formValid) {  
            if (angular.element('#imageBase64')[0].value != '') {
                vm.ImageBase64 = angular.element('#imageBase64')[0].value;
                vm.attributes.ImageBase64 = vm.ImageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
            } else {
                vm.attributes.ImageBase64 = '';
            }
            console.log(vm.anonymousFields, vm.whiteCards, vm.employmentCategory);
            vm.attributes.Attachments = vm.licenseImageSelected; //Multi images
            vm.attributes.CountryId = vm.country.Value;
            vm.attributes.StateId = vm.state.Value;
            vm.attributes.LicenseClassId = vm.licenseClass.LicenseClassId;
            vm.attributes.StatusLookupId = vm.employmentStatus.StatusId;
            vm.attributes.EmploymentCategoryId = vm.employmentCategory.EmploymentCategoryId;
            vm.attributes.DriverWhiteCard = vm.whiteCards;
            vm.attributes.DriverWhiteCardCount = (vm.whiteCards.length)? vm.whiteCards.length:0;
            vm.attributes.DriverInductionCard = vm.inductionCards;
            vm.attributes.DriverInductionCardCount = (vm.inductionCards.length)? vm.inductionCards.length:0;
            vm.attributes.DriverVocCard = vm.vocCards;
            vm.attributes.DriverVocCardCount = (vm.vocCards.length)? vm.vocCards.length:0;
            vm.attributes.AnonymousField = vm.anonymousFields;
            vm.attributes.AnonymousFieldCount = (vm.anonymousFields.length)? vm.anonymousFields.length:0;
           
            if (typeof vm.DriverId !== 'undefined' && vm.DriverId !== '') {
                console.log("update", vm.attributes);
                DriverFactory
                 .updateDriver(vm.attributes)
                 .then(function () {
                     $state.go("configuration.DriverList");
                 });

            } else {
                console.log("add", vm.attributes);
                DriverFactory
                 .addDriver(vm.attributes)
                 .then(function () {
                     $state.go("configuration.DriverList");
                 });
            }
        }

        /**
          * @name goDriverList
          * @desc Navigate to Driver list page         
          */
        function goDriverList() {
            $state.go('configuration.DriverList');
        }
    }

    //Inject required stuff as parameters to factories controller function
    FleetTypeListCtrl.$inject = ['$scope', 'ManageFleetTypeFactory', 'UtilsFactory', '$state'];

    /**
     * @name FleetTypeListCtrl
     * @param $scope    
     * @param ManageFleetTypeFactory
     * @param UtilsFactory  
     * @param $state
     */
    function FleetTypeListCtrl($scope, ManageFleetTypeFactory, UtilsFactory, $state) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.assetGroupList = '';

        //methods
        vm.getFleetTypeList = getFleetTypeList;
        vm.deleteFleetType = deleteFleetType;       

        /**
        * @name getAssetGroupList           
        * @desc Get all Asset Groups from db
        * @returns {*}         
        */
        function getFleetTypeList() {          
            ManageFleetTypeFactory
            .getAllFleetType()
            .then(function () {
                vm.assetGroupList = ManageFleetTypeFactory.fleetTypes.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.assetGroupList);               
            });
        }

        //call on Page load
        getFleetTypeList();

        /**
       * @name getAssetGroupList           
       * @desc Get all Asset Groups from db
       * @returns {*}         
       */
        function deleteFleetType(fleetTypeId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageFleetTypeFactory
                        .deleteFleetType(fleetTypeId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }
    }

    //Inject required stuff as parameters to factories controller function
    ManageFleetTypeCtrl.$inject = ['$scope', 'ManageFleetTypeFactory', 'UtilsFactory', '$stateParams', '$state', 'CONST'];

    /**
     * @name ManageFleetTypeCtrl
     * @param $scope    
     * @param ManageFleetTypeFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     */
    function ManageFleetTypeCtrl($scope, ManageFleetTypeFactory, UtilsFactory, $stateParams, $state, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.assetGroupId = '';
        vm.assetGroupDetail = '';
        vm.Image = CONST.CONFIG.DEFAULT_IMG;
        vm.attributes = {
            Fleet: '',
            Description: '',
            ImageBase64: '',
            IsActive: true
        };

        //methods
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.clearImage = clearImage;
        vm.updateDetails = updateDetails;

        // Get detail of Asset Group by Id
        if (typeof $stateParams.FleetTypeId !== 'undefined' && $stateParams.FleetTypeId !== '') {
            vm.assetGroupId = $stateParams.FleetTypeId;

            ManageFleetTypeFactory
            .getFleetTypeDetail(vm.assetGroupId)
            .then(function () {
                vm.attributes = ManageFleetTypeFactory.fleetTypeInfo.DataObject;
                vm.Image = CONST.CONFIG.IMG_URL + vm.attributes.Image;
                $("#fileName").val(vm.attributes.Image);
                $("#previewId").attr('src', CONST.CONFIG.IMG_URL + vm.attributes.Image);
                //console.log(vm.attributes);
            });
        }

        /**
         * @name loadImageFileAsURL
         * @desc Convert Image to base64
         * @returns {*}
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
                    $("#fileName").html(filesSelected[0].name);
                    $('#clearImageId').show();
                };

                fileReader.readAsDataURL(fileToLoad);
            }
        }

        /**
         * @name clearImage
         * @desc For clear image and set as default image
         * @returns {*}
         */
        function clearImage() {
            $("#imageBase64").val('');
            $("#fileName").html('');
            $("#previewId").attr('src', CONST.CONFIG.DEFAULT_IMG);
            $('#clearImageId').hide();
        }

        /**
         * @name updateDetails           
         * @desc Save Asset Group Data into db
         * @returns {*}         
         */

        function updateDetails() {
            if (angular.element('#imageBase64')[0].value != '') {
                vm.ImageBase64 = angular.element('#imageBase64')[0].value;
                vm.attributes.ImageBase64 = vm.ImageBase64.replace(/^data:image\/[a-z]+;base64,/, "");
            } else {
                vm.attributes.ImageBase64 = '';
            }
            //console.log(vm.attributes);
            if (typeof vm.assetGroupId !== 'undefined' && vm.assetGroupId !== '') {
                console.log("update", vm.attributes);
                ManageFleetTypeFactory
                 .updateFleetType(vm.attributes)
                 .then(function () {
                     $state.go("configuration.FleetTypeList");
                 });

            } else {
                console.log("add", vm.attributes);
                ManageFleetTypeFactory
                 .addFleetType(vm.attributes)
                 .then(function () {
                     $state.go("configuration.FleetTypeList");
                 });
            }
        }

    }

    //Inject required stuff as parameters to factories controller function
    WorkTypeCtrl.$inject = ['$scope', 'ManageWorkTypeFactory', 'UtilsFactory', '$state', '$uibModal'];

    /**
     * @name WorkTypeCtrl
     * @param $scope    
     * @param ManageWorkTypeFactory
     * @param UtilsFactory  
     * @param $state
     * @param $uibModal
     */
    function WorkTypeCtrl($scope, ManageWorkTypeFactory, UtilsFactory, $state, $uibModal) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.workTypes = '';
        vm.type = '';
        vm.workTypeId = '';
        vm.attributes = {
            Type: '',
            IsActive: true
        };

        //methods
        vm.getWorkTypeList = getWorkTypeList;
        vm.deleteWorkType = deleteWorkType;
        vm.addWorkType = addWorkType;
        vm.editWorkType = editWorkType;
        vm.hideCancel = hideCancel;
        vm.saveWorkType = saveWorkType;

        /**
        * @name getWorkTypeList           
        * @desc Get all WorkType from db
        * @returns {*}         
        */
        function getWorkTypeList() {          
            ManageWorkTypeFactory
            .getAllWorkType()
            .then(function () {
                vm.getWorkTypeList = ManageWorkTypeFactory.workTypes.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.getWorkTypeList);
                //console.log(vm.getWorkTypeList);
            });
        }

        //call on Page load
        getWorkTypeList();

        /**
         * @name addCheckList
         * @desc callin popup
         */
        function addWorkType() {
            vm.type = '';
            vm.workTypeId = '';
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/CreateWorkType.html',
                scope: $scope,
                controller: 'WorkTypeCtrl',
                controllerAs: 'wtCtrl'
            });
        };

        /**
         * @name hideCancel
         * @desc Hide popup
         */
        function hideCancel() {
            $scope.modalInstance.dismiss();
        }

        /**
        * @name editCheckList
        * @desc callin popup
        */
        function editWorkType(data) {
            //console.log(data);
            vm.type = data.Type;
            vm.workTypeId = data.WorkTypeId;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/CreateWorkType.html',
                scope: $scope
            });
        };

        /**
           * @name saveWorkType
           * @desc Save Data
           */
        function saveWorkType() {
            console.log(vm.attributes);
            if (vm.workTypeId === '') {
                vm.attributes.Type = vm.type;
                ManageWorkTypeFactory
                .addWorkType(vm.attributes)
                .then(function () {
                    $scope.modalInstance.dismiss();
                    getWorkTypeList();
                });
            } else {
                vm.attributes.Type = vm.type;
                vm.attributes.WorkTypeId = vm.workTypeId;
                ManageWorkTypeFactory
                .updateWorkType(vm.attributes)
                .then(function () {
                    $scope.modalInstance.dismiss();
                    getWorkTypeList();
                });
            }
        }

        /**
           * @name deleteWorkType           
           * @desc Delete Record from db
           * @returns {*}         
           */
        function deleteWorkType(workTypeId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageWorkTypeFactory
                        .deleteWorkType(workTypeId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }
    }

    //Inject required stuff as parameters to factories controller function
    CheckListCtrl.$inject = ['$scope', 'ManageCheckListFactory', 'UtilsFactory', '$state', '$uibModal'];

    /**
     * @name CheckListCtrl
     * @param $scope    
     * @param ManageCheckListFactory
     * @param UtilsFactory  
     * @param $state
     * @param $uibModal
     */
    function CheckListCtrl($scope, ManageCheckListFactory, UtilsFactory, $state, $uibModal) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.checkList = '';
        vm.title = '';
        vm.docketCheckListId = '';
        vm.attributes = {
            Title: '',
            IsActive: true
        };

        //methods
        vm.getCheckList = getCheckList;
        vm.deleteCheckList = deleteCheckList;
        vm.addCheckList = addCheckList;
        vm.editCheckList = editCheckList;
        vm.hideCancel = hideCancel;
        vm.saveCheckList = saveCheckList;

        /**
        * @name getCheckList           
        * @desc Get all CheckList from db
        * @returns {*}         
        */
        function getCheckList() {           
            ManageCheckListFactory
            .getAllCheckList()
            .then(function () {
                vm.checkList = ManageCheckListFactory.checkLists.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.checkList);                
            });
        }

        //call on Page load
        getCheckList();

        /**
         * @name addCheckList
         * @desc callin popup
         */
        function addCheckList() {
            vm.title = '';
            vm.docketCheckListId = '';
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/CreateCheckList.html',
                scope: $scope,
                controller: 'CheckListCtrl',
                controllerAs: 'clCtrl'
                //controller: ModalInstanceCtrl,
            });
        };

        /**
         * @name hideCancel
         * @desc Hide popup
         */
        function hideCancel() {
            $scope.modalInstance.dismiss();
        }

        /**
        * @name editCheckList
        * @desc callin popup
        */
        function editCheckList(data) {
            vm.title = data.Title;
            vm.docketCheckListId = data.DocketCheckListId;
            //vm.attributes = data;            
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/CreateCheckList.html',
                scope: $scope
            });
        };

        /**
           * @name saveCheckList
           * @desc Save Data
           */
        function saveCheckList() {           
            if (vm.docketCheckListId === '') {
                vm.attributes.Title = vm.title;
                ManageCheckListFactory
                .addCheckList(vm.attributes)
                .then(function () {
                    $scope.modalInstance.dismiss();
                    getCheckList();
                });
            } else {
                vm.attributes.Title = vm.title;
                vm.attributes.DocketCheckListId = vm.docketCheckListId;
                ManageCheckListFactory
                .updateCheckList(vm.attributes)
                .then(function () {
                    $scope.modalInstance.dismiss();
                    getCheckList();
                    //$state.reload();
                });
            }
        }

        /**
       * @name deleteCheckList           
       * @desc Get all Asset Groups from db
       * @returns {*}         
       */
        function deleteCheckList(checkListId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageCheckListFactory
                        .deleteCheckList(checkListId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }
    }

    //Inject required stuff as parameters to factories controller function
    CustomerListCtrl.$inject = ['$scope', 'ManageCustomerFactory', 'UtilsFactory', '$state', '$uibModal'];

    /**
     * @name CustomerListCtrl
     * @param $scope    
     * @param ManageCustomerFactory
     * @param UtilsFactory  
     * @param $state
     * @param $uibModal
     */
    function CustomerListCtrl($scope, ManageCustomerFactory, UtilsFactory, $state, $uibModal) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.customerList = '';
            
        //methods
        vm.getCustomerList = getCustomerList;
        vm.deleteCustomer = deleteCustomer;      

        /**
        * @name getCustomerList           
        * @desc Get all Customer from db
        * @returns {*}         
        */
        function getCustomerList() {           
            ManageCustomerFactory
            .getAllCustomer()
            .then(function () {
                vm.customerList = ManageCustomerFactory.customerList.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.customerList);                
            });
        }

        //call on Page load
        getCustomerList();

        /**
       * @name deleteCustomer           
       * @desc Delete customer from db
       * @returns {*}         
       */
        function deleteCustomer(customerId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageCustomerFactory
                        .deleteCustomer(customerId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }
    }

    //Inject required stuff as parameters to factories controller function
    ManageCustomerCtrl.$inject = ['$scope', 'ManageCustomerFactory', 'UtilsFactory', '$stateParams', '$state', 'CONST'];

    /**
     * @name ManageCustomerCtrl
     * @param $scope    
     * @param ManageCustomerFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     */
    function ManageCustomerCtrl($scope, ManageCustomerFactory, UtilsFactory, $stateParams, $state, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.siteTabDisabled = true;
        vm.hideTab = false;
        vm.customerId = '';
        vm.customerDetail = '';
        vm.filesSelected = '';
        vm.tabs = { active: true, active: false };

        vm.attributes = {
            ABN: '',
            AccountsContact: '',       
            AccountsNumber: '',
            ContactName: '',
            ContactNumber:'',
            CustomerId: '',
            CustomerName: '',
            EmailForInvoices: '',
            Fax: '',
            MobileNumber1: '',
            MobileNumber2: '',
            OfficePostalCode: '',
            OfficeState: '',
            OfficeStreet: '',
            OfficeSuburb: '',
            PhoneNumber1: '',
            PhoneNumber2: '',
            PostalPostCode: '',
            PostalStreetPoBox: '',
            PostalSuburb: '',
            SiteDetail:'',
            IsActive: true
        };

        //for multiple Site detail object
        //vm.siteDetailRow = {
        //    siteName: '',
        //    poNumber: '',
        //    siteDetails: '',
        //    fuelIncluded: '',
        //    tollsIncluded: '',
        //    creditTermsAgreed: '',
        //    attachment: '',
        //    supervisor : [],
        //    gate: [],
        //    contactPerson: [],          
        //}
        //vm.siteDetails = [];

        //Site detail Attribute
        vm.siteDetails = {
            SiteName: '',
            PoNumber: '',
            SiteDetails: '',
            FuelIncluded: false,
            TollsIncluded: false,
            CreditTermsAgreed: '',           
            SupervisorList: [],
            GateList: [],
            IsActive: true
        };

        //Supervisor object
        vm.supervisorRow = {
            supervisorName: '',
            supervisorEmail: '',
            supervisorMobile: '',           
        }
        vm.supervisors = [];

        //Gate object
        vm.gateRow = {
            gateNumber: '',
            // equipmentType: '',
            tipOffRate: '',
            tippingSite: '',
            contactPerson: [],           
        }
        vm.gates = [];

        //Contact object
        //vm.contactPersonRow = {
        //    contactPersonName: '',
        //    contactPersonEmail: '',
        //    contactPersonMobile: '',            
        //}
        //vm.contactPersons = [];

        vm.creditTermAgreedList = [30, 60, 90];

        //methods       
        vm.updateDetails = updateDetails;
        vm.goCustomerList = goCustomerList;
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.updateSiteDetails = updateSiteDetails;

        /**
          * @name addSupervisorRow          
          * @desc Temporary add Supervisor Data into array               
          */
        vm.addSupervisorRow = function () {
            console.log(vm.supervisorRow.supervisorEmail, vm.supervisorRow.supervisorMobile);
            if ((vm.supervisorRow.supervisorName !== '' && typeof vm.supervisorRow.supervisorName !== 'undefined')
                && (vm.supervisorRow.supervisorEmail !== '' && typeof vm.supervisorRow.supervisorEmail !== 'undefined')
                && (vm.supervisorRow.supervisorMobile !== '' && typeof vm.supervisorRow.supervisorMobile !== 'undefined')) {
                vm.supervisors.push({
                    'SupervisorName': vm.supervisorRow.supervisorName,
                    'SupervisorEmail': vm.supervisorRow.supervisorEmail,
                    'SupervisorMobile': vm.supervisorRow.supervisorMobile,
                    'IsActive': true
                });
                //Supervisor object
                vm.supervisorRow = {
                    supervisorName: '',
                    supervisorEmail: '',
                    supervisorMobile: '',
                }
                console.log(vm.supervisors);
            }
        };

        /**
          * @name removeSupervisorRow          
          * @desc Temporary remove Supervisor Data into from array               
          */
        vm.removeSupervisorRow = function (index) {
            vm.supervisors.splice(index, 1);
        }

        /**
         * @name addGateRow          
         * @desc Temporary add Contact Data into array               
         */
        vm.addGateRow = function () {
            console.log("gate", vm.gateRow);
                     
            if ((vm.gateRow.gateNumber !== '' && typeof vm.gateRow.gateNumber !== 'undefined')
                && (vm.gateRow.tipOffRate !== '' && typeof vm.gateRow.tipOffRate !== 'undefined')
                && (vm.gateRow.tippingSite !== '' && typeof vm.gateRow.tippingSite !== 'undefined')) {
                vm.gates.push({
                    'GateNumber': vm.gateRow.gateNumber,
                    //'EquipmentType': vm.gateRow.equipmentType,
                    'TipOffRate': vm.gateRow.tipOffRate,
                    'TippingSite': vm.gateRow.tippingSite,
                    // 'contactPerson': vm.contactPersons,
                    'IsActive': true
                });

                vm.gateRow = {
                    gateNumber: '',
                    // equipmentType: '',
                    tipOffRate: '',
                    tippingSite: '',
                    // contactPerson: [],
                }

               // vm.contactPersons = [];
                console.log(vm.gates);
            }
        };

        /**
          * @name removeGateRow          
          * @desc Temporary remove Gate Data into from array               
          */
        vm.removeGateRow = function (index) {
            vm.gates.splice(index, 1);
        }

        /**
          * @name addContactPersonRow          
          * @desc Temporary add Contact Data into array               
          */
        //vm.addContactPersonRow = function () {
        //    //console.log(vm.inductionRow.issueDate, vm.inductionRow.expiryDate);
        //    if (vm.contactPersonRow.contactPersonName !== '' && vm.contactPersonRow.contactPersonEmail !== '' && vm.contactPersonRow.contactPersonMobile !== '') {
        //        vm.contactPersons.push({
        //            'ContactPersonName': vm.contactPersonRow.contactPersonName,
        //            'ContactPersonEmail': vm.contactPersonRow.contactPersonEmail,
        //            'ContactPersonMobile': vm.contactPersonRow.contactPersonMobile,
        //            'IsActive': true
        //        });
        //        vm.contactPersonRow = {
        //            contactPersonName: '',
        //            contactPersonEmail: '',
        //            contactPersonMobile: '',
        //        }
        //    }
        //};

        /**
          * @name removeContactRow          
          * @desc Temporary remove Contact Data into from array               
          */
        //vm.removeContactRow = function (index) {
        //    vm.contactPersons.splice(index, 1);
        //}

        /**
        * @name addSiteDetailRow          
        * @desc Temporary add SiteDetail Data into array               
        */
        //vm.addSiteDetailRow = function () {
        //    //console.log(vm.inductionRow.issueDate, vm.inductionRow.expiryDate);

        //    if (vm.siteDetailRow.siteName !== '' && vm.siteDetailRow.creditTermsAgreed !== '') {
        //        vm.siteDetails.push({
        //            'SiteName': vm.siteDetailRow.siteName,
        //            'PoNumber': vm.siteDetailRow.poNumber,
        //            'SiteDetails': vm.siteDetailRow.siteDetails,
        //            'FuelIncluded': vm.siteDetailRow.fuelIncluded,
        //            'TollsIncluded': vm.siteDetailRow.tollsIncluded,
        //            'CreditTermsAgreed': vm.siteDetailRow.creditTermsAgreed,
        //            'Attachment': '',
        //            'Supervisor ': vm.supervisors,
        //            'Gate': vm.gates,
        //            //'ContactPerson': vm.contactPersons,
        //            'IsActive': true
        //        });                
        //        vm.siteDetailRow = {
        //            siteName: '',
        //            poNumber: '',
        //            siteDetails: '',
        //            fuelIncluded: '',
        //            tollsIncluded: '',
        //            creditTermsAgreed: '',
        //            attachment: '',
        //            supervisor: [],
        //            gate: [],
        //            contactPerson: [],
        //        }

        //        //Remove all data from child Arrays
        //        vm.supervisorRow = [];
        //        vm.gateRow = [];
        //        //vm.contactPersons = [];
        //        console.log(vm.siteDetail);
        //    }
        //};

        /**
          * @name removeSiteDetailRow          
          * @desc Temporary remove SiteDetail Data into from array               
          */
        //vm.removeSiteDetailRow = function (index) {
        //    vm.siteDetail.splice(index, 1);
        //}

        // Get detail of Customer by Id
        if (typeof $stateParams.CustomerId !== 'undefined' && $stateParams.CustomerId !== '') {

            vm.customerId = $stateParams.CustomerId;
            vm.siteTabDisabled = false;
            vm.hideTab = true;
            ManageCustomerFactory
            .getCustomerByCustomerId(vm.customerId)
            .then(function () {
              
                vm.attributes = ManageCustomerFactory.customerInfo.DataObject;               
                vm.attributes.ABN = parseInt(vm.attributes.ABN);
                vm.attributes.ContactNumber = (vm.attributes.ContactNumber !== null)?parseInt(vm.attributes.ContactNumber):'';//Handle null value
                vm.attributes.AccountsNumber = parseInt(vm.attributes.AccountsNumber);
                vm.attributes.PhoneNumber1 = (vm.attributes.PhoneNumber1 !== null) ? parseInt(vm.attributes.PhoneNumber1) : '';//Handle null value
                vm.attributes.PhoneNumber2 = (vm.attributes.PhoneNumber2 !== null) ? parseInt(vm.attributes.PhoneNumber2) : '';//Handle null value
                vm.attributes.MobileNumber1 = (vm.attributes.MobileNumber1 !== null) ? parseInt(vm.attributes.MobileNumber1) : '';//Handle null value
                vm.attributes.MobileNumber2 = (vm.attributes.MobileNumber2 !== null) ? parseInt(vm.attributes.MobileNumber2) : '';//Handle null value
                vm.attributes.Fax = (vm.attributes.Fax !== null) ? parseInt(vm.attributes.Fax) : '';//Handle null value
                vm.attributes.OfficePostalCode = (vm.attributes.OfficePostalCode !== null) ? parseInt(vm.attributes.OfficePostalCode) : '';//Handle null value
                vm.attributes.PostalPostCode = (vm.attributes.PostalPostCode !== null) ? parseInt(vm.attributes.PostalPostCode) : ''; // Handle null value           
                console.log(vm.attributes);

            });
        }

       
        /**
        * @name loadImageFileAsURL
        * @descDisplay Image name 
        * @returns {*}
        */
        function loadImageFileAsURL() {
            vm.filesSelected = document.getElementById("inputFileToLoad").files;
            console.log(vm.filesSelected);
        }

        /**
         * @name updateDetails          
         * @desc Save Customer Data into db
         * @returns {*}         
         */
        function updateDetails() {                      
            if (vm.customerId !== 'undefined' && vm.customerId !== '') {

                vm.attributes.CustomerId = vm.customerId;
                vm.attributes.IsActive = true;               

                ManageCustomerFactory
                 .updateCustomer(vm.attributes)
                 .then(function () {
                     //$state.go("configuration.CustomerList");
                 });

            } else {

                ManageCustomerFactory
                 .addCustomer(vm.attributes)
                 .then(function (response) {
                     console.log(response.data.Id);
                     if (response.data.Id) {
                         vm.siteTabDisabled = false;
                         vm.customerId = response.data.Id;
                         vm.tabs[0].active = false;
                         vm.tabs[1].active = true;
                     }
                    
                     //$state.go("configuration.CustomerList");
                 });
            }
        }
        
        /**
        * @name updateSiteDetails          
        * @desc Save SiteDetail Data into db
        * @returns {*}         
        */
        function updateSiteDetails() {
            if (vm.customerId !== 'undefined' && vm.customerId !== '') {

                //vm.siteDetails.push({
                //    'CustomerId' : vm.customerId,
                //    'SiteName': vm.siteDetailRow.siteName,
                //    'PoNumber': vm.siteDetailRow.poNumber,
                //    'SiteDetails': vm.siteDetailRow.siteDetails,
                //    'FuelIncluded': vm.siteDetailRow.fuelIncluded,
                //    'TollsIncluded': vm.siteDetailRow.tollsIncluded,
                //    'CreditTermsAgreed': vm.siteDetailRow.creditTermsAgreed,                   
                //    'Supervisor ': vm.supervisors,
                //    'Gate': vm.gates,
                //    'ContactPerson': vm.contactPersons,
                //    'files':vm.filesSelected,
                //    'IsActive': true
                //});

                vm.siteDetails.files = vm.filesSelected;
                vm.siteDetails.CustomerId = vm.customerId;                                 
                vm.siteDetails.SupervisorList =  vm.supervisors;
                vm.siteDetails.GateList = vm.gates;
                vm.siteDetails.SupervisorCount = vm.supervisors.length;
                vm.siteDetails.GateCount = vm.gates.length;
               
                ManageCustomerFactory
                .addCustomerSite(vm.siteDetails)
                .then(function () {
                    $state.go("configuration.CustomerList");
                });
            }
        }

        /**
        * @name goCustomerList          
        * @desc Navigate to customer list page
        * @returns {*}         
        */
        function goCustomerList() {
            $state.go("configuration.CustomerList");
        }
    }

    //Inject required stuff as parameters to factories controller function
    SiteListCtrl.$inject = ['$scope', 'ManageSiteFactory', 'BCAFactory', 'UtilsFactory', '$stateParams', '$state', 'CONST'];

    /**
     * @name SiteListCtrl
     * @param $scope    
     * @param ManageSiteFactory
     * @param BCAFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     */
    function SiteListCtrl($scope, ManageSiteFactory, BCAFactory, UtilsFactory, $stateParams, $state, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.customer = '';
        vm.siteList = '';
        vm.customerList = '';
        vm.bcaTableParams = UtilsFactory.bcaTableOptions('');

        //methods
        vm.getCustomerList = getCustomerList;
        vm.changeCustomer = changeCustomer;
        vm.getSiteList = getSiteList;
        vm.deleteSite = deleteSite;

        /**
           * @name getCustomerList           
           * @desc Get all Customer from db
           * @returns {*}         
           */
        function getCustomerList() {
            BCAFactory.getCustomersDDL()
            .then(function () {
                vm.customerList = BCAFactory.customerList.DataList;               
            });
        }

        //call On page load
        vm.getCustomerList();

        /**
        * @name changeCustomer           
        * @desc Get list of Site according to selected customer from db
        * @returns {*}         
        */
        function changeCustomer(coustomerId) {           
            if (typeof coustomerId !== 'undefined') {              
                vm.getSiteList(coustomerId);
            } else {
                vm.bcaTableParams = UtilsFactory.bcaTableOptions('');
            }
        }

        /**
        * @name getSiteList           
        * @desc Get all Site from db
        * @returns {*}         
        */
        function getSiteList(coustomerId) {
            ManageSiteFactory
            .getAllSite(coustomerId)
            .then(function () {
                vm.siteList = ManageSiteFactory.siteList.DataList;
                console.log(vm.siteList);
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.siteList);             
              
            });
        }

        //Call on page load for default all Site listing
        vm.getSiteList(0);

        /**
       * @name deleteSite           
       * @desc delete Site from db
       * @returns {*}         
       */
        function deleteSite(siteId) {

            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageSiteFactory
                        .deleteSite(siteId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }

    }

    //Inject required stuff as parameters to factories controller function
    ManageSiteCtrl.$inject = ['$scope', 'ManageSiteFactory', 'UtilsFactory', '$stateParams', '$state', 'CONST', '$filter'];

    /**
     * @name ManageSiteCtrl
     * @param $scope    
     * @param ManageSiteFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     */
    function ManageSiteCtrl($scope, ManageSiteFactory, UtilsFactory, $stateParams, $state, CONST, $filter) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.customerId = '';
        vm.siteId = '';
        vm.siteDetail = '';
        vm.attributes = {
            SiteName: '',
            PoNumber: '',
            SiteDetail: '',
            FuelIncluded: false,
            TollTax: false,
            CreditTermAgreed: '',
            ImageBase64: '',
            Image: '',
            files: '',
            IsActive: true,
            CreatedBy: '',
            EditedBy: '',
        };
        vm.filesSelected = '';
        vm.CreditTermAgreedList = [30, 60, 90];

        //methods   
        vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.updateDetails = updateDetails;
        vm.cancel = cancel;

        // Get detail of Site by Id
        if (typeof $stateParams.SiteId !== 'undefined' && $stateParams.SiteId !== '') {
            vm.siteId = $stateParams.SiteId;           
            ManageSiteFactory
            .getSiteDetail(vm.siteId)
            .then(function () {
                vm.attributes = ManageSiteFactory.siteInfo.DataObject;               
                vm.attributes.CreditTermAgreed = parseInt(vm.attributes.CreditTermAgreed);               
            });
        }

        /**
           * @name loadImageFileAsURL
           * @descDisplay Image name 
           * @returns {*}
           */       
        function loadImageFileAsURL() {
            vm.filesSelected = '';
            vm.filesSelected = document.getElementById("inputFileToLoad").files;
          
            if (typeof vm.filesSelected.length !== 0) {
               
                vm.attributes.Image = vm.filesSelected[0].name;
            }           
            console.log(vm.filesSelected);
         }
        
        /**
         * @name updateDetails          
         * @desc Save Site Data into db
         * @returns {*}         
         */
        function updateDetails() {
            vm.attributes.files = vm.filesSelected;
            vm.attributes.CreatedBy = '';
            vm.attributes.EditedBy = '';
            if (typeof $stateParams.SiteId !== 'undefined' && $stateParams.SiteId !== '') {
                vm.attributes.SiteId = $stateParams.SiteId;
                vm.attributes.IsActive = true;
                console.log(vm.attributes);
                ManageSiteFactory
                 .updateSite(vm.attributes)
                 .then(function () {
                     $state.go("configuration.SiteList");
                 });

            } else {
                ManageSiteFactory
                 .addSite(vm.attributes)
                 .then(function () {
                     $state.go("configuration.SiteList");
                 });
            }
        }

        /**
         * @name hideCancel          
         * @desc Go to list page
         * @returns {*}         
         */
        function cancel() {
            $state.go('configuration.SiteList');
        }
    }

    //Inject required stuff as parameters to factories controller function
    SupervisorCtrl.$inject = ['$scope', 'ManageSupervisorFactory', 'BCAFactory', 'UtilsFactory', '$stateParams', '$state', 'CONST', '$uibModal'];

    /**
     * @name SupervisorCtrl
     * @param $scope    
     * @param ManageSupervisorFactory
     * @param BCAFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     * @param CONST
     * @param $uibModal
     */
    function SupervisorCtrl($scope, ManageSupervisorFactory, BCAFactory, UtilsFactory, $stateParams, $state, CONST, $uibModal) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.customer = '';
        vm.site = '';
        vm.customerList = '';
        vm.siteList = '';
        vm.supervisorList = '';
        vm.attributes = {
            SupervisorName: '',
            Email : '',
            MobileNumber: '',
            IsActive: true
        }
        vm.bcaTableParams = UtilsFactory.bcaTableOptions('');

        //methods
        vm.getCustomerList = getCustomerList;
        vm.getSiteList = getSiteList;
        vm.changeCustomer = changeCustomer;
        vm.changeSite = changeSite;
        vm.editSupervisor = editSupervisor;
        vm.getSupervisorList = getSupervisorList;
        vm.saveSupervisor = saveSupervisor;
        vm.deleteSupervisor = deleteSupervisor;
        vm.hideCancel = hideCancel;

        /**
           * @name getCustomerList           
           * @desc Get all Customer from db
           * @returns {*}         
           */
        function getCustomerList() {
            BCAFactory.getCustomersDDL()
            .then(function () {
                vm.customerList = BCAFactory.customerList.DataList;               
            });
        }

        /**
          * @name getSiteList           
          * @desc Get list of site by customer Id from db
          * @returns {*}         
          */
        function getSiteList(customerId) {
            BCAFactory.getSitesByCustomerIdDDL(customerId)
            .then(function () {
                vm.siteList = BCAFactory.siteList.DataList;              
            });
        }

        /**
         * @name getSupervisorList           
         * @desc Get list of supervisor by site Id from db
         * @returns {*}         
         */
        function getSupervisorList(siteId) {
            ManageSupervisorFactory.getAllSupervisor(siteId)
            .then(function () {
                vm.supervisorList = ManageSupervisorFactory.supervisorList.DataList;
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.supervisorList);                
            });
        }

        /**
        * @name changeCustomer           
        * @desc Get list of Site according to selected customer from db
        * @returns {*}         
        */
        function changeCustomer(coustomerId) {
            vm.bcaTableParams = UtilsFactory.bcaTableOptions('');
            if (typeof coustomerId !== 'undefined') {
                vm.getSiteList(coustomerId);
            }
        } 

        /**
           * @name changeSupervisor           
           * @desc Get list of changeSite according to selected site from db
           * @returns {*}         
           */
        function changeSite(siteId) {
            if (typeof siteId !== 'undefined') {
                vm.getSupervisorList(siteId);
            } else {
                vm.bcaTableParams = UtilsFactory.bcaTableOptions('');
            }
        }

        //call On page load
        vm.getCustomerList();

        //call on page load for default listing of supervisor
        vm.getSupervisorList(0);

        /**
       * @name editSupervisor
       * @desc calling popup
       */
        function editSupervisor(data) {
            //console.log(data);
            vm.supervisorName = data.SupervisorName;
            vm.email = data.Email;
            vm.mobileNumber = parseInt(data.MobileNumber);
            vm.supervisorId = data.SupervisorId;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'views/configuration/partial/CreateSupervisor.html',
                scope: $scope
            });
        };

        /**
           * @name saveSupervisor
           * @desc Save Data
           */
        function saveSupervisor() {
                    
            vm.attributes.SupervisorName = vm.supervisorName;
            vm.attributes.Email = vm.email;
            vm.attributes.MobileNumber = vm.mobileNumber;
            vm.attributes.SupervisorId = vm.supervisorId;
            console.log(vm.attributes);
            ManageSupervisorFactory
            .updateSupervisor(vm.attributes)
            .then(function () {
                $state.reload();
            });           
        }

        /**
       * @name deleteSupervisor           
       * @desc delete Supervisor from db
       * @returns {*}         
       */
        function deleteSupervisor(supervisorId) {
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageSupervisorFactory
                        .deleteSupervisor(supervisorId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });

        }

        /**
          * @name hideCancel
          * @desc Hide popup
          */
        function hideCancel() {
            $scope.modalInstance.dismiss();
        }
    }
   
    //Inject required stuff as parameters to factories controller function
    GateListCtrl.$inject = ['$scope', 'ManageGateFactory', 'BCAFactory', 'UtilsFactory', '$state', 'CONST'];

    /**
     * @name GateListCtrl
     * @param $scope    
     * @param ManageGateFactory
     * @param BCAFactory
     * @param UtilsFactory     
     * @param $state
     */
    function GateListCtrl($scope, ManageGateFactory, BCAFactory, UtilsFactory, $state, CONST) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.gateList = '';
        vm.attributes = {
            SupervisorName: '',
            Email: '',
            MobileNumber: '',
            IsActive: true
        }
        vm.bcaTableParams = UtilsFactory.bcaTableOptions('');

        //methods
        vm.getGateList = getGateList;
        vm.getCustomerList = getCustomerList;
        vm.getSiteList = getSiteList;
        vm.changeCustomer = changeCustomer;
        vm.changeSite = changeSite;
        vm.deleteGate = deleteGate;        

        /**
         * @name getCustomerList           
         * @desc Get all Customer from db
         * @returns {*}         
         */
        function getCustomerList() {
            BCAFactory.getCustomersDDL()
            .then(function () {
                vm.customerList = BCAFactory.customerList.DataList;
            });
        }

        /**
          * @name getSiteList           
          * @desc Get list of site by customer Id from db
          * @returns {*}         
          */
        function getSiteList(customerId) {
            BCAFactory.getSitesByCustomerIdDDL(customerId)
            .then(function () {
                vm.siteList = BCAFactory.siteList.DataList;
            });
        }

        /**
         * @name getGateList           
         * @desc Get all Gate from db
         * @returns {*}         
         */
        function getGateList(siteId) {
            ManageGateFactory.getAllGate(siteId)
            .then(function () {
                vm.gateList = ManageGateFactory.gateList.DataList;               
                vm.bcaTableParams = UtilsFactory.bcaTableOptions(vm.gateList);
            });
        }

        /**
        * @name changeCustomer           
        * @desc Get list of Site according to selected customer from db
        * @returns {*}         
        */
        function changeCustomer(coustomerId) {
            vm.bcaTableParams = UtilsFactory.bcaTableOptions('');
            if (typeof coustomerId !== 'undefined') {
                vm.getSiteList(coustomerId);
            }
        }

        /**
           * @name changeSupervisor           
           * @desc Get list of changeSite according to selected site from db
           * @returns {*}         
           */
        function changeSite(siteId) {
            if (typeof siteId !== 'undefined') {
                vm.getGateList(siteId);
            } else {
                vm.bcaTableParams = UtilsFactory.bcaTableOptions('');
            }
        }

        //call On page load
        vm.getCustomerList();
        
        //Call on page Load
        vm.getGateList(0);       

        /**
           * @name deleteGate           
           * @desc delete Gate from db
           * @returns {*}         
           */
        function deleteGate(gateId) {
            UtilsFactory.confirmBox('Confirm', 'Are you sure to delete record?', function (isConfirm) {
                if (isConfirm) {
                    //Call UserFactory factory get all factories data
                    ManageGateFactory
                        .deleteGate(gateId)
                        .then(function () {
                            $state.reload();
                        });
                }
            });
        }

    }

    //Inject required stuff as parameters to factories controller function
    ManageGateCtrl.$inject = ['$scope', 'ManageGateFactory', 'ManageBookingSiteFactory', 'UtilsFactory', '$stateParams', '$state', '$uibModal'];

    /**
     * @name ManageGateCtrl
     * @param $scope    
     * @param ManageGateFactory
     * @param UtilsFactory    
     * @param $stateParams
     * @param $state
     * @param $uibModal
     */
    function ManageGateCtrl($scope, ManageGateFactory, ManageBookingSiteFactory, UtilsFactory, $stateParams, $state, $uibModal) {
        //Assign controller scope pointer to a variable
        var vm = this;
        vm.customerId = '';
        vm.siteId = '';
        vm.gateId = '';
        vm.gateDetail = '';
        vm.addContact = [];

        // Gate Attributes
        vm.attributes = {
            GateNumber: '',
            TipOffRate: '',
            TippingSite: '', 
            GateContactPerson: '',
            IsActive: true,
            CreatedBy: '',
            EditedBy: ''
        };

        //Contact Attributes
        vm.contactAttributes = {
            ContactPerson: '',
            MobileNumber: '',
            Email: '',
            IsActive: true,
            CreatedBy: '',
            EditedBy: ''
        };
            
      
        //methods   
        //vm.loadImageFileAsURL = loadImageFileAsURL;
        vm.updateDetails = updateDetails;        
        vm.addContactModal = addContactModal;
        vm.editContactModal = editContactModal;
        vm.saveContact = saveContact;
        vm.cancelGate = cancelGate;
        vm.hideCancel = hideCancel;

        // Get detail of Site by Id
        if (typeof $stateParams.GateId !== 'undefined' && $stateParams.GateId !== '') {
            vm.gateId = $stateParams.GateId;
            ManageGateFactory
            .getGateDetail(vm.gateId)
            .then(function () {
                vm.attributes = ManageGateFactory.gateInfo.DataObject;
                console.log(vm.attributes);
                vm.addContact = vm.attributes.GateContactPerson;
            });
        }        

        /**
         * @name addContactModal
         * @desc calling popup
         */
        function addContactModal() {
            // vm.contact.GateContactPersonId = '';
            vm.contactAttributes.GateId = vm.gateId;
            vm.contactAttributes.ContactPerson = '';
            vm.contactAttributes.MobileNumber = '';
            vm.contactAttributes.Email = '';
            vm.contactAttributes.IsActive = true;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/configuration/partial/CreateContact.html',
                scope: $scope
                //controller: ModalInstanceCtrl,
            });
        };

        /**
        * @name editContactModal
        * @desc calling popup
        */
        function editContactModal(contactData) {
            vm.contactAttributes.GateContactPersonId = contactData.GateContactPersonId;
            vm.contactAttributes.GateId = contactData.GateId;
            vm.contactAttributes.ContactPerson = contactData.ContactPerson;
            vm.contactAttributes.MobileNumber = parseInt(contactData.MobileNumber);
            vm.contactAttributes.Email = contactData.Email;
            vm.contactAttributes.IsActive = true;
            $scope.modalInstance = $uibModal.open({
                templateUrl: 'Views/configuration/partial/CreateContact.html',
                scope: $scope
                //controller: ModalInstanceCtrl,
            });
        }

        /**
         * @name updateDetails          
         * @desc Save Site Data into db
         * @returns {*}         
         */
        function updateDetails() {
            vm.attributes.GateContactPerson = vm.addContact;
            vm.attributes.CreatedBy = '';
            vm.attributes.EditedBy = '';
            if (typeof $stateParams.GateId !== 'undefined' && $stateParams.GateId !== '') {
                vm.attributes.GateId = $stateParams.GateId;
                vm.attributes.IsActive = true;
                ManageGateFactory
                 .updateGate(vm.attributes)
                 .then(function () {
                     $state.go("configuration.GateList");
                 });

            } else {
                ManageGateFactory
                 .addGate(vm.attributes)
                 .then(function () {
                     $state.go("configuration.GateList");
                 });
            }
        }

       /**
        * @name saveContact          
        * @desc Save Contact Data into DB               
        */
        function saveContact() {
            if (typeof vm.contactAttributes.GateContactPersonId !== 'undefined') {
                ManageBookingSiteFactory
               .updateGateContactPerson(vm.contactAttributes)
               .then(function () {
                   $state.reload();
               });
            } else {
                ManageBookingSiteFactory
               .addGateContactPerson(vm.contactAttributes)
               .then(function () {
                   $state.reload();
               });
            }
           
            //vm.addContact.push({                
            //    'ContactPerson': vm.contactAttributes.ContactPerson,
            //    'Email': vm.contactAttributes.Email,
            //    'MobileNumber': vm.contactAttributes.MobileNumber,
            //    'IsActive': vm.contactAttributes.IsActive,
            //    'CreatedBy': '',
            //    'EditedBy': ''
            //});

            //Dismiss modal after storing data into a temp array
            vm.hideCancel();
            console.log(vm.addContact);
        }       

        /**
       * @name updateContact          
       * @desc Update Contact Data into DB               
       */
        function updateContact() {

            ManageBookingSiteFactory
            .updateGateContactPerson(vm.contactAttributes)
            .then(function () {
                $scope.reload();
            });
            //vm.addContact.push({                
            //    'ContactPerson': vm.contactAttributes.ContactPerson,
            //    'Email': vm.contactAttributes.Email,
            //    'MobileNumber': vm.contactAttributes.MobileNumber,
            //    'IsActive': vm.contactAttributes.IsActive,
            //    'CreatedBy': '',
            //    'EditedBy': ''
            //});

            //Dismiss modal after storing data into a temp array
            vm.hideCancel();
            console.log(vm.addContact);
        }

        /**
        * @name removeContactRow          
        * @desc Temporary remove Contact Data into from array               
        */
        vm.removeContactRow = function (index) {
            vm.addContact.splice(index, 1);           
        }     

        /**
        * @name cancelGate          
        * @desc Dismiss contact popup
        * @returns {*}         
        */
        function cancelGate() {
            $state.go("configuration.GateList");
        }

        /**
         * @name hideCancel          
         * @desc Dismiss contact popup
         * @returns {*}         
         */
        function hideCancel() {
            $scope.modalInstance.dismiss();
        }
    }
    
}());