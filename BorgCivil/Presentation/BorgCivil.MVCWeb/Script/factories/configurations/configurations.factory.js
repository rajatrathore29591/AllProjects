(function () {
    'use strict';

    /**
     * Associate booking factory, booking  info factory with BCA module
     */
    angular
		.module('borgcivil')
		.factory('UserFactory', UserFactory)
        .factory('DriverFactory', DriverFactory)
        .factory('ManageFleetTypeFactory', ManageFleetTypeFactory)
        .factory('ManageWorkTypeFactory', ManageWorkTypeFactory)
        .factory('ManageCheckListFactory', ManageCheckListFactory)
        .factory('ManageFleetRegistrationFactory', ManageFleetRegistrationFactory)
        .factory('ManageCustomerFactory', ManageCustomerFactory)
        .factory('ManageSiteFactory', ManageSiteFactory)        
        .factory('ManageSupervisorFactory', ManageSupervisorFactory)
        .factory('ManageGateFactory', ManageGateFactory)

    //Inject required modules to factory method
    UserFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name UserFactory
      * @desc User data
      * @returns {{users: Array, countries: Array, states: Array, roles: Array, userInfo: object}}
      * @constructor
      */
    function UserFactory($http, $q, CONST, NotificationFactory) {

        var userObj = {

            users: [],
            userInfo: [],
            countries: [],
            states: [],
            roles: [],

            /**
              * @name addEmployee
              * @desc Add New User 
              * @returns {*}
              */
            addEmployee: function (userData) {
                return $http.post(CONST.CONFIG.API_URL + 'Configuration/AddEmployee', userData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
             * @name updateEmployee
             * @desc Update User 
             * @returns {*}
             */
            updateEmployee: function (userData) {
                console.log(userData);
                return $http.post(CONST.CONFIG.API_URL + 'Configuration/UpdateEmployee', userData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
            * @name deleteEmployee
            * @desc Delete User 
            * @returns {*}
            */
            deleteEmployee: function (userId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/DeleteEmployee/' + userId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
             * @name getUserListing
             * @desc Retrieve users listing from db
             * @returns {*}
             */
            getAllEmployee: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllEmployee')
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            userObj.users = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });

                return userObj.users;
            },

            /**
            * @name getEmployeeDetail
            * @desc Retrieve user Details from db
            * @returns {*}
            */
            getEmployeeDetail: function (EmployeeId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetEmployeeDetail/' + EmployeeId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        userObj.userInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return userObj.userInfo;
            },

            /**
              * @name getAllCountry
              * @desc Retrieve All Countries from db
              * @returns {*}
              */
            getAllCountry: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllCountry')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        userObj.countries = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return userObj.countries;
            },

            /**
             * @name getAllState
             * @desc Retrieve All states from db
             * @returns {*}
             */
            getAllState: function (CountryId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllState/' + CountryId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        userObj.states = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return userObj.states;
            },

            /**
             * @name getAllRoles
             * @desc Retrieve users listing from db
             * @returns {*}
             */
            getAllRoles: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllRoles')
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            userObj.roles = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });

                return userObj.roles;
            },

            /**
            * @name changePassword
            * @desc change password of the user 
            * @returns {*}
            */
            changePassword: function (passwordData) {
                return $http.post(CONST.CONFIG.API_URL + 'Configuration/ChangePassword', passwordData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },
        }

        return userObj;
    }

    //Inject required modules to factory method
    DriverFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory', 'Upload'];

    /**
      * @name DriverFactory
      * @desc Driver data
      * @returns {{drivers: Array, driverInfo: object}}
      * @constructor
      */
    function DriverFactory($http, $q, CONST, NotificationFactory, $Upload) {

        var driverObj = {

            drivers: [],
            driverInfo: [],
           
            /**
            * @name getAllDriver
            * @desc Retrieve drivers listing from db
            * @returns {*}
            */
            getAllDriver: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetAllDriver')
                    .then(function (response) {
                        if (response.status == 200 && typeof response.data === 'object') {
                            driverObj.drivers = response.data;
                        } else {
                            NotificationFactory.error(response.data);
                        }

                    }, function (response) {
                        return $q.reject(response.data);
                        NotificationFactory.error(false);
                    });

                return driverObj.drivers;
            },

            /**
            * @name getDriverDetail
            * @desc Retrieve driver Details from db
            * @returns {*}
            */
            getDriverDetail: function (driverId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetDriverDetail/' + driverId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        driverObj.driverInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return driverObj.driverInfo;
            },

            /**
              * @name addDriver
              * @desc Add New Driver 
              * @returns {*}
              */
            addDriver: function (driverData) {
                console.log(JSON.stringify(driverData));
                return $Upload.upload({
                    url: CONST.CONFIG.API_URL + 'Fleet/AddDriver',
                    headers: { 'Accept': 'application/json', 'Content-Type': 'application/json; ; charset=UTF-8' },
                    data: { model: driverData},
                }).then(function (response) {
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }
                }, function (response) {
                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
            },

            /**
             * @name updateDriver
             * @desc Update Driver 
             * @returns {*}
             */
            updateDriver: function (driverData) {
                console.log('driverData', driverData)
                return $Upload.upload({
                    url: CONST.CONFIG.API_URL + 'Fleet/UpdateDriver',
                    data: { model: driverData},
                }).then(function (response) {
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }
                }, function (response) {
                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
            },

            /**
            * @name deleteDriver
            * @desc Delete driver 
            * @returns {*}
            */
            deleteDriver: function (driverId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/DeleteDriver/' + driverId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },          

        }

        return driverObj;
    }

    //Inject required modules to factory method
    ManageFleetTypeFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name ManageFleetTypeFactory
      * @desc Fleet Type data
      * @returns {{fleetTypeInfo: object, fleetTypes: Array}}
      * @constructor
      */
    function ManageFleetTypeFactory($http, $q, CONST, NotificationFactory) {

        var fleetObj = {

            fleetTypes: [],
            fleetTypeInfo: {},

            /**
             * @name getAllFleetType
             * @desc Retrieve All Fleet types from db
             * @returns {*}
             */
            getAllFleetType: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetAllFleetType')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        fleetObj.fleetTypes = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return fleetObj.fleetTypes;
            },

            /**
            * @name getFleetTypeDetail
            * @desc Retrieve Fleet Type info from db
            * @returns {*}
            */
            getFleetTypeDetail: function (FleetTypeId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetFleetTypeDetail/' + FleetTypeId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        fleetObj.fleetTypeInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return fleetObj.fleetTypeInfo;
            },

            /**
            * @name addFleetType
            * @desc Add New Fleet Type into db
            * @returns {*}
            */
            addFleetType: function (fleetTypeData) {
                return $http.post(CONST.CONFIG.API_URL + 'Fleet/AddFleetType', fleetTypeData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
            * @name updateFleetType
            * @desc Update Fleet Type into db
            * @returns {*}
            */
            updateFleetType: function (fleetTypeData) {
                return $http.post(CONST.CONFIG.API_URL + 'Fleet/UpdateFleetType', fleetTypeData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
           * @name deleteFleetType
           * @desc Delete Fleet Type from db
           * @returns {*}
           */
            deleteFleetType: function (fleetTypeId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/DeleteFleetType/' + fleetTypeId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return fleetObj;
    }

    //Inject required modules to factory method
    ManageWorkTypeFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name ManageWorkTypeFactory
      * @desc WorkType data
      * @returns {{workTypeInfo: object, workTypes: Array}}
      * @constructor
      */
    function ManageWorkTypeFactory($http, $q, CONST, NotificationFactory) {

        var workTypeObj = {

            workTypes: [],
            workTypeInfo: {},

            /**
             * @name getAllWorkType
             * @desc Retrieve All Work Group from db
             * @returns {*}
             */
            getAllWorkType: function () {
                return $http.get(CONST.CONFIG.API_URL + 'WorkType/GetAllWorkType')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        workTypeObj.workTypes = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return workTypeObj.workTypes;
            },

            /**
            * @name getWorkTypeDetail
            * @desc Retrieve Work Type Detail from db
            * @returns {*}
            */
            getWorkTypeDetail: function (WorkTypeId) {
                return $http.get(CONST.CONFIG.API_URL + 'WorkType/GetWorkTypeDetail/' + WorkTypeId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        workTypeObj.workTypeInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return workTypeObj.workTypeInfo;
            },

            /**
            * @name addWorkType
            * @desc Add New WorkType into db
            * @returns {*}
            */
            addWorkType: function (workTypeData) {
                return $http.post(CONST.CONFIG.API_URL + 'WorkType/addWorkType', workTypeData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
            * @name updateWorkType
            * @desc Update WorkType Data into db
            * @returns {*}
            */
            updateWorkType: function (workTypeData) {
                return $http.post(CONST.CONFIG.API_URL + 'WorkType/UpdateWorkType', workTypeData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
           * @name deleteWorkType
           * @desc Delete WorkType from db
           * @returns {*}
           */
            deleteWorkType: function (fleetTypeId) {
                return $http.get(CONST.CONFIG.API_URL + 'WorkType/DeleteWorkType/' + fleetTypeId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return workTypeObj;
    }

    //Inject required modules to factory method
    ManageCheckListFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name ManageCheckListFactory
      * @desc CheckList data
      * @returns {{checkListInfo: object, checkLists: Array}}
      * @constructor
      */
    function ManageCheckListFactory($http, $q, CONST, NotificationFactory) {

        var checkListObj = {

            checkLists: [],
            checkListInfo: {},

            /**
             * @name getAllCheckList
             * @desc Retrieve All CheckList from db
             * @returns {*}
             */
            getAllCheckList: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetAllDocketCheckList')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        checkListObj.checkLists = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return checkListObj.checkLists;
            },

            /**
            * @name getCheckListDetail
            * @desc Retrieve Checklist Detail from db
            * @returns {*}
            */
            getCheckListDetail: function (DocketCheckListId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/GetDocketCheckListDetail/' + DocketCheckListId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        checkListObj.checkListInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return checkListObj.checkListInfo;
            },

            /**
            * @name addCheckList
            * @desc Add New WorkType into db
            * @returns {*}
            */
            addCheckList: function (checkListData) {
                return $http.post(CONST.CONFIG.API_URL + 'Configuration/AddDocketCheckboxList', checkListData, {})
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
            * @name updateCheckList
            * @desc Update CheckList Data into db
            * @returns {*}
            */
            updateCheckList: function (checkListData) {
                return $http.post(CONST.CONFIG.API_URL + 'Configuration/UpdateDocketCheckboxList', checkListData, {})
                   .then(function (response) {
                       
                       if (response.status == 200) {
                           console.log(CONST.MSG.SUCCESS_RECORD_ADDED)
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
           * @name deleteCheckList
           * @desc Delete CheckList from db
           * @returns {*}
           */
            deleteCheckList: function (checkListId) {
                return $http.get(CONST.CONFIG.API_URL + 'Configuration/DeleteDocketCheckboxList/' + checkListId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);;
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return checkListObj;
    }

    //Inject required modules to factory method
    ManageFleetRegistrationFactory.$inject = ['$http', '$q', 'CONST', 'Upload', 'NotificationFactory'];

    /**
      * @name ManageFleetRegistrationFactory
      * @desc Fleet Registration data
      * @returns {{fleetRegInfo: object, fleetRegLists: Array}}
      * @constructor
      */
    function ManageFleetRegistrationFactory($http, $q, CONST, $Upload, NotificationFactory) {

        var fleetRegObj = {

            fleetRegLists: [],
            fleetRegInfo: {},

            /**
             * @name getAllFleetRegistration
             * @desc Retrieve All fleetRegistration from db
             * @returns {*}
             */
            getAllFleetRegistration: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetAllFleetRegistration')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        fleetRegObj.fleetRegLists = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return fleetRegObj.checkLists;
            },

            /**
            * @name getFleetRegistrationDetail
            * @desc Retrieve Fleet Registration Detail from db
            * @returns {*}
            */
            getFleetRegistrationDetail: function (FleetRegistrationId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/GetFleetRegistrationDetail/' + FleetRegistrationId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        fleetRegObj.fleetRegInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return fleetRegObj.fleetRegInfo;
            },

            /**
           * @name addFleetRegistration
           * @desc Add New Fleet Registration into db
           * @returns {*}
           */
            addFleetRegistration: function (fleetRegData) {
                return $Upload.upload({
                    url: CONST.CONFIG.API_URL + 'Fleet/AddFleetRegistration',
                    data: { model: fleetRegData},
                }).then(function (response) {
                    console.log('ddd', response);
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }
                }, function (response) {
                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
            },

            /**
            * @name updateFleetRegistration
            * @desc Update Fleet Registration Data into db
            * @returns {*}
            */
            updateFleetRegistration: function (fleetRegData) {
                return $Upload.upload({
                    url: CONST.CONFIG.API_URL + 'Fleet/UpdateFleetRegistration',
                    data: { model: fleetRegData},
                }).then(function (response) {
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }
                }, function (response) {
                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
            },

            /**
           * @name deleteFleetRegistration
           * @desc Delete Fleet Registration from db
           * @returns {*}
           */
            deleteFleetRegistration: function (FleetRegistrationId) {
                return $http.get(CONST.CONFIG.API_URL + 'Fleet/DeleteFleetRegistration/' + FleetRegistrationId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return fleetRegObj;
    }

    //Inject required modules to factory method
    ManageCustomerFactory.$inject = ['$http', '$q', 'CONST', 'Upload', 'NotificationFactory'];

    /**
      * @name ManageCustomerFactory
      * @desc Customer data
      * @returns {{customerInfo: object, customerList: Array}}
      * @constructor
      */
    function ManageCustomerFactory($http, $q, CONST, $Upload, NotificationFactory) {

        var customerObj = {

            customerList: [],
            customerInfo: {},

            /**
             * @name getAllCustomer
             * @desc Retrieve All customers from db
             * @returns {*}
             */
            getAllCustomer: function () {
                return $http.get(CONST.CONFIG.API_URL + 'Customer/GetAllCustomer')
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        customerObj.customerList = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return customerObj.customerLists;
            },

            /**
            * @name getCustomerByCustomerId
            * @desc Retrieve Customer Detail from db
            * @returns {*}
            */
            getCustomerByCustomerId: function (customerId) {
                return $http.get(CONST.CONFIG.API_URL + 'Customer/GetCustomerByCustomerId/' + customerId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        customerObj.customerInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return customerObj.customerInfo;
            },

            /**
           * @name addCustomer
           * @desc Add New Customer into db
           * @returns {*}
           */
            addCustomer: function (customerData) {
                return $http.post(CONST.CONFIG.API_URL + 'Customer/AddCustomer', customerData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }
                      console.log(response);
                      return response;

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });                
            },

            /**
            * @name updateCustomer
            * @desc Update Customer Data into db
            * @returns {*}
            */
            updateCustomer: function (customerData) {
                return $http.post(CONST.CONFIG.API_URL + 'Customer/UpdateCustomer', customerData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });                
            },

            /**
           * @name deleteCustomer
           * @desc Delete Customer from db
           * @returns {*}
           */
            deleteCustomer: function (customerId) {
                return $http.get(CONST.CONFIG.API_URL + 'Customer/DeleteCustomer/' + customerId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
             * @name addCustomerSite
             * @desc Add New Site of the customer into db
             * @returns {*}
             */
            addCustomerSite: function (customerSiteData) {
                console.log(customerSiteData);
                return $Upload.upload({

                    url: CONST.CONFIG.API_URL + 'Site/AddCustomerSite',
                    data: { model: customerSiteData },

                }).then(function (response) {

                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }

                }, function (response) {

                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
            },
        }
        return customerObj;
    }

    //Inject required modules to factory method
    ManageSiteFactory.$inject = ['$http', '$q', 'CONST', 'Upload', 'NotificationFactory'];

    /**
      * @name ManageSiteFactory
      * @desc Site data
      * @returns {{siteInfo: object, siteList: Array}}
      * @constructor
      */
    function ManageSiteFactory($http, $q, CONST, $Upload, NotificationFactory) {

        var siteObj = {

            siteList: [],
            siteInfo: {},

            /**
             * @name getAllSite
             * @desc Retrieve All Sites from db
             * @returns {*}
             */
            getAllSite: function (customerId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/GetAllSite/' + customerId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        siteObj.siteList = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return siteObj.siteList;
            },

            /**
            * @name getSiteDetail
            * @desc Retrieve Site Detail from db
            * @returns {*}
            */
            getSiteDetail: function (siteId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/GetSiteDetail/' + siteId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        siteObj.siteInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return siteObj.siteInfo;
            },

            /**
           * @name addSite
           * @desc Add New Site into db
           * @returns {*}
           */
            addSite: function (siteData) {
                return $http.post(CONST.CONFIG.API_URL + 'Site/AddSite', siteData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });
            },

            /**
            * @name updateSite
            * @desc Update Site Data into db
            * @returns {*}
            */
            updateSite: function (siteData) {
                return $Upload.upload({
                    url: CONST.CONFIG.API_URL + 'Site/UpdateSite',
                    data: { model: siteData, files: siteData.files },
                }).then(function (response) {
                    if (response.status == 200) {
                        NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                    } else {
                        NotificationFactory.error(response.data.Message);
                    }
                }, function (response) {
                    NotificationFactory.error(CONST.MSG.ERROR_RECORD_ADDED);
                    return $q.reject(response.data);
                });
                //return $http.post(CONST.CONFIG.API_URL + 'Site/UpdateSite', siteData, {})
                //  .then(function (response) {
                //      if (response.status == 200) {
                //          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                //      } else {
                //          NotificationFactory.error(response.data.Message);
                //      }

                //  }, function (response) {
                //      //return $q.reject(response.data);
                //      NotificationFactory.error("Error: " + response.statusText);
                //  });
            },

            /**
           * @name deleteSite
           * @desc Delete Site from db
           * @returns {*}
           */
            deleteSite: function (siteId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/DeleteSite/' + siteId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return siteObj;
    }


    //Inject required modules to factory method
    ManageSupervisorFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name ManageSupervisorFactory
      * @desc Site data
      * @returns {{siteInfo: object, siteList: Array}}
      * @constructor
      */
    function ManageSupervisorFactory($http, $q, CONST, NotificationFactory) {

        var supervisorObj = {

            supervisorList: [],
            supervisorInfo: {},

            /**
             * @name getAllSupervisor
             * @desc Retrieve All Supervisor from db
             * @returns {*}
             */
            getAllSupervisor: function (SiteId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/GetAllSupervisor/' + SiteId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        supervisorObj.supervisorList = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return supervisorObj.supervisorList;
            },

            /**
            * @name getSupervisorBySupervisorId
            * @desc Retrieve Supervisor Detail from db
            * @returns {*}
            */
            getSupervisorBySupervisorId: function (SupervisorId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/GetSupervisorBySupervisorId/' + SupervisorId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        supervisorObj.supervisorInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return siteObj.siteInfo;
            },

            /**
           * @name addSupervisor
           * @desc Add New Supervisor into db
           * @returns {*}
           */
            addSupervisor: function (supervisorData) {
                return $http.post(CONST.CONFIG.API_URL + 'Site/AddSupervisor', supervisorData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });
            },

            /**
            * @name updateSupervisor
            * @desc Update Supervisor Data into db
            * @returns {*}
            */
            updateSupervisor: function (supervisorData) {
                return $http.post(CONST.CONFIG.API_URL + 'Site/UpdateSupervisor', supervisorData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });
            },

            /**
           * @name deleteSupervisor
           * @desc Delete Supervisor from db
           * @returns {*}
           */
            deleteSupervisor: function (supervisorId) {
                return $http.get(CONST.CONFIG.API_URL + 'Site/DeleteSupervisor/' + supervisorId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            }
        }
        return supervisorObj;
    }

    //Inject required modules to factory method
    ManageGateFactory.$inject = ['$http', '$q', 'CONST', 'NotificationFactory'];

    /**
      * @name ManageGateFactory
      * @desc Gate data
      * @returns {{gateInfo: object, gateList: Array}}
      * @constructor
      */
    function ManageGateFactory($http, $q, CONST, NotificationFactory) {

        var gateObj = {

            gateList: [],
            gateInfo: {},
            gateContactList: {},

            /**
             * @name getAllGate
             * @desc Retrieve All Supervisor from db
             * @returns {*}
             */
            getAllGate: function (siteId) {
                return $http.get(CONST.CONFIG.API_URL + 'booking/GetAllGate/' + siteId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        gateObj.gateList = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }
                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });
                return gateObj.gateList;
            },

            /**
            * @name getGateDetail
            * @desc Retrieve Gate Detail from db
            * @returns {*}
            */
            getGateDetail: function (gateId) {
                return $http.get(CONST.CONFIG.API_URL + 'booking/GetGateDetail/' + gateId)
                .then(function (response) {
                    if (response.status == 200 && typeof response.data === 'object') {
                        gateObj.gateInfo = response.data;
                    } else {
                        NotificationFactory.error(response.data);
                    }

                }, function (response) {
                    return $q.reject(response.data);
                    NotificationFactory.error(false);
                });

                return gateObj.gateInfo;
            },

            /**
           * @name addGate
           * @desc Add New Gate into db
           * @returns {*}
           */
            addGate: function (gateData) {
                return $http.post(CONST.CONFIG.API_URL + 'booking/AddGate', gateData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });
            },

            /**
            * @name updateGate
            * @desc Update Gate Data into db
            * @returns {*}
            */
            updateGate: function (gateData) {
                return $http.post(CONST.CONFIG.API_URL + 'booking/UpdateGate', gateData, {})
                  .then(function (response) {
                      if (response.status == 200) {
                          NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_UPDATED);
                      } else {
                          NotificationFactory.error(response.data.Message);
                      }

                  }, function (response) {
                      //return $q.reject(response.data);
                      NotificationFactory.error("Error: " + response.statusText);
                  });
            },

            /**
           * @name deleteGate
           * @desc Delete Gate from db
           * @returns {*}
           */
            deleteGate: function (gateId) {
                return $http.get(CONST.CONFIG.API_URL + 'booking/DeleteGate/' + gateId)
                   .then(function (response) {
                       if (response.status == 200) {
                           NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_DELETED);
                       } else {
                           NotificationFactory.error(response.data.Message);
                       }

                   }, function (response) {
                       //return $q.reject(response.data);
                       NotificationFactory.error("Error: " + response.statusText);
                   });
            },

            /**
             * @name getGateContactDetail
             * @desc Get gateContact Detail from db
             * @returns {*}
             */
            getGateContactDetail: function (gateContactId) {
                return $http.get(CONST.CONFIG.API_URL + 'booking/GetGateContactDetail/' + gateContactId)
               .then(function (response) {
                   if (response.status == 200 && typeof response.data === 'object') {
                       gateObj.gateContactList = response.data;
                   } else {
                       NotificationFactory.error(response.data);
                   }

               }, function (response) {
                   return $q.reject(response.data);
                   NotificationFactory.error(false);
               });

                return gateObj.gateContactList;
            },

            /**
             * @name addGateContactPerson
             * @desc add new gateContact Detail from db
             * @returns {*}
             */
            addGateContactPerson: function (gateContactData) {
                return $http.post(CONST.CONFIG.API_URL + 'booking/AddGateContactPerson', gateContactData, {})
                 .then(function (response) {
                     if (response.status == 200) {
                         NotificationFactory.success(CONST.MSG.SUCCESS_RECORD_ADDED);
                     } else {
                         NotificationFactory.error(response.data.Message);
                     }

                 }, function (response) {
                     //return $q.reject(response.data);
                     NotificationFactory.error("Error: " + response.statusText);
                 });
            }
        }
        return gateObj;
    }
})();